using PulseStore.BLL.Entities.TemplateFiles.Enums;
using PulseStore.BLL.Repositories;
using PulseStore.BLL.Services.BlobStorage;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace PulseStore.BLL.Services.TemplateToHtml;

public class TemplateToHtmlService : ITemplateToHtmlService
{
    private readonly ITemplateFileRepository _templateFileRepository;
    private readonly IBlobStorageService _blobStorageService;

    public TemplateToHtmlService(ITemplateFileRepository templateFileRepository, IBlobStorageService blobStorageService, IConfiguration config)
    {
        _templateFileRepository = templateFileRepository;
        _blobStorageService = blobStorageService;
        _blobStorageService.ContainerName = config["BlobStorageContainers:TemplatesContainerName"];
        _blobStorageService.CheckContainer();
    }

    public async Task<Stream?> GetHtmlFileFromTemplateAsync<T, K>(T dataObject, TemplateFileType templateFileType, IEnumerable<K>? repeatingObjects = null)
    {
        var htmlTeamplateStream = await GetTemplateAsync(templateFileType);

        if (htmlTeamplateStream is null)
        {
            throw new ArgumentNullException(nameof(htmlTeamplateStream), $"Html template with type {templateFileType} is null.");
        }

        var mappedHtmlStream = MapObjectOnHtml(dataObject, htmlTeamplateStream, repeatingObjects);

        return mappedHtmlStream;
    }

    private string ConvertStreamToString(Stream stream)
    {
        using (var reader = new StreamReader(stream, Encoding.UTF8))
        {
            return reader.ReadToEnd();
        }
    }

    private Stream ConvertStringToStream(string inputString)
    {
        var byteArray = Encoding.UTF8.GetBytes(inputString);
        return new MemoryStream(byteArray);
    }

    /// <summary>
    ///     Gets HTML template stream by template type.
    /// </summary>
    /// <param name="templateFileType">Type of template to get.</param>
    /// <returns>
    ///     <see cref="Stream"/> with HTML template.
    /// </returns>
    private async Task<Stream?> GetTemplateAsync(TemplateFileType templateFileType)
    {
        var templateHtmlFile = await _templateFileRepository.GetAsync(templateFileType);
        if (templateHtmlFile is null)
        {
            return null;
        }

        var uri = new Uri(templateHtmlFile.FilePath);
        var htmlFilename = uri.Segments.Last();
        var htmlBlob = await _blobStorageService.DownloadAsync(htmlFilename);

        if ((htmlBlob?.Content is null))
        {
            return null;
        }

        return htmlBlob.Content;
    }

    /// <summary>
    ///     Maps object on HTML template stream.
    /// </summary>
    /// <param name="dataObject">Object that will be mapped on template.</param>
    /// <param name="htmlStream">Stream with HTML template/</param>
    /// <param name="repeatingObjects">List of repeating objects. Is used for generating repeating html blocks.</param>
    /// <returns>
    ///     <see cref="Stream"/> with HTML.
    /// </returns>
    private Stream MapObjectOnHtml<T, K>(T dataObject, Stream htmlStream, IEnumerable<K>? repeatingObjects)
    {
        var htmlTemplate = ConvertStreamToString(htmlStream);
        var resultHtmlString = MapObjectPropertiesOnHtml(dataObject, htmlTemplate);

        // Map repeating objects
        if (repeatingObjects is not null)
        {
            resultHtmlString = MapRepeatingObjectsPropertiesOnHtml(repeatingObjects, resultHtmlString);
        }

        return ConvertStringToStream(resultHtmlString);
    }

    /// <summary>
    ///     Maps object properties on HTML template string using Regex.
    /// </summary>
    /// <param name="dataObject">Object which properties will be mapped on template.</param>
    /// <param name="htmlTemplate"> <see cref="string"/> with HTML template.</param>
    /// <returns>
    ///     <see cref="string"/> with mapped HTML.
    /// </returns>
    private string MapObjectPropertiesOnHtml<T>(T dataObject, string htmlTemplate)
    {
        var pattern = @"\{\{([^\}]+)\}\}";

        return Regex.Replace(htmlTemplate, pattern, match =>
        {
            string propertyName = match.Groups[1].Value;

            var property = typeof(T)
                .GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (property is not null)
            {
                var value = property.GetValue(dataObject);
                return value?.ToString() ?? string.Empty;
            }

            return match.Value;
        });
    }

    /// <summary>
    ///     Maps objects properties from list on HTML template string using Regex to generate repeating HTML block.
    /// </summary>
    /// <param name="items">List og object which properties to map.</param>
    /// <param name="htmlString"> <see cref="string"/> with HTML template.</param>
    /// <returns>
    ///     <see cref="string"/> with mapped HTML with repeating blocks.
    /// </returns>
    private string MapRepeatingObjectsPropertiesOnHtml<T>(IEnumerable<T> items, string htmlString)
    {
        var pattern = @"{{#(\w+)}}(.*?){{/\1}}";
        var match = Regex.Match(htmlString, pattern, RegexOptions.Singleline);

        if (match.Success)
        {
            string htmlItemTemplate = match.Groups[2].Value;

            var resultBuilder = new StringBuilder();

            foreach (var item in items)
            {
                var itemHtml = MapObjectPropertiesOnHtml(item, htmlItemTemplate);
                resultBuilder.AppendLine(itemHtml);
            }

            var itemsHtml = resultBuilder.ToString();

            // Replace the repeating block in the original string
            return htmlString.Replace(match.Value, itemsHtml);
        }

        return htmlString;
    }
}
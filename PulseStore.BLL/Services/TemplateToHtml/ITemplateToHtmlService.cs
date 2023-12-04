using PulseStore.BLL.Entities.TemplateFiles.Enums;

namespace PulseStore.BLL.Services.TemplateToHtml;

public interface ITemplateToHtmlService
{
    /// <summary>
    ///     Gets HTML file with object mapped to template with specified <see cref="TemplateFileType"/>.
    /// </summary>
    /// <param name="dataObject">Object that will be mapped on template.</param>
    /// <param name="templateFileType">Type of template to use to generate HTML.</param>
    /// <param name="repeatingObjects">Optional property with list of repeating objects. Is used for generating repeating html blocks.</param>
    /// <returns>
    ///     <see cref="Stream"/> with HTML.
    /// </returns>
    Task<Stream?> GetHtmlFileFromTemplateAsync<T, K>(T dataObject, TemplateFileType templateFileType, IEnumerable<K>? repeatingObjects = null);
}
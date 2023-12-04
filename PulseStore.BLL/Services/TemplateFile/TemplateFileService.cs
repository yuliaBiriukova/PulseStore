using PulseStore.BLL.Entities.TemplateFiles.Enums;
using PulseStore.BLL.Repositories;
using PulseStore.BLL.Services.BlobStorage;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace PulseStore.BLL.Services.TemplateFile;

public class TemplateFileService : ITemplateFileService
{
    private readonly ITemplateFileRepository _templateFileRepository;
    private readonly IBlobStorageService _blobStorageService;

    public TemplateFileService(ITemplateFileRepository templateFileRepository, IBlobStorageService blobStorageService, IConfiguration config)
    {
        _templateFileRepository = templateFileRepository;
        _blobStorageService = blobStorageService;
        _blobStorageService.ContainerName = config["BlobStorageContainers:TemplatesContainerName"];
        _blobStorageService.CheckContainer();
    }

    public async Task<int> UpsertAsync(IFormFile file, TemplateFileType templateFileType)
    {
        var blobResponse = await _blobStorageService.UploadAsync(file, true);
        var newFilePath = blobResponse.Blob.Uri;

        if (newFilePath is not null)
        {
            var existingTemplateFile = await _templateFileRepository.GetAsync(templateFileType);

            if (existingTemplateFile is not null)
            {
                var oldFilePath = existingTemplateFile.FilePath;
                existingTemplateFile.FilePath = newFilePath;
                var isUpdated = await _templateFileRepository.UpdateAsync(existingTemplateFile);
                if(isUpdated)
                {
                    var templateFileName = GetFileNameFromFilePath(oldFilePath);
                    var blobDeleteResult = await _blobStorageService.DeleteAsync(templateFileName);
                    return existingTemplateFile.Id;
                }
            } 
            else
            {
                var id = await _templateFileRepository.AddAsync(newFilePath, templateFileType);
                return id;
            }
        }

        return 0;
    }

    private string GetFileNameFromFilePath(string filePath)
    {
        var uri = new Uri(filePath);
        return uri.Segments.Last();
    }
}
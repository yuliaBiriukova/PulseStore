using PulseStore.BLL.Entities.TemplateFiles.Enums;
using Microsoft.AspNetCore.Http;

namespace PulseStore.BLL.Services.TemplateFile;

public interface ITemplateFileService
{
    /// <summary>
    ///     Updates template file if template with specified type exists; otherwise adds.
    /// </summary>
    /// <param name="file">File to upsert.</param>
    /// <param name="templateFileType">Type of template to upsert.</param>
    /// <returns>
    ///     Id of updated or added template file.
    /// </returns>
    Task<int> UpsertAsync(IFormFile file, TemplateFileType templateFileType);
}
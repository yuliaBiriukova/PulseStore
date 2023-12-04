using PulseStore.BLL.Entities.TemplateFiles.Enums;
using PulseStore.BLL.Entities.TemplateFiles;

namespace PulseStore.BLL.Repositories;

public interface ITemplateFileRepository
{
    Task<int> AddAsync(string filePath, TemplateFileType type);

    Task<bool> CheckExistsAsync(int id);

    Task<TemplateFile?> GetAsync(TemplateFileType type);

    Task<bool> UpdateAsync(TemplateFile templateFile);
}
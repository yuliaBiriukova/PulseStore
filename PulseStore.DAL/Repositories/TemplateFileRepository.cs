using PulseStore.BLL.Repositories;
using PulseStore.DAL.Database;
using Microsoft.EntityFrameworkCore;
using PulseStore.BLL.Entities.TemplateFiles;
using PulseStore.BLL.Entities.TemplateFiles.Enums;

namespace PulseStore.DAL.Repositories;

public class TemplateFileRepository : ITemplateFileRepository
{
    private readonly PulseStoreContext _dbContext;

    public TemplateFileRepository(PulseStoreContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> AddAsync(string filePath, TemplateFileType type)
    {
        var blobInfo = new TemplateFile()
        {
            FilePath = filePath,
            Type = type,
        };

        _dbContext.TemplateFiles.Add(blobInfo);
        await _dbContext.SaveChangesAsync();
        return blobInfo.Id;
    }

    public async Task<bool> CheckExistsAsync(int id)
    {
        return await _dbContext.TemplateFiles.AnyAsync(t => t.Id == id);
    }

    public async Task<TemplateFile?> GetAsync(TemplateFileType type)
    {
        return await _dbContext.TemplateFiles.SingleOrDefaultAsync(t => t.Type == type);
    }

    public async Task<bool> UpdateAsync(TemplateFile templateFile)
    {
        var templateFileExists = await CheckExistsAsync(templateFile.Id);
        if(templateFileExists) 
        {
            _dbContext.Attach(templateFile);
            _dbContext.Entry(templateFile)
                .Property(t => t.FilePath)
                .IsModified = true;
            var rowsAffected = await _dbContext.SaveChangesAsync();
            return rowsAffected > 0;
        }
        return false;
    }
}
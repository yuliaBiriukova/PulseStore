using PulseStore.BLL.Entities.OrderDocuments;
using PulseStore.BLL.Entities.OrderDocuments.Enums;
using PulseStore.BLL.Repositories;
using PulseStore.DAL.Database;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace PulseStore.DAL.Repositories;

public class OrderDocumentRepository : IOrderDocumentRepository
{
    private readonly PulseStoreContext _dbContext;

    public OrderDocumentRepository(PulseStoreContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> AddAsync(OrderDocument orderDocument)
    {
        _dbContext.OrderDocuments.Add(orderDocument);
        var rowsAffected = await _dbContext.SaveChangesAsync();
        return rowsAffected > 0;
    }

    public async Task<bool> CheckExistsAsync(int orderId, OrderDocumentType type)
    {
        return await _dbContext.OrderDocuments.AnyAsync(od => od.OrderId == orderId && od.Type == type);
    }

    public async Task<OrderDocument?> GetAsync(int orderId, OrderDocumentType type)
    {
        return await _dbContext.OrderDocuments
            .Where(od => od.OrderId == orderId && od.Type == type)
            .FirstOrDefaultAsync();
    }

    public async Task<string?> GetFilePathAsync(int orderId, OrderDocumentType type)
    {
        return await _dbContext.OrderDocuments
            .AsNoTracking()
            .FirstOrDefaultAsync(od => od.OrderId == orderId && od.Type == type)
            .Select(od => od?.FilePath);
    }

    public async Task<bool> UpdateFilePathAsync(int orderId, OrderDocumentType type, string newFilePath)
    {
        var orderDocumentExists = await CheckExistsAsync(orderId, type);
        if (orderDocumentExists)
        {
            var orderDocument = new OrderDocument { OrderId = orderId, Type = type, FilePath = newFilePath };

            _dbContext.Attach(orderDocument);

            _dbContext.Entry(orderDocument)
                .Property(od => od.FilePath)
                .IsModified = true;

            var rowsAffected = await _dbContext.SaveChangesAsync();
            return rowsAffected > 0;
        }
        return false;
    }
}
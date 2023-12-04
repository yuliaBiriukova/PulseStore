using PulseStore.BLL.Entities.Orders;
using PulseStore.BLL.Entities.Orders.Enums;
using PulseStore.BLL.Models.Filters;
using PulseStore.BLL.Models.Order;
using PulseStore.BLL.Models.Utils;
using PulseStore.BLL.Repositories;
using PulseStore.DAL.Database;
using Microsoft.EntityFrameworkCore;

namespace PulseStore.DAL.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly PulseStoreContext _dbContext;

    public OrderRepository(PulseStoreContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> AddAsync(Order order)
    {
        _dbContext.Orders.Add(order);
        await _dbContext.SaveChangesAsync();
        return order.Id;
    }

    public async Task<bool> CheckOrderExistsAsync(int id)
    {
        return await _dbContext.Orders.AnyAsync(o => o.Id == id);
    }

    public async Task<IEnumerable<Order>> GetAllOrdersAsync(PaginationFilter filter, Guid userId)
    {
        var query = _dbContext.Orders
            .Where(o => o.UserId == userId)
            .Include(o => o.OrderProducts)
            .ThenInclude(op => op.Product)
            .ThenInclude(p => p.ProductPhotos)
            .AsQueryable();

        var totalProductsCount = await query.CountAsync();

        var orders = await query.OrderBy(o => o.DateCreated)
                            .Skip((filter.PageNumber - 1) * filter.PageSize)
                            .Take(filter.PageSize)
                            .ToListAsync();

        return orders;
    }

    public async Task<OrdersListModel<Order>> GetAsync(OrderFilter filter)
    {
        var query = _dbContext.Orders
            .Include(o => o.OrderProducts)
            .AsQueryable();

        if (filter.MinDate is not null)
        {
            query = query.Where(o => o.DateCreated >= filter.MinDate);
        }

        if (filter.MaxDate is not null)
        {
            query = query.Where(o => o.DateCreated <= filter.MaxDate.Value.AddDays(1));
        }

        if (filter.OrderStatuses is not null)
        {
            query = query.Where(o => filter.OrderStatuses.Contains(o.OrderStatus));
        }

        if (string.Equals(filter.SortBy, Constants.IdProperty, StringComparison.OrdinalIgnoreCase))
        {
            query = string.Equals(filter.SortOrder, Constants.SortAscending, StringComparison.OrdinalIgnoreCase)
                ? query.OrderBy(o => o.Id)
                : query.OrderByDescending(o => o.Id);
        }
        else if (string.Equals(filter.SortBy, Constants.OrderDateCreatedProperty, StringComparison.OrdinalIgnoreCase))
        {
            query = string.Equals(filter.SortOrder, Constants.SortAscending, StringComparison.OrdinalIgnoreCase)
                ? query.OrderBy(o => o.DateCreated)
                : query.OrderByDescending(o => o.DateCreated);
        }

        var totalOrdersCount = await query.CountAsync();

        var pageNumber = Math.Max(
            1,
            Math.Min(
                filter.PageNumber,
                (int)Math.Ceiling((double)query.Count() / filter.PageSize)
                )
            );

        var orders = await query.Skip((pageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();
        var paginationModel = new PaginationModel<Order>(pageNumber, filter.PageSize, totalOrdersCount, orders);

        return new OrdersListModel<Order>(paginationModel);
    }

    public async Task<Order?> GetByIdWithOrderProductsAsNoTrackingAsync(int id)
    {
        return await _dbContext.Orders
           .Include(o => o.OrderProducts)
           .ThenInclude(op => op.Product)
           .AsNoTracking()
           .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<Order?> GetByIdAsNoTrackingAsync(int id)
    {
        return await _dbContext.Orders
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<Order?> GetByIdWithOrderProductsAndPhotosAsync(int id)
    {
        return await _dbContext.Orders
            .Include(o => o.OrderProducts)
            .ThenInclude(op => op.Product)
            .ThenInclude(p => p.Category)
            .Include(o => o.OrderProducts)
            .ThenInclude(op => op.Product)
            .ThenInclude(p => p.ProductPhotos)
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<bool> UpdateOrderStatusAsync(int id, OrderStatus orderStatus)
    {
        var orderExists = await CheckOrderExistsAsync(id);
        if (orderExists)
        {
            var order = new Order() { Id = id, OrderStatus = orderStatus };
            _dbContext.Attach(order);
            _dbContext.Entry(order)
                .Property(o => o.OrderStatus)
                .IsModified = true;
            var rowsAffected = await _dbContext.SaveChangesAsync();
            return rowsAffected > 0;
        }
        return false;
    }
}
using PulseStore.BLL.Entities.Orders;
using PulseStore.BLL.Models.Filters;
using PulseStore.BLL.Models.Utils;
using PulseStore.BLL.Models.Order;
using PulseStore.BLL.Entities.Orders.Enums;

namespace PulseStore.BLL.Repositories;

public interface IOrderRepository
{
    Task<int> AddAsync(Order order);

    /// <summary>
    ///     Checks if <see cref="Order"/> entity with specified id exists.
    /// </summary>
    /// <param name="id">Id of entity to check.</param>
    /// <returns>
    ///      true if entity with specified id exists; otherwise false.
    /// </returns>
    Task<bool> CheckOrderExistsAsync(int id);

    Task<IEnumerable<Order>> GetAllOrdersAsync(PaginationFilter filter, Guid userId);

    /// <summary>
    ///     Gets <see cref="OrdersListModel{T}"/> with <see cref="Order"/> entities by conditions in <see cref="OrderFilter"/>.
    /// </summary>
    /// <param name="filter" ><see cref="OrderFilter"/> entity with conditions for product</param>
    /// <returns>
    ///     List of <see cref="Order"/> entities, that satisfy the filter, with pagination.
    /// </returns>
    Task<OrdersListModel<Order>> GetAsync(OrderFilter filter);

    /// <summary>
    ///     Gets <see cref="Order"/> entity by specified id as no-tracking.
    /// </summary>
    /// <param name="id" >Id of order to get.</param>
    /// <returns>
    ///     <see cref="Order"/> entity with specified id if such exists; otherwise null.
    /// </returns>
    Task<Order?> GetByIdWithOrderProductsAsNoTrackingAsync(int id);

    /// <summary>
    ///     Gets <see cref="Order"/> entity by specified id including order products and product photos.
    /// </summary>
    /// <param name="id" >Id of order to get.</param>
    /// <returns>
    ///     <see cref="Order"/> entity with specified id if such exists; otherwise null.
    /// </returns>
    Task<Order?> GetByIdWithOrderProductsAndPhotosAsync(int id);

    /// <summary>
    ///     Gets <see cref="Order"/> entity by specified id as no-tracking.
    /// </summary>
    /// <param name="id" >Id of order to get.</param>
    /// <returns>
    ///     <see cref="Order"/> entity with specified id if such exists; otherwise null.
    /// </returns>
    Task<Order?> GetByIdAsNoTrackingAsync(int id);

    /// <summary>
    ///     Updates OrderStatus of <see cref="Order"/> entity with specified id.
    /// </summary>
    /// <param name="id">Id of order to update.</param>
    /// <param name="orderStatus">New order status.</param>
    /// <returns>
    ///     true if status was successfully updated; otherwise false.
    /// </returns>
    Task<bool> UpdateOrderStatusAsync(int id, OrderStatus orderStatus);
}
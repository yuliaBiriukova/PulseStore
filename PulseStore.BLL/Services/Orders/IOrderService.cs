using PulseStore.BLL.Entities.Orders.Enums;
using PulseStore.BLL.Models.Filters;
using PulseStore.BLL.Models.Order;
using PulseStore.BLL.Models.Order.AdminOrder;
using PulseStore.BLL.Models.Utils;
using PulseStore.BLL.Result;
using LanguageExt.Common;
using System.ComponentModel.DataAnnotations;

namespace PulseStore.BLL.Services.Orders;

public interface IOrderService
{
    Task<Result<int>> AddAsync(AddOrderDto order, Guid userId);

    Task<IEnumerable<OrderDto>> GetAllOrdersAsync(PaginationFilter filter, Guid userid);

    /// <summary>
    ///     Gets <see cref="OrdersListModel{T}"/> with <see cref="AdminOrderDto"/> entities by conditions in <see cref="OrderFilter"/>.
    /// </summary>
    /// <param name = "filter" ><see cref="OrderFilter"/> entity with conditions for product</param>
    /// <returns>
    ///     List of <see cref="AdminOrderDto"/> entities, that satisfy the filter, with pagination.
    /// </returns>
    Task<OrdersListModel<AdminOrderDto>> GetAsync(OrderFilter filter);

    /// <summary>
    ///     Gets <see cref="AdminOrderDetailsDto"/> entity by specified id.
    /// </summary>
    /// <param name="id" >Id of order to get.</param>
    /// <returns>
    ///     <see cref="AdminOrderDetailsDto"/> entity with specified id if such exists; otherwise null.
    /// </returns>
    Task<AdminOrderDetailsDto?> GetByIdWithOrderProductsAsync(int id);

    /// <summary>
    ///     Gets <see cref="OrderCustomer"/> by specified order id.
    /// </summary>
    /// <param name="id" >Id of order which customer to get.</param>
    /// <returns>
    ///     <see cref="OrderCustomer"/> object if order with specified id exists; otherwise null.
    /// </returns>
    Task<OrderCustomer?> GetCustomerByOrderIdAsync(int id);

    Task<Result<bool, ValidationException>> CancelOrderAsync(Guid userId, int orderId);
    Task<Result<bool, ValidationException>> ConfirmPaymentOrderAsync(Guid userId, int orderId);

    /// <summary>
    ///     Updates status of order with specified id.
    /// </summary>
    /// <param name="id">Id of order to update.</param>
    /// <param name="orderStatus">New order status.</param>
    /// <returns>
    ///     true if status was successfully updated; otherwise false.
    /// </returns>
    Task<Result<bool>> UpdateOrderStatusAsync(int id, OrderStatus orderStatus);

    Task<PaymentMethod?> GetOrderPaymentMethodAsync(int id);
}
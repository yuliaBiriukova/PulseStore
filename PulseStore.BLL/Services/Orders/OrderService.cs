using AutoMapper;
using PulseStore.BLL.Entities.Orders;
using PulseStore.BLL.Entities.Orders.Enums;
using PulseStore.BLL.Models.Filters;
using PulseStore.BLL.Models.Order;
using PulseStore.BLL.Models.Order.AdminOrder;
using PulseStore.BLL.Repositories;
using PulseStore.BLL.Result;
using LanguageExt.Common;
using System.ComponentModel.DataAnnotations;

namespace PulseStore.BLL.Services.Orders;

public class OrderService : IOrderService
{
    private readonly IMapper _mapper;
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IStockProductRepository _stockProductRepository;

    public OrderService(IMapper mapper, IOrderRepository orderRepository, IProductRepository productRepository, IStockProductRepository stockProductRepository)
    {
        _mapper = mapper;
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _stockProductRepository = stockProductRepository;
    }

    public async Task<Result<int>> AddAsync(AddOrderDto order, Guid userId)
    {
        var error = await ValidateAddOrderAsync(order);
        if (error is not null)
        {
            return new Result<int>(error);
        }

        if (userId != Guid.Empty)
        {
            order.UserId = userId;
        }
        order.DateCreated = DateTime.Now;
        order.OrderStatus = OrderStatus.New;
        var newOrder = _mapper.Map<Order>(order);
        return await _orderRepository.AddAsync(newOrder);
    }

    public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync(PaginationFilter filter, Guid userid)
    {
        var orders = await _orderRepository.GetAllOrdersAsync(filter, userid);
        var result = _mapper.Map<IEnumerable<OrderDto>>(orders);

        return result;
    }

    public async Task<OrdersListModel<AdminOrderDto>> GetAsync(OrderFilter filter)
    {
        var orders = await _orderRepository.GetAsync(filter);
        return _mapper.Map<OrdersListModel<AdminOrderDto>>(orders);
    }

    public async Task<Result<bool, ValidationException>> CancelOrderAsync(Guid userId, int orderId)
    {
        var existingOrder = await _orderRepository.GetByIdAsNoTrackingAsync(orderId);

        if (existingOrder is null)
        {
            return new ValidationException($"There is no such product with id {orderId}");
        }

        if(existingOrder.UserId == null || !existingOrder.UserId.Equals(userId))
        {
            return new ValidationException($"You can't cancel this order!");
        }

        if(existingOrder.OrderStatus == OrderStatus.Paid || existingOrder.OrderStatus == OrderStatus.Done)
        {
            return new ValidationException($"You can't cancel this order!");
        }

        var result = await _orderRepository.UpdateOrderStatusAsync(orderId, OrderStatus.Cancelled);

        return result;
    }

    public async Task<Result<bool, ValidationException>> ConfirmPaymentOrderAsync(Guid userId, int orderId)
    {
        var existingOrder = await _orderRepository.GetByIdAsNoTrackingAsync(orderId);

        if (existingOrder is null)
        {
            return new ValidationException($"There is no such product with id {orderId}");
        }

        if (existingOrder.UserId == null || !existingOrder.UserId.Equals(userId))
        {
            return new ValidationException($"You can't pay for this order!");
        }

        if (existingOrder.OrderStatus != OrderStatus.Pending)
        {
            return new ValidationException($"You can't pay for this order!");
        }

        var result = await _orderRepository.UpdateOrderStatusAsync(orderId, OrderStatus.Paid);

        return result;
    }

    public async Task<AdminOrderDetailsDto?> GetByIdWithOrderProductsAsync(int id)
    {
        var order = await _orderRepository.GetByIdWithOrderProductsAndPhotosAsync(id);

        if(order is null)
        {
            return null;
        }

        var productIds = order.OrderProducts.Select(item => item.ProductId);
        var availableQuantities = await _stockProductRepository.GetProductsQuantitiesInAllStocksAsync(productIds);
        var orderDetails = _mapper.Map<AdminOrderDetailsDto>(order);

        foreach (var orderProduct in orderDetails.OrderProducts)
        {
            if (availableQuantities.TryGetValue(orderProduct.ProductId, out int availableQuantity))
            {
                orderProduct.AvailableQuantity = availableQuantity;
            }
        }

        return orderDetails;
    }

    public async Task<Result<bool>> UpdateOrderStatusAsync(int id, OrderStatus orderStatus)
    {
        var error = await ValidateOrderIdAsync(id);
        if (error is not null)
        {
            return new Result<bool>(error);
        }

        return await _orderRepository.UpdateOrderStatusAsync(id, orderStatus);
    }

    private async Task<ValidationException?> ValidateAddOrderAsync(AddOrderDto order)
    {
        var duplicateProductIds = order.OrderProducts
            .GroupBy(dto => dto.ProductId)
            .Where(group => group.Count() > 1)
            .Select(group => group.Key);

        if(duplicateProductIds.Any())
        {
            string formattedDuplicateProductIds = "[" + string.Join(", ", duplicateProductIds) + "]";
            return new ValidationException($"Products with ids {formattedDuplicateProductIds} have duplicate.");
        }

        foreach (var orderProduct in order.OrderProducts)
        {
            if (!await _productRepository.CheckProductExistsAsync(orderProduct.ProductId))
            {
                return new ValidationException($"Product with id={orderProduct.ProductId} does not exist.");
            }
        }

        return null;
    }

    private async Task<ValidationException?> ValidateOrderIdAsync(int id)
    {
        var orderExists = await _orderRepository.CheckOrderExistsAsync(id);
        if (!orderExists)
        {
            return new ValidationException($"Order with id={id} does not exist.");
        }

        return null;
    }

    public async Task<OrderCustomer?> GetCustomerByOrderIdAsync(int id)
    {
        var order = await _orderRepository.GetByIdAsNoTrackingAsync(id);
        return _mapper.Map<OrderCustomer>(order);
    }

    public async Task<PaymentMethod?> GetOrderPaymentMethodAsync(int id)
    {
        var order = await _orderRepository.GetByIdAsNoTrackingAsync(id);
        return order?.PaymentMethod ?? null;
    }
}
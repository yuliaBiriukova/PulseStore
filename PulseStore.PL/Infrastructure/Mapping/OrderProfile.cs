using AutoMapper;
using PulseStore.BLL.Entities.Orders;
using PulseStore.BLL.Models.Order;
using PulseStore.BLL.Models.Order.AdminOrder;
using PulseStore.BLL.Models.Order.Invoice;
using PulseStore.BLL.Models.Order.PaymentEmail;
using PulseStore.BLL.Models.Order.Reject;
using PulseStore.BLL.Models.Utils;
using PulseStore.PL.ViewModels.Order;

namespace PulseStore.PL.Infrastructure.Mapping;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<AddOrderViewModel, AddOrderDto>().ReverseMap();
        CreateMap<AddOrderDto, Order>().ReverseMap();
        CreateMap<AddOrderProductViewModel, AddOrderProductDto>().ReverseMap();
        CreateMap<AddOrderProductDto, OrderProduct>().ReverseMap();
        CreateMap<Order, OrderDto>().ReverseMap();
        CreateMap<OrderDto, OrderViewModel>();
        CreateMap<OrderProduct, OrderProductDto>();
        CreateMap<OrderProductDto, OrderProductListViewModel>();

        CreateMap<AddOrderViewModel, AddOrderDto>();
        CreateMap<AddOrderDto, Order>();
        CreateMap<AddOrderProductViewModel, AddOrderProductDto>();
        CreateMap<AddOrderProductDto, OrderProduct>();

        CreateMap<Order, AdminOrderDto>()
            .ForMember(dest => dest.ItemsAmount, opt => opt.MapFrom(src => src.OrderProducts.Count))
            .ForMember(dest => dest.Sum, opt => opt.MapFrom(src => src.OrderProducts.Sum(op => op.Quantity * op.PricePerItem)));

        CreateMap<AdminOrderDto, AdminOrderViewModel>();

        CreateMap(typeof(OrdersListModel<>), typeof(OrdersListModel<>));
        CreateMap<OrdersListModel<AdminOrderDto>, OrdersListViewModel>();

        CreateMap<Order, AdminOrderDetailsDto>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FirstName + " " + src.LastName));

        CreateMap<AdminOrderDetailsDto, AdminOrderDetailsViewModel>();

        CreateMap<OrderProduct, AdminOrderProductDto>()
            .ForMember(
                dest => dest.ProductPhotoPath,
                opt => opt.MapFrom(src =>
                    src.Product.ProductPhotos.FirstOrDefault(p => p.SequenceNumber == 1).ImagePath
                )
            );

        CreateMap<AdminOrderProductDto, AdminOrderProductViewModel>();

        CreateMap<Order, OrderInvoice>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FirstName + " " + src.LastName));

        CreateMap<OrderProduct, InvoiceOrderProduct>();

        CreateMap<AdminOrderDetailsDto, RejectOrderEmail>()
            .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.OrderProducts.Sum(op => op.Quantity * op.PricePerItem)));
        CreateMap<AdminOrderProductDto, RejectOrderProductDto>()
            .ForMember(dest => dest.Sum, opt => opt.MapFrom(src => src.Quantity * src.PricePerItem))
            .ForMember(dest => dest.CustomTextColor, opt => opt.MapFrom(src => src.Quantity <= src.AvailableQuantity ? Constants.Black : Constants.Red));

        CreateMap<Order, OrderCustomer>()
           .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));

        CreateMap<AdminOrderDetailsDto, OrderPaymentEmail>()
            .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.OrderProducts.Sum(op => op.Quantity * op.PricePerItem)));
        CreateMap<AdminOrderProductDto, PaymentOrderProductDto>()
            .ForMember(dest => dest.Sum, opt => opt.MapFrom(src => src.Quantity * src.PricePerItem));
    }
}
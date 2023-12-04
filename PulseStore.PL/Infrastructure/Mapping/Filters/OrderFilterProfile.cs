using AutoMapper;
using PulseStore.BLL.Models.Filters;
using PulseStore.PL.ViewModels.Filters;

namespace PulseStore.PL.Infrastructure.Mapping.Filters;

public class OrderFilterProfile : Profile
{
    public OrderFilterProfile()
    {
        CreateMap<OrderFilterViewModel, OrderFilter>();
    }
}
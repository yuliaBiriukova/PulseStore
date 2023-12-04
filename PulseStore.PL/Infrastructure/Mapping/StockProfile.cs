using AutoMapper;
using PulseStore.BLL.Entities;
using PulseStore.BLL.Models.Stock;
using PulseStore.PL.ViewModels.Stock;

namespace PulseStore.PL.Infrastructure.Mapping;

public class StockProfile : Profile
{
    public StockProfile()
    {
        CreateMap<Stock, StockDto>();
        CreateMap<StockDto, StockViewModel>();
    }
}
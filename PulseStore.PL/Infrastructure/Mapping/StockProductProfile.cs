using AutoMapper;
using PulseStore.BLL.Models.StockProduct;
using PulseStore.PL.ViewModels.StockProduct;

namespace PulseStore.PL.Infrastructure.Mapping
{
    public class StockProductProfile : Profile
    {
        public StockProductProfile()
        {
            CreateMap<StockProductQuantityDto, StockProductQuantityViewModel>();
        }
    }
}
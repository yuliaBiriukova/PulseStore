using AutoMapper;
using PulseStore.BLL.Models.Filters;
using PulseStore.PL.ViewModels.Filters;

namespace PulseStore.PL.Infrastructure.Mapping.Filters;

public class ProductFilterProfile : Profile
{
    public ProductFilterProfile()
    {
        CreateMap<ProductFilterViewModel, ProductFilter>();
        CreateMap<ProductFilterExtendedViewModel, ProductFilterExtended>();
    }
}
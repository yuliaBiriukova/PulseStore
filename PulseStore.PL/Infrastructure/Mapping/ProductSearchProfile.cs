using AutoMapper;
using PulseStore.BLL.Models.Filters;
using PulseStore.BLL.Models.Product;
using PulseStore.PL.ViewModels.Filters;
using PulseStore.PL.ViewModels.Product;

namespace PulseStore.PL.Infrastructure.Mapping;

public class ProductSearchProfile : Profile
{
    public ProductSearchProfile()
    {
        CreateMap<ProductSearchFilterViewModel, ProductSearchFilter>();
    }
}
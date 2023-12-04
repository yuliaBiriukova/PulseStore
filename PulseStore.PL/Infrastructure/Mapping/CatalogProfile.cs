using AutoMapper;
using PulseStore.BLL.Models.Catalog;
using PulseStore.BLL.Models.Product;
using PulseStore.PL.ViewModels.Catalog;

namespace PulseStore.PL.Infrastructure.Mapping;

public class CatalogProfile : Profile
{
    public CatalogProfile()
    {
        CreateMap(typeof(CatalogModel<>), typeof(CatalogModel<>));
        CreateMap<CatalogModel<CatalogProductDto>, CatalogViewModel>();
        CreateMap<CatalogModel<CatalogProductExtendedDto>, CatalogExtendedViewModel>();
        CreateMap<CatalogModel<AdminCatalogProductDto>, AdminCatalogViewModel>();
    }
}
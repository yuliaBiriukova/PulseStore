using AutoMapper;
using PulseStore.BLL.Entities;
using PulseStore.BLL.Models.ProductPhoto;
using PulseStore.PL.ViewModels.ProductPhoto;

namespace PulseStore.PL.Infrastructure.Mapping;

public class ProductPhotoProfile : Profile
{
    public ProductPhotoProfile()
    {
        CreateMap<ProductPhoto, CatalogProductPhotoDto>();
        CreateMap<CatalogProductPhotoDto, CatalogProductPhotoViewModel>();
        CreateMap<ProductPhoto, ProductPhotoForEditViewModel>();
    }
}
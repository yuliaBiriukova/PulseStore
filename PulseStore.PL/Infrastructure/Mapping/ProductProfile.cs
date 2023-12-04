using AutoMapper;
using PulseStore.BLL.Entities;
using PulseStore.BLL.Models.Product;
using PulseStore.BLL.Models.ProductPhoto;
using PulseStore.PL.ViewModels.Product;

namespace PulseStore.PL.Infrastructure.Mapping;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, CatalogProductDto>();
        CreateMap<CatalogProductDto, CatalogProductViewModel>();

        CreateMap<Product, CatalogProductViewModel>().ReverseMap();
        CreateMap<CatalogProductPhotoDto, ProductPhoto>().ReverseMap();

        CreateMap<Product, CatalogProductExtendedDto>();
        CreateMap<CatalogProductExtendedDto, CatalogProductExtendedViewModel>();

        CreateMap<Product, AdminCatalogProductDto>();
        CreateMap<AdminCatalogProductDto, AdminCatalogProductViewModel>();

        CreateMap<CreateProductViewModel, Product>()
            .ForMember(dest => dest.IsPublished, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => DateTime.Now));
        CreateMap<EditProductViewModel, Product>();

        CreateMap<Product, CartProductDto>()
            .ForMember(dest => dest.ProductPhoto, opt => opt.MapFrom(src => src.ProductPhotos.FirstOrDefault(p => p.SequenceNumber == 1)));
        CreateMap<CartProductDto, CartProductViewModel>();
        CreateMap<Product, ProductForEditViewModel>();
        CreateMap<DuplicateProductViewModel, Product>()
            .ForMember(dest => dest.Name, opt=> opt.MapFrom(src => src.Name + " - Copy"))
            .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => DateTime.Now)); 
    }
}
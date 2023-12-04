using AutoMapper;
using PulseStore.BLL.Entities;
using PulseStore.BLL.Models.Product;
using PulseStore.BLL.Models.Product.UserProductView;
using PulseStore.PL.ViewModels.Product.UserProductView;

namespace PulseStore.PL.Infrastructure.Mapping;

public class UserProductViewProfile : Profile
{
    public UserProductViewProfile()
    {
        CreateMap<UserProductViewDto, UserProductView>().ReverseMap();
        CreateMap<CatalogProductDto, Product>()
            .ForMember(dest => dest.ProductPhotos, opt => opt.MapFrom(src => src.ProductPhotos))
            .ReverseMap();

        CreateMap<UserProductView, UserProductViewViewModel>()
            .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product))
            .ReverseMap();

        CreateMap<UserProductViewDto, UserProductViewViewModel>()
            .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product))
            .ReverseMap();

        CreateMap<AddUserProductViewViewModel, AddUserProductViewDto>();
        CreateMap<AddUserProductViewDto, UserProductView>();
    }
}
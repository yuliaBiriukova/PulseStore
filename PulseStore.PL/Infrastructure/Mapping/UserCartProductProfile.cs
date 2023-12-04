using AutoMapper;
using PulseStore.BLL.Entities;
using PulseStore.BLL.Models.Cart;
using PulseStore.BLL.Models.Product;
using PulseStore.PL.ViewModels.Cart;
using PulseStore.PL.ViewModels.Product;

namespace PulseStore.PL.Infrastructure.Mapping;

public class UserCartProductProfile : Profile
{
    public UserCartProductProfile()
    {
        CreateMap<UserCartProduct, UserCartProductDto>().ReverseMap();
        CreateMap<UserCartProductDto, UserCartProductViewModel>().ReverseMap();
        CreateMap<AddCartProductViewModel, AddUserCartProductDto>();
        CreateMap<AddUserCartProductDto, UserCartProduct>();
        CreateMap<UpdateUserCartProductViewModel, UpdateUserCartProductDto>();
        CreateMap<UpdateUserCartProductDto, UserCartProduct>();
        CreateMap<CartProductInfoDto, CartProductInfoViewModel>();
    }
}
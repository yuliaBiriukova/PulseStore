using AutoMapper;
using PulseStore.BLL.Entities;
using PulseStore.BLL.Models.Category;
using PulseStore.PL.ViewModels.Category;

namespace PulseStore.PL.Infrastructure.Mapping;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<Category, CategoryDto>();
        CreateMap<CategoryDto, CategoryViewModel>();
        CreateMap<CategoryCreateViewModel, Category>();

        CreateMap<Category, CategoryExtendedDto>()
            .ForMember(
                dest => dest.ProductsQuantity,
                opt => opt.MapFrom(src => src.Products.Count())
            );
        CreateMap<CategoryExtendedDto, CategoryExtendedViewModel>();
        CreateMap<Category, CatalogCategoryDto>();
        CreateMap<CatalogCategoryDto, CatalogCategoryViewModel>();
    }
}
using AutoMapper;
using PulseStore.BLL.Models.Product;
using PulseStore.PL.ViewModels.Product;

namespace PulseStore.PL.Infrastructure.Mapping;

public class MoveToCategoryProfile : Profile
{
    public MoveToCategoryProfile()
    {
        CreateMap<ProductMoveCategoryViewModel, ProductMoveCategoryDto>();
    }
}
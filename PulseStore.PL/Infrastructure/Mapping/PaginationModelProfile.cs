using AutoMapper;
using PulseStore.BLL.Entities.Security;
using PulseStore.BLL.Features.Security.Queries;
using PulseStore.BLL.Models.Filters;
using PulseStore.BLL.Models.Utils;
using PulseStore.PL.ViewModels.Filters;
using PulseStore.PL.ViewModels.Security;

namespace PulseStore.PL.Infrastructure.Mapping;

public class PaginationModelProfile : Profile
{
    public PaginationModelProfile()
    {
        CreateMap(typeof(PaginationModel<>), typeof(PaginationModel<>));

        CreateMap<PaginationFilterViewModel, PaginationFilter>();
        CreateMap<PaginationModel<SecurityUser>, PaginationModel<SecurityUsersDto>>();
        CreateMap<PaginationModel<SecurityUsersDto>, PaginationModel<SecurityUserViewModel>>();

    }
}
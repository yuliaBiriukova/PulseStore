using AutoMapper;
using PulseStore.BLL.Entities.Security;
using PulseStore.BLL.Features.Security.Commands;
using PulseStore.BLL.Features.Security.Queries;
using PulseStore.PL.ViewModels.Security;

namespace PulseStore.PL.Infrastructure.Mapping.Security;

public class SecurityUserProfile : Profile
{
    public SecurityUserProfile()
    {
        CreateMap<AddSecurityUserCommand, SecurityUser>();
        CreateMap<SecurityUser, SecurityUsersDto>();
        CreateMap<SecurityUsersDto, SecurityUserViewModel>();
        CreateMap<EditSecurityUserCommand, SecurityUser>();
    }
}
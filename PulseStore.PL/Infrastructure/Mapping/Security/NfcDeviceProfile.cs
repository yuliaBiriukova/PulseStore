using AutoMapper;
using PulseStore.BLL.Entities.Security;
using PulseStore.BLL.Features.Security.Queries;
using PulseStore.PL.ViewModels.Security;

namespace PulseStore.PL.Infrastructure.Mapping.Security;

public class NfcDeviceProfile : Profile
{
    public NfcDeviceProfile()
    {
        CreateMap<NfcDevice, NfcDeviceDto>();
        CreateMap<NfcDeviceDto, NfcDeviceViewModel>();
    }
}
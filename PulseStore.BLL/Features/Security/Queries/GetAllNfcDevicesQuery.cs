using AutoMapper;
using PulseStore.BLL.Repositories.Security;
using MediatR;

namespace PulseStore.BLL.Features.Security.Queries;

public record GetAllNfcDevicesQuery() : IRequest<IEnumerable<NfcDeviceDto>>;

public record NfcDeviceDto(
    int Id,
    string NUID);

public class GetAllNfcDevicesQueryHandler : IRequestHandler<GetAllNfcDevicesQuery, IEnumerable<NfcDeviceDto>>
{
    private readonly IMapper _mapper;
    private readonly INfcDeviceRepository _nfcDeviceRepository;

    public GetAllNfcDevicesQueryHandler(IMapper mapper, INfcDeviceRepository nfcDeviceRepository)
    {
        _mapper = mapper;
        _nfcDeviceRepository = nfcDeviceRepository;
    }

    public async Task<IEnumerable<NfcDeviceDto>> Handle(GetAllNfcDevicesQuery request, CancellationToken cancellationToken)
    {
        var nfcDevices = await _nfcDeviceRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<NfcDeviceDto>>(nfcDevices);
    }
}
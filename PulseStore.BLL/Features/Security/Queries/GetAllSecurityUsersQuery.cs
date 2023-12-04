using AutoMapper;
using PulseStore.BLL.Entities;
using PulseStore.BLL.Entities.Security.Enums;
using PulseStore.BLL.Models.Filters;
using PulseStore.BLL.Models.Stock;
using PulseStore.BLL.Models.Utils;
using PulseStore.BLL.Repositories.Security;
using MediatR;

namespace PulseStore.BLL.Features.Security.Queries;
public record GetAllSecurityUsersQuery(PaginationFilter filter) : IRequest<PaginationModel<SecurityUsersDto>>;

public record SecurityUsersDto(
    int Id,
    string FirstName,
    string LastName,
    SecurityUserType UserType,
    ICollection<StockDto> Stocks
    );

public class GetAllSecurityUsersQueryHandler : IRequestHandler<GetAllSecurityUsersQuery, PaginationModel<SecurityUsersDto>>
{
    private readonly IMapper _mapper;
    private readonly ISecurityUserRepository _securityUserRepository;

    public GetAllSecurityUsersQueryHandler(IMapper mapper, ISecurityUserRepository securityUserRepository)
    {
        _mapper = mapper;
        _securityUserRepository = securityUserRepository;
    }

    public async Task<PaginationModel<SecurityUsersDto>> Handle(GetAllSecurityUsersQuery request, CancellationToken cancellationToken)
    {
        var securityUsers = await _securityUserRepository.GetUsersAsync(request.filter);
        var result = _mapper.Map<PaginationModel<SecurityUsersDto>>(securityUsers);
        return result;
        
    }
}
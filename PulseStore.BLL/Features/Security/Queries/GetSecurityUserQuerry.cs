using AutoMapper;
using PulseStore.BLL.Models.Utils;
using PulseStore.BLL.Repositories.Security;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PulseStore.BLL.Features.Security.Queries
{
    public record GetSecurityUserQuery(int id) : IRequest<SecurityUsersDto>;


    public class GetSecurityUserQueryHandler : IRequestHandler<GetSecurityUserQuery, SecurityUsersDto>
    {
        private readonly IMapper _mapper;
        private readonly ISecurityUserRepository _securityUserRepository;

        public GetSecurityUserQueryHandler(IMapper mapper, ISecurityUserRepository securityUserRepository)
        {
            _mapper = mapper;
            _securityUserRepository = securityUserRepository;
        }

        public async Task<SecurityUsersDto> Handle(GetSecurityUserQuery request, CancellationToken cancellationToken)
        {
            var securityUsers = await _securityUserRepository.GetSecurityUser(request.id);
            var result = _mapper.Map<SecurityUsersDto>(securityUsers);
            return result;

        }
    }
}

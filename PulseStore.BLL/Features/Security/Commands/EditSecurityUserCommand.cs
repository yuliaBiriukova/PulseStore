using AutoMapper;
using FluentValidation;
using PulseStore.BLL.Entities.Security;
using PulseStore.BLL.Repositories.Security;
using PulseStore.BLL.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PulseStore.BLL.Entities.Security.Enums;

namespace PulseStore.BLL.Features.Security.Commands
{
    public record EditSecurityUserCommand(
    int Id,
    string? FirstName,
    string? LastName,
    SecurityUserType? UserType,
    int NfcDeviceId,
    IEnumerable<int>? StockIds) : IRequest<int>;

    public class EditSecurityUserCommandHandler : IRequestHandler<EditSecurityUserCommand, int>
    {
        private readonly IMapper _mapper;
        private readonly IStockRepository _stockRepository;
        private readonly ISecurityUserRepository _securityUserRepository;

        public EditSecurityUserCommandHandler(IMapper mapper, IStockRepository stockRepository, ISecurityUserRepository securityUserRepository)
        {
            _mapper = mapper;
            _stockRepository = stockRepository;
            _securityUserRepository = securityUserRepository;
        }

        public async Task<int> Handle(EditSecurityUserCommand request, CancellationToken cancellationToken)
        {
            var newSecurityUser = _mapper.Map<SecurityUser>(request);
            if (request.StockIds != null)
            {
                newSecurityUser.Stocks = (await _stockRepository.GetByIdsAsync(request.StockIds)).ToList();
            }
            


            var result = await _securityUserRepository.EditSecurityUser(newSecurityUser);
            return result;
        }

    }

    public class EditSecurityUserCommandValidator : AbstractValidator<EditSecurityUserCommand>
    {
        private readonly IStockRepository _stockRepository;

        public EditSecurityUserCommandValidator(INfcDeviceRepository nfcDeviceRepository, IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;


            RuleFor(x => x.StockIds)
                .MustAsync(ContainValidStockIds)
                .WithMessage("One or more StockIds are invalid.");
        }


        private async Task<bool> ContainValidStockIds(IEnumerable<int> stockIds, CancellationToken cancellationToken)
        {
            var validStockIds = await _stockRepository.GetAllIdsAsync();

            return stockIds.All(id => validStockIds.Contains(id));
        }
    }
}

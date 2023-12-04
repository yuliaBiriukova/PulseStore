using AutoMapper;
using FluentValidation;
using PulseStore.BLL.Entities.Security;
using PulseStore.BLL.Entities.Security.Enums;
using PulseStore.BLL.Repositories;
using PulseStore.BLL.Repositories.Security;
using MediatR;

namespace PulseStore.BLL.Features.Security.Commands;

public record AddSecurityUserCommand(
    string FirstName,
    string LastName,
    SecurityUserType UserType,
    int NfcDeviceId,
    IEnumerable<int> StockIds) : IRequest<int>;

public class AddSecurityUserCommandHandler : IRequestHandler<AddSecurityUserCommand, int>
{
    private readonly IMapper _mapper;
    private readonly IStockRepository _stockRepository;
    private readonly ISecurityUserRepository _securityUserRepository;

    public AddSecurityUserCommandHandler(IMapper mapper, IStockRepository stockRepository, ISecurityUserRepository securityUserRepository)
    {
        _mapper = mapper;
        _stockRepository = stockRepository;
        _securityUserRepository = securityUserRepository;
    }

    public async Task<int> Handle(AddSecurityUserCommand request, CancellationToken cancellationToken)
    {

        var newSecurityUser = _mapper.Map<SecurityUser>(request);
        newSecurityUser.Stocks = (await _stockRepository.GetByIdsAsync(request.StockIds)).ToList();

        var id = await _securityUserRepository.AddAsync(newSecurityUser);
        return id;
    }
}

public class AddSecurityUserCommandValidator : AbstractValidator<AddSecurityUserCommand>
{
    private readonly INfcDeviceRepository _nfcDeviceRepository;
    private readonly IStockRepository _stockRepository;

    public AddSecurityUserCommandValidator(INfcDeviceRepository nfcDeviceRepository, IStockRepository stockRepository)
    {
        _nfcDeviceRepository = nfcDeviceRepository;
        _stockRepository = stockRepository;

        RuleFor(x => x.NfcDeviceId)
            .MustAsync(BeAmongAllowedNfcDeviceIds)
            .WithMessage("There is no such NfcDevice with specified NfcDeviceId.");

        RuleFor(x => x.StockIds)
            .MustAsync(ContainValidStockIds)
            .WithMessage("One or more StockIds are invalid.");
    }

    private async Task<bool> BeAmongAllowedNfcDeviceIds(int value, CancellationToken cancellationToken)
    {
        var allowedNfcDeviceIds = await _nfcDeviceRepository.GetAllIdsAsync();
        return allowedNfcDeviceIds.Contains(value);
    }

    private async Task<bool> ContainValidStockIds(IEnumerable<int> stockIds, CancellationToken cancellationToken)
    {
        var validStockIds = await _stockRepository.GetAllIdsAsync();

        return stockIds.All(id => validStockIds.Contains(id));
    }
}
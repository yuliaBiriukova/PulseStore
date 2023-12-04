using PulseStore.BLL.Repositories.Security;
using MediatR;

public record DeleteSecurityUserCommand( int userId) : IRequest<bool>;

public class DeleteSecurityUserCommandHandler : IRequestHandler<DeleteSecurityUserCommand, bool>
{

    private readonly ISecurityUserRepository _securityUserRepository;

    public DeleteSecurityUserCommandHandler(ISecurityUserRepository securityUserRepository)
    {
        _securityUserRepository = securityUserRepository;
    }

    public async Task<bool> Handle(DeleteSecurityUserCommand request, CancellationToken cancellationToken)
    {

        var result = await _securityUserRepository.DeleteAsync(request.userId);
        return result;
    }

}

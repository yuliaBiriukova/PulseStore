using PulseStore.BLL.Entities.Security;
using PulseStore.BLL.Models.Filters;
using PulseStore.BLL.Models.Utils;

namespace PulseStore.BLL.Repositories.Security;

public interface ISecurityUserRepository
{
    /// <summary>
    ///     Adds <see cref="SecurityUser"/> entity.
    /// </summary>
    /// <param name="securityUser"><see cref="SecurityUser"/> entity to add.</param>
    /// <returns>
    ///     Id of the added entity.
    /// </returns>
    Task<int> AddAsync(SecurityUser securityUser);
    /// <summary>
    ///    Get all <see cref="SecurityUser"/> entities.
    /// </summary>
    /// <param name="filter"><see cref="PaginationFilter"/> filter for result of method.</param>
    /// <returns>
    ///     Paginated <see cref="SecurityUser"/>.
    /// </returns>
    Task<PaginationModel<SecurityUser>> GetUsersAsync(PaginationFilter filter);

    Task<bool> DeleteAsync(int userId);
    Task<int> EditSecurityUser(SecurityUser changedUser);
    Task<SecurityUser> GetSecurityUser(int id);
}
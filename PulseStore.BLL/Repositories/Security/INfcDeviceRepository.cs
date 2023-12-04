using PulseStore.BLL.Entities.Security;

namespace PulseStore.BLL.Repositories.Security;

public interface INfcDeviceRepository
{
    /// <summary>
    ///     Gets all <see cref="NfcDevice"/> entities.
    /// </summary>
    /// <returns>
    ///     List of all <see cref="NfcDevice"/> entities.
    /// </returns>
    Task<IEnumerable<NfcDevice>> GetAllAsync();

    /// <summary>
    ///     Gets ids of all <see cref="NfcDevice"/> entities.
    /// </summary>
    /// <returns>
    ///     List of ids of all <see cref="NfcDevice"/> entities.
    /// </returns>
    Task<IEnumerable<int>> GetAllIdsAsync();
}
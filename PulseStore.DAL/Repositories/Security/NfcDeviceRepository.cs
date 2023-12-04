using PulseStore.BLL.Entities.Security;
using PulseStore.BLL.Repositories.Security;
using PulseStore.DAL.Database;
using Microsoft.EntityFrameworkCore;

namespace PulseStore.DAL.Repositories.Security;

public class NfcDeviceRepository : INfcDeviceRepository
{
    private readonly PulseStoreContext _dbContext;

    public NfcDeviceRepository(PulseStoreContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<NfcDevice>> GetAllAsync()
    {
        return await _dbContext.NfcDevices.ToListAsync();
    }

    public async Task<IEnumerable<int>> GetAllIdsAsync()
    {
        return await _dbContext.NfcDevices.Select(n => n.Id).ToListAsync();
    }
}
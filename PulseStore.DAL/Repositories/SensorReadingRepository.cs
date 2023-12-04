using PulseStore.BLL.Entities.SensorReadings;
using PulseStore.BLL.Entities.SensorReadings.Enums;
using PulseStore.BLL.Repositories;
using PulseStore.DAL.Database;
using Microsoft.EntityFrameworkCore;

namespace PulseStore.DAL.Repositories
{
    public class SensorReadingRepository : ISensorReadingRepository
    {
        private readonly PulseStoreContext _dbContext;

        public SensorReadingRepository(PulseStoreContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddSensorAsync(Sensor entity) => _dbContext.Sensors.Add(entity);

        public async Task<bool> CheckSensorExistsAsync(string deviceMacAddress) => await _dbContext.Sensors.AnyAsync(s => s.DeviceMacAddress == deviceMacAddress);

        public async Task<SensorReading?> GetLastTodaySensorReadingAsync(int stockId, SensorType sensorType)
        {
            return await _dbContext.SensorReadings
                .Include(sr => sr.Sensor)
                .Where(sr => sr.ReadingDate.Date == DateTime.Today && sr.Sensor.StockId == stockId && sr.Sensor.SensorType == sensorType)
                .OrderByDescending(sr => sr.ReadingDate)
                .FirstOrDefaultAsync();
        }

        public async Task<Sensor> GetSensorByCodeAsync(string deviceMacAddress) => await _dbContext.Sensors
            .Include(s => s.SensorReadings)
            .SingleAsync(s => s.DeviceMacAddress == deviceMacAddress);

        public async Task<IEnumerable<SensorReading>> GetSensorReadingsAsync(int stockId, SensorType sensorType)
        {
            return await _dbContext.SensorReadings
                .Include(sr => sr.Sensor)
                .Where(sr => sr.ReadingDate.Date > DateTime.Today.AddDays(-7) && sr.Sensor.StockId == stockId && sr.Sensor.SensorType == sensorType)
                .OrderByDescending(sr => sr.ReadingDate)
                .ToListAsync();
        }

        public async Task SaveAsync() => await _dbContext.SaveChangesAsync();
    }
}
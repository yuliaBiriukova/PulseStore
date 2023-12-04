using PulseStore.BLL.Entities.SensorReadings;
using PulseStore.BLL.Entities.SensorReadings.Enums;

namespace PulseStore.BLL.Repositories
{
    public interface ISensorReadingRepository
    {
        void AddSensorAsync(Sensor entity);

        Task<bool> CheckSensorExistsAsync(string deviceMacAddress);

        Task<SensorReading?> GetLastTodaySensorReadingAsync(int stockId, SensorType sensorType);

        Task<Sensor> GetSensorByCodeAsync(string deviceMacAddress);

        Task<IEnumerable<SensorReading>> GetSensorReadingsAsync(int stockId, SensorType sensorType);

        Task SaveAsync();
    }
}

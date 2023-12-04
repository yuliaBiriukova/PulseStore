using System.Runtime.Serialization;
using PulseStore.BLL.Entities.SensorReadings.ValueObject;
using Newtonsoft.Json;

namespace PulseStore.BLL.Entities.SensorReadings.Handlers
{
    public static class SensorReadingHandler
    {
        public static SensorSensorReading CreateSensorReading(string json)
        {
            var reading = JsonConvert.DeserializeObject<SensorSensorReading>(json);
            
            if(reading is null) {
                throw new Exception("Not correct json");
            }

            reading.ReadingDate ??= new DateTime();

            return reading;
        }
    }
}

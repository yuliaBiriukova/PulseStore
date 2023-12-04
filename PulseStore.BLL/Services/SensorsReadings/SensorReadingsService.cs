using System.Reactive.Linq;
using System.Text;
using AutoMapper;
using PulseStore.BLL.Entities.SensorReadings;
using PulseStore.BLL.Entities.SensorReadings.ValueObject;
using PulseStore.BLL.ExternalServices;
using PulseStore.BLL.Models.SensorReadings;
using PulseStore.BLL.Repositories;

namespace PulseStore.BLL.Services.SensorsReadings
{
    public class SensorReadingsService : ISensorReadingService
    {
        private readonly IMapper _mapper;
        private readonly ISensorReadingRepository _sensorReadingRepository;
        private readonly IStockProductRepository _stockProductRepository;
        private readonly IMqttExternalService _mqttService;
        private IDisposable? _subscription;

        public SensorReadingsService(IMapper mapper, ISensorReadingRepository sensorReadingRepository, IMqttExternalService mqttService, IStockProductRepository stockProductRepository)
        {
            _mapper = mapper;
            _sensorReadingRepository = sensorReadingRepository;
            _mqttService = mqttService;
            _stockProductRepository = stockProductRepository;
        }

        public async Task ConnectToMqttServer(MqttOptionsDto optionsMapped)
        {
            await _mqttService.CreateMqttSubscription(optionsMapped);

            _subscription = _mqttService.NewReadingStream
                .TakeUntil(Observable.Timer(TimeSpan.FromMinutes(5)))
                .Select(e => Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment))
                .Select(item => Observable.FromAsync(() => HandleNewSensorReadingAsync(item)))
                .Concat()
                .Subscribe(
                    (e) =>
                    {
                        Console.WriteLine("Succeed");
                    },
                    async () =>
                    {
                        await _mqttService.DisconnectFromMqttAsync();
                        _subscription!.Dispose();
                    });

            if (optionsMapped.IsTestDataRequired)
            {
                var mockedData = Enumerable.Range(0, 10)
                    .Select(_ => SensorSensorReading.CreateMockReading());
                await _mqttService.TestPublish(optionsMapped, mockedData);
            }

        }

        private async Task HandleNewSensorReadingAsync(string message)
        {
            Sensor entity;
            var sensorSensorReading = Sensor.CreateSensorSensorReading(message);
            var sensorReading = _mapper.Map<SensorReading>(sensorSensorReading);

            if (await _sensorReadingRepository.CheckSensorExistsAsync(sensorSensorReading.DeviceAddress))
            {
                entity = await _sensorReadingRepository.GetSensorByCodeAsync(sensorSensorReading.DeviceAddress);
            }
            else
            {
                entity = Sensor.CreateFromSensorSensorReading(sensorSensorReading);
                _sensorReadingRepository.AddSensorAsync(entity);
                
                //TODO: logic to set stock is not clear so get the first one
                var stock = await _stockProductRepository.GetMainStock();
                entity.SetStockId(stock.Id);
            }

            entity.AddReading(sensorReading);
            entity.Validate();

            await _sensorReadingRepository.SaveAsync();
        }
    }
}

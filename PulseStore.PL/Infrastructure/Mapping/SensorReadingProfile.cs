using AutoMapper;
using PulseStore.BLL.Entities.SensorReadings;
using PulseStore.BLL.Features.SensorReading.Queries;
using PulseStore.PL.ViewModels.SensorReading;

namespace PulseStore.PL.Infrastructure.Mapping;

public class SensorReadingProfile : Profile
{
    public SensorReadingProfile()
    {
        CreateMap<DailySensorReadingDto, DailySensorReadingViewModel>();
        CreateMap<SensorReading, SensorReadingDto>();
        CreateMap<SensorReadingDto, SensorReadingViewModel>();
    }
}
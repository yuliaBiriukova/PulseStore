using FluentValidation;
using PulseStore.BLL.Entities.SensorReadings.Enums;
using PulseStore.BLL.Repositories;
using MediatR;

namespace PulseStore.BLL.Features.SensorReading.Queries;

public record GetDailySensorReadingsQuery(int StockId, SensorType SensorType) : IRequest<IEnumerable<DailySensorReadingDto>>;

public record DailySensorReadingDto(
    DateTime Date,
    float? AverageTemperature,
    float? AverageHumidity);

public class GetDailySensorReadingsQueryHandler : IRequestHandler<GetDailySensorReadingsQuery, IEnumerable<DailySensorReadingDto>>
{
    private readonly ISensorReadingRepository _sensorReadingRepository;

    public GetDailySensorReadingsQueryHandler(ISensorReadingRepository sensorReadingRepository)
    {
        _sensorReadingRepository = sensorReadingRepository;
    }

    public async Task<IEnumerable<DailySensorReadingDto>> Handle(GetDailySensorReadingsQuery request, CancellationToken cancellationToken)
    {
        var sensorReadings = await _sensorReadingRepository.GetSensorReadingsAsync(request.StockId, request.SensorType);

        var dailySensorReadings = sensorReadings
            .GroupBy(sr => sr.ReadingDate.Date)
            .Select(group => new DailySensorReadingDto(
                group.Key,
                group.Average(sr => sr.Temperature),
                group.Average(sr => sr.Humidity)
            ));

        return dailySensorReadings;
    }
}

public class GetDailySensorReadingsQueryValidator : AbstractValidator<GetDailySensorReadingsQuery>
{
    public GetDailySensorReadingsQueryValidator()
    {
        RuleFor(x => x.StockId).GreaterThan(0);
        RuleFor(x => x.SensorType)
            .Must(value => Enum.IsDefined(typeof(SensorType), value))
            .WithMessage("Value of SensorType is not defined in enum SensorType.");
    }
}
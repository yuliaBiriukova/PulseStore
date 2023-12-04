using AutoMapper;
using FluentValidation;
using PulseStore.BLL.Entities.SensorReadings.Enums;
using PulseStore.BLL.Repositories;
using MediatR;

namespace PulseStore.BLL.Features.SensorReading.Queries;

public record GetLastTodaySensorReadingQuery(int StockId, SensorType SensorType) : IRequest<SensorReadingDto?>;

public record SensorReadingDto(
    int Id,
    DateTime ReadingDate,
    float? Temperature,
    float? Humidity);

public class GetLastTodaySensorReadingQueryHandler : IRequestHandler<GetLastTodaySensorReadingQuery, SensorReadingDto?>
{
    private readonly IMapper _mapper;
    private readonly ISensorReadingRepository _sensorReadingRepository;

    public GetLastTodaySensorReadingQueryHandler(IMapper mapper, ISensorReadingRepository sensorReadingRepository)
    {
        _mapper = mapper;
        _sensorReadingRepository = sensorReadingRepository;
    }

    public async Task<SensorReadingDto?> Handle(GetLastTodaySensorReadingQuery request, CancellationToken cancellationToken)
    {
        var lastTodaySensorReading = await _sensorReadingRepository.GetLastTodaySensorReadingAsync(request.StockId, request.SensorType);
        return _mapper.Map<SensorReadingDto>(lastTodaySensorReading);
    }
}

public class GetLastTodaySensorReadingQueryValidator : AbstractValidator<GetLastTodaySensorReadingQuery>
{
    public GetLastTodaySensorReadingQueryValidator()
    {
        RuleFor(x => x.StockId).GreaterThan(0);
        RuleFor(x => x.SensorType)
            .Must(value => Enum.IsDefined(typeof(SensorType), value))
            .WithMessage("Value of SensorType is not defined in enum SensorType.");
    }
}
using AutoMapper;
using PulseStore.BLL.Features.SensorReading.Queries;
using PulseStore.PL.ViewModels.SensorReading;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace PulseStore.PL.Controllers.AdminControllers
{
    [Route("api/admin/sensor-readings")]
    [ApiExplorerSettings(GroupName = "Admin/SensorReadings")]
    [ApiController]
    public class SensorReadingsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public SensorReadingsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        /// <summary>
        ///     Gets list of objects with average temperature and humidity for each day.
        /// </summary>
        /// <param name="query">The query with filtering parameters.</param>
        /// <returns>
        ///     List of objects with average temperature and humidity for each day.
        /// </returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DailySensorReadingViewModel>>> GetDailySensorReadings([FromQuery] GetDailySensorReadingsQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(_mapper.Map<IEnumerable<DailySensorReadingViewModel>>(result));
        }

        /// <summary>
        ///     Gets last today sensor reading.
        /// </summary>
        /// <param name="query">The query with filtering parameters.</param>
        /// <returns>
        ///     Last today sensor reading if such exists; otherwise null.
        /// </returns>
        [HttpGet("last")]
        public async Task<ActionResult<SensorReadingViewModel?>> GetLastTodaySensorReadings([FromQuery] GetLastTodaySensorReadingQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(_mapper.Map<SensorReadingViewModel>(result));
        }
    }
}
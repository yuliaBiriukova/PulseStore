using AutoMapper;
using PulseStore.BLL.Features.Security.Commands;
using PulseStore.BLL.Features.Security.Queries;
using PulseStore.BLL.Models.Filters;
using PulseStore.BLL.Models.Utils;
using PulseStore.PL.ViewModels.Security;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace PulseStore.PL.Controllers.AdminControllers
{
    [Route("api/admin/[controller]")]
    [ApiExplorerSettings(GroupName = "Admin/Security")]
    [ApiController]
    public class SecurityController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public SecurityController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        /// <summary>
        ///     Gets list of all NFC devices.
        /// </summary>
        /// <returns>
        ///     List of all NFC devices.
        /// </returns>
        [HttpGet("nfc-devices")]
        public async Task<ActionResult<IEnumerable<NfcDeviceViewModel>>> GetAllNfcDevices()
        {
            var result = await _mediator.Send(new GetAllNfcDevicesQuery());
            return Ok(_mapper.Map<IEnumerable<NfcDeviceViewModel>>(result));
        }

        /// <summary>
        ///     Adds new user to security.
        /// </summary>
        /// <returns>
        ///     Id of the added security user.
        /// </returns>
        [HttpPost("users")]
        public async Task<ActionResult<int>> AddSecurityUser([FromForm] AddSecurityUserCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("users")]
        public async Task<ActionResult<PaginationModel<SecurityUserViewModel>>> GetSecurityUsers([FromQuery] GetAllSecurityUsersQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(_mapper.Map<PaginationModel<SecurityUserViewModel>>(result));
        }

        [HttpDelete("users")]
        public async Task<ActionResult<bool>> DeleteSecurityUser([FromQuery] DeleteSecurityUserCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("users")]
        public async Task<ActionResult<int>> EditSecurityUser([FromForm] EditSecurityUserCommand command )
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("user")]
        public async Task<ActionResult<int>> GetSecurityUser([FromQuery] GetSecurityUserQuery command)
        {
            var result = await _mediator.Send(command);
            return Ok(_mapper.Map<SecurityUserViewModel>(result));
        }
    }
}

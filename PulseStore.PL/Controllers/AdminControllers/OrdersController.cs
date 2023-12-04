using AutoMapper;
using PulseStore.BLL.Entities.OrderDocuments.Enums;
using PulseStore.BLL.Entities.Orders.Enums;
using PulseStore.BLL.Models.Filters;
using PulseStore.BLL.Services.OrderDocument;
using PulseStore.BLL.Services.OrderLetters;
using PulseStore.BLL.Services.Orders;
using PulseStore.PL.ViewModels.Filters;
using PulseStore.PL.ViewModels.Order;
using Microsoft.AspNetCore.Mvc;

namespace PulseStore.PL.Controllers.AdminControllers
{
    [Route("api/admin/[controller]")]
    [ApiExplorerSettings(GroupName = "Admin/Orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IOrderService _orderService;
        private readonly IOrderDocumentService _orderDocumentService;
        private readonly IOrderLetterService _orderLetterService;

        public OrdersController(IMapper mapper, IOrderService orderService, IOrderDocumentService orderDocumentService, IOrderLetterService orderLetterService)
        {
            _mapper = mapper;
            _orderService = orderService;
            _orderDocumentService = orderDocumentService;
            _orderLetterService = orderLetterService;
        }

        /// <summary>
        ///     Gets <see cref="OrdersListViewModel"/> entity by conditions in <see cref="OrderFilterViewModel"/> with pagination and filters for admin.
        /// </summary>
        /// <param name = "filter" ><see cref="OrderFilterViewModel"/> entity with conditions for orders</param>
        /// <returns>
        ///     <see cref="OrdersListViewModel"/> entity with orders that satisfy the filter.
        /// </returns>
        [HttpGet]
        public async Task<ActionResult<OrdersListViewModel>> GetOrders([FromQuery] OrderFilterViewModel filterModel)
        {
            var filter = _mapper.Map<OrderFilter>(filterModel);
            var orders = await _orderService.GetAsync(filter);
            return Ok(_mapper.Map<OrdersListViewModel>(orders));
        }

        /// <summary>
        ///     Gets <see cref="AdminOrderDetailsViewModel"/> entity by specified id..
        /// </summary>
        /// <param name="id" >Id of order to get.</param>
        /// <returns>
        ///     <see cref="AdminOrderDetailsViewModel"/> entity with with specified id if such exists; otherwise null.
        /// </returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<AdminOrderDetailsViewModel>> GetOrderDetailsById(int id)
        {
            var order = await _orderService.GetByIdWithOrderProductsAsync(id);
            return Ok(_mapper.Map<AdminOrderDetailsViewModel>(order));
        }

        /// <summary>
        ///     Changes order status to Pending and sends payment email.
        /// </summary>
        /// <param name="id">Id of order which status to change and invoice to send.</param>
        /// <returns>
        ///     HTTP 200 OK if validation and update succeded; otherwise HTTP 400 BadRequest with error message.
        /// </returns>
        [HttpPut("{id}/status/pending")]
        public async Task<ActionResult<bool>> ChangeOrderStatusToPending(int id)
        {
            var isInvoiceCreated = await _orderDocumentService.UpsertAsync(id, OrderDocumentType.InvoiceDocument);
            if(!isInvoiceCreated)
            {
                return Ok(isInvoiceCreated);
            }

            var result = await _orderService.UpdateOrderStatusAsync(id, OrderStatus.Pending);
            if (result.IsSuccess)
            {
                var isEmailSent = await _orderLetterService.SendConfirmOrderLetter(id);
                return isEmailSent.Match<ActionResult<bool>>(
                    value => Ok(value),
                    err => BadRequest(err.Message));
            }

            return result.Match<ActionResult<bool>>(
                value => Ok(value),
                err => BadRequest(err.Message));
        }

        [HttpPut("{id}/status/rejected")]
        public async Task<ActionResult<bool>> ChangeOrderStatusToRejected(int id)
        {
            var result = await _orderService.UpdateOrderStatusAsync(id, OrderStatus.Rejected);
            if (result.IsSuccess)
            {
                var isEmailSent = await _orderLetterService.SendRejectOrderLetter(id);
                return result.Match<ActionResult<bool>>(
                    value => Ok(value),
                    err => BadRequest(err.Message));
            }

            return result.Match<ActionResult<bool>>(
                    value => Ok(value),
                    err => BadRequest(err.Message));
        }

        /// <summary>
        ///     Changes order status to Done and sends shipment email.
        /// </summary>
        /// <param name="id">Id of order which status to change and shipment document to send.</param>
        /// <returns>
        ///     HTTP 200 OK if validation and update succeded; otherwise HTTP 400 BadRequest with error message.
        /// </returns>
        [HttpPut("{id}/status/done")]
        public async Task<ActionResult<bool>> ChangeOrderStatusToDone(int id)
        {
            var isShipmentDocCreated = await _orderDocumentService.UpsertAsync(id, OrderDocumentType.ShipmentDocument);

            if (isShipmentDocCreated)
            {
                var isEmailSent = await _orderLetterService.SendOrderShipmentLetterAsync(id);

                if(isEmailSent.IsSuccess)
                {
                    var isStatusUpdated = await _orderService.UpdateOrderStatusAsync(id, OrderStatus.Done);
                    return isStatusUpdated.Match<ActionResult<bool>>(
                        value => Ok(value),
                        err => BadRequest(err.Message));
                }

                return isEmailSent.Match<ActionResult<bool>>(
                    value => Ok(value),
                    err => BadRequest(err.Message));
            }

            return Ok(isShipmentDocCreated);
        }
    }
}
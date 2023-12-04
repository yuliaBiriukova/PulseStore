using AutoMapper;
using PulseStore.BLL.Entities.OrderDocuments.Enums;
using PulseStore.BLL.Models.Filters;
using PulseStore.BLL.Models.Order;
using PulseStore.BLL.Services.OrderDocument;
using PulseStore.BLL.Services.Orders;
using PulseStore.PL.Extensions;
using PulseStore.PL.ViewModels.Filters;
using PulseStore.PL.ViewModels.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PulseStore.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "RequireAuthorizedUser")]
    public class OrdersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IOrderService _orderService;
        private readonly IOrderDocumentService _orderDocumentService;

        public OrdersController(IMapper mapper, IOrderService orderService, IOrderDocumentService orderDocumentService)
        {
            _mapper = mapper;
            _orderService = orderService;
            _orderDocumentService = orderDocumentService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderViewModel>>> GetAllOrdersAsync(
            [FromQuery] PaginationFilterViewModel filterModel)
        {
            Guid userId = User.GetUserId();

            var filter = _mapper.Map<PaginationFilter>(filterModel);

            var result = await _orderService.GetAllOrdersAsync(filter, userId);
            return Ok(_mapper.Map<IEnumerable<OrderViewModel>>(result));
        }

        /// <summary>
        ///     Adds new order for current user if authorized; otherwise anonimous.
        /// </summary>
        /// <param name="model"><see cref="AddOrderViewModel"/> with data about order.</param>
        /// <remarks>
        ///     Gets userId if authorized.
        /// </remarks>
        /// <returns>
        ///     HTTP 200 OK with Id of the added entity if addition succeded; otherwise HTTP 400 BadRequest with error message.
        /// </returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<int>> AddOrder(AddOrderViewModel model)
        {
            var userId = User.GetUserId();
            var newOrder = _mapper.Map<AddOrderDto>(model);
            var result = await _orderService.AddAsync(newOrder, userId);
            return result.Match<ActionResult<int>>(
                value => Ok(value),
                err => BadRequest(err.Message));
        }

        [HttpPost("{id}/cancel")]
        public async Task<ActionResult> CancelOrder(int id)
        {
            var userId = User.GetUserId();

            var result = await _orderService.CancelOrderAsync(userId, id);

            return result.Match<ActionResult>(
                success => Ok($"The order has been succesfully canceled!"),
                err => BadRequest(err.Message));
        }

        [HttpPost("{id}/confirm-payment")]
        public async Task<ActionResult> ConfirmPaymentOrder(int id)
        {
            var userId = User.GetUserId();

            var result = await _orderService.ConfirmPaymentOrderAsync(userId, id);

            return result.Match<ActionResult>(
                success => Ok($"The order has been succesfully paid!"),
                err => BadRequest(err.Message));
        }

        /// <summary>
        ///     Gets order invoice PDF file path by specified order id.
        /// </summary>
        /// <param name="id" >Id of order which invoice file path to get.</param>
        /// <returns>
        ///     Order invoice PDF file path.
        /// </returns>
        [HttpGet("invoice/{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<string>> GetOrderInvoicePdfById(int id)
        {
            var invoiceFilePath = await _orderDocumentService.GetOrderDocumentFilePathAsync(id, OrderDocumentType.InvoiceDocument);

            if (invoiceFilePath is null)
            {
                return BadRequest($"Getting invoice for order with id={id} failed.");
            }

            return Ok(invoiceFilePath);
        }
    }
}
using PulseStore.PL.ViewModels.Order;
using PulseStore.PL.ViewModels.Payment;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;

namespace PulseStore.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public PaymentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("create-checkout-session")]
        public IActionResult CreateCheckoutSession(PaymentDataViewModel paymentData)
        {
            var domain = _configuration["Customer:Host"];

            var items = paymentData.orderProducts.Select(orderProduct => new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = (long)(orderProduct.PricePerItem * 100),
                    Currency = "usd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = orderProduct.product.Name,
                        Images = new List<string> { orderProduct.product.ProductPhoto.ImagePath },
                    },
                },
                Quantity = orderProduct.Quantity
            }).ToList();

            var options = new SessionCreateOptions
            {
                LineItems = items,
                Mode = "payment",
                SuccessUrl = $"{domain}/profile/orders?orderId={paymentData.orderId}&resultType=success&index={paymentData.index}",
                CancelUrl = $"{domain}/profile/orders?orderId={paymentData.orderId}&resultType=error&index={paymentData.index}",
            };

            var service = new SessionService();
            Session session = service.Create(options);


            var responseData = new {url = session.Url};

            return new JsonResult(responseData) { StatusCode = 201 };
        }
    }
}

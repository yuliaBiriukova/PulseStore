using AutoMapper;
using PulseStore.BLL.Entities.TemplateFiles.Enums;
using PulseStore.BLL.ExternalServices.EmailSender;
using PulseStore.BLL.Models.Email;
using PulseStore.BLL.Models.Order.Reject;
using PulseStore.BLL.Services.Orders;
using PulseStore.BLL.Services.TemplateToHtml;
using LanguageExt.Common;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;
using System.Text;
using PulseStore.BLL.Entities.OrderDocuments.Enums;
using OrderDocumentEntity = PulseStore.BLL.Entities.OrderDocuments.OrderDocument;
using PulseStore.BLL.Services.OrderDocument;
using PulseStore.BLL.Entities.Orders.Enums;
using PulseStore.BLL.Models.Order.PaymentEmail;

namespace PulseStore.BLL.Services.OrderLetters;

public class OrderLetterService : IOrderLetterService
{
    private readonly IEmailSenderService _emailSenderService;
    private readonly ITemplateToHtmlService _templateToHtmlService;
    private readonly IOrderService _orderService;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    private readonly string _customerHost;
    private readonly IOrderDocumentService _orderDocumentService;

    public OrderLetterService(
        IEmailSenderService emailSenderService,
        ITemplateToHtmlService templateToHtmlService,
        IOrderService orderService,
        IMapper mapper,
        IConfiguration configuration,
        IOrderDocumentService orderDocumentService)
    {
        _emailSenderService = emailSenderService;
        _templateToHtmlService = templateToHtmlService;
        _orderService = orderService;
        _mapper = mapper;
        _configuration = configuration;
        _customerHost = _configuration["Customer:Host"];
        _orderDocumentService = orderDocumentService;
    }

    public async Task<Result<bool>> SendRejectOrderLetter(int orderId)
    {
        var order = await _orderService.GetByIdWithOrderProductsAsync(orderId);
        if(order == null )
        {
            return new Result<bool>(new ValidationException($"Order with id={orderId} does not exist."));
        }

        var rejectOrder = _mapper.Map<RejectOrderEmail>(order);
        // todo: add paths to matching functionality
        rejectOrder.CancelOrderPath = _customerHost;
        rejectOrder.OrderAvailablePath = _customerHost;

        var emailStream = await _templateToHtmlService.GetHtmlFileFromTemplateAsync(rejectOrder, TemplateFileType.RejectedEmail, rejectOrder.OrderProducts);
        if(emailStream == null )
        {
            return new Result<bool>(new ValidationException($"Template file for type {TemplateFileType.RejectedEmail} does not exist."));
        }

        var letterSubject = $"Your order #{orderId} was rejected";
        await SendEmail(rejectOrder.FullName, rejectOrder.Email, emailStream, letterSubject);

        return true;
    }

    public async Task<Result<bool>> SendOrderShipmentLetterAsync(int orderId)
    {
        var orderDocument = await _orderDocumentService.GetOrderDocumentAsync(orderId, OrderDocumentType.ShipmentDocument);

        if (orderDocument is null)
        {
            return new Result<bool>(new ValidationException($"Order with id={orderId} does not exist."));
        }

        var emailStream = await _templateToHtmlService
            .GetHtmlFileFromTemplateAsync<OrderDocumentEntity, object>(orderDocument, TemplateFileType.ShipmentEmail);

        if (emailStream is null)
        {
            return new Result<bool>(new ValidationException($"Template file for type {TemplateFileType.ShipmentEmail} does not exist."));
        }

        var orderCustomer = await _orderService.GetCustomerByOrderIdAsync(orderId);

        if (orderCustomer?.FullName is null) 
        {
            return new Result<bool>(new ValidationException($"Customer for order with id={orderId} does not exist."));
        }

        var letterSubject = $"Your order #{orderId} is sent to delivery";
        await SendEmail(orderCustomer.FullName, orderCustomer.Email, emailStream, letterSubject);

        return true;
    }

    public async Task<Result<bool>> SendConfirmOrderLetter(int orderId)
    {
        var paymentMethod = await _orderService.GetOrderPaymentMethodAsync(orderId);
        if (paymentMethod == null)
        {
            return new Result<bool>(new ValidationException($"Order with id={orderId} does not exist."));
        }

        var order = await _orderService.GetByIdWithOrderProductsAsync(orderId);
        if (order == null)
        {
            return new Result<bool>(new ValidationException($"Order with id={orderId} does not exist."));
        }

        var paymentOrder = _mapper.Map<OrderPaymentEmail>(order);
        // todo: add paths to matching functionality
        paymentOrder.CancelOrderPath = _customerHost;
        paymentOrder.ConfirmPaymentPath = _customerHost;

        var templateFileType = TemplateFileType.InvoicePaymentEmail;

        switch (paymentMethod)
        {
            case PaymentMethod.SellerBankAccount:
                paymentOrder.ProductPdfPath = await _orderDocumentService.GetOrderDocumentFilePathAsync(orderId, OrderDocumentType.InvoiceDocument);
                templateFileType = TemplateFileType.InvoicePaymentEmail;
                break;
            case PaymentMethod.Card:
                // todo: add valid path for stripe payment from email
                paymentOrder.PayWithStripePath = _customerHost;
                templateFileType = TemplateFileType.StripePaymentEmail;
                break;
            default:
                break;
        }

        var emailStream = await _templateToHtmlService.GetHtmlFileFromTemplateAsync(paymentOrder, templateFileType, paymentOrder.OrderProducts);
        if (emailStream == null)
        {
            return new Result<bool>(new ValidationException($"Template file for type {templateFileType} does not exist."));
        }

        var letterSubject = $"Your order #{orderId} was confirmed";
        await SendEmail(paymentOrder.FullName, paymentOrder.Email, emailStream, letterSubject);

        return true;
    }

    private async Task SendEmail(string fullName, string emailAddress, Stream emailStream, string letterSubject)
    {
        using (StreamReader reader = new StreamReader(emailStream, Encoding.UTF8))
        {
            var emailContent = reader.ReadToEnd();
            var email = new EmailMessage(fullName, emailAddress, letterSubject, emailContent);
            await _emailSenderService.SendEmailAsync(email);
        }
    }
}
using AutoMapper;
using PulseStore.BLL.Entities.TemplateFiles.Enums;
using PulseStore.BLL.Models.Order.Invoice;
using PulseStore.BLL.Repositories;
using PulseStore.BLL.Services.TemplateToHtml;
using PulseStore.BLL.Helpers.HtmlToPdf;
using PulseStore.BLL.Entities.OrderDocuments.Enums;
using PulseStore.BLL.Services.BlobStorage;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;

namespace PulseStore.BLL.Services.OrderDocument;

public class OrderDocumentService : IOrderDocumentService
{
    private readonly IMapper _mapper;
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderDocumentRepository _orderDocumentRepository;
    private readonly ITemplateToHtmlService _htmlFileService;
    private readonly IBlobStorageService _blobStorageService;

    public OrderDocumentService(IMapper mapper, IOrderRepository orderRepository, IOrderDocumentRepository orderDocumentRepository, 
        ITemplateToHtmlService htmlFileService, IBlobStorageService blobStorageService, IConfiguration config)
    {
        _mapper = mapper;
        _orderRepository = orderRepository;
        _orderDocumentRepository = orderDocumentRepository;
        _htmlFileService = htmlFileService;
        _blobStorageService = blobStorageService;
        _blobStorageService.ContainerName = config["BlobStorageContainers:DocumentsContainerName"];
        _blobStorageService.CheckContainer();
    }

    /// <summary>
    ///     Gets order info for order document as <see cref="OrderInvoice"/> object.
    /// </summary>
    /// <param name="orderId">Id of order which info to get.</param>
    /// <returns>
    ///     <see cref="OrderInvoice"/> with info of order with specified id.
    /// </returns>
    private async Task<OrderInvoice?> GetOrderInfoForDocumentAsync(int orderId)
    {
        var order = await _orderRepository.GetByIdWithOrderProductsAsNoTrackingAsync(orderId);

        if (order is null)
        {
            return null;
        }

        var invoice = _mapper.Map<OrderInvoice>(order);
        invoice.DateIssued = DateTime.Now;
        invoice.PaymentDueDate = invoice.DateIssued.AddDays(1);
        return invoice;
    }

    /// <summary>
    ///     Gets order document PDF of specified template type for order with specified id. 
    /// </summary>
    /// <param name="orderId">Id of order which invoice to get.</param>
    /// <returns>
    ///     <see cref="Stream"/> with invoice PDF for order with specified id.
    /// </returns>
    private async Task<Stream?> GetOrderDocumentPdfAsync(int orderId, TemplateFileType templateType)
    {
        var orderInfo = await GetOrderInfoForDocumentAsync(orderId);

        if (orderInfo is null)
        {
            return null;
        }

        var mappedHtmlStream = await _htmlFileService.GetHtmlFileFromTemplateAsync(orderInfo, templateType, orderInfo.OrderProducts);

        if (mappedHtmlStream is null)
        {
            return null;
        }

        return HtmlToPdfConverter.ConvertHtmlToPdf(mappedHtmlStream);
    }

    public async Task<Entities.OrderDocuments.OrderDocument?> GetOrderDocumentAsync(int orderId, OrderDocumentType documentType)
    {
        return await _orderDocumentRepository.GetAsync(orderId, documentType);
    }

    public async Task<string?> GetOrderDocumentFilePathAsync(int orderId, OrderDocumentType documentType)
    {
        return await _orderDocumentRepository.GetFilePathAsync(orderId, documentType);
    }

    public async Task<bool> UpsertAsync(int orderId, OrderDocumentType documentType)
    {
        var templateType = (TemplateFileType)((int)documentType);
        var fileStream = await GetOrderDocumentPdfAsync(orderId, templateType);
        if (fileStream is null)
        {
            return false;
        }

        var file = new FormFile(fileStream, 0, fileStream.Length, "file", "document.pdf");

        var blobResponse = await _blobStorageService.UploadAsync(file, true);
        var newFilePath = blobResponse.Blob.Uri;

        if (newFilePath is not null)
        {
            var existingOrderDocumentFilePath = await _orderDocumentRepository.GetFilePathAsync(orderId, documentType);

            if (!string.IsNullOrWhiteSpace(existingOrderDocumentFilePath))
            {
                var isUpdated = await _orderDocumentRepository.UpdateFilePathAsync(orderId, documentType, newFilePath);
                if (isUpdated)
                {
                    var uri = new Uri(existingOrderDocumentFilePath);
                    var documentFileName = uri.Segments.Last();
                    var blobDeleteResult = await _blobStorageService.DeleteAsync(documentFileName);
                    return !blobDeleteResult.Error;
                }
            }
            else
            {
                var newOrderDocument = new Entities.OrderDocuments.OrderDocument
                {
                    OrderId = orderId,
                    Type = documentType,
                    FilePath = newFilePath
                };
                var isAdded = await _orderDocumentRepository.AddAsync(newOrderDocument);
                return isAdded;
            }
        }

        return false;
    }
}
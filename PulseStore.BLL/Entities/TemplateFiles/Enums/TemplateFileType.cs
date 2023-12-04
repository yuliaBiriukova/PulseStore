namespace PulseStore.BLL.Entities.TemplateFiles.Enums;

public enum TemplateFileType
{
    InvoiceDocument = 1,
    ShipmentDocument = 2,
    OrderCreatedEmail = 3,
    InvoicePaymentEmail = 4,
    StripePaymentEmail = 5,
    ShipmentEmail = 6,
    RejectedEmail = 7,
    // TODO: add others when needed
}
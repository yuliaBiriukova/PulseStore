using PulseStore.BLL.Entities.OrderDocuments.Enums;

namespace PulseStore.BLL.Services.OrderDocument;

public interface IOrderDocumentService
{
    /// <summary>
    ///     Gets order document with specified orderId and type.
    /// </summary>
    /// <param name="orderId">Id of order which document to get.</param>
    /// <param name="type">Type of order document to get.</param>
    /// <returns>
    ///    Order document with specified orderId and type if such exists; otherwise null.
    /// </returns>
    Task<Entities.OrderDocuments.OrderDocument?> GetOrderDocumentAsync(int orderId, OrderDocumentType documentType);

    /// <summary>
    ///     Gets file path of order document with specified orderId and type. 
    /// </summary>
    /// <param name="orderId">Id of order which document to get.</param>
    /// <param name="documentType">Type of order document to get.</param>
    /// <returns>
    ///     File path of order document with specified orderId and type if such exists; otherwise null.
    /// </returns>
    Task<string?> GetOrderDocumentFilePathAsync(int orderId, OrderDocumentType documentType);

    /// <summary>
    ///     Updates order document if entity with specified orderId and type exists; otherwise adds.
    /// </summary>
    /// <param name="orderId">Id of order which document to add.</param>
    /// <param name="type">Type of order document to upsert.</param>
    /// <param name="file">File to upsert.</param>
    /// <returns>
    ///     true if order document was upserted; otherwise false.
    /// </returns>
    Task<bool> UpsertAsync(int orderId, OrderDocumentType documentType);

}
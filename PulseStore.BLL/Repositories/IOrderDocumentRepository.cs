using PulseStore.BLL.Entities.OrderDocuments;
using PulseStore.BLL.Entities.OrderDocuments.Enums;

namespace PulseStore.BLL.Repositories;

public interface IOrderDocumentRepository
{
    /// <summary>
    ///     Adds <see cref="OrderDocument"/> entity.
    /// </summary>
    /// <param name="orderDocument"><see cref="OrderDocument"/> entity to add.</param>
    /// <returns>
    ///     true if entity was added; otherwise false.
    /// </returns>
    Task<bool> AddAsync(OrderDocument orderDocument);

    /// <summary>
    ///     Checks if <see cref="OrderDocument"/> entity with specified orderId and type exists.
    /// </summary>
    /// <param name="orderId">Id of order which document to check.</param>
    /// <param name="type">Type of order document to check.</param>
    /// <returns>
    ///      true if entity with specified orderId and type exists; otherwise false.
    /// </returns>
    Task<bool> CheckExistsAsync(int orderId, OrderDocumentType type);

    /// <summary>
    ///     Gets <see cref="OrderDocument"/> entity with specified orderId and type.
    /// </summary>
    /// <param name="orderId">Id of order which document to get.</param>
    /// <param name="type">Type of order document to get.</param>
    /// <returns>
    ///     <see cref="OrderDocument"/> entity with specified orderId and type if such exists; otherwise null.
    /// </returns>
    Task<OrderDocument?> GetAsync(int orderId, OrderDocumentType type);

    /// <summary>
    ///     Gets file path of order document with specified orderId and type.
    /// </summary>
    /// <param name="orderId">Id of order which document file path to get.</param>
    /// <param name="type">Type of order document which file path to get.</param>
    /// <returns>
    ///     File path of order document with specified orderId and type if such exists; otherwise null.
    /// </returns>
    Task<string?> GetFilePathAsync(int orderId, OrderDocumentType type);

    /// <summary>
    ///     Updates file path of <see cref="OrderDocument"/> entity with specified orderId and type.
    /// </summary>
    /// <param name="orderId">Id of order which document file path to update.</param>
    /// <param name="type">Type of order document which file path to update.</param>
    /// <param name="newFilePath">New order document file path.</param>
    /// <returns>
    ///     true if file path was successfully updated; otherwise false.
    /// </returns>
    Task<bool> UpdateFilePathAsync(int orderId, OrderDocumentType type, string newFilePath);
}
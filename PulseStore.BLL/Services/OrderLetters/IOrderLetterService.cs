using LanguageExt.Common;

namespace PulseStore.BLL.Services.OrderLetters;

public interface IOrderLetterService
{
    Task<Result<bool>> SendRejectOrderLetter(int orderId);

    /// <summary>
    ///     Sends letter with order shipment data mapped by orderId.
    /// </summary>
    /// <param name="orderId">Id of order which shipment letter to send.</param>
    /// <returns>
    ///     true if email was sent; otherwise false.
    /// </returns>
    Task<Result<bool>> SendOrderShipmentLetterAsync(int orderId);

    Task<Result<bool>> SendConfirmOrderLetter(int orderId);
}
using PulseStore.PL.ViewModels.Order;

namespace PulseStore.PL.ViewModels.Payment
{
    public record PaymentDataViewModel(
        int orderId,
        int index,
        List<OrderProductListViewModel> orderProducts
    );
}
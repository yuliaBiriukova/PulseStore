using PulseStore.BLL.Models.Utils;

namespace PulseStore.PL.ViewModels.Order;

public record OrdersListViewModel (
   PaginationModel<AdminOrderViewModel> PaginationModel);
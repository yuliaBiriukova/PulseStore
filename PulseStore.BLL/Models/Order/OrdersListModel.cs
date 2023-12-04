using PulseStore.BLL.Models.Utils;

namespace PulseStore.BLL.Models.Order;

public record OrdersListModel<T>(
   PaginationModel<T> PaginationModel) where T : class;
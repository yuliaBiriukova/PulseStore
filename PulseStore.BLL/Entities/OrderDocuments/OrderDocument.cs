using PulseStore.BLL.Entities.OrderDocuments.Enums;
using PulseStore.BLL.Entities.Orders;

namespace PulseStore.BLL.Entities.OrderDocuments;

public class OrderDocument
{
    public int OrderId { get; set; }
    public Order Order { get; set; }
    public string FilePath { get; set; }
    public OrderDocumentType Type { get; set; }
}
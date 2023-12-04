using PulseStore.BLL.Entities.Security.Enums;
using PulseStore.BLL.Models.Stock;

namespace PulseStore.PL.ViewModels.Security;

public record SecurityUserViewModel(
    int Id,
    string FirstName,
    string LastName,
    SecurityUserType UserType,
    ICollection<StockDto> Stocks);

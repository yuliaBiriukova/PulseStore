namespace PulseStore.PL.ViewModels.Stock;

public record PutInStockCommandViewModel(
    int ProductId,
    StockQuantityViewModel[] stockQuantities);
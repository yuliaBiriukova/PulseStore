namespace PulseStore.PL.ViewModels.Product;

public record CartProductInfoViewModel(
     CartProductViewModel Product,
     int MaxQuantity);
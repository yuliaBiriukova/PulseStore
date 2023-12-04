using System.ComponentModel.DataAnnotations;

namespace PulseStore.PL.ViewModels.Cart;

public record AddCartProductViewModel(
    int ProductId,
    [Range(1, int.MaxValue)] int Quantity);
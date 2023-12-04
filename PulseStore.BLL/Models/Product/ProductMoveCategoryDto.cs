namespace PulseStore.BLL.Models.Product;

public record ProductMoveCategoryDto(
    int[] ProductIds,
    int CategoryId);
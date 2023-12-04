namespace PulseStore.BLL.Models.Filters;

public record PossibleFiltersExtended(
    decimal totalMinPrice,
    decimal totalMaxPrice,
    int? totalMinQuantity,
    int? totalMaxQuantity
) : PossibleFilters(totalMinPrice, totalMaxPrice);
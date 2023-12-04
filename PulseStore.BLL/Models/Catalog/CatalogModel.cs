using PulseStore.BLL.Models.Filters;
using PulseStore.BLL.Models.Utils;

namespace PulseStore.BLL.Models.Catalog;

public record CatalogModel<T>(
    PossibleFilters PossibleFilters,
    PaginationModel<T> PaginationModel);
using PulseStore.BLL.Models.Filters;
using PulseStore.BLL.Models.Utils;
using PulseStore.PL.ViewModels.Product;

namespace PulseStore.PL.ViewModels.Catalog;

public record CatalogExtendedViewModel(
    PossibleFiltersExtended PossibleFilters,
    PaginationModel<CatalogProductExtendedViewModel> PaginationModel);

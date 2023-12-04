using System.ComponentModel.DataAnnotations;
using AutoMapper;
using PulseStore.BLL.Repositories;
using PulseStore.BLL.Models.Product;
using PulseStore.BLL.Models.Utils;
using PulseStore.BLL.Models.Filters;
using PulseStore.BLL.Models.Catalog;
using PulseStore.BLL.Entities;
using PulseStore.BLL.Result;

namespace PulseStore.BLL.Services.Products;

public class ProductService : IProductService
{
    private readonly IMapper _mapper;
    private readonly IProductRepository _productRepository;
    private readonly IStockProductRepository _stockProductRepository;
    private readonly ICategoryRepository _categoryRepository;

    public ProductService(IMapper mapper, IProductRepository productRepository, 
        IStockProductRepository stockProductRepository, ICategoryRepository categoryRepository)
    {
        _mapper = mapper;
        _productRepository = productRepository;
        _stockProductRepository = stockProductRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<CatalogModel<CatalogProductDto>> GetAsync(ProductFilter filter)
    {
        var result = await _productRepository.GetAsync(filter);
        return _mapper.Map<CatalogModel<CatalogProductDto>>(result);
    }

    public async Task<CatalogModel<AdminCatalogProductDto>> GetAdminProductsSearchAsync(ProductSearchFilter filter)
    {
        var result = await _productRepository.GetProductsSearchAsync(filter);
        return _mapper.Map<CatalogModel<AdminCatalogProductDto>>(result);
    }

    public async Task<CatalogModel<CatalogProductDto>> GetProductsSearchAsync(ProductSearchFilter filter)
    {
        var result = await _productRepository.GetProductsSearchAsync(filter);
        return _mapper.Map<CatalogModel<CatalogProductDto>>(result);
    }

    public async Task<CatalogModel<CatalogProductExtendedDto>> GetExtendedAsync(ProductFilterExtended filter)
    {
        var result = await _productRepository.GetAsync(filter);
        var resultMapped = _mapper.Map<CatalogModel<CatalogProductExtendedDto>>(result);
        var productIds = resultMapped.PaginationModel.Items.Select(p => p.Id).ToList();

        Dictionary<int, int> quantities;
        if(filter.StockId is not null)
        {
            quantities = await _stockProductRepository.GetProductsQuantitiesInStockAsync(productIds, filter.StockId.Value);
        } 
        else 
        {
            quantities = await _stockProductRepository.GetProductsQuantitiesInAllStocksAsync(productIds);
        }

        foreach (var product in resultMapped.PaginationModel.Items)
        {
            if (quantities.TryGetValue(product.Id, out var quantity))
            {
                product.Quantity = quantity;
            }
        }
        return _mapper.Map<CatalogModel<CatalogProductExtendedDto>>(resultMapped);
    }

    public async Task<int> CreateAsync(Product product)
    {
        int addedProductId = await _productRepository.CreateAsync(product);
        var random = new Random();
        if (addedProductId > 0)
        {
            // todo: temporary solution for demo
            var quantity = random.Next(1, 11) * 10;
            await _stockProductRepository.PutProductInStockAsync(addedProductId, Constants.DefaultStockId, quantity);
        }
        return addedProductId != 0 ? addedProductId : 0;
    }

    public async Task<CatalogProductDto> GetByIdAsync(int productId)
    {
        var result = await _productRepository.GetByIdAsync(productId);
        return _mapper.Map<CatalogProductDto>(result);
    }

    public async Task<IEnumerable<CatalogProductDto>> GetRecentlyAddedAsync(int amount)
    {
        var result = await _productRepository.GetRecentlyAddedAsync(amount);
        return _mapper.Map<IEnumerable<CatalogProductDto>>(result);
    }

    public async Task<string> UpdateAsync(Product product)
    {
        int result = await _productRepository.UpdateAsync(product);
        return $"Product with id({product.Id}) changed";
    }

    public async Task<string> DeleteProducts(int[] ids)
    {
       return await _productRepository.DeleteAsync(ids);
    }

    public async Task<int[]> GetAllPhotoesId(int productId)
    {
        return await _productRepository.GetAllPhotoesId(productId);
    }

    public async Task<bool> ProductIsPublished(int productId)
    {
        return  await _productRepository.IsPublished(productId);
    }

    public async Task<IEnumerable<CartProductInfoDto>> GetCartProductsByIdsAsync(int[] ids)
    {
        var cartProducts = _mapper.Map<IEnumerable<CartProductDto>>(await _productRepository.GetProductsByIdsAsync(ids));
        var maxQuantities = await _stockProductRepository.GetProductsQuantitiesInAllStocksAsync(ids);

        var cartProductInfoList = cartProducts
            .Select((product, index) => new CartProductInfoDto
            {
                Product = product,
                MaxQuantity = maxQuantities.TryGetValue(product.Id, out var maxQuantity) ? maxQuantity : 0
            })
            .ToList();
        return cartProductInfoList;
    }

    public async Task<bool> PutInStockAsync(int productId, int stockId, int quantity)
    {
        return await _stockProductRepository.PutProductInStockAsync(productId, stockId, quantity);
    }

    public async Task<Product?> GetProductById(int productId)
    {
        return await _productRepository.GetByIdAsync(productId);
    }

    public async Task<Result<string, ValidationException>> MoveToCategoryAsync(ProductMoveCategoryDto productMoveCategoryDto)
    {
        var existingCategory = await _categoryRepository.GetByIdAsync(productMoveCategoryDto.CategoryId);

        if (existingCategory is null)
        {
            return new ValidationException($"There is no such category with id {productMoveCategoryDto.CategoryId}");
        }

        var products = await _productRepository.GetProductsByIdsAsync(productMoveCategoryDto.ProductIds);

        var notFoundProducts = new List<int>();
        var productsToUpdate = new List<Product>();

        foreach (var productId in productMoveCategoryDto.ProductIds)
        {
            var product = products.FirstOrDefault(p => p.Id == productId);

            if (product is null)
            {
                notFoundProducts.Add(productId);
            }
            else
            {
                product.CategoryId = productMoveCategoryDto.CategoryId;
                productsToUpdate.Add(product);
            }
        }
        
        if (notFoundProducts.Any())
        {
            return new ValidationException(
                $"The following products with these IDs don't exist: {string.Join(", ", notFoundProducts)}");
        }
        
        await _productRepository.UpdateProductsAsync(productsToUpdate);
        
        return $"All products are successfully moved to category {existingCategory.Name}";
    }

    public async Task<bool> ChangeProductPublishedStatus(int productId, bool isPublished)
    {
        return await _productRepository.ChangeProductPublishedStatus(productId, isPublished);
    }
}
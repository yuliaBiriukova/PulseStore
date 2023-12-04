using PulseStore.BLL.Entities;
using PulseStore.BLL.Models.Catalog;
using PulseStore.BLL.Models.Filters;
using PulseStore.BLL.Models.Utils;
using PulseStore.BLL.Repositories;
using PulseStore.DAL.Database;
using Microsoft.EntityFrameworkCore;

namespace PulseStore.DAL.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly PulseStoreContext _dbContext;

    public ProductRepository(PulseStoreContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CatalogModel<Product>> GetAsync(ProductFilter filter)
    {
        var query = _dbContext.Products
            .Include(p => p.Category)
            .Include(p => p.ProductPhotos)
            .Include(p => p.StockProducts)
            .AsQueryable();

        var possibleFilters = new PossibleFilters(0M, 0M);

        if (filter is ProductFilterExtended)
        {
            var possibleFiltersExtended = new PossibleFiltersExtended(0M, 0M, 0, 0);
            query = ApplyFilterExtended(filter as ProductFilterExtended, query, ref possibleFiltersExtended);
            possibleFilters = possibleFiltersExtended;
        }
        else
        {
            query = ApplyFilter(filter, query, ref possibleFilters);
        }

        if (string.Equals(filter.SortBy, Constants.ProductPriceProperty, StringComparison.OrdinalIgnoreCase))
        {
            query = string.Equals(filter.SortOrder, Constants.SortAscending, StringComparison.OrdinalIgnoreCase)
                ? query.OrderBy(p => p.Price)
                : query.OrderByDescending(p => p.Price);
        }
        else if (string.Equals(filter.SortBy, Constants.ProductQuantityProperty, StringComparison.OrdinalIgnoreCase))
        {
            query = string.Equals(filter.SortOrder, Constants.SortAscending, StringComparison.OrdinalIgnoreCase)
                ? query.OrderBy(p => p.StockProducts.Sum(s => s.Quantity))
                : query.OrderByDescending(p => p.StockProducts.Sum(s => s.Quantity));
        }
        else if(filter is ProductFilterExtended)
        {
            query = query.OrderBy(p => p.Id);
        }
        else
        {
            query = query.OrderByDescending(p => p.DateCreated);
        }

        var totalProductsCount = await query.CountAsync();

        var pageNumber = Math.Max(
            1, 
            Math.Min(
                filter.PageNumber,
                (int)Math.Ceiling((double)query.Count() / filter.PageSize)
                )
            );

        var products = await query.Skip((pageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();
        var paginationModel = new PaginationModel<Product>(pageNumber, filter.PageSize, totalProductsCount, products);
        var catalogModel = new CatalogModel<Product>(possibleFilters, paginationModel);

        return catalogModel;
    }

    public async Task<CatalogModel<Product>> GetProductsSearchAsync(ProductSearchFilter filter)
    {
        var pageNumber = filter.PageNumber;
        var pageSize = filter.PageSize;

        var query = _dbContext.Products
            .Include(p => p.Category)
            .Include(p => p.ProductPhotos)
            .AsQueryable();

        var searchTerms = filter.SearchName.Split(' ');

        foreach (var term in searchTerms)
        {
            query = query.Where(p => p.Name.Contains(term));
        }
        
        var possibleFilters = new PossibleFilters(0M, 0M);

        query = ApplyFilter(filter, query, ref possibleFilters);

        if (string.Equals(filter.SortBy, "price", StringComparison.OrdinalIgnoreCase))
        {
            query = string.Equals(filter.SortOrder, "ASC", StringComparison.OrdinalIgnoreCase)
                ? query.OrderBy(p => p.Price)
                : query.OrderByDescending(p => p.Price);
        } 
        else
        {
            query = query.OrderByDescending(p => p.DateCreated);
        }
        
        if (filter.SortBy is null && filter.SortOrder is null)
        {
            query = query
                .OrderBy(p => p.Name == filter.SearchName ? 1
                    : p.Name.StartsWith(filter.SearchName) ? 2
                    : p.Name.Contains(filter.SearchName) ? 3
                    : 4)
                .ThenBy(p => p.Name);
        }

        var totalProductsCount = await query.CountAsync();

        var products = query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);

        var paginationModel = new PaginationModel<Product>(filter.PageNumber, filter.PageSize, totalProductsCount, products);
        var result = new CatalogModel<Product>(possibleFilters, paginationModel);

        return result;
    }
    
    public async Task<int> CreateAsync(Product product)
    {
        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync();
        return product.Id;
    }

    private IQueryable<Product> ApplyFilter(ProductFilter filter, IQueryable<Product> query, ref PossibleFilters possibleFilters)
    {
        if (filter.CategoryId is not null)
        {
            query = query.Where(p => p.CategoryId == filter.CategoryId);
        }
           
        if (filter.IsPublished is not null)
        {
            query = query.Where(p => p.IsPublished == filter.IsPublished);
        }

        if (filter.Ids?.Any() == true)
        {
            query = query.Where(p => filter.Ids.Contains(p.Id));
        }

        possibleFilters = GetPossibleFilters(query);

        if (filter.MinPrice is not null)
        {
            query = query.Where(p => p.Price >= filter.MinPrice);
        }
            
        if (filter.MaxPrice is not null)
        {
            query = query.Where(p => p.Price <= filter.MaxPrice);
        }

        query = query.Where(p => p.ProductPhotos.Any());

        return query;
    }

    private IQueryable<Product> ApplyFilterExtended(ProductFilterExtended filter, IQueryable<Product> query, ref PossibleFiltersExtended possibleFiltersExtended)
    {
        if (filter.CategoryId is not null)
        {
            query = query.Where(p => p.CategoryId == filter.CategoryId);
        }

        if (filter.StockId is not null)
        {
            query = query.Where(p => p.StockProducts.Any(sp => sp.StockId == filter.StockId));
        }

        possibleFiltersExtended = GetPossibleFiltersExtended(query);

        if (filter.IsPublished is not null)
        {
            query = query.Where(p => p.IsPublished == filter.IsPublished);
        }

        if (filter.MinPrice is not null)
        {
            query = query.Where(p => p.Price >= filter.MinPrice);
        }

        if (filter.MaxPrice is not null)
        {
            query = query.Where(p => p.Price <= filter.MaxPrice);
        }

        if (filter.MinQuantity is not null)
        {
            query = query.Where(p => p.StockProducts.Sum(s => s.Quantity) >= filter.MinQuantity);
        }

        if (filter.MaxQuantity is not null)
        {
            query = query.Where(p => p.StockProducts.Sum(s => s.Quantity) <= filter.MaxQuantity);
        }
        return query;
    }

    private PossibleFilters GetPossibleFilters(IQueryable<Product> query)
    {
        var totalMinPrice = 0M;
        var totalMaxPrice = 0M;

        if (query.Any())
        {
            totalMinPrice = query.Min(p => p.Price);
            totalMaxPrice = query.Max(p => p.Price);
        }

        return new PossibleFilters(totalMinPrice, totalMaxPrice);
    }

    private PossibleFiltersExtended GetPossibleFiltersExtended(IQueryable<Product> query)
    {
        var possibleFilters = GetPossibleFilters(query);
        var totalMinQuantity = 0;
        var totalMaxQuantity = 0;

        var sortedQuery = query.OrderBy(p => p.StockProducts.Sum(s => s.Quantity));
        if(sortedQuery.Count() > 0)
        {
            totalMinQuantity = sortedQuery.FirstOrDefault().StockProducts.Sum(s => s.Quantity);
            totalMaxQuantity = sortedQuery.LastOrDefault().StockProducts.Sum(s => s.Quantity);
        }

        return new PossibleFiltersExtended(possibleFilters.totalMinPrice, possibleFilters.totalMaxPrice, totalMinQuantity, totalMaxQuantity);
    }

    public async Task<Product?> GetByIdAsync(int productId)
    {
        var product = await _dbContext.Products
            .Include(p => p.ProductPhotos)
            .FirstOrDefaultAsync(p => p.Id == productId);

        return product;
    }

    public async Task<IEnumerable<Product>> GetRecentlyAddedAsync(int amount)
    {
        return await _dbContext.Products
            .Include(p => p.ProductPhotos)
            .OrderByDescending(p => p.DateCreated)
            .Where(p => p.IsPublished && p.ProductPhotos.Any())
            .Take(amount)
            .ToListAsync();
    }

    public async Task<int> UpdateAsync(Product product)
    {
        var editProduct = await _dbContext.Products.Where(p => p.Id == product.Id).FirstAsync();
        
        foreach (var toProduct in typeof(Product).GetProperties()) {
            var fromProduct = typeof(Product).GetProperty(toProduct.Name);
            var value = fromProduct.GetValue(product, null);
            if (value != null)
            {
               toProduct.SetValue(editProduct, value, null);
            }
            
        }

        await _dbContext.SaveChangesAsync();
        return product.Id;
    }

    public async Task<string> DeleteAsync(int[] ids)
    {
        int count = 0;
        for (int i = 0; i < ids.Length; i++)
        {
            var product = await _dbContext.Products.Where(p=>p.Id == ids[i]).FirstOrDefaultAsync();
            if (product!=null && product.IsPublished!=true)
            {
                _dbContext.Products.Remove(product);
                await _dbContext.SaveChangesAsync();
                count++;
            }
        }
        if (count!=0)
        {
            
            return $"\nDeleted {count} products";
        }
        else
        {
            return $"Nothing was deleted";
        }
        
    }

    public async Task<int[]> GetAllPhotoesId(int productId)
    {
        List<int> photoIds = new List<int>();
        var product = await _dbContext.Products.Where(p => p.Id == productId).Include(p=>p.ProductPhotos).FirstOrDefaultAsync();
        if (product!=null && product.ProductPhotos !=null)
        {
            foreach(var photo in product.ProductPhotos)
            {
                var photoId = photo.Id;
                photoIds.Add(photoId);
            }
            return photoIds.ToArray();
        }
        return photoIds.ToArray();
    }

    public async Task<bool> IsPublished(int productId)
    {
        var product = await _dbContext.Products.Where(p=>p.Id ==productId).FirstOrDefaultAsync();
        return product.IsPublished;
    }

    public async Task<bool> CheckProductExistsAsync(int id)
    {
        return await _dbContext.Products.AnyAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Product>> GetProductsByIdsAsync(int[] ids)
    {
        return await _dbContext.Products
            .Include(p => p.ProductPhotos)
            .Where(p => ids.Contains(p.Id))
            .ToListAsync();
    }

    public async Task UpdateProductsAsync(List<Product> products)
    {
        _dbContext.UpdateRange(products);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> ChangeProductPublishedStatus(int productId, bool isPublished)
    {
        var product = _dbContext.Products.FirstOrDefault(p => p.Id == productId);
        if(product != null)
        {
            product.IsPublished = isPublished;
            await _dbContext.SaveChangesAsync();
            return true;
        }
        else
        {
            return false;
        }
    }
}
using AutoMapper;
using PulseStore.BLL.Entities;
using PulseStore.BLL.Models.Cart;
using PulseStore.BLL.Repositories;
using LanguageExt.Common;
using System.ComponentModel.DataAnnotations;

namespace PulseStore.BLL.Services.UserCartProducts;

public class CartService : ICartService
{
    private readonly IMapper _mapper;
    private readonly IUserCartProductRepository _userCartProductRepository;
    private readonly IStockProductRepository _stockProductRepository;
    private readonly IProductRepository _productRepository;

    public CartService(IMapper mapper, IUserCartProductRepository userCartProductRepository, 
        IStockProductRepository stockProductRepository, IProductRepository productRepository)
    {
        _mapper = mapper;
        _userCartProductRepository = userCartProductRepository;
        _stockProductRepository = stockProductRepository;
        _productRepository = productRepository;
    }

    public async Task<Result<int>> UpsertAsync(AddUserCartProductDto userCartProduct, Guid userId)
    {
        var error = await ValidateAddUserCartProductAsync(userCartProduct);
        if (error is not null)
        {
            return new Result<int>(error);
        }

        var existingUserCartProduct = await _userCartProductRepository.GetByProductIdAsync(userCartProduct.ProductId, userId);
        if(existingUserCartProduct is not null) 
        {
            existingUserCartProduct.Quantity = userCartProduct.Quantity;
            var affectedRows = await _userCartProductRepository.UpdateAsync(existingUserCartProduct);
            return existingUserCartProduct.Id;
        }

        userCartProduct.UserId = userId;
        var newUserCartProduct = _mapper.Map<UserCartProduct>(userCartProduct);
        return await _userCartProductRepository.AddAsync(newUserCartProduct);
    }

    public async Task<Result<IEnumerable<int>>> AddManyAsync(IEnumerable<AddUserCartProductDto> userCartProducts, Guid userId)
    {
        var addedItemsIds = new List<int>();

        if (!userCartProducts.Any())
        {
            return addedItemsIds;
        }

        foreach (var userCartProduct in userCartProducts)
        {
            var error = await ValidateAddUserCartProductAsync(userCartProduct);
            if(error is not null)
            {
                return new Result<IEnumerable<int>>(error);
            }
        }

        var newUserCartProducts = _mapper.Map<IEnumerable<UserCartProduct>>(userCartProducts);
        await _userCartProductRepository.DeleteAllAsync(userId);
        
        foreach (var newUserCartProduct in newUserCartProducts)
        {
            var userCartProduct = await _userCartProductRepository.GetByProductIdAsync(newUserCartProduct.ProductId, userId);
            if (userCartProduct is null)
            {
                newUserCartProduct.UserId = userId;
                addedItemsIds.Add(await _userCartProductRepository.AddAsync(newUserCartProduct));
            }
        }

        return addedItemsIds;
    }

    public async Task<int> DeleteAsync(int productId, Guid userId)
    {
        return await _userCartProductRepository.DeleteAsync(productId, userId);
    }

    public async Task DeleteAllItemsAsync(Guid userId)
    {
        await _userCartProductRepository.DeleteAllAsync(userId);
    }

    public async Task<IEnumerable<UserCartProductDto>> GetAsync(Guid userId)
    {
        var userCartProducts = await _userCartProductRepository.GetAsync(userId);
        var userCartProductsDtos = _mapper.Map<IEnumerable<UserCartProductDto>>(userCartProducts);
        var productIds = userCartProducts.Select(item => item.ProductId).ToList();
        var maxQuantities = await _stockProductRepository.GetProductsQuantitiesInAllStocksAsync(productIds);

        userCartProductsDtos = userCartProductsDtos
             .Select(cartProduct => {
                 if (maxQuantities.TryGetValue(cartProduct.ProductId, out var maxQuantity))
                 {
                     cartProduct.MaxQuantity = maxQuantity;
                 }
                 return cartProduct;
             })
             .ToList();

        return userCartProductsDtos;
    }

    public async Task<Result<int>> UpdateAsync(UpdateUserCartProductDto userCartProduct, Guid userId)
    {
        userCartProduct.UserId = userId;
        var error = await ValidateUpdateUserCartProductAsync(userCartProduct);
        if (error is not null)
        {
            return new Result<int>(error);
        }
        var updatedUserCartProduct = _mapper.Map<UserCartProduct>(userCartProduct);
        return await _userCartProductRepository.UpdateAsync(updatedUserCartProduct);
    }

    private async Task<bool> CheckProductQuantityAsync(int productId, int quantity)
    {
        var maxQuantity = await _stockProductRepository.GetProductQuantityAsync(productId);
        return quantity <= maxQuantity;
    }

    private async Task<ValidationException?> ValidateAddUserCartProductAsync(AddUserCartProductDto cartProduct)
    {
        if (!await _productRepository.CheckProductExistsAsync(cartProduct.ProductId))
        {
            return new ValidationException($"Product with id={cartProduct.ProductId} does not exist.");
        }

        return await ValidateProductQuantityAsync(cartProduct.ProductId, cartProduct.Quantity);
    }

    private async Task<ValidationException?> ValidateProductQuantityAsync(int productId, int quantity)
    {
        if (!await CheckProductQuantityAsync(productId, quantity))
        {
            return new ValidationException($"Product quantity is invalid in entity with productId={productId}. " +
                $"Quantity must be less or equal to quantity in stocks.");
        }

        return null;
    }

    private async Task<ValidationException?> ValidateUpdateUserCartProductAsync(UpdateUserCartProductDto cartProduct)
    {
        if (!await _userCartProductRepository.CheckUserCartProductExistsAsync(cartProduct.Id, cartProduct.ProductId, cartProduct.UserId))
        {
            return new ValidationException($"UserCartProduct with id={cartProduct.Id} and productId={cartProduct.ProductId} does not exist for current user.");
        }

        return await ValidateProductQuantityAsync(cartProduct.ProductId, cartProduct.Quantity);
    }
}
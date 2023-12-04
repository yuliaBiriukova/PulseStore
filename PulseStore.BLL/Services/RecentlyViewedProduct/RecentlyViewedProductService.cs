using AutoMapper;
using PulseStore.BLL.Entities;
using PulseStore.BLL.Models.Product.UserProductView;
using PulseStore.BLL.Repositories;
using LanguageExt.Common;
using System.ComponentModel.DataAnnotations;

namespace PulseStore.BLL.Services.RecentlyViewedProduct;

public class RecentlyViewedProductService : IRecentlyViewedProductService
{
    private readonly IMapper _mapper;
    private readonly IProductRepository _productRepository;
    private readonly IUserProductViewRepository _userProductViewRepository;

    public RecentlyViewedProductService(IMapper mapper, IProductRepository productRepository, IUserProductViewRepository userProductViewRepository)
    {
        _mapper = mapper;
        _productRepository = productRepository;
        _userProductViewRepository = userProductViewRepository;
    }

    public async Task<bool> CheckProductExists(AddUserProductViewDto userProductView)
    {
        return await _productRepository.CheckProductExistsAsync(userProductView.ProductId);
    }

    public async Task<IEnumerable<UserProductViewDto>> GetByUserIdAsync(Guid userId)
    {
        var result = await _userProductViewRepository.GetUserProductViewsByUserIdAsync(userId);
        return _mapper.Map<IEnumerable<UserProductViewDto>>(result);
    }

    public async Task<Result<bool>> UpsertAsync(Guid userId, int productId)
    {
        var error = await ValidateRecentlyViewedProductAsync(productId);
        if (error is not null)
        {
            return new Result<bool>(error);
        }

        var userProductViewExists = await _userProductViewRepository.CheckUserProductViewExistsAsync(userId, productId);
        if (userProductViewExists)
        {
            return await _userProductViewRepository.UpdateUserProductViewAsync(userId, productId, DateTime.UtcNow);
        }

        UserProductViewDto userProductViewDto = new UserProductViewDto(userId, productId, DateTime.UtcNow);
        var newUserProductView = _mapper.Map<UserProductView>(userProductViewDto);
        var isAdded = await _userProductViewRepository.AddUserProductViewAsync(newUserProductView);

        if (!isAdded)
        {
            return isAdded;
        }

        await ClearExtraUserProductViews(userId);

        return isAdded;
    }

    public async Task<Result<bool>> UpsertAsync(Guid userId, AddUserProductViewDto userProductView)
    {
        var error = await ValidateRecentlyViewedProductAsync(userProductView.ProductId);
        if (error is not null)
        {
            return new Result<bool>(error);
        }

        var userProductViewExists = await _userProductViewRepository.CheckUserProductViewExistsAsync(userId, userProductView.ProductId);
        if (userProductViewExists)
        {
            return await _userProductViewRepository.UpdateUserProductViewAsync(userId, userProductView.ProductId, userProductView.ViewedAt);
        }

        var newUserProductView = _mapper.Map<UserProductView>(userProductView);
        newUserProductView.UserId = userId;

        var isAdded = await _userProductViewRepository.AddUserProductViewAsync(newUserProductView);

        if (!isAdded)
        {
            return isAdded;
        }

        return isAdded;
    }

    public async Task<bool> UpsertManyAsync(IEnumerable<AddUserProductViewDto> userProductViews, Guid userId)
    {
        foreach (var userProductView in userProductViews)
        {
            var isAdded = await UpsertAsync(userId, userProductView);
        }
        await ClearExtraUserProductViews(userId);
        return true;
    }

    public async Task ClearExtraUserProductViews(Guid userId)
    {
        var userProductViews = await _userProductViewRepository.GetUserProductViewsByUserIdAsync(userId);
        var maxItemsCount = 15;
        if (userProductViews.Count() > maxItemsCount)
        {
            var extraProductIds = userProductViews.Skip(15).Select(item => item.ProductId);
            var isDeleted = await _userProductViewRepository.DeleteManyAsync(userId, extraProductIds);
        }
    }

    private async Task<ValidationException?> ValidateRecentlyViewedProductAsync(int productId)
    {
        if (!await _productRepository.CheckProductExistsAsync(productId))
        {
            return new ValidationException($"Product with id={productId} does not exist.");
        }

        return null;
    }
}
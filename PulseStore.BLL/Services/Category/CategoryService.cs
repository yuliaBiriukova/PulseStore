using System.ComponentModel.DataAnnotations;
using AutoMapper;
using PulseStore.BLL.Models.Category;
using PulseStore.BLL.Models.Product;
using PulseStore.BLL.Repositories;
using PulseStore.BLL.Result;

namespace PulseStore.BLL.Services.Category;

public class CategoryService : ICategoryService
{
    private readonly IMapper _mapper;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IPhotoRepository _photoRepository;

    public CategoryService(IMapper mapper, ICategoryRepository categoryRepository, IPhotoRepository photoRepository)
    {
        _mapper = mapper;
        _categoryRepository = categoryRepository;
        _photoRepository = photoRepository;
    }

    public async Task<CategoryDto> GetByIdAsync(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        return _mapper.Map<CategoryDto>(category);
    }

    public async Task<IEnumerable<CatalogCategoryDto>> GetAllAsync()
    {
        var categories = await _categoryRepository.GetAllAsync();
        var categoryDtos = _mapper.Map<IEnumerable<CatalogCategoryDto>>(categories);
        var categoriesIds = categoryDtos.Select(c => c.Id);
        var categoriesImages = await _photoRepository.GetImagePathsByCategoryIdsAsync(categoriesIds);
        categoryDtos = categoryDtos
            .Select((category, index) => new CatalogCategoryDto(category.Id, category.Name)
            {
                ImagePath = categoriesImages.TryGetValue(category.Id, out var imagePath) ? imagePath : null
            });
        return categoryDtos;
    }

    public async Task<Result<int, string>> CreateAsync(Entities.Category category)
    {
        if (string.IsNullOrEmpty(category.Name))
        {
            return $"Category name can't be empty!";
        }
        var existingCategory = await _categoryRepository.GetCategoryByName(category.Name);

        if (existingCategory is not null)
        {
            return $"The category with name {category.Name} already exists!";
        }
        
        int createdCategoryId = await _categoryRepository.CreateAsync(category);
        return createdCategoryId;
    }

    public async Task<Result<string, ValidationException>> EditAsync(int id, Entities.Category category)
    {
        if (string.IsNullOrEmpty(category.Name))
        {
            return new ValidationException($"Category name can't be empty!"); 
        }

        var existingCategory = await _categoryRepository.GetCategoryByName(category.Name);
        if (existingCategory is not null)
        {
            return new ValidationException($"The category with name {category.Name} already exists!");
        }

        var categoryToChange = await _categoryRepository.GetByIdAsync(id);
        if (categoryToChange is null)
        {
            return new ValidationException($"There is no category with id {id}");
        }
        
        var oldCategoryName = categoryToChange.Name;
        
        categoryToChange.Name = category.Name;

        await _categoryRepository.EditAsync(categoryToChange);

        return oldCategoryName;
    }

    public async Task<IEnumerable<CategoryExtendedDto>> GetAllExtendedAsync()
    {
        var categories = await _categoryRepository.GetAllExtendedAsync();
        var result = _mapper.Map<List<CategoryExtendedDto>>(categories);
        return result;
    }

    public async Task<bool> DeleteCategoriesByIdsAsync(int[] ids)
    {
        return await _categoryRepository.DeleteCategoriesByIdsAsync(ids);
    }
}
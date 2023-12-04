using AutoMapper;
using PulseStore.BLL.Services.Category;
using PulseStore.PL.ViewModels.Category;
using Microsoft.AspNetCore.Mvc;

namespace PulseStore.PL.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ICategoryService _categoryService;

    public CategoriesController(IMapper mapper, ICategoryService categoryService)
    {
        _mapper = mapper;
        _categoryService = categoryService;
    }

    /// <summary>
    ///     Gets all <see cref="CatalogCategoryViewModel"/> entities.
    /// </summary>
    /// <returns>
    ///     List of <see cref="CatalogCategoryViewModel"/> entities.
    /// </returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CatalogCategoryViewModel>>> GetAll()
    {
        var categories = await _categoryService.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<CatalogCategoryViewModel>>(categories));
    }
}
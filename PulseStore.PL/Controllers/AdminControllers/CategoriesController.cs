using AutoMapper;
using PulseStore.BLL.Entities;
using PulseStore.BLL.Services.Category;
using PulseStore.PL.ViewModels.Category;
using Microsoft.AspNetCore.Mvc;

namespace PulseStore.PL.Controllers.AdminControllers
{
    [Route("api/admin/[controller]")]
    [ApiExplorerSettings(GroupName = "Admin/Categories")]
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
        ///     Gets the <see cref="CategoryViewModel"/> entity by its ID.
        /// </summary>
        /// <param name="id">The ID of the category.</param>
        /// <returns>
        ///     The <see cref="CategoryViewModel"/> entity.
        /// </returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryViewModel>> GetCategoryById(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<CategoryViewModel>(category));
        }

        /// <summary>
        ///     Gets all <see cref="CategoryExtendedViewModel"/> entities.
        /// </summary>
        /// <returns>
        ///     List of <see cref="CategoryExtendedViewModel"/> entities.
        /// </returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryViewModel>>> GetCategories()
        {
            var categories = await _categoryService.GetAllExtendedAsync();
            return Ok(_mapper.Map<IEnumerable<CategoryExtendedViewModel>>(categories));
        }
        
        [HttpPost]
        public async Task<ActionResult<string>> CreateCategory([FromBody] CategoryCreateViewModel category)
        {
            var categoryToCreate = _mapper.Map<Category>(category);
            var createdCategory = await _categoryService.CreateAsync(categoryToCreate);
            
            return createdCategory.Match<ActionResult>(
                responseCategoryId => Ok($"Added category {category.Name} with id {responseCategoryId}"),
                error => BadRequest(error)
            );
        }
        
        [HttpPut("{id}")] 
        public async Task<ActionResult<string>> EditCategory(int id, [FromBody] CategoryCreateViewModel category)
        {
            var categoryToEdit = _mapper.Map<Category>(category);
            var editedCategory = await _categoryService.EditAsync(id, categoryToEdit);
            
            return editedCategory.Match<ActionResult>(
                responseCategoryName => Ok($"Successfully changed old category name {responseCategoryName} " +
                                           $"with {category.Name} for category with id {id} "),
                error => BadRequest(error)
            );
        }

        /// <summary>
        ///     Deletes categories by ids.
        /// </summary>
        /// <param name="ids">category ids to delete.</param>
        /// <returns>
        ///     HTTP 200 OK with message if categories have been deleted
        /// </returns>
        [HttpDelete]
        public async Task<ActionResult> DeleteCategories(int[] ids)
        {
            bool categoriesAreDeleted = await _categoryService.DeleteCategoriesByIdsAsync(ids);
            return categoriesAreDeleted ?
                Ok("Categories have been deleted") :
                BadRequest("Categories haven't been deleted");
        }
    }
}
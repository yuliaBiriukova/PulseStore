using AutoMapper;
using PulseStore.BLL.Entities;
using PulseStore.BLL.Models.Filters;
using PulseStore.BLL.Services.Photo;
using PulseStore.BLL.Services.Products;
using PulseStore.PL.ViewModels.Catalog;
using PulseStore.PL.ViewModels.Filters;
using PulseStore.PL.ViewModels.Product;
using PulseStore.PL.ViewModels.Stock;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PulseStore.BLL.Models.Product;

namespace PulseStore.PL.Controllers.AdminControllers
{
    [Route("api/admin/[controller]")]
    [ApiExplorerSettings(GroupName = "Admin/Products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IProductService _productService;
        private readonly IPhotoService _photoService;

        public ProductsController(IMapper mapper, IProductService productService, IPhotoService photoService)
        {
            _mapper = mapper;
            _productService = productService;
            _photoService = photoService;
        }

        /// <summary>
        ///     Gets <see cref="CatalogExtendedViewModel"/> entity by conditions in <see cref="ProductFilterExtendedViewModel"/> with pagination and filters for administrator.
        /// </summary>
        /// <param name = "filter" ><see cref="ProductFilterExtendedViewModel"/> entity with conditions for product</param>
        /// <returns>
        ///     <see cref="CatalogExtendedViewModel"/> entity with products with pagination that satisfy the filter and possible filters.
        /// </returns>
        [HttpGet]
        public async Task<ActionResult<CatalogExtendedViewModel>> GetProducts([FromQuery] ProductFilterExtendedViewModel filterModel)
        {
            var filter = _mapper.Map<ProductFilterExtended>(filterModel);
            var result = await _productService.GetExtendedAsync(filter);
            return Ok(_mapper.Map<CatalogExtendedViewModel>(result));
        }

        /// <summary>
        ///     Gets <see cref="AdminCatalogViewModel"/> entity by conditions in <see cref="ProductSearchFilterViewModel"/> with pagination and filters for administrator.
        /// </summary>
        /// <param name = "filterModel" ><see cref="ProductSearchFilterViewModel"/> entity with conditions for administrator product search.</param>
        /// <returns>
        ///     <see cref="AdminCatalogViewModel"/> entity with products with pagination that satisfy the filter and possible filters.
        /// </returns>
        [HttpGet("search")]
        public async Task<ActionResult<AdminCatalogViewModel>> GetAdminSearchProducts(
            [FromQuery] ProductSearchFilterViewModel filterModel)
        {
            var filter = _mapper.Map<ProductSearchFilter>(filterModel);
            var result = await _productService.GetAdminProductsSearchAsync(filter);
            return Ok(_mapper.Map<AdminCatalogViewModel>(result));
        }

        [HttpPost]
        public async Task<ActionResult<string>> CreateProduct([FromForm] CreateProductViewModel productModel)
        {
            var product = _mapper.Map<Product>(productModel);
            var productId = await _productService.CreateAsync(product);
            List<int> photoIds = new List<int>();
            if (productModel.Files != null)
            {
                photoIds = await _photoService.UploadAsync(productModel.Files, productId);
                string resultStrings = "";
                foreach (var photoId in photoIds)
                {
                    resultStrings += $"{photoId}, ";
                }
                return Ok($"Added product id: {productId} \n Added photo ids: {resultStrings}");
            }

            return Ok($"Added product id: {productId}");
        }

        [HttpPost("duplicate")]
        public async Task<ActionResult<string>> DuplicateProduct([FromForm] DuplicateProductViewModel productModel)
        {
            var product = _mapper.Map<Product>(productModel);
            var productId = await _productService.CreateAsync(product);
            List<int> photoIds = new List<int>();
            string resultStrings = "";
            if (productModel.PhotosUrls != null)
            {
                photoIds = await _photoService.UploadAsync(productModel.PhotosUrls, productId);
                
                foreach (var photoId in photoIds)
                {
                    resultStrings += $"{photoId}, ";
                }
            }
            if (productModel.NewPhotos != null)
            {
                photoIds = await _photoService.UploadAsync(productModel.NewPhotos, productId);
                foreach (var photoId in photoIds)
                {
                    resultStrings += $"{photoId}, ";
                }

            }
            if (resultStrings.IsNullOrEmpty())
            {
                return Ok($"Added product id: {productId}"); 
            }

            return Ok($"Added product id: {productId} \n Added photo ids: {resultStrings}");
        }

        [HttpPut]
        public async Task<ActionResult<string>> EditProduct([FromForm] EditProductViewModel editProduct)
        {
            var product = _mapper.Map<Product>(editProduct);
            string editResult=await _productService.UpdateAsync(product);
            string deletePhotoesResult = "";
            if (editProduct.PhotoesDeleteId != null)
            {
                deletePhotoesResult = await _photoService.DeletePhotoesAsync(editProduct.PhotoesDeleteId);
            }
            if (editProduct.PhotoesToAdd!=null)
            {
                await _photoService.UploadAsync(editProduct.PhotoesToAdd, product.Id);
            }
            
            return Ok($"{editResult}\n{deletePhotoesResult}");
        }

        [HttpPatch]
        public async Task<ActionResult<string>> MoveToCategory(
            [FromBody] ProductMoveCategoryViewModel productMoveCategory)
        { 
            var productsToMove = _mapper.Map<ProductMoveCategoryDto>(productMoveCategory);
            var movedProducts = await _productService.MoveToCategoryAsync(productsToMove);
            
            return movedProducts.Match<ActionResult>(
                responseMessage => Ok(responseMessage),
                error => BadRequest(error.Message)
            );
        }

        [HttpDelete]
        public async Task<ActionResult<string>> DeleteProduct(int[] productIds) {
            string result="";
            for (int i = 0; i < productIds.Length; i++)
            {
                if (await _productService.ProductIsPublished(productIds[i]) != true)
                {
                    int[] photoesIds = await _productService.GetAllPhotoesId(productIds[i]);
                    if (photoesIds.IsNullOrEmpty())
                    {
                        result += "No photo of product was deleted\n";
                    }
                    else
                    {
                        result += await _photoService.DeletePhotoesAsync(photoesIds);
                    } 
                }
            }
            result += await _productService.DeleteProducts(productIds);

            return Ok($"{result}\n");
        }

        [HttpGet("product")]
        public async Task<ActionResult<ProductForEditViewModel>> GetProduct([FromQuery] int id)
        {
            var result = await _productService.GetProductById(id);
            return Ok(_mapper.Map<ProductForEditViewModel>(result));
        }

        /// <summary>
        ///     Add some quantity of product on one of existing stocks.
        /// </summary>
        /// <param name="command" ><see cref="PutInStockCommandViewModel"/> commant to put product in stock</param>
        [HttpPut("stock")]
        public async Task<ActionResult<string>> PutProductsInStock([FromBody] PutInStockCommandViewModel command)
        {
            var result = true;
            foreach(var stockQuantity in command.stockQuantities)
            {
                result = await _productService.PutInStockAsync(command.ProductId, stockQuantity.StockId, stockQuantity.Quantity) && result;
            }
            return result ? Ok("Put in stock success") : BadRequest("Put in stock error");
        }
        /// <summary>
        ///     Change published status of a specific product
        /// </summary>
        /// <param name="command" ><see cref="PutInStockCommandViewModel"/> commant to put product in stock</param>
        [HttpPatch("status")]
        public async Task<ActionResult<string>> ChangeProductPublishedStatus([FromBody] ChangeProductStatusCommandViewModel command)
        {
            var result = await _productService.ChangeProductPublishedStatus(command.ProductId, command.IsPublished);
            return result ? 
                Ok("Product status changed successfully") :
                BadRequest("Error while changing product status");
        }
    }
}
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using GameShopStore.Application.Helpers;
using GameShopStore.Application.Interfaces;
using GameShopStore.Core.Dtos.CategoryDtos;
using GameShopStore.Core.Dtos.LanguageDtos;
using GameShopStore.Core.Dtos.ProductDtos;
using GameShopStore.Core.Dtos.SubCategoryDtos;
using GameShopStore.Core.Dtos.UserDtos;
using GameShopStore.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace GameShopStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private Cloudinary _cloudinary;
        private readonly IUnitOfWork _unitOfWork;

        public AdminController(UserManager<User> userManager, IMapper mapper,
             IOptions<CloudinarySettings> cloudinaryConfig, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _cloudinaryConfig = cloudinaryConfig;
            _mapper = mapper;
            _userManager = userManager;


            Account acc = new Account(_cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(acc);

        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("users-with-roles")]
        public async Task<IActionResult> GetUsersWithRoles()
        {
            var userList = await _unitOfWork.User.GetAllWithRolesAsync();

            if (userList == null)
            {
                return NotFound();
            }

            return Ok(userList);
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("edit-roles/{userName}")]
        public async Task<IActionResult> EditRoles(string userName, EditingRoleDto roleEditDto)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null)
            {
                return BadRequest("User not found");
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            var selectedRoles = roleEditDto?.RoleNames;

            selectedRoles = selectedRoles ?? new string[] { };

            var result = await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));
            if (!result.Succeeded)
            {
                return BadRequest("Adding roles to the specified user has failed");
            }

            result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));
            if (!result.Succeeded)
            {
                return BadRequest("Removing roles to the specified user has failed");
            }

            return Ok(await _userManager.GetRolesAsync(user));
        }

        [Authorize(Policy = "ModerateProductRole")]
        [HttpGet("prodcuts-for-moderation")]
        public async Task<IActionResult> GetProductsForModeration([FromQuery] ProductParameters productParameters)
        {
            if (productParameters.PageNumber < 1 || productParameters.PageSize < 1)
            {
                return BadRequest("Than PageNumber or PageSize is less than 1");
            }

            var products = await _unitOfWork.Product.GetProductsForModerationAsync(productParameters);

            if (products == null)
            {
                return NotFound();
            }
            else if (products.PageSize < 1 || products.TotalCount < 0 || products.TotalPages < 1 || products.CurrentPage < 1)
            {
                return BadRequest("Pagination parameters wasn't set properly");
            }

            var productsToReturn = _mapper.Map<IEnumerable<ProductModerationDto>>(products);

            Response.AddPagination(products.CurrentPage, products.PageSize, products.TotalCount, products.TotalPages);

            return Ok(productsToReturn);
        }

        [Authorize(Policy = "ModerateProductRole")]
        [HttpGet("prodcuts-for-stock-moderation")]
        public async Task<IActionResult> GetProductsForStockModeration([FromQuery] ProductParameters productParams)
        {
            if (productParams.PageNumber < 1 || productParams.PageSize < 1)
            {
                return BadRequest("The PageNumber or PageSize is less than 1");
            }

            var products = await _unitOfWork.Product.GetProductsForStockModeration(productParams);

            if (products == null)
            {
                return NotFound();
            }
            else if (products.PageSize < 1 || products.TotalCount < 0 || products.TotalPages < 1 || products.CurrentPage < 1)
            {
                return BadRequest("Pagination parameters wasn't set properly");
            }

            var productsToReturn = _mapper.Map<IEnumerable<ProductStockModerationDto>>(products);

            Response.AddPagination(products.CurrentPage, products.PageSize, products.TotalCount, products.TotalPages);

            return Ok(productsToReturn);
        }

        [Authorize(Policy = "ModerateProductRole")]
        [HttpGet("product-for-edit/{id}")]
        public async Task<IActionResult> GetProductForEdit(int id)
        {
            var requirements = await _unitOfWork.Requirements.GetRequirementsForProductAsync(id);

            if (requirements == null)
            {
                return BadRequest("Error occured during retrieving requirements for selected product");
            }

            var photosFromRepo = await _unitOfWork.Picture.GetPicturesForProduct(id);

            if (photosFromRepo == null)
            {
                return BadRequest("Error occured during retrieving photos for selected product");
            }

            var productToEdit = await _unitOfWork.Product.GetProductToEditAsync(requirements, photosFromRepo, id);

            if (productToEdit == null)
            {
                return BadRequest("Error occured during retrieving product");
            }


            return Ok(productToEdit);
        }

        [Authorize(Policy = "ModerateProductRole")]
        [HttpPost("create-product")]
        public async Task<IActionResult> CreateProduct(ProductCreationDto productForCreationDto)
        {

            if (productForCreationDto == null)
            {
                return BadRequest("The sended product was null");
            }

            if (productForCreationDto.CategoryId < 1)
            {
                return BadRequest("Category wasn't passed or passed bad CategoryId");
            }

            var selectedCategory = await _unitOfWork.Category.GetAsync(productForCreationDto.CategoryId);

            var requirementsForProduct = _mapper.Map<Requirements>(productForCreationDto.Requirements);

            var productToCreate = await _unitOfWork.Product.ScaffoldProductForCreationAsync(productForCreationDto, requirementsForProduct, selectedCategory);

            if (productToCreate == null)
            {
                return BadRequest("Something went wrong during creating Product");
            }

            _unitOfWork.Product.Add(productToCreate);

            if (await _unitOfWork.SaveAsync())
            {
                return CreatedAtRoute("GetProduct", new { id = productToCreate.Id }, productToCreate);

            }

            return BadRequest("Error occured during Saving Product");

        }


        [Authorize(Policy = "ModerateProductRole")]
        [HttpPost("edit-product/{id}")]
        public async Task<IActionResult> EditProduct(int id, ProductEditDto productToEditDto)
        {
            if (productToEditDto == null)
            {
                return BadRequest("The sended product was null");
            }

            var productFromDb = await _unitOfWork.Product.GetAsync(id);

            if (productFromDb == null)
            {
                return BadRequest($"Product with Id:{id} not found");
            }

            var selectedCategory = await _unitOfWork.Category.FindAsync(c => c.Id == productToEditDto.CategoryId);


            var requirementsToUpdate = _mapper.Map<Requirements>(productToEditDto.Requirements);

            var updatedProduct = await _unitOfWork.Product.ScaffoldProductForEditAsync(id, productToEditDto, requirementsToUpdate, selectedCategory, productFromDb);

            productFromDb = _mapper.Map(updatedProduct, productFromDb);

            if (await _unitOfWork.SaveAsync())
            {
                var editedProduct = await _unitOfWork.Product.GetAsync(id);
                return CreatedAtRoute("GetProduct", new { id = id }, editedProduct);
            }

            return BadRequest("Fail occured during editing Product");
        }

        [Authorize(Policy = "ModerateProductRole")]
        [HttpDelete("delete-product/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {

            var selectedProduct = await _unitOfWork.Product.GetAsync(id);

            if (selectedProduct == null)
            {
                return BadRequest("There's no Id for the product");
            }

            var photos = selectedProduct.Pictures?.ToList();

            if (photos != null && photos.Select(p => p.PublicId).Any())
            {

                foreach (var photo in photos)
                {
                    if (photo.PublicId != null)
                    {
                        var deleteParams = new DeletionParams(photo.PublicId);

                        var result = _cloudinary.Destroy(deleteParams);

                        if (result.Result == "ok")
                        {
                            _unitOfWork.Picture.Delete(photo);
                        }
                    }
                    else
                    {
                        _unitOfWork.Picture.Delete(photo);
                    }
                }

            }
            _unitOfWork.Product.Delete(selectedProduct);


            if (!await _unitOfWork.SaveAsync())
            {
                return BadRequest("Something went wrong during deleting product");
            }
            return NoContent();
        }

        [Authorize(Policy = "ModerateProductRole")]
        [HttpPost("edit-product/{id}/stock-quantity/{quantity}")]
        public async Task<IActionResult> EditStockForProduct(int id, int quantity)
        {

            if (quantity < 0)
            {
                return BadRequest("Quantity value to can't be less than 0 when set");
            }

            var product = await _unitOfWork.Product.GetWithStockOnlyAsync(id);

            if (product == null)
            {
                return BadRequest("The Id for the product doesn't exist");
            }

            if (product.Stock?.Quantity == quantity)
            {
                return BadRequest("Passed same quantity as product already have");
            }

            if (product.Stock != null)
            {
                product.Stock.Quantity = quantity;
            }
            else
            {
                product.Stock = new Stock()
                {
                    Quantity = quantity,
                    ProductId = id
                };
            }

            if (await _unitOfWork.SaveAsync())
            {
                return Ok(await _unitOfWork.Stock.GetByProductId(id));
            }

            return BadRequest("Something went wrong during saving Stock for Product");

        }


        [Authorize(Policy = "ModerateProductRole")]
        [HttpGet("available-categories")]
        public async Task<IActionResult> GetCategories()
        {

            var categoriesList = await _unitOfWork.Category.GetAllOrderedByAsync(x => x.Id);

            if (categoriesList == null || !categoriesList.Any())
            {
                return NotFound();
            }

            var categoriesToRetrun = _mapper.Map<IEnumerable<CategoryToReturnDto>>(categoriesList);

            return Ok(categoriesToRetrun);
        }

        [Authorize(Policy = "ModerateProductRole")]
        [HttpGet("available-subCategories")]
        public async Task<IActionResult> GetSubCategories()
        {
            var subCategoriesList = await _unitOfWork.SubCategory.GetAllAsync();

            if (subCategoriesList == null || !subCategoriesList.Any())
            {
                return NotFound();
            }

            var subCategoryToRetrun = _mapper.Map<IEnumerable<SubCategoryReturnDto>>(subCategoriesList);

            return Ok(subCategoryToRetrun);
        }

        [Authorize(Policy = "ModerateProductRole")]
        [HttpGet("available-languages")]
        public async Task<IActionResult> GetLanguages()
        {

            var languagesList = await _unitOfWork.Language.GetAllOrderedByAsync(x => x.Id);

            if (languagesList == null || !languagesList.Any())
            {
                return NotFound();
            }

            var languagesListToReturn = _mapper.Map<IEnumerable<ReturnLanguagesDto>>(languagesList);

            return Ok(languagesListToReturn);
        }

        [Authorize(Policy = "ModerateProductRole")]
        [HttpGet("category-test/{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category = await _unitOfWork.Category.GetAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            var categoryToReturn = _mapper.Map<CategoryToReturnDto>(category);

            return Ok(categoryToReturn);

        }
    }
}

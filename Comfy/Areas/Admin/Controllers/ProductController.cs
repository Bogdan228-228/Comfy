using BLL.Interfaces;
using Comfy.ViewModels;
using DOMAIN;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Comfy.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ProductController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        public IActionResult Index()
        {
            var products = _productService.GetAllProducts();
            return View(products);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var categories = _categoryService.GetAllCategories()
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                });

            ViewBag.Categories = categories;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var product = new Product
                { 
                    Name = model.Name,
                    Description = model.Description,
                    Price = model.Price,
                    Quantity = model.Quantity,
                    CategoryId = model.CategoryId,
                    ImageUrl = model.ImageUrl ?? ""
                };

                await _productService.AddProductAsync(product);
                return RedirectToAction("Index");
            }

            ViewBag.Categories = _categoryService.GetAllCategories()
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                });

            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _productService.DeleteProductAsync(id);
            return RedirectToAction("Index");
        }
    }
}
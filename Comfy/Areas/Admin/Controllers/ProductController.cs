using BLL.Interfaces;
using Comfy.ViewModels;
using DOMAIN;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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
        [ValidateAntiForgeryToken]
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

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null) return NotFound();

            ViewBag.Categories = _categoryService.GetAllCategories()
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                });

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductViewModel model)
        {
            var updatedProduct = new Product
            {
                Id = id,
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                Quantity = model.Quantity,
                CategoryId = model.CategoryId,
                ImageUrl = model.ImageUrl ?? ""
            };

            if (id != updatedProduct.Id) return BadRequest();

            ViewBag.Categories = _categoryService.GetAllCategories()
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                });

            if (ModelState.IsValid)
            {
                try
                {
                    await _productService.UpdateProductAsync(updatedProduct);
                    return RedirectToAction("Index");
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
            }

            return View(updatedProduct);
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _productService.DeleteProductAsync(id);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Detail(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null) return NotFound();

            return View(product);
        }
    }
}
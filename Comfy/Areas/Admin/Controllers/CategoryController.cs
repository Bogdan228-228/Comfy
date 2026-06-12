using BLL.Interfaces;
using Comfy.Areas.Admin.Models;
using DOMAIN;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Comfy.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var categories = _categoryService.GetAllCategories();
            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var category = new Category { Name = model.Name };
                var result = await _categoryService.AddCategoryAsync(category);

                if (!result)
                {
                    ModelState.AddModelError("Name", "Категорія з такою назвою вже існує.");
                    return View(model);
                }

                return RedirectToAction("Index");
            }
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _categoryService.DeleteCategoryAsync(id);
            return RedirectToAction("Index");
        }
    }
}

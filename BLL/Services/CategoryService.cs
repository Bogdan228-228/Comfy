using BLL.Interfaces;
using DLL;
using DOMAIN;

namespace BLL.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ComfyDbContext _context;

        public CategoryService(ComfyDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddCategoryAsync(Category category)
        {
            if (_context.Categories.Any(c => c.Name == category.Name))
            {
                return false;
            }

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = GetCategoryById(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }

        public IEnumerable<Category> GetAllCategories() => _context.Categories.ToList();

        public Category GetCategoryById(int id) => _context.Categories.FirstOrDefault(c => c.Id == id);
    }
}

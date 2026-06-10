using DOMAIN;

namespace BLL.Interfaces
{
    public interface ICategoryService
    {
        IEnumerable<Category> GetAllCategories();
        Category GetCategoryById(int id);
        Task<bool> AddCategoryAsync(Category category);
        Task DeleteCategoryAsync(int id);
    }
}

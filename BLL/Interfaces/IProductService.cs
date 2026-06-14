using DOMAIN;

namespace BLL.Interfaces
{
    public interface IProductService
    {
        IEnumerable<Product> GetAllProducts();
        Task<Product> GetProductByIdAsync(int id);
        Task AddProductAsync(Product product);
        Task DeleteProductAsync(int id);
        List<Product> SearchProductsByName(string searchString);
        Task EditProductAsync(Product updatedProduct);
    }
}
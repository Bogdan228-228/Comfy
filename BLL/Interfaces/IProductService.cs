using DOMAIN;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IProductService
    {
        IEnumerable<Product> GetAllProducts();
        Product? GetProductById(int id);
        Task AddProductAsync(Product product);
        Task DeleteProductAsync(int id);
        List<Product> SearchProductsByName(string searchString);
        Task UpdateProductAsync(Product product);
        Task<Product> GetProductByIdAsync(int id);
    }
}
using DOMAIN;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IProductService
    {
        IEnumerable<Product> GetAllProducts();
<<<<<<< HEAD
        Product? GetProductById(int id);
        Task AddProductAsync(Product product);
        Task DeleteProductAsync(int id);
        List<Product> SearchProductsByName(string searchString);
        Task UpdateProductAsync(Product product);
=======
        Task<Product> GetProductByIdAsync(int id);
        Task AddProductAsync(Product product);
        Task DeleteProductAsync(int id);
        List<Product> SearchProductsByName(string searchString);
        Task EditProductAsync(Product updatedProduct);
>>>>>>> 2e8ea6e988554a1fa6e4cdf29892f46c226dd840
    }
}
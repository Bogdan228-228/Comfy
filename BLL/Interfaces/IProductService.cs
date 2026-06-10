using DOMAIN;

namespace BLL.Interfaces
{
    public interface IProductService
    {
        IEnumerable<Product> GetAllProducts();
        Product GetProductById(int id);
        Task AddProductAsync(Product product);
        Task DeleteProductAsync(int id);
    }
}
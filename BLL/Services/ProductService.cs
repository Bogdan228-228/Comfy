using DOMAIN;
using DLL;
using BLL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services
{
    public class ProductService : IProductService
    {
        private readonly ComfyDbContext _context;

        public ProductService(ComfyDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Product> GetAllProducts() => _context.Products.Include(c => c.Category).ToList();

<<<<<<< HEAD
        public Product? GetProductById(int id) => _context.Products.FirstOrDefault(p => p.Id == id);
=======
        public async Task<Product> GetProductByIdAsync(int id) => await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);
>>>>>>> 2e8ea6e988554a1fa6e4cdf29892f46c226dd840

        public async Task AddProductAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await GetProductByIdAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        public List<Product> SearchProductsByName(string searchString)
        {
            var products = _context.Products.Include(p => p.Category).AsQueryable();
            if (!string.IsNullOrEmpty(searchString))
            {
                products = products.Where(p => p.Name.Contains(searchString));
            }
            return products.ToList();
        }

<<<<<<< HEAD
        public async Task UpdateProductAsync(Product product)
        {
            _context.Products.Update(product);
=======
        public async Task EditProductAsync(Product updatedProduct)
        {
            var product = await GetProductByIdAsync(updatedProduct.Id);
            if (product == null)
            {
                throw new ArgumentException("Product not found");
            }

            product.Name = updatedProduct.Name;
            product.Description = updatedProduct.Description;
            product.Price = updatedProduct.Price;
            product.Quantity = updatedProduct.Quantity;
            product.ImageUrl = updatedProduct.ImageUrl;
            product.CategoryId = updatedProduct.CategoryId;

>>>>>>> 2e8ea6e988554a1fa6e4cdf29892f46c226dd840
            await _context.SaveChangesAsync();
        }
    }
}
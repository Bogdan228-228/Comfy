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

        public Product GetProductById(int id) => _context.Products.FirstOrDefault(p => p.Id == id);

        public async Task AddProductAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = GetProductById(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
    }
}
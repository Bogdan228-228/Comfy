using DOMAIN;
using DLL;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class ProductService : IProductService
    {
        private readonly ComfyDbContext _context;

        public ProductService(ComfyDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Product> GetAllProducts() => _context.Products.ToList();

        public Product GetProductById(int id) => _context.Products.FirstOrDefault(p => p.Id == id);

        public void AddProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public void DeleteProduct(int id)
        {
            var product = GetProductById(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
        }
    }
}
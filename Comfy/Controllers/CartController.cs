using Comfy.Helpers;
using Comfy.Models;
using DLL;
using DOMAIN;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

public class CartController : Controller
{
    private readonly ComfyDbContext _context;

    public CartController(ComfyDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public IActionResult Add(int productId, int quantity)
    {
        var product = _context.Products.Find(productId);
        if (product == null) return NotFound();

        var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();

        var existing = cart.FirstOrDefault(c => c.ProductId == productId);
        if (existing != null)
        {
            existing.Quantity += quantity;
        }
        else
        {
            cart.Add(new CartItem
            {
                ProductId = product.Id,
                Name = product.Name,
                Price = product.Price,
                Quantity = quantity
            });
        }

        HttpContext.Session.SetObjectAsJson("Cart", cart);

        return RedirectToAction("Index", "Home");
    }

    public IActionResult Index()
    {
        var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
        return View(cart);
    }

    [Authorize]
    public IActionResult Checkout()
    {
        var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart");
        if (cart == null || !cart.Any()) return RedirectToAction("Index");

        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        var order = new Order
        {
            UserId = userId,
            OrderDate = DateTime.Now,
            TotalPrice = cart.Sum(c => c.Price * c.Quantity),
            Status = OrderStatus.Pending,
            OrderDetails = cart.Select(c => new OrderDetail
            {
                ProductId = c.ProductId,
                Quantity = c.Quantity
            }).ToList()
        };

        _context.Orders.Add(order);
        _context.SaveChanges();

        HttpContext.Session.Remove("Cart");

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public IActionResult Remove(int productId)
    {
        var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();

        var item = cart.FirstOrDefault(c => c.ProductId == productId);
        if (item != null)
        {
            cart.Remove(item);
            HttpContext.Session.SetObjectAsJson("Cart", cart);
        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult UpdateQuantity(int productId, int quantity)
    {
        var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();

        var item = cart.FirstOrDefault(c => c.ProductId == productId);
        if (item != null)
        {
            item.Quantity = quantity > 0 ? quantity : 1;
            HttpContext.Session.SetObjectAsJson("Cart", cart);

        }

        return RedirectToAction("Index");
    }
}
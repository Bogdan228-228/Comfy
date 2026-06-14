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
    [HttpGet]
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
            OrderDetails = new List<OrderDetail>()
        };

        foreach (var item in cart)
        {
            var product = _context.Products.Find(item.ProductId);
            if (product != null)
            {
                if (product.Quantity < item.Quantity)
                {
                    throw new Exception("Недостатньо товару на складі");
                }

                order.OrderDetails.Add(new OrderDetail
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                });
            }
        }

        HttpContext.Session.SetObjectAsJson("PendingOrder", order);

        var random = new Random();
        var code = random.Next(100000, 999999);
        HttpContext.Session.SetString("ConfirmCode", code.ToString());

        return RedirectToAction("Confirm");
    }

    [Authorize]
    [HttpGet]
    public IActionResult Confirm()
    {
        var order = HttpContext.Session.GetObjectFromJson<Order>("PendingOrder");

        ViewBag.Code = HttpContext.Session.GetString("ConfirmCode");

        return View(order);
    }
    [Authorize]
    [HttpPost]
    public IActionResult Confirm(string enteredCode)
    {
        var expectedCode = HttpContext.Session.GetString("ConfirmCode");
        if (enteredCode == expectedCode)
        {
            var order = HttpContext.Session.GetObjectFromJson<Order>("PendingOrder");
            if (order == null) return RedirectToAction("Index", "Cart");

            foreach (var item in order.OrderDetails)
            {
                var product = _context.Products.Find(item.ProductId);
                if (product != null)
                {
                    if (product.Quantity < item.Quantity)
                    {
                        throw new Exception("Недостатньо товару на складі");
                    }
                    product.Quantity -= item.Quantity;

                    _context.Products.Update(product);
                }
            }

            _context.Orders.Add(order);
            _context.SaveChanges();

            HttpContext.Session.Remove("Cart");
            HttpContext.Session.Remove("PendingOrder");
            HttpContext.Session.Remove("ConfirmCode");

            return RedirectToAction("Success");
        }

        ModelState.AddModelError("", "Невірний код підтвердження");
        var orderRetry = HttpContext.Session.GetObjectFromJson<Order>("PendingOrder");
        ViewBag.Code = HttpContext.Session.GetString("ConfirmCode");
        return View(orderRetry);
    }

    [Authorize]
    [HttpGet]
    public IActionResult Success()
    {
        return View();
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
using IcSoft.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

[Route("OrderItems")]
public class OrderItemsController : Controller
{
    private readonly ApplicationDbContext _context;

    public OrderItemsController(ApplicationDbContext context)
    {
        _context = context;
    }
    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        // Simulate a test order with fixed data for testing
        var order = new Order
        {
            Id = 1, // Example Order ID
            UserId = "testUser123",
            ShopUser = new ShopUser { FirstName = "Nguyễn Thu Hoài" },
            TotalAmount = 7900000, // Example total amount
            ShippingAddress = "Cầu Giấy - Hà Nội",
            PaymentMethod = "Credit Card",
            CreatedAt = DateTime.UtcNow,
            OrderItems = new List<OrderItem>
            {
                new OrderItem
                {
                    Id = 1,
                    ProductId = 1,
                    Quantity = 1,
                    Price = 2700000,
                    Product = new Product { ProductName = "AUTUMN" }
                },
                new OrderItem
                {
                    Id = 2,
                    ProductId = 2,
                    Quantity = 1,
                    Price = 2500000,
                    Product = new Product { ProductName = "DAWN" }
                },
                new OrderItem
                {
                    Id = 3,
                    ProductId = 3,
                    Quantity = 1,
                    Price = 2700000,
                    Product = new Product { ProductName = "SERENE" }
                }
            }
        };

        // Return the order data to the view
        return View("~/Views/OrderItems/OrderItems.cshtml", order);
    }
}
    
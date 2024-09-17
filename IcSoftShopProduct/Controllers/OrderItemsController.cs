using IcSoft.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace IcSoftShopProduct.Controllers
{
    public class OrderItemsController : Controller
    {
        public IActionResult OrderItems(int id)
        {
            // Example data creation (In reality, this data should come from your database)
            var order = new Order
            {
                Id = 33,
                UserId = "user123",
                ShopUser = new ShopUser { FirstName = "Nguyễn Thu Hoài" },
                TotalAmount = 7900000,
                ShippingAddress = "Cầu Giấy - Hà Nội",
                PaymentMethod = "Credit Card",
                CreatedAt = DateTime.UtcNow,
                OrderItems = new List<OrderItem>
            {
                new OrderItem { Id = 1, ProductId = 1, Quantity = 1, Price = 2700000, Product = new Product {  ProductName = "AUTUMN"}},
                new OrderItem { Id = 2, ProductId = 2, Quantity = 1, Price = 2500000, Product = new Product {  ProductName = "DAWN"}},
                new OrderItem { Id = 3, ProductId = 3, Quantity = 1, Price = 2700000, Product = new Product {  ProductName = "SERENE"}},
            }
            };

            return View("~/Views/Pages/OrderItems.cshtml", order);// Pass the order to the view
        }
    }
}

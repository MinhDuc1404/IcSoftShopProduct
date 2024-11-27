using IcSoft.Infrastructure.Migrations;
using IcSoft.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("OrderItems")]
public class OrderItemsController : Controller
{
    private readonly ApplicationDbContext _context;

    public OrderItemsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index(int id)
    {
        if (id <= 0)
        {
            return BadRequest("Order ID must be greater than zero.");
        }

        // Retrieve the order
        var order = await _context.Orders
            .Where(o => o.Id == id)
            .Include(o => o.ShopUser)
            .Include(o => o.Coupon)
            .Include(o => o.OrderItems)  
                .ThenInclude(oi => oi.Product)
                .ThenInclude(p => p.ProductImage)
            .FirstOrDefaultAsync();

        if (order == null)
        {
            return NotFound($"Order with ID {id} not found.");
        }

        // Retrieve order items by OrderId
        var orderItems = await _context.OrderItems
            .Where(oi => oi.OrderId == id) 
            .Include(oi => oi.Product)
            .ThenInclude(p => p.ProductImage)
            .ToListAsync();

      
        var orderModel = new Order
        {
            Id = order.Id,
            UserId = order.UserId,
            Coupon = order.Coupon,
            ShopUser = order.ShopUser,
            TotalAmount = order.TotalAmount,
            ShippingAddress = order.ShippingAddress,
            PaymentMethod = order.PaymentMethod,
            CreatedAt = order.CreatedAt,
            status = order.status,
            OrderItems = orderItems
           
        };

        return View("~/Views/OrderItems/OrderItems.cshtml", orderModel);
    }
}

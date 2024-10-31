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
            .Include(o => o.OrderItems)  // Eager load OrderItems
                .ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync();

        if (order == null)
        {
            return NotFound($"Order with ID {id} not found.");
        }

        // Retrieve order items by OrderId
        var orderItems = await _context.OrderItems
            .Where(oi => oi.OrderId == id) // Correctly filter by OrderId
            .Include(oi => oi.Product)
            .ToListAsync();

        // Create order model
        var orderModel = new Order
        {
            Id = order.Id,
            UserId = order.UserId,
            ShopUser = order.ShopUser,
            TotalAmount = order.TotalAmount,
            ShippingAddress = order.ShippingAddress,
            PaymentMethod = order.PaymentMethod,
            CreatedAt = order.CreatedAt,
            status = order.status,
            OrderItems = orderItems // Should now contain the correct items
        };

        // Return the order data to the view
        return View("~/Views/OrderItems/OrderItems.cshtml", orderModel);
    }
}

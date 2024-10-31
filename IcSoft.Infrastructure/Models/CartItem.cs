using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IcSoft.Infrastructure.Models
{
    public class CartItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }  // Bạn có thể thay thế bằng ProductName
        public string Color { get; set; }
        public string Size { get; set; }
        public int Quantity { get; set; }

        public string ProductImageUrl { get; set; }
        public decimal Price { get; set; }  // Giá của một sản phẩm
        public decimal TotalPrice => Quantity * Price;  // Tổng giá = số lượng * giá
    }
}

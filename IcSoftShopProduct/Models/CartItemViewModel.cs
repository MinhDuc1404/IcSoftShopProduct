namespace IcSoftShopProduct.Models
{
    public class CartItemViewModel
    {
        public string ProductName { get; set; }  // Bạn có thể thay thế bằng ProductName
        public string Color { get; set; }
        public string Size { get; set; }
        public int Quantity { get; set; }

        public string ProductImageUrl { get; set; }
        public decimal Price { get; set; }  // Giá của một sản phẩm
        public decimal TotalPrice => Quantity * Price;  // Tổng giá = số lượng * giá
    }
}

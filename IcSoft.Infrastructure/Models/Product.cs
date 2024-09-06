using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IcSoft.Infrastructure.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Vui lòng điền thông tin tên sản phẩm")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Vui lòng điền thông tin giá sản phẩm")]
        [Column(TypeName = "decimal(18, 2)")]

        public decimal ProductPrice { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? ProductDiscount { get; set; }

        [Required(ErrorMessage = "Vui lòng điền thông tin số lượng sản phẩm")]
        public int ProductQuantity { get; set; }

        public string ProductDescription { get; set; }

        public string ProductColor { get; set; }

        public string ProductSize { get; set; }

        public int CategoryID { get; set; }

        public int CollectionID { get; set; }
        public List<ProductImage> ProductImage { get; set; }

        public virtual Collection Collection { get; set; }

        public virtual Category Category { get; set; }
    }
}
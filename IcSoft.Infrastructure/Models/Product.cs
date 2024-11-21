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

        public List<ProductColor>? ProductColors { get; set; }

        public List<ProductSize>? ProductSizes { get; set; }

        [Required(ErrorMessage = "Vui lòng điền thông tin số lượng sản phẩm")]
        public int ProductQuantity { get; set; }

        public int? ProductSale { get; set; }

        public string ProductDescription { get; set; }
        public DateTime? CreatedDate { get; set; }
        [ForeignKey("Category")]
        [Required(ErrorMessage = "Vui lòng chọn danh mục")]
        public int CategoryID { get; set; }
        [ForeignKey("Collection")]
        [Required(ErrorMessage = "Vui lòng chọn bộ sưu tập")]
        public int CollectionID { get; set; }
        public List<ProductImage>? ProductImage { get; set; }

        public string ProductSizeImage { get; set; }

        public virtual Collection Collection {  get; set; }

        public virtual Category Category { get; set; }

    }
}

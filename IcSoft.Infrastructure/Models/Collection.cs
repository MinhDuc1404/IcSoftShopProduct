using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IcSoft.Infrastructure.Models
{
    public class Collection
    {
        [Key]
        public int CollectionId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên danh mục")]
        public string CollectionName { get; set; }

        public List<Product> Product { get; set; }
    }
}

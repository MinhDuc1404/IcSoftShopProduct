using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IcSoft.Infrastructure.Models
{
    public class Size
    {
        [Key]
        public int SizeId { get; set; }
        public string SizeName { get; set; }

        public List<ProductSize> ProductSizes { get; set; }
    }
}

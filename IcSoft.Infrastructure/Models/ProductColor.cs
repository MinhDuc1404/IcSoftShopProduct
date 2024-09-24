using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IcSoft.Infrastructure.Models
{
    public class ProductColor
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int ColorId { get; set; }
        public Colors Color { get; set; }
    }

}

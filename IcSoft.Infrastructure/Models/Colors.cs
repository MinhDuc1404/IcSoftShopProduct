using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IcSoft.Infrastructure.Models
{
    public class Colors
    {
        [Key]
        public int ColorId { get; set; }
        [Required]
        public string ColorName { get; set; } 

        [Required]
        public string ColorCode { get; set; } 

        public List<ProductColor> ProductColors { get; set; }
    }
}

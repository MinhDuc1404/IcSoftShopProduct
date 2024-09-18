using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IcSoft.Infrastructure.Models
{
    public class Coupon
    {
        public int Id { get; set; }
        public string Code { get; set; }

        public decimal Discount { get; set; }

        // Start date of validity
        public DateTime ValidFrom { get; set; }

        // Expiration date
        public DateTime ValidUntil { get; set; }

        // Maximum number of uses
        public int UsageLimit { get; set; }
    }
}

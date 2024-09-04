using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IcSoft.Infrastructure.Models
{
    public class Collection
    {
        public int CollectionId { get; set; }

        public string CollectionName { get; set; }

        public virtual List<Product> Product { get; set; }
    }
}

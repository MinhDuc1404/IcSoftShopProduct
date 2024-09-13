﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IcSoft.Infrastructure.Models
{
    public class ProductImage
    {
        [Key]
        public int ImageId { get; set; }

        public string ImageUrl { get; set; }

        public int ProductId { get; set; }

        public virtual Product Product { get; set; }
    }
}

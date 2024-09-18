using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
namespace IcSoft.Infrastructure.Models
{
    public class ShopUser : IdentityUser
    {
        [PersonalData]
        public string FirstName { get; set; }
        [PersonalData]
        public string LastName { get; set; }
        public string Phone { get; set; } = "";
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public string? Address { get; set; }

       
    }
}

using IcSoft.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IcSoft.Infrastructure.Services.Interface
{
    public interface ISizeServices
    {
        Task<Size> AddSize(Size Size);
        Task<List<Size>> GetListSize();

        Task<Size> FindSize(int id);

        Task<Size> UpdateSize(Size size);

        Task<Size> DeleteSize(Size size);
       

    }
}

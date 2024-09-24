using IcSoft.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IcSoft.Infrastructure.Services.Interface
{
    public interface IColorServices
    {
        Task<Colors> AddColor(Colors colors);

        Task<List<Colors>> GetListColor();

        Task<Colors> FindColor(int id);

        Task<Colors> UpdateColor(Colors colors);

        Task<Colors> DeleteColor(Colors colors);
      
    }
}

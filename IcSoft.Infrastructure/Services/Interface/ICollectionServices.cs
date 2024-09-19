using IcSoft.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IcSoft.Infrastructure.Services.Interface
{
    public interface ICollectionServices
    {
        Task<Collection> AddCollection(Collection collection);
    }
}

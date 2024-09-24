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
        Task<List<Collection>> GetListCollection();
        Task<Collection> FindCollection(int id);
        Task<Collection> UpdateCollection(Collection collection);
        Task<Collection> DeleteCollection(Collection collection);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HUBT_Social_MongoDb_Service.Services
{
    public interface IMongoService<Collection>
        where Collection : class
    {
        Task<Collection?> GetById(string id);
        Task<bool> Create(Collection collection);
        Task<bool> Delete(Collection collection);
        Task<bool> Update(Collection collection);
        Task<IEnumerable<Collection>> GetAll();
        Task<bool> Exists(string id);
        Task<long> Count();
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;

namespace ElasticSearchDotNet
{
    public interface IElasticRepository<T>
    {
        string Index { get; }
        Task<IEnumerable<User>> GetAll();
        Task<T> GetById(string id);
        Task Add(T model);
        Task Update(string id, T model);
        Task Delete(string id);
    }
}
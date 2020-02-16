using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nest;

namespace ElasticSearchDotNet
{
    public class UserElasticRepository : IElasticRepository<User>
    {
        public readonly ElasticClient _elastic;
        public UserElasticRepository(ElasticClient elastic)
        {
            _elastic = elastic;
        }

        public string Index => "users_idx";

        public async Task Add(User model)
        {
            await _elastic.IndexAsync(model, idx => idx.Index(Index).Id(model.Id));
        }

        public async Task Delete(string id)
        {
            var response = await _elastic.DeleteAsync(new DocumentPath<User>(id), q => q.Index(Index));
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            var response = await _elastic.SearchAsync<User>(s => s.Index(Index));

            return response.Documents;
        }

        public async Task<User> GetById(string id)
        {
            var response = await _elastic.SearchAsync<User>(s => s.Index(Index)
                                                                  .Query(q => q.Match(m => m.Field(f => f.Id)
                                                                                            .Query(id))));

            return response.Documents.FirstOrDefault();
        }

        public async Task Update(string id, User model)
        {
            await _elastic.UpdateAsync(new DocumentPath<User>(id), u => u.Index(Index).Doc(model));
        }
    }
}
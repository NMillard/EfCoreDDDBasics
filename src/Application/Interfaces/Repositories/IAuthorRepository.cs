using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.GoodExamples;

namespace Application.Interfaces.Repositories {
    public interface IAuthorRepository {
        Task<IEnumerable<Author>> GetAllAsync();
    }
}
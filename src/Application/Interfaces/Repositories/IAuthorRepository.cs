using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.GoodExamples;

namespace Application.Interfaces.Repositories {
    public interface IAuthorRepository {
        Task<IEnumerable<Author>> GetAllAsync();
        Task<IEnumerable<ISimpleBook>> GetBooksAsync();
    }

    public interface ISimpleBook {
        string Title { get; }
        string Genre { get; }
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Interfaces.Repositories;

namespace Application.Queries {
    public interface IGetSimpleBooksQuery {
        Task<IEnumerable<ISimpleBook>> ExecuteAsync();
    }
    
    public class SimpleBooksQuery : IGetSimpleBooksQuery {
        private readonly IAuthorRepository repository;

        public SimpleBooksQuery(IAuthorRepository repository) {
            this.repository = repository;
        }

        public async Task<IEnumerable<ISimpleBook>> ExecuteAsync() => await repository.GetBooksAsync();
    }
}
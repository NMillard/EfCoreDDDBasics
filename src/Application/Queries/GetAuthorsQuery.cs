using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Interfaces.Repositories;
using Domain.GoodExamples;

namespace Application.Queries {

    public interface IGetAuthorsQuery {
        Task<IEnumerable<Author>> ExecuteAsync();
    }
    
    internal class GetAuthorsQuery : IGetAuthorsQuery {
        private readonly IAuthorRepository repository;

        public GetAuthorsQuery(IAuthorRepository repository) {
            this.repository = repository;
        }

        public async Task<IEnumerable<Author>> ExecuteAsync() => await repository.GetAllAsync();
    }
}
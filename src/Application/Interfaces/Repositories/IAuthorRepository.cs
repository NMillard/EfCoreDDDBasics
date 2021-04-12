using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.GoodExamples;

namespace Application.Interfaces.Repositories {
    public interface IAuthorRepository {
        Task<IEnumerable<Author>> GetAllAsync();
        Task<Author> GetAuthorAsync(Guid byId);
        Task<IEnumerable<ISimpleBook>> GetBooksAsync();
        Task<bool> UpdateAuthorAsync(Author author);
    }

    public interface ISimpleBook {
        string Title { get; }
        string Genre { get; }
    }
}
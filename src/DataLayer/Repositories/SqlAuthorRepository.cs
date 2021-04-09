using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Interfaces.Repositories;
using Domain.GoodExamples;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repositories {
    internal class SqlAuthorRepository : IAuthorRepository {
        private readonly AppDbContext context;

        public SqlAuthorRepository(AppDbContext context) {
            this.context = context;
        }

        public async Task<IEnumerable<Author>> GetAllAsync() => await context.Authors.ToListAsync();
        public async Task<IEnumerable<ISimpleBook>> GetBooksAsync() => await context.SimpleBooks.ToListAsync();
    }
}
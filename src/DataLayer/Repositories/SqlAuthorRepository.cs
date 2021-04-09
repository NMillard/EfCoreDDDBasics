using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces.Repositories;
using Domain.GoodExamples;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repositories {
    
    /*
     * A repository should never return types defined in the data layer.
     * 
     * Always return types that are either domain types or specialized types
     * from the application layer.
     *
     * 
     */
    internal class SqlAuthorRepository : IAuthorRepository {
        private readonly AppDbContext context;

        public SqlAuthorRepository(AppDbContext context) {
            this.context = context;
        }

        /*
         * The two includes are not required because EF Core 5 can be configured to
         * load them automatically.
         *
         * But, this is one way of ensuring an aggregate is fully loaded. Especially useful
         * if you're using previous versions of EF Core.
         */
        private IQueryable<Author> Authors => context.Authors
            .Include(a => a.Books)
            .Include(a => a.MainAddress)
            .AsQueryable();

        public async Task<IEnumerable<Author>> GetAllAsync() => await Authors.ToListAsync();
        public async Task<IEnumerable<ISimpleBook>> GetBooksAsync() => await context.SimpleBooks.ToListAsync();
    }
}
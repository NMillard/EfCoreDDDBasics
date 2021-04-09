using System.Threading.Tasks;
using DataLayer.Views;
using Domain.BadExamples;
using Domain.GoodExamples;
using Microsoft.EntityFrameworkCore;

namespace DataLayer {
    public interface IAppContext {
        /*
         * Verify that we can connect to the database.
         * Great for health checks.
         */
        Task<bool> CanConnectAsync();
    }
    
    /*
     * Notice the class is marked 'internal'.
     * We don't want to expose this to the world.
     */
    internal class AppDbContext : DbContext, IAppContext {
        /*
         * Taking DbContextOptions as constructor param allows us to later
         * specify another provider which is handy when writing unit and
         * integration tests.
         *
         * We're then also allowed to create migrations without any
         * startup project.
         */
        public AppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<BadAuthor> BadAuthors { get; set; }
        public DbSet<BadBook> BadBooks { get; set; }

        public DbSet<Author> Authors { get; set; }
        public DbSet<BooksView> SimpleBooks { get; set; }
        
        public async Task<bool> CanConnectAsync() => await Database.CanConnectAsync();

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            modelBuilder.HasDefaultSchema("EfCore");
        }
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataLayer.Repositories;
using Domain.GoodExamples;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DataLayer.Tests {
    public class SqlRepositoryShould {
        [Fact]
        public async Task GetAllAuthors() {
            // Arrange
            DbContextOptionsBuilder options = new DbContextOptionsBuilder().UseInMemoryDatabase(
                Guid.NewGuid().ToString() // Use guid, otherwise parallel tests will use same in-mem db.
                );
            var db = new AppDbContext(options.Options);
            db.Authors.Add(new Author("Hello")); // Add some arbitrary author
            await db.SaveChangesAsync();
            
            var repo = new SqlAuthorRepository(db);

            // Act
            IEnumerable<Author> authors = await repo.GetAllAsync();

            // Assert
            Assert.Single(authors);
        }
    }
}
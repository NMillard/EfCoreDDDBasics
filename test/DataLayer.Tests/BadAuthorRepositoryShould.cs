using System;
using System.Threading.Tasks;
using DataLayer.Repositories;
using Domain.BadExamples;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DataLayer.Tests {
    public class BadAuthorRepositoryShould {
        [Fact]
        public async Task CreateAuthorId() {
            var author = new BadAuthor("test-name");
            DbContextOptionsBuilder inMemoryDb = new DbContextOptionsBuilder().UseInMemoryDatabase(Guid.NewGuid().ToString());

            var sut = new BadAuthorRepository(new AppDbContext(inMemoryDb.Options));

            bool result = await sut.CreateAsync(author);
        }
    }
}
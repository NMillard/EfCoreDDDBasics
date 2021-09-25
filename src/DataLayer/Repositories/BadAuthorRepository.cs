using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Interfaces.Repositories;
using Domain.BadExamples;
using Domain.GoodExamples;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repositories {
    internal class BadAuthorRepository : IAuthorRepository {
        private readonly AppDbContext database;

        public BadAuthorRepository(AppDbContext database) {
            this.database = database;
        }
        
        public async Task<bool> CreateAsync(BadAuthor author) {
            database.Add(author);
            try {
                await database.SaveChangesAsync(); // <- author gets it's ID from the database.
                return true;
            } catch (DbUpdateException due) {
                // log error
                return false;
            }
        }

        public Task<IEnumerable<Author>> GetAllAsync() {
            throw new NotImplementedException();
        }

        public Task<Author> GetAuthorAsync(Guid byId) {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ISimpleBook>> GetBooksAsync() {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAuthorAsync(Author author) {
            throw new NotImplementedException();
        }
    }
}
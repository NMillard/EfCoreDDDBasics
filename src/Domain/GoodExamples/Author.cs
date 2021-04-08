using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.GoodExamples {
    /*
     * Domain class design should be ignorant of any persistence logic.
     */
    public class Author {
        private readonly List<Book> books;
        
        public Author(string name) {
            Id = Guid.NewGuid();
            Name = name;

            books = new List<Book>();
        }
        
        public Guid Id { get; }
        public string Name { get; private set; }

        public AuthorStatus Status { get; internal set; }
        
        /*
         * Never use public setters for collections.
         * Only expose the collection thru a read only property, with an encapsulated
         * backing field.
         */
        public IReadOnlyList<Book> Books => books;
        
        public void ChangeName(string name) {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Must have a value", nameof(name));
            Name = name;
        }

        public bool AddBook(Book book) {
            if (books.SingleOrDefault(b => b.Title.Equals(book.Title)) is { }) return false;
            books.Add(book);
            
            return true;
        }
    }

    public enum AuthorStatus {
        Active = 0,
        Inactive,
    }
}
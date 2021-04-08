using System;

namespace Domain.GoodExamples {
    public class BookType {
        private Guid id;

        public BookType(string genre) {
            Genre = genre;
            id = Guid.NewGuid();
        }

        public string Genre { get; }
    }
}
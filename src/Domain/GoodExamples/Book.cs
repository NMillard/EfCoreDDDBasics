namespace Domain.GoodExamples {
    /*
     * For illustrative purposes, think of this book class as an owned type.
     * It cannot exist on its own. It will always need a parent, in this case, an Author.
     *
     * The Book does not need to know about its author, as it will always only be accessible thru
     * the Author's read only collection property "Books".
     */
    public class Book {
        public Book(string title, BookType bookType) {
            Title = title;
            BookType = bookType;
        }
        
        public string Title { get; }

        public BookType BookType { get; }
    }
}
using Application.Interfaces.Repositories;

namespace DataLayer.Views {
    /*
     * The BooksView defines a narrow view of the books class.
     * Typically, you'd want to access owned entities through the
     * aggregate root, the Author in this case. But, for performance
     * reasons, you may want to take a shortcut and go directly for the
     * aggregates.
     *
     * Creating a specialized class works nicely for this.
     *
     * The issue is, since the class is defined in the data layer, and needs to be
     * used in the application layer, we'll have to create an interface in the application
     * layer that the BooksView then implements.
     *
     * Note that you'll have to manually write the SQL for the view.
     * So, essentially, you'll create a migration, and write raw SQL in the up and
     * down methods.
     */
    public class BooksView : ISimpleBook {
        public string Title { get; set; }
        public string Genre { get; set; }
    }
}
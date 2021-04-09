using DataLayer.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Configurations {
    public class BooksViewConfig : IEntityTypeConfiguration<BooksView> {
        public void Configure(EntityTypeBuilder<BooksView> builder) {
            // Calling ToView() ensures that no table is generated for this class.
            // You'll have to run 'migrations add' and add the view manually.
            builder.ToView("BooksView");
            builder.HasNoKey(); // because it's a view.
        }
    }
}
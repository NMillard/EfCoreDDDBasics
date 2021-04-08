using Domain.GoodExamples;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Configurations {
    public class BookTypeConfig : IEntityTypeConfiguration<BookType> {
        public void Configure(EntityTypeBuilder<BookType> builder) {
            builder.ToTable("BookTypes");

            builder.HasKey("id");
            builder.Property(bt => bt.Genre)
                .IsRequired()
                .HasMaxLength(100)
                .HasDefaultValue("Unspecified");
        }
    }
}
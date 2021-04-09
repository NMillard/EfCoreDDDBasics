using System;
using Domain.GoodExamples;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Configurations {
    public class AuthorConfig : IEntityTypeConfiguration<Author> {
        public void Configure(EntityTypeBuilder<Author> builder) {
            builder.ToTable("Authors");

            // Id is get-only. We're required to tell EF that it should use this as the key
            builder.HasKey(nameof(Author.Id));

            builder.Property(a => a.Name)
                .HasMaxLength(100) // May provide huge performance gains
                .IsRequired();

            builder.Property(a => a.Status)
                .HasMaxLength(50)
                .IsRequired()
                .HasConversion<string>();

            builder.Navigation(nameof(Author.Books))
                .UsePropertyAccessMode(PropertyAccessMode.Field) // Convention based -> will find field named "books"
                .AutoInclude(); // Automatically include this in any query

            // A book may never be an entity of its own. It'll depend on having an existing Author parent.
            builder.OwnsMany<Book>(nameof(Author.Books), bookBuilder => {
                bookBuilder.ToTable("Books");
                
                /*
                 * Use composite primary key for the book, as we don't want the book to have an ID of its own.
                 */
                const string foreignKey = "AuthorId"; // Convention based naming
                bookBuilder.Property<Guid>(foreignKey);
                bookBuilder.WithOwner().HasForeignKey(foreignKey); // Not required since we named the key properly
                bookBuilder.HasKey(foreignKey, nameof(Book.Title));

                bookBuilder.Property(p => p.Title)
                    .HasMaxLength(150)
                    .IsRequired();

                bookBuilder.Ignore(b => b.Genre);

                bookBuilder.Property(b => b.BookType)
                    .IsRequired()
                    .HasConversion( // You can even convert a value into an object
                        value => value.Genre,
                        converted => new BookType(converted)
                    );
            });
        }
    }
}
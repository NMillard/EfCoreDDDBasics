using System;
using Domain.GoodExamples;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Configurations.Seeds {
    public class BookTypeSeeding : IEntityTypeConfiguration<BookType> {
        public void Configure(EntityTypeBuilder<BookType> builder) {
            /* 1. Use anonymous objects to seed data for properties that'd otherwise
             *    be encapsulated in the class. E.g. a private ID field.
             *    NOTE: The anon object's property names MUST match
             *          the column names.
             * 
             * 2. If you're using GUIDs, then make sure to generate the GUID
             *    manually. Otherwise, running "add migrations" will delete
             *    the previously created seed data and seed new rows, due
             *    to the changed GUID.
             *
             * 3. HasData is also called when calling EnsureCreated().
             *    This allows you to define data that should also
             *    be present when running tests with an
             *    In-Memory provider.
             */
            builder.HasData(
                new { id = Guid.Parse("26f40fdf-8f92-4c4f-80c1-71090d86aef4"), Genre = "Horror" },
                new { id = Guid.Parse("1b0f8308-feb0-4d55-93ec-0765971e0bb7"), Genre = "Fiction" },
                new { id = Guid.Parse("5f7f47e3-610f-499c-9119-b73e1df23b62"), Genre = "Crime" },
                new { id = Guid.Parse("804B81BF-1730-43C8-AEEB-DE6685E41CC3"), Genre = "Other" }
            );
        }
    }
}
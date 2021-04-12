# Basic EF Core with DDD approach

You can easily use EntityFramework Core with a domain driven approach.
Newer versions of EF Core are awesome and plays nicely with proper domain models. Typically, many .NET developers think they have to cater for EF Core and compromise their domain models to make life easier with EF Core. This is absolutely not the case.

You should never compromise your domain design to please an external framework.

Instead, spend a few minutes to learn EF Core and its affordances, as it entirely possible to protect your domain and its invariants without concessions, resorting to horrendous practices like using public setters and exposing mutable collections.

## What bad practices I'm talking about?
You'll often find "domain" classes with poor separation of concerns, where significant persistence concerns are implemented. If you haven't spend the necessary time to understand model configurations with EF Core, you're inclined to produce models like below.

```csharp
[Table("Authors")]
public class BadAuthor {
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("AuthorName")]
    public string Name { get; set; }

    public List<BadBook> Books { get; set; }
}

[Table("Books")]
public class BadBook {
    [Required(ErrorMessage = "The book needs a title")]
    [MaxLength(50)]
    public string Title { get; set; }

    [DataType("DateTime2")]
    [Required]
    public DateTime Released { get; set; }

    public BadAuthor Author { get; set; }
        
    [ForeignKey(nameof(Author))]
    public Guid AuthorId { get; set; }
}
```

You see, these attributes are purely persistence--sometimes called infrastructure--concerns. They are completely irrelevant to the domain. Which means, they shouldn't be included at all.

Defining foreign keys in domain models are also a big no-go. Again, these are only used for persistence.

What's worse is the public setters. You're effectively allowing invalid states. Even if you instantiate them correctly, you're still at risk of client code changing an object to an invalid invariant. Nothing's stopping that.

## Making domain models persistence ignorant requires more code.
I get why you'd want to write some quick and dirty code. It's easy. It's fast. It might even be okay. From a pragmatic stance, it sometimes make sense to mix concerns, occasionally. Like, if you're writing a PoC, or something short-lived (we know nothing is short-lived though, it's a lie).

This is not one of those articles that'll take a pragmatic approach. I want to examplify how you can properly weed out infrastructure concerns from your domain models, even in combination with EF Core.

As a constrast to the BadAuthor shown before, a more DDD aligned Author class is demonstrated below. This is obviously a contrived example, but does however demonstrate important aspects and principles, such as not exposing the collection of books, only having private or internal setters, and the use of a value object, that is, the `Address`.

Lots of code, but nothing out of the ordinary. There's not any crazy going on. But do spend a few seconds glaring over the code.

```csharp
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
    public Address MainAddress { get; private set; }

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

    public void UpdateAddress(Address address) =>
        MainAddress = address ?? throw new ArgumentException("Must have a value", nameof(address));
}

public enum AuthorStatus {
    Active = 0,
    Inactive,
}

// Book doesn't need an ID of its own as it'll always be an owned type of the Author.
public class Book {
    public Book(string title, BookType bookType) {
        Title = title;
        BookType = bookType;
    }

    public string Title { get; }
    public BookType BookType { get; }
    public string Genre => BookType.Genre;
}

public class BookType {
    private Guid id;    
    public BookType(string genre) {
        Genre = genre;
        id = Guid.NewGuid();
    }   
    public string Genre { get; }
}
```

## The context class and how to properly encapsulate it.
Before jumping into the model configuration, let's take a quick minute to set up our `DbContext`. It's essentially the type representing our database and allows us to work with data almost as if it was all in-memory. You absolutely don't want this class exposed to the world. It must only be available from within your data access project.

```csharp
public interface IAppContext {
    /*
     * Verify that we can connect to the database.
     * Great for health checks.
     */
    Task<bool> CanConnectAsync();
}

/*
 * Notice the class is marked 'internal'.
 * We don't want to expose this to the world.
 */
internal class AppDbContext : DbContext, IAppContext {
    /*
     * Taking DbContextOptions as constructor param allows us to later
     * specify another provider which is handy when writing unit and
     * integration tests.
     *
     * We're then also allowed to create migrations without any
     * startup project.
     */
    public AppDbContext(DbContextOptions options) : base(options) { }
    public DbSet<Author> Authors { get; set; }
    
    public async Task<bool> CanConnectAsync() => await Database.CanConnectAsync();
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        modelBuilder.HasDefaultSchema("EfCore");
    }
}
```

They only thing that might make you scratch your head is the `ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly)`. But, don't worry, it's quite simple.

All it's doing is to search the provided assembly for any class implementing `IEntityTypeConfiguration<T>`, and configure the models using that class. That's it.

Also, I've marked the `AppDbContext` as internal. If you already have a bit experience using EF Core, I'm sure you're used to having your context marked public. This is however a bad practice. You tell the world that all and any project may use this infrastructure class, and completely bypass all query logic you've implemented in repository classes.

However, you will need to register this with a dependency container at some point, that is likely to reside in an other project. The way you go about this is to create an extension method on the container itself, that'll register your internal class, as demonstrated below.

```csharp
public static class ServiceInjector {
    /*
     * Register our db context with the DI container, without exposing the concrete AppDbContext class.
     */
    public static IServiceCollection AddAppContext(this IServiceCollection services, string connectionString) {
        services.AddDbContext<AppDbContext>(builder => {
            builder.UseSqlServer(connectionString, builder => {
                builder.MigrationsHistoryTable("_MigrationHistory", "EfCore");
            });
        }); 

        /*
         * You typically don't want to expose the db context, not even thru an interface.
         * But, if you have to, then make sure you'll only expose the very minimum functionality
         * required by clients.
         */
        services.AddScoped<IAppContext, AppDbContext>();

        return services;
    }
}
```

## Configuring the domain models in EF Core
Again, this is a wall of code. But, I'll show excatly how you can configure EF Core to use domain models that don't have any infrastructure concerns baked into them.

```csharp
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
        builder.OwnsOne<Address>(nameof(Author.MainAddress), addressBuilder => {
            /*
             * Rename properties' columns because they'd otherwise be prefixed with 'Address_'
             */
            addressBuilder.Property(a => a.Street).HasColumnName(nameof(Address.Street));
            addressBuilder.Property(a => a.Zipcode).HasColumnName(nameof(Address.Zipcode));
            addressBuilder.Property(a => a.HouseNumber).HasColumnName(nameof(Address.HouseNumber));
            addressBuilder.Property(a => a.City).HasColumnName(nameof(Address.City));
        }); 
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
```

## Should I use DbContext directly in service/controller classes?
Absolutely not.

You'll likely retrieve only partially loaded entities. Related entities or value objects are not loaded by default.
With encapsulation and fully loaded aggregate roots in mind, it's better to use a repository that performs all the includes, and hands back a fully loaded aggregate root.

This is also one of the reasons that you should always mark your context class as internal. It must not be used outside of your data layer project.

Okay, so, say you need to perform a business operation that only requires some particular nested collections or objects to be loaded. You don't need the full aggregate root. Should you then go on and only load the required relationships? That's a big no.

Reason is you'll always work with fully defined and loaded entities. Partial loading leads to unnecessary headaches and complexities. This approach is called eager loading.
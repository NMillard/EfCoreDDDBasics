# Basic EF Core with DDD approach

You can easily use EntityFramework Core with a domain driven approach.
Typically, many .NET developers think they have to cater for EF Core and compromise their domain models to make life easier with EF Core. This is absolutely not the case.

You should never compromise your domain design to please an external framework.

Instead, spend a few minutes to learn EF Core and its affordances, as it entirely possible to protect your domain and its invariants without concessions, resorting to horrendous practices like using public setters and exposing mutable collections.

## What bad practices I'm talking about?
You'll often find "domain" classes with poor separation of concerns, where significant persistence concerns are implemented. Typically, you see junior developers create domain classes such as below.

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

This is not one of those articles that'll take a pragmatic approach. I want to examplify how you can properly weed out infrastructure concernns from your domain models, even in combination with EF Core.

## Should I use DbContext directly in service/controller classes?
Absolutely not.

You'll likely retrieve only partially loaded entities. Related entities or value objects are not loaded by default.
With encapsulation and fully loaded aggregate roots in mind, it's better to use a repository that performs all the includes, and hands back a fully loaded aggregate root.

This is also one of the reasons that you should always mark your context class as internal. It must not be used outside of your data layer project.

Okay, so, say you need to perform a business operation that only requires some particular nested collections or objects to be loaded. You don't need the full aggregate root. Should you then go on and only load the required relationships? That's a big no.

Reason is you'll always work with fully defined and loaded entities. Partial loading leads to unnecessary headaches and complexities. This approach is called eager loading.
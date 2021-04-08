using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.BadExamples {

    /*
     * Allowing persistence logic to creep into your domain models
     * is considered bad practice in DDD.
     *
     * How to avoid: follow the Persistence Ignorance principle.
     * Never let any persistence framework dictate your models' design.
     */
    [Table("BadBooks")]
    public class BadBook {
        public BadBook() {
            // Never have constructors that leave your objects in an invalid state.
            // Especially when it's just to please an external framework...
        }
        
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "The book needs a title")]
        [MaxLength(50)]
        public string Title { get; set; }

        [DataType("DateTime2")]
        [Required]
        public DateTime Released { get; set; }
        
        public BadAuthor Author { get; set; }
        
        [ForeignKey(nameof(Author))]
        public Guid AuthorId { get; set; }

        public bool IsDeleted { get; set; }
    }
}
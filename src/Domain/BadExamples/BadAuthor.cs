using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.BadExamples {
    [Table("BadAuthors")]
    public class BadAuthor {

        public BadAuthor() {
            Books = new List<BadBook>(); // Always be a good citizen and instantiate collections
        }
        
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("AuthorName")]
        public string Name { get; set; }

        public List<BadBook> Books { get; set; }
    }
}
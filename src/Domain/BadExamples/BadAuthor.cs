using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.BadExamples {
    [Table("BadAuthors")]
    public class BadAuthor {

        private BadAuthor() {
            // Make EF core happy
        }
        
        public BadAuthor(string name) {
            Name = name;
            Books = new List<BadBook>();
        }
        
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("AuthorName")]
        public string Name { get; set; }

        public List<BadBook> Books { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace Kontaktnik.Models
{
    public class ContactType
    {
        [Key]
        public int Id { get; init; }
        [Required]
        [MaxLength(50)]
        public string ContactTypeName { get; set; }

        public IEnumerable<ContactDetail> ContactDetails { get; set; }
    }
}

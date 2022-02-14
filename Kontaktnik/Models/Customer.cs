using System.ComponentModel.DataAnnotations;

namespace Kontaktnik.Models
{
    public class Customer
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        [Range(10000000, 99999999)]
        public int TaxNumber { get; set; }

     
        public IEnumerable<ContactDetail> ContactDetails { get; set; } 
    }
}

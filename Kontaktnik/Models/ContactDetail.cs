using System.ComponentModel.DataAnnotations;

namespace Kontaktnik.Models
{
    public class ContactDetail
    {
        [Key]
        public int Id { get; init; }
        [Required]
        public string DataValue { get; set; }

        public Guid DataTypeId { get; set; }
        public ContactType ContactType { get; set; }

        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}

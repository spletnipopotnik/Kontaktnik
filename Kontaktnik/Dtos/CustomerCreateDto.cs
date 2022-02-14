using System.ComponentModel.DataAnnotations;

namespace Kontaktnik.Dtos
{
    public class CustomerCreateDto
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        
       [Range(10000000,99999999)]
        public int TaxNumber { get; set; }
    }
}

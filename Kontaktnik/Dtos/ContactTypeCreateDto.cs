using System.ComponentModel.DataAnnotations;

namespace Kontaktnik.Dtos
{
    public class ContactTypeCreateDto
    {
        
        [Required]
        [MaxLength(50)]
        public string ContactTypeName { get; set; }
    }
}

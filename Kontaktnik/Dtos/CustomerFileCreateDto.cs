using System.ComponentModel.DataAnnotations;

namespace Kontaktnik.Dtos
{
    public class CustomerFileCreateDto
    {
        [MaxLength(100)]
        public string FileDescription { get; set; }
       // [MaxLength]
        //public IFormFile File { get; set; }
        [Required]
        public Guid CustomerId { get; set; }
    }
}

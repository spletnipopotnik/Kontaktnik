using System.ComponentModel.DataAnnotations;

namespace Kontaktnik.Models
{
    public class CustomerFile
    {
        [Key]
        public int FileId { get; set; }
        [MaxLength(100)]
        public string FileName { get; set; }

        [MaxLength(100)]
        public string FileType { get; set; }
        [MaxLength(100)]
        public string FileDescription { get; set; }
        [MaxLength]
        public byte[] FileData { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}

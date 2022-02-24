namespace Kontaktnik.Dtos
{
    public class CustomerFileReadDto
    {
        public int FileId { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string FileDescription { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid CustomerId { get; set; }
    }
}

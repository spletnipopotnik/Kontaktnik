namespace Kontaktnik.Dtos
{
    public class CustomerReadDto
    {
        
        public Guid Id { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public int TaxNumber { get; set; }
    }
}

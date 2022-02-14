using Kontaktnik.Models;
namespace Kontaktnik.Dtos
{
    public class CustomerDetailsDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int TaxNumber { get; set; }
        public IEnumerable<CustomerContactsDto> ContactDetails { get; set; }
    }
}

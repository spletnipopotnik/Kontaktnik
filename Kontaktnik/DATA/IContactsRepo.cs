using Kontaktnik.Dtos;
using Kontaktnik.Models;

namespace Kontaktnik.DATA
{
    public interface IContactsRepo
    {
        Task<bool> SaveChanges();
        Task<IEnumerable<ContactTypeReadDto>> GetAllContactTypes();
        Task<IEnumerable<string>> GetFilterContacts();
        Task<IEnumerable<CustomerContactsDto>> GetAllCustomerContacts(Guid id);
        Task<ContactType> GetContactTypeById(int id);
        Task<ContactDetail> CreateNewCustomerContact(ContactDetail contactdetails);
        Task<ContactType> CreateNewContactType(ContactType contacttype);


    }
}

using Kontaktnik.Dtos;
using Kontaktnik.Models;
using Microsoft.EntityFrameworkCore;

namespace Kontaktnik.DATA
{
    public class ContactsRepo : IContactsRepo
    {
        private KontaktnikDbContext _context;

        public ContactsRepo(KontaktnikDbContext context)
        {
            _context = context;
        }
        public  void CreateCustomerContact(ContactDetail contactdetails)
        {
            throw new NotImplementedException();
        }

        public async Task<ContactType> CreateNewContactType(ContactType contacttype)
        {           
           var result = await _context.ContactTypes.AddAsync(contacttype);
            return result.Entity;
        }
        //ustvari vse parametre za filtriranje po kontaktnih podatkih
        public async Task<IEnumerable<string>> GetFilterContacts()
        {
            List<string> firstcontacts = new List<string> { "Vse", "Ime", "Priimek", "Davčna številka" };
            var contacts = await _context.ContactTypes
                .Select(c => c.ContactTypeName)
                .ToListAsync();
            var allcontacts = firstcontacts.Concat(contacts).ToList(); // 
            return allcontacts;
        }

        public async Task<IEnumerable<ContactTypeReadDto>> GetAllContactTypes()
        {
            var allcontacts = await _context.ContactTypes
                .Select(c => new ContactTypeReadDto
                {
                    Id = c.Id,
                    ContactTypeName = c.ContactTypeName
                })
                .ToListAsync();
            return allcontacts;
        }
        // vrne vse kontaktne podatke ( tip in vrednost) za stranko z določenim id-jem
        public async Task<IEnumerable<CustomerContactsDto>> GetAllCustomerContacts(Guid id)
        {
            var allContacts = await _context.Contacts
                .Include(x => x.ContactType)
                .Where(x => x.CustomerId == id)
                .Select(x => new CustomerContactsDto
                {
                    ContactTypeName = x.ContactType.ContactTypeName,
                    ContactValue = x.DataValue
                })
                .ToListAsync();
            return allContacts;
        }

        public async Task<ContactType> GetContactTypeById(int id)
        {
            var contact = await _context.ContactTypes.SingleOrDefaultAsync(x => x.Id == id);
            return contact;
          
        }

        

        public Task<IEnumerable<ContactType>> GetSearchContacts()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SaveChanges()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }
    }
}

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
       
        // vpiše v bazo  nov tip kontaktnih podatkov
        public async Task<ContactType> CreateNewContactType(ContactType contacttype)
        {           
           var result = await _context.ContactTypes.AddAsync(contacttype);
            return result.Entity;
        }

        // vpiše v bazo  nov  kontaktni podatk stranke
        public async Task<ContactDetail> CreateNewCustomerContact(ContactDetail contactdetails)
        {
            var result = await _context.Contacts.AddAsync(contactdetails);
            return result.Entity;
        }

        //ustvari in vrne vse parametre za filtriranje po kontaktnih podatkih
        public async Task<IEnumerable<string>> GetFilterContacts()
        {
            List<string> firstcontacts = new List<string> { "Vse", "Ime", "Priimek", "Davčna številka" };
            var contacts = await _context.ContactTypes
                .Select(c => c.ContactTypeName)
                .ToListAsync();
            var allcontacts = firstcontacts.Concat(contacts).ToList(); // 
            return allcontacts;
        }

        //vrne seznam tipov kontaktnih podatkov
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
                    ContactTypeId = x.ContactTypeId,
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

        public async Task<bool> SaveChanges()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }

       
    }
}

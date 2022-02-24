using Kontaktnik.Dtos;
using Kontaktnik.Models;
using Microsoft.EntityFrameworkCore;

namespace Kontaktnik.DATA
{
    public class CustomerContactRepo : ICustomerContactRepo
    {
        private KontaktnikDbContext _context;
        private IContactsRepo _contactsrepo;

        public CustomerContactRepo(KontaktnikDbContext context, IContactsRepo contactsrepo)
        {
            _context = context;
            _contactsrepo = contactsrepo;
        }

        

        //vrne stranko  glede na njen Id
        public async Task<Customer> GetCustomerById(Guid id)
        {
            var customer = await _context.Customers.SingleOrDefaultAsync(x => x.Id == id);

            return customer;           
        }
        // vrne stranko gledee na njeno davčno številko
        public async Task<Customer> GetCustomerByTax(int tax)
        {
            var customer = await _context.Customers.SingleOrDefaultAsync(x => x.TaxNumber == tax);

            return customer;
        }
        // vrne saznam vseh strank
        public async Task<IEnumerable<CustomerDetailsDto>> GetAllCustomers()
        {
           var allcustomers =  _context.Customers.Select( x => new CustomerDetailsDto
           {
                 Id = x.Id,
                 LastName = x.LastName,
                 FirstName = x.FirstName,
                 TaxNumber = x.TaxNumber,
                 ContactDetails =  _context.Contacts
                                        .Include(y => y.ContactType)
                                        .Where(y => y.CustomerId == x.Id)
                                        .Select(y => new CustomerContactsDto
                                        {
                                            ContactTypeId = y.ContactTypeId,
                                            ContactTypeName = y.ContactType.ContactTypeName,
                                            ContactValue = y.DataValue
                                        })
                                        .ToList()
             });  
           
            return await allcustomers.ToListAsync();
        }
       
        public async Task<IEnumerable<CustomerReadDto>> GetFilteredCustomers(FilterItem filter)
        {
              IQueryable<CustomerReadDto> allcustomers = _context.Customers.Select(x => new CustomerReadDto
              {
                  Id = x.Id,
                  LastName = x.LastName,
                  FirstName = x.FirstName,
                  TaxNumber = x.TaxNumber
              });

            if (filter != null)
            {
                // filtrira po izbranem podatku
                switch(filter.ContactTypeName)
                {
                    case "Ime":
                        allcustomers = allcustomers.Where(c => c.FirstName.Contains(filter.FilterValue));
                        break;
                    case "Priimek":
                        allcustomers = allcustomers.Where(c => c.LastName.Contains(filter.FilterValue));
                        break;
                    case "Davčna številka":
                        int number = 0;
                        bool isParsable = Int32.TryParse(filter.FilterValue, out number);
                        if (isParsable)
                            allcustomers = allcustomers.Where(c => c.TaxNumber == number);
                        break;
                    default:
                        if (filter.ContactTypeId > 0)
                        {
                            var filteredCustomers = (
                            from cd in _context.Contacts
                            from con in allcustomers

                            where cd.ContactTypeId == filter.ContactTypeId
                            where cd.DataValue.Contains(filter.FilterValue)
                            where con.Id == cd.CustomerId
                            select new CustomerReadDto
                            {
                                Id = con.Id,
                                FirstName = con.FirstName,
                                LastName = con.LastName,
                                TaxNumber = con.TaxNumber
                            }
                            ).ToList();
                            return filteredCustomers;
                        }
                        break;
                }    

                if (filter.ContactTypeName == "Ime")
                {
                    
                }

                if (filter.ContactTypeName == "Priimek")
                {
                    
                }

                if (filter.ContactTypeName == "Davčna Številka")
                {
                   
                }

                // filtrira če je id večji od nič
                if (filter.ContactTypeId > 0)
                {
                   
                }

                
            }
            return await allcustomers.ToListAsync();
        }

        // commit v bazo
        public async Task<bool> SaveChanges()
        {
           
           return ( await _context.SaveChangesAsync() >= 0);

        }
        // nova stranka
        public async Task<Customer> CreateCustomer(Customer customer)
        {
            
            var result = await _context.Customers.AddAsync(customer);
            return result.Entity;
        }
        public async Task<Customer> UpdateCustomer(Customer updatedCustomer, List<CustomerContactsDto> updatedContacts)
        {
            var customer = await _context.Customers.FindAsync(updatedCustomer.Id);
            var neki = await GetCustomerById(updatedCustomer.Id);
            if (customer != null)
            {
                customer.FirstName = updatedCustomer.FirstName;
                customer.LastName = updatedCustomer.LastName;
                customer.TaxNumber = updatedCustomer.TaxNumber;
                // Če seznam ni prazen  posodobi vnešene podatke
                if (updatedContacts != null)
                {
                    //naredi seznam starih kontaktov
                    var oldContacts = await _context.Contacts.Where(x => x.CustomerId == updatedCustomer.Id).ToListAsync();
                    foreach (ContactDetail oldContact in oldContacts)
                    {
                        var contactToUpdate = updatedContacts.FirstOrDefault(x => x.ContactTypeId == oldContact.ContactTypeId);
                        var contact = await _context.Contacts.SingleOrDefaultAsync(x => x.Id == oldContact.Id);

                        // če tip starega kontakta obstaja med novimi ga posodobi, drugače ga izbriše
                        if (contactToUpdate != null)
                        {
                            contact.DataValue = contactToUpdate.ContactValue;
                            updatedContacts.Remove(contactToUpdate); //iz novih kontaktov izbriše tistega, ki ga je že dodal
                        }
                        else
                        {
                            _context.Contacts.Remove(contact);
                        }
    ;
                    }
                    //doda vse nove kontakte
                    foreach (CustomerContactsDto newUpdateContact in updatedContacts)
                    {
                        // če tip id tipa kontakta nedefiniran  pomeni, da še ne obstaja zato ustvari novega
                        int typeId = newUpdateContact.ContactTypeId;
                        if (newUpdateContact.ContactTypeId == null)
                        {
                            var newType = new ContactType
                            {
                                ContactTypeName = newUpdateContact.ContactTypeName
                            };
                            await _context.ContactTypes.AddAsync(newType);
                            await _context.SaveChangesAsync();
                            typeId = newType.Id;
                        }
                        var newContact = new ContactDetail
                        {
                            DataValue = newUpdateContact.ContactValue,
                            CustomerId = updatedCustomer.Id,
                            ContactTypeId = typeId

                        };
                        _context.Contacts.Add(newContact);
                    }
                }
            }
            return updatedCustomer;
        }
        // izbris stranke
        public async Task<bool> DeleteCustomer(Guid id)
        {
            try
            {
                var customerToDelete = await _context.Customers.SingleOrDefaultAsync(x => x.Id == id);
                _context.Customers.Remove(customerToDelete);
                return true;
            }
           catch (Exception)
            {

            }
            return false;
           
        }

       
    }
}

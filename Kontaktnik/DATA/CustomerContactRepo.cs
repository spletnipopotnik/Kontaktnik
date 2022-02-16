using Kontaktnik.Dtos;
using Kontaktnik.Models;
using Microsoft.EntityFrameworkCore;

namespace Kontaktnik.DATA
{
    public class CustomerContactRepo : ICustomerContactRepo
    {
        private KontaktnikDbContext _context;

        public CustomerContactRepo(KontaktnikDbContext context)
        {
            _context = context;
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
        public async Task<IEnumerable<CustomerReadDto>> GetAllCustomers()
        {
           var allcustomers = await _context.Customers.Select(x => new CustomerReadDto
             {
                 Id = x.Id,
                 LastName = x.LastName,
                 FirstName = x.FirstName,
                 TaxNumber = x.TaxNumber
             }).ToListAsync();  
           
            return allcustomers;
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
            if(customer == null)
            {
                throw new ArgumentNullException(nameof(customer));
            }
            var result = await _context.Customers.AddAsync(customer);
            return result.Entity;
        }

       
    }
}

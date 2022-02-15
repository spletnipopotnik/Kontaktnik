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
            })
                .ToListAsync();
           
            return allcustomers;
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

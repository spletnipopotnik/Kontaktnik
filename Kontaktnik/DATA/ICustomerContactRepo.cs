
using Kontaktnik.Dtos;
using Kontaktnik.Models;

namespace Kontaktnik.DATA
{
    public interface ICustomerContactRepo
    {
        Task<bool> SaveChanges();
        Task<IEnumerable<CustomerReadDto>> GetAllCustomers();
        Task<Customer> GetCustomerById (Guid id);
        Task<Customer> GetCustomerByTax (int tax);
        Task<Customer> CreateCustomer (Customer customer);
      
    }
}

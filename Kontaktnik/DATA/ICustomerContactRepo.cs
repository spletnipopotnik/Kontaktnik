
using Kontaktnik.Dtos;
using Kontaktnik.Models;

namespace Kontaktnik.DATA
{
    public interface ICustomerContactRepo
    {
        Task<bool> SaveChanges();
        Task<IEnumerable<CustomerDetailsDto>> GetAllCustomers();
        Task<IEnumerable<CustomerReadDto>> GetFilteredCustomers(FilterItem filter);
        Task<Customer> GetCustomerById (Guid id);
        Task<Customer> GetCustomerByTax (int tax);
        Task<Customer> CreateCustomer (Customer customer);
        Task<Customer> UpdateCustomer(Customer updatedCustomer, List<CustomerContactsDto> updatedContacts);
        Task<bool> DeleteCustomer(Guid id);
      
    }
}

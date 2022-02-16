using System.Text.Json;
using System.Text.Json.Serialization;
using Kontaktnik.DATA;
using Kontaktnik.Dtos;
using Kontaktnik.Models;
using Microsoft.AspNetCore.Mvc;

namespace Kontaktnik.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerContactRepo _customerrepo;
        private readonly IContactsRepo _contactsrepo;

        public CustomersController(ICustomerContactRepo customerrepo, IContactsRepo contactsrepo)
        {
            _customerrepo = customerrepo;
            _contactsrepo = contactsrepo;
        }

       

        //api/customers -izpis vseh strank
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerReadDto>>> GetAllCustomers()
        {
            try
            {
               
                var allCustomers = await _customerrepo.GetAllCustomers();
                
                return Ok(allCustomers);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                         "Error retrieving data from the database");
            }
           
        }
        [HttpGet("{filter}")]
        public async Task<ActionResult<IEnumerable<CustomerReadDto>>> GetFilteredCustomers(string filter)
        {
             try
             {
                
                if (filter != null)     
                 {
                    FilterItem filterData = new FilterItem();   
                    try
                    {
                        filterData = JsonSerializer.Deserialize<FilterItem>(filter);
                    }
                    catch(Exception)
                    {
                        return StatusCode(StatusCodes.Status400BadRequest);
                    }
                    var allCustomers = await _customerrepo.GetFilteredCustomers(filterData);

                 return Ok(allCustomers);
                }
                return StatusCode(StatusCodes.Status204NoContent);
            }
             catch (Exception)
             {
                 return StatusCode(StatusCodes.Status500InternalServerError,
                                          "Error retrieving data from the database");
             }
            return Ok();
        }
        //api/customers/{id} - Izpis stranke glede na njegov {id}
        [HttpGet("{id:Guid}", Name = "GetCustomerById")]
        public async Task<ActionResult<CustomerDetailsDto>> GetCustomerById(Guid id)
        {
            try
            {
                var customer = await _customerrepo.GetCustomerById(id);
                if (customer != null)
                {
                    var contactDetails = await _contactsrepo.GetAllCustomerContacts(id);
                   
                    
                    var customerDetailsDto = new CustomerDetailsDto
                    {
                        Id = customer.Id,
                        FirstName = customer.FirstName,
                        LastName = customer.LastName,
                        TaxNumber = customer.TaxNumber,
                        ContactDetails =  contactDetails

                    };

                    return Ok(customerDetailsDto);
                }
                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                          "Error retrieving data from the database");
            }
        }
        // Vstvari novo stranko
       /* [HttpPost]
        public async Task <ActionResult<CustomerReadDto>> CreateCustomer(CustomerCreateDto customer)
        {
            try
            {
                if (customer == null)
                {
                    return BadRequest();
                }

                var cust = await _customerrepo.GetCustomerByTax(customer.TaxNumber);
                if (cust != null)
                {
                    ModelState.AddModelError("taxNumber", "Davčna številka že obstaja.");
                    return BadRequest(ModelState);
                }
                 
                //doda novo stranko
                var newCustomer = new Customer
                {
                    Id = Guid.NewGuid(),
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    TaxNumber = customer.TaxNumber
                };
                await _customerrepo.CreateCustomer(newCustomer);
                

                //doda njegove kontaktne podatke, če so bili vnešeni zraven prvega vpisa
                if (customer.ContactDetails != null)
                {
                    foreach (CustomerContactsDto dto in customer.ContactDetails)
                    {
                        var newContact = new ContactDetail
                        {
                            DataValue = dto.ContactValue,
                            CustomerId = newCustomer.Id,
                            ContactTypeId = dto.ContactTypeId
                        };
                        await _contactsrepo.CreateNewCustomerContact(newContact);
                    }
                }

                await _contactsrepo.SaveChanges();

                return CreatedAtRoute(nameof(GetCustomerById), new { Id = newCustomer.Id }, customer);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                         "Error retrieving data from the database");
            }
        }   */
    }
}

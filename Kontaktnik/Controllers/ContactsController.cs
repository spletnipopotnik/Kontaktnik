using Kontaktnik.DATA;
using Kontaktnik.Dtos;
using Kontaktnik.Models;
using Microsoft.AspNetCore.Mvc;

namespace Kontaktnik.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly ICustomerContactRepo _customerrepo;
        private readonly IContactsRepo _contactsrepo;

        public ContactsController(ICustomerContactRepo customerrepo, IContactsRepo contactsrepo)
        {
            _customerrepo = customerrepo;
            _contactsrepo = contactsrepo;
        }

        



        [HttpGet("{id:int}", Name = "GetContactById")]
        public async Task<ActionResult<string>> GetContactById(int id)
        {
            try
            {
                var contactType = await _contactsrepo.GetContactTypeById(id);
                if (contactType != null)
                {
                    var contactDetailsDto = new ContactTypeReadDto
                    {
                        Id = contactType.Id,
                        ContactTypeName = contactType.ContactTypeName
                    };

                    return Ok(contactDetailsDto);
                }
                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                      "Error retrieving data from the database");
            }
        }

        [HttpPost]
        public async Task<ActionResult<CustomerContactWriteDto>> CreateNewCustomerContact(CustomerContactWriteDto customercontact)
        {
            try
            {
                if (customercontact != null)
                {
                    var ctype = new ContactDetail
                    {
                        ContactTypeId = customercontact.ContactTypeId,
                        CustomerId = customercontact.CustomerId,
                        DataValue = customercontact.DataValue
                    };
                    await _contactsrepo.CreateNewCustomerContact(ctype);
                    await _contactsrepo.SaveChanges();

                    return CreatedAtRoute(nameof(GetContactById), new { Id = ctype.Id }, ctype);
                }
                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                     "Error retrieving data from the database");
            }
        }
    }
}

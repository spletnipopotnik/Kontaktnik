using Kontaktnik.DATA;
using Kontaktnik.Dtos;
using Kontaktnik.Models;
using Microsoft.AspNetCore.Mvc;

namespace Kontaktnik.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactTypesController : ControllerBase
    {
        private readonly ICustomerContactRepo _customerrepo;
        private readonly IContactsRepo _contactsrepo;

        public ContactTypesController(ICustomerContactRepo customerrepo, IContactsRepo contactsrepo)
        {
            _customerrepo = customerrepo;
            _contactsrepo = contactsrepo;
        }
        //api/contacttypes/filter  - izpiše vse možnosti filtriranja strank

        [HttpGet("{filter}")]
        public async Task<ActionResult<IEnumerable<ContactTypeReadDto>>> FilterContacts()
        {
            try
            {
                var filtercontacts = await _contactsrepo.GetFilterContacts();
                if (filtercontacts != null)
                {
                    return Ok(filtercontacts);
                }
                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                                      "Error retrieving data from the database");
            }
        }
        //api/contacttype - izpiše vse tipe kontaktnih podatkov v bazi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContactTypeReadDto>>> GetAllContactTypes()
        {
            try
            {
                var allcontacts = await _contactsrepo.GetAllContactTypes();
                return Ok(allcontacts);
            }
           catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                       "Error retrieving data from the database");
            }
        }


        //api/contacttype/{id} - izpis tipa kontaktnih podatkov glede na njegov id
        [HttpGet("{id:int}", Name = "GetContactTypeById")]
        public async Task<ActionResult<string>> GetContactTypeById(int id)
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
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                      "Error retrieving data from the database");
            }
        }
        // ustvari nov tip kontaktnih podatkov
        [HttpPost]
        public async Task<ActionResult<ContactTypeReadDto>> CreateContactType(ContactTypeCreateDto contacttype)
        {
            try
            {
                if (contacttype != null)
                {
                    var ctype = new ContactType
                    {
                        ContactTypeName = contacttype.ContactTypeName
                    };
                    await _contactsrepo.CreateNewContactType(ctype);
                    await _contactsrepo.SaveChanges();

                    return CreatedAtRoute(nameof(GetContactTypeById), new { Id = ctype.Id }, ctype);
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

using Kontaktnik.DATA;
using Kontaktnik.DATA.FileManager;
using Kontaktnik.Dtos;
using Kontaktnik.Models;
using Microsoft.AspNetCore.Mvc;

namespace Kontaktnik.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerFilesController : ControllerBase
    {
        private ICustomerContactRepo _customerrepo;
        private IFileManager _fileManager;

        public CustomerFilesController( IFileManager filemanager, ICustomerContactRepo customerrepo)
        {
            _customerrepo = customerrepo;
            _fileManager = filemanager;
        }

        //vrne opis dokumenta stranke
        [HttpGet("{fid:int}", Name = "GetCustomerFileDecriptionById")]
        public async Task<ActionResult<CustomerFileReadDto>> GetCustomerFileDecriptionById(int fid)
        {
            try
            {
                var file = _fileManager.GetCustomerFileDescriptionById(fid);
                if (file == null)
                {
                    return NotFound();
                }
                return Ok(file);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                                                    "Prišlo je do napake pri povezavi s strežnikom.");
            }
           
           
        }
        //izpis vseh dokumentov stranke
        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<IEnumerable<CustomerFileReadDto>>> GetAllCustomerFileNames(Guid id)
        {
            try
            {
                var customer = _customerrepo.GetCustomerById(id);
                if (customer == null)
                {
                    return NotFound();
                }
                var customerFiles = _fileManager.GetAllCustomerFileNames(id);
                return Ok(customerFiles);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                                     "Prišlo je do napake pri povezavi s strežnikom.");
            }
            
        }
        //doda novo stranko
        [HttpPost]
        public async Task<ActionResult> UploadFile(CustomerFileCreateDto filesDto, IFormFile file)
        {
          
            try
            {
                if (filesDto != null && file != null)
                {
                    if (file.Length > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var fileExtension = Path.GetExtension(fileName);

                        var objFiles = new CustomerFile()
                        {
                            FileId = 0,
                            FileName = fileName,
                            FileType = fileExtension,
                            FileDescription = filesDto.FileDescription,
                            CreatedOn = DateTime.Now,
                            CustomerId = filesDto.CustomerId
                        };
                        using (var target = new MemoryStream())
                        {
                            file.CopyTo(target);
                            objFiles.FileData = target.ToArray();
                        }
                        await _fileManager.SaveFile(objFiles);
                        await _fileManager.SaveChanges();
                        var filedes = new CustomerFileReadDto
                        {
                            CustomerId = objFiles.CustomerId,
                            FileId = objFiles.FileId,
                            FileDescription = filesDto.FileDescription,
                            FileName = objFiles.FileName,
                            FileType = objFiles.FileType,
                            CreatedOn = objFiles.CreatedOn,
                        };
                        return CreatedAtRoute(nameof(GetCustomerFileDecriptionById), new { Id = objFiles.FileId }, filedes);
                    }
                    return StatusCode(StatusCodes.Status400BadRequest);
                }
                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                       "Prišlo je do napake pri povezavi s strežnikom.");
            }        

        }
        //izbris datoteke
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteFileById(int id)
        {
            try
            {
                var file = await _fileManager.GetCustomerFileDescriptionById(id);
                if (file == null)
                {
                    return NotFound($"Datoteka z id = {id} ni najdena.");
                }
                var success =  _fileManager.DeleteFileById(id);
                await _fileManager.SaveChanges();
                if (success)
                {
                    return Ok($"Datoteka z id = {id} je bila uspešno izbrisana.");
                }
                return StatusCode(StatusCodes.Status500InternalServerError,
                                         "Prišlo je do napake pri brisanju datoteke.");
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                       "Prišlo je do napake pri brisanju stranke.");
            }
        }
    }
}

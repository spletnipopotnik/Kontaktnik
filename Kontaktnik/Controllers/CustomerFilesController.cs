using Kontaktnik.DATA;
using Kontaktnik.DATA.FileManager;
using Kontaktnik.Dtos;
using Kontaktnik.Models;
using Microsoft.AspNetCore.Mvc;

namespace Kontaktnik.Controllers
{
    public class CustomerFilesController : ControllerBase
    {
        private IFileManager _fileManager;

        public CustomerFilesController( IFileManager filemanager)
        {
           
            _fileManager = filemanager;
        }
        [HttpGet("{fid:int}")]
        public async Task<ActionResult<CustomerFileReadDto>> GetCustomerFileDescriptionById(int fid)
        {
           /* var file = _fileManager.GetCustomerFileDescriptionById(fid);
            if(file != null)
            {

            }*/
            return Ok();
        }
        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<IEnumerable<CustomerFileReadDto>>> GetAllCustomerFileNames(Guid id)
        {
            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult> UploadFile(CustomerFileCreateDto filesDto)
        {
            try
            {
                if (filesDto != null)
                {
                    if (filesDto.File.Length > 0)
                    {
                        var fileName = Path.GetFileName(filesDto.File.FileName);
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
                            filesDto.File.CopyTo(target);
                            objFiles.FileData = target.ToArray();
                        }
                        await _fileManager.SaveFile(objFiles);
                        await _fileManager.SaveChanges();
                        return CreatedAtRoute(nameof(GetCustomerFileById), new { Id = objFiles.FileId }, fileName);
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
    }
}

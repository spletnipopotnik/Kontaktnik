using Kontaktnik.Dtos;
using Kontaktnik.Models;

namespace Kontaktnik.DATA.FileManager
{
    public class FileManager : IFileManager
    {
        private KontaktnikDbContext _context;
        private string _imagePath;

        public FileManager(KontaktnikDbContext context)
        {
            _context = context;
         
        }       
      
        public async Task<CustomerFile> SaveFile(CustomerFile file)
        {
            var result = await _context.CustomerFiles.AddAsync(file);                        
            return result.Entity;            
        }

        public async Task<bool> SaveChanges()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }

        public Task<CustomerFileReadDto> GetCustomerFileDescriptionById(int fid)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<CustomerFileReadDto>> GetAllCustomerFileNames(Guid id)
        {
            var  allfiles = _context.CustomerFiles
                .Where(x => x.CustomerId == id)
                .Select(x => new CustomerFileReadDto
                {
                    CustomerId = x.CustomerId,
                    FileName = x.FileName,
                    FileDescription = x.FileDescription,
                    FileType = x.FileType,
                    FileId = x.FileId
                }).ToList();
            return allfiles;

        }

        public  bool DeleteFileById(int id)
        {
            try
            {
                var fileToDelete =  _context.CustomerFiles.SingleOrDefault(x => x.FileId == id);
                _context.CustomerFiles.Remove(fileToDelete);
                return true;
            }
            catch (Exception)
            {

            }
            return false;
        }
    }
}

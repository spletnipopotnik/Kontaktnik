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
        public bool RemoveCustomerFile(string filedelete)
        {
            try
            {
                var file = Path.Combine(_imagePath, filedelete);
                if (File.Exists(file))
                {
                    File.Delete(file);                   
                }
                return true;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
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

        public Task<IEnumerable<CustomerFileReadDto>> GetAllCustomerFileNames(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}

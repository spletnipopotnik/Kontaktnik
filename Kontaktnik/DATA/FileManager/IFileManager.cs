using Kontaktnik.Dtos;
using Kontaktnik.Models;

namespace Kontaktnik.DATA.FileManager
{
    public interface IFileManager
    {
        Task<bool> SaveChanges();
        Task<CustomerFile> SaveFile(CustomerFile file);
        Task<CustomerFileReadDto> GetCustomerFileDescriptionById(int fid);
        Task<IEnumerable<CustomerFileReadDto>> GetAllCustomerFileNames(Guid id);
        bool DeleteFileById(int id);



    }
}

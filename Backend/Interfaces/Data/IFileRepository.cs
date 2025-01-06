using Backend.Dtos.File;
using File = Backend.Models.File;

namespace Backend.Interfaces
{
    public interface IFileRepository
    {
        Task<File> CreateAsync(File file);
        Task<bool> DeleteAsync(int id);
        Task<File?> UpdateAsync(int id, RequestFileDto fileDto, string userId);
        Task<File?> GetByIdAsync(int id);
        Task<List<File>> GetAllAsync(int page);
        Task<int> CountAllAsync();
    }
}

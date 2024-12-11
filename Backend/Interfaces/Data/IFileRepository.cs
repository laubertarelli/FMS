using Backend.Dtos.File;
using File = Backend.Models.File;

namespace Backend.Interfaces
{
    public interface IFileRepository
    {
        Task<File> CreateAsync(File file);
        Task<File?> DeleteAsync(int id);
        Task<File?> UpdateAsync(int id, RequestFileDto file, string userId);
        Task<File?> GetByIdAsync(int id);
        Task<List<File>> GetAllAsync(int page);
        Task<int> CountAllAsync();
    }
}

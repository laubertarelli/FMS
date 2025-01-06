using Backend.Dtos.File;
using File = Backend.Models.File;

namespace Backend.Interfaces.Services
{
    public interface IFileService
    {
        Task<File> CreateAsync(RequestFileDto fileDto, string userId);
        Task<bool> DeleteAsync(int id);
        Task<FileDto?> UpdateAsync(int id, RequestFileDto file, string userId);
        Task<FileDto?> GetByIdAsync(int id);
        Task<List<FileDto>> GetAllAsync(int page);
        Task<int> CountAllAsync();
    }
}

using Backend.Dtos.File;
using Backend.Enums;
using Backend.Interfaces;
using Backend.Interfaces.Services;
using Backend.Mappers;
using File = Backend.Models.File;

namespace Backend.Services
{
    public class FileService(IFileRepository repo) : IFileService
    {
        public async Task<int> CountAllAsync()
        {
            return await repo.CountAllAsync();
        }

        public async Task<File> CreateAsync(RequestFileDto fileDto, string userId)
        {
            var fileModel = fileDto.ToFileFromRequestDto(userId);
            return await repo.CreateAsync(fileModel);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await repo.DeleteAsync(id);
        }

        public async Task<List<FileDto>> GetAllAsync(int page)
        {
            var files = await repo.GetAllAsync(page);
            return files.Select(f => f.ToFileDto()).ToList();
        }

        public async Task<FileDto?> GetByIdAsync(int id)
        {
            var file = await repo.GetByIdAsync(id);
            return file?.ToFileDto();
        }

        public List<FileStateDto> GetFileStates()
        {
            return ((FileState[])Enum.GetValues(typeof(FileState)))
                .Select(e => new FileStateDto() { Value = (int)e, Name = e.ToString() })
                .ToList();
        }

        public async Task<List<int>> GetIdsAsync()
        {
            return await repo.GetIdsAsync();
        }

        public async Task<FileDto?> UpdateAsync(int id, RequestFileDto fileDto, string userId)
        {
            var file = await repo.UpdateAsync(id, fileDto, userId);
            return file?.ToFileDto();
        }
    }
}

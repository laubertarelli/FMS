using Backend.Dtos.File;
using Backend.Interfaces;
using Microsoft.EntityFrameworkCore;
using File = Backend.Models.File;

namespace Backend.Data
{
    public class FileRepository(FmsContext context) : IFileRepository
    {
        public async Task<File> CreateAsync(File file)
        {
            await context.Files.AddAsync(file);
            await context.SaveChangesAsync();
            return file;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var file = await context.Files.FirstOrDefaultAsync(f => f.Id == id);
            if (file is null)
            {
                return false;
            }
            context.Files.Remove(file);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<File?> UpdateAsync(int id, RequestFileDto fileDto, string userId)
        {
            var existingFile = await context.Files.FirstOrDefaultAsync(f => f.Id == id);
            if (existingFile is null)
            {
                return null;
            }

            existingFile.Cover = fileDto.Cover;
            existingFile.State = fileDto.State;
            existingFile.UserId = userId;

            await context.SaveChangesAsync();
            return existingFile!;
        }

        public async Task<File?> GetByIdAsync(int id)
        {
            return await context.Files
                .Include(f => f.Procedures)
                .Include(f => f.User)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<List<File>> GetAllAsync(int page)
        {
            return await context.Files
                .Include(f => f.Procedures)
                .Include(f => f.User)
                .Skip((page - 1) * 5)
                .Take(5)
                .ToListAsync();
        }

        public async Task<List<int>> GetIdsAsync()
        {
            return await context.Files
                .Select(f => f.Id)
                .ToListAsync();
        }

        public async Task<int> CountAllAsync()
        {
            return await context.Files.CountAsync();
        }
    }
}

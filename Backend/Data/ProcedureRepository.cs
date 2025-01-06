using Backend.Dtos.Procedure;
using Backend.Enums;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data
{
    public class ProcedureRepository(FmsContext context) : IProcedureRepository
    {
        public async Task<Procedure> CreateAsync(Procedure procedure)
        {
            await context.Procedures.AddAsync(procedure);
            await context.SaveChangesAsync();
            return procedure;
        }

        public async Task<Procedure?> DeleteAsync(int id)
        {
            var procedure = await context.Procedures.FirstOrDefaultAsync(p => p.Id == id);
            if (procedure is null)
            {
                return null;
            }
            context.Procedures.Remove(procedure);
            await context.SaveChangesAsync();
            return procedure;
        }

        public async Task<Procedure?> UpdateAsync(int id, RequestProcedureDto procedureDto, string userId)
        {
            var existingProcedure = await context.Procedures.FirstOrDefaultAsync(p => p.Id == id);
            if (existingProcedure is null)
            {
                return null;
            }

            existingProcedure.Content = procedureDto.Content;
            existingProcedure.Label = procedureDto.Label;
            existingProcedure.FileId = procedureDto.FileId;
            existingProcedure.UserId = userId;

            await context.SaveChangesAsync();
            return existingProcedure;
        }

        public async Task<Procedure?> GetByIdAsync(int id)
        {
            return await context.Procedures.Include(p => p.File).Include(p => p.User).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Procedure>> GetByFileIdAsync(int fileId, int page)
        {
            return await context.Procedures
                .Include(p => p.File).Include(p => p.User)
                .Where(p => p.FileId == fileId)
                .Skip((page - 1) * 5)
                .Take(5)
                .ToListAsync();
        }

        public async Task<List<Procedure>> GetByLabelAsync(ProcedureLabel label, int page)
        {
            return await context.Procedures
                .Include(p => p.File).Include(p => p.User)
                .Where(p => p.Label == label)
                .Skip((page - 1) * 5)
                .Take(5)
                .ToListAsync();
        }

        public async Task<List<Procedure>> GetAllAsync(int page)
        {
            return await context.Procedures
                .Include(p => p.File).Include(p => p.User)
                .Skip((page - 1) * 5)
                .Take(5)
                .ToListAsync();
        }
        public async Task<int> CountAllAsync()
        {
            return await context.Procedures.CountAsync();
        }

        public async Task<int> CountByFileIdAsync(int fileId) // IMPLEMENTAR
        {
            return await context.Procedures
                .Where(p => p.FileId == fileId)
                .CountAsync();
        }

        public async Task<int> CountByLabelAsync(ProcedureLabel label)
        {
            return await context.Procedures
                .Where(p => p.Label == label)
                .CountAsync();
        }
    }
}

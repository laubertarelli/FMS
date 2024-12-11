using Backend.Dtos.Procedure;
using Backend.Enums;
using Backend.Models;

namespace Backend.Interfaces
{
    public interface IProcedureRepository
    {
        Task<Procedure> CreateAsync(Procedure procedure);
        Task<Procedure?> DeleteAsync(int id);
        Task<Procedure?> UpdateAsync(int id, RequestProcedureDto procedureDto, string userId);
        Task<Procedure?> GetByIdAsync(int id);
        Task<List<Procedure>> GetByFileIdAsync(int fileId, int page);
        Task<List<Procedure>> GetByLabelAsync(ProcedureLabel label, int page);
        Task<List<Procedure>> GetAllAsync(int page);
        Task<int> CountAllAsync();
        Task<int> CountByFileIdAsync(int fileId);
        Task<int> CountByLabelAsync(ProcedureLabel label);
    }
}

using Backend.Dtos.Procedure;
using Backend.Enums;
using Backend.Models;

namespace Backend.Interfaces.Services
{
    public interface IProcedureService
    {
        Task<Procedure> CreateAsync(RequestProcedureDto procedure, string userId);
        Task<bool> DeleteAsync(int id, string userId);
        Task<ProcedureDto?> UpdateAsync(int id, RequestProcedureDto procedureDto, string userId);
        Task<ProcedureDto?> GetByIdAsync(int id);
        Task<List<ProcedureDto>> GetByFileIdAsync(int fileId, int page);
        Task<List<ProcedureDto>> GetByLabelAsync(ProcedureLabel label, int page);
        Task<List<ProcedureDto>> GetAllAsync(int page);
        Task<int> CountAllAsync();
        Task<int> CountByFileIdAsync(int fileId);
        Task<int> CountByLabelAsync(ProcedureLabel label);
    }
}

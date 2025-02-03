using Backend.Dtos.Procedure;
using Backend.Enums;
using Backend.Interfaces;
using Backend.Interfaces.Services;
using Backend.Mappers;
using Backend.Models;

namespace Backend.Services
{
    public class ProcedureService(IProcedureRepository repo, IUpdateStateService stateService) : IProcedureService
    {
        public async Task<int> CountAllAsync()
        {
            return await repo.CountAllAsync();
        }

        public async Task<int> CountByFileIdAsync(int fileId)
        {
            return await repo.CountByFileIdAsync(fileId);
        }

        public async Task<int> CountByLabelAsync(ProcedureLabel label)
        {
            return await repo.CountByLabelAsync(label);
        }

        public async Task<Procedure> CreateAsync(RequestProcedureDto procedureDto, string userId)
        {
            var procedureModel = procedureDto.ToProcedureFromRequestDto(userId);
            var result = await repo.CreateAsync(procedureModel);
            await stateService.UpdateState(procedureModel.FileId, userId);
            return result;
        }

        public async Task<bool> DeleteAsync(int id, string userId)
        {
            var procedureModel = await repo.DeleteAsync(id);
            if (procedureModel is not null)
            {
                await stateService.UpdateState(procedureModel.FileId, userId);
                return true;
            }
            return false;
        }

        public async Task<List<ProcedureDto>> GetAllAsync(int page)
        {
            var procedures = await repo.GetAllAsync(page);
            return procedures.Select(p => p.ToProcedureDto()).ToList();
        }

        public async Task<List<ProcedureDto>> GetByFileIdAsync(int fileId, int page)
        {
            var procedures = await repo.GetByFileIdAsync(fileId, page);
            return procedures.Select(p => p.ToProcedureDto()).ToList();
        }

        public async Task<ProcedureDto?> GetByIdAsync(int id)
        {
            var procedure = await repo.GetByIdAsync(id);
            return procedure?.ToProcedureDto();
        }

        public async Task<List<ProcedureDto>> GetByLabelAsync(ProcedureLabel label, int page)
        {
            var procedures = await repo.GetByLabelAsync(label, page);
            return procedures.Select(p => p.ToProcedureDto()).ToList();
        }

        public List<ProcedureLabelDto> GetProcedureLabels()
        {
            return ((ProcedureLabel[])Enum.GetValues(typeof(ProcedureLabel)))
                .Select(e => new ProcedureLabelDto() { Value = (int)e, Name = e.ToString() })
                .ToList();
        }

        public async Task<ProcedureDto?> UpdateAsync(int id, RequestProcedureDto procedureDto, string userId)
        {
            var procedureModel = await repo.UpdateAsync(id, procedureDto, userId);
            if (procedureModel is not null)
            {
                await stateService.UpdateState(procedureModel.FileId, userId);
            }
            return procedureModel?.ToProcedureDto();
        }
    }
}

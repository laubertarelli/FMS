using Backend.Dtos.Procedure;
using Backend.Models;

namespace Backend.Mappers
{
    public static class ProcedureMapper
    {
        public static ProcedureDto ToProcedureDto(this Procedure procedureModel)
        {
            return new ProcedureDto
            {
                Id = procedureModel.Id,
                Content = procedureModel.Content,
                Label = procedureModel.Label.ToString(),
                CreatedOn = procedureModel.CreatedOn,
                UpdatedOn = procedureModel.UpdatedOn,
                FileId = procedureModel.FileId,
                UserFullName = procedureModel.User?.FullName ?? "Not Found"
            };
        }

        public static Procedure ToProcedureFromRequestDto(this RequestProcedureDto procedureDto, string userId)
        {
            return new Procedure
            {
                Content = procedureDto.Content,
                Label = procedureDto.Label,
                FileId = procedureDto.FileId,
                UserId = userId
            };
        }
    }
}

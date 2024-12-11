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
                Content = procedureModel.Content,
                Label = procedureModel.Label,
                CreatedOn = procedureModel.CreatedOn,
                UpdatedOn = procedureModel.UpdatedOn,
                FileId = procedureModel.FileId,
                // If model's user is null, a user with a messagge "User not found" is assigned, located in the FirstName prop
                UserId = procedureModel.UserId
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

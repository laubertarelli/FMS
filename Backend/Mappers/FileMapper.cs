using Backend.Dtos.File;
using File = Backend.Models.File;

namespace Backend.Mappers
{
    public static class FileMapper
    {
        public static FileDto ToFileDto(this File fileModel)
        {
            return new FileDto
            {
                Cover = fileModel.Cover,
                State = fileModel.State,
                CreatedOn = fileModel.CreatedOn,
                UpdatedOn = fileModel.UpdatedOn,
                Procedures = fileModel.Procedures.Select(p => p.ToProcedureDto()).ToList(),
                UserId = fileModel.UserId
            };
        }

        public static File ToFileFromRequestDto(this RequestFileDto fileDto, string userId)
        {
            return new File
            {
                Cover = fileDto.Cover,
                State = fileDto.State,
                UserId = userId
            };
        }
    }
}

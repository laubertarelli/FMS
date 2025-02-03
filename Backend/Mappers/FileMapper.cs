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
                Id = fileModel.Id,
                Cover = fileModel.Cover,
                State = fileModel.State.ToString(),
                CreatedOn = fileModel.CreatedOn,
                UpdatedOn = fileModel.UpdatedOn,
                Procedures = fileModel.Procedures.Count,
                UserFullName = fileModel.User?.FullName ?? "Not Found"
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

using Backend.Dtos.Procedure;
using Backend.Enums;

namespace Backend.Dtos.File
{
    public class FileDto
    {
        public string Cover { get; set; } = "";
        public FileState State { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public List<ProcedureDto> Procedures { get; set; } = [];
        public string UserId { get; set; } = null!;
    }
}

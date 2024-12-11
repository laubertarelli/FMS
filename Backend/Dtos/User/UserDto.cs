using Backend.Dtos.File;
using Backend.Dtos.Procedure;

namespace Backend.Dtos.User
{
    public class UserDto
    {
        public string Username { get; set; } = "";
        public string Email { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public List<FileDto> Files { get; set; } = [];
        public List<ProcedureDto> Procedures { get; set; } = [];
    }
}

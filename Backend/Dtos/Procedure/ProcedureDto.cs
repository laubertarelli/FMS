using Backend.Enums;

namespace Backend.Dtos.Procedure
{
    public class ProcedureDto
    {
        public string Content { get; set; } = "";
        public ProcedureLabel Label { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int FileId { get; set; }
        public string UserFullName { get; set; } = "";
    }
}

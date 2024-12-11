using Backend.Enums;
using Backend.Interfaces;

namespace Backend.Models
{
    public class Procedure : ITimestampable
    {
        public int Id { get; set; }
        public string Content { get; set; } = "";
        public ProcedureLabel Label { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int FileId { get; set; }
        public File File { get; set; } = null!;
        public string UserId { get; set; } = "";
        public User? User { get; set; }
    }
}

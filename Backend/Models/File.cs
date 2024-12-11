using Backend.Enums;
using Backend.Interfaces;

namespace Backend.Models
{
    public class File : ITimestampable
    {
        public int Id { get; set; }
        public string Cover { get; set; } = "";
        public FileState State { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public virtual List<Procedure> Procedures { get; set; } = [];
        public string UserId { get; set; } = "";
        public virtual User? User { get; set; }
    }
}

using Backend.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Backend.Models
{
    public class User : IdentityUser, ITimestampable // More than one interface?
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public virtual List<File> Files { get; set; } = [];
        public virtual List<Procedure> Procedures { get; set; } = [];
    }
}

using Microsoft.AspNetCore.Identity;

namespace Backend.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public virtual List<File> Files { get; set; } = [];
        public virtual List<Procedure> Procedures { get; set; } = [];

        public string FullName => $"{FirstName} {LastName}";
    }
}

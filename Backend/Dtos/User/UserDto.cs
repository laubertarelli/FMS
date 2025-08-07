using System.Security.Claims;

namespace Backend.Dtos.User
{
    public class UserDto
    {
        public string Id { get; set; } = "";
        public string Username { get; set; } = "";
        public string Email { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public int Files { get; set; }
        public int Procedures { get; set; }
        public List<string> Roles { get; set; } = [];
        public string FullName => $"{FirstName} {LastName}";
    }
}

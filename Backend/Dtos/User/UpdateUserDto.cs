using System.ComponentModel.DataAnnotations;

namespace Backend.Dtos.User
{
    public class UpdateUserDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";
        [Required]
        public string UserName { get; set; } = "";
        [Required]
        public string FirstName { get; set; } = "";
        [Required]
        public string LastName { get; set; } = "";
    }
}

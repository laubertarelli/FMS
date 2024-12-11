using Backend.Enums;
using System.ComponentModel.DataAnnotations;

namespace Backend.Dtos.File
{
    public class RequestFileDto
    {
        [Required]
        [MinLength(1, ErrorMessage = "Cover cannot be empty")]
        public string Cover { get; set; } = "";
        [Required]
        [Range(0, 4)]
        public FileState State { get; set; }
    }
}

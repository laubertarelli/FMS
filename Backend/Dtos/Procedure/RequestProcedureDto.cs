using Backend.Enums;
using System.ComponentModel.DataAnnotations;

namespace Backend.Dtos.Procedure
{
    public class RequestProcedureDto()
    {
        [Required]
        [MinLength(1, ErrorMessage = "Content cannot be empty")]
        public string Content { get; set; } = "";
        [Required]
        [Range(0, 5)]
        public ProcedureLabel Label { get; set; }
        [Required]
        [Range(1, double.MaxValue)]
        public int FileId { get; set; }
    }
}

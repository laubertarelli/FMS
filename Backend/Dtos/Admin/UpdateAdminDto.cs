﻿using System.ComponentModel.DataAnnotations;

namespace Backend.Dtos.Admin
{
    public class UpdateAdminDto
    {
        [Required]
        public string Id { get; set; } = "";
        [Required]
        public string UserName { get; set; } = "";
        [Required]
        public string FirstName { get; set; } = "";
        [Required]
        public string LastName { get; set; } = "";
        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";
    }
}

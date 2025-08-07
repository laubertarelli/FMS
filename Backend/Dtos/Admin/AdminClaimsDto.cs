namespace Backend.Dtos.Admin
{
    public class AdminClaimsDto
    {
        public string UserId { get; set; } = "";
        public List<PermissionSelectedDto> Permissions { get; set; } = [];
    }
}

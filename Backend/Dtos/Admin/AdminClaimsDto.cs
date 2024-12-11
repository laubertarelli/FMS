namespace Backend.Dtos.User
{
    public class AdminClaimsDto
    {
        public string Email { get; set; } = "";
        public List<ClaimSelectedDto> Claims { get; set; } = [];
    }
}

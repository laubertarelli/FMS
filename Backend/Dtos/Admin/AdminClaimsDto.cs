namespace Backend.Dtos.User
{
    public class AdminClaimsDto
    {
        public string UserId { get; set; } = "";
        public List<ClaimSelectedDto> Claims { get; set; } = [];
    }
}

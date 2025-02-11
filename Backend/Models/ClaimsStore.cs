using System.Security.Claims;

namespace Backend.Models
{
    public static class ClaimsStore
    {
        public static List<Claim> GetAllClaims()
        {
            return
            [
                new Claim("permissions", "create"),
                new Claim("permissions", "update"),
                new Claim("permissions", "delete")
            ];
        }
    }
}

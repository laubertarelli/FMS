using System.Security.Claims;

namespace Backend.Models
{
    public static class ClaimsStore
    {
        public static List<Claim> GetAllClaims()
        {
            return
            [
                new Claim("permission", "create"),
                new Claim("permission", "update"),
                new Claim("permission", "delete")
            ];
        }
    }
}

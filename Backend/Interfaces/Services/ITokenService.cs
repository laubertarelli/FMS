using Backend.Models;
using System.Security.Claims;

namespace Backend.Interfaces
{
    public interface ITokenService
    {
        public Task<string> CreateToken(User user);
        public Task<List<Claim>> GetClaims(User user);
    }
}

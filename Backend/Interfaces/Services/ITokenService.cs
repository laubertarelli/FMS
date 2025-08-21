using Backend.Models;

namespace Backend.Interfaces
{
    public interface ITokenService
    {
        public Task<string> CreateToken(User user);
        public string CreateGuestToken();
    }
}

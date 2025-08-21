using Backend.Interfaces;
using Backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Backend.Services
{
    public class TokenService(IConfiguration configuration, UserManager<User> userManager) : ITokenService
    {
        private string GetToken(List<Claim> claims)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT_KEY"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = credentials
            };

            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateToken(tokenDescriptor);
            return handler.WriteToken(token);
        }

        private async Task<List<Claim>> GetClaims(User user)
        {
            var options = new IdentityOptions();
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                //new Claim(ClaimTypes.Name, user.UserName!),
                //new Claim(ClaimTypes.Email, user.Email!)
            };

            var userClaims = await userManager.GetClaimsAsync(user);
            claims.AddRange(userClaims);

            var roles = await userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }
        
        public async Task<string> CreateToken(User user)
        {
            // User info for token
            var userClaims = await GetClaims(user);
            return GetToken(userClaims);
        }

        public string CreateGuestToken()
        {
            var guestClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "guest"),
                new Claim(ClaimTypes.Role, "user")
            };
            return GetToken(guestClaims);
        }
    }
}

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
        public async Task<string> CreateToken(User user)
        {
            // User info for token
            var userClaims = await GetClaims(user);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Token:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            // Token details
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(userClaims),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = credentials
            };

            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateToken(tokenDescriptor);
            return handler.WriteToken(token);
        }

        public async Task<List<Claim>> GetClaims(User user)
        {
            var options = new IdentityOptions();
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.Email, user.Email!)
            };

            var userClaims = await userManager.GetClaimsAsync(user);
            claims.AddRange(userClaims);

            var roles = await userManager.GetRolesAsync(user);
            claims.Add(new Claim(ClaimTypes.Role, roles[0]));

            return claims;
        }
    }
}

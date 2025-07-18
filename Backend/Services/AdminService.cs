using Backend.Dtos.Admin;
using Backend.Dtos.User;
using ClaimEnum = Backend.Enums.Claim;
using Backend.Interfaces.Services;
using Backend.Mappers;
using Backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Claim = System.Security.Claims.Claim;

namespace Backend.Services
{
    public class AdminService(UserManager<User> userManager) : IAdminService
    {
        public async Task<int> CountAll()
        {
            return await userManager.Users.CountAsync();
        }

        public async Task<bool> Delete(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user is null)
            {
                return false;
            }

            var result = await userManager.DeleteAsync(user);
            return result.Succeeded;
        }

        public async Task<List<UserDto>> GetAll(int page)
        {
            var users = await userManager.Users
                .Include(u => u.Files)
                .Include(u => u.Procedures)
                .Skip((page - 1) * 5)
                .Take(5)
                .ToListAsync();
            return users.Select(u => u.ToUserDto()).ToList();
        }

        public async Task<UserDto?> GetById(string id)
        {
            var user = await userManager.Users
                .Include(u => u.Files)
                .Include(u => u.Procedures)
                .FirstOrDefaultAsync(u => u.Id == id);
            return user?.ToUserDto();
        }

        public List<ClaimDto> GetAllClaims()
        {
            return ((ClaimEnum[])Enum.GetValues(typeof(ClaimEnum)))
                .Select(c => new ClaimDto() { Value = (int)c, Name = c.ToString() })
                .ToList();
        }

        public async Task<IList<Claim>?> GetUserClaims(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user is null)
            {
                return null;
            }

            return await userManager.GetClaimsAsync(user);
        }

        public async Task<IList<Claim>?> ManageUserClaims(AdminClaimsDto userDto)
        {
            var user = await userManager.FindByIdAsync(userDto.UserId);
            if (user is null)
            {
                return null;
            }

            var existingClaims = await userManager.GetClaimsAsync(user);
            foreach (var newClaim in userDto.Claims)
            {
                if (!existingClaims.Any(c => c.Value == newClaim.ClaimValue) && newClaim.IsSelected)
                {
                    await userManager.AddClaimAsync(user, new Claim("permissions", newClaim.ClaimValue));
                }
                else if (existingClaims.Any(c => c.Value == newClaim.ClaimValue) && !newClaim.IsSelected)
                {
                    await userManager.RemoveClaimAsync(user, new Claim("permissions", newClaim.ClaimValue));
                }
            }
            return await userManager.GetClaimsAsync(user);
        }

        public async Task<UserDto?> Update(UpdateAdminDto updateDto)
        {
            var user = await userManager.FindByIdAsync(updateDto.Id);
            if (user is null)
            {
                return null;
            }

            user.UserName = updateDto.UserName;
            user.Email = updateDto.Email;
            user.FirstName = updateDto.FirstName;
            user.LastName = updateDto.LastName;

            var result = await userManager.UpdateAsync(user);
            return result.Succeeded ? user.ToUserDto() : null;
        }
    }
}

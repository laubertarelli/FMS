using Backend.Dtos.Admin;
using Backend.Dtos.User;
using ClaimEnum = Backend.Enums.Claim;
using RoleEnum = Backend.Enums.Role;
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
                .OrderBy(u => u.Id)
                .Skip((page - 1) * 5)
                .Take(5)
                .ToListAsync();

            var userDtos = new List<UserDto>();
            foreach (var user in users)
            {
                userDtos.Add(await user.ToUserDtoAsync(userManager));
            }
            return userDtos;
        }

        public async Task<UserDto?> GetById(string id)
        {
            var user = await userManager.Users
                .Include(u => u.Files)
                .Include(u => u.Procedures)
                .FirstOrDefaultAsync(u => u.Id == id);
            return user != null ? await user.ToUserDtoAsync(userManager) : null;
        }

        public List<PermissionDto> GetAllClaims()
        {
            return ((ClaimEnum[])Enum.GetValues(typeof(ClaimEnum)))
                .Select(c => new PermissionDto() { Value = (int)c, Name = c.ToString() })
                .ToList();
        }

        public List<PermissionDto> GetAllRoles()
        {
            return ((RoleEnum[])Enum.GetValues(typeof(RoleEnum)))
                .Select(r => new PermissionDto() { Value = (int)r, Name = r.ToString().ToLower() })
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

        public async Task<IList<string>?> GetUserRoles(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user is null)
            {
                return null;
            }

            return await userManager.GetRolesAsync(user);
        }

        public async Task<IList<Claim>?> ManageUserClaims(AdminClaimsDto userDto)
        {
            var user = await userManager.FindByIdAsync(userDto.UserId);
            if (user is null)
            {
                return null;
            }

            const string CLAIM_TYPE = "permissions";
            var existingClaims = await userManager.GetClaimsAsync(user);
            var existingClaimValues = existingClaims
                .Where(c => c.Type == CLAIM_TYPE)
                .Select(c => c.Value)
                .ToHashSet();

            foreach (var newClaim in userDto.Permissions)
            {
                var hasExistingClaim = existingClaimValues.Contains(newClaim.PermissionValue);

                if (!hasExistingClaim && newClaim.IsSelected)
                {
                    await userManager.AddClaimAsync(user, new Claim(CLAIM_TYPE, newClaim.PermissionValue));
                }
                else if (hasExistingClaim && !newClaim.IsSelected)
                {
                    await userManager.RemoveClaimAsync(user, new Claim(CLAIM_TYPE, newClaim.PermissionValue));
                }
            }

            return await userManager.GetClaimsAsync(user);
        }

        public async Task<IList<string>?> ManageUserRoles(AdminClaimsDto userDto)
        {
            var user = await userManager.FindByIdAsync(userDto.UserId);
            if (user is null)
            {
                return null;
            }

            var existingRoles = await userManager.GetRolesAsync(user);
        
            foreach (var role in userDto.Permissions)
            {
                var roleNameLower = role.PermissionValue.ToLower();
                var hasExistingRole = existingRoles.Contains(roleNameLower);
                
                if (!hasExistingRole && role.IsSelected)
                {
                    await userManager.AddToRoleAsync(user, roleNameLower);
                }
                else if (hasExistingRole && !role.IsSelected)
                {
                    await userManager.RemoveFromRoleAsync(user, roleNameLower);
                }
            }

            return await userManager.GetRolesAsync(user);
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
            return result.Succeeded ? await user.ToUserDtoAsync(userManager) : null;
        }
    }
}

using Backend.Dtos.User;
using Backend.Interfaces;
using Backend.Interfaces.Services;
using Backend.Mappers;
using Backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
    public class UserService(UserManager<User> userManager, SignInManager<User> signInManager, ITokenService tokenService) : IUserService
    {
        public async Task<NewUserDto?> Login(LoginUserDto loginDto)
        {
            var userModel = await userManager.Users.FirstOrDefaultAsync(u => u.Email!.Equals(loginDto.Email));
            if (userModel is null)
            {
                return null;
            }

            var result = await signInManager.CheckPasswordSignInAsync(userModel, loginDto.Password, false);
            if (!result.Succeeded)
            {
                return null;
            }

            var token = await tokenService.CreateToken(userModel);
            return userModel.ToNewUserDto(token);
        }

        public async Task<bool> Logout()
        {
            await signInManager.SignOutAsync();
            return true;
        }

        public async Task<UserDto?> MyAccount(string userId)
        {
            var user = await userManager.Users
                .Include(u => u.Files)
                .Include(u => u.Procedures)
                .FirstOrDefaultAsync(u => u.Id == userId);
            
            return user != null ? await user.ToUserDtoAsync(userManager) : null;
        }

        public async Task<bool> ResetPassword(ResetPasswordUserDto userDto)
        {
            var userModel = await userManager.FindByEmailAsync(userDto.Email);
            if (userModel is null)
            {
                return false;
            }

            var code = await userManager.GeneratePasswordResetTokenAsync(userModel);
            var result = await userManager.ResetPasswordAsync(userModel, code, userDto.Password);
            if (!result.Succeeded)
            {
                // Opcional: Loguea los errores para depuración
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"Error: {error.Code} - {error.Description}");
                }
            }
            return result.Succeeded;
        }

        public async Task<NewUserDto?> Signup(SignupUserDto userDto)
        {
            var userModel = userDto.ToUserFromSignup();
            var createdUser = await userManager.CreateAsync(userModel, userDto.Password);
            if (!createdUser.Succeeded)
            {
                return null;
            }

            var admins = await userManager.GetUsersInRoleAsync("superadmin");
            if (!admins.Any())
            {
                // If no superadmin exists, assign "SuperAdmin" role to this user.
                await userManager.AddToRolesAsync(userModel, ["superadmin", "admin", "user"]);
                // Also claims are added to admin
                await userManager.AddClaimsAsync(userModel, ClaimsStore.GetAllClaims()); 
            }
            else
            {
                await userManager.AddToRoleAsync(userModel, "user");
            }

            var token = await tokenService.CreateToken(userModel);
            return userModel.ToNewUserDto(token);
        }

        public async Task<UserDto?> Update(UpdateUserDto userDto, string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return null;
            }

            user.Email = userDto.Email;
            user.UserName = userDto.UserName;
            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;

            var result = await userManager.UpdateAsync(user);
            return result.Succeeded ? await user.ToUserDtoAsync(userManager) : null;
        }
    }
}

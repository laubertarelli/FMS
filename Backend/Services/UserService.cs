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

        public async Task<UserDto?> MyAccount(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            return user?.ToUserDto();
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

            if (userManager.Users.Count() == 1)
            {
                await userManager.AddToRoleAsync(userModel, "admin");                     // If it's the first user registered, "Admin" role is assigned.
                await userManager.AddClaimsAsync(userModel, ClaimsStore.GetAllClaims()); // Also claims are added to admin
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
            return result.Succeeded ? user.ToUserDto() : null;
        }
    }
}

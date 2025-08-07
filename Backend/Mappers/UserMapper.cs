using Backend.Dtos.User;
using Backend.Models;
using Microsoft.AspNetCore.Identity;

namespace Backend.Mappers
{
    public static class UserMapper
    {
        public static async Task<UserDto> ToUserDtoAsync(this User userModel, UserManager<User> userManager)
        {
            var roles = await userManager.GetRolesAsync(userModel);
            
            return new UserDto
            {
                Id = userModel.Id,
                Username = userModel.UserName!,
                Email = userModel.Email!,
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
                Files = userModel.Files.Count,
                Procedures = userModel.Procedures.Count,
                Roles = roles.ToList()
            };
        }

        public static User ToUserFromSignup(this SignupUserDto userDto)
        {
            return new User
            {
                UserName = userDto.Username,
                Email = userDto.Email,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName
            };
        }

        public static NewUserDto ToNewUserDto(this User userModel, string token)
        {
            return new NewUserDto
            {
                UserName = userModel.UserName!,
                Email = userModel.Email!,
                Token = token
            };
        }
    }
}

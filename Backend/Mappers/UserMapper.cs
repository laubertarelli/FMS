using Backend.Dtos.User;
using Backend.Models;

namespace Backend.Mappers
{
    public static class UserMapper
    {
        public static UserDto ToUserDto(this User userModel)
        {
            return new UserDto
            {
                Username = userModel.UserName!,
                Email = userModel.Email!,
                FullName = userModel.FullName,
                Files = userModel.Files.Select(f => f.ToFileDto()).ToList(),
                Procedures = userModel.Procedures.Select(p => p.ToProcedureDto()).ToList()
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

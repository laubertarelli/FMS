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
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
                CreatedOn = userModel.CreatedOn,
                UpdatedOn = userModel.UpdatedOn,
                Files = userModel.Files.Select(f => f.ToFileDto()).ToList(),
                Procedures = userModel.Procedures.Select(p => p.ToProcedureDto()).ToList()
            };
        }

        public static User FromSignupToUser(this SignupUserDto userDto)
        {
            return new User
            {
                UserName = userDto.Username,
                Email = userDto.Email,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName
            };
        }
    }
}

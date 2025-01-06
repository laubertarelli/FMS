using Backend.Dtos.User;

namespace Backend.Interfaces.Services
{
    public interface IUserService
    {
        Task<NewUserDto?> Signup(SignupUserDto signupDto);
        Task<NewUserDto?> Login(LoginUserDto loginDto);
        Task<bool> ResetPassword(ResetPasswordUserDto userDto);
        Task<UserDto?> MyAccount(string userId);
        Task<UserDto?> Update(UpdateUserDto updateDto, string userId);
    }
}

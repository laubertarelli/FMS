using Backend.Dtos.User;
using Backend.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend.Controllers
{
    [ApiController]
    [Route("")]
    public class UserController(IUserService service) : ControllerBase
    {
        string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] SignupUserDto signupDto)
        {
            var user = await service.Signup(signupDto);
            return (user is null) ? BadRequest() : Ok("Your account has been created successfully!");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto loginDto)
        {
            var user = await service.Login(loginDto);
            return (user is null) ? NotFound("Email or password invalid.") : Ok(user);
        }

        [HttpPost("login/guest")]
        public async Task<IActionResult> LoginAsGuest()
        {
            var guestUser = await service.LoginAsGuest();
            return Ok(guestUser);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var result = await service.Logout();
            return result ? Ok() : NotFound();
        }

        [HttpPut("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordUserDto userDto)
        {
            var result = await service.ResetPassword(userDto);
            return result ? Ok("Your password has been updated successfully!") : NotFound("Email not found.");
        }

        [HttpGet("account")]
        [Authorize]
        public async Task<IActionResult> MyAccount()
        {
            var user = await service.MyAccount(UserId);
            return (user is null) ? NotFound() : Ok(user);
        }

        [HttpPut("account/update")]
        [Authorize]
        public async Task<IActionResult> Update([FromBody] UpdateUserDto updateDto)
        {
            var user = await service.Update(updateDto, UserId);
            return (user is null) ? NotFound() : Ok("Your account has been updated successfully!");
        }
    }
}

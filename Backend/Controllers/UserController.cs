using Backend.Dtos.User;
using Backend.Interfaces;
using Backend.Mappers;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Backend.Controllers
{
    [ApiController]
    [Route("")]
    public class UserController(UserManager<User> userManager, ITokenService tokenService, SignInManager<User> signInManager) : ControllerBase
    {
        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] SignupUserDto userDto)
        {
            try
            {
                var userModel = userDto.FromSignupToUser();
                var createdUser = await userManager.CreateAsync(userModel, userDto.Password);
                if (!createdUser.Succeeded)
                {
                    return BadRequest(createdUser.Errors);
                }

                var roleResult = new IdentityResult();
                if (userManager.Users.Count() == 1)
                {
                    roleResult = await userManager.AddToRoleAsync(userModel, "admin"); // If it's the first user registered, "Admin" role is assigned.
                    await userManager.AddClaimsAsync(userModel, ClaimsStore.GetAllClaims()); // Also claims are added to admin
                }
                else
                {
                    roleResult = await userManager.AddToRoleAsync(userModel, "user");
                }
                if (!roleResult.Succeeded)
                {
                    return BadRequest(roleResult.Errors);
                }
                return Ok(new NewUserDto
                {
                    UserName = userModel.UserName!,
                    Email = userModel.Email!,
                    Token = await tokenService.CreateToken(userModel)
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto loginDto)
        {
            var userModel = await userManager.Users.FirstOrDefaultAsync(u => u.Email!.Equals(loginDto.Email));
            if (userModel is null)
            {
                return Unauthorized("Invalid email");
            }

            var result = await signInManager.CheckPasswordSignInAsync(userModel, loginDto.Password, false);
            if (!result.Succeeded)
            {
                return Unauthorized("Invalid password");
            }

            return Ok(new NewUserDto
            {
                UserName = userModel.UserName!,
                Email = userModel.Email!,
                Token = await tokenService.CreateToken(userModel)
            });
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return Ok("Logged out");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordUserDto userDto)
        {
            var userModel = await userManager.FindByEmailAsync(userDto.Email);
            if (userModel is null)
            {
                return Unauthorized("Invalid email.");
            }

            var code = await userManager.GeneratePasswordResetTokenAsync(userModel);
            var result = await userManager.ResetPasswordAsync(userModel, code, userDto.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return RedirectToAction(nameof(Login));
        }

        [HttpGet("account")]
        [Authorize]
        public async Task<IActionResult> MyAccount()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var user = await userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return NotFound();
            }

            return Ok(user.ToUserDto());
        }

        [HttpPut("account/update")]
        [Authorize]
        public async Task<IActionResult> Update([FromBody] UpdateUserDto userDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var user = await userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return NotFound();
            }

            user.Email = userDto.Email;
            user.UserName = userDto.UserName;
            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;

            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            return Ok(user.ToUserDto());
        }
    }
}

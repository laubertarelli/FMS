using Backend.Dtos.User;
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
    [Route("users")]
    [Authorize(Roles = "admin")]
    public class AdminController(UserManager<User> userManager) : ControllerBase
    {
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateAdminDto userDto)
        {
            var user = await userManager.FindByIdAsync(userDto.Id);
            if (user is null)
            {
                return NotFound();
            }

            user.UserName = userDto.UserName;
            user.Email = userDto.Email;
            user.FirstName = user.FirstName;
            user.LastName = user.LastName;

            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            return Ok(user.ToUserDto());
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user is null)
            {
                return NotFound();
            }

            var result = await userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            return NoContent();
        }

        [HttpGet("{page:int}")]
        public async Task<IActionResult> GetAll([FromRoute] int page)
        {
            var users = await userManager.Users.Skip((page - 1) * 5).Take(5).ToListAsync();
            var result = users.Select(u => u.ToUserDto()).ToList();
            return Ok(result);
        }

        [HttpGet("view")]
        public async Task<IActionResult> GetById([FromBody] string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user is null)
            {
                return NotFound();
            }
            return Ok(user.ToUserDto());
        }

        [HttpGet("count")]
        public async Task<IActionResult> CountAll()
        {
            int count = await userManager.Users.CountAsync();
            return Ok(count);
        }

        [HttpPost("claims")]
        public async Task<IActionResult> ManageUserClaims([FromBody] AdminClaimsDto userModel)
        {
            var user = await userManager.FindByEmailAsync(userModel.Email);
            if (user is null)
            {
                return Unauthorized("Invalid email");
            }

            var existingClaims = await userManager.GetClaimsAsync(user);
            foreach (var newClaim in userModel.Claims)
            {
                if (!existingClaims.Any(c => c.Value == newClaim.ClaimValue) && newClaim.IsSelected)
                {
                    await userManager.AddClaimAsync(user, new Claim("permission", newClaim.ClaimValue));
                }
                else if (existingClaims.Any(c => c.Value == newClaim.ClaimValue) && !newClaim.IsSelected)
                {
                    await userManager.RemoveClaimAsync(user, new Claim("permission", newClaim.ClaimValue));
                }
            }
            return Ok(await userManager.GetClaimsAsync(user));
        }
    }
}

using Backend.Dtos.Admin;
using Backend.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("users")]
    [Authorize(Roles = "admin, superadmin")]
    public class AdminController(IAdminService service) : ControllerBase
    {
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateAdminDto updateDto)
        {
            var user = await service.Update(updateDto);
            return (user is null) ? NotFound() : Ok("The user has been updated successfully!");
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] string email)
        {
            var result = await service.Delete(email);
            return result ? NoContent() : NotFound();
        }

        [HttpGet("{page:int}")]
        public async Task<IActionResult> GetAll([FromRoute] int page)
        {
            var users = await service.GetAll(page);
            return Ok(users);
        }

        [HttpGet("details/{id}")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var user = await service.GetById(id);
            return (user is null) ? NotFound() : Ok(user);
        }

        [HttpGet("count")]
        public async Task<IActionResult> CountAll()
        {
            var count = await service.CountAll();
            return Ok(count);
        }

        [HttpGet("claims")]
        public IActionResult GetAllClaims()
        {
            var claims = service.GetAllClaims();
            return Ok(claims);
        }

        [HttpGet("roles")]
        public IActionResult GetAllRoles()
        {
            var roles = service.GetAllRoles();
            return Ok(roles);
        }

        [HttpGet("claims/{id}")]
        public async Task<IActionResult> GetUserClaims([FromRoute] string id)
        {
            var claims = await service.GetUserClaims(id);
            return (claims is null) ? NotFound() : Ok(claims);
        }

        [HttpGet("roles/{id}")]
        public async Task<IActionResult> GetUserRoles([FromRoute] string id)
        {
            var roles = await service.GetUserRoles(id);
            return (roles is null) ? NotFound() : Ok(roles);
        }

        [HttpPut("claims")]
        public async Task<IActionResult> ManageUserClaims([FromBody] AdminClaimsDto userDto)
        {
            var claims = await service.ManageUserClaims(userDto);
            return (claims is null) ? NotFound() : Ok(claims);
        }

        [HttpPut("roles")]
        public async Task<IActionResult> ManageUserRoles([FromBody] AdminClaimsDto userDto)
        {
            var roles = await service.ManageUserRoles(userDto);
            return (roles is null) ? NotFound() : Ok(roles);
        }
    }
}

using Backend.Dtos.File;
using Backend.Interfaces.Services;
using Backend.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("files")]
    public class FileController(IFileService service) : ControllerBase
    {
        string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        [HttpPost]
        [Authorize(Policy = "CreatePolicy")]
        public async Task<IActionResult> Create([FromBody] RequestFileDto fileDto)
        {
            var fileModel = await service.CreateAsync(fileDto, UserId);
            return CreatedAtAction(nameof(GetById), new { id = fileModel.Id }, fileModel.ToFileDto());
        }

        [HttpPut("{id:int}")]
        [Authorize(Policy = "UpdatePolicy")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] RequestFileDto fileDto)
        {
            var file = await service.UpdateAsync(id, fileDto, UserId);
            return (file is null) ? NotFound() : Ok(file);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Policy = "DeletePolicy")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var result = await service.DeleteAsync(id);
            return result ? NoContent() : NotFound();
        }

        [HttpGet("{page:int}")]
        public async Task<IActionResult> GetAll([FromRoute] int page)
        {
            var files = await service.GetAllAsync(page);
            return Ok(files);
        }

        [HttpGet("view/{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var file = await service.GetByIdAsync(id);
            return (file is null) ? NotFound() : Ok(file);
        }

        [HttpGet("states")]
        public IActionResult GetFileStates()
        {
            var states = service.GetFileStates();
            return Ok(states);
        }

        [HttpGet("ids")]
        public async Task<IActionResult> GetIds()
        {
            var ids = await service.GetIdsAsync();
            return Ok(ids);
        }

        [HttpGet("count")]
        public async Task<IActionResult> CountAll()
        {
            var count = await service.CountAllAsync();
            return Ok(count);
        }
    }
}

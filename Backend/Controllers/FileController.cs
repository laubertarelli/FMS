using Backend.Dtos.File;
using Backend.Interfaces;
using Backend.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("files")]
    public class FileController(IFileRepository repo) : ControllerBase
    {
        [HttpPost]
        [Authorize(Policy = "CreatePolicy")]
        public async Task<IActionResult> Create([FromBody] RequestFileDto fileDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var fileModel = fileDto.ToFileFromRequestDto(userId);
            await repo.CreateAsync(fileModel);
            return CreatedAtAction(nameof(GetById), new { id = fileModel.Id }, fileModel.ToFileDto());
        }

        [HttpPut("{id:int}")]
        [Authorize(Policy = "UpdatePolicy")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] RequestFileDto fileDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var fileModel = await repo.UpdateAsync(id, fileDto, userId);
            if (fileModel is null)
            {
                return NotFound();
            }
            return Ok(fileModel.ToFileDto());
        }

        [HttpDelete]
        [Authorize(Policy = "DeletePolicy")]
        public async Task<IActionResult> Delete(int id)
        {
            var file = await repo.DeleteAsync(id);
            if (file is null)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpGet("{page:int}")]
        public async Task<IActionResult> GetAll([FromRoute] int page)
        {
            var files = await repo.GetAllAsync(page);
            var result = files.Select(f => f.ToFileDto()).ToList();
            return Ok(result);
        }

        [HttpGet("view/{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var file = await repo.GetByIdAsync(id);
            if (file is null)
            {
                return NotFound();
            }
            return Ok(file.ToFileDto());
        }

        [HttpGet("count")]
        public async Task<IActionResult> CountAll()
        {
            int count = await repo.CountAllAsync();
            return Ok(count);
        }
    }
}

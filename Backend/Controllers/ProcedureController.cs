using Backend.Dtos.Procedure;
using Backend.Enums;
using Backend.Interfaces.Services;
using Backend.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("procedures")]
    public class ProcedureController(IProcedureService service) : ControllerBase
    {
        string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        [HttpPost]
        [Authorize(Policy = "CreatePolicy")]
        public async Task<IActionResult> Create([FromBody] RequestProcedureDto procedureDto)
        {
            var procedureModel = await service.CreateAsync(procedureDto, UserId);
            return CreatedAtAction(nameof(GetById), new { id = procedureModel.Id }, procedureModel.ToProcedureDto());
        }

        [HttpPut("{id:int}")]
        [Authorize(Policy = "UpdatePolicy")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] RequestProcedureDto procedureDto)
        {
            var procedureModel = await service.UpdateAsync(id, procedureDto, UserId);
            return (procedureModel is null) ? NotFound() : Ok(procedureModel);
        }

        [HttpDelete]
        [Authorize(Policy = "DeletePolicy")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await service.DeleteAsync(id, UserId);
            return result ? NoContent() : NotFound();
        }

        [HttpGet("{page:int}")]
        public async Task<IActionResult> GetAll([FromRoute] int page)
        {
            var procedures = await service.GetAllAsync(page);
            return Ok(procedures);
        }

        [HttpGet("labels")]
        public IActionResult GetProcedureLabels()
        {
            var labels = service.GetProcedureLabels();
            return Ok(labels);
        }

        [HttpGet("view/{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var procedure = await service.GetByIdAsync(id);
            return (procedure is null) ? NotFound() : Ok(procedure);
        }

        [HttpGet("file/{fileId:int}/{page:int}")]
        public async Task<IActionResult> GetByFileId([FromRoute] int fileId, [FromRoute] int page)
        {
            var procedures = await service.GetByFileIdAsync(fileId, page);
            return Ok(procedures);
        }

        [HttpGet("label/{labelId:int}/{page:int}")]
        public async Task<IActionResult> GetByLabel([FromRoute] int labelId, [FromRoute] int page)
        {
            var procedures = await service.GetByLabelAsync((ProcedureLabel)labelId, page);
            return Ok(procedures);
        }

        [HttpGet("count")]
        public async Task<IActionResult> CountAll()
        {
            var count = await service.CountAllAsync();
            return Ok(count);
        }

        [HttpGet("count/file/{fileId:int}")]
        public async Task<IActionResult> CountByFileId([FromRoute] int fileId)
        {
            var count = await service.CountByFileIdAsync(fileId);
            return Ok(count);
        }

        [HttpGet("count/label/{labelId:int}")]
        public async Task<IActionResult> CountByLabel([FromRoute] int labelId)
        {
            var count = await service.CountByLabelAsync((ProcedureLabel)labelId);
            return Ok(count);
        }
    }
}

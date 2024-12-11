using Backend.Dtos.Procedure;
using Backend.Enums;
using Backend.Interfaces;
using Backend.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("procedures")]
    public class ProcedureController(IProcedureRepository repo, IUpdateStateService stateService) : ControllerBase
    {
        [HttpPost]
        [Authorize(Policy = "CreatePolicy")]
        public async Task<IActionResult> Create([FromBody] RequestProcedureDto procedureDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var procedureModel = procedureDto.ToProcedureFromRequestDto(userId);
            await repo.CreateAsync(procedureModel);
            await stateService.UpdateState(procedureModel.FileId, userId);
            return CreatedAtAction(nameof(GetById), new { id = procedureModel.Id }, procedureModel.ToProcedureDto());
        }

        [HttpPut("{id:int}")]
        [Authorize(Policy = "UpdatePolicy")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] RequestProcedureDto procedureDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var procedureModel = await repo.UpdateAsync(id, procedureDto, userId);
            if (procedureModel is null)
            {
                return NotFound();
            }
            await stateService.UpdateState(procedureModel.FileId, userId);
            return Ok(procedureModel.ToProcedureDto());
        }

        [HttpDelete]
        [Authorize(Policy = "DeletePolicy")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var procedure = await repo.DeleteAsync(id);
            if (procedure is null)
            {
                return NotFound();
            }
            await stateService.UpdateState(procedure.FileId, userId);
            return NoContent();
        }

        [HttpGet("{page:int}")]
        public async Task<IActionResult> GetAll([FromRoute] int page)
        {
            var procedures = await repo.GetAllAsync(page);
            var result = procedures.Select(p => p.ToProcedureDto()).ToList();
            return Ok(result);
        }

        [HttpGet("view/{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var procedure = await repo.GetByIdAsync(id);
            if (procedure is null)
            {
                return NotFound();
            }
            return Ok(procedure.ToProcedureDto());
        }

        [HttpGet("file/{fileId:int}/{page:int}")]
        public async Task<IActionResult> GetByFileId([FromRoute] int fileId, [FromRoute] int page)
        {
            var procedures = await repo.GetByFileIdAsync(fileId, page);
            var result = procedures.Select(p => p.ToProcedureDto()).ToList();
            return Ok(result);
        }

        [HttpGet("label/{labelId:int}/{page:int}")]
        public async Task<IActionResult> GetByLabel([FromRoute] int labelId, [FromRoute] int page)
        {
            var procedures = await repo.GetByLabelAsync((ProcedureLabel)labelId, page);
            var result = procedures.Select(p => p.ToProcedureDto()).ToList();
            return Ok(result);
        }

        [HttpGet("count")]
        public async Task<IActionResult> CountAll()
        {
            var count = await repo.CountAllAsync();
            return Ok(count);
        }

        [HttpGet("count/file/{fileId:int}")]
        public async Task<IActionResult> CountByFileId([FromRoute] int fileId)
        {
            var count = await repo.CountByFileIdAsync(fileId);
            return Ok(count);
        }

        [HttpGet("count/label/{labelId:int}")]
        public async Task<IActionResult> CountByLabel([FromRoute] int labelId)
        {
            var count = await repo.CountByLabelAsync((ProcedureLabel)labelId);
            return Ok(count);
        }
    }
}

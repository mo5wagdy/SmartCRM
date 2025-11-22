using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartCRM.Application.Dtos.Deal_Dtos;
using SmartCRM.Application.Dtos.Lead_Dtos;
using SmartCRM.Application.Interfaces.Services;
using SmartCRM.Domain.Entities;

namespace SmartCRM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DealsController : ControllerBase
    {
        private readonly IDealService _svc;
        private readonly ILogger<DealsController> _logger;

        public DealsController(IDealService svc, ILogger<DealsController> logger)
        {
            _svc = svc;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DealDto>), StatusCodes.Status200OK)]
        public async Task <IActionResult> GetAll([FromQuery] int Page = 1, [FromQuery] int PageSize = 20, [FromQuery] string? Q = null)
        {
            var items = await _svc.GetAllAsync(Page, PageSize, Q);
            return Ok(items);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(DealDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task <IActionResult> Get(int id)
        {
            var item = await _svc.GetByIdAsync(id);
            if (item == null) return NotFound(new { message = $"Deal {id} Not Found" });
            return Ok(item);
        }

        [HttpPost]
        [ProducesResponseType(typeof(DealDto), StatusCodes.Status201Created)]
        public async Task <IActionResult> Create([FromBody] CreateDealDto dto)
        {
            var created = await _svc.CreateAsync(dto);
            _logger.LogInformation("Deal Created {DealId}", created.DealId);
            return CreatedAtAction(nameof(Get), new { id = created.DealId }, created);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateDealDto dto)
        {
            await _svc.UpdateAsync(id, dto);
            _logger.LogInformation("Deal Updated {DealId}", id);
            return NoContent();
        }

        // Patch to update the stage of a deal => Not Put Cuz Put Updates the Whole Resource
        [HttpPatch("{id:int}/stage")]
        [ProducesResponseType(typeof(DealDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> ChangeStage(int id, [FromQuery] string to, [FromQuery] int? userId = null)
        {
            var updated = await _svc.ChangeStageAsync(id, to, userId);
            _logger.LogInformation("Deal {DealId} stage changed to {Stage} by {User}", id, to, userId);
            return Ok(updated);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(int id)
        {
            await _svc.SoftDeleteAsync(id);
            _logger.LogInformation("Deal soft-deleted {DealId}", id);
            return NoContent();
        }

    }
}

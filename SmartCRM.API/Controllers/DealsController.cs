using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartCRM.API.Extensions;
using SmartCRM.API.Models;
using SmartCRM.Application.Dtos.Deal_Dtos;
using SmartCRM.Application.Interfaces.Services;

namespace SmartCRM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class DealsController : ControllerBase
    {
        private readonly IDealService _svc;
        private readonly ILogger<DealsController> _logger;

        public DealsController(IDealService svc, ILogger<DealsController> logger)
        {
            _svc = svc;
            _logger = logger;
        }

        [HttpGet(Name = "GetDeals")]
        [ProducesResponseType(typeof(IEnumerable<DealDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string? q = null,
            CancellationToken cancellationToken = default)
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 200);

            var items = (await _svc.GetAllAsync(page, pageSize, q)).ToList();
            long total = items.Count < pageSize && page == 1 ? items.Count : items.Count;
            var meta = new PaginationMetadata
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = total,
                TotalPages = Math.Max(1, (int)Math.Ceiling(total / (double)pageSize))
            };
            Response.AddPaginationHeader(meta);
            return Ok(items);
        }

        [HttpGet("{id:int}", Name = "GetDeal")]
        [ProducesResponseType(typeof(DealDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken = default)
        {
            var item = await _svc.GetByIdAsync(id);
            if (item == null) return NotFound(new { message = $"Deal {id} not found" });
            return Ok(item);
        }

        [HttpPost(Name = "CreateDeal")]
        [ProducesResponseType(typeof(DealDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateDealDto dto, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            var created = await _svc.CreateAsync(dto);
            _logger.LogInformation("Deal created {DealId}", created.DealId);
            return CreatedAtRoute("GetDeal", new { id = created.DealId }, created);
        }

        [HttpPut("{id:int}", Name = "UpdateDeal")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateDealDto dto, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            await _svc.UpdateAsync(id, dto);
            _logger.LogInformation("Deal updated {DealId}", id);
            return NoContent();
        }

        [HttpPatch("{id:int}/stage", Name = "ChangeDealStage")]
        [ProducesResponseType(typeof(DealDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ChangeStage(int id, [FromQuery] string to, [FromQuery] int? userId = null, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(to)) return BadRequest(new { message = "Target stage 'to' is required." });
            var updated = await _svc.ChangeStageAsync(id, to, userId);
            _logger.LogInformation("Deal {DealId} stage changed to {Stage} by {User}", id, to, userId);
            return Ok(updated);
        }

        [HttpDelete("{id:int}", Name = "DeleteDeal")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
        {
            await _svc.SoftDeleteAsync(id);
            _logger.LogInformation("Deal soft-deleted {DealId}", id);
            return NoContent();
        }
    }
}
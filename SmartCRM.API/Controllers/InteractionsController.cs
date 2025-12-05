using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartCRM.API.Extensions;
using SmartCRM.API.Models;
using SmartCRM.Application.Dtos.Interaction_Dtos;
using SmartCRM.Application.Interfaces.Services;

namespace SmartCRM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class InteractionsController : ControllerBase
    {
        private readonly IInteractionService _svc;
        private readonly ILogger<InteractionsController> _logger;

        public InteractionsController(IInteractionService svc, ILogger<InteractionsController> logger)
        {
            _svc = svc;
            _logger = logger;
        }

        [HttpGet(Name = "GetInteractions")]
        [ProducesResponseType(typeof(IEnumerable<InteractionDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string? q = null,
            [FromQuery] string? type = null,
            [FromQuery] string? status = null,
            [FromQuery] string? relatedTo = null,
            [FromQuery] int? relatedId = null,
            [FromQuery] int? customerId = null,
            [FromQuery] int? dealId = null,
            [FromQuery] int? assignedTo = null,
            [FromQuery] DateTime? from = null,
            [FromQuery] DateTime? to = null,
            CancellationToken cancellationToken = default)
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 200);

            var items = (await _svc.GetAllAsync(page, pageSize, q, type, status, relatedTo, relatedId, customerId, dealId, assignedTo, from, to)).ToList();
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

        [HttpGet("{id:int}", Name = "GetInteraction")]
        [ProducesResponseType(typeof(InteractionDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken = default)
        {
            var item = await _svc.GetByIdAsync(id);
            if (item == null) return NotFound(new { message = $"Interaction {id} not found" });
            return Ok(item);
        }

        [HttpPost(Name = "CreateInteraction")]
        [ProducesResponseType(typeof(InteractionDto), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] CreateInteractionDto dto, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            var created = await _svc.CreateAsync(dto);
            _logger.LogInformation("Interaction created {InteractionId}", created.InteractionId);
            return CreatedAtRoute("GetInteraction", new { id = created.InteractionId }, created);
        }

        [HttpPut("{id:int}", Name = "UpdateInteraction")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateInteractionDto dto, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            await _svc.UpdateAsync(id, dto);
            _logger.LogInformation("Interaction updated {InteractionId}", id);
            return NoContent();
        }

        [HttpDelete("{id:int}", Name = "DeleteInteraction")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
        {
            await _svc.SoftDeleteAsync(id);
            _logger.LogInformation("Interaction soft-deleted {InteractionId}", id);
            return NoContent();
        }
    }
}
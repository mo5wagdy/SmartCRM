using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartCRM.API.Extensions;
using SmartCRM.API.Models;
using SmartCRM.Application.Dtos.Lead_Dtos;
using SmartCRM.Application.Interfaces.Services;

namespace SmartCRM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class LeadsController : ControllerBase
    {
        private readonly ILeadService _svc;
        private readonly ILogger<LeadsController> _logger;

        public LeadsController(ILeadService svc, ILogger<LeadsController> logger)
        {
            _svc = svc;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<LeadDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery] int Page = 1, [FromQuery] int PageSize = 20, [FromQuery] string? Q = null, CancellationToken cancellationToken = default)
        {
            Page = Math.Max(1, Page);
            PageSize = Math.Clamp(PageSize, 1, 200);

            var items = (await _svc.GetAllAsync(Page, PageSize, Q)).ToList();
            long total = items.Count < PageSize && Page == 1 ? items.Count : items.Count;

            var meta = new PaginationMetadata
            {
                Page = Page,
                PageSize = PageSize,
                TotalCount = total,
                TotalPages = Math.Max(1, (int)Math.Ceiling(total / (double)PageSize))
            };

            Response.AddPaginationHeader(meta);
            return Ok(items);
        }

        [HttpGet("{id:int}", Name = "GetLead")]
        [ProducesResponseType(typeof(LeadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken = default)
        {
            var item = await _svc.GetByIdAsync(id);
            if (item == null) return NotFound(new { message = $"Lead {id} Not Found"});
            return Ok(item);
        }

        [HttpPost(Name = "CreateLead")]
        [ProducesResponseType(typeof(LeadDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateLeadDto dto, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            var created = await _svc.CreateAsync(dto);
            _logger.LogInformation("Lead created {LeadId}", created.LeadId);
            return CreatedAtRoute("GetLead", new { id = created.LeadId }, created);
        }

        [HttpPut("{id:int}", Name = "UpdateLead")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateLeadDto dto, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            await _svc.UpdateAsync(id, dto);
            _logger.LogInformation("Lead updated {LeadId}", id);
            return NoContent();
        }

        [HttpPatch("{id:int}/status", Name = "TransitionLeadStatus")]
        [ProducesResponseType(typeof(LeadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> TransitionStatus(int id, [FromQuery] string to, [FromQuery] int? userId = null, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(to)) return BadRequest(new { message = "Target status 'to' is required." });
            var updated = await _svc.TransitionStatusAsync(id, to, userId);
            _logger.LogInformation("Lead {LeadId} status changed to {Status} by {User}", id, to, userId);
            return Ok(updated);
        }

        [HttpPost("{id:int}/convert", Name = "ConvertLead")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ConvertToCustomer(int id, [FromQuery] bool createDeal = false, [FromQuery] int? userId = null, CancellationToken cancellationToken = default)
        {
            var result = await _svc.ConvertToCustomerAsync(id, createDeal, userId);
            _logger.LogInformation("Lead {LeadId} converted to Customer {CustomerId}. DealCreated: {DealId}", id, result.CustomerId, result.DealId);
            return Ok(new { CustomerId = result.CustomerId, DealId = result.DealId });
        }

        [HttpDelete("{id:int}", Name = "DeleteLead")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
        {
            await _svc.SoftDeleteAsync(id);
            _logger.LogInformation("Lead soft-deleted {LeadId}", id);
            return NoContent();
        }
    }
}

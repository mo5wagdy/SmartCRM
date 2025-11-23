using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartCRM.API.Extensions;
using SmartCRM.API.Models;
using SmartCRM.Application.Dtos.Note_Dtos;
using SmartCRM.Application.Interfaces.Services;

namespace SmartCRM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class NotesController : ControllerBase
    {
        private readonly INoteService _svc;
        private readonly ILogger<NotesController> _logger;

        public NotesController(INoteService svc, ILogger<NotesController> logger)
        {
            _svc = svc;
            _logger = logger;
        }

        [HttpGet(Name = "GetNotes")]
        [ProducesResponseType(typeof(IEnumerable<NoteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string? q = null,
            [FromQuery] string? relatedTo = null,
            [FromQuery] int? relatedId = null,
            [FromQuery] int? customerId = null,
            [FromQuery] int? dealId = null,
            [FromQuery] int? userId = null,
            [FromQuery] DateTime? from = null,
            [FromQuery] DateTime? to = null,
            CancellationToken cancellationToken = default)
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 200);

            var items = (await _svc.GetAllAsync(page, pageSize, q, relatedTo, relatedId, customerId, dealId, userId, from, to)).ToList();
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

        [HttpGet("{id:int}", Name = "GetNote")]
        [ProducesResponseType(typeof(NoteDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken = default)
        {
            var item = await _svc.GetByIdAsync(id);
            if (item == null) return NotFound(new { message = $"Note {id} not found" });
            return Ok(item);
        }

        [HttpPost(Name = "CreateNote")]
        [ProducesResponseType(typeof(NoteDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateNoteDto dto, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            var created = await _svc.CreateAsync(dto);
            _logger.LogInformation("Note created {NoteId}", created.NoteId);
            return CreatedAtRoute("GetNote", new { id = created.NoteId }, created);
        }

        [HttpPut("{id:int}", Name = "UpdateNote")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateNoteDto dto, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            await _svc.UpdateAsync(id, dto);
            _logger.LogInformation("Note updated {NoteId}", id);
            return NoContent();
        }

        [HttpDelete("{id:int}", Name = "DeleteNote")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
        {
            await _svc.SoftDeleteAsync(id);
            _logger.LogInformation("Note soft-deleted {NoteId}", id);
            return NoContent();
        }
    }
}
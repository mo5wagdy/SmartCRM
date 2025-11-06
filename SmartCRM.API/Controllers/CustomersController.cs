using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartCRM.Application.Dtos.Customer_Dtos;
using SmartCRM.Application.Exceptions;
using SmartCRM.Application.Interfaces.Services;

namespace SmartCRM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _svc;
        public CustomersController(ICustomerService svc) => _svc = svc;

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int Page = 1, [FromQuery] int PageSize = 20, [FromQuery] string? Q = null)
        {
            var items = await _svc.GetAllAsync(Page, PageSize, Q);
            return Ok(items);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var item = await _svc.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCustomerDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var created = await _svc.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = created.CustomerId }, created);
        } 

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCustomerDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var ok = await _svc.UpdateAsync(id, dto);
                return ok ? NoContent() : NotFound();
            }
            catch (NotFoundException) { return NotFound(); }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var ok = await _svc.SoftDeleteAsync(id);
                return ok ? NoContent() : NotFound();
            }
            catch (NotFoundException) { return NotFound(); }
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartCRM.API.Extensions;
using SmartCRM.API.Models;
using SmartCRM.Application.Dtos.Customer_Dtos;
using SmartCRM.Application.Exceptions;
using SmartCRM.Application.Interfaces.Services;

namespace SmartCRM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _svc;
        private readonly ILogger<CustomersController> _logger;
        public CustomersController(ICustomerService svc, ILogger<CustomersController> loger)
        {
            _svc = svc;
            _logger = loger;
        } 

        [HttpGet(Name = "GetCustomer")]
        [ProducesResponseType(typeof(IEnumerable<CustomerDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery] int Page = 1, [FromQuery] int PageSize = 20, [FromQuery] string? Q = null, CancellationToken cancellationToken = default)
        {
            Page = Math.Max(1, Page);
            PageSize = Math.Clamp(PageSize, 1, 200);

            var items = await _svc.GetAllAsync(Page, PageSize, Q);
            
            var totalItems = items.Count();
            long total = totalItems < PageSize && Page == 1 ? totalItems : (long)totalItems;
            var totalPages = (int)Math.Ceiling(total / (double)PageSize);

            var meta = new PaginationMetadata
            {
                Page = Page,
                PageSize = PageSize,
                TotalCount = total,
                TotalPages = Math.Max(1, totalPages)
            };
            Response.AddPaginationHeader(meta);
            return Ok(items);
        }

        [HttpGet("{id:int}", Name = "GetCustomer")]
        [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken = default)
        {
            var item = await _svc.GetByIdAsync(id);
            if (item == null) return NotFound(new { message = $"Customer {id} Not Found"});
            return Ok(item);
        }

        [HttpPost (Name = "CreateCustomer")]
        [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Create([FromBody] CreateCustomerDto dto, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            
            var created = await _svc.CreateAsync(dto);
            _logger.LogInformation("Customer {CustomerId} created.", created.CustomerId);
            return CreatedAtRoute("GetCustomer", new { id = created.CustomerId }, created);
        } 

        [HttpPut("{id:int}", Name = "UpdateCustomer")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCustomerDto dto, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                await _svc.UpdateAsync(id, dto);
                _logger.LogInformation("Customer {CustomerId} updated.", id);
                return NoContent();
            }
            catch (NotFoundException) { return NotFound(); }
        }

        [HttpDelete("{id:int}", Name = "DeleteCustomer")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
        {
            await _svc.SoftDeleteAsync(id);
            _logger.LogInformation("Customer soft-deleted {CustomerId}", id);
            return NoContent();
        }
    }
}

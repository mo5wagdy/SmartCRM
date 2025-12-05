using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartCRM.API.Extensions;
using SmartCRM.API.Models;
using SmartCRM.Application.Dtos.Product_Dtos;
using SmartCRM.Application.Interfaces.Services;

namespace SmartCRM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _svc;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductService svc, ILogger<ProductsController> logger)
        {
            _svc = svc;
            _logger = logger;
        }

        [HttpGet(Name = "GetProducts")]
        [ProducesResponseType(typeof(IEnumerable<ProductDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string? q = null,
            [FromQuery] string? category = null,
            [FromQuery] decimal? minPrice = null,
            [FromQuery] decimal? maxPrice = null,
            [FromQuery] bool? inStock = null,
            CancellationToken cancellationToken = default)
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 200);

            var items = (await _svc.GetAllAsync(page, pageSize, q, category, minPrice, maxPrice, inStock)).ToList();
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

        [HttpGet("{id:int}", Name = "GetProduct")]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken = default)
        {
            var item = await _svc.GetByIdAsync(id);
            if (item == null) return NotFound(new { message = $"Product {id} not found" });
            return Ok(item);
        }

        [HttpPost(Name = "CreateProduct")]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create(
        [FromForm] CreateProductDto dto,
        [FromForm] IFormFile? imageFile,
        CancellationToken cancellationToken = default)
        {
            string? imagePath = null;
            if (imageFile != null && imageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Pictuers", "ProductImagesUploads");
                if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);
                var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(imageFile.FileName)}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream, cancellationToken);
                }
                imagePath = $"/wwwroot/Pictuers/ProductImagesUploads/{fileName}";
            }

            dto.ImagePath = imagePath;
            var created = await _svc.CreateAsync(dto);
            return CreatedAtRoute("GetProduct", new { id = created.ProductId }, created);
        }

        [HttpPut("{id:int}", Name = "UpdateProduct")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProductDto dto, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            await _svc.UpdateAsync(id, dto);
            _logger.LogInformation("Product updated {ProductId}", id);
            return NoContent();
        }

        [HttpDelete("{id:int}", Name = "DeleteProduct")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
        {
            await _svc.SoftDeleteAsync(id);
            _logger.LogInformation("Product soft-deleted {ProductId}", id);
            return NoContent();
        }
    }
}


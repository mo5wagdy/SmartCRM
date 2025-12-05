using AutoMapper;
using SmartCRM.Application.Dtos.Product_Dtos;
using SmartCRM.Application.Exceptions;
using SmartCRM.Application.Interfaces.Repositories;
using SmartCRM.Application.Interfaces.Services;
using SmartCRM.Domain.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCRM.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync(int page, int pageSize, string? q, string? category, decimal? minPrice, decimal? maxPrice, bool? inStock)
        {
            var products = _uow.Products.QueryNoTracking().Where(p => !p.IsDeleted);

            if (!string.IsNullOrEmpty(q))
                products = products.Where(p => p.Name.Contains(q) || (p.Description != null || p.Description.Contains(q)));
            if (minPrice.HasValue)
                products = products.Where(p => p.Price >= minPrice.Value);
            if (maxPrice.HasValue)
                products = products.Where(p => p.Price <= maxPrice.Value);
            if (inStock.HasValue)
                products = products.Where(p => inStock.Value ? p.QuantityInStock > 0 : p.QuantityInStock == 0);

            var pagedQuery = products
            .OrderByDescending(p => p.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize);

            var productsQuery = await _uow.Products.ToListAsync(pagedQuery);

            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            var product = await _uow.Products.GetByIdAsync(id);
            if (product == null || product.IsDeleted) return null;
            return _mapper.Map<ProductDto>(product);
        }

        public async Task<ProductDto> CreateAsync(CreateProductDto dto)
        {
            var product = _mapper.Map<Product>(dto);
            product.CreatedAt = DateTime.UtcNow;
            product.IsDeleted = false;
            await _uow.Products.AddAsync(product);
            await _uow.SaveAsync();
            // TODO: Audit log
            return _mapper.Map<ProductDto>(product);
        }

        public async Task UpdateAsync(int id, UpdateProductDto dto)
        {
            var product = await _uow.Products.GetByIdAsync(id);
            if (product == null || product.IsDeleted) throw new NotFoundException($"Product {id} not found");
            _mapper.Map(dto, product);
            product.UpdatedAt = DateTime.UtcNow;
            await _uow.SaveAsync();
            // TODO: Audit log
        }

        public async Task SoftDeleteAsync(int id)
        {
            var product = await _uow.Products.GetByIdAsync(id);
            if (product == null || product.IsDeleted) throw new NotFoundException($"Product {id} not found");
            product.IsDeleted = true;
            await _uow.SaveAsync();
            // TODO: Audit log
        }
    }
}

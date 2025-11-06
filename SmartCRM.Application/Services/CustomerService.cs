using AutoMapper;
using SmartCRM.Application.Dtos.Customer_Dtos;
using SmartCRM.Application.Exceptions;
using SmartCRM.Application.Interfaces.Repositories;
using SmartCRM.Application.Interfaces.Services;
using SmartCRM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SmartCRM.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public CustomerService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<CustomerDto> CreateAsync(CreateCustomerDto dto)
        {
            var entity = _mapper.Map<Customer>(dto);
            entity.CreatedAt = DateTime.UtcNow;
            await _uow.Customers.AddAsync(entity);
            await _uow.SaveAsync();
            return _mapper.Map<CustomerDto>(entity);
        }

        public async Task<IEnumerable<CustomerDto>> GetAllAsync(int Page = 1, int PageSize = 20, string? Q = null)
        {
            var query = _uow.Customers.QueryNoTracking().Where(c => !c.IsDeleted );

            if (!string.IsNullOrWhiteSpace(Q))
            {
                Q = Q.Trim().ToLower();
                query = query.Where(c => c.FullName.ToLower().Contains(Q) || c.Email.ToLower().Contains(Q) || c.Phone.ToLower().Contains(Q));
            }

            var PagedQuery = query
                .OrderByDescending(c => c.CreatedAt)
                .Skip((Page - 1) * PageSize)
                .Take(PageSize);

            var Items = await _uow.Customers.ToListAsync(PagedQuery);

            return _mapper.Map<IEnumerable<CustomerDto>>(Items);
        }

        public async Task<CustomerDto?> GetByIdAsync(int id)
        {
            var c = await _uow.Customers.GetByIdAsync(id);
            if (c == null || c.IsDeleted)
                return null;
            var dto = _mapper.Map<CustomerDto>(c);
            //dto.Score = await GetCustomerScoreAsync(CustomerId);
            return dto;
        }

        public async Task<bool> UpdateAsync(int Id, UpdateCustomerDto dto)
        {
            var existing = await _uow.Customers.GetByIdAsync(Id);
            if (existing == null || existing.IsDeleted) throw new NotFoundException($"Customer {Id} Not Found");
            _mapper.Map(dto, existing);
            existing.UpdatedAt = DateTime.UtcNow;
            await _uow.Customers.UpdateAsync(existing);
            await _uow.SaveAsync();
            return true;
        }

        public async Task<bool> SoftDeleteAsync(int Id)
        {
            var existing = await _uow.Customers.GetByIdAsync(Id);
            if (existing == null || existing.IsDeleted) throw new NotFoundException($"Customer {Id} Not Found");
            existing.IsDeleted = true;
            existing.IsActive = false;
            existing.UpdatedAt = DateTime.UtcNow;
            await _uow.Customers.UpdateAsync(existing);
            await _uow.SaveAsync();
            return true;
        }

        // Score Computed From Number of Deals, Total Value, Last Activity Date 
        public async Task<decimal> GetCustomerScoreAsync(int CustomerId, CancellationToken cancellationToken = default)
        {
            var customer = await _uow.Customers.GetByIdAsync(CustomerId);
            if (customer == null || customer.IsDeleted) throw new NotFoundException($"Customer {CustomerId} Not Found");
            
            var dealsQuery = _uow.Deals.QueryNoTracking().Where(d => d.CustomerId == CustomerId && !d.IsDeleted);
            var notesQuery = _uow.Notes.QueryNoTracking().Where(n => n.CustomerId == CustomerId && !n.IsDeleted);

            var dealCountTask =  _uow.Deals.CountAsync(dealsQuery, cancellationToken);
            var totalDealValueTask =  _uow.Deals.SumAsync(dealsQuery, d => d.Value, cancellationToken);
            var lastNoteDateTask =  _uow.Notes.MaxAsync(notesQuery, n => n.CreatedAt, cancellationToken);

            await Task.WhenAll(dealCountTask, totalDealValueTask, lastNoteDateTask);

            var dealCount = dealCountTask.Result;
            var totalDealValue = totalDealValueTask.Result ?? 0;
            var lastNoteDate = lastNoteDateTask.Result ?? customer.CreatedAt;

            const decimal dealsWeight = 0.4m;
            const decimal valueWeight = 0.4m;
            const decimal recencyWeight = 0.2m;

            const int dealScale = 10;
            const decimal valueScale = 1000m;
            const decimal recencyScaleDays = 100m;

            decimal dealScore = Math.Min(100m, dealCount * dealScale);
            decimal valueScore = Math.Min(100m, (totalDealValue / valueScale) * 10m);
            double daysSinceLastNote = (DateTime.UtcNow - lastNoteDate).TotalDays;
            decimal recencyScore = (decimal)Math.Max(0m, 100 - (decimal)daysSinceLastNote);

            decimal finalScore = (dealScore * dealsWeight) + (valueScore * valueWeight) + (recencyScore * recencyWeight);
            finalScore = Math.Round(Math.Min(100m, finalScore), 2);
            return finalScore;

        }
    }
}

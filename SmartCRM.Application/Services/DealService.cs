using AutoMapper;
using SmartCRM.Application.Dtos.Deal_Dtos;
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
    public class DealService : IDealService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        //private readonly INoteService _noteService;
        public DealService(IUnitOfWork uow, IMapper mapper/*, INoteService noteService*/)
        {
            _uow = uow;
            _mapper = mapper;
            //_noteService = noteService;
        }
        public async Task<IEnumerable<DealDto>> GetAllAsync(int Page = 1, int PageSize = 20, string? Q = null)
        {
            var query = _uow.Deals.QueryNoTracking().Where(D => !D.IsDeleted);

            if (!string.IsNullOrWhiteSpace(Q))
            {
                Q = Q.Trim().ToLower();
                query = query.Where(D => D.Title.Contains(Q));
            }

            var PagedQuery = query
            .OrderByDescending(D => D.CreatedDate)
            .Skip((Page - 1) * PageSize)
            .Take(PageSize);

            var deals = await _uow.Deals.ToListAsync(PagedQuery);

            return _mapper.Map<IEnumerable<DealDto>>(deals);
        }
        
        public async Task<DealDto?> GetByIdAsync(int Id)
        {
            var deal = _uow.Deals.GetByIdAsync(Id);
            return deal == null ? null : _mapper.Map<DealDto>(deal);
        }
        public async Task<DealDto> CreateAsync(CreateDealDto dto)
        {
            var deal = _mapper.Map<Deal>(dto);
            deal.Stage = "Lead";
            await _uow.Deals.AddAsync(deal);
            await _uow.SaveAsync();
            return _mapper.Map<DealDto>(deal);
        }

        public async Task UpdateAsync(int Id, UpdateDealDto dto)
        {
            var deal = await _uow.Deals.GetByIdAsync(Id);
            if (deal == null || deal.IsDeleted)
                throw new NotFoundException($"Deal {Id} not found");
            _mapper.Map(dto, deal);
            await _uow.Deals.UpdateAsync(deal);
            deal.UpdatedAt = DateTime.UtcNow;
            await _uow.SaveAsync();
        }

        public async Task SoftDeleteAsync(int Id)
        {
            var deal = await _uow.Deals.GetByIdAsync(Id);
            if (deal == null) throw new NotFoundException($"Deal {Id} not found");
            deal.IsDeleted = true;
            await _uow.SaveAsync();
        }

        public async Task<DealDto> ChangeStageAsync(int Id, string ToStage, int? UserId = null)
        {
            var deal = await _uow.Deals.GetByIdAsync(Id);
            
            if (deal == null || deal.IsDeleted)
                throw new NotFoundException($"Deal {Id} not found");

            var validStages = new[] { "Lead", "Qualified", "ProposalSent", "Negotiation", "ClosedWon", "ClosedLost" };
            if (!validStages.Contains(ToStage))
                throw new BusinessRuleException($"Invalid stage: {ToStage}");

            //Business rule: Can't move to Negtiation unless proposal sent
            if (ToStage == "Negotiation" && deal.Stage != "ProposalSent")
                throw new BusinessRuleException("Cannot move to Negotiation unless Proposal Sent");

            //On closedwon, decreament product stock
            if (ToStage == "ClosedWon")
            {
                foreach (var dp in deal.DealProducts)
                {
                    var product = await _uow.Products.GetByIdAsync(dp.ProductId);
                    if (product == null || product.QuantityInStock < dp.Quantity)
                        throw new BusinessRuleException($"Insufficient stock for product {product?.Name ?? dp.ProductId.ToString()}");
                    product.QuantityInStock -= dp.Quantity;
                }

                var prevStage = deal.Stage;
                deal.Stage = ToStage;
                await _uow.Deals.UpdateAsync(deal);
                await _uow.SaveAsync();

                //Log the stage change as Note
                //await _noteService.LogDealStageChangeAsync(deal.DealId, prevStage, toStage, userId);

                return _mapper.Map<DealDto>(deal);
            }
        }
    }
}

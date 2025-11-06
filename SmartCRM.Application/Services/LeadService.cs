using AutoMapper;
using SmartCRM.Application.Dtos.Lead_Dtos;
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
    public class LeadService : ILeadService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        private static readonly Dictionary<string, string[]> _allowedTransitions = new()
        {
            { "New", new[] { "Contacted", "Qualified" }  },
            { "Contacted", new[] { "Qulified", "New" } },
            { "Qualified", new[] { "Converted", "ProposalSent"} },
            { "ProposalSent", new[] { "Negotiation", "ClosedLost"} },
            { "Negotiation", new[] { "ClosedWon", "ClosedLost" } },
            { "ClosedWon", Array.Empty<string>() },
            { "ClosedLost", Array.Empty<string>() },
            { "Converted", Array.Empty<string>() }
        };

        public LeadService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<LeadDto> CreateAsync(CreateLeadDto dto)
        {
            var entity = _mapper.Map<Lead>(dto);
            entity.CreatedAt = DateTime.UtcNow;
            entity.Status = "New";
            await _uow.Leads.AddAsync(entity);
            await _uow.SaveAsync();
            return _mapper.Map<LeadDto>(entity);
        }

        public async Task<IEnumerable<LeadDto>> GetAllAsync(int Page = 1, int PageSize = 20, string? Q = null)
        {
            var query = _uow.Leads.QueryNoTracking().Where(l => l.IsDeleted);
            if (!string.IsNullOrWhiteSpace(Q))
            {
                Q = Q.Trim().ToLower();
                query = query.Where(l => l.ContactName.ToLower().Contains(Q) || l.Email.Contains(Q) || l.ContactPhone.Contains(Q));
            }

            var PagedQuery = query
                .OrderByDescending(l => l.CreatedAt)
                .Skip((Page - 1) * PageSize)
                .Take(PageSize);

            var items = await _uow.Leads.ToListAsync(PagedQuery);
            return _mapper.Map<IEnumerable<LeadDto>>(items);
        }

        public async Task<LeadDto?> GetByIdAsync(int id)
        {
            var l = await _uow.Leads.GetByIdAsync(id);
            if (l == null || l.IsDeleted)
                return null;
            return _mapper.Map<LeadDto>(l);
        }

        public async Task<bool> UpdateAsync(int Id, UpdateLeadDto dto)
        {
            var existing = await _uow.Leads.GetByIdAsync(Id);
            if (existing == null || existing.IsDeleted) throw new NotFoundException($"Lead {Id} Not Found");
            _mapper.Map(dto, existing);
            existing.UpdatedAt = DateTime.UtcNow;
            await _uow.Leads.UpdateAsync(existing);
            await _uow.SaveAsync();
            return true;
        }

        public async Task<bool> SoftDeleteAsync(int Id)
        {
            var existing = await _uow.Leads.GetByIdAsync(Id);
            if (existing == null || existing.IsDeleted) throw new NotFoundException($"Lead {Id} Not Found");
            existing.IsDeleted = true;
            existing.IsActive = false;
            existing.UpdatedAt = DateTime.UtcNow;
            await _uow.Leads.UpdateAsync(existing);
            await _uow.SaveAsync();
            return true;
        }

        public async Task<LeadDto> TransitionStatusAsync(int LeadId, string NewStatus, int? CreatedByUserId = null)
        {
            var lead = await _uow.Leads.GetByIdAsync(LeadId);
            if (lead == null || lead.IsDeleted) throw new NotFoundException($"Lead {LeadId} Not Found");

            var current = lead.Status ?? "new";
            if (!_allowedTransitions.ContainsKey(current) || !_allowedTransitions[current].Contains(NewStatus))
            {
                throw new BusinessRuleException($"Invalid Status Transition From {current} To {NewStatus}");
            }

            // Update Status
            lead.Status = NewStatus;
            lead.UpdatedAt = DateTime.UtcNow;
            await _uow.Leads.UpdateAsync(lead);

            // Create an Automatic Note Recording The Transition
            var note = new Note
            {
                Content = $"Lead Status Changed From '{current}' To '{NewStatus}'",
                RelatedTo = "Lead",
                RelatedId = lead.LeadId,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = CreatedByUserId
            };

            await _uow.Notes.AddAsync(note);
            await _uow.SaveAsync();
            return _mapper.Map<LeadDto>(lead);
        }

        public async Task<(int CustomerId, int? DealId)> ConvertToCustomerAsync(int LeadId, bool CreateInitialDeal = false, int? CreatedByUserId = null)
        {
            var lead = await _uow.Leads.GetByIdAsync(LeadId);
            if (lead == null || lead.IsDeleted) throw new NotFoundException($"Lead {LeadId} Not Found");
            if (lead.Status == "Converted") throw new BusinessRuleException($"Lead {LeadId} Is Already Converted");

            // Map Lead To Customer

            var customer = new Customer
            {
                FullName = lead.ContactName,
                Email = lead.Email,
                Phone = lead.ContactPhone,
                Address = "", // Left Blank As Lead May Not Have Address Info
                CustomerType = "Customer",
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                IsDeleted = false,
            };
            await _uow.Customers.AddAsync(customer);

            // Update Lead

            lead.Status = "Converted";
            lead.UpdatedAt = DateTime.UtcNow;
            await _uow.Leads.UpdateAsync(lead);

            int? createdDealId = null;

            if (CreateInitialDeal)
            {
                var deal = new Deal
                {
                    Title = $"Deal From Lead {lead.ContactName}",
                    Value = 0m,
                    Stage = "Lead",
                    CreatedDate = DateTime.UtcNow,
                    CustomerId = customer.CustomerId,
                    AssignedTo = CreatedByUserId,
                };
                await _uow.Deals.AddAsync(deal);
                createdDealId = deal.DealId;
            }

            // Add Note Recording The Conversion
            var note = new Note
            {
                Content = $"Lead Converted To Customer {customer.CustomerId}",
                RelatedTo = "Lead",
                RelatedId = lead.LeadId,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = CreatedByUserId
            };

            await _uow.Notes.AddAsync(note);
            await _uow.SaveAsync();
            return (customer.CustomerId, createdDealId);

        }


    }
}

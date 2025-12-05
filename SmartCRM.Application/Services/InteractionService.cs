using AutoMapper;
using SmartCRM.Application.Dtos.Interaction_Dtos;
using SmartCRM.Application.Exceptions;
using SmartCRM.Application.Interfaces.Repositories;
using SmartCRM.Application.Interfaces.Services;
using SmartCRM.Domain.Entities;

public class InteractionService : IInteractionService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public InteractionService(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<IEnumerable<InteractionDto>> GetAllAsync(
        int page, int pageSize, string? q, string? type, string? status, string? relatedTo, int? relatedId,
        int? customerId, int? dealId, int? assignedTo, DateTime? from, DateTime? to)
    {
        var query = _uow.Interactions.QueryNoTracking().Where(i => !i.IsDeleted);

        if (!string.IsNullOrEmpty(q))
            query = query.Where(i => i.Title.Contains(q) || i.Description.Contains(q));
        if (!string.IsNullOrEmpty(type))
            query = query.Where(i => i.ImteractionType == type);
        if (!string.IsNullOrEmpty(status))
            query = query.Where(i => i.Status == status);
        if (!string.IsNullOrEmpty(relatedTo))
            query = query.Where(i => i.RelatedTo == relatedTo);
        if (relatedId.HasValue)
            query = query.Where(i => i.RelatedId == relatedId);
        if (customerId.HasValue)
            query = query.Where(i => i.CustomerId == customerId);
        if (dealId.HasValue)
            query = query.Where(i => i.DealId == dealId);
        if (assignedTo.HasValue)
            query = query.Where(i => i.AssignedTo == assignedTo);
        if (from.HasValue)
            query = query.Where(i => i.InteractionDate >= from.Value);
        if (to.HasValue)
            query = query.Where(i => i.InteractionDate <= to.Value);

        var paged = query
            .OrderByDescending(i => i.InteractionDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize);

        var interactions = await _uow.Interactions.ToListAsync(paged);
        return _mapper.Map<IEnumerable<InteractionDto>>(interactions);
    }

    public async Task<InteractionDto?> GetByIdAsync(int id)
    {
        var interaction = await _uow.Interactions.GetByIdAsync(id);
        if (interaction == null || interaction.IsDeleted) return null;
        return _mapper.Map<InteractionDto>(interaction);
    }

    public async Task<InteractionDto> CreateAsync(CreateInteractionDto dto)
    {
        var entity = _mapper.Map<Interaction>(dto);
        entity.InteractionDate = DateTime.UtcNow;
        entity.IsDeleted = false;
        await _uow.Interactions.AddAsync(entity);
        await _uow.SaveAsync();
        return _mapper.Map<InteractionDto>(entity);
    }

    public async Task UpdateAsync(int id, UpdateInteractionDto dto)
    {
        var entity = await _uow.Interactions.GetByIdAsync(id);
        if (entity == null || entity.IsDeleted) throw new NotFoundException($"Interaction {id} not found");
        _mapper.Map(dto, entity);
        entity.UpdatedAt = DateTime.UtcNow;
        await _uow.SaveAsync();
    }

    public async Task SoftDeleteAsync(int id)
    {
        var entity = await _uow.Interactions.GetByIdAsync(id);
        if (entity == null || entity.IsDeleted) throw new NotFoundException($"Interaction {id} not found");
        entity.IsDeleted = true;
        await _uow.SaveAsync();
    }
}
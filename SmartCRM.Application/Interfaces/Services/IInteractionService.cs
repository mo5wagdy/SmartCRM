using SmartCRM.Application.Dtos.Interaction_Dtos;

public interface IInteractionService
{
    Task<IEnumerable<InteractionDto>> GetAllAsync(
        int page, int pageSize, string? q, string? type, string? status, string? relatedTo, int? relatedId,
        int? customerId, int? dealId, int? assignedTo, DateTime? from, DateTime? to);

    Task<InteractionDto?> GetByIdAsync(int id);
    Task<InteractionDto> CreateAsync(CreateInteractionDto dto);
    Task UpdateAsync(int id, UpdateInteractionDto dto);
    Task SoftDeleteAsync(int id);
}
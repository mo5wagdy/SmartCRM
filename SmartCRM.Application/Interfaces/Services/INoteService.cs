using SmartCRM.Application.Dtos.Note_Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCRM.Application.Interfaces.Services
{
    public interface INoteService
    {
        Task<IEnumerable<NoteDto>> GetAllAsync(int Page, int PageSize, string? Q, string? RelatedTo, int? RelatedId, int? CustomerId, int? DealId, int? UserId, DateTime? From, DateTime? To);
        Task<NoteDto?> GetByIdAsync(int Id);
        Task<NoteDto> CreateAsync(CreateNoteDto dto);
        Task UpdateAsync(int Id, UpdateNoteDto dto);
        Task SoftDeleteAsync(int Id);
        Task LogDealStageChangeAsync(int DealId, string FromStage, string ToStage, int? UserId);
        Task LogCustomerActivityAsync(int CustomerId, string Content, int? UserId);
    }
}

using SmartCRM.Application.Dtos.Lead_Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCRM.Application.Interfaces.Services
{
    public interface ILeadService
    {
        Task<IEnumerable<LeadDto>> GetAllAsync(int Page = 1, int PageSize = 20, string? Q = null);
        Task<LeadDto?> GetByIdAsync(int Id);
        Task<LeadDto> CreateAsync(CreateLeadDto dto);
        Task<bool> UpdateAsync(int Id, UpdateLeadDto dto);
        Task<bool> SoftDeleteAsync(int Id);

        // Workflow Operations
        Task<LeadDto> TransitionStatusAsync(int LeadId, string NewStatus, int? CreatedByUserId = null);
        Task<(int CustomerId, int? DealId)> ConvertToCustomerAsync(int LeadId, bool CreateInitialDeal = false, int? CreateByUserId = null);
    }
}

using SmartCRM.Application.Dtos.Deal_Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCRM.Application.Interfaces.Services
{
    public interface IDealService
    {
        Task<IEnumerable<DealDto>> GetAllAsync(int Page, int PageSize, string? Q);
        Task<DealDto?> GetByIdAsync(int Id);
        Task<DealDto> CreateAsync(CreateDealDto dto);
        Task UpdateAsync(int Id, UpdateDealDto dto);
        Task SoftDeleteAsync(int Id);
        Task<DealDto> ChangeStageAsync(int Id, string ToStage, int? UserId = null);
    }
}

using SmartCRM.Application.Dtos.Customer_Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCRM.Application.Interfaces.Services
{
    public interface ICustomerService
    {
        Task<IEnumerable<CustomerDto?>> GetAllAsync(int Page = 1, int PageSize = 20, string? Q = null);
        Task<CustomerDto?> GetByIdAsync(int Id);
        Task<CustomerDto?> CreateAsync(CreateCustomerDto dto);
        Task<bool> UpdateAsync(int Id, UpdateCustomerDto dto);
        Task<bool> SoftDeleteAsync(int Id);
        Task<decimal> GetCustomerScoreAsync(int CustomerId, CancellationToken cancellationToken = default); // => Report Helper
    }
}

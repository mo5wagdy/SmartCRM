using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCRM.Application.Dtos.Customer_Dtos
{
    public record CustomerDto
    (
        int CustomerId,
        string FullName,
        string Email,
        string Phone,
        string Address,
        string? CompanyName,
        string CustomerType,
        DateTime CreatedAt,
        DateTime? UpdatedAt,
        bool IsActive,
        bool IsDeleted,
        string Status
    );
}

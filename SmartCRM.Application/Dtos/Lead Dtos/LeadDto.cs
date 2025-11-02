using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCRM.Application.Dtos.Lead_Dtos
{
    public record LeadDto
        
    (
        int LeadId,
        string ContactName,
        string Email,
        string ContactPhone,
        string Source,
        string Status,
        string? Notes,
        DateTime CreatedAt,
        int? AssignedTo,
        int? CustomerId
    );
}

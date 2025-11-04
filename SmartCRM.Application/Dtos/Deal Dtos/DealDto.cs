using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCRM.Application.Dtos.Deal_Dtos
{
    public record DealDto 
    
    (
        int DealId,
        string Title,
        decimal Value,
        string Stage,
        DateTime ExpectedCloseDate,
        DateTime CreatedDate,
        DateTime? UpdatedAt,
        bool IsActive,
        bool IsDeleted,
        int CustomerId,
        int? AssignedTo
    );
}

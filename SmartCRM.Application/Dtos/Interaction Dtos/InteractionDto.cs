using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCRM.Application.Dtos.Interaction_Dtos
{
    public record InteractionDto
       
    (
        int InteractionId,
        string Title,
        string Description,
        DateTime InteractionDate,
        string ImteractionType,
        string Status,
        string RelatedTo,
        int RelatedId,
        int CreatedBy,
        int? AssignedTo,
        int? CustomerId,
        int? DealId,
        DateTime? UpdatedAt,
        bool IsActive,
        bool IsDeleted
    );


}

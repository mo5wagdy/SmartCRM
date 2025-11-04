using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCRM.Application.Dtos.Note_Dtos
{
    public record NoteDto
    
    (
        int NoteId,
        string Content,
        string RelatedTo,
        int RelatedId,
        DateTime CreatedAt,
        DateTime? UpdatedAt,
        bool IsActive,
        bool IsDeleted,
        int? CustomerId,
        int? DealId,
        int? UserId
    );
}

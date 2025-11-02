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
        int? CustomerId,
        int? DealId,
        int? ContactId,
        int? UserId
    );
}

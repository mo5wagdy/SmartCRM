using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCRM.Application.Dtos.Ticket_Dtos
{
    public record TicketDto

    (
        int TicketID,
        string Title,
        string Description,
        string Status,
        string Priority,
        DateTime CreatedAt,
        DateTime? UpdatedAt,
        bool IsActive,
        bool IsDeleted,
        int CustomerId,
        int? AssignedTo
    );
}

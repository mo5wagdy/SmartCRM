using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCRM.Application.Dtos.Company_Dtos
{
    public record CompanyDto
    (
        int CompanyId,
        string CompanyName,
        string Address,
        string Industry,
        string Email,
        string Phone,
        DateTime CreatedDate,
        DateTime? UpdatedAt,
        bool IsActive,
        bool IsDeleted
    );
}

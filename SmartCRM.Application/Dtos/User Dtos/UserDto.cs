using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCRM.Application.Dtos.User_Dtos
{
    public record UserDto
    (
        int UserId,
        string FullName,
        string Email,
        string Phone,
        string Role,
        string Status,
        DateTime CreatedAt,
        DateTime? UpdatedAt,
        bool IsActive,
        string? ImagePath
    );
}

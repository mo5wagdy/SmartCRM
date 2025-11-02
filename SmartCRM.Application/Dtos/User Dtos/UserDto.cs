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
        string UserName,
        string Email,
        string Phone,
        string Role,
        DateTime CreatedAt,
        string Status
    );
}

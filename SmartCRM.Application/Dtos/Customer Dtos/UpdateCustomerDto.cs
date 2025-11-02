using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCRM.Application.Dtos.Customer_Dtos
{
    public class UpdateCustomerDto : CreateCustomerDto
    {
        public string Status { get; set; } = "Active";
    }
}

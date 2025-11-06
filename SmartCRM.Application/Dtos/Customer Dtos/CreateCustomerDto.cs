using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCRM.Application.Dtos.Customer_Dtos
{
    public class CreateCustomerDto
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string? CompanyName { get; set; }
        public string CustomerType { get; set; } = "Indiviual";
        public decimal Score { get; set; } = 0m; // => Only For Read == Implemented Only In Service
    }
}

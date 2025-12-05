using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCRM.Application.Dtos.Customer_Dtos
{
    public record class CustomerDto
    {
        public CustomerDto()
        {
        }

        public int CustomerId { get; init;}
        public string FullName { get; init; }
        public string Email { get; init; }
        public string Phone { get; init; }
        public string Address { get; init; }
        string? CompanyName { get; init; }
        public string CustomerType { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime? UpdatedAt { get; init; }
        public bool IsActive { get; init; }
        public bool IsDeleted { get; init; }
        public string Status { get; init; }
        public decimal Score { get; init; }
    }
}

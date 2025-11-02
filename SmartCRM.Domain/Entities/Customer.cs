using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCRM.Domain.Entities
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string? CompanyName { get; set; }
        public string CustomerType { get; set; } = "Individual"; // Individual, Business
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = "Active"; // Active, Inactive

        //Relationships
        public ICollection<Lead>? Leads { get; set; }
        public ICollection<Deal>? Deals { get; set; }
        public ICollection<Ticket>? Tickets { get; set; }
        public ICollection<Note>? Notes { get; set; }
        public ICollection<Interaction>? Interactions { get; set; }
    }
}

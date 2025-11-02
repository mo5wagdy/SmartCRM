using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCRM.Domain.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = "User"; // => Admin, Sales, Support, Manager
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = "Active"; // Active, Inactive

        //Navigation Properties
        public ICollection<Interaction>? Interactions { get; set; }

        //Relationships
        public ICollection<Lead>? Leads { get; set; }
        public ICollection<Deal>? Deals { get; set; }
        public ICollection<Ticket>? Tickets { get; set; }
    }
}

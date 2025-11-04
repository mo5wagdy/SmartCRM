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
        public string Status { get; set; } = "Active"; // Active, Inactive
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; } = true;  
        public bool IsDeleted { get; set; } = false;
        public string? ImagePath { get; set; }

        //Navigation Properties
        public ICollection<Interaction>? Interactions { get; set; }
        public ICollection<Lead>? Leads { get; set; }
        public ICollection<Deal>? Deals { get; set; }
        public ICollection<Ticket>? Tickets { get; set; }
        public ICollection<Note>? Notes { get; set; }
    }
}

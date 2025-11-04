using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCRM.Domain.Entities
{
    public class Lead
    {
        public int LeadId { get; set; }
        public string ContactName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string ContactPhone { get; set; } = string.Empty;
        public string Source { get; set; } = string.Empty; // => eg. Website, Call
        public string Status { get; set; } = "New"; // New, Contacted, Qualifieds
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;

        //Foreign Keys
        public int? AssignedTo { get; set; }
        public User? User { get; set; }
        public int? CustomerId { get; set; }
        public Customer? Customer { get; set; }
    }
}

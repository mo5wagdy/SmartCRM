using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SmartCRM.Domain.Entities
{
    public class Ticket
    {
        public int TicketID { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = "Open"; //in progress, resolved, closed
        public string Priority { get; set; } = "Medium"; //low, medium, high
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        //Foreign Keys
        public int CustomerID { get; set; }
        public Customer? Customer { get; set; }
        public int? AssignedTo { get; set; }
        public User User { get; set; }
    }
}

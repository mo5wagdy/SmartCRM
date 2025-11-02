using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCRM.Domain.Entities
{
    public class Interaction
    {
        public int InteractionId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime InteractionDate { get; set; } = DateTime.UtcNow;
        public string ImteractionType { get; set; } = "Call"; // Call, Meeting, Email, Task
        public string Status { get; set; } = "Scheduled"; // Scheduled, Completed, Canceled

        //Related entities info
        public string RelatedTo { get; set; }
        public int RelatedId { get; set; }

        //Created By 
        public int CreatedBy { get; set; }

        //Foreign Keys
        public int? AssignedTo { get; set; }
        public User? User { get; set; }
        public int? CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public int? DealId { get; set; }
        public Deal? Deal { get; set; }
    }
}

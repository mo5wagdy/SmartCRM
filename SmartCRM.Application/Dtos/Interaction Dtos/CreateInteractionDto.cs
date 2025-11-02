using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCRM.Application.Dtos.Interaction_Dtos
{
    public class CreateInteractionDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime InteractionDate { get; set; } = DateTime.UtcNow;
        public string ImteractionType { get; set; } = "Call";
        public string Status { get; set; } = "Scheduled";
        public string RelatedTo { get; set; } = string.Empty;
        public int RelatedId { get; set; }
        public int CreatedBy { get; set; }
        public int? AssignedTo { get; set; }
        public int? CustomerId { get; set; }
        public int? DealId { get; set; }
    }
}

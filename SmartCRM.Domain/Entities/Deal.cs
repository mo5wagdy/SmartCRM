using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCRM.Domain.Entities
{
    public class Deal
    {
        public int DealId { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal Value { get; set; }
        public string Stage { get; set; } = "Negotiating"; // => Prospecting, Nogitiating, Won, Lost
        public DateTime ExpectedCloseDate { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;

        //Foreign Keys
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public int? AssignedTo { get; set; }
        public User? User { get; set; }

        //Navigation Properties
        public ICollection<Note> Notes { get; set; }
        public ICollection<Interaction> Interactions { get; set; }
        public ICollection<DealProduct> DealProducts { get; set; }
    }
}

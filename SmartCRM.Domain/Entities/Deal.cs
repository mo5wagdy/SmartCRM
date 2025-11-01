using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCRM.Domain.Entities
{
    public class Deal
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Status { get; set; } = "Pending"; // => Pending, Won, Lost
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCRM.Domain.Entities
{
    public class Note
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

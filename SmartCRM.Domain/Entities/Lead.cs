using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCRM.Domain.Entities
{
    public class Lead
    {
        public int Id { get; set; }
        public string Source { get; set; } = string.Empty; // => eg. Website, Call
        public string ContactName { get; set; } = string.Empty;
        public string ContactPhone { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

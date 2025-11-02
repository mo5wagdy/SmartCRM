using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCRM.Application.Dtos.Lead_Dtos
{
    public class CreateLeadDto
    {
        public string ContactName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string ContactPhone { get; set; } = string.Empty;
        public string Source { get; set; } = string.Empty;
        public string Status { get; set; } = "New";
        public string? Notes { get; set; }
        public int? AssignedTo { get; set; }
        public int? CustomerId { get; set; }
    }
}

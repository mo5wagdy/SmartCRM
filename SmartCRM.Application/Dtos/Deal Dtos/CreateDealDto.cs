using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCRM.Application.Dtos.Deal_Dtos
{
    public class CreateDealDto
    {
        public string Title { get; set; } = string.Empty;
        public decimal Value { get; set; }
        public string Stage { get; set; } = "Negotiating";
        public DateTime ExpectedCloseDate { get; set; }
        public int CustomerId { get; set; }
        public int? AssignedTo { get; set; }
    }
}

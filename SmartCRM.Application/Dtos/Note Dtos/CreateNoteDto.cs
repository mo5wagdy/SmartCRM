using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCRM.Application.Dtos.Note_Dtos
{
    public class CreateNoteDto
    {
        public string Content { get; set; } = string.Empty;
        public string RelatedTo { get; set; } = string.Empty;
        public int RelatedId { get; set; }
        public int? CustomerId { get; set; }
        public int? DealId { get; set; }
        public int? UserId { get; set; }
    }
}

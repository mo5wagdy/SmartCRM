using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCRM.Domain.Entities
{
    public class Note
    {
        public int NoteId { get; set; }
        public string Content { get; set; } = string.Empty;
        public string RelatedTo { get; set; }
        public int RelatedId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        //Foreign Keys
        public int? CreatedBy { get; set; }

        //Optional Foreign Keys
        public Customer? Customer { get; set; }
        public User? User { get; set; }
    }
}

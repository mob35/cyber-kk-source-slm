using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLM.Resource.Data
{
    public class NoteHistoryData
    {
        public string No { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string TicketId { get; set; }
        public string CreateBy { get; set; }
        public string NoteDetail { get; set; }
        public string EmailSubject { get; set; }
        public bool? SendEmailFlag { get; set; }
    }
}

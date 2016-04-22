using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLM.Resource.Data
{
    public class BranchData
    {
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public string StartTimeHour { get; set; }
        public string StartTimeMinute { get; set; }
        public string EndTimeHour { get; set; }
        public string EndTimeMinute { get; set; }
        public string ChannelId { get; set; }
        public string ChannelDesc { get; set; }
        public string Status { get; set; }
        public string StatusDesc { get; set; }
    }
}

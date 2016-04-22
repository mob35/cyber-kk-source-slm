using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;

namespace SLM.Resource.Data
{

    [Serializable]
    public class CampaignMasterData
    {
        public int slm_CampaignId { get; set; }
        public string slm_CampaignCode { get; set; }
        public string slm_CampaignName { get; set; }

        public int? Is_Delete { get; set; }
        public string slm_UpdatedBy { get; set; }
        public DateTime? slm_UpdatedDate { get; set; }
        public string slm_CreatedBy { get; set; }
        public DateTime? slm_CreatedDate { get; set; }
    }
}

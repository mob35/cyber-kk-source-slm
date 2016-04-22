using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SLM.Dal;
using SLM.Resource.Data;
using SLM.Resource;
using SLM.Dal.Models;

namespace SLM.Biz
{
    public class CampaignMasterBiz
    {
        public static List<CampaignMasterData> GetCampaignMasterList()
        {
            KKSlmMsCampaignMasterModel search = new KKSlmMsCampaignMasterModel();
            //string createDate = data.CreatedDate.Year != 1 ? data.CreatedDate.Year + data.CreatedDate.ToString("-MM-dd") : string.Empty;
            //string assignDate = data.AssignedDate.Year != 1 ? data.AssignedDate.Year + data.AssignedDate.ToString("-MM-dd") : string.Empty;
            return search.GetCampaignMasterList();
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using SLM.Resource.Data;
using System.Configuration;
using SLM.Resource;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Transactions;

namespace SLM.Dal.Models
{
    public class KKSlmMsCampaignMasterModel
    {

        private SLM_DBEntities slmdb = null;
        private string _errorMessage = "";

        public string ErrorMessage
        {
            get { return _errorMessage; }
        }

        public KKSlmMsCampaignMasterModel()
        {
            slmdb = new SLM_DBEntities();
        }

        public List<CampaignMasterData> GetCampaignMasterList()
        {
            try
            {
                string sql = @"select * from kkslm_ms_campaign_master";
                string whr = @"Is_Deleted = 0";
                sql += (whr == "" ? "" : " WHERE " + whr);
                //sql += " ORDER BY seq";

                return slmdb.ExecuteStoreQuery<CampaignMasterData>(sql).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

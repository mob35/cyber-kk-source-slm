using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SLM.Resource.Data;
using SLM.Resource;

namespace SLM.Dal.Models
{
    public class KKSlmMsCampaignModel
    {
        private SLM_DBEntities slmdb = null;

        public KKSlmMsCampaignModel()
        {
            slmdb = new SLM_DBEntities();
        }

        public List<ControlListData> GetAllActiveCampaignData()
        {
            return slmdb.kkslm_ms_campaign.Where(p => p.is_Deleted == 0 && p.slm_Status == "A").OrderBy(p => p.slm_Seq).Select(p => new ControlListData { TextField = p.slm_CampaignName, ValueField = p.slm_CampaignId }).ToList();
        }

        public List<ControlListData> GetCampaignData()
        {
            //return slmdb.kkslm_ms_campaign.Where(p => p.is_Deleted == 0 && DateTime.Today >= p.slm_StartDate && DateTime.Today <= p.slm_EndDate && p.slm_Status == "A").OrderBy(p => p.slm_Seq).Select(p => new ControlListData { TextField = p.slm_CampaignName, ValueField = p.slm_CampaignId }).ToList();
            return slmdb.kkslm_ms_campaign.Where(p => p.is_Deleted == 0 && p.slm_Status == "A" && p.slm_CampaignType == SLMConstant.CampaignType.Mass).OrderBy(p => p.slm_Seq).Select(p => new ControlListData { TextField = p.slm_CampaignName, ValueField = p.slm_CampaignId }).ToList();
        }

        public List<ControlListData> GetCampaignEditData()
        {
            //return slmdb.kkslm_ms_campaign.Where(p => p.is_Deleted == 0 && DateTime.Today >= p.slm_StartDate && DateTime.Today <= p.slm_EndDate && p.slm_Status == "A").OrderBy(p => p.slm_Seq).Select(p => new ControlListData { TextField = p.slm_CampaignName, ValueField = p.slm_CampaignId }).ToList();
            return slmdb.kkslm_ms_campaign.Where(p => p.is_Deleted == 0).OrderBy(p => p.slm_Seq).Select(p => new ControlListData { TextField = p.slm_CampaignName, ValueField = p.slm_CampaignId }).ToList();
        }

        public List<CampaignWSData> GetCampaignPopupData()
        {
            var data = (from campaign in slmdb.kkslm_ms_campaign.Where(p => p.is_Deleted == 0 && DateTime.Today >= p.slm_StartDate && DateTime.Today <= p.slm_EndDate && p.slm_Status == "A").OrderBy(p => p.slm_Seq)
                        select new CampaignWSData { CampaignId = campaign.slm_CampaignId, CampaignName = campaign.slm_CampaignName, CampaignDetail= campaign.slm_Offer+": " + campaign.slm_criteria }).ToList();
            return data;
        }
        public List<CampaignWSData> GetCampaignPopupData(string CampaignList)
        {
            string[] camList =  CampaignList.Split(',') ;
            string[] statusList = { "A", "X" };
            //string[] statusList = { "A" };
            var data = (from campaign in slmdb.kkslm_ms_campaign.Where(p => p.is_Deleted == 0 && camList.Contains(p.slm_CampaignId) == false && statusList.Contains(p.slm_Status) == true).OrderBy(p => p.slm_Seq)
                        select new CampaignWSData { CampaignId = campaign.slm_CampaignId, CampaignName = campaign.slm_CampaignName, CampaignDetail = campaign.slm_Offer + campaign.slm_criteria }).ToList();
            return data;
        }

        public CampaignDetail GetCampaignDetail(string CampaignId)
        {
            return slmdb.kkslm_ms_campaign.Where(p => p.slm_CampaignId == CampaignId && p.is_Deleted == 0).OrderBy(p => p.slm_Seq).Select(p => new CampaignDetail { Offer = p.slm_Offer, Criteria = p.slm_criteria }).FirstOrDefault();
        }

        public CampaignWSData GetCampaign(string campaignId)
        {
            return slmdb.kkslm_ms_campaign.Where(p => p.slm_CampaignId == campaignId).Select(p => new CampaignWSData { CampaignName = p.slm_CampaignName, CampaignDetail = p.slm_Offer + " : " + p.slm_criteria }).FirstOrDefault();
        }

        //========================================= SLM 5 ==============================================================================
        public List<ControlListData> GetCampaignList(string campaignType)
        {
            if (!string.IsNullOrEmpty(campaignType))
                return slmdb.kkslm_ms_campaign.Where(p => p.is_Deleted == 0 && p.slm_Status == "A" && p.slm_CampaignType == campaignType).OrderBy(p => p.slm_Seq).Select(p => new ControlListData { TextField = p.slm_CampaignName, ValueField = p.slm_CampaignId }).ToList();
            else
                return slmdb.kkslm_ms_campaign.Where(p => p.is_Deleted == 0 && p.slm_Status == "A").OrderBy(p => p.slm_Seq).Select(p => new ControlListData { TextField = p.slm_CampaignName, ValueField = p.slm_CampaignId }).ToList();
        }
    }
}

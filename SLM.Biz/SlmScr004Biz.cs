using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SLM.Dal.Models;
using SLM.Resource.Data;

namespace SLM.Biz
{
    public class SlmScr004Biz
    {
        public static LeadData SearchSCR004Data(string ticketid)
        {
            SearchLeadModel search = new SearchLeadModel();
            return search.SearchSCR004Data(ticketid);
        }

        public static List<CampaignWSData> GetCampaignFinalData(string ticketid)
        {
            SearchLeadModel search = new SearchLeadModel();
            return search.GetCampaignFinalList(ticketid);
        }

        public static void InsertCampaginFinalList(List<CampaignWSData> ListCampaign, string username)
        {
            KKSLMTrCampaignFinalModel final = new KKSLMTrCampaignFinalModel();
            final.InsertCampaignList(ListCampaign, username);
        }

        public static List<ProductData> GetBundleProduct(string campaignId)
        {
            KKSlmProductBundleConfigModel bundle = new KKSlmProductBundleConfigModel();
            return bundle.GetBundleProduct(campaignId);
        }

        public static List<ProductData> SearchCampaignViewPage(string productGroupId, string productId, string campaignId, string bundleCamIdList)
        {
            CmtCampaignProductModel campaign = new CmtCampaignProductModel();
            return campaign.SearchCampaignViewPage(productGroupId, productId, campaignId, bundleCamIdList);
        }

        public static List<ProductData> SearchCampaign(string searchWord, string bundleCamIdList)
        {
            CmtCampaignProductModel campaign = new CmtCampaignProductModel();
            return campaign.SearchCampaign(searchWord, bundleCamIdList);
        }

        public static List<ProductData> GetProductCampaignData(string campaignId)
        {
            CmtCampaignProductModel campaign = new CmtCampaignProductModel();
            return campaign.GetProductCampaignData(campaignId);
        }

        public static void InsertCampaginFinalList(List<ProductData> productList, string ticketId, string username)
        {
            KKSLMTrCampaignFinalModel final = new KKSLMTrCampaignFinalModel();
            final.InsertCampaignList(productList, ticketId, username);
        }

        public static List<StaffData> GetChannelStaffData(string username)
        {
            KKSlmMsStaffModel staff = new KKSlmMsStaffModel();
            return staff.GetChannelStaffData(username);
        }

        public static List<ProductData> GetProductCampaignDataForCmt(string campaignId)
        {
            CmtCampaignProductModel campaign = new CmtCampaignProductModel();
            return campaign.GetProductCampaignDataForCmt(campaignId);
        }

        public static List<ControlListData> GetCampaignDataViewPage(string productGroupId, string productId, string CmtCampaignIdList)
        {
            CmtCampaignProductModel campaign = new CmtCampaignProductModel();
            return campaign.GetCampaignDataViewPage(productGroupId, productId, CmtCampaignIdList);
        }
    }
}

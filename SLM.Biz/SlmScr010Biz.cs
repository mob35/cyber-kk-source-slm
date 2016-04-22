using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SLM.Dal.Models;
using SLM.Resource.Data;

namespace SLM.Biz
{
    public class SlmScr010Biz
    {
        public static List<ControlListData> GetBranchData()
        {
            KKSlmMsBranchModel branch = new KKSlmMsBranchModel();
            return branch.GetBranchData();
        }

        public static List<ControlListData> GetOccupationData(bool useWebservice)
        {
            KKSlmMsOccupationModel occupation = new KKSlmMsOccupationModel();
            return occupation.GetOccupationData(useWebservice);
        }

        public static List<ControlListData> GetTambolData(string provinceCode, string amphurCode, bool useWebservice)
        {
            KKSlmMsTambolModel tambol = new KKSlmMsTambolModel();
            return tambol.GetTambolData(provinceCode, amphurCode, useWebservice);
        }

        public static List<ControlListData> GetAmphurData(string provinceCode)
        {
            KKSlmMsAmphurModel amphur = new KKSlmMsAmphurModel();

            return amphur.GetAmphurData(provinceCode);

        }

        public static List<ControlListData> GetProvinceData()
        {
            KKSlmMsProvinceModel province = new KKSlmMsProvinceModel();
            return province.GetProvinceData();
        }

        public static List<ControlListData> GetBrandData()
        {
            KKSlmMsBrandModel brand = new KKSlmMsBrandModel();
            return brand.GetBrandData();
        }

        public static List<ControlListData> GetModelData(string brandcode)
        {
            KKSlmMsModelModel model = new KKSlmMsModelModel();
            return model.GetModelData(brandcode);
        }

        public static List<ControlListData> GetPaymentTypeData(bool useWebservice)
        {
            KKSlmMsPaymentTypeModel payment = new KKSlmMsPaymentTypeModel();
            return payment.GetPaymentTypeData(useWebservice);
        }
        public static List<ControlListData> GetAccTypeData(bool useWebservice)
        {
            KKSlmMsModuleModel module = new KKSlmMsModuleModel();
            return module.GetModuleData(useWebservice);
        }
        public static List<ControlListData> GetAccPromotionData(bool useWebservice)
        {
            KKSlmMsPromotionModel promotion = new KKSlmMsPromotionModel();
            return promotion.GetPromotionData(useWebservice);
        }

        public static LeadData GetLeadData(string ticketid)
        {
            SearchLeadModel search = new SearchLeadModel();
            return search.SearchSCR004Data(ticketid);
        }

        public static string GetBranchData(string username)
        {
            KKSlmMsStaffModel staff = new KKSlmMsStaffModel();
            return staff.GetBrachCode(username);
        }

        public static List<ControlListData> GetSubModelData(string BrandCode, string Family, bool useWebservice)
        {
            KKSlmMsSubmodelModel submodel = new KKSlmMsSubmodelModel();
            return submodel.GetSubModelData(BrandCode, Family, useWebservice);
        }
        public static List<ControlListData> GetPlanBancData()
        {
            KKSlmMsPlanBancModel plan = new KKSlmMsPlanBancModel();
            return plan.GetPlanBancData();
        }
        public static string GetStaffIdData(string username)
        {
            KKSlmMsStaffModel staff = new KKSlmMsStaffModel();
            return staff.GetStaffIdData(username);
        }
        public static string GetOwner(string ticketId)
        {
            KKSlmTrLeadModel lead = new KKSlmTrLeadModel();
            return lead.GetOwner(ticketId);
        }

        public static string GetStaffNameData(string username)
        {
            KKSlmMsStaffModel staff = new KKSlmMsStaffModel();
            return staff.GetStaffNameData(username);
        }

        public static List<CampaignWSData> GetCampaignData()
        {
            KKSlmMsCampaignModel campaign = new KKSlmMsCampaignModel();
            return campaign.GetCampaignPopupData();
        }
        public static StaffData GetStaffData(string username)
        {
            KKSlmMsStaffModel staff = new KKSlmMsStaffModel();
            return staff.GetStaffData(username);
        }

        public static string GetCampaignDetail(string campaignId)
        {
            KKSlmMsCampaignModel cam = new KKSlmMsCampaignModel();
            CampaignDetail cDetail = cam.GetCampaignDetail(campaignId);
            if (cDetail != null)
            {
                if (string.IsNullOrEmpty(cDetail.Offer) == true && string.IsNullOrEmpty(cDetail.Criteria) == true)
                    return "";
                else if (string.IsNullOrEmpty(cDetail.Offer) == false && string.IsNullOrEmpty(cDetail.Criteria) == true)
                    return cDetail.Offer;
                else if (string.IsNullOrEmpty(cDetail.Offer) == true && string.IsNullOrEmpty(cDetail.Criteria) == false)
                    return cDetail.Criteria;
                else
                    return cDetail.Offer + ": " + cDetail.Criteria;
            }
            else
                return "";
        }

        public static List<ControlListData> GetOwnerListByCampaignIdAndBranch(string campaignId, string branchcode)
        {
            KKSlmMsStaffModel staff = new KKSlmMsStaffModel();
            return staff.GetOwnerListByCampaignIdAndBranch(campaignId,branchcode);
        }

        public static bool PassPrivilegeCampaign(int flag,string campaignId, string username)
        {
            //ใช้หลายหน้าจอ
            KKSlmMsStaffModel staff = new KKSlmMsStaffModel();
            return staff.PassPrivilegeCampaign(flag,campaignId, username);
        }

        public static string GetAssignFlag(string ticketId)
        {
            KKSlmTrLeadModel lead = new KKSlmTrLeadModel();
            return lead.GetAssignFlag(ticketId);
        }

        public static string GetChannelDefault(string username)
        {
            KKSlmMsBranchModel branch = new KKSlmMsBranchModel();
            return branch.GetChannelStaffData(username);
        }

        public static List<BranchData> GetBranchAccessRight(string campaignID)
        {
            return new List<BranchData>();
        }

        public static List<ControlListData> GetBranchListByAccessRight(int flag,string campaignId)
        {
            KKSlmMsBranchModel branch = new KKSlmMsBranchModel();
            return branch.GetBranchAccessRightList(flag,campaignId);
        }

        public static bool CheckStaffAccessRightExist(string campaignId, string branch, string username)
        {
            KKSlmMsStaffModel staff = new KKSlmMsStaffModel();
            return staff.CheckStaffAccessRightExist(campaignId, branch, username);
        }

        //public static bool CheckBranchIsEdit(string branch)
        //{
        //    KKSlmMsStaffModel staff = new KKSlmMsStaffModel();
        //    return staff.CheckStaffAccessRightExist(campaignId, branch, username);
        //}
        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SLM.Dal.Models;
using SLM.Resource.Data;
using System.Transactions;

namespace SLM.Biz
{
    public class SlmScr018Biz
    {
        public static List<CampaignWSData> GetCampaignData()
        {
            KKSlmMsCampaignModel cModel = new KKSlmMsCampaignModel();
            return cModel.GetCampaignPopupData();
        }

        public static StaffDataManagement GetStaffData(int staffId)
        {
            KKSlmMsStaffModel sModel = new KKSlmMsStaffModel();
            return sModel.GetStaffDataForInsert(staffId);
        }

        //public static List<StaffGroupData> GetStaffGroupData(string staffid)
        //{
        //    KKSlmMsStaffGroupModel gModel = new KKSlmMsStaffGroupModel();
        //    return gModel.GetStaffGroupData(int.Parse(staffid));
        //}

        public static List<CampaignWSData> GetCampaignData(string CampaignList)
        {
            KKSlmMsCampaignModel campaign = new KKSlmMsCampaignModel();
            return campaign.GetCampaignPopupData(CampaignList);
        }

        //public static void InsertStaffGroup(List<StaffGroupData> ListStaffGroup, string username)
        //{
        //    KKSlmMsStaffGroupModel  sg = new KKSlmMsStaffGroupModel();
        //    sg.InsertStaffGroupList(ListStaffGroup, username);
        //}

        //public static void DeleteStaffGroup(decimal StaffGroupId, string username)
        //{
        //    KKSlmMsStaffGroupModel sg = new KKSlmMsStaffGroupModel();
        //    sg.UpdateStaffGroupList(StaffGroupId, username);
        //}
        public static List<SearchLeadResult> GetLeadOwnerDataTab18_1(string owner)
        {
            KKSlmTrLeadModel lead = new KKSlmTrLeadModel();
            return lead.GetLeadOwnerDataTab18_1(owner);
        }
        public static List<SearchLeadResult> GetLeadDelegateDataTab18_2(string DelegateUser)
        {
            KKSlmTrLeadModel lead = new KKSlmTrLeadModel();
            return lead.GetLeadDelegateDataTab18_2(DelegateUser);
        }

        public static StaffDataManagement GetStaffDataByEmpcode(string empcode,string dept)
        {
            KKSlmMsStaffModel sModel = new KKSlmMsStaffModel();
            return sModel.GetStaffDataByEmpcode(empcode, dept);
        }

        public static bool CheckExistLeadOnHand(string username)
        {
            KKSlmTrLeadModel lead = new KKSlmTrLeadModel();
            return lead.CheckExistLeadOnHand(username);
        }

        public static void UpdateTransferOwnerLead(List<string> ticketList, string newowner,int staffid, string username,string branchcode,string Oldowner)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
                {
                    KKSlmTrLeadModel lead = new KKSlmTrLeadModel();
                    lead.UpdateTransferLeadOwner(ticketList, newowner, staffid, username, branchcode);

                    KKSlmTrActivityModel act = new KKSlmTrActivityModel();
                    act.InsertDataForTransfer(ticketList, Oldowner, newowner, username, "", "");

                    ts.Complete();
                }
            }
            
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void UpdateTransferDelegateLead(List<string> ticketList, string newDelegate, int staffid, string username, string branchcode,string OldDelegate)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
                {
                    KKSlmTrLeadModel lead = new KKSlmTrLeadModel();
                    lead.UpdateTransferLeadDelegate(ticketList, newDelegate, staffid, username, branchcode);

                    KKSlmTrActivityModel act = new KKSlmTrActivityModel();
                    act.InsertDataForTransfer(ticketList, "", "", username, OldDelegate, newDelegate);

                    ts.Complete();
                }
            }
            
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

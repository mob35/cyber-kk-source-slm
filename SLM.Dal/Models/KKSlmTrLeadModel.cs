using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using SLM.Resource.Data;
using SLM.Resource;

namespace SLM.Dal.Models
{
    public class KKSlmTrLeadModel
    {
        private SLM_DBEntities slmdb = null;
        private string SLMDBName = ConfigurationManager.AppSettings["SLMDBName"] != null ? ConfigurationManager.AppSettings["SLMDBName"] : "SLMDB";

        public KKSlmTrLeadModel()
        {
            slmdb = new SLM_DBEntities();
        }

        public KKSlmTrLeadModel(SLM_DBEntities db)
        {
            slmdb = db;
        }

        public void InsertData(LeadData leadData, string createByUsername, DateTime createDate)
        {
            try
            {
                kkslm_tr_lead lead = new kkslm_tr_lead();
                lead.slm_ticketId = leadData.TicketId;
                lead.slm_Name = leadData.Name;
                lead.slm_LastName = leadData.LastName;
                lead.slm_TelNo_1 = leadData.TelNo_1;
                //lead.slm_Ext_1 = leadData.Ext_1; 
                lead.slm_CampaignId = leadData.CampaignId;

                if(!string.IsNullOrEmpty(leadData.Owner_Branch))
                    lead.slm_Owner_Branch = leadData.Owner_Branch;

                if (!string.IsNullOrEmpty(leadData.Owner))
                {
                    lead.slm_Owner = leadData.Owner;
                    lead.slm_Owner_Position = GetPositionId(leadData.Owner, slmdb);
                }

                lead.slm_Delegate = leadData.Delegate;
                lead.slm_Status = "00";
                lead.slm_StatusBy = createByUsername;
                lead.slm_StatusDate = createDate;
                lead.slm_AvailableTime = leadData.AvailableTime;
                if (leadData.StaffId != null)
                    lead.slm_StaffId = Convert.ToInt32("0" + leadData.StaffId);
                if (leadData.counting != null)
                    lead.slm_Counting = Convert.ToDecimal(leadData.counting);
                lead.slm_EmailFlag = leadData.EmailFlag;
                lead.slm_SmsFlag = leadData.SmsFlag;
                lead.slm_ChannelId = leadData.ChannelId;
                lead.slm_CreatedBy = createByUsername;
                lead.slm_CreatedBy_Position = GetPositionId(createByUsername, slmdb);
                lead.slm_CreatedDate = createDate;
                lead.slm_CreatedBy_Branch = leadData.CreatedBy_Branch;
                lead.slm_UpdatedBy = createByUsername;
                lead.slm_UpdatedDate = createDate;
                lead.slm_Delegate_Flag = 0;
                lead.slm_AssignedFlag = "0";
                lead.slm_Product_Group_Id = leadData.ProductGroupId;
                lead.slm_Product_Id = leadData.ProductId;
                lead.slm_Product_Name = leadData.ProductName;
                lead.slm_ContractNoRefer = leadData.ContractNoRefer;

                slmdb.kkslm_tr_lead.AddObject(lead);
                slmdb.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private int? GetPositionId(string username, SLM_DBEntities slmdb)
        {
            KKSlmMsStaffModel staff = new KKSlmMsStaffModel(slmdb);
            return staff.GetPositionId(username);
        }

        public void UpdateData(LeadData leadData, string UserId, bool actDelegate, bool actOwner, DateTime updateDate)
        {
            var lead = slmdb.kkslm_tr_lead.Where(p => p.slm_ticketId == leadData.TicketId).FirstOrDefault();
            if (lead != null)
            {
                try
                {
                    lead.slm_ticketId = leadData.TicketId;
                    lead.slm_Name = leadData.Name;
                    lead.slm_LastName = leadData.LastName;
                    lead.slm_TelNo_1 = leadData.TelNo_1;
                    //lead.slm_Ext_1 = leadData.Ext_1; 
                    lead.slm_CampaignId = leadData.CampaignId;

                    //lead.slm_Status = leadData.Status;
                    //if (leadData.StatusDate != null)
                    //    lead.slm_StatusDate = leadData.StatusDate;
                    lead.slm_AvailableTime = leadData.AvailableTime;
                    if (leadData.StaffId != null)
                        lead.slm_StaffId = Convert.ToInt32("0" + leadData.StaffId);
                    else
                        lead.slm_StaffId = null;

                    lead.slm_ChannelId = leadData.ChannelId;

                    if (actDelegate)
                    {
                        lead.slm_Delegate_Flag = leadData.Delegate_Flag.Value;
                        if (string.IsNullOrEmpty(leadData.Delegate))
                            lead.slm_DelegateDate = null;
                        else
                            lead.slm_DelegateDate = updateDate;

                        lead.slm_Delegate = string.IsNullOrEmpty(leadData.Delegate) ? null : leadData.Delegate;
                        lead.slm_Delegate_Position = string.IsNullOrEmpty(leadData.Delegate) ? null : GetPositionId(leadData.Delegate, slmdb);
                        
                        if (!string.IsNullOrEmpty(leadData.Delegate_Branch))
                            lead.slm_Delegate_Branch = leadData.Delegate_Branch;
                        else
                            lead.slm_Delegate_Branch = null;
                    }

                    if (actOwner)
                    {
                        lead.slm_AssignedFlag = "0";
                        lead.slm_AssignedDate = null;
                        lead.slm_AssignedBy = null;

                        if (!string.IsNullOrEmpty(leadData.Owner_Branch))
                            lead.slm_Owner_Branch = leadData.Owner_Branch;
                        else
                            lead.slm_Owner_Branch = null;

                        lead.slm_Owner = leadData.Owner;
                        lead.slm_Owner_Position = GetPositionId(leadData.Owner, slmdb);
                    }

                    lead.slm_OldOwner = leadData.slmOldOwner;
                    lead.slm_UpdatedBy = UserId;
                    lead.slm_UpdatedDate = updateDate;
                    lead.slm_ContractNoRefer = leadData.ContractNoRefer;
                    slmdb.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public void ChangeNoteFlag(string ticketId, bool noteFlag, string updateBy)
        {
            try
            {
                var lead = slmdb.kkslm_tr_lead.Where(p => p.slm_ticketId == ticketId).FirstOrDefault();
                if (lead != null)
                {
                    lead.slm_NoteFlag = noteFlag ? "1" : "0";
                    lead.slm_UpdatedBy = updateBy;
                    lead.slm_UpdatedDate = DateTime.Now;
                    slmdb.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public LeadData GetStatusAndAssignFlag(string ticketId)
        {
            try
            {
                return slmdb.kkslm_tr_lead.Where(p => p.slm_ticketId == ticketId).Select(p => new LeadData { Status = p.slm_Status, AssignedFlag = p.slm_AssignedFlag }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetLeadStatus(string ticketId)
        {
            try
            {
                var lead = slmdb.kkslm_tr_lead.Where(p => p.slm_ticketId == ticketId).FirstOrDefault();
                if (lead != null)
                {
                    return lead.slm_Status;
                }
                else
                    return string.Empty;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string GetOwner(string ticketId)
        {
            try
            {
                var lead = slmdb.kkslm_tr_lead.Where(p => p.slm_ticketId == ticketId && p.is_Deleted == 0).FirstOrDefault();
                if (lead != null)
                {
                    return lead.slm_Owner != null ? lead.slm_Owner : string.Empty;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool HasOwnerOrDelegate(string ticketId)
        {
            try
            {
                var lead = slmdb.kkslm_tr_lead.Where(p => p.slm_ticketId == ticketId && p.is_Deleted == 0).FirstOrDefault();
                if (lead != null)
                {
                    if (string.IsNullOrEmpty(lead.slm_Owner) && string.IsNullOrEmpty(lead.slm_Delegate))
                        return false;
                    else
                        return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public LeadOwnerDelegateData GetOwnerAndDelegateName(string ticketId)
        {
            string sql = @"SELECT LEAD.slm_ticketId AS TicketId, ISNULL(staff1.slm_StaffNameTH, LEAD.slm_Owner) AS [OwnerName], ISNULL(staff2.slm_StaffNameTH, LEAD.slm_Delegate) AS [DelegateName]
                            FROM SLMDB.dbo.kkslm_tr_lead LEAD
                            LEFT JOIN SLMDB.dbo.kkslm_ms_staff staff1 ON LEAD.slm_Owner = staff1.slm_UserName
                            LEFT JOIN SLMDB.dbo.kkslm_ms_staff staff2 ON LEAD.slm_Delegate = staff2.slm_UserName
                            WHERE LEAD.slm_ticketId = '" + ticketId + "'";

            return slmdb.ExecuteStoreQuery<LeadOwnerDelegateData>(sql).FirstOrDefault();
        }

        public List<SearchLeadResult> GetLeadOwnerDataTab18_1(string owner)
        {
            string sql = @"SELECT lead.slm_ticketId AS TicketId,cus.slm_CitizenId AS CitizenId,lead.slm_Name AS Firstname,lead.slm_LastName AS Lastname,
	                            ct.slm_CardTypeName AS CardTypeDesc,
                                OP.slm_OptionDesc AS StatusDesc, LEAD.slm_CampaignId AS CampaignId, CAM.slm_CampaignName AS CampaignName,CHANNEL.slm_ChannelDesc AS ChannelDesc,
                                CASE WHEN posowner.slm_PositionNameAbb IS NULL THEN SOWNER.slm_StaffNameTH
		                             ELSE posowner.slm_PositionNameAbb + ' - ' + SOWNER.slm_StaffNameTH END AS OwnerName
	                             ,OBRANCH.slm_BranchName AS OwnerBranchName,LEAD.slm_CreatedDate AS CreatedDate
	                             ,LEAD.slm_AssignedDate AS AssignedDate
                                 ,CASE WHEN posdelegate.slm_PositionNameAbb IS NULL THEN DeStaff.slm_StaffNameTH
		                               ELSE posdelegate.slm_PositionNameAbb + ' - ' + DeStaff.slm_StaffNameTH END AS DelegateName
                                 ,DBRANCH.slm_BranchName AS DelegateBranchName
                            FROM " + SLMDBName + @".DBO.kkslm_tr_lead LEAD 
                                INNER JOIN " + SLMDBName + @".DBO.kkslm_ms_staff sOwner on sOwner.slm_UserName = LEAD.slm_Owner 
	                            INNER JOIN " + SLMDBName + @".DBO.kkslm_ms_branch OBRANCH ON OBRANCH.slm_BranchCode = LEAD.slm_Owner_Branch 
	                            INNER JOIN " + SLMDBName + @".DBo.kkslm_ms_option op on op.slm_OptionCode = lead.slm_Status AND OP.slm_OptionType = 'lead status'
	                            INNER JOIN " + SLMDBName + @".DBO.kkslm_ms_campaign CAM ON CAM.slm_CampaignId = LEAD.slm_CampaignId 
	                            INNER JOIN " + SLMDBName + @".DBO.kkslm_ms_channel CHANNEL ON CHANNEL.slm_ChannelId = LEAD.slm_ChannelId 
	                            LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_staff AS DeStaff on DeStaff.slm_UserName = LEAD.slm_Delegate 
	                            LEFT JOIN " + SLMDBName + @".DBO.kkslm_tr_cusinfo cus on cus.slm_TicketId = lead.slm_ticketId
	                            LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_branch DBRANCH ON DBRANCH.slm_BranchCode = LEAD.slm_Delegate_Branch  
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_cardtype ct ON cus.slm_CardType = ct.slm_CardTypeId
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_position posowner ON lead.slm_Owner_Position = posowner.slm_Position_id
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_position posdelegate ON lead.slm_Delegate_Position = posdelegate.slm_Position_id
                            WHERE LEAD.is_Deleted = 0 AND LEAD.slm_Status NOT IN ('" + SLMConstant.StatusCode.Reject + @"','" + SLMConstant.StatusCode.Cancel + @"','" + SLMConstant.StatusCode.Close + @"') AND 
                                  LEAD.slm_Owner = '" + owner + "' ORDER BY  LEAD.slm_CreatedDate DESC ";

            return slmdb.ExecuteStoreQuery<SearchLeadResult>(sql).ToList();
        }
        public List<SearchLeadResult> GetLeadDelegateDataTab18_2(string delegateuser)
        {
            string sql = @"SELECT lead.slm_ticketId AS TicketId,cus.slm_CitizenId AS CitizenId,lead.slm_Name AS Firstname,lead.slm_LastName AS Lastname,
	                            ct.slm_CardTypeName AS CardTypeDesc, 
                                OP.slm_OptionDesc AS StatusDesc, LEAD.slm_CampaignId AS CampaignId, CAM.slm_CampaignName AS CampaignName,CHANNEL.slm_ChannelDesc AS ChannelDesc
                                ,CASE WHEN posowner.slm_PositionNameAbb IS NULL THEN SOWNER.slm_StaffNameTH
		                              ELSE posowner.slm_PositionNameAbb + ' - ' + SOWNER.slm_StaffNameTH END AS OwnerName
	                             ,OBRANCH.slm_BranchName AS OwnerBranchName,LEAD.slm_CreatedDate AS CreatedDate,
	                             LEAD.slm_AssignedDate AS AssignedDate
                                ,CASE WHEN posdelegate.slm_PositionNameAbb IS NULL THEN DeStaff.slm_StaffNameTH
		                              ELSE posdelegate.slm_PositionNameAbb + ' - ' + DeStaff.slm_StaffNameTH END AS DelegateName
                                ,DBRANCH.slm_BranchName AS DelegateBranchName
                            FROM " + SLMDBName + @".DBO.kkslm_tr_lead LEAD 
                                INNER JOIN " + SLMDBName + @".DBO.kkslm_ms_staff sOwner on sOwner.slm_UserName = LEAD.slm_Owner 
	                            INNER JOIN " + SLMDBName + @".DBO.kkslm_ms_branch OBRANCH ON OBRANCH.slm_BranchCode = LEAD.slm_Owner_Branch 
	                            INNER JOIN " + SLMDBName + @".DBo.kkslm_ms_option op on op.slm_OptionCode = lead.slm_Status AND OP.slm_OptionType = 'lead status'
	                            INNER JOIN " + SLMDBName + @".DBO.kkslm_ms_campaign CAM ON CAM.slm_CampaignId = LEAD.slm_CampaignId 
	                            INNER JOIN " + SLMDBName + @".DBO.kkslm_ms_channel CHANNEL ON CHANNEL.slm_ChannelId = LEAD.slm_ChannelId 
	                            INNER JOIN " + SLMDBName + @".DBO.kkslm_ms_staff AS DeStaff on DeStaff.slm_UserName = LEAD.slm_Delegate 
                                INNER JOIN " + SLMDBName + @".DBO.kkslm_ms_branch DBRANCH ON DBRANCH.slm_BranchCode = LEAD.slm_Delegate_Branch 
	                            LEFT JOIN " + SLMDBName + @".DBO.kkslm_tr_cusinfo cus on cus.slm_TicketId = lead.slm_ticketId
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_cardtype ct ON cus.slm_CardType = ct.slm_CardTypeId
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_position posowner ON lead.slm_Owner_Position = posowner.slm_Position_id
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_position posdelegate ON lead.slm_Delegate_Position = posdelegate.slm_Position_id
                            WHERE LEAD.is_Deleted = 0 AND LEAD.slm_Status NOT IN ('" + SLMConstant.StatusCode.Reject + @"','" + SLMConstant.StatusCode.Cancel + @"','" + SLMConstant.StatusCode.Close + @"') AND
                                  LEAD.slm_Delegate = '" + delegateuser + "'  ORDER BY  LEAD.slm_CreatedDate DESC ";

            return slmdb.ExecuteStoreQuery<SearchLeadResult>(sql).ToList();
        }

        public void UpdateTransferLeadOwner(List<string> TicketList,string newowner,int staffid, string Username,string branchcode)
        {
            var leadlist = slmdb.kkslm_tr_lead.Where(p => TicketList.Contains(p.slm_ticketId) == true).ToList();
            try
            {
                if (leadlist.Count > 0)
                {
                    foreach (kkslm_tr_lead lead in leadlist)
                    {
                        DateTime transferDate = DateTime.Now;
                        lead.slm_OldOwner = lead.slm_Owner;
                        lead.slm_Owner = newowner;
                        lead.slm_Owner_Position = GetPositionId(newowner, slmdb);
                        lead.slm_StaffId = staffid;
                        lead.slm_Owner_Branch = branchcode;
                        lead.slm_AssignedFlag = "0";
                        lead.slm_AssignedDate = null;
                        lead.slm_AssignedBy = null;
                        lead.slm_UpdatedBy = Username;
                        lead.slm_UpdatedDate = transferDate;
                        lead.slm_TransferDate = transferDate;
                    }
                    slmdb.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateTransferLeadDelegate(List<string> TicketList, string newDelegate, int staffid, string Username,string branchcode)
        {
            var leadlist = slmdb.kkslm_tr_lead.Where(p => TicketList.Contains(p.slm_ticketId) == true).ToList();
            try
            {
                if (leadlist.Count > 0)
                {
                    foreach (kkslm_tr_lead lead in leadlist)
                    {
                        DateTime transferDate = DateTime.Now;
                        lead.slm_Delegate = newDelegate;
                        lead.slm_Delegate_Position = GetPositionId(newDelegate, slmdb);
                        lead.slm_Delegate_Branch = branchcode;
                        lead.slm_Delegate_Flag = 1;
                        lead.slm_DelegateDate = transferDate;
                        lead.slm_UpdatedBy = Username;
                        lead.slm_UpdatedDate = transferDate;
                        lead.slm_TransferDate = transferDate; 
                    }
                    slmdb.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool CheckExistLeadOnHand(string username)
        {
            string sql = @" SELECT  COUNT(LEAD.SLM_TICKETID) AS CNT
                            FROM    " + SLMDBName + @".DBO.kkslm_tr_lead LEAD 
                            WHERE LEAD.is_Deleted = 0 AND LEAD.slm_Status NOT IN ('" + SLMConstant.StatusCode.Reject + @"','" + SLMConstant.StatusCode.Cancel + @"','" + SLMConstant.StatusCode.Close + @"') 
                                    AND (LEAD.slm_Owner = '" + username + @"' OR LEAD.slm_Delegate = '" + username + @"')" ;

            var result = slmdb.ExecuteStoreQuery<int>(sql).Select(p => p.ToString()).FirstOrDefault();
            if (result != null)
            {
                int cnt = int.Parse(result);
                if (cnt > 0)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        public void InsertData(string ticketId, string new_ticketId, ProductData productData, string username, DateTime createdDate, string channelId)
        {
            try
            {
                var leadData = slmdb.kkslm_tr_lead.Where(p => p.slm_ticketId == ticketId).FirstOrDefault();
                if (leadData != null)
                {
                    kkslm_tr_lead new_lead = new kkslm_tr_lead();
                    new_lead.slm_ticketId = new_ticketId;
                    new_lead.slm_Name = leadData.slm_Name;
                    new_lead.slm_LastName = leadData.slm_LastName;
                    new_lead.slm_TelNo_1 = leadData.slm_TelNo_1;
                    new_lead.slm_Status = "00";
                    new_lead.slm_StatusDate = createdDate;
                    new_lead.slm_StatusBy = username;
                    new_lead.slm_AvailableTime = leadData.slm_AvailableTime;
                    new_lead.slm_ChannelId = channelId;
                    new_lead.slm_CreatedBy = username;
                    new_lead.slm_CreatedBy_Position = GetPositionId(username, slmdb);
                    new_lead.slm_CreatedDate = createdDate;
                    new_lead.slm_UpdatedBy = username;
                    new_lead.slm_UpdatedDate = createdDate;
                    new_lead.slm_Counting = 0;
                    new_lead.slm_Delegate_Flag = 0;
                    new_lead.slm_AssignedFlag = "0";
                    new_lead.slm_ContractNoRefer = leadData.slm_ContractNoRefer;

                    //ข้อมูลใหม่
                    new_lead.slm_Owner = username;
                    new_lead.slm_Owner_Position = GetPositionId(username, slmdb);
                    var staff = slmdb.kkslm_ms_staff.Where(p => p.slm_UserName == new_lead.slm_Owner).FirstOrDefault();
                    if (staff != null)
                    {
                        new_lead.slm_StaffId = staff.slm_StaffId;
                        new_lead.slm_Owner_Branch = staff.slm_BranchCode;
                        new_lead.slm_CreatedBy_Branch = staff.slm_BranchCode;
                    }
                    new_lead.slm_CampaignId = productData.CampaignId;
                    new_lead.slm_Product_Group_Id = productData.ProductGroupId;
                    new_lead.slm_Product_Id = productData.ProductId;
                    new_lead.slm_Product_Name = productData.ProductName;

                    slmdb.kkslm_tr_lead.AddObject(new_lead);
                }
                else
                    throw new Exception("ไม่พบ Ticket Id " + ticketId + " ใน Table kkslm_tr_lead");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetAssignFlag(string ticketId)
        {
            try
            {
                var lead = slmdb.kkslm_tr_lead.Where(p => p.slm_ticketId == ticketId).FirstOrDefault();
                if (lead != null)
                {
                    return lead.slm_AssignedFlag != null ? lead.slm_AssignedFlag : string.Empty;
                }
                else
                    return "";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public LeadDataPhoneCallHistory GetLeadDataPhoneCallHistory(string ticketId)
        {
            string sql = @"SELECT lead.slm_ticketId AS TicketId, lead.slm_Name AS Name, lead.slm_LastName AS LastName, lead.slm_CampaignId AS CampaignId, cam.slm_CampaignName AS CampaignName
                            , lead.slm_Owner_Branch AS OwnerBranch, lead.slm_Owner AS [Owner], lead.slm_Delegate_Branch AS DelegateBranch, lead.slm_Delegate AS Delegate
                            , lead.slm_TelNo_1 AS TelNo1, lead.slm_Status AS LeadStatus, lead.slm_AssignedFlag AS AssignedFlag, lead.slm_Delegate_Flag AS DelegateFlag, lead.slm_Product_Id AS ProductId
                            , cus.slm_CardType AS CardType, cus.slm_CitizenId AS CitizenId
                            FROM " + SLMDBName + @".dbo.kkslm_tr_lead lead
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_tr_cusinfo cus ON lead.slm_ticketId = cus.slm_TicketId
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_campaign cam ON lead.slm_CampaignId = cam.slm_CampaignId
                            WHERE lead.slm_ticketId = '" + ticketId + "'";

            return slmdb.ExecuteStoreQuery<LeadDataPhoneCallHistory>(sql).FirstOrDefault();
        }

        public List<string> GetAssignedFlagAndDelegateFlag(string ticketId)
        {
            try
            {
                List<string> list = new List<string>();

                var lead = slmdb.kkslm_tr_lead.Where(p => p.slm_ticketId == ticketId).FirstOrDefault();
                if (lead != null)
                {
                    list.Add(lead.slm_AssignedFlag != null ? lead.slm_AssignedFlag.Trim() : "");
                    list.Add(lead.slm_Delegate_Flag.ToString());
                    return list;
                }
                else
                    throw new Exception("ไม่พบ Ticket Id " + ticketId + " ในระบบ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

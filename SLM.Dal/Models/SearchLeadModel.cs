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

namespace SLM.Dal.Models
{
    public class SearchLeadModel
    {
        private SLM_DBEntities slmdb = null;
        private string SLMDBName = SLMConstant.SLMDBName;

        public SearchLeadModel()
        {
            slmdb = new SLM_DBEntities();
        }

        //Reference : SlmScr003Biz,RoleBiz
        public List<SearchLeadResult> SearchLeadData(string ticketId, string firstname, string lastname, string cardTypeId, string citizenId, string campaignId, string channelId, string ownerLeadUsername,
            string createDate, string assignDate, string leadStatusList, string username, decimal? stafftype, string ownerBranch, string delegateBranch, string delegateLead, string contractNoRefer,
            string createByBranch, string createBy, string orderByFlag)
        {
            string sql = "";
            if (stafftype != SLMConstant.StaffType.Marketing &&
                stafftype != SLMConstant.StaffType.ManagerOper &&
                stafftype != SLMConstant.StaffType.SupervisorOper &&
                stafftype != SLMConstant.StaffType.Oper)
            {
                KKSlmMsStaffModel staff = new KKSlmMsStaffModel();
                string UserLoginBranchCode = staff.GetBrachCode(username);

                sql = @" SELECT A.* FROM(
                            SELECT DISTINCT result.*
                            FROM (
	                            SELECT lead.slm_ticketId AS TicketId, lead.slm_Counting AS Counting, lead.slm_Name AS Firstname, opt.slm_OptionDesc AS StatusDesc, 
			                            cardtype.slm_CardTypeName AS CardTypeDesc,
                                        info.slm_CitizenId AS CitizenId, info.slm_LastName AS LastName, campaign.slm_CampaignName AS CampaignName, channel.slm_ChannelDesc AS ChannelDesc, 
			                             CASE WHEN ISNULL(poOwner.slm_PositionNameABB,'99999999') = '99999999' THEN staff.slm_StaffNameTH
					                            ELSE poOwner.slm_PositionNameABB +' - '+staff.slm_StaffNameTH  END OwnerName, 
                                        lead.slm_CreatedDate AS CreatedDate, lead.slm_AssignedDate AS AssignedDate, 
                                        lead.slm_NoteFlag AS NoteFlag, staff.slm_StaffId,
			                            CASE WHEN ISNULL(poDelegate.slm_PositionNameABB,'99999999') = '99999999' THEN delegate.slm_StaffNameTH 
					                        ELSE poDelegate.slm_PositionNameABB+' - '+delegate.slm_StaffNameTH  END DelegateName,
                                        ownerbranch.slm_BranchName AS OwnerBranchName,
			                            Delegatebranch.slm_BranchName AS DelegateBranchName,
                                        CASE WHEN ISNULL(createby.slm_StaffNameTH ,LEAD.slm_CreatedBy) = LEAD.slm_CreatedBy THEN LEAD.slm_CreatedBy
					                        WHEN ISNULL(poCreateby.slm_PositionNameABB,'99999999') = '99999999' THEN createby.slm_StaffNameTH 
					                            ELSE poCreateby.slm_PositionNameABB+' - '+createby.slm_StaffNameTH END CreateName,
			                            CreateBybranch.slm_BranchName AS BranchCreateBranchName, lead.slm_Product_Name AS ProductName, ISNULL(MP.HasADAMUrl, 0) AS HasAdamUrl
                                        , lead.slm_CampaignId AS CampaignId, prodinfo.slm_LicenseNo AS LicenseNo, lead.slm_TelNo_1 AS TelNo1, prodinfo.slm_ProvinceRegis AS ProvinceRegis
                                        , campaign.slm_Url AS CalculatorUrl, lead.slm_Product_Group_Id AS ProductGroupId, lead.slm_Product_Id AS ProductId, lead.coc_Appno AS AppNo
                                        , CONVERT(VARCHAR,lead.coc_IsCOC) AS IsCOC,lead.slm_status as slmStatusCode, lead.slm_ContractNoRefer AS ContractNoRefer, lead.coc_CurrentTeam AS COCCurrentTeam
                                        , lead.slm_ExternalSubStatusDesc AS ExternalSubStatusDesc, lead.slm_NextSLA AS NextSLA, DATEDIFF(minute, GETDATE(), lead.slm_NextSLA) AS NextSLADiffCurrent
	                            FROM " + SLMDBName + @".DBO.kkslm_tr_lead lead INNER JOIN 
	                            (
                                    SELECT CP.PR_CampaignId AS slm_CampaignId FROM kkslm_ms_access_right AR INNER  JOIN CMT_CAMPAIGN_PRODUCT CP ON CP.PR_ProductId = AR.slm_Product_Id
                                    WHERE AR.slm_Product_Id IS NOT NULL AND
                                    AR.slm_BranchCode = '" + UserLoginBranchCode + @"' AND AR.slm_StaffTypeId = '" + stafftype + @"'
                                    UNION ALL
                                    SELECT AR.slm_CampaignId  AS slm_CampaignId  FROM kkslm_ms_access_right AR 
                                    WHERE AR.slm_CampaignId IS NOT NULL AND
                                    AR.slm_BranchCode = '" + UserLoginBranchCode + @"' AND AR.slm_StaffTypeId = '" + stafftype + @"'
                                ) AS Z ON Z.slm_CampaignId = lead.slm_CampaignId 
	                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_tr_cusinfo info ON lead.slm_ticketId = info.slm_TicketId
	                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_campaign campaign ON lead.slm_CampaignId = campaign.slm_CampaignId
	                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_channel channel ON lead.slm_ChannelId = channel.slm_ChannelId
	                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff staff ON lead.slm_Owner = staff.slm_UserName  
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_position poOwner on lead.slm_Owner_Position = poOwner.slm_Position_id 
	                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_option opt ON lead.slm_Status = opt.slm_OptionCode AND opt.slm_OptionType = 'lead status' 
	                            LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_staff delegate on delegate.slm_UserName = lead.slm_Delegate
                                LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_position poDelegate on lead.slm_Delegate_Position = poDelegate.slm_Position_id 
	                            LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_branch ownerbranch on lead.slm_Owner_Branch = ownerbranch.slm_BranchCode 
	                            LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_branch Delegatebranch on lead.slm_Delegate_Branch = Delegatebranch.slm_BranchCode 
	                            LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_staff createby on createby.slm_UserName = lead.slm_CreatedBy
                                LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_position poCreateby on lead.slm_CreatedBy_Position = poCreateby.slm_Position_id 
	                            LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_branch CreateBybranch on lead.slm_CreatedBy_Branch = CreateBybranch.slm_BranchCode 
                                LEFT JOIN " + SLMDBName + @".dbo.CMT_MAPPING_PRODUCT MP ON lead.slm_Product_Id = MP.sub_product_id
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_tr_productinfo prodinfo ON lead.slm_ticketId = prodinfo.slm_TicketId
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_cardtype cardtype ON info.slm_CardType = cardtype.slm_CardTypeId
	                            WHERE lead.is_Deleted = 0 {0} 
	                            UNION ALL
	                            SELECT lead.slm_ticketId AS TicketId, lead.slm_Counting AS Counting, lead.slm_Name AS Firstname, opt.slm_OptionDesc AS StatusDesc, 
			                            cardtype.slm_CardTypeName AS CardTypeDesc,
                                        info.slm_CitizenId AS CitizenId, info.slm_LastName AS LastName, campaign.slm_CampaignName AS CampaignName, channel.slm_ChannelDesc AS ChannelDesc, 
			                            CASE WHEN ISNULL(poOwner.slm_PositionNameABB,'99999999') = '99999999' THEN staff.slm_StaffNameTH
					                        ELSE poOwner.slm_PositionNameABB +' - '+staff.slm_StaffNameTH  END OwnerName, 
                                        lead.slm_CreatedDate AS CreatedDate, 
                                        lead.slm_AssignedDate AS AssignedDate, lead.slm_NoteFlag AS NoteFlag, staff.slm_StaffId,
                                        CASE WHEN ISNULL(poDelegate.slm_PositionNameABB,'99999999') = '99999999' THEN delegate.slm_StaffNameTH 
					                            ELSE poDelegate.slm_PositionNameABB+' - '+delegate.slm_StaffNameTH  END DelegateName,
                                        ownerbranch.slm_BranchName AS OwnerBranchName,
			                            Delegatebranch.slm_BranchName AS DelegateBranchName,
                                        CASE WHEN ISNULL(createby.slm_StaffNameTH ,LEAD.slm_CreatedBy) = LEAD.slm_CreatedBy THEN LEAD.slm_CreatedBy
					                            WHEN ISNULL(poCreateby.slm_PositionNameABB,'99999999') = '99999999' THEN createby.slm_StaffNameTH 
					                            ELSE poCreateby.slm_PositionNameABB+' - '+createby.slm_StaffNameTH END CreateName,
			                            CreateBybranch.slm_BranchName AS BranchCreateBranchName, lead.slm_Product_Name AS ProductName, ISNULL(MP.HasADAMUrl, 0) AS HasAdamUrl
                                        , lead.slm_CampaignId AS CampaignId, prodinfo.slm_LicenseNo AS LicenseNo, lead.slm_TelNo_1 AS TelNo1, prodinfo.slm_ProvinceRegis AS ProvinceRegis
                                        , campaign.slm_Url AS CalculatorUrl, lead.slm_Product_Group_Id AS ProductGroupId, lead.slm_Product_Id AS ProductId, lead.coc_Appno AS AppNo
                                        , CONVERT(VARCHAR,lead.coc_IsCOC) AS IsCOC,lead.slm_status as slmStatusCode, lead.slm_ContractNoRefer AS ContractNoRefer, lead.coc_CurrentTeam AS COCCurrentTeam
                                        , lead.slm_ExternalSubStatusDesc AS ExternalSubStatusDesc, lead.slm_NextSLA AS NextSLA, DATEDIFF(minute, GETDATE(), lead.slm_NextSLA) AS NextSLADiffCurrent
	                            FROM " + SLMDBName + @".dbo.kkslm_tr_lead lead
	                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_tr_cusinfo info ON lead.slm_ticketId = info.slm_TicketId
	                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_campaign campaign ON lead.slm_CampaignId = campaign.slm_CampaignId
	                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_channel channel ON lead.slm_ChannelId = channel.slm_ChannelId
	                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff staff ON lead.slm_Owner = staff.slm_UserName  
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_position poOwner on lead.slm_Owner_Position = poOwner.slm_Position_id 
	                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_option opt ON lead.slm_Status = opt.slm_OptionCode AND opt.slm_OptionType = 'lead status' 
	                            LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_staff delegate on delegate.slm_UserName = lead.slm_Delegate
                                LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_position poDelegate on lead.slm_Delegate_Position = poDelegate.slm_Position_id
	                            LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_branch ownerbranch on lead.slm_Owner_Branch = ownerbranch.slm_BranchCode 
	                            LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_branch Delegatebranch on lead.slm_Delegate_Branch = Delegatebranch.slm_BranchCode 
	                            LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_staff createby on createby.slm_UserName = lead.slm_CreatedBy
                                LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_position poCreateby on lead.slm_CreatedBy_Position = poCreateby.slm_Position_id 
	                            LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_branch CreateBybranch on lead.slm_CreatedBy_Branch = CreateBybranch.slm_BranchCode 
                                LEFT JOIN " + SLMDBName + @".dbo.CMT_MAPPING_PRODUCT MP ON lead.slm_Product_Id = MP.sub_product_id
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_tr_productinfo prodinfo ON lead.slm_ticketId = prodinfo.slm_TicketId
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_cardtype cardtype ON info.slm_CardType = cardtype.slm_CardTypeId
	                            WHERE lead.slm_Delegate = '" + username + @"' AND lead.is_Deleted = 0 {0}   
                                 ) AS result 
                                ) A ";

                if (orderByFlag == SLMConstant.SearchOrderBy.SLA)
                {
                    sql += @" ORDER BY A.Counting DESC
				                        , CASE WHEN A.NextSLA IS NULL THEN 0 ELSE 1 END DESC
				                        , A.NextSLADiffCurrent ASC, A.NextSLA ASC, A.CreatedDate DESC ";
                }
                else if (orderByFlag == SLMConstant.SearchOrderBy.Note)
                {
                    sql += @" ORDER BY CASE WHEN A.NoteFlag IS NULL THEN '0' 
					                    WHEN A.NoteFlag = '0' THEN '0'
					                    WHEN A.NoteFlag = '1' THEN '1' END DESC, A.CreatedDate DESC";
                }
            }
            else
            {
                string recusiveList = GetRecursiveStaff(username);
                sql = @" SELECT lead.slm_ticketId AS TicketId, lead.slm_Counting AS Counting, lead.slm_Name AS Firstname, opt.slm_OptionDesc AS StatusDesc, 
                                cardtype.slm_CardTypeName AS CardTypeDesc,
                            info.slm_CitizenId AS CitizenId, info.slm_LastName AS LastName, campaign.slm_CampaignName AS CampaignName, channel.slm_ChannelDesc AS ChannelDesc, 
                            CASE WHEN ISNULL(poOwner.slm_PositionNameABB,'99999999') = '99999999' THEN staff.slm_StaffNameTH
					            ELSE poOwner.slm_PositionNameABB +' - '+staff.slm_StaffNameTH  END OwnerName, 
                            lead.slm_CreatedDate AS CreatedDate, 
                            lead.slm_AssignedDate AS AssignedDate, lead.slm_NoteFlag AS NoteFlag, staff.slm_StaffId,
                            CASE WHEN ISNULL(poDelegate.slm_PositionNameABB,'99999999') = '99999999' THEN delegate.slm_StaffNameTH 
					            ELSE poDelegate.slm_PositionNameABB+' - '+delegate.slm_StaffNameTH  END DelegateName,
                            ownerbranch.slm_BranchName AS OwnerBranchName,
						    Delegatebranch.slm_BranchName AS DelegateBranchName,
                            CASE WHEN ISNULL(createby.slm_StaffNameTH ,LEAD.slm_CreatedBy) = LEAD.slm_CreatedBy THEN LEAD.slm_CreatedBy
					            WHEN ISNULL(poCreateby.slm_PositionNameABB,'99999999') = '99999999' THEN createby.slm_StaffNameTH 
					            ELSE poCreateby.slm_PositionNameABB+' - '+createby.slm_StaffNameTH END CreateName,
						    CreateBybranch.slm_BranchName AS BranchCreateBranchName, lead.slm_Product_Name AS ProductName, ISNULL(MP.HasADAMUrl, 0) AS HasAdamUrl
                            , lead.slm_CampaignId AS CampaignId, prodinfo.slm_LicenseNo AS LicenseNo, lead.slm_TelNo_1 AS TelNo1, prodinfo.slm_ProvinceRegis AS ProvinceRegis
                            , campaign.slm_Url AS CalculatorUrl, lead.slm_Product_Group_Id AS ProductGroupId, lead.slm_Product_Id AS ProductId, lead.coc_Appno AS AppNo
                            , CONVERT(VARCHAR,lead.coc_IsCOC) AS IsCOC,lead.slm_status as slmStatusCode, lead.slm_ContractNoRefer AS ContractNoRefer, lead.coc_CurrentTeam AS COCCurrentTeam
                            , lead.slm_ExternalSubStatusDesc AS ExternalSubStatusDesc, lead.slm_NextSLA AS NextSLA, DATEDIFF(minute, GETDATE(), lead.slm_NextSLA) AS NextSLADiffCurrent
                        FROM " + SLMDBName + @".DBO.kkslm_tr_lead lead 
                        LEFT JOIN " + SLMDBName + @".dbo.kkslm_tr_cusinfo info ON lead.slm_ticketId = info.slm_TicketId
                        LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_campaign campaign ON lead.slm_CampaignId = campaign.slm_CampaignId
                        LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_channel channel ON lead.slm_ChannelId = channel.slm_ChannelId
                        LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff staff ON lead.slm_Owner = staff.slm_UserName  
                        LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_position poOwner on lead.slm_Owner_Position = poOwner.slm_Position_id 
                        LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_option opt ON lead.slm_Status = opt.slm_OptionCode AND opt.slm_OptionType = 'lead status' 
                        LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_staff delegate on delegate.slm_UserName = lead.slm_Delegate
                        LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_position poDelegate on lead.slm_Delegate_Position = poDelegate.slm_Position_id 
					    LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_branch ownerbranch on lead.slm_Owner_Branch = ownerbranch.slm_BranchCode 
					    LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_branch Delegatebranch on lead.slm_Delegate_Branch = Delegatebranch.slm_BranchCode 
					    LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_staff createby on createby.slm_UserName = lead.slm_CreatedBy
                        LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_position poCreateby on lead.slm_CreatedBy_Position = poCreateby.slm_Position_id 
					    LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_branch CreateBybranch on lead.slm_CreatedBy_Branch = CreateBybranch.slm_BranchCode 
                        LEFT JOIN " + SLMDBName + @".dbo.CMT_MAPPING_PRODUCT MP ON lead.slm_Product_Id = MP.sub_product_id
                        LEFT JOIN " + SLMDBName + @".dbo.kkslm_tr_productinfo prodinfo ON lead.slm_ticketId = prodinfo.slm_TicketId
                        LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_cardtype cardtype ON info.slm_CardType = cardtype.slm_CardTypeId
                        WHERE lead.is_Deleted = 0 AND (lead.slm_Owner IN (" + recusiveList + ") OR lead.slm_Delegate IN (" + recusiveList + @")) {0} ";

                if (orderByFlag == SLMConstant.SearchOrderBy.SLA)
                {
                    sql += @" ORDER BY lead.slm_Counting DESC
		                                , CASE WHEN lead.slm_NextSLA IS NULL THEN 0 ELSE 1 END DESC
		                                , NextSLADiffCurrent ASC, lead.slm_NextSLA ASC, lead.slm_CreatedDate DESC ";
                }
                else if (orderByFlag == SLMConstant.SearchOrderBy.Note)
                {
                    sql += @" ORDER BY CASE WHEN lead.slm_NoteFlag IS NULL THEN '0' 
					                        WHEN lead.slm_NoteFlag = '0' THEN '0'
					                        WHEN lead.slm_NoteFlag = '1' THEN '1' END DESC, lead.slm_CreatedDate DESC";
                }
            }

            string whr = "";

            whr += (ticketId == "" ? "" : (whr == "" ? "" : " AND ") + " lead.slm_ticketId LIKE '%" + ticketId + "%' ");
            whr += (firstname == "" ? "" : (whr == "" ? "" : " AND ") + " lead.slm_Name LIKE @firstname ");
            whr += (lastname == "" ? "" : (whr == "" ? "" : " AND ") + " info.slm_LastName LIKE @lastname ");
            whr += (contractNoRefer == "" ? "" : (whr == "" ? "" : " AND ") + " lead.slm_ContractNoRefer LIKE @contractnorefer ");
            whr += (citizenId == "" ? "" : (whr == "" ? "" : " AND ") + " info.slm_CitizenId LIKE '%" + citizenId + "%' ");
            whr += (cardTypeId == "" ? "" : (whr == "" ? "" : " AND ") + " info.slm_CardType = '" + cardTypeId + "' ");
            whr += (campaignId == "" ? "" : (whr == "" ? "" : " AND ") + " lead.slm_CampaignId = '" + campaignId + "' ");
            whr += (channelId == "" ? "" : (whr == "" ? "" : " AND ") + " lead.slm_ChannelId = '" + channelId + "' ");
            whr += (ownerLeadUsername == "" ? "" : (whr == "" ? "" : " AND ") + " lead.slm_Owner = '" + ownerLeadUsername + "' ");      //Owner Lead
            whr += (createDate == "" ? "" : (whr == "" ? "" : " AND ") + " CONVERT(DATE, lead.slm_CreatedDate) = '" + createDate + "' ");
            whr += (assignDate == "" ? "" : (whr == "" ? "" : " AND ") + " CONVERT(DATE, lead.slm_AssignedDate) = '" + assignDate + "' ");
            whr += (ownerBranch == "" ? "" : (whr == "" ? "" : " AND ") + " lead.slm_Owner_Branch = '" + ownerBranch + "' ");           //Owner Branch
            whr += (delegateBranch == "" ? "" : (whr == "" ? "" : " AND ") + " lead.slm_Delegate_Branch = '" + delegateBranch + "' ");  //Delegate Branch
            whr += (delegateLead == "" ? "" : (whr == "" ? "" : " AND ") + " lead.slm_Delegate = '" + delegateLead + "' ");             //Delegate Lead
            whr += (createByBranch == "" ? "" : (whr == "" ? "" : " AND ") + " lead.slm_CreatedBy_Branch = '" + createByBranch + "' "); //CreateBy Branch
            whr += (createBy == "" ? "" : (whr == "" ? "" : " AND ") + " lead.slm_CreatedBy = '" + createBy + "' ");                    //CreateBy Lead
            whr += (leadStatusList == "" ? "" : (whr == "" ? "" : " AND ") + " lead.slm_Status IN (" + leadStatusList + ") ");

            whr = whr == "" ? "" : " AND " + whr;
            sql = string.Format(sql, whr);

            object[] param = new object[] 
            { 
                new SqlParameter("@firstname", "%" + firstname + "%")
                , new SqlParameter("@lastname", "%" + lastname + "%")
                , new SqlParameter("@contractnorefer", "%" + contractNoRefer + "%")
            };

            return slmdb.ExecuteStoreQuery<SearchLeadResult>(sql, param).ToList();
        }

        private string GetRecursiveStaff(string username)
        {
            string userList = "";
            KKSlmMsStaffModel sModel = new KKSlmMsStaffModel();

            List<StaffData> staffList = sModel.GetStaffList();

            StaffData sData = sModel.GetStaff(username);
            int? staffId = sData.StaffId;

            ArrayList arr = new ArrayList();
            arr.Add("'" + username +"'");    //เก็บ login staff

            FindStaffRecusive(staffId, arr, staffList);

            foreach (string tmp_username in arr)
            {
                userList += (userList == "" ? "" : ",") + tmp_username;
            }

            return userList;
        }

        private void FindStaffRecusive(int? headId, ArrayList arr, List<StaffData> staffList)
        {
            foreach (StaffData staff in staffList)
            {
                if (staff.HeadStaffId == headId)
                {
                    arr.Add("'" + staff.UserName + "'");
                    FindStaffRecusive(staff.StaffId, arr, staffList);
                }
            }
        }
        //Reference : SlmScr005Biz
        public List<SearchLeadResult> SearchExistingLead(string citizenId, string ticketId)
        {
            string sql = @"SELECT lead.slm_ticketId AS TicketId, lead.slm_Counting AS Counting, lead.slm_Name AS Firstname, opt.slm_OptionDesc AS StatusDesc, 
                            cardtype.slm_CardTypeName AS CardTypeDesc,
                            info.slm_CitizenId AS CitizenId, info.slm_LastName AS LastName, campaign.slm_CampaignName AS CampaignName, channel.slm_ChannelDesc AS ChannelDesc, 
                            CASE WHEN pos.slm_PositionNameAbb IS NULL THEN staff.slm_StaffNameTH
	                             ELSE pos.slm_PositionNameAbb + ' - ' + staff.slm_StaffNameTH END AS OwnerName,
                            lead.slm_CreatedDate AS CreatedDate, lead.slm_AssignedDate AS AssignedDate, lead.slm_NoteFlag AS NoteFlag
                            FROM " + SLMDBName + @".dbo.kkslm_tr_lead lead
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_tr_cusinfo info ON lead.slm_ticketId = info.slm_TicketId
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_campaign campaign ON lead.slm_CampaignId = campaign.slm_CampaignId
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_channel channel ON lead.slm_ChannelId = channel.slm_ChannelId
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_position pos ON lead.slm_Owner_Position = pos.slm_Position_id
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff staff ON lead.slm_Owner = staff.slm_UserName  
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_option opt ON lead.slm_Status = opt.slm_OptionCode AND opt.slm_OptionType = 'lead status' 
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_cardtype cardtype ON info.slm_CardType = cardtype.slm_CardTypeId ";
                        
            //if (!string.IsNullOrEmpty(citizenId))
            //    sql += " WHERE (info.slm_CitizenId = '" + citizenId + "' OR lead.slm_TelNo_1 = '" + telNo1 + "') AND lead.is_Deleted = 0 ";
            //else
            //    sql += " WHERE lead.slm_TelNo_1 = '" + telNo1 + "' AND lead.is_Deleted = 0 ";

            if (!string.IsNullOrEmpty(citizenId))
                sql += " WHERE (info.slm_CitizenId = '" + citizenId + "' OR lead.slm_ticketId = '" + ticketId + "') AND lead.is_Deleted = 0 ";
            else
                sql += " WHERE lead.slm_ticketId = '" + ticketId + "' AND lead.is_Deleted = 0 ";

            sql += " ORDER BY lead.slm_CreatedDate DESC ";

            return slmdb.ExecuteStoreQuery<SearchLeadResult>(sql).ToList();
        }

        #region Backup SearchLeadData

//        public List<SearchLeadResult> SearchLeadData(string ticketId, string firstname, string lastname, string citizenId, string campaignId, string channelId, string ownerLeadUsername, string createDate, string assignDate, string leadStatusList, string telNo1, string orderBy)
//        {
//            string sql = @"SELECT lead.slm_ticketId AS TicketId, lead.slm_Counting AS Counting, lead.slm_Name AS Firstname, opt.slm_OptionDesc AS StatusDesc, 
//                            info.slm_CitizenId AS CitizenId, info.slm_LastName AS LastName, campaign.slm_CampaignName AS CampaignName, channel.slm_ChannelDesc AS ChannelDesc, 
//                            staff.slm_StaffNameTH AS OwnerName, lead.slm_CreatedDate AS CreatedDate, lead.slm_AssignedDate AS AssignedDate, lead.slm_NoteFlag AS NoteFlag
        //                            FROM "+ SLMDBName + @".dbo.kkslm_tr_lead lead
        //                            LEFT JOIN "+ SLMDBName + @".dbo.kkslm_tr_cusinfo info ON lead.slm_ticketId = info.slm_TicketId
        //                            LEFT JOIN "+ SLMDBName + @".dbo.kkslm_ms_campaign campaign ON lead.slm_CampaignId = campaign.slm_CampaignId
        //                            LEFT JOIN "+ SLMDBName + @".dbo.kkslm_ms_channel channel ON lead.slm_ChannelId = channel.slm_ChannelId
        //                            LEFT JOIN "+ SLMDBName + @".dbo.kkslm_ms_staff staff ON lead.slm_Owner = staff.slm_UserName  
        //                            LEFT JOIN "+ SLMDBName + @".dbo.kkslm_ms_option opt ON lead.slm_Status = opt.slm_OptionCode ";

//            string whr = " lead.is_Deleted = 0 ";

//            whr += (ticketId == "" ? "" : (whr == "" ? "" : " AND ") + " lead.slm_ticketId = '" + ticketId + "' ");
//            whr += (firstname == "" ? "" : (whr == "" ? "" : " AND ") + " lead.slm_Name LIKE '%" + firstname + "%' ");
//            whr += (lastname == "" ? "" : (whr == "" ? "" : " AND ") + " info.slm_LastName LIKE '%" + lastname + "%' ");
//            whr += (citizenId == "" ? "" : (whr == "" ? "" : " AND ") + " info.slm_CitizenId = '" + citizenId + "' ");
//            whr += (campaignId == "" ? "" : (whr == "" ? "" : " AND ") + " lead.slm_CampaignId = '" + campaignId + "' ");
//            whr += (channelId == "" ? "" : (whr == "" ? "" : " AND ") + " lead.slm_ChannelId = '" + channelId + "' ");
//            whr += (ownerLeadUsername == "" ? "" : (whr == "" ? "" : " AND ") + " lead.slm_Owner = '" + ownerLeadUsername + "' ");
//            whr += (createDate == "" ? "" : (whr == "" ? "" : " AND ") + " CONVERT(DATE, lead.slm_CreatedDate) = '" + createDate + "' ");
//            whr += (assignDate == "" ? "" : (whr == "" ? "" : " AND ") + " CONVERT(DATE, lead.slm_AssignedDate) = '" + assignDate + "' ");
//            whr += (leadStatusList == "" ? "" : (whr == "" ? "" : " AND ") + " lead.slm_Status IN (" + leadStatusList + ") ");
//            whr += (telNo1 == "" ? "" : (whr == "" ? "" : " AND ") + " lead.slm_TelNo_1 = '" + telNo1 + "' ");

//            sql += (whr == "" ? "" : " WHERE " + whr);
//            sql += (orderBy == "" ? "" : " ORDER BY " + orderBy);

//            return slmdb.ExecuteStoreQuery<SearchLeadResult>(sql).ToList();
//        }

        #endregion

        public LeadData SearchSCR004Data(string ticketId)
        {
            string sql = @"SELECT lead.slm_ticketId AS TicketId,lead.slm_Name AS name, info.slm_LastName AS LastName,campaign.slm_CampaignName AS CampaignName
	                        ,channel.slm_ChannelDesc AS ChannelDesc,lead.slm_TelNo_1 AS TelNo_1,info.slm_TelNo_2 AS TelNo_2,info.slm_TelNo_3 AS TelNo_3
	                        ,lead.slm_Ext_1 AS Ext_1,info.slm_Ext_2 AS Ext_2, info.slm_Ext_3 AS Ext_3, lead.slm_Delegate_Flag AS Delegate_Flag
                            ,cardtype.slm_CardTypeName AS CardTypeDesc
	                        ,info.slm_Birthdate AS Birthdate, info.slm_CardType AS CardType, info.slm_CitizenId AS CitizenID,info.slm_Email AS Email,info.slm_Topic AS Topic
	                        ,info.slm_Detail AS Detail,lead.slm_AvailableTime AS AvailableTime,branch.slm_BranchName AS BranchName
                            ,CASE WHEN posowner.slm_PositionNameAbb IS NULL THEN staff.slm_StaffNameTH
	                              ELSE posowner.slm_PositionNameAbb + ' - ' + staff.slm_StaffNameTH END AS OwnerName
                            ,CASE WHEN posdelegate.slm_PositionNameAbb IS NULL THEN delegate.slm_StaffNameTH
	                              ELSE posdelegate.slm_PositionNameAbb + ' - ' + delegate.slm_StaffNameTH END AS DelegateName
	                        ,lead.slm_CreatedDate AS CreatedDateView
	                        ,lead.slm_AssignedDate AS AssignedDateView,info.slm_AddressNo AS AddressNo,info.slm_BuildingName AS BuildingName,info.slm_Floor AS [Floor]
	                        ,info.slm_Soi AS Soi,info.slm_Street AS Street,tambol.slm_TambolNameTH AS TambolName,amphur.slm_AmphurNameTH AS AmphurName
	                        ,province.slm_ProvinceNameTH AS ProvinceName,info.slm_PostalCode AS PostalCode,info.slm_IsCustomer AS IsCustomer,info.slm_CusCode AS CusCode
	                        ,occ.slm_OccupationNameTH AS OccupationName,info.slm_BaseSalary AS BaseSalary,prod.slm_InterestedProd AS InterestedProd
	                        ,prod.slm_LicenseNo  AS LicenseNo,prod.slm_YearOfCar AS YearOfCar,prod.slm_YearOfCarRegis AS YearOfCarRegis
	                        ,brand.slm_BrandName AS BrandName,prod.slm_CarPrice AS CarPrice,model.slm_FamilyDesc AS ModelName,submodel.slm_SubModel+' : '+ submodel.slm_Description AS SubModelName
	                        ,prod.slm_DownPayment AS DownPayment,prod.slm_DownPercent AS DownPercent,prod.slm_FinanceAmt AS FinanceAmt,prod.slm_PaymentTerm AS PaymentTerm
	                        ,prod.slm_PaymentType AS PaymentType,prod.slm_BalloonAmt AS BalloonAmt,prod.slm_BalloonPercent AS BalloonPercent
	                        ,provinceregis.slm_ProvinceNameTH AS ProvinceRegisName,prod.slm_CoverageDate AS CoverageDate,prod.slm_PlanType AS PlanType
	                        ,module.slm_ModuleNameTH AS AccTypeName,promotion.slm_PromotionName AS PromotionName,prod.slm_AccTerm AS AccTerm,prod.slm_Interest As Interest
	                        ,prod.slm_Invest AS Invest,prod.slm_LoanOd AS LoanOd,prod.slm_LoanOdTerm AS LoanOdTerm,prod.slm_Ebank AS Ebank,prod.slm_Atm AS Atm
	                        ,channelinfo.slm_Company AS Company,info.slm_PathLink AS PathLink,opt.slm_OptionDesc AS StatusName
	                        ,phone1.slm_CreateDate AS ContactLatestDate,phone2.slm_CreateDate AS ContactFirstDate,branchprod.slm_BranchName as Branchprod
                            ,lead.slm_CampaignId AS CampaignId ,lead.slm_Status AS [Status],lead.slm_Owner AS [Owner] ,lead.slm_Delegate As Delegate 
                            ,lead.slm_ChannelId As ChannelId, lead.slm_CreatedDate As LeadCreateDate, channelinfo.slm_branch As Branch 
                            ,info.slm_Occupation As Occupation, info.slm_ContactBranch As ContactBranch, info.slm_Province As Province  
                            ,info.slm_Amphur As Amphur, info.slm_Tambon As Tambon, prod.slm_ProvinceRegis As ProvinceRegis,prod.slm_Brand As Brand
                            ,prod.slm_Model As Model, prod.slm_Submodel As SubModel,prod.slm_AccType As AccType,prod.slm_AccPromotion As AccPromotion 
                            ,prod.slm_CarType As CarType,province.slm_ProvinceCode AS ProvinceCode,tambol.slm_TambolCode As TambolCode
                            ,amphur.slm_AmphurCode As AmphurCode,provinceregis.slm_ProvinceCode As ProvinceRegisCode
                            ,brand.slm_BrandCode AS BrandCode,model.slm_Family As Family,Delegatebranch.slm_BranchName As DelegatebranchName, lead.slm_NoteFlag AS NoteFlag
                            ,PaymentType.slm_PaymentNameTH As PaymentName,PlanBanc.slm_Plan_Banc_T_Desc As PlanBancName, lead.slm_AssignedFlag AS AssignedFlag  
                            ,CASE WHEN ISNULL(createby.slm_StaffNameTH, lead.slm_CreatedBy) = lead.slm_CreatedBy THEN lead.slm_CreatedBy
	                              WHEN poscreateby.slm_PositionNameAbb IS NULL THEN createby.slm_StaffNameTH
	                              ELSE poscreateby.slm_PositionNameAbb + ' - ' + createby.slm_StaffNameTH END AS LeadCreateBy
                            ,model.slm_Family AS ModelFamily,CONVERT(VARCHAR, submodel.slm_RedBookNo) AS SubModelCode, PaymentType.slm_PaymentCode AS PaymentTypeCode
                            ,CONVERT(VARCHAR, module.slm_ModuleCode) AS AccTypeCode, slm_PromotionCode AS AccPromotionCode, occ.slm_OccupationCode AS OccupationCode
                            ,OwnerBranch.slm_BranchName AS OwnerBranchName, lead.slm_Owner_Branch AS Owner_Branch
                            ,lead.slm_Delegate_Branch AS Delegate_Branch,lead.slm_CreatedBy_Branch AS CreatedBy_Branch, lead.slm_DealerCode AS DealerCode, lead.slm_DealerName AS DealerName 
                            ,ISNULL(MP.HasADAMUrl, 0) AS HasAdamsUrl, campaign.slm_url AS CalculatorUrl, lead.coc_Appno AS AppNo, lead.slm_Product_Id AS ProductId
                            ,CONVERT(VARCHAR,lead.coc_IsCOC) AS ISCOC, lead.coc_CurrentTeam AS COCCurrentTeam 
                            ,CASE WHEN ISNULL(mktowner.slm_StaffNameTH, lead.coc_MarketingOwner) = lead.coc_MarketingOwner THEN lead.coc_MarketingOwner
                                  WHEN posmktowner.slm_PositionNameAbb IS NULL THEN mktowner.slm_StaffNameTH
                                  ELSE posmktowner.slm_PositionNameAbb + ' - ' + mktowner.slm_StaffNameTH END AS MarketingOwnerName
                            ,CASE WHEN ISNULL(lastowner.slm_StaffNameTH, lead.coc_LastOwner) = lead.coc_LastOwner THEN lead.coc_LastOwner
	                              WHEN poslastowner.slm_PositionNameAbb IS NULL THEN lastowner.slm_StaffNameTH
	                              ELSE poslastowner.slm_PositionNameAbb + ' - ' + lastowner.slm_StaffNameTH END AS LastOwnerName
                            ,cocstatus.slm_OptionDesc AS CocStatusDesc, lead.coc_AssignedDate AS CocAssignedDate, lead.slm_ContractNoRefer AS ContractNoRefer, lead.slm_ExternalSubStatusDesc AS ExternalSubStatusDesc
                        FROM " + SLMDBName + @".dbo.kkslm_tr_lead lead 
                            LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_staff createby on createby.slm_UserName = lead.slm_CreatedBy
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_position poscreateby ON lead.slm_CreatedBy_Position = poscreateby.slm_Position_id 
	                        LEFT JOIN " + SLMDBName + @".dbo.kkslm_tr_cusinfo info ON lead.slm_ticketId = info.slm_TicketId
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_tr_productinfo prod ON prod.slm_TicketId = lead.slm_ticketId
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_campaign campaign ON lead.slm_CampaignId = campaign.slm_CampaignId
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_channel channel ON lead.slm_ChannelId = channel.slm_ChannelId
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff staff ON lead.slm_Owner = staff.slm_UserName  
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_position posowner ON lead.slm_Owner_Position = posowner.slm_Position_id
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_option opt ON lead.slm_Status = opt.slm_OptionCode AND opt.slm_OptionType = 'lead status' AND opt.is_Deleted = '0' 
                            LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_branch branch ON branch.slm_BranchCode = info.slm_ContactBranch 
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff delegate ON lead.slm_Delegate = delegate.slm_UserName  
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_position posdelegate ON lead.slm_Delegate_Position = posdelegate.slm_Position_id 
                            LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_tambol tambol ON tambol.slm_TambolId = info.slm_Tambon 
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_amphur amphur ON amphur.slm_AmphurId = info.slm_Amphur
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_province province ON province.slm_ProvinceId = info.slm_Province 
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_occupation occ ON occ.slm_OccupationId = info.slm_Occupation 
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_brand brand ON brand.slm_BrandId = prod.slm_Brand 
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_model model ON model.slm_ModelId = prod.slm_Model 
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_submodel submodel ON submodel.slm_SubModelId = prod.slm_Submodel 
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_province provinceregis ON provinceregis.slm_ProvinceId = prod.slm_ProvinceRegis 
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_module module ON module.slm_ModuleId = prod.slm_AccType 
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_promotion promotion ON promotion.slm_PromotionId = prod.slm_AccPromotion 
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_tr_channelinfo channelinfo on channelinfo.slm_TicketId = lead.slm_ticketId 
                                LEFT JOIN (SELECT TOP 1 slm_CreateDate,slm_TicketId FROM " + SLMDBName + @".DBO.kkslm_phone_call WHERE slm_TicketId = '" + ticketId + @"' ORDER BY slm_CreateDate DESC) AS phone1 
		                        ON phone1.slm_TicketId = LEAD.slm_ticketId 
	                        LEFT JOIN (SELECT TOP 1 slm_CreateDate,slm_TicketId FROM " + SLMDBName + @".DBO.kkslm_phone_call WHERE slm_TicketId = '" + ticketId + @"' ORDER BY slm_CreateDate ASC) AS phone2
		                        ON phone2.slm_TicketId = LEAD.slm_ticketId 
					        LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_branch branchProd on branchProd.slm_BranchCode = channelinfo.slm_branch 
                            LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_branch Delegatebranch on Delegatebranch.slm_BranchCode = lead.slm_Delegate_Branch 
                            LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_paymenttype PaymentType on PaymentType.slm_PaymentId = prod.slm_PaymentType
                            LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_plan_banc PlanBanc on PlanBanc.slm_Plan_Banc_Code = prod.slm_PlanType 
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_branch OwnerBranch on OwnerBranch.slm_BranchCode = lead.slm_Owner_Branch 
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff mktowner ON lead.coc_MarketingOwner = mktowner.slm_EmpCode
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_position posmktowner ON lead.coc_MarketingOwner_Position = posmktowner.slm_Position_id
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff lastowner ON lead.coc_LastOwner = lastowner.slm_EmpCode
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_position poslastowner ON lead.coc_LastOwner_Position = poslastowner.slm_Position_id 
                            LEFT JOIN (SELECT DISTINCT slm_OptionCode, slm_OptionSubCode, slm_OptionType, slm_OptionDesc
			                            FROM " + SLMDBName + @".dbo.kkslm_ms_option ) cocstatus ON lead.coc_Status = cocstatus.slm_OptionCode AND ISNULL(lead.coc_SubStatus, '0123456789') = ISNULL(cocstatus.slm_OptionSubCode, '0123456789') AND cocstatus.slm_OptionType = 'coc_status' 
                            LEFT JOIN " + SLMDBName + @".dbo.CMT_MAPPING_PRODUCT MP ON MP.sub_product_id = lead.slm_Product_Id 
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_cardtype cardtype ON info.slm_CardType = cardtype.slm_CardTypeId ";
            
            string whr = " lead.is_Deleted = 0 ";

            whr += (ticketId == "" ? "" : (whr == "" ? "" : " AND ") + " lead.slm_ticketId = '" + ticketId + "' ");

            sql += (whr == "" ? "" : " WHERE " + whr);
            sql += " ORDER BY lead.slm_ticketId";

            return slmdb.ExecuteStoreQuery<LeadData>(sql).FirstOrDefault();
        }

        //Reference : SlmScr006Biz
        public List<ExistingProductData> SearchExistingProduct(string citizenId)
        {
            if (!string.IsNullOrEmpty(citizenId))
            {
                //return slmdb.kkslm_existing_product.Where(p => p.slm_CitizenId == citizenId).AsEnumerable().Select((p, index) => new ExistingProductData
                //{
                //    No = (index + 1).ToString(),
                //    CitizenId = p.slm_CitizenId,
                //    ProductGroup = p.slm_ProductGroup,
                //    ProductName = p.slm_ProductName,
                //    Grade = p.slm_Grade,
                //    ContactNo = p.slm_ContactNo,
                //    StartDate = p.slm_StartDate,
                //    EndDate = p.slm_EndDate,
                //    PaymentTerm = p.slm_PaymentTerm,
                //    Status = p.slm_Status
                //}).ToList();

                return slmdb.kkslm_existing_product.Where(p => p.slm_CitizenId == citizenId).AsEnumerable().Select(p => new ExistingProductData
                {
                    CitizenId = p.slm_CitizenId,
                    ProductGroup = p.slm_ProductGroup,
                    ProductName = p.slm_ProductName,
                    Grade = p.slm_Grade,
                    ContactNo = p.slm_ContactNo,
                    StartDate = p.slm_StartDate,
                    EndDate = p.slm_EndDate,
                    PaymentTerm = p.slm_PaymentTerm,
                    Status = p.slm_Status
                }).ToList();
            }
            else
                return new List<ExistingProductData>();
        }

        //Reference : SlmScr007Biz
        public List<OwnerLoggingData> SearchOwnerLogging(string ticketId)
        { 
//            string sql = @" SELECT act.slm_CreatedDate AS CreatedDate, lead.slm_ticketId AS TicketId, lead.slm_Name AS Firstname, info.slm_CitizenId AS CitizenId, 
//                            info.slm_LastName AS LastName, optOld.slm_OptionDesc AS OldStatusDesc, staffOld.slm_StaffNameTH AS OldOwnerName,
//                            optNew.slm_OptionDesc AS NewStatusDesc, staffNew.slm_StaffNameTH AS NewOwnerName,DelegateOld.slm_StaffNameTH AS OldDelegateName,
//                            DelegateNew.slm_StaffNameTH AS NewDelegateName,
//                            CASE WHEN ACT.slm_Type = '01' THEN 'System Assign' 
//                                 WHEN ACT.slm_Type = '02' THEN 'Change Status'
//                                 WHEN ACT.slm_Type = '03' THEN 'Delegate' 
//                                 WHEN ACT.slm_Type = '04' THEN 'Transfer' 
//                                 WHEN ACT.slm_Type = '05' THEN 'User Assign' 
//                                 WHEN ACT.slm_Type = '06' THEN 'Consulidate' 
//                                 WHEN ACT.slm_Type = '07' THEN 'Reset Owner' 
//                                 WHEN ACT.slm_Type = '08' THEN 'Update Owner' ELSE '' END Action ,
//	                        ISNULL(CreateBy.slm_StaffNameTH, act.slm_CreatedBy) as CreateBy  
//                            FROM " + SLMDBName + @".dbo.kkslm_tr_activity act
//                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_option optOld ON act.slm_OldStatus = optOld.slm_OptionCode AND optOld.slm_OptionType = 'lead status' AND optOld.is_Deleted = '0'
//                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_option optNew ON act.slm_NewStatus = optNew.slm_OptionCode AND optNew.slm_OptionType = 'lead status' AND optNew.is_Deleted = '0'
//                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff staffOld ON act.slm_OldOwner = staffOld.slm_UserName
//                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff staffNew ON act.slm_NewOwner = staffNew.slm_UserName
//                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_tr_Lead lead ON lead.slm_ticketId = act.slm_TicketId
//                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_tr_cusinfo info ON lead.slm_ticketId = info.slm_TicketId
//                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff DelegateOld ON act.slm_OldDelegate = DelegateOld.slm_UserName
//                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff DelegateNew ON act.slm_NewDelegate = DelegateNew.slm_UserName
//                            LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_staff CreateBy on CreateBy.slm_UserName = act.slm_CreatedBy 
//                            WHERE lead.slm_TicketId = '" + ticketId + @"' AND act.is_Deleted = '0' AND act.slm_WorkId IS NULL 
//                            ORDER BY act.slm_CreatedDate DESC ";
            string sql = @"SELECT Z.* FROM (

                                SELECT act.slm_CreatedDate AS CreatedDate, lead.slm_ticketId AS TicketId, lead.slm_Name AS Firstname, info.slm_CitizenId AS CitizenId, 
                                    info.slm_LastName AS LastName, optOld.slm_OptionDesc AS OldStatusDesc, act.slm_ExternalSubStatusDesc_Old AS OldSubStatusDesc
                                    ,CASE WHEN posoldowner.slm_PositionNameAbb IS NULL THEN staffOld.slm_StaffNameTH
		                                  ELSE posoldowner.slm_PositionNameAbb + ' - ' + staffOld.slm_StaffNameTH END AS OldOwnerName
                                    ,CASE WHEN posnewowner.slm_PositionNameAbb IS NULL THEN staffNew.slm_StaffNameTH
		                                  ELSE posnewowner.slm_PositionNameAbb + ' - ' + staffNew.slm_StaffNameTH END AS NewOwnerName
                                    ,CASE WHEN posolddelegate.slm_PositionNameAbb IS NULL THEN DelegateOld.slm_StaffNameTH
		                                  ELSE posolddelegate.slm_PositionNameAbb + ' - ' + DelegateOld.slm_StaffNameTH END AS OldDelegateName
                                    ,CASE WHEN posnewdelegate.slm_PositionNameAbb IS NULL THEN DelegateNew.slm_StaffNameTH
		                                  ELSE posnewdelegate.slm_PositionNameAbb + ' - ' + DelegateNew.slm_StaffNameTH END AS NewDelegateName
                                    ,optNew.slm_OptionDesc AS NewStatusDesc, act.slm_ExternalSubStatusDesc_New AS NewSubStatusDesc
                                    ,CASE WHEN ACT.slm_Type = '01' THEN 'System Assign' 
                                         WHEN ACT.slm_Type = '02' THEN 'Change Status'
                                         WHEN ACT.slm_Type = '03' THEN 'Delegate' 
                                         WHEN ACT.slm_Type = '04' THEN 'Transfer' 
                                         WHEN ACT.slm_Type = '05' THEN 'User Assign' 
                                         WHEN ACT.slm_Type = '06' THEN 'Consolidate' 
                                         WHEN ACT.slm_Type = '07' THEN 'Reset Owner' 
                                         WHEN ACT.slm_Type = '08' THEN 'Update Owner' 
                                         WHEN ACT.slm_Type = '09' THEN 'EOD Update Current' 
                                         WHEN ACT.slm_Type = '10' THEN 'Change Owner' 
                                         WHEN ACT.slm_Type = '11' THEN 'EOD History Logs' 
                                         WHEN ACT.slm_Type = '12' THEN 'Error Assign' 
                                         ELSE '' END Action
                                    ,CASE WHEN ISNULL(CreateBy.slm_StaffNameTH, act.slm_CreatedBy) = act.slm_CreatedBy THEN act.slm_CreatedBy
		                                 WHEN poscreateby.slm_PositionNameAbb IS NULL THEN CreateBy.slm_StaffNameTH
		                                 ELSE poscreateby.slm_PositionNameAbb + ' - ' + CreateBy.slm_StaffNameTH END AS CreateBy
                                    ,ISNULL(ACT.slm_SystemAction, 'SLM') AS SystemAction     
                                FROM " + SLMDBName + @".dbo.kkslm_tr_activity act
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_option optOld ON act.slm_OldStatus = optOld.slm_OptionCode AND optOld.slm_OptionType = 'lead status'
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_option optNew ON act.slm_NewStatus = optNew.slm_OptionCode AND optNew.slm_OptionType = 'lead status'
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_position posoldowner ON act.slm_OldOwner_Position = posoldowner.slm_Position_id
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff staffOld ON act.slm_OldOwner = staffOld.slm_UserName
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_position posnewowner ON act.slm_NewOwner_Position = posnewowner.slm_Position_id
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff staffNew ON act.slm_NewOwner = staffNew.slm_UserName
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_tr_Lead lead ON lead.slm_ticketId = act.slm_TicketId
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_tr_cusinfo info ON lead.slm_ticketId = info.slm_TicketId
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_position posolddelegate ON act.slm_OldDelegate_Position = posolddelegate.slm_Position_id
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff DelegateOld ON act.slm_OldDelegate = DelegateOld.slm_UserName
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_position posnewdelegate ON act.slm_NewDelegate_Position = posnewdelegate.slm_Position_id
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff DelegateNew ON act.slm_NewDelegate = DelegateNew.slm_UserName
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_position poscreateby ON act.slm_CreatedBy_Position = poscreateby.slm_Position_id
                                LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_staff CreateBy on CreateBy.slm_UserName = act.slm_CreatedBy 
                                WHERE lead.slm_TicketId = '" + ticketId + @"' AND act.is_Deleted = '0' AND act.slm_WorkId IS NULL AND ACT.coc_Team IS NULL
                                UNION ALL
                                SELECT act.slm_CreatedDate AS CreatedDate, lead.slm_ticketId AS TicketId, lead.slm_Name AS Firstname, info.slm_CitizenId AS CitizenId, 
                                    info.slm_LastName AS LastName, optOld.slm_OptionDesc AS OldStatusDesc, act.slm_ExternalSubStatusDesc_Old AS OldSubStatusDesc
                                    ,CASE WHEN posoldowner.slm_PositionNameAbb IS NULL THEN staffOld.slm_StaffNameTH
		                                  ELSE posoldowner.slm_PositionNameAbb + ' - ' + staffOld.slm_StaffNameTH END AS OldOwnerName
                                    ,CASE WHEN posnewowner.slm_PositionNameAbb IS NULL THEN staffNew.slm_StaffNameTH
		                                  ELSE posnewowner.slm_PositionNameAbb + ' - ' + staffNew.slm_StaffNameTH END AS NewOwnerName
                                    ,CASE WHEN posolddelegate.slm_PositionNameAbb IS NULL THEN DelegateOld.slm_StaffNameTH
		                                  ELSE posolddelegate.slm_PositionNameAbb + ' - ' + DelegateOld.slm_StaffNameTH END AS OldDelegateName
                                    ,CASE WHEN posnewdelegate.slm_PositionNameAbb IS NULL THEN DelegateNew.slm_StaffNameTH
		                                  ELSE posnewdelegate.slm_PositionNameAbb + ' - ' + DelegateNew.slm_StaffNameTH END AS NewDelegateName
                                    ,optNew.slm_OptionDesc AS NewStatusDesc, act.slm_ExternalSubStatusDesc_New AS NewSubStatusDesc
                                    ,CASE WHEN ACT.slm_Type = '01' THEN 'System Assign' 
                                         WHEN ACT.slm_Type = '02' THEN 'Change Status'
                                         WHEN ACT.slm_Type = '03' THEN 'Delegate' 
                                         WHEN ACT.slm_Type = '04' THEN 'Transfer' 
                                         WHEN ACT.slm_Type = '05' THEN 'User Assign' 
                                         WHEN ACT.slm_Type = '06' THEN 'Consolidate' 
                                         WHEN ACT.slm_Type = '07' THEN 'Reset Owner' 
                                         WHEN ACT.slm_Type = '08' THEN 'Update Owner' 
                                         WHEN ACT.slm_Type = '09' THEN 'EOD Update Current' 
                                         WHEN ACT.slm_Type = '10' THEN 'Change Owner' 
                                         WHEN ACT.slm_Type = '11' THEN 'EOD History Logs' 
                                         WHEN ACT.slm_Type = '12' THEN 'Error Assign' 
                                         ELSE '' END Action
                                    ,CASE WHEN ISNULL(CreateBy.slm_StaffNameTH, act.slm_CreatedBy) = act.slm_CreatedBy THEN act.slm_CreatedBy
		                                 WHEN poscreateby.slm_PositionNameAbb IS NULL THEN CreateBy.slm_StaffNameTH
		                                 ELSE poscreateby.slm_PositionNameAbb + ' - ' + CreateBy.slm_StaffNameTH END AS CreateBy
                                    ,ISNULL(ACT.slm_SystemAction, 'SLM') AS SystemAction    
                                FROM " + SLMDBName + @".dbo.kkslm_tr_activity act
                                LEFT JOIN (
				                            SELECT DISTINCT slm_OptionCode, slm_OptionSubCode, slm_OptionType, slm_OptionDesc
				                            FROM " + SLMDBName + @".dbo.kkslm_ms_option ) optOld ON act.slm_OldStatus = optOld.slm_OptionCode AND ISNULL(ACT.slm_OldSubStatus,'0123456789') = ISNULL(optOld.slm_OptionSubCode,'0123456789') AND optOld.slm_OptionType = 'coc_status' 
                                LEFT JOIN (
				                            SELECT DISTINCT slm_OptionCode, slm_OptionSubCode, slm_OptionType, slm_OptionDesc
				                            FROM " + SLMDBName + @".dbo.kkslm_ms_option ) optNew ON act.slm_NewStatus = optNew.slm_OptionCode AND ISNULL(ACT.slm_NewSubStatus,'0123456789') = ISNULL(optNew.slm_OptionSubCode,'0123456789') AND optNew.slm_OptionType = 'coc_status' 
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_position posoldowner ON act.slm_OldOwner_Position = posoldowner.slm_Position_id
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff staffOld ON act.slm_OldOwner = staffOld.slm_EmpCode
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_position posnewowner ON act.slm_NewOwner_Position = posnewowner.slm_Position_id
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff staffNew ON act.slm_NewOwner = staffNew.slm_EmpCode
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_tr_Lead lead ON lead.slm_ticketId = act.slm_TicketId
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_tr_cusinfo info ON lead.slm_ticketId = info.slm_TicketId
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_position posolddelegate ON act.slm_OldDelegate_Position = posolddelegate.slm_Position_id
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff DelegateOld ON act.slm_OldDelegate = DelegateOld.slm_EmpCode
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_position posnewdelegate ON act.slm_NewDelegate_Position = posnewdelegate.slm_Position_id
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff DelegateNew ON act.slm_NewDelegate = DelegateNew.slm_EmpCode
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_position poscreateby ON act.slm_CreatedBy_Position = poscreateby.slm_Position_id
                                LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_staff CreateBy on CreateBy.slm_UserName = act.slm_CreatedBy 
                                WHERE lead.slm_TicketId = '" + ticketId + @"' AND act.is_Deleted = '0' AND act.slm_WorkId IS NULL AND ACT.coc_Team IS NOT NULL) AS Z
                                ORDER BY Z.CreatedDate DESC
                                ";

            return slmdb.ExecuteStoreQuery<OwnerLoggingData>(sql).ToList();
        }

        //Reference : SlmScr008Biz
        public List<PhoneCallHistoryData> SearchPhoneCallHistory(string citizenId, string ticketid, bool thisLead)
        {
            string sql = @"SELECT phone.slm_CreateDate AS CreatedDate, lead.slm_ticketId AS TicketId, lead.slm_Name AS Firstname, opt.slm_OptionDesc AS StatusDesc, 
                            cardtype.slm_CardTypeName AS CardTypeDesc,
                            info.slm_CitizenId AS CitizenId, info.slm_LastName AS LastName, phone.slm_ContactPhone AS ContactPhone, phone.slm_ContactDetail AS ContactDetail
                            ,CASE WHEN posowner.slm_PositionNameAbb IS NULL THEN staff.slm_StaffNameTH
	                              ELSE posowner.slm_PositionNameAbb + ' - ' + staff.slm_StaffNameTH END AS OwnerName
                            ,cam.slm_CampaignName AS CampaignName
                            ,CASE WHEN ISNULL(staff2.slm_StaffNameTH, phone.slm_CreateBy) = phone.slm_CreateBy THEN phone.slm_CreateBy
	                              WHEN poscreateby.slm_PositionNameAbb IS NULL THEN staff2.slm_StaffNameTH
	                              ELSE poscreateby.slm_PositionNameAbb + ' - ' + staff2.slm_StaffNameTH END AS CreatedName
                            FROM " + SLMDBName + @".dbo.kkslm_phone_call phone
                            INNER JOIN " + SLMDBName + @".dbo.kkslm_tr_lead lead on phone.slm_TicketId = lead.slm_ticketId
                            INNER JOIN " + SLMDBName + @".dbo.kkslm_tr_cusinfo info ON lead.slm_ticketId = info.slm_TicketId
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_position posowner ON phone.slm_Owner_Position = posowner.slm_Position_id
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff staff ON phone.slm_Owner = staff.slm_UserName  
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_option opt ON phone.slm_Status = opt.slm_OptionCode AND opt.slm_OptionType = 'lead status' 
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_campaign cam ON lead.slm_CampaignId = cam.slm_CampaignId
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_position poscreateby ON phone.slm_CreatedBy_Position = poscreateby.slm_Position_id
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff staff2 ON phone.slm_CreateBy = staff2.slm_UserName 
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_cardtype cardtype ON info.slm_CardType = cardtype.slm_CardTypeId ";

            //if (!string.IsNullOrEmpty(ticketid))
            //{
            //    sql += " WHERE lead.slm_ticketId = '" + ticketid + "' ";
            //}
            //else
            //{
            //    if (!string.IsNullOrEmpty(citizenId))
            //        sql += " WHERE (info.slm_CitizenId = '" + citizenId + "' OR lead.slm_TelNo_1 = '" + telNo1 + @"') AND phone.is_Deleted = '0' ";
            //    else
            //        sql += " WHERE lead.slm_TelNo_1 = '" + telNo1 + @"' AND phone.is_Deleted = '0' ";
            //}

            if (thisLead)
            {
                sql += " WHERE phone.slm_TicketId = '" + ticketid + "' AND phone.is_Deleted = '0' ";
            }
            else
            {
                if (!string.IsNullOrEmpty(citizenId))
                    sql += " WHERE (info.slm_CitizenId = '" + citizenId + "' OR phone.slm_TicketId = '" + ticketid + @"') AND phone.is_Deleted = '0' ";
                else
                    sql += " WHERE phone.slm_TicketId = '" + ticketid + @"' AND phone.is_Deleted = '0' ";
            }
          
            sql += " ORDER BY phone.slm_CreateDate DESC";

            return slmdb.ExecuteStoreQuery<PhoneCallHistoryData>(sql).ToList();
        }

        //Reference : SlmScr009Biz
        public List<NoteHistoryData> SearchNoteHistory(string ticketId)
        {
            string sql = @"SELECT note.slm_CreateDate AS CreatedDate, note.slm_TicketId AS TicketId
                            ,CASE WHEN ISNULL(staff.slm_StaffNameTH, NOTE.slm_CreateBy) = NOTE.slm_CreateBy THEN NOTE.slm_CreateBy
	                              WHEN pos.slm_PositionNameAbb IS NULL THEN staff.slm_StaffNameTH
	                              ELSE pos.slm_PositionNameAbb + ' - ' + staff.slm_StaffNameTH END AS CreateBy
                            ,note.slm_NoteDetail AS NoteDetail,note.slm_EmailSubject AS EmailSubject, note.slm_SendEmailFlag AS SendEmailFlag
                            FROM " + SLMDBName + @".dbo.kkslm_note note
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_position pos ON note.slm_CreatedBy_Position = pos.slm_Position_id
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff staff ON note.slm_CreateBy = staff.slm_UserName
                            WHERE note.slm_TicketId = '" + ticketId + @"'
                            ORDER BY note.slm_CreateDate DESC";

            return slmdb.ExecuteStoreQuery<NoteHistoryData>(sql).ToList();
        }

        //Reference : SlmScr015Biz
        public string GetNumOfUnassignLead(string username,decimal? stafftype)
        {
            //string[] statusCode = { "00", "01" };   //สนใจ, ติดต่อไม่ได้
            //return slmdb.kkslm_tr_lead.Where(p => p.slm_Owner == null && p.is_Deleted == 0 && statusCode.Contains(p.slm_Status)).Count().ToString("#,##0");

            #region            //Back up 24/3/2014
            //            string sql = @"SELECT COUNT(LEAD.slm_ticketId) AS UnAssignCount
//                            FROM " + SLMDBName + @".dbo.kkslm_tr_lead lead INNER JOIN 
//                            (
//		                            SELECT DISTINCT B.SLM_CAMPAIGNID
//		                            FROM
//		                            (
//		                            SELECT SLM_CAMPAIGNID
//		                            FROM " + SLMDBName + @".DBO.kkslm_ms_group [GROUP] 
//		                            WHERE slm_GroupId IN (
//				                            SELECT slm_GroupId 
//				                            FROM " + SLMDBName + @".DBO.kkslm_ms_staff_group 
//				                            WHERE is_Deleted = 0 AND slm_StaffId IN (
//					                            SELECT slm_StaffId
//					                            FROM " + SLMDBName + @".DBO.kkslm_ms_staff staff 
//					                            WHERE staff.slm_UserName = '" + username + @"'))
//		                            UNION ALL
//		                            SELECT SLM_CAMPAIGNID
//		                            FROM " + SLMDBName + @".DBO.kkslm_ms_group [GROUP] 
//		                            WHERE slm_StaffId IN (
//			                            SELECT slm_StaffId
//			                            FROM " + SLMDBName + @".DBO.kkslm_ms_staff staff 
//			                            WHERE staff.slm_UserName = '" + username + @"')
//		                            ) AS B) AS Z ON Z.slm_CampaignId = LEAD.slm_CampaignId 
            //                              WHERE slm_Status IN ('" + SLMConstant.StatusCode.Interest + @"','" + SLMConstant.StatusCode.WaitContact + @"') AND lead.slm_Owner IS NULL AND LEAD.is_Deleted = 0 
            //                            ";
            #endregion

            #region //Code เดิม ก่อน CR5
//            string sql = @"SELECT COUNT(LEAD.slm_ticketId) AS UnAssignCount
//                            FROM " + SLMDBName + @".dbo.kkslm_tr_lead lead INNER JOIN 
//                            (
//	                            SELECT DISTINCT SLM_CAMPAIGNID 
//	                            FROM " + SLMDBName + @".DBO.kkslm_ms_staff_group 
//	                            WHERE is_Deleted = 0 AND slm_StaffId IN (
//		                            SELECT slm_StaffId
//		                            FROM " + SLMDBName + @".DBO.kkslm_ms_staff staff 
//		                            WHERE staff.slm_UserName = '" + username + @"')
//		                      ) AS Z ON Z.slm_CampaignId = LEAD.slm_CampaignId 
            //                              WHERE slm_Status IN ('" + SLMConstant.StatusCode.Interest + @"','" + SLMConstant.StatusCode.WaitContact + @"')                                                            
//                              AND lead.slm_Owner IS NULL AND LEAD.is_Deleted = 0 ";
            #endregion

            KKSlmMsStaffModel staff = new KKSlmMsStaffModel();
            string UserLoginBranchCode = staff.GetBrachCode(username);
            string sql = @"SELECT COUNT(slm_ticketId) AS UnAssignCount
                            FROM " + SLMDBName + @".DBO.kkslm_tr_lead 
                            WHERE is_Deleted = 0 AND slm_Owner IS NULL 
                                AND slm_Status IN ('" + SLMConstant.StatusCode.Interest + @"','" + SLMConstant.StatusCode.WaitContact + @"')
                                AND slm_CAMPAIGNID IN 
	                            ( SELECT CP.PR_CampaignId AS slm_CampaignId FROM kkslm_ms_access_right AR INNER  JOIN CMT_CAMPAIGN_PRODUCT CP ON CP.PR_ProductId = AR.slm_Product_Id
                                    WHERE AR.slm_Product_Id IS NOT NULL AND
                                    AR.slm_BranchCode = '" + UserLoginBranchCode + @"' AND AR.slm_StaffTypeId = '" + stafftype + @"'
                                    UNION ALL
                                    SELECT AR.slm_CampaignId  AS slm_CampaignId  FROM kkslm_ms_access_right AR 
                                    WHERE AR.slm_CampaignId IS NOT NULL AND
                                    AR.slm_BranchCode = '" + UserLoginBranchCode + @"' AND AR.slm_StaffTypeId = '" + stafftype + @"')
                            ";

            return slmdb.ExecuteStoreQuery<int>(sql).Select(p => p.ToString("#,##0")).FirstOrDefault();
        }

        //Reference : SlmScr015Biz
        public List<UserMonitoringData> SearchUserMonitoring(string userList, string active, string assignDateFrom, string asssignDateTo)
        {
            
            #region code before SLM5
            string sql = @" SELECT ISNULL(X.AMOUNT,0) AS AMOUNT ,STAFF.slm_StaffNameTH AS FullnameTH, STAFF.slm_UserName AS Username, STAFF.slm_IsActive AS ACTIVE 
		                            ,ST.slm_StaffTypeDesc AS [ROLE]
                            FROM 
                            SLMDB.DBO.kkslm_ms_staff STAFF INNER JOIN SLMDB.DBO.kkslm_ms_staff_type ST ON ST.slm_StaffTypeId = STAFF.slm_StaffTypeId
                            LEFT JOIN 
                            (
                            SELECT SUM(Z.AMOUNT) AS AMOUNT,Z.Username,Z.ROLE AS [ROLE],Z.ACTIVE
                            FROM
                            (
                                SELECT COUNT(B.slm_ticketId) AS AMOUNT,B.Username,B.ROLE AS [ROLE],B.ACTIVE
                                FROM (
                                        SELECT DISTINCT lead.slm_ticketId ,lead.slm_Owner AS Username,staff.slm_StaffTypeId AS [ROLE], staff.slm_IsActive AS ACTIVE
                                        FROM SLMDB.dbo.kkslm_tr_lead lead INNER JOIN SLMDB.DBO.kkslm_ms_staff staff on staff.slm_UserName = lead.slm_Owner
                                        WHERE  LEAD.slm_Owner IN (" + userList + @")  AND LEAD.is_Deleted = 0 
				                            AND STAFF.slm_IsActive  IN (" + active + @") 
                                            AND STAFF.slm_StaffTypeId IN ('" + SLMConstant.StaffType.Supervisor + @"','" + SLMConstant.StaffType.Telesales + @"') ";
            if (assignDateFrom != "" && asssignDateTo != "")
                sql += "                    AND CONVERT(DATE, LEAD.slm_AssignedDate) BETWEEN '" + assignDateFrom + @"' AND '" + asssignDateTo + @"' ";
            sql += @"                ) B
                               GROUP BY  B.Username,B.ROLE ,B.ACTIVE
                            ) AS Z
                            GROUP BY Z.Username,Z.ROLE ,Z.ACTIVE 
                            ) AS X  ON X.Username = STAFF.slm_UserName 
                            WHERE STAFF.slm_UserName IN  (" + userList + @") AND STAFF.slm_IsActive  IN (" + active + @")  
                            ORDER BY X.AMOUNT DESC,STAFF.slm_StaffTypeId ASC, STAFF.slm_StaffNameTH ASC ";
            

            #region CR Include Delegate
            //            string sql = @" SELECT ISNULL(X.AMOUNT,0) AS AMOUNT ,STAFF.slm_StaffNameTH AS FullnameTH, STAFF.slm_UserName AS Username, STAFF.slm_IsActive AS ACTIVE 
//		                            ,ST.slm_StaffTypeDesc AS [ROLE]
//                            FROM 
//                            SLMDB.DBO.kkslm_ms_staff STAFF INNER JOIN SLMDB.DBO.kkslm_ms_staff_type ST ON ST.slm_StaffTypeId = STAFF.slm_StaffTypeId
//                            LEFT JOIN 
//                            (
//                            SELECT SUM(Z.AMOUNT) AS AMOUNT,Z.Username,Z.ROLE AS [ROLE],Z.ACTIVE
//                            FROM
//                            (
//                                SELECT COUNT(B.slm_ticketId) AS AMOUNT,B.Username,B.ROLE AS [ROLE],B.ACTIVE
//                                FROM (
//                                        SELECT DISTINCT A.*
//                                        FROM 
//                                        (
//                                            SELECT lead.slm_ticketId ,lead.slm_Owner AS Username,staff.slm_StaffTypeId AS [ROLE], staff.slm_IsActive AS ACTIVE
//                                            FROM SLMDB.dbo.kkslm_tr_lead lead INNER JOIN SLMDB.DBO.kkslm_ms_staff staff on staff.slm_UserName = lead.slm_Owner
//                                            WHERE  LEAD.slm_Owner IN (" + userList + @")  AND LEAD.is_Deleted = 0 
//				                                AND LEAD.slm_Delegate IS NULL AND STAFF.slm_IsActive  IN (" + active + @") 
//                                                AND STAFF.slm_StaffTypeId IN ('" + SLMConstant.StaffType.Supervisor + @"','" + SLMConstant.StaffType.Telesales + @"') "; 
//                                     if(assignDateFrom != "" && asssignDateTo != "")
//                                        sql += " AND CONVERT(DATE, LEAD.slm_AssignedDate) BETWEEN '" + assignDateFrom + @"' AND '" + asssignDateTo + @"' ";
//                                 sql +=  @" UNION ALL
//                                            SELECT lead.slm_ticketId ,lead.slm_Delegate AS Username,staff.slm_StaffTypeId AS [ROLE], staff.slm_IsActive AS ACTIVE 
//                                             FROM SLMDB.dbo.kkslm_tr_lead lead  INNER JOIN SLMDB.DBO.kkslm_ms_staff staff on staff.slm_UserName = lead.slm_Delegate 
//                                             WHERE  LEAD.slm_Delegate IN (" + userList + @")  AND LEAD.is_Deleted = 0 
//				                                 AND LEAD.slm_Delegate IS NOT NULL AND STAFF.slm_IsActive  IN (" + active + @")  
//                                                 AND STAFF.slm_StaffTypeId IN ('" + SLMConstant.StaffType.Supervisor + @"','" + SLMConstant.StaffType.Telesales + @"') ";
//                                 if (assignDateFrom != "" && asssignDateTo != "")
//                                     sql += " AND LEAD.slm_AssignedDate BETWEEN '" + assignDateFrom + @"' AND '" + asssignDateTo + @"' ";
//                                 sql += @" ) A 
//                                        ) B
//                                GROUP BY  B.Username,B.ROLE ,B.ACTIVE
//                            ) AS Z
//                            GROUP BY Z.Username,Z.ROLE ,Z.ACTIVE 
//                            ) AS X  ON X.Username = STAFF.slm_UserName 
//                            WHERE STAFF.slm_UserName IN  (" + userList + @") AND STAFF.slm_IsActive  IN (" + active + @")  
//                            ORDER BY X.AMOUNT DESC,STAFF.slm_StaffTypeId ASC, STAFF.slm_StaffNameTH ASC ";
            #endregion

            #region Version 21/3/2557  ระหว่าง CR
                                 //***********************************************************************************************************
//Version 21/3/2557  ระหว่าง CR
//            string sql = @"SELECT SUM(C.AMOUNT) AS AMOUNT,C.Username,C.FullnameTH,C.ROLE,C.Active
//                            FROM
//                            (
//                                SELECT SUM(B.AMOUNT) AS AMOUNT,B.Username,B.FullnameTH,B.ROLE,B.Active
//                                FROM
//                                (
//		                            --//////////////////////////หาจำนวน ticket ในแต่ละ campaign ตาม Owner(TeleSale)//////////////////////////
//		                            SELECT COUNT(LEAD.slm_ticketId) AS AMOUNT, Z.SLM_OWNER AS Username,Z.FullnameTH,Z.[Role] AS [ROLE],Z.Active,Z.slm_StaffTypeId 
//			                              FROM 
//				                            (
//				                            --****************************** หา Campaign ของ User Login*********************************
//				                            SELECT DISTINCT CAM1.*
//				                            FROM
//				                            (
//				                            SELECT GROUP2.SLM_CAMPAIGNID,staff.slm_UserName AS SLM_OWNER ,STAFF.slm_StaffNameTH  AS FullnameTH,
//				                            stafftype.slm_StaffTypeDesc AS [Role], staff.slm_IsActive AS Active,STAFF.slm_StaffTypeId 
//				                            FROM " + SLMDBName + @".DBO.kkslm_ms_staff_group GROUP2 
//					                            INNER JOIN " + SLMDBName + @".DBO.kkslm_ms_staff staff on staff.slm_StaffId = group2.slm_StaffId
//					                            INNER JOIN " + SLMDBName + @".dbo.kkslm_ms_staff_type stafftype ON staff.slm_StaffTypeId = stafftype.slm_StaffTypeId
//					                            INNER JOIN " + SLMDBName + @".dbo.kkslm_ms_campaign campaign ON  GROUP2.SLM_CAMPAIGNID = campaign.slm_CampaignId
//				                            WHERE GROUP2.slm_StaffId IN 
//                                                    (
//                                                        SELECT DISTINCT slm_StaffId 
//                                                        FROM	kkslm_ms_staff_group SGROUP 
//                                                        WHERE	slm_CampaignId IN 
//	                                                        (
//		                                                        SELECT slm_campaignid
//		                                                        FROM " + SLMDBName + @".DBO.kkslm_ms_staff staff 
//		                                                        WHERE staff.slm_UserName = '" + username + @"' AND STAFF.slm_IsActive IN (" + active + @") 
//                                                            )
//                                                    ) 
//				                            ) AS CAM1
//				                            --******************************************************************************************
//				                             ) AS Z 
//				                             LEFT JOIN " + SLMDBName + @".dbo.kkslm_tr_lead lead ON Z.slm_CampaignId = LEAD.slm_CampaignId 
//				                             AND LEAD.slm_Owner = Z.SLM_OWNER AND LEAD.is_Deleted = 0 AND LEAD.slm_Delegate IS NULL ";
//            if(assignDateFrom != "" && asssignDateTo != "")
//                sql += " AND LEAD.slm_AssignedDate BETWEEN '" + assignDateFrom + @"' AND '" + asssignDateTo + @"' ";                 
//           sql += @"                GROUP BY Z.SLM_OWNER ,Z.slm_CampaignId,Z.FullnameTH,Z.Role,Z.Active,Z.slm_StaffTypeId      
//		                            UNION ALL
//		                            --====================หาจำนวนงานตาม TicketId โดยเอาเฉพาะที่ถูก Delegate(TeleSale)=================================
//		                            SELECT DISTINCT A.AMOUNT AS Amount,staff.slm_UserName AS Username, staff.slm_StaffNameTH AS FullnameTH,stafftype.slm_StaffTypeDesc AS Role, 
//				                            staff.slm_IsActive AS Active,staff.slm_StaffTypeId 
//		                            FROM 
//		                            (
//		                              SELECT COUNT(lead.slm_ticketId) AS AMOUNT, lead.slm_Delegate, LEAD.slm_CampaignId
//		                              FROM " + SLMDBName + @".dbo.kkslm_tr_lead lead 
//		                              WHERE  LEAD.slm_Owner = '" + username + @"' AND LEAD.is_Deleted = 0 
//				                            AND LEAD.slm_Delegate IS NOT NULL ";
//            if (assignDateFrom != "" && asssignDateTo != "")
//               sql += " AND LEAD.slm_AssignedDate BETWEEN '" + assignDateFrom + @"' AND '" + asssignDateTo + @"' ";
//            sql += @"
//		                              GROUP BY lead.slm_Delegate, LEAD.slm_CampaignId
//		                              ) A INNER JOIN " + SLMDBName + @".dbo.kkslm_ms_staff staff ON A.slm_Delegate = staff.slm_UserName and staff.is_Deleted = 0
//		                            INNER JOIN " + SLMDBName + @".dbo.kkslm_ms_staff_type stafftype ON staff.slm_StaffTypeId = stafftype.slm_StaffTypeId
//		                            INNER JOIN " + SLMDBName + @".dbo.kkslm_ms_staff_group staffgroup ON staffgroup.slm_StaffId = staff.slm_StaffId
//		                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_campaign campaign ON staffgroup.slm_CampaignId = campaign.slm_CampaignId AND campaign.slm_CampaignId = A.slm_CampaignId
//		                            WHERE staffgroup.is_Deleted = 0  AND STAFF.slm_IsActive IN (" + active + @")  
//                                ) AS B
//                                where b.slm_StaffTypeId IN ('" + SLMConstant.StaffType.Supervisor + @"','" + SLMConstant.StaffType.Telesales + @"')
//                                GROUP BY B.Username,B.FullnameTH,B.ROLE,B.Active 
//                                UNION ALL
//                                --@@@@@@@@@@@@@@@@@@@@@@@@ หาจำนวนงานตาม TicketID ตาม Owner(Marketing)@@@@@@@@@@@@@@@@@@@@@@@@@@@@
//                                SELECT SUM(Z.Amount) AS AMOUNT,Z.Username,Z.FullnameTH,Z.[ROLE],Z.Active
//                                FROM
//                                (
//                                SELECT COUNT(lead.slm_ticketid) as Amount,staff.slm_UserName AS Username,staff.slm_StaffNameTH AS FullnameTH,
//                                stafftype.slm_StaffTypeDesc As [ROLE],staff.slm_IsActive AS Active
//                                FROM kkslm_tr_lead lead inner join kkslm_ms_staff staff on staff.slm_UserName = lead.slm_Owner 
//                                    INNER JOIN kkslm_ms_staff_type stafftype on stafftype .slm_StaffTypeId = staff.slm_StaffTypeId 
//                                where STAFF.slm_StaffTypeId = '" + SLMConstant.StaffType.Marketing + @"' AND LEAD.is_Deleted = 0 
//                                    AND LEAD.slm_Delegate IS NULL 
//                                    AND LEAD.slm_Owner = '" + username + @"'  AND STAFF.slm_IsActive IN (" + active + @")  ";
//    if (assignDateFrom != "" && asssignDateTo != "")
//        sql += " AND LEAD.slm_AssignedDate BETWEEN '" + assignDateFrom + @"' AND '" + asssignDateTo + @"' ";
//    sql += @"
//                                GROUP BY staff.slm_UserName ,staff.slm_StaffNameTH,stafftype.slm_StaffTypeDesc,staff.slm_IsActive
//                                UNION ALL 
//                                --@@@@@@@@@@@@@@@@@@@@@@@@ หาจำนวนงานตาม TicketID ตาม Delegate(Marketing)@@@@@@@@@@@@@@@@@@@@@@@@@@@@
//                                SELECT COUNT(lead.slm_ticketid) as Amount,staff.slm_UserName AS Username,staff.slm_StaffNameTH AS FullnameTH,
//                                stafftype.slm_StaffTypeDesc As [ROLE],staff.slm_IsActive AS Active
//                                FROM kkslm_tr_lead lead inner join kkslm_ms_staff staff on staff.slm_UserName = lead.slm_Delegate  
//                                    INNER JOIN kkslm_ms_staff_type stafftype on stafftype .slm_StaffTypeId = staff.slm_StaffTypeId 
//                                where staff.slm_StaffTypeId = '" + SLMConstant.StaffType.Marketing + @"' AND LEAD.is_Deleted = 0 
//                                    AND LEAD.slm_Delegate IS NOT NULL AND LEAD.slm_Owner = '" + username + @"'  AND STAFF.slm_IsActive IN (" + active + @")  ";
//    if (assignDateFrom != "" && asssignDateTo != "")
//        sql += " AND LEAD.slm_AssignedDate BETWEEN '" + assignDateFrom + @"' AND '" + asssignDateTo + @"' ";
//    sql += @"
//                                GROUP BY staff.slm_UserName ,staff.slm_StaffNameTH,stafftype.slm_StaffTypeDesc,staff.slm_IsActive) AS Z
//                                GROUP BY Z.Username,Z.FullnameTH,Z.[ROLE],Z.Active) C
//                                GROUP BY C.Username,C.FullnameTH,C.ROLE,C.Active
                                 //                                ORDER BY SUM(C.AMOUNT) DESC, C.[ROLE] ASC, C.FullnameTH ASC ";
#endregion

 #region Version 21/3/2557  ก่อน CR
                                 //***********************************************************************************************************
            //Version 21/3/2557  ก่อน CR
//            string sql = @"  SELECT SUM(C.AMOUNT) AS AMOUNT,C.Username,C.FullnameTH,C.ROLE,C.CampaignName,C.Active
//                            FROM
//                            (
//	                            SELECT SUM(B.AMOUNT) AS AMOUNT,B.Username,B.FullnameTH,B.ROLE,B.CampaignName,B.Active
//	                            FROM
//	                            (
//	                            --//////////////////////////หาจำนวน ticket ในแต่ละ campaign ตาม Owner(TeleSale)//////////////////////////
//	                            SELECT COUNT(LEAD.slm_ticketId) AS AMOUNT, Z.SLM_OWNER AS Username,Z.FullnameTH,Z.[Role] AS [ROLE],Z.CampaignName,Z.Active,Z.slm_StaffTypeId 
//		                              FROM 
//			                            (
//			                            --****************************** หา Campaign ของ User Login*********************************
//			                            SELECT DISTINCT CAM1.*
//			                            FROM
//			                            (
//			                            SELECT [GROUP].SLM_CAMPAIGNID,staff.slm_UserName AS SLM_OWNER ,STAFF.slm_StaffNameTH  AS FullnameTH,
//			                            stafftype.slm_StaffTypeDesc AS [Role], campaign.slm_CampaignName AS CampaignName,staff.slm_IsActive AS Active,
//			                            STAFF.slm_StaffTypeId 
//			                            FROM " + SLMDBName + @".DBO.kkslm_ms_group [GROUP] INNER JOIN " + SLMDBName + @".DBO.kkslm_ms_staff_group GROUP2 ON GROUP2.slm_GroupId = [GROUP].slm_GroupId
//				                            INNER JOIN " + SLMDBName + @".DBO.kkslm_ms_staff staff on staff.slm_StaffId = group2.slm_StaffId
//				                            INNER JOIN " + SLMDBName + @".dbo.kkslm_ms_staff_type stafftype ON staff.slm_StaffTypeId = stafftype.slm_StaffTypeId
//				                            INNER JOIN " + SLMDBName + @".dbo.kkslm_ms_campaign campaign ON  [GROUP].SLM_CAMPAIGNID = campaign.slm_CampaignId
//			                            WHERE [GROUP].slm_GroupId IN (
//					                            SELECT slm_GroupId 
//					                            FROM " + SLMDBName + @".DBO.kkslm_ms_staff_group 
//					                            WHERE is_Deleted = 0 AND slm_StaffId IN (
//						                            SELECT slm_StaffId
//						                            FROM " + SLMDBName + @".DBO.kkslm_ms_staff staff 
//						                            WHERE staff.slm_UserName = '" + username + @"'))
//			                            UNION ALL
//			                            SELECT [GROUP].SLM_CAMPAIGNID,staff.slm_UserName AS SLM_OWNER ,STAFF.slm_StaffNameTH  AS FullnameTH,
//			                            stafftype.slm_StaffTypeDesc AS [Role], campaign.slm_CampaignName AS CampaignName,staff.slm_IsActive AS Active,
//			                            STAFF.slm_StaffTypeId 
//			                            FROM " + SLMDBName + @".DBO.kkslm_ms_group [GROUP] INNER JOIN " + SLMDBName + @".DBO.kkslm_ms_staff_group GROUP2 ON GROUP2.slm_GroupId = [GROUP].slm_GroupId
//				                            INNER JOIN " + SLMDBName + @".DBO.kkslm_ms_staff staff on staff.slm_StaffId = group2.slm_StaffId
//				                            INNER JOIN " + SLMDBName + @".dbo.kkslm_ms_staff_type stafftype ON staff.slm_StaffTypeId = stafftype.slm_StaffTypeId
//				                            INNER JOIN " + SLMDBName + @".dbo.kkslm_ms_campaign campaign ON  [GROUP].SLM_CAMPAIGNID = campaign.slm_CampaignId
//			                            WHERE [GROUP].slm_StaffId IN (
//					                            SELECT slm_StaffId
//					                            FROM " + SLMDBName + @".DBO.kkslm_ms_staff staff 
//					                            WHERE staff.slm_UserName = '" + username + @"')) AS CAM1
//					                    --******************************************************************************************
//			                             ) AS Z 
//			                             LEFT JOIN " + SLMDBName + @".dbo.kkslm_tr_lead lead ON Z.slm_CampaignId = LEAD.slm_CampaignId 
//			                             AND LEAD.slm_Owner = Z.SLM_OWNER AND lead.slm_Status NOT IN ('" + SLMConstant.StatusCode.Reject + @"','" + SLMConstant.StatusCode.Cancel + @"','" + SLMConstant.StatusCode.Close + @"') 
//			                             AND LEAD.is_Deleted = 0 
//			                             GROUP BY Z.SLM_OWNER ,Z.slm_CampaignId,Z.FullnameTH,Z.Role,Z.CampaignName,Z.Active,Z.slm_StaffTypeId      
//	                            UNION ALL
//	                            --====================หาจำนวนงานตาม TicketId โดยเอาเฉพาะที่ถูก Delegate(TeleSale)=================================
//	                            SELECT A.AMOUNT AS Amount,staff.slm_UserName AS Username, staff.slm_StaffNameTH AS FullnameTH,stafftype.slm_StaffTypeDesc AS Role, 
//			                            campaign.slm_CampaignName AS CampaignName, staff.slm_IsActive AS Active,staff.slm_StaffTypeId 
//	                            FROM 
//	                            (
//	                              SELECT COUNT(lead.slm_ticketId) AS AMOUNT, lead.slm_Delegate, LEAD.slm_CampaignId
//	                              FROM " + SLMDBName + @".dbo.kkslm_tr_lead lead 
//	                              WHERE  LEAD.slm_Owner = '" + username + @"' AND LEAD.is_Deleted = 0 
//                                        AND slm_Status NOT IN ('" + SLMConstant.StatusCode.Reject + @"','" + SLMConstant.StatusCode.Cancel + @"','" + SLMConstant.StatusCode.Close + @"')  
//                                        AND LEAD.slm_Delegate IS NOT NULL
//	                              GROUP BY lead.slm_Delegate, LEAD.slm_CampaignId
//	                              ) A INNER JOIN " + SLMDBName + @".dbo.kkslm_ms_staff staff ON A.slm_Delegate = staff.slm_UserName and staff.is_Deleted = 0
//	                            INNER JOIN " + SLMDBName + @".dbo.kkslm_ms_staff_type stafftype ON staff.slm_StaffTypeId = stafftype.slm_StaffTypeId
//	                            INNER JOIN " + SLMDBName + @".dbo.kkslm_ms_staff_group staffgroup ON staffgroup.slm_StaffId = staff.slm_StaffId
//	                            INNER JOIN " + SLMDBName + @".dbo.kkslm_ms_group groups ON groups.slm_GroupId = staffgroup.slm_GroupId
//	                            INNER JOIN " + SLMDBName + @".dbo.kkslm_ms_campaign campaign ON groups.slm_CampaignId = campaign.slm_CampaignId AND campaign.slm_CampaignId = A.slm_CampaignId
//	                            WHERE staffgroup.is_Deleted = 0  ) AS B
//	                            where b.slm_StaffTypeId IN ('" + SLMConstant.StaffType.Supervisor + @"','" + SLMConstant.StaffType.Telesales + @"')
//	                            GROUP BY B.Username,B.FullnameTH,B.ROLE,B.CampaignName,B.Active 
//	                            UNION ALL
//	                            --########################## หาจำนวนงานตาม TicketID ตาม Owner(Marketing)###############################
//	                            SELECT SUM(Z.Amount) AS AMOUNT,Z.Username,Z.FullnameTH,Z.[ROLE],Z.CampaignName,Z.Active
//	                            FROM
//	                            (
//	                            SELECT COUNT(lead.slm_ticketid) as Amount,staff.slm_UserName AS Username,staff.slm_StaffNameTH AS FullnameTH,
//	                            stafftype.slm_StaffTypeDesc As [ROLE],'' AS CampaignName,staff.slm_IsActive AS Active
//	                            FROM kkslm_tr_lead lead inner join kkslm_ms_staff staff on staff.slm_UserName = lead.slm_Owner 
//		                            INNER JOIN kkslm_ms_staff_type stafftype on stafftype .slm_StaffTypeId = staff.slm_StaffTypeId 
//	                            where staff.slm_StaffTypeId = '" + SLMConstant.StaffType.Marketing + @"' AND slm_Status NOT IN ('" + SLMConstant.StatusCode.Reject + @"','" + SLMConstant.StatusCode.Cancel + @"','" + SLMConstant.StatusCode.Close + @"') 
//                                    AND LEAD.is_Deleted = 0 
//		                            AND LEAD.slm_Owner = '" + username + @"'
//	                            GROUP BY staff.slm_UserName ,staff.slm_StaffNameTH,stafftype.slm_StaffTypeDesc,staff.slm_IsActive
//	                            UNION ALL 
//	                            --@@@@@@@@@@@@@@@@@@@@@@@@ หาจำนวนงานตาม TicketID ตาม Delegate(Marketing)@@@@@@@@@@@@@@@@@@@@@@@@@@@@
//	                            SELECT COUNT(lead.slm_ticketid) as Amount,staff.slm_UserName AS Username,staff.slm_StaffNameTH AS FullnameTH,
//	                            stafftype.slm_StaffTypeDesc As [ROLE],'' AS CampaignName,staff.slm_IsActive AS Active
//	                            FROM kkslm_tr_lead lead inner join kkslm_ms_staff staff on staff.slm_UserName = lead.slm_Delegate  
//		                            INNER JOIN kkslm_ms_staff_type stafftype on stafftype .slm_StaffTypeId = staff.slm_StaffTypeId 
//	                            where staff.slm_StaffTypeId = '" + SLMConstant.StaffType.Marketing + @"' AND slm_Status NOT IN ('" + SLMConstant.StatusCode.Reject + @"','" + SLMConstant.StatusCode.Cancel + @"','" + SLMConstant.StatusCode.Close + @"') 
//                                    AND LEAD.is_Deleted = 0 
//		                            AND LEAD.slm_Delegate IS NOT NULL AND LEAD.slm_Owner = '" + username + @"'
//	                            GROUP BY staff.slm_UserName ,staff.slm_StaffNameTH,stafftype.slm_StaffTypeDesc,staff.slm_IsActive) AS Z
//	                            GROUP BY Z.Username,Z.FullnameTH,Z.[ROLE],Z.CampaignName,Z.Active) C
//	                            GROUP BY C.Username,C.FullnameTH,C.ROLE,C.CampaignName,C.Active
                                 //	                            ORDER BY SUM(C.AMOUNT) DESC, C.[ROLE] ASC,C.CampaignName ASC, C.FullnameTH ASC ";
#endregion
            return slmdb.ExecuteStoreQuery<UserMonitoringData>(sql).ToList();
            #endregion
        }

        public List<UserMonitoringMKTData> SearchUserMonitoringMKT(string userList, string productId,string campaign, string branchcode, string active, string assignDateFrom, string asssignDateTo)
        {
            string whOwner = " AND lead.slm_Owner IN (" + userList + ")";
            string whDelegate = " AND lead.slm_Delegate IN (" + userList + ")";
            string whUserList = " WHERE staff.slm_UserName IN (" + userList + ")";
            string whProductId = "";
            string whCampaign = "";
            string whBranchCode = "";
            string whActive = "";
            string whAssignDate= "";

            if (productId != "0")
                whProductId = " AND lead.slm_Product_Id = '" + productId + "'";

            if (campaign != "")
                whCampaign = " AND lead.slm_CampaignId = '" + campaign + "'";

            if (branchcode != "")
                whBranchCode = " AND staff.slm_BranchCode = '" + branchcode + "'";

            if (active != "")
                whActive = " AND staff.slm_IsActive IN (" + active + ")";

            if (assignDateFrom != "" && asssignDateTo != "")
                whAssignDate = " AND CONVERT(DATE,LEAD.slm_AssignedDate) BETWEEN '" + assignDateFrom + "' AND '" + asssignDateTo + "'";


            string sql = @"   SELECT ST.slm_StaffTypeDesc AS RoleName ,staff.slm_UserName AS Username, staff.slm_StaffNameTH as FullnameTH ,
                                    staff.slm_IsActive AS Active, staff.slm_StaffTypeId,
                                    ISNULL(SUM(Z.STATUS_00_Owner),0) + ISNULL(SUM(Z.STATUS_00_Delegate),0) AS SUM_STATUS_00,  
	                                ISNULL(SUM(Z.STATUS_05_Owner),0) + ISNULL(SUM(Z.STATUS_05_Delegate),0) AS SUM_STATUS_05,
	                                ISNULL(SUM(Z.STATUS_06_Owner),0) + ISNULL(SUM(Z.STATUS_06_Delegate),0) AS SUM_STATUS_06,
	                                ISNULL(SUM(Z.STATUS_07_Owner),0) + ISNULL(SUM(Z.STATUS_07_Delegate),0) AS SUM_STATUS_07 ,
	                                ISNULL(SUM(Z.STATUS_11_Owner),0) + ISNULL(SUM(Z.STATUS_11_Delegate),0) AS SUM_STATUS_11 ,
	                                ISNULL(SUM(Z.STATUS_14_Owner),0) + ISNULL(SUM(Z.STATUS_14_Delegate),0) AS SUM_STATUS_14 ,
	                                ISNULL(SUM(Z.STATUS_15_Owner),0) + ISNULL(SUM(Z.STATUS_15_Delegate),0) AS SUM_STATUS_15 ,
	                                ISNULL(SUM(Z.STATUS_00_Owner),0) + ISNULL(SUM(Z.STATUS_00_Delegate),0) +
	                                ISNULL(SUM(Z.STATUS_05_Owner),0) + ISNULL(SUM(Z.STATUS_05_Delegate),0) +
                                    ISNULL(SUM(Z.STATUS_06_Owner),0) + ISNULL(SUM(Z.STATUS_06_Delegate),0) +
	                                ISNULL(SUM(Z.STATUS_07_Owner),0) + ISNULL(SUM(Z.STATUS_07_Delegate),0) +
	                                ISNULL(SUM(Z.STATUS_11_Owner),0) + ISNULL(SUM(Z.STATUS_11_Delegate),0) + 
	                                ISNULL(SUM(Z.STATUS_14_Owner),0) + ISNULL(SUM(Z.STATUS_14_Delegate),0) +
	                                ISNULL(SUM(Z.STATUS_15_Owner),0) + ISNULL(SUM(Z.STATUS_15_Delegate),0) AS SUM_TOTAL
                            FROM kkslm_ms_staff staff  INNER JOIN SLMDB.DBO.kkslm_ms_staff_type ST ON ST.slm_StaffTypeId = STAFF.slm_StaffTypeId
								LEFT JOIN (
		                            --*********************************** สนใจ ***********************************
		                            -- คำนวนงานที่สนใจ และเฉพาะที่ตัวเองเป็น Owner
		                            SELECT ST.slm_StaffTypeDesc AS ROLE_NAME,staff.slm_UserName,staff.slm_StaffTypeId as StaffType, 
				                            staff.slm_StaffNameTH as OwnerName,staff.slm_IsActive,
				                            COUNT(LEAD.slm_ticketId) AS STATUS_00_Owner,0 AS STATUS_00_Delegate,  
				                            0 AS STATUS_05_Owner, 0 AS STATUS_05_Delegate, 
				                            0 AS STATUS_06_Owner, 0 AS STATUS_06_Delegate,
				                            0 AS STATUS_07_Owner, 0 AS STATUS_07_Delegate,
				                            0 AS STATUS_11_Owner, 0 AS STATUS_11_Delegate,
				                            0 AS STATUS_14_Owner, 0 AS STATUS_14_Delegate,
				                            0 AS STATUS_15_Owner, 0 AS STATUS_15_Delegate  
		                            FROM kkslm_tr_lead LEAD INNER JOIN kkslm_ms_option OP ON OP.slm_OptionCode = LEAD.slm_Status AND OP.slm_OptionType = 'lead status'
			                            INNER JOIN kkslm_ms_staff staff on staff.slm_UserName = lead.slm_Owner
			                            INNER JOIN kkslm_ms_staff_type ST ON ST.slm_StaffTypeId = staff.slm_StaffTypeId
		                            WHERE LEAD.slm_Status ='00' AND LEAD.is_Deleted = 0 AND slm_AssignedFlag = '1' 
                                             {0} {3} {4} {5} {6} {7}  
		                            GROUP BY ST.slm_StaffTypeDesc,staff.slm_UserName, staff.slm_StaffNameTH ,staff.slm_IsActive,staff.slm_StaffTypeId
		                            UNION ALL
		                            -- คำนวนงานที่สนใจ และเฉพาะที่ตัวเองเป็น Delegate
		                            SELECT ST.slm_StaffTypeDesc AS ROLE_NAME,staff.slm_UserName,staff.slm_StaffTypeId as StaffType,  
				                            staff.slm_StaffNameTH as OwnerName,staff.slm_IsActive,
				                            0 AS STATUS_00_Owner,COUNT(LEAD.slm_ticketId) AS STATUS_00_Delegate, 
				                            0 AS STATUS_05_Owner, 0 AS STATUS_05_Delegate, 
				                            0 AS STATUS_06_Owner, 0 AS STATUS_06_Delegate,
				                            0 AS STATUS_07_Owner, 0 AS STATUS_07_Delegate,
				                            0 AS STATUS_11_Owner, 0 AS STATUS_11_Delegate,
				                            0 AS STATUS_14_Owner, 0 AS STATUS_14_Delegate,
				                            0 AS STATUS_15_Owner, 0 AS STATUS_15_Delegate  
		                            FROM kkslm_tr_lead LEAD INNER JOIN kkslm_ms_option OP ON OP.slm_OptionCode = LEAD.slm_Status AND OP.slm_OptionType = 'lead status'
			                            INNER JOIN kkslm_ms_staff staff on staff.slm_UserName = lead.slm_Delegate
			                            INNER JOIN kkslm_ms_staff_type ST ON ST.slm_StaffTypeId = staff.slm_StaffTypeId
		                            WHERE LEAD.slm_Status ='00' AND LEAD.is_Deleted = 0 AND  slm_Delegate_Flag = '0' AND LEAD.slm_Delegate IS NOT NULL
                                             {1} {3} {4} {5} {6} {7}  
		                            GROUP BY ST.slm_StaffTypeDesc,staff.slm_UserName, staff.slm_StaffNameTH ,staff.slm_IsActive,staff.slm_StaffTypeId

		                            --*********************************** รอผลการพิจารณา  ***********************************
		                            UNION ALL
		                            -- คำนวนงานที่สนใจ และเฉพาะที่ตัวเองเป็น Owner
		                            SELECT ST.slm_StaffTypeDesc AS ROLE_NAME,staff.slm_UserName, staff.slm_StaffTypeId as StaffType, 
				                            staff.slm_StaffNameTH as OwnerName,staff.slm_IsActive,
				                            0 AS STATUS_00_Owner,0 AS STATUS_00_Delegate, 
				                            COUNT(LEAD.slm_ticketId) AS STATUS_05_Owner, 0 AS STATUS_05_Delegate,
				                            0 AS STATUS_06_Owner, 0 AS STATUS_06_Delegate,
				                            0 AS STATUS_07_Owner, 0 AS STATUS_07_Delegate,
				                            0 AS STATUS_11_Owner, 0 AS STATUS_11_Delegate,
				                            0 AS STATUS_14_Owner, 0 AS STATUS_14_Delegate,
				                            0 AS STATUS_15_Owner, 0 AS STATUS_15_Delegate 
		                            FROM kkslm_tr_lead LEAD INNER JOIN kkslm_ms_option OP ON OP.slm_OptionCode = LEAD.slm_Status AND OP.slm_OptionType = 'lead status'
			                            INNER JOIN kkslm_ms_staff staff on staff.slm_UserName = lead.slm_Owner
			                            INNER JOIN kkslm_ms_staff_type ST ON ST.slm_StaffTypeId = staff.slm_StaffTypeId
		                            WHERE LEAD.slm_Status ='05' AND LEAD.is_Deleted = 0 AND slm_AssignedFlag = '1'
                                             {0} {3} {4} {5} {6} {7}  
		                            GROUP BY ST.slm_StaffTypeDesc,staff.slm_UserName, staff.slm_StaffNameTH ,staff.slm_IsActive,staff.slm_StaffTypeId 
		                            UNION ALL
		                            -- คำนวนงานที่สนใจ และเฉพาะที่ตัวเองเป็น Delegate
		                            SELECT ST.slm_StaffTypeDesc AS ROLE_NAME,staff.slm_UserName, staff.slm_StaffTypeId as StaffType, 
				                            staff.slm_StaffNameTH as OwnerName,staff.slm_IsActive,
				                            0 AS STATUS_00_Owner,0 AS STATUS_00_Delegate, 
				                            0 AS STATUS_05_Owner, COUNT(LEAD.slm_ticketId) AS STATUS_05_Delegate, 
				                            0 AS STATUS_06_Owner, 0 AS STATUS_06_Delegate,
				                            0 AS STATUS_07_Owner, 0 AS STATUS_07_Delegate,
				                            0 AS STATUS_11_Owner, 0 AS STATUS_11_Delegate,
				                            0 AS STATUS_14_Owner, 0 AS STATUS_14_Delegate,
				                            0 AS STATUS_15_Owner, 0 AS STATUS_15_Delegate 
		                            FROM kkslm_tr_lead LEAD INNER JOIN kkslm_ms_option OP ON OP.slm_OptionCode = LEAD.slm_Status AND OP.slm_OptionType = 'lead status'
			                            INNER JOIN kkslm_ms_staff staff on staff.slm_UserName = lead.slm_Delegate
			                            INNER JOIN kkslm_ms_staff_type ST ON ST.slm_StaffTypeId = staff.slm_StaffTypeId
		                            WHERE LEAD.slm_Status ='05' AND LEAD.is_Deleted = 0 AND slm_Delegate_Flag = '0' AND LEAD.slm_Delegate IS NOT NULL
                                             {1} {3} {4} {5} {6} {7}  
		                            GROUP BY ST.slm_StaffTypeDesc,staff.slm_UserName, staff.slm_StaffNameTH ,staff.slm_IsActive,staff.slm_StaffTypeId
		
		                            --*********************************** อนุมัติ - ตามเสนอ  ***********************************
		                            UNION ALL
		                            -- คำนวนงานที่สนใจ และเฉพาะที่ตัวเองเป็น Owner
		                            SELECT ST.slm_StaffTypeDesc AS ROLE_NAME,staff.slm_UserName, staff.slm_StaffTypeId as StaffType, 
				                            staff.slm_StaffNameTH as OwnerName,staff.slm_IsActive,
				                            0 AS STATUS_00_Owner,0 AS STATUS_00_Delegate,
				                            0 AS STATUS_05_Owner, 0 AS STATUS_05_Delegate, 
				                            COUNT(LEAD.slm_ticketId) AS STATUS_06_Owner,0 AS STATUS_06_Delegate,
				                            0 AS STATUS_07_Owner, 0 AS STATUS_07_Delegate,
				                            0 AS STATUS_11_Owner, 0 AS STATUS_11_Delegate,
				                            0 AS STATUS_14_Owner, 0 AS STATUS_14_Delegate,
				                            0 AS STATUS_15_Owner, 0 AS STATUS_15_Delegate 
		                            FROM kkslm_tr_lead LEAD INNER JOIN kkslm_ms_option OP ON OP.slm_OptionCode = LEAD.slm_Status AND OP.slm_OptionType = 'lead status'
			                            INNER JOIN kkslm_ms_staff staff on staff.slm_UserName = lead.slm_Owner
			                            INNER JOIN kkslm_ms_staff_type ST ON ST.slm_StaffTypeId = staff.slm_StaffTypeId
		                            WHERE LEAD.slm_Status ='06' AND LEAD.is_Deleted = 0 AND slm_AssignedFlag = '1' 
                                             {0} {3} {4} {5} {6} {7}  
		                            GROUP BY ST.slm_StaffTypeDesc,staff.slm_UserName, staff.slm_StaffNameTH ,staff.slm_IsActive,staff.slm_StaffTypeId
		                            UNION ALL
		                            -- คำนวนงานที่สนใจ และเฉพาะที่ตัวเองเป็น Delegate
		                            SELECT ST.slm_StaffTypeDesc AS ROLE_NAME,staff.slm_UserName, staff.slm_StaffTypeId as StaffType, 
				                            staff.slm_StaffNameTH as OwnerName,staff.slm_IsActive,
				                            0 AS STATUS_00_Owner, 0 AS STATUS_00_Delegate,
				                            0 AS STATUS_05_Owner, 0 AS STATUS_05_Delegate, 
				                            0 AS STATUS_06_Owner, COUNT(LEAD.slm_ticketId) AS STATUS_06_Delegate,
				                            0 AS STATUS_07_Owner, 0 AS STATUS_07_Delegate,
				                            0 AS STATUS_11_Owner, 0 AS STATUS_11_Delegate,
				                            0 AS STATUS_14_Owner, 0 AS STATUS_14_Delegate,
				                            0 AS STATUS_15_Owner, 0 AS STATUS_15_Delegate 
		                            FROM kkslm_tr_lead LEAD INNER JOIN kkslm_ms_option OP ON OP.slm_OptionCode = LEAD.slm_Status AND OP.slm_OptionType = 'lead status'
			                            INNER JOIN kkslm_ms_staff staff on staff.slm_UserName = lead.slm_Delegate
			                            INNER JOIN kkslm_ms_staff_type ST ON ST.slm_StaffTypeId = staff.slm_StaffTypeId
		                            WHERE LEAD.slm_Status ='06' AND LEAD.is_Deleted = 0 AND slm_Delegate_Flag = '0' AND LEAD.slm_Delegate IS NOT NULL
                                             {1} {3} {4} {5} {6} {7}  
		                            GROUP BY ST.slm_StaffTypeDesc,staff.slm_UserName, staff.slm_StaffNameTH ,staff.slm_IsActive,staff.slm_StaffTypeId
		
		                           
                                      --*********************************** ส่งกลับแก้ไข ***********************************
		                            UNION ALL
                                    -- คำนวนงานที่สนใจ และเฉพาะที่ตัวเองเป็น Owner
		                            SELECT ST.slm_StaffTypeDesc AS ROLE_NAME,staff.slm_UserName,staff.slm_StaffTypeId as StaffType,  
				                            staff.slm_StaffNameTH as OwnerName,staff.slm_IsActive,
				                            0 AS STATUS_00_Owner,0 AS STATUS_00_Delegate,
				                            0 AS STATUS_05_Owner, 0 AS STATUS_05_Delegate, 
				                            0 AS STATUS_06_Owner, 0 AS STATUS_06_Delegate,
				                            0 AS STATUS_07_Owner, 0 AS STATUS_07_Delegate,
				                            COUNT(LEAD.slm_ticketId) AS STATUS_11_Owner,0 AS STATUS_11_Delegate,
				                            0 AS STATUS_14_Owner, 0 AS STATUS_14_Delegate,
				                            0 AS STATUS_15_Owner, 0 AS STATUS_15_Delegate 
		                            FROM kkslm_tr_lead LEAD INNER JOIN kkslm_ms_option OP ON OP.slm_OptionCode = LEAD.slm_Status AND OP.slm_OptionType = 'lead status'
			                            INNER JOIN kkslm_ms_staff staff on staff.slm_UserName = lead.slm_Owner
			                            INNER JOIN kkslm_ms_staff_type ST ON ST.slm_StaffTypeId = staff.slm_StaffTypeId
		                            WHERE LEAD.slm_Status ='11' AND LEAD.is_Deleted = 0 AND slm_AssignedFlag = '1'
                                              {0} {3} {4} {5} {6} {7}   
		                            GROUP BY ST.slm_StaffTypeDesc,staff.slm_UserName, staff.slm_StaffNameTH ,staff.slm_IsActive,staff.slm_StaffTypeId
		                            UNION ALL
		                            -- คำนวนงานที่สนใจ และเฉพาะที่ตัวเองเป็น Delegate
		                            SELECT ST.slm_StaffTypeDesc AS ROLE_NAME,staff.slm_UserName,staff.slm_StaffTypeId as StaffType,  
				                            staff.slm_StaffNameTH as OwnerName,staff.slm_IsActive,
				                            0 AS STATUS_00_Owner, 0 AS STATUS_00_Delegate, 
				                            0 AS STATUS_05_Owner, 0 AS STATUS_05_Delegate, 
				                            0 AS STATUS_06_Owner, 0 AS STATUS_06_Delegate,
				                            0 AS STATUS_07_Owner, 0 AS STATUS_07_Delegate,
				                            0 AS STATUS_11_Owner, COUNT(LEAD.slm_ticketId) AS STATUS_11_Delegate,
				                            0 AS STATUS_14_Owner, 0 AS STATUS_14_Delegate,
				                            0 AS STATUS_15_Owner, 0 AS STATUS_15_Delegate 
		                            FROM kkslm_tr_lead LEAD INNER JOIN kkslm_ms_option OP ON OP.slm_OptionCode = LEAD.slm_Status AND OP.slm_OptionType = 'lead status'
			                            INNER JOIN kkslm_ms_staff staff on staff.slm_UserName = lead.slm_Delegate
			                            INNER JOIN kkslm_ms_staff_type ST ON ST.slm_StaffTypeId = staff.slm_StaffTypeId
		                            WHERE LEAD.slm_Status ='11' AND LEAD.is_Deleted = 0 AND slm_Delegate_Flag = '0' AND LEAD.slm_Delegate IS NOT NULL
                                              {1} {3} {4} {5} {6} {7}  
		                            GROUP BY ST.slm_StaffTypeDesc,staff.slm_UserName, staff.slm_StaffNameTH ,staff.slm_IsActive,staff.slm_StaffTypeId
		                            
		                            --*********************************** อยู่ระหว่างดำเนินการ ***********************************
		                            UNION ALL
                                    -- คำนวนงานที่สนใจ และเฉพาะที่ตัวเองเป็น Owner
		                            SELECT ST.slm_StaffTypeDesc AS ROLE_NAME,staff.slm_UserName,staff.slm_StaffTypeId as StaffType,  
				                            staff.slm_StaffNameTH as OwnerName,staff.slm_IsActive,
				                            0 AS STATUS_00_Owner,0 AS STATUS_00_Delegate,
				                            0 AS STATUS_05_Owner, 0 AS STATUS_05_Delegate, 
				                            0 AS STATUS_06_Owner, 0 AS STATUS_06_Delegate,
				                            0 AS STATUS_07_Owner, 0 AS STATUS_07_Delegate,
				                            0 AS STATUS_11_Owner, 0 AS STATUS_11_Delegate,
				                            COUNT(LEAD.slm_ticketId) AS STATUS_14_Owner,0 AS STATUS_14_Delegate,
				                            0 AS STATUS_15_Owner, 0 AS STATUS_15_Delegate 
		                            FROM kkslm_tr_lead LEAD INNER JOIN kkslm_ms_option OP ON OP.slm_OptionCode = LEAD.slm_Status AND OP.slm_OptionType = 'lead status'
			                            INNER JOIN kkslm_ms_staff staff on staff.slm_UserName = lead.slm_Owner
			                            INNER JOIN kkslm_ms_staff_type ST ON ST.slm_StaffTypeId = staff.slm_StaffTypeId
		                            WHERE LEAD.slm_Status ='14' AND LEAD.is_Deleted = 0 AND slm_AssignedFlag = '1' 
                                              {0} {3} {4} {5} {6} {7}  
		                            GROUP BY ST.slm_StaffTypeDesc,staff.slm_UserName, staff.slm_StaffNameTH ,staff.slm_IsActive,staff.slm_StaffTypeId
		                            UNION ALL
		                            -- คำนวนงานที่สนใจ และเฉพาะที่ตัวเองเป็น Delegate
		                            SELECT ST.slm_StaffTypeDesc AS ROLE_NAME,staff.slm_UserName,staff.slm_StaffTypeId as StaffType,  
				                            staff.slm_StaffNameTH as OwnerName,staff.slm_IsActive,
				                            0 AS STATUS_00_Owner, 0 AS STATUS_00_Delegate,  
				                            0 AS STATUS_05_Owner, 0 AS STATUS_05_Delegate, 
				                            0 AS STATUS_06_Owner, 0 AS STATUS_06_Delegate,
				                            0 AS STATUS_07_Owner, 0 AS STATUS_07_Delegate,
				                            0 AS STATUS_11_Owner, 0 AS STATUS_11_Delegate,
				                            0 AS STATUS_14_Owner, COUNT(LEAD.slm_ticketId) AS STATUS_14_Delegate,
                                            0 AS STATUS_15_Owner, 0 AS STATUS_15_Delegate  
		                            FROM kkslm_tr_lead LEAD INNER JOIN kkslm_ms_option OP ON OP.slm_OptionCode = LEAD.slm_Status AND OP.slm_OptionType = 'lead status'
			                            INNER JOIN kkslm_ms_staff staff on staff.slm_UserName = lead.slm_Delegate
			                            INNER JOIN kkslm_ms_staff_type ST ON ST.slm_StaffTypeId = staff.slm_StaffTypeId
		                            WHERE LEAD.slm_Status ='14' AND LEAD.is_Deleted = 0 AND slm_Delegate_Flag = '0' AND LEAD.slm_Delegate IS NOT NULL
                                              {1} {3} {4} {5} {6} {7}  
		                            GROUP BY ST.slm_StaffTypeDesc,staff.slm_UserName, staff.slm_StaffNameTH ,staff.slm_IsActive,staff.slm_StaffTypeId
		                            
		                            --*********************************** อยู่ระหว่างดำเนินการ ***********************************
		                            UNION ALL
                                    -- คำนวนงานที่สนใจ และเฉพาะที่ตัวเองเป็น Owner
		                            SELECT ST.slm_StaffTypeDesc AS ROLE_NAME,staff.slm_UserName,staff.slm_StaffTypeId as StaffType,  
				                            staff.slm_StaffNameTH as OwnerName,staff.slm_IsActive,
				                            0 AS STATUS_00_Owner,0 AS STATUS_00_Delegate,
				                            0 AS STATUS_05_Owner, 0 AS STATUS_05_Delegate, 
				                            0 AS STATUS_06_Owner, 0 AS STATUS_06_Delegate,
				                            0 AS STATUS_07_Owner, 0 AS STATUS_07_Delegate,
				                            0 AS STATUS_11_Owner, 0 AS STATUS_11_Delegate,
				                            0 AS STATUS_14_Owner, 0 AS STATUS_14_Delegate,
				                            COUNT(LEAD.slm_ticketId) AS STATUS_15_Owner,0 AS STATUS_15_Delegate 
		                            FROM kkslm_tr_lead LEAD INNER JOIN kkslm_ms_option OP ON OP.slm_OptionCode = LEAD.slm_Status AND OP.slm_OptionType = 'lead status'
			                            INNER JOIN kkslm_ms_staff staff on staff.slm_UserName = lead.slm_Owner
			                            INNER JOIN kkslm_ms_staff_type ST ON ST.slm_StaffTypeId = staff.slm_StaffTypeId
		                            WHERE LEAD.slm_Status ='15' AND LEAD.is_Deleted = 0 AND slm_AssignedFlag = '1' 
                                              {0} {3} {4} {5} {6} {7} 
		                            GROUP BY ST.slm_StaffTypeDesc,staff.slm_UserName, staff.slm_StaffNameTH ,staff.slm_IsActive,staff.slm_StaffTypeId
		                            UNION ALL
		                            -- คำนวนงานที่สนใจ และเฉพาะที่ตัวเองเป็น Delegate
		                            SELECT ST.slm_StaffTypeDesc AS ROLE_NAME,staff.slm_UserName,staff.slm_StaffTypeId as StaffType,  
				                            staff.slm_StaffNameTH as OwnerName,staff.slm_IsActive,
				                            0 AS STATUS_00_Owner, 0 AS STATUS_00_Delegate,  
				                            0 AS STATUS_05_Owner, 0 AS STATUS_05_Delegate, 
				                            0 AS STATUS_06_Owner, 0 AS STATUS_06_Delegate,
				                            0 AS STATUS_07_Owner, 0 AS STATUS_07_Delegate,
				                            0 AS STATUS_11_Owner, 0 AS STATUS_11_Delegate,
				                            0 AS STATUS_14_Owner, 0 AS STATUS_14_Delegate,
				                            0 AS STATUS_15_Owner, COUNT(LEAD.slm_ticketId) AS STATUS_15_Delegate 
		                            FROM kkslm_tr_lead LEAD INNER JOIN kkslm_ms_option OP ON OP.slm_OptionCode = LEAD.slm_Status AND OP.slm_OptionType = 'lead status'
			                            INNER JOIN kkslm_ms_staff staff on staff.slm_UserName = lead.slm_Delegate
			                            INNER JOIN kkslm_ms_staff_type ST ON ST.slm_StaffTypeId = staff.slm_StaffTypeId
		                            WHERE LEAD.slm_Status ='15' AND LEAD.is_Deleted = 0 AND slm_Delegate_Flag = '0' AND LEAD.slm_Delegate IS NOT NULL
                                              {1} {3} {4} {5} {6} {7} 
		                            GROUP BY ST.slm_StaffTypeDesc,staff.slm_UserName, staff.slm_StaffNameTH ,staff.slm_IsActive,staff.slm_StaffTypeId
	                            ) AS Z on z.slm_UserName = staff.slm_UserName
	                         {2} {5} {6} 
                            GROUP BY ST.slm_StaffTypeDesc ,staff.slm_UserName , staff.slm_StaffNameTH ,
                                    staff.slm_IsActive , staff.slm_StaffTypeId
                            ORDER BY ISNULL(SUM(Z.STATUS_00_Owner),0) + ISNULL(SUM(Z.STATUS_00_Delegate),0) +
	                                ISNULL(SUM(Z.STATUS_05_Owner),0) + ISNULL(SUM(Z.STATUS_05_Delegate),0) +
                                    ISNULL(SUM(Z.STATUS_06_Owner),0) + ISNULL(SUM(Z.STATUS_06_Delegate),0) +
	                                ISNULL(SUM(Z.STATUS_07_Owner),0) + ISNULL(SUM(Z.STATUS_07_Delegate),0) +
	                                ISNULL(SUM(Z.STATUS_11_Owner),0) + ISNULL(SUM(Z.STATUS_11_Delegate),0) + 
	                                ISNULL(SUM(Z.STATUS_14_Owner),0) + ISNULL(SUM(Z.STATUS_14_Delegate),0) +
	                                ISNULL(SUM(Z.STATUS_15_Owner),0) + ISNULL(SUM(Z.STATUS_15_Delegate),0) DESC,
	                                st.slm_StaffTypeDesc ASC,staff.slm_StaffNameTH ASC ";

            sql = string.Format(sql, whOwner, whDelegate, whUserList, whProductId, whCampaign, whBranchCode, whActive, whAssignDate);
            return slmdb.ExecuteStoreQuery<UserMonitoringMKTData>(sql).ToList();
        }

        public List<SearchLeadResult> GetUserMonitoringMKTListByUser(string productId, string campaign, string branchcode, 
            string assignDateFrom, string asssignDateTo,string username,string statuscode)
        {
            string whOwner = " AND lead.slm_Owner IN (" + username + ")";
            string whDelegate = " AND lead.slm_Delegate IN (" + username + ")";
            string whProductId = "";
            string whCampaign = "";
            string whBranchCode = "";
            string whAssignDate = "";
            string whStatuscode = "";

            if (productId != "0")
                whProductId = " AND lead.slm_Product_Id = '" + productId + "'";

            if (campaign != "")
                whCampaign = " AND lead.slm_CampaignId = '" + campaign + "'";

            if (branchcode != "")
                whBranchCode = " AND staff.slm_BranchCode = '" + branchcode + "'";

            if (assignDateFrom != "" && asssignDateTo != "")
                whAssignDate = " AND CONVERT(DATE,LEAD.slm_AssignedDate) BETWEEN '" + assignDateFrom + "' AND '" + asssignDateTo + "'";
            string sql = "";

            if (statuscode == "ALL")
                whStatuscode = @" AND LEAD.slm_Status IN  ( '" + SLMConstant.StatusCode.Interest + "', '" + SLMConstant.StatusCode.WaitConsider + "','" + SLMConstant.StatusCode.ApproveAccept + "','" + SLMConstant.StatusCode.ApproveEdit + "','" + SLMConstant.StatusCode.RoutebackEdit + "','" + SLMConstant.StatusCode.OnProcess + "','" + SLMConstant.StatusCode.WaitContact + "')";
            else if (statuscode == SLMConstant.StatusCode.Interest)
                whStatuscode = @" AND LEAD.slm_Status = '" + SLMConstant.StatusCode.Interest + "'";
            else if (statuscode == SLMConstant.StatusCode.WaitConsider)
                whStatuscode = @" AND LEAD.slm_Status = '" + SLMConstant.StatusCode.WaitConsider + "'";
            else if (statuscode == SLMConstant.StatusCode.ApproveAccept)
                whStatuscode = @" AND LEAD.slm_Status = '" + SLMConstant.StatusCode.ApproveAccept + "'";
            else if (statuscode == SLMConstant.StatusCode.ApproveEdit)
                whStatuscode = @" AND LEAD.slm_Status = '" + SLMConstant.StatusCode.ApproveEdit + "'";
            else if (statuscode == SLMConstant.StatusCode.RoutebackEdit)
                whStatuscode = @" AND LEAD.slm_Status = '" + SLMConstant.StatusCode.RoutebackEdit + "'";
            else if (statuscode == SLMConstant.StatusCode.OnProcess)
                whStatuscode = @" AND LEAD.slm_Status = '" + SLMConstant.StatusCode.OnProcess + "'";
            else if (statuscode == SLMConstant.StatusCode.WaitContact)
                whStatuscode = @" AND LEAD.slm_Status = '" + SLMConstant.StatusCode.WaitContact + "'";

                sql = @"   
                        SELECT CONVERT(int, ROW_NUMBER() OVER(ORDER BY Z.slm_CreatedDate desc)) AS SEQ,
	                        Z.TICKETID,Z.CitizenId,Z.Firstname,Z.Lastname,
                                Z.StatusDesc, Z.ChannelDesc,Z.OwnerName,
                                Z.CreatedDate, Z.AssignedDate,Z.DelegateName,
                                Z.CampaignName  ,Z.TranferType ,Z.CampaignId,
                                Z.CardTypeDesc, Z.Counting, Z.HasAdamUrl
                        FROM(
                        SELECT lead.slm_CreatedDate,LEAD.slm_ticketId AS TICKETID,CUS.slm_CitizenId AS CitizenId,LEAD.slm_Name AS Firstname,LEAD.slm_LastName AS Lastname,
                                OP.slm_OptionDesc AS StatusDesc,CHANNEL.slm_ChannelDesc AS ChannelDesc,STAFF.slm_StaffNameTH AS OwnerName,
                                LEAD.slm_CreatedDate AS  CreatedDate, LEAD.slm_AssignedDate AS AssignedDate,DELEGATE.slm_StaffNameTH AS DelegateName,
                                cam.slm_CampaignName AS CampaignName  ,'Owner Lead' as TranferType ,LEAD.slm_CampaignId AS CampaignId,
                                 ct.slm_CardTypeName AS CardTypeDesc, LEAD.slm_Counting AS Counting, ISNULL(MP.HasADAMUrl, 0) AS HasAdamUrl
                        FROM kkslm_tr_lead LEAD INNER JOIN kkslm_ms_option OP ON OP.slm_OptionCode = LEAD.slm_Status AND OP.slm_OptionType = 'lead status'
                             INNER JOIN " + SLMDBName + @".dbo.kkslm_ms_campaign cam on CAM.slm_CampaignId = LEAD.slm_CampaignId 
	                        INNER JOIN " + SLMDBName + @".dbo.kkslm_ms_staff staff on staff.slm_UserName = lead.slm_Owner
                            INNER JOIN " + SLMDBName + @".dbo.kkslm_ms_staff_type ST ON ST.slm_StaffTypeId = staff.slm_StaffTypeId
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_tr_cusinfo CUS ON CUS.slm_TicketId = LEAD.slm_ticketId 
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_channel CHANNEL ON CHANNEL.slm_ChannelId = LEAD.slm_ChannelId 
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff DELEGATE ON DELEGATE.slm_UserName = LEAD.slm_Delegate  
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_cardtype ct ON cus.slm_CardType = ct.slm_CardTypeId
                            LEFT JOIN " + SLMDBName + @".dbo.CMT_MAPPING_PRODUCT MP ON lead.slm_Product_Id = MP.sub_product_id
                        WHERE LEAD.is_Deleted = 0 AND slm_AssignedFlag = '1' 
                                {0}
		                        {1}
                                {3} {4} {5} {6} 
                        UNION ALL
                        -- คำนวนงานที่สนใจ และเฉพาะที่ตัวเองเป็น Delegate
                        SELECT lead.slm_CreatedDate,LEAD.slm_ticketId AS TicketId,CUS.slm_CitizenId AS CitizenId,LEAD.slm_Name AS Firstname,LEAD.slm_LastName AS Lastname,
                                OP.slm_OptionDesc AS StatusDesc,CHANNEL.slm_ChannelDesc AS ChannelDesc,STAFF.slm_StaffNameTH AS OwnerName,
                                LEAD.slm_CreatedDate AS  CreatedDate, LEAD.slm_AssignedDate AS AssignedDate,DELEGATE.slm_StaffNameTH AS DelegateName,
                                cam.slm_CampaignName AS CampaignName  ,'Delegate Lead' as TranferType ,LEAD.slm_CampaignId AS CampaignId,
                                 ct.slm_CardTypeName AS CardTypeDesc, LEAD.slm_Counting AS Counting, ISNULL(MP.HasADAMUrl, 0) AS HasAdamUrl
                        FROM kkslm_tr_lead LEAD INNER JOIN kkslm_ms_option OP ON OP.slm_OptionCode = LEAD.slm_Status AND OP.slm_OptionType = 'lead status'
                            INNER JOIN  " + SLMDBName + @".dbo.kkslm_ms_campaign cam on CAM.slm_CampaignId = LEAD.slm_CampaignId 
                            INNER JOIN " + SLMDBName + @".dbo.kkslm_ms_staff staff on staff.slm_UserName = lead.slm_Owner
                            INNER JOIN " + SLMDBName + @".dbo.kkslm_ms_staff_type ST ON ST.slm_StaffTypeId = staff.slm_StaffTypeId
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_tr_cusinfo CUS ON CUS.slm_TicketId = LEAD.slm_ticketId 
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_channel CHANNEL ON CHANNEL.slm_ChannelId = LEAD.slm_ChannelId 
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff DELEGATE ON DELEGATE.slm_UserName = LEAD.slm_Delegate  
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_cardtype ct ON cus.slm_CardType = ct.slm_CardTypeId
                            LEFT JOIN " + SLMDBName + @".dbo.CMT_MAPPING_PRODUCT MP ON lead.slm_Product_Id = MP.sub_product_id
                        WHERE LEAD.is_Deleted = 0 AND  slm_Delegate_Flag = '0' AND LEAD.slm_Delegate IS NOT NULL 
                                {0} 
		                        {2}  
                                {3} {4} {5} {6}  ) AS Z  
 ";

                sql = string.Format(sql,whStatuscode, whOwner, whDelegate, whProductId, whCampaign, whBranchCode, whAssignDate);
                return slmdb.ExecuteStoreQuery<SearchLeadResult>(sql).ToList();
        }
       
        //Reference : SlmScr004Biz
        public List<CampaignWSData> GetCampaignFinalList(string ticketId)
        {
            string sql = @"SELECT FINAL.slm_CampaignId AS CampaignId, FINAL.slm_CampaignName AS CampaignName, CAM.slm_Offer + ' : ' + CAM.slm_Criteria AS CampaignDetail,
	                            FINAL.slm_CreatedDate AS CreatedDate,
                                CASE WHEN FINAL.slm_CreatedBy  = 'SYSTEM' THEN FINAL.slm_CreatedBy 
		                             WHEN pos.slm_PositionNameAbb IS NULL THEN STAFF.slm_StaffNameTH
		                             ELSE pos.slm_PositionNameAbb + ' - ' + STAFF.slm_StaffNameTH END CreatedByName,
	                            FINAL.slm_CampaignFinalId AS CampaignFinalId, FINAL.slm_TicketId
                            FROM " + SLMDBName + @".[dbo].kkslm_tr_campaignfinal AS FINAL
	                        LEFT JOIN " + SLMDBName + @".[dbo].[kkslm_ms_staff] AS STAFF ON STAFF.slm_UserName = FINAL.slm_CreatedBy 
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_campaign CAM ON FINAL.slm_CampaignId = CAM.slm_CampaignId
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_position pos ON FINAL.slm_CreatedBy_Position = pos.slm_Position_id 
                            WHERE FINAL.slm_TicketId = '" + ticketId + @"'
                            ORDER BY FINAL.slm_CreatedDate ASC"; 

            return slmdb.ExecuteStoreQuery<CampaignWSData>(sql).ToList();
        }

        //Reference : SlmScr015BizPopup
        public List<SearchLeadResult> GetUserMonitoringList(string username)
        {
//            string sql = @" SELECT CONVERT(int, ROW_NUMBER() OVER(ORDER BY lead.slm_CampaignId desc)) AS SEQ,LEAD.slm_ticketId AS TicketId,CUS.slm_CitizenId AS CitizenId,LEAD.slm_Name AS Firstname,LEAD.slm_LastName AS Lastname,
//	                            OP.slm_OptionDesc AS StatusDesc,CHANNEL.slm_ChannelDesc AS ChannelDesc,STAFF.slm_StaffNameTH AS OwnerName,
//	                            LEAD.slm_CreatedDate AS  CreatedDate, LEAD.slm_AssignedDate AS AssignedDate,DELEGATE.slm_StaffNameTH AS DelegateName,
//                                CAMP.slm_CampaignName AS CampaignName  
//                            FROM kkslm_tr_lead LEAD 
//                                INNER JOIN 
//                                (
//                                    SELECT DISTINCT SLM_CAMPAIGNID	
//                                    FROM " + SLMDBName + @".DBO.kkslm_ms_staff_group 
//                                    WHERE is_Deleted = 0 AND slm_StaffId IN (
//                                        SELECT slm_StaffId
//                                        FROM " + SLMDBName + @".DBO.kkslm_ms_staff staff 
//                                        WHERE staff.slm_UserName = '" + username + @"')
//                                  ) AS Z ON Z.slm_CampaignId = lead.slm_CampaignId 
//	                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_tr_cusinfo CUS ON CUS.slm_TicketId = LEAD.slm_ticketId 
//	                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_option OP ON OP.slm_OptionCode = LEAD.slm_Status AND OP.slm_OptionType = 'lead status'
//	                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_channel CHANNEL ON CHANNEL.slm_ChannelId = LEAD.slm_ChannelId 
//	                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff STAFF ON STAFF.slm_UserName = LEAD.slm_Owner 
//	                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff DELEGATE ON DELEGATE.slm_UserName = LEAD.slm_Delegate  
//                            WHERE lead.is_Deleted = 0 and LEAD.slm_Owner =  '" + username + "' ";
//            if (assignDateFrom != "" && asssignDateTo != "")
//                sql += " AND CONVERT(DATE, LEAD.slm_AssignedDate) BETWEEN '" + assignDateFrom + @"' AND '" + asssignDateTo + @"' ";
//            //sql += " ORDER BY lead.slm_CampaignId ASC ";

            KKSlmMsStaffModel staff = new KKSlmMsStaffModel();
            string UserLoginBranchCode = staff.GetBrachCode(username);
            decimal? stafftype = staff.GetStaffType(username);

            string sql = @" SELECT CONVERT(int, ROW_NUMBER() OVER(ORDER BY lead.slm_CreatedDate desc)) AS SEQ,LEAD.slm_ticketId AS TicketId,CUS.slm_CitizenId AS CitizenId,LEAD.slm_Name AS Firstname,LEAD.slm_LastName AS Lastname,
                            OP.slm_OptionDesc AS StatusDesc,CHANNEL.slm_ChannelDesc AS ChannelDesc,STAFF.slm_StaffNameTH AS OwnerName,
                            LEAD.slm_CreatedDate AS  CreatedDate, LEAD.slm_AssignedDate AS AssignedDate,DELEGATE.slm_StaffNameTH AS DelegateName,
                            cam.slm_CampaignName AS CampaignName  ,'Owner Lead' as TranferType ,LEAD.slm_CampaignId AS CampaignId,
                             ct.slm_CardTypeName AS CardTypeDesc, LEAD.slm_Counting AS Counting, ISNULL(MP.HasADAMUrl, 0) AS HasAdamUrl
                        FROM kkslm_tr_lead LEAD 
                            INNER JOIN  SLMDB.dbo.kkslm_ms_campaign cam on CAM.slm_CampaignId = LEAD.slm_CampaignId 
                            INNER JOIN
    
                            ( 
                                SELECT DISTINCT B.slm_CampaignId
								FROM (
                                    SELECT CP.PR_CampaignId AS slm_CampaignId FROM kkslm_ms_access_right AR INNER  JOIN CMT_CAMPAIGN_PRODUCT CP ON CP.PR_ProductId = AR.slm_Product_Id
                                    WHERE AR.slm_Product_Id IS NOT NULL AND
                                    AR.slm_BranchCode = '" + UserLoginBranchCode + @"' AND AR.slm_StaffTypeId = '" + stafftype +@"' 
                                    UNION ALL
                                    SELECT AR.slm_CampaignId  AS slm_CampaignId  FROM kkslm_ms_access_right AR 
                                    WHERE AR.slm_CampaignId IS NOT NULL AND
                                    AR.slm_BranchCode = '" + UserLoginBranchCode + @"' AND AR.slm_StaffTypeId = '" + stafftype + @"'
                                ) AS B
                              ) AS Z ON Z.slm_CampaignId = cam.slm_CampaignId 
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_tr_cusinfo CUS ON CUS.slm_TicketId = LEAD.slm_ticketId 
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_option OP ON OP.slm_OptionCode = LEAD.slm_Status AND OP.slm_OptionType = 'lead status'
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_channel CHANNEL ON CHANNEL.slm_ChannelId = LEAD.slm_ChannelId 
	                        LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff STAFF ON STAFF.slm_UserName = LEAD.slm_Owner 
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff DELEGATE ON DELEGATE.slm_UserName = LEAD.slm_Delegate  
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_cardtype ct ON cus.slm_CardType = ct.slm_CardTypeId
                            LEFT JOIN " + SLMDBName + @".dbo.CMT_MAPPING_PRODUCT MP ON lead.slm_Product_Id = MP.sub_product_id
                        WHERE lead.is_Deleted = 0 and lead.slm_Owner is null and lead.slm_Status IN ('" + SLMConstant.StatusCode.Interest + @"','" + SLMConstant.StatusCode.WaitContact + @"') ";

            return slmdb.ExecuteStoreQuery<SearchLeadResult>(sql).ToList();
        }

        //Reference : SlmScr015BizPopup
        public List<SearchLeadResult> GetUnassignLeadList(string username)
        {
            string sql = @"SELECT CONVERT(int, ROW_NUMBER() OVER(ORDER BY lead.slm_CampaignId desc)) AS SEQ,LEAD.slm_ticketId AS TicketId,CUS.slm_CitizenId AS CitizenId,LEAD.slm_Name AS Firstname,LEAD.slm_LastName AS Lastname,
                            OP.slm_OptionDesc AS StatusDesc,CHANNEL.slm_ChannelDesc AS ChannelDesc,STAFF.slm_StaffNameTH AS OwnerName,
                            LEAD.slm_CreatedDate AS  CreatedDate, LEAD.slm_AssignedDate AS AssignedDate,DELEGATE.slm_StaffNameTH AS DelegateName,
                            CAMP.slm_CampaignName AS CampaignName 
                            FROM " + SLMDBName + @".dbo.kkslm_tr_lead lead 
                            INNER JOIN 
                            (
                                SELECT DISTINCT SLM_CAMPAIGNID	
                                FROM " + SLMDBName + @".DBO.kkslm_ms_staff_group 
                                WHERE is_Deleted = 0 AND slm_StaffId IN (
                                    SELECT slm_StaffId
                                    FROM " + SLMDBName + @".DBO.kkslm_ms_staff staff 
                                    WHERE staff.slm_UserName = '" + username + @"')
                              ) AS Z ON Z.slm_CampaignId = lead.slm_CampaignId 
                            INNER JOIN " + SLMDBName + @".dbo.kkslm_ms_campaign CAMP ON lead.slm_CampaignId = CAMP.slm_CampaignId 
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_tr_cusinfo CUS ON CUS.slm_TicketId = LEAD.slm_ticketId 
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_option OP ON OP.slm_OptionCode = LEAD.slm_Status AND OP.slm_OptionType = 'lead status'
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_channel CHANNEL ON CHANNEL.slm_ChannelId = LEAD.slm_ChannelId 
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff STAFF ON STAFF.slm_UserName = LEAD.slm_Owner 
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff DELEGATE ON DELEGATE.slm_UserName = LEAD.slm_Delegate  
                            WHERE lead.slm_Status IN ('" + SLMConstant.StatusCode.Interest + @"','" + SLMConstant.StatusCode.WaitContact + @"') 
                            AND lead.slm_Owner IS NULL AND LEAD.is_Deleted = 0 ";

            return slmdb.ExecuteStoreQuery<SearchLeadResult>(sql).ToList();
        }

        public LeadDataForAdam GetLeadDataForAdam(string ticketId)
        {
            string sql = @"SELECT LEAD.slm_ticketId AS TicketId, LEAD.slm_Name AS Firstname, LEAD.slm_TelNo_1 AS TelNo1, LEAD.slm_CampaignId AS Campaign, LEAD.slm_Ext_1 AS ExtNo1, LEAD.slm_AvailableTime AS AvailableTime, LEAD.slm_Owner AS TelesaleName, LEAD.slm_Status AS Status
                            , LEAD.slm_Product_Group_Id AS ProductGroupId, LEAD.slm_Product_Id AS ProductId
                            , CUS.slm_LastName AS Lastname, CUS.slm_Email AS Email, CUS.slm_TelNo_2 AS TelNo2, CUS.slm_TelNo_3 AS TelNo3, CUS.slm_Ext_2 AS ExtNo2
                            , CUS.slm_Ext_3 AS ExtNo3, CUS.slm_BuildingName AS BuildingName, CUS.slm_AddressNo AS AddrNo, CUS.slm_Floor AS Floor, CUS.slm_Soi AS Soi
                            , CUS.slm_Street AS Street, CUS.slm_Tambon AS Tambol, TAM.slm_TambolCode AS TambolCode, CUS.slm_Amphur AS Amphur, AM.slm_AmphurCode AS AmphurCode, CUS.slm_Province AS Province, PRO.slm_ProvinceCode AS ProvinceCode, CUS.slm_PostalCode AS PostalCode
                            , CUS.slm_Occupation AS Occupation, OCC.slm_OccupationCode AS OccupationCode, CUS.slm_BaseSalary AS BaseSalary, CUS.slm_IsCustomer AS IsCustomer, CUS.slm_CusCode AS CustomerCode
                            , CUS.slm_Birthdate AS DateOfBirth, CUS.slm_CitizenId AS Cid, CUS.slm_Topic AS Topic, CUS.slm_Detail AS Detail, CUS.slm_PathLink AS PathLink
                            , CUS.slm_ContactBranch AS ContactBranch
                            , PROD.slm_InterestedProd AS InterestedProdAndType, PROD.slm_LicenseNo AS LicenseNo, PROD.slm_YearOfCar AS YearOfCar, PROD.slm_YearOfCarRegis AS YearOfCarRegis
                            , PROD.slm_ProvinceRegis AS RegisterProvince, PRO2.slm_ProvinceCode AS RegisterProvinceCode, PROD.slm_Brand AS Brand, PROD.slm_Model AS Model, PROD.slm_Submodel AS Submodel, PROD.slm_DownPayment AS DownPayment
                            , PROD.slm_DownPercent AS DownPercent, PROD.slm_CarPrice AS CarPrice, PROD.slm_CarType AS CarType, PROD.slm_FinanceAmt AS FinanceAmt, PROD.slm_PaymentTerm AS Term
                            , PROD.slm_PaymentType AS PaymentType, PROD.slm_BalloonAmt AS BalloonAmt, PROD.slm_BalloonPercent AS BalloonPercent, PROD.slm_PlanType AS PlanType
                            , PROD.slm_CoverageDate AS CoverageDate, PROD.slm_AccType AS AccType, PROD.slm_AccPromotion AS AccPromotion, PROD.slm_AccTerm AS AccTerm, PROD.slm_Interest AS Interest
                            , PROD.slm_Invest AS Invest, PROD.slm_LoanOd AS LoanOd, PROD.slm_LoanOdTerm AS LoanOdTerm, PROD.slm_Ebank AS SlmBank, PROD.slm_Atm AS SlmAtm
                            , PROD.slm_OtherDetail_1 AS OtherDetail1, PROD.slm_OtherDetail_2 AS OtherDetail2, PROD.slm_OtherDetail_3 AS OtherDetail3, PROD.slm_OtherDetail_4 AS OtherDetail4
                            , CHAN.slm_ChannelId AS ChannelId, CHAN.slm_RequestDate AS RequestDate, CHAN.slm_RequestBy AS CreateUser, CHAN.slm_IPAddress AS Ipaddress, CHAN.slm_Company AS Company
                            , CHAN.slm_Branch AS Branch, CHAN.slm_BranchNo AS BranchNo, CHAN.slm_MachineNo AS MachineNo, CHAN.slm_ClientServiceType AS ClientServiceType
                            , CHAN.slm_DocumentNo AS DocumentNo, CHAN.slm_CommPaidCode AS CommPaidCode, CHAN.slm_Zone AS Zone, CHAN.slm_TransId AS TransId
                            , BRAND.slm_BrandCode AS BrandCode, MODEL.slm_Family AS ModelFamily, CONVERT(VARCHAR, SUBMODEL.slm_RedBookNo) AS SubmodelRedBookNo, PAYTYPE.slm_PaymentCode AS PaymentCode
                            , CONVERT(VARCHAR, ACCTYPE.slm_ModuleCode) AS AccTypeCode, PROMOTE.slm_PromotionCode AS AccPromotionCode, OPT.slm_OptionDesc AS StatusDesc, MP.url_ADAM AS AdamsUrl
                            FROM " + SLMDBName + @".dbo.kkslm_tr_lead LEAD
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_tr_cusinfo CUS ON LEAD.slm_ticketId = CUS.slm_TicketId
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_tr_productinfo PROD ON LEAD.slm_ticketId = PROD.slm_TicketId
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_tr_channelinfo CHAN ON LEAD.slm_ticketId = CHAN.slm_TicketId
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_tambol TAM ON CUS.slm_Tambon = TAM.slm_TambolId
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_amphur AM ON CUS.slm_Amphur = AM.slm_AmphurId
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_province PRO ON CUS.slm_Province = PRO.slm_ProvinceId
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_occupation OCC ON CUS.slm_Occupation = OCC.slm_OccupationId
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_province PRO2 ON PROD.slm_ProvinceRegis = PRO2.slm_ProvinceId
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_brand BRAND ON PROD.slm_Brand = BRAND.slm_BrandId
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_model MODEL ON PROD.slm_Model = MODEL.slm_ModelId
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_submodel SUBMODEL ON PROD.slm_Submodel = SUBMODEL.slm_SubModelId
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_paymenttype PAYTYPE ON PROD.slm_PaymentType = PAYTYPE.slm_PaymentId
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_module ACCTYPE ON PROD.slm_AccType = ACCTYPE.slm_ModuleId
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_promotion PROMOTE ON PROD.slm_AccPromotion = PROMOTE.slm_PromotionId
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_option OPT ON OPT.slm_OptionCode = LEAD.slm_Status AND OPT.slm_OptionType = 'lead status'
                            LEFT JOIN " + SLMDBName + @".dbo.CMT_MAPPING_PRODUCT MP ON MP.sub_product_id = LEAD.slm_Product_Id
                            WHERE LEAD.slm_ticketId = '" + ticketId + "'";

            return slmdb.ExecuteStoreQuery<LeadDataForAdam>(sql).FirstOrDefault();
        }

//        public List<CampaignReferData> SearchCampaignReferMockup()
//        {
//            string sql = @"SELECT 'อุดมรัตน์' AS FIRSTNAME,'จันทร์ประดิษฐ์' AS LASTNAME,'0833944787' AS PHONE,'00021312' AS CONTRACT_NO,
//		                    '' AS REMARK,'kruawan.rat' AS ASSIGNMENT,'02041406002' AS CampaignCode, 'Home loan - New Home' AS CampaignName,
//		                    'สินเชื่อบ้านใหม่ : ลูกค้าที่ต้องการซื้อบ้านใหม่' AS CampaignDetailCut,'สินเชื่อบ้านใหม่ : ลูกค้าที่ต้องการซื้อบ้านใหม่' AS CampaignDetail,
//                            CONVERT(DATE,'2015-12-31') AS ExpireDate,'Branch,Call Center,Telesales' AS ChannelName ";
//            return slmdb.ExecuteStoreQuery<CampaignReferData>(sql).ToList();
//        }

//        public List<CampaignReferData> SearchCampaignReferHistoryMockup()
//        {
//            string sql = @"SELECT 'เบญจมาศ' AS FIRSTNAME,'กำแพงทิพย์' AS LASTNAME,'0833944787' AS PHONE,'123123214' AS CONTRACT_NO,
//		                    '' AS REMARK,'04051501057' AS CampaignCode, 'ฝากประจำ อัตราดอกเบี้ยสูงสุด 4 %' AS CampaignName,
//		                    'ฝากประจำดอกเบี้ย 4 % สำหรับลูกค้าเช่าซื้อรถยนต์กับธนาคารเกียรตินาคิน : ฝากประจำดอกเบี้ย 4 % สำหรับลูกค้าเช่าซื้อรถยนต์กับธนาคารเกียรตินาคิน' AS CampaignDetail,CONVERT(DATE,'2015-12-31') AS ExpireDate,
//		                    'Call Center' AS ChannelName,'สำนักงานใหญ่ - Call Center' as BranchName,'เครือวรรณ รัตนทิพย์' as OfferName,
//		                    '2558-03-20' as UpdatedDate,'ฝากประจำดอกเบี้ย 4 % สำหรับลูกค้าเช่าซื้อรถยนต์กับธนาคารเกียรตินาคิน : ฝากประจำดอกเบี้ย 4 % สำหรับลูกค้าเช่าซื้อรถยนต์กับธนาคารเกียรตินาคิน'  as CampaignDetailCut,'สนใจ' as Interest  ";
//            return slmdb.ExecuteStoreQuery<CampaignReferData>(sql).ToList();
//        }

    }
}

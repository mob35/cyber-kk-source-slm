using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using SLM.Resource.Data;
using SLM.Resource;
using System.Collections;

namespace SLM.Dal.Models
{
    public class KKSlmMsStaffModel
    {
        private SLM_DBEntities slmdb = null;
        private string SLMDBName = ConfigurationManager.AppSettings["SLMDBName"] != null ? ConfigurationManager.AppSettings["SLMDBName"] : "SLMDB";

        public KKSlmMsStaffModel()
        {
            slmdb = new SLM_DBEntities();
        }

        public KKSlmMsStaffModel(SLM_DBEntities db)
        {
            slmdb = db;
        }

        public List<ControlListData> GetStaffData(int staffTypeId)
        {
            return slmdb.kkslm_ms_staff.Where(p => p.slm_StaffTypeId == staffTypeId && p.is_Deleted == 0).OrderBy(p => p.slm_StaffNameTH).Select(p => new ControlListData { TextField = p.slm_StaffNameTH, ValueField = p.slm_UserName }).ToList();
        }

        public List<StaffData> GetStaffList()
        {
            return slmdb.kkslm_ms_staff.Where(p => p.is_Deleted == 0).Select(p => new StaffData { UserName = p.slm_UserName, StaffId = p.slm_StaffId, HeadStaffId = p.slm_HeadStaffId }).ToList();
        }

        public List<ControlListData> GetStaffList(string branchCode)
        {
            string sql = @"SELECT CASE WHEN pos.slm_PositionNameAbb IS NULL THEN staff.slm_StaffNameTH
			                            ELSE pos.slm_PositionNameAbb + ' - ' + staff.slm_StaffNameTH END AS TextField, staff.slm_UserName AS ValueField
                            FROM " + SLMDBName + @".dbo.kkslm_ms_staff staff
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_position pos ON staff.slm_Position_id = pos.slm_Position_id
                            WHERE staff.slm_BranchCode = '" + branchCode + @"' AND staff.is_Deleted = '0'
                            AND staff.slm_StaffTypeId <> '" + SLMConstant.StaffType.ITAdministrator + @"'
                            ORDER BY staff.slm_StaffNameTH ";

            return slmdb.ExecuteStoreQuery<ControlListData>(sql).ToList();
        }

        public List<ControlListData> GetStaffBranchAndRecursiveList(string branchCode,string recursivelist)
        {
            string sql = @"SELECT CASE WHEN pos.slm_PositionNameAbb IS NULL THEN staff.slm_StaffNameTH
			                            ELSE pos.slm_PositionNameAbb + ' - ' + staff.slm_StaffNameTH END AS TextField, staff.slm_UserName AS ValueField
                            FROM " + SLMDBName + @".dbo.kkslm_ms_staff staff
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_position pos ON staff.slm_Position_id = pos.slm_Position_id
                            WHERE staff.slm_BranchCode = '" + branchCode + @"' AND staff.is_Deleted = '0' 
                            AND staff.slm_UserName IN ("+ recursivelist + @") 
                            AND staff.slm_StaffTypeId <> '" + SLMConstant.StaffType.ITAdministrator + @"'
                            ORDER BY staff.slm_StaffNameTH ";

            return slmdb.ExecuteStoreQuery<ControlListData>(sql).ToList();
        }

        public List<ControlListData> GetHeadStaffList(string branchCode)
        {
            string sql = @"SELECT CASE WHEN pos.slm_PositionNameAbb IS NULL THEN staff.slm_StaffNameTH
			                            ELSE pos.slm_PositionNameAbb + ' - ' + staff.slm_StaffNameTH END AS TextField, CONVERT(VARCHAR, staff.slm_StaffId) AS ValueField
                            FROM " + SLMDBName + @".dbo.kkslm_ms_staff staff
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_position pos ON staff.slm_Position_id = pos.slm_Position_id
                            WHERE staff.slm_BranchCode = '" + branchCode + @"' AND staff.is_Deleted = '0'
                            AND staff.slm_StaffTypeId <> '" + SLMConstant.StaffType.ITAdministrator + @"'
                            ORDER BY staff.slm_StaffNameTH ";

            return slmdb.ExecuteStoreQuery<ControlListData>(sql).ToList();
        }

        public List<ControlListData> GetStaffAllDataByAccessRight(string campaignId, string branch)
        {
            string sql = @"SELECT CASE WHEN PO.slm_PositionNameAbb IS NULL THEN  staff.slm_StaffNameTH  
                                  ELSE PO.slm_PositionNameAbb + ' - ' + STAFF.slm_StaffNameTH END TextField  ,staff.slm_UserName  AS ValueField 
                            FROM " + SLMDBName + @".dbo.kkslm_ms_staff staff 
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_position po on staff.slm_Position_id = po.slm_Position_id 
                            INNER JOIN (
                                            SELECT DISTINCT Z.slm_BranchCode,Z.slm_StaffTypeId  
                                            FROM (
                                                    SELECT AR.slm_BranchCode,AR.slm_StaffTypeId 
                                                    FROM " + SLMDBName + @".dbo.kkslm_ms_access_right AR INNER JOIN kkslm_ms_campaign CAM ON CAM.slm_CampaignId = AR.slm_CampaignId 
                                                    WHERE CAM.slm_CampaignId = '{1}' 
                                                    UNION ALL
                                                    SELECT AR.slm_BranchCode,AR.slm_StaffTypeId 
                                                    FROM " + SLMDBName + @".dbo.kkslm_ms_access_right AR INNER JOIN " + SLMDBName + @".dbo.CMT_CAMPAIGN_PRODUCT CP ON CP.PR_ProductId = AR.slm_Product_Id
                                                    WHERE CP.PR_ProductId = (SELECT PR_ProductId FROM " + SLMDBName + @".dbo.CMT_CAMPAIGN_PRODUCT 
													                            WHERE PR_CAMPAIGNID = '{1}')
                                                 ) AS Z
                                          ) AS A ON A.slm_BranchCode = STAFF.slm_BranchCode AND A.slm_StaffTypeId = STAFF.slm_StaffTypeId              
                            where staff.is_Deleted = 0 and staff.slm_BranchCode ='{0}' AND STAFF.slm_StaffTypeId NOT IN ({2})    
                            ORDER BY staff.slm_StaffNameTH";

            sql = string.Format(sql, branch, campaignId, SLMConstant.StaffType.ITAdministrator);

            return slmdb.ExecuteStoreQuery<ControlListData>(sql).ToList();
        }

        public bool CheckStaffAccessRightExist(string campaignId, string branch,string username)
        {
            string sql = @"SELECT CASE WHEN PO.slm_PositionNameAbb IS NULL THEN  staff.slm_StaffNameTH  
		                          ELSE PO.slm_PositionNameAbb+' - '+ STAFF.slm_StaffNameTH END TextField  ,staff.slm_UserName  AS ValueField 
                            FROM kkslm_ms_staff staff left join kkslm_ms_position po on staff.slm_Position_id = po.slm_Position_id 
                            where staff.is_Deleted = 0 and staff.slm_BranchCode ='{0}' and
	                            staff.slm_StaffTypeId in 
	                            (	SELECT DISTINCT Z.slm_StaffTypeId 
		                            FROM (
				                            SELECT AR.slm_StaffTypeId
				                            FROM kkslm_ms_access_right AR INNER JOIN kkslm_ms_campaign CAM ON CAM.slm_CampaignId = AR.slm_CampaignId 
				                            WHERE CAM.slm_CampaignId = '{1}'
				                            UNION ALL
				                            SELECT AR.slm_StaffTypeId
				                            FROM kkslm_ms_access_right AR INNER JOIN CMT_CAMPAIGN_PRODUCT CP ON CP.PR_ProductId = AR.slm_Product_Id
				                            WHERE CP.PR_ProductId = (SELECT PR_ProductId FROM CMT_CAMPAIGN_PRODUCT WHERE PR_CAMPAIGNID = '{1}')
			                              ) AS Z 
                                  ) AND STAFF.slm_StaffTypeId NOT IN ({2}) and staff.slm_UserName = '{3}' 
                            ORDER BY staff.slm_StaffNameTH";

            sql = string.Format(sql, branch, campaignId, SLMConstant.StaffType.ITAdministrator,username );

            var list = slmdb.ExecuteStoreQuery<ControlListData>(sql).ToList();
            if (list.Count > 0)
                return true;
            else
                return false;
        }
        public List<ControlListData> GetStaffHeadData(string branch)
        {
            decimal[] dec = { SLMConstant.StaffType.ITAdministrator };
            return slmdb.kkslm_ms_staff.Where(p => p.slm_BranchCode == branch && p.is_Deleted == 0 && dec.Contains(p.slm_StaffTypeId) == false).OrderBy(p => p.slm_StaffNameTH).AsEnumerable().Select(p => new ControlListData { TextField = p.slm_StaffNameTH, ValueField = p.slm_StaffId.ToString() }).ToList();
        }

        public string GetBrachCode(string username)
        {
            var staffdata =  slmdb.kkslm_ms_staff.Where(p => p.slm_UserName == username && p.is_Deleted == 0).FirstOrDefault();
            if (staffdata != null)
                return staffdata.slm_BranchCode;
            else
                return string.Empty;
        }

        public string GetBrachCodeByEmpCode(string empCode)
        {
            var staffdata = slmdb.kkslm_ms_staff.Where(p => p.slm_EmpCode == empCode && p.is_Deleted == 0).FirstOrDefault();
            if (staffdata != null)
                return staffdata.slm_BranchCode;
            else
                return string.Empty;
        }

        public string GetUsernameByEmpCode(string empCode)
        {
            var staffdata = slmdb.kkslm_ms_staff.Where(p => p.slm_EmpCode == empCode && p.is_Deleted == 0).FirstOrDefault();
            if (staffdata != null)
                return staffdata.slm_UserName;
            else
                return string.Empty;
        }

        public StaffData GetStaffData(string username)
        {
            var data = (from staff in slmdb.kkslm_ms_staff
                        join branch in slmdb.kkslm_ms_branch on staff.slm_BranchCode equals branch.slm_BranchCode into branchjoin
                        from branch1 in branchjoin.DefaultIfEmpty()
                        where staff.slm_UserName == username && staff.is_Deleted == 0
                        select new StaffData { StaffNameTH = staff.slm_StaffNameTH, BranchName = branch1.slm_BranchName, BranchCode = branch1.slm_BranchCode , StaffId = staff.slm_StaffId, PositionId = staff.slm_Position_id }).FirstOrDefault();

            return data;
        }

        public string GetStaffIdData(string username)
        {
            var staffdata = slmdb.kkslm_ms_staff.Where(p => p.slm_UserName == username && p.is_Deleted == 0).FirstOrDefault();
            if (staffdata != null)
                return staffdata.slm_StaffId.ToString();
            else
                return string.Empty;
        }

        public string GetStaffNameData(string username)
        {
            var staffdata = slmdb.kkslm_ms_staff.Where(p => p.slm_UserName == username && p.is_Deleted == 0).FirstOrDefault();
            if (staffdata != null)
                return staffdata.slm_StaffNameTH.ToString();
            else
                return string.Empty;
        }

        public List<ControlListData> GetOwnerList(string username)
        {
            string sql = @"SELECT DISTINCT staff.slm_StaffNameTH AS TextField, staff.slm_UserName AS ValueField
                            FROM " + SLMDBName + @".dbo.kkslm_ms_staff staff
                            INNER JOIN " + SLMDBName + @".dbo.kkslm_ms_staff_group staffgroup ON staff.slm_StaffId = staffgroup.slm_StaffId
                            INNER JOIN " + SLMDBName + @".dbo.kkslm_ms_group grp ON grp.slm_GroupId = staffgroup.slm_GroupId
                            WHERE grp.slm_CampaignId IN (
										SELECT DISTINCT B.slm_CampaignId
										FROM
										(
											SELECT SLM_CAMPAIGNID
											FROM " + SLMDBName + @".DBO.kkslm_ms_group [GROUP] 
											WHERE slm_GroupId IN (
													SELECT slm_GroupId 
													FROM " + SLMDBName + @".DBO.kkslm_ms_staff_group 
													WHERE is_Deleted = 0 AND slm_StaffId IN (
														SELECT slm_StaffId
														FROM " + SLMDBName + @".DBO.kkslm_ms_staff staff 
														WHERE staff.slm_UserName = '" + username + @"'))
											UNION ALL
											SELECT SLM_CAMPAIGNID
											FROM " + SLMDBName + @".DBO.kkslm_ms_group [GROUP] 
											WHERE slm_StaffId IN (
														SELECT slm_StaffId
														FROM " + SLMDBName + @".DBO.kkslm_ms_staff staff 
														WHERE staff.slm_UserName =  '" + username + @"')
										) AS B) AND staff.is_Deleted = 0 AND staff.slm_StaffTypeId <> 7 ORDER BY staff.slm_StaffNameTH";

            return slmdb.ExecuteStoreQuery<ControlListData>(sql).ToList();
        }

        public List<ControlListData> GetOwnerListByCampaignId(string campaignId)
        {
            string sql = @"SELECT DISTINCT staff.slm_StaffNameTH AS TextField, staff.slm_UserName AS ValueField
                            FROM " + SLMDBName + @".dbo.kkslm_ms_staff staff
                            INNER JOIN " + SLMDBName + @".dbo.kkslm_ms_staff_group staffgroup ON staff.slm_StaffId = staffgroup.slm_StaffId
                            WHERE staffgroup.slm_CampaignId IN ('" + campaignId + "') AND staff.is_Deleted = 0  AND staff.slm_StaffTypeId <> 7  ORDER BY staff.slm_StaffNameTH";

            return slmdb.ExecuteStoreQuery<ControlListData>(sql).ToList();
        }

        public List<ControlListData> GetOwnerListByCampaignIdAndBranch(string campaignId, string branchcode)
        {
            string sql = "";
            if (campaignId == "")
            {
                sql = @"SELECT Z.*
                        FROM (
                        SELECT DISTINCT A.*
                        FROM
                        (
                            SELECT slm_PositionName+' - '+staff.slm_StaffNameTH  AS TextField, staff.slm_UserName AS ValueField,staff.slm_StaffNameTH
                            FROM " + SLMDBName + @".dbo.kkslm_ms_staff staff
                            INNER JOIN " + SLMDBName + @".dbo.kkslm_ms_staff_group staffgroup ON staff.slm_StaffId = staffgroup.slm_StaffId
                            WHERE staffgroup.slm_CampaignId IN ('" + campaignId + @"') AND staff.is_Deleted = 0  AND staff.slm_StaffTypeId <> " + SLMConstant.StaffType.ITAdministrator + @"
                            UNION ALL
                            SELECT slm_PositionName+' - '+staff.slm_StaffNameTH  AS TextField, staff.slm_UserName AS ValueField,staff.slm_StaffNameTH
                            FROM " + SLMDBName + @".dbo.kkslm_ms_staff staff
                            INNER JOIN " + SLMDBName + @".dbo.kkslm_ms_staff_group staffgroup ON staff.slm_StaffId = staffgroup.slm_StaffId
                            WHERE staff.is_Deleted = 0  AND staff.slm_StaffTypeId = "+ SLMConstant.StaffType.Marketing +@"      
                        ) AS A ) AS Z   ORDER BY Z.slm_StaffNameTH ";
            }
            else
            {
                sql = @"SELECT Z.*
                        FROM (
                        SELECT DISTINCT A.*
                        FROM
                        (
                        SELECT slm_PositionName+' - '+staff.slm_StaffNameTH  AS TextField, staff.slm_UserName AS ValueField,staff.slm_StaffNameTH
                        FROM " + SLMDBName + @".dbo.kkslm_ms_staff staff
                        INNER JOIN " + SLMDBName + @".dbo.kkslm_ms_staff_group staffgroup ON staff.slm_StaffId = staffgroup.slm_StaffId
                        WHERE staffgroup.slm_CampaignId IN ('" + campaignId + @"') AND staff.is_Deleted = 0  AND staff.slm_StaffTypeId <> "+ SLMConstant.StaffType.ITAdministrator +@" 
                                and staff.slm_BranchCode = '" + branchcode + @"' 
                        UNION ALL
                        SELECT slm_PositionName+' - '+staff.slm_StaffNameTH  AS TextField, staff.slm_UserName AS ValueField,staff.slm_StaffNameTH
                        FROM " + SLMDBName + @".dbo.kkslm_ms_staff staff
                        INNER JOIN " + SLMDBName + @".dbo.kkslm_ms_staff_group staffgroup ON staff.slm_StaffId = staffgroup.slm_StaffId
                        WHERE staff.is_Deleted = 0  AND staff.slm_StaffTypeId = "+ SLMConstant.StaffType.Marketing +@" and staff.slm_BranchCode = '" + branchcode + @"'      
                        ) AS A ) AS Z   ORDER BY Z.slm_StaffNameTH 
                           ";
            }

            return slmdb.ExecuteStoreQuery<ControlListData>(sql).ToList();
        }

        public decimal? GetStaffType(string username)
        {
            var staffdata = slmdb.kkslm_ms_staff.Where(p => p.slm_UserName == username && p.is_Deleted == 0).FirstOrDefault();
            if (staffdata != null)
                return staffdata.slm_StaffTypeId;
            else
                return null;
        }

        //Reference: SlmScr016Biz, SlmScr004Biz
        public List<StaffData> GetChannelStaffData(string username)
        {
            if (username != "")
            {
                string sql = @"SELECT staff.slm_UserName AS UserName
                                , CASE WHEN pos.slm_PositionNameAbb IS NULL THEN staff.slm_StaffNameTH
	                                   ELSE pos.slm_PositionNameAbb + ' - ' + staff.slm_StaffNameTH END AS StaffNameTH
                                , chan.slm_ChannelId AS ChannelId, branch.slm_BranchName AS BranchName,chan.slm_ChannelDesc AS ChannelDesc 
                                FROM " + SLMDBName + @".dbo.kkslm_ms_staff staff
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_position pos ON staff.slm_Position_id = pos.slm_Position_id
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_branch branch ON staff.slm_BranchCode = branch.slm_BranchCode
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_channel chan ON branch.slm_ChannelId = chan.slm_ChannelId
                                WHERE staff.slm_UserName = '" + username + "' AND staff.is_Deleted = 0 ";

                return slmdb.ExecuteStoreQuery<StaffData>(sql).ToList();
            }
            else
            {
                string sql = @"SELECT staff.slm_UserName AS UserName
                                , CASE WHEN pos.slm_PositionNameAbb IS NULL THEN staff.slm_StaffNameTH
	                                   ELSE pos.slm_PositionNameAbb + ' - ' + staff.slm_StaffNameTH END AS StaffNameTH
                                , chan.slm_ChannelId AS ChannelId, branch.slm_BranchName AS BranchName,chan.slm_ChannelDesc AS ChannelDesc 
                                FROM " + SLMDBName + @".dbo.kkslm_ms_staff staff 
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_position pos ON staff.slm_Position_id = pos.slm_Position_id
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_branch branch ON staff.slm_BranchCode = branch.slm_BranchCode
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_channel chan ON branch.slm_ChannelId = chan.slm_ChannelId
                                WHERE staff.is_Deleted = 0 ";

                return slmdb.ExecuteStoreQuery<StaffData>(sql).ToList();
            }
        }

        public int? GetDeptData(string username)
        {
            var staffdata = slmdb.kkslm_ms_staff.Where(p => p.slm_UserName == username && p.is_Deleted == 0).FirstOrDefault();
            if (staffdata != null)
                return staffdata.slm_DepartmentId;
            else
                return null;
        }

        public bool CheckUsernameExist(string username,int? staffid)
        {
            if (staffid == null)
            {
                var user = slmdb.kkslm_ms_staff.Where(p => p.slm_UserName == username && p.is_Deleted == 0).FirstOrDefault();
                return user != null ? true : false;
            }
            else
            {
                var user = slmdb.kkslm_ms_staff.Where(p => p.slm_UserName == username && p.is_Deleted == 0 && p.slm_StaffId != staffid).FirstOrDefault();
                return user != null ? true : false;
            }
        }

        public bool CheckEmpCodeExist(string empCode, int? staffid)
        {
            if (staffid == null)
            {
                var user = slmdb.kkslm_ms_staff.Where(p => p.slm_EmpCode == empCode && p.is_Deleted == 0).FirstOrDefault();
                return user != null ? true : false;
            }
            else
            {
                var user = slmdb.kkslm_ms_staff.Where(p => p.slm_EmpCode == empCode && p.is_Deleted == 0 && p.slm_StaffId != staffid).FirstOrDefault();
                return user != null ? true : false;
            }
        }

        public bool CheckMarketingCodeExist(string marketingCode, int? staffid)
        {
            if (staffid == null)
            {
                var user = slmdb.kkslm_ms_staff.Where(p => p.slm_MarketingCode == marketingCode && p.is_Deleted == 0).FirstOrDefault();
                return user != null ? true : false;
            }
            else
            {
                var user = slmdb.kkslm_ms_staff.Where(p => p.slm_MarketingCode == marketingCode && p.is_Deleted == 0 && p.slm_StaffId != staffid).FirstOrDefault();
                return user != null ? true : false;
            }
        }

        public string InsertStaff(StaffDataManagement data, string username)
        {
            try
            {
                kkslm_ms_staff staff = new kkslm_ms_staff();
                staff.slm_UserName = data.Username;
                staff.slm_EmpCode = data.EmpCode;

                if (!string.IsNullOrEmpty(data.MarketingCode))
                    staff.slm_MarketingCode = data.MarketingCode;

                staff.slm_StaffNameTH = data.StaffNameTH;

                if (!string.IsNullOrEmpty(data.TelNo))
                    staff.slm_TellNo = data.TelNo;

                staff.slm_StaffEmail = data.StaffEmail;
                staff.slm_Position_id = data.PositionId;

                if (!string.IsNullOrEmpty(data.Team))
                    staff.slm_Team = data.Team;
                if (data.StaffTypeId != null)
                    staff.slm_StaffTypeId = data.StaffTypeId.Value;
                if (!string.IsNullOrEmpty(data.BranchCode))
                    staff.slm_BranchCode = data.BranchCode;
                if (data.HeadStaffId != null)
                    staff.slm_HeadStaffId = data.HeadStaffId.Value;
                if (data.DepartmentId != null)
                    staff.slm_DepartmentId = data.DepartmentId.Value;

                staff.slm_CreatedBy = username;
                staff.slm_CreatedDate = DateTime.Now;
                staff.is_Deleted = 0;
                staff.slm_IsActive = 0;
                staff.slm_IsLocked = 0;
                staff.slm_UpdateStatusDate = DateTime.Now;
                staff.slm_UpdateStatusBy = username;

                slmdb.kkslm_ms_staff.AddObject(staff);
                slmdb.SaveChanges();
                return staff.slm_StaffId.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string UpdateStaff(StaffDataManagement data, string username,int flag)
        {
            var staff= slmdb.kkslm_ms_staff.Where(p => p.slm_StaffId == data.StaffId).FirstOrDefault();
            if (staff != null)
            {
                try
                {
                    staff.slm_UserName = data.Username;
                    staff.slm_EmpCode = data.EmpCode;
                    staff.slm_MarketingCode = data.MarketingCode;
                    staff.slm_StaffNameTH = data.StaffNameTH;
                    staff.slm_TellNo = data.TelNo;
                    staff.slm_StaffEmail = data.StaffEmail;
                    staff.slm_Position_id = data.PositionId;
                    staff.slm_Team = data.Team;
                    if (data.StaffTypeId != null)
                        staff.slm_StaffTypeId = data.StaffTypeId.Value;
                    if (!string.IsNullOrEmpty(data.BranchCode))
                        staff.slm_BranchCode = data.BranchCode;

                    staff.slm_DepartmentId = data.DepartmentId;

                    staff.slm_HeadStaffId = data.HeadStaffId;
                    staff.slm_UpdatedBy = username;
                    staff.slm_UpdatedDate = DateTime.Now;
                    staff.is_Deleted = data.Is_Deleted.Value ;
                    if (flag == 1)
                    {
                        staff.slm_UpdateStatusDate = DateTime.Now;
                        staff.slm_UpdateStatusBy = username;
                    }

                    slmdb.SaveChanges();
                    return data.StaffId.ToString();
                    
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
                return data.StaffId.ToString();
           
        }

        public StaffDataManagement GetStaffDataForInsert(int staffId)
        {
            return slmdb.kkslm_ms_staff.Where(p => p.slm_StaffId == staffId).Select(p =>
                new StaffDataManagement
                {
                    StaffId = p.slm_StaffId, 
                    Username = p.slm_UserName , 
                    EmpCode = p.slm_EmpCode,
                    MarketingCode = p.slm_MarketingCode,
                    StaffNameTH = p.slm_StaffNameTH ,
                    TelNo = p.slm_TellNo,
                    StaffEmail = p.slm_StaffEmail ,
                    StaffTypeId = p.slm_StaffTypeId,
                    Team = p.slm_Team,
                    BranchCode = p.slm_BranchCode,
                    HeadStaffId = p.slm_HeadStaffId ,
                    PositionId = p.slm_Position_id,
                    PositionName = p.slm_PositionName,
                    DepartmentId = p.slm_DepartmentId,
                    Is_Deleted = p.is_Deleted}).FirstOrDefault();
        }

        public List<StaffDataManagement> SearchStaffList(string username, string branchCode, string empCode, string marketingCode, string staffNameTH, string positionId
            , string staffTypeId, string team, string departmentId)
        {
            string sql = @"SELECT staff.slm_StaffId AS StaffId, staff.slm_EmpCode AS EmpCode, staff.slm_MarketingCode AS MarketingCode, staff.slm_UserName AS Username, staff.slm_StaffNameTH AS StaffNameTH
                            , staff.slm_Position_id AS PositionId, pos.slm_PositionNameTH AS PositionName, staff.slm_StaffTypeId AS StaffTypeId, st.slm_StaffTypeDesc AS StaffTypeDesc, staff.slm_Team AS Team, staff.slm_BranchCode AS BranchCode, branch.slm_BranchName AS BranchName
                            , staff.slm_DepartmentId AS DepartmentId, dep.slm_DepartmentName AS DepartmentName, staff.is_Deleted AS Is_Deleted,staff.slm_UpdateStatusDate AS UpdateStatusDate 
                            FROM " + SLMDBName + @".dbo.kkslm_ms_staff staff
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff_type st ON staff.slm_StaffTypeId = st.slm_StaffTypeId
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_branch branch ON staff.slm_BranchCode = branch.slm_BranchCode
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_department dep ON staff.slm_DepartmentId = dep.slm_DepartmentId 
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_position pos ON staff.slm_Position_id = pos.slm_Position_id ";

            string whr = "";
            whr += (username == "" ? "" : (whr == "" ? "" : " AND ") + " staff.slm_UserName LIKE @UserName ");
            whr += (branchCode == "" ? "" : (whr == "" ? "" : " AND ") + " staff.slm_BranchCode = '" + branchCode + "' ");
            whr += (empCode == "" ? "" : (whr == "" ? "" : " AND ") + " staff.slm_EmpCode LIKE @EmpCode ");
            whr += (marketingCode == "" ? "" : (whr == "" ? "" : " AND ") + " staff.slm_MarketingCode LIKE @MarketingCode ");
            whr += (staffNameTH == "" ? "" : (whr == "" ? "" : " AND ") + " staff.slm_StaffNameTH LIKE @FullName ");
            whr += (positionId == "" ? "" : (whr == "" ? "" : " AND ") + " staff.slm_Position_id = '" + positionId + "' ");
            whr += (staffTypeId == "" ? "" : (whr == "" ? "" : " AND ") + " staff.slm_StaffTypeId = '" + staffTypeId + "' ");
            whr += (team == "" ? "" : (whr == "" ? "" : " AND ") + " staff.slm_Team LIKE @Team ");
            whr += (departmentId == "" ? "" : (whr == "" ? "" : " AND ") + " staff.slm_DepartmentId = '" + departmentId + "' ");

            sql += (whr == "" ? "" : " WHERE " + whr);
            sql += " ORDER BY staff.slm_EmpCode ";

            object[] param = new object[] 
            { 
                new SqlParameter("@UserName", (username != null ? "%" + username + "%" : "")),
                new SqlParameter("@EmpCode", (empCode != null ? "%" + empCode + "%" : "")),
                new SqlParameter("@MarketingCode", (marketingCode != null ? "%" + marketingCode + "%" : "")),
                new SqlParameter("@FullName", (staffNameTH != null ? "%" + staffNameTH + "%" : "")),
                new SqlParameter("@Team", (team != null ? "%" + team + "%" : ""))
            };

            return slmdb.ExecuteStoreQuery<StaffDataManagement>(sql, param).ToList();
        }

        public StaffDataManagement GetStaffDataByEmpcode(string empcode, string dept)
        {
            string sql = @" SELECT staff.slm_StaffId AS StaffId, staff.slm_EmpCode AS EmpCode, staff.slm_MarketingCode AS MarketingCode, staff.slm_UserName AS Username, staff.slm_StaffNameTH AS StaffNameTH
	                            , staff.slm_Position_id AS PositionId, staff.slm_PositionName AS PositionName, staff.slm_StaffTypeId AS StaffTypeId, st.slm_StaffTypeDesc AS StaffTypeDesc, staff.slm_Team AS Team, staff.slm_BranchCode AS BranchCode
	                            , staff.slm_DepartmentId AS DepartmentId, dep.slm_DepartmentName AS DepartmentName, staff.is_Deleted AS Is_Deleted, staff.slm_HeadStaffId AS HeadStaffId
                                , staff.slm_TellNo AS TelNo, staff.slm_StaffEmail As StaffEmail
                            FROM SLMDB.dbo.kkslm_ms_staff staff
                            LEFT JOIN SLMDB.dbo.kkslm_ms_staff_type st ON staff.slm_StaffTypeId = st.slm_StaffTypeId
                            LEFT JOIN SLMDB.dbo.kkslm_ms_department dep ON staff.slm_DepartmentId = dep.slm_DepartmentId 
                            WHERE staff.is_Deleted = 0 AND staff.slm_EmpCode = '" + empcode + "'";

            return slmdb.ExecuteStoreQuery<StaffDataManagement>(sql).FirstOrDefault();

            //if (dept == "")
            //    return null;
            //else
            //{
            //    sql += (dept == "IT" ? "" : " AND staff.slm_DepartmentId = " + dept);
            //    return slmdb.ExecuteStoreQuery<StaffDataManagement>(sql).FirstOrDefault();
            //}
        }

        public string GetEmployeeCode(string username)
        {
            var staff = slmdb.kkslm_ms_staff.Where(p => p.slm_UserName == username).FirstOrDefault();

            if (staff != null)
                return staff.slm_EmpCode != null ? staff.slm_EmpCode.Trim() : string.Empty;
            else
                return string.Empty;
        }

        public StaffData GetStaff(string username)
        {
            string sql = @"SELECT staff.slm_EmpCode AS EmpCode, staff.slm_StaffTypeId AS StaffTypeId, stafftype.slm_StaffTypeDesc AS StaffTypeDesc,staff.slm_StaffId AS StaffId, staff.slm_Collapse AS Collapse
                            , CASE WHEN pos.slm_PositionNameAbb IS NULL THEN staff.slm_StaffNameTH
	                               ELSE pos.slm_PositionNameAbb + ' - ' + staff.slm_StaffNameTH END AS StaffNameTH
                            , chan.slm_ChannelId AS ChannelId, chan.slm_ChannelDesc AS ChannelDesc,staff.slm_BranchCode as BranchCode
                            FROM " + SLMDBName + @".dbo.kkslm_ms_staff staff
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff_type stafftype ON staff.slm_StaffTypeId = stafftype.slm_StaffTypeId
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_position pos ON staff.slm_Position_id = pos.slm_Position_id
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_branch branch ON staff.slm_BranchCode = branch.slm_BranchCode
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_channel chan ON branch.slm_ChannelId = chan.slm_ChannelId
                            WHERE staff.slm_UserName = '" + username + "'";

            return slmdb.ExecuteStoreQuery<StaffData>(sql).FirstOrDefault();
        }

        public StaffData GetDefaultSearch(string username)
        {
            string sql = @"SELECT staff.slm_DefaultSearch as DefaultSearch
                            FROM " + SLMDBName + @".dbo.kkslm_ms_staff staff
                            WHERE staff.slm_UserName = '" + username + "'";

            return slmdb.ExecuteStoreQuery<StaffData>(sql).FirstOrDefault();
        }

        public bool PassPrivilegeCampaign(int flag, string campaignId, string username)
        {
            string sql = "";

            #region Code Old 2015-04-17
            //            sql = @"SELECT Z.*
//                        FROM (
//                        SELECT DISTINCT A.*
//                        FROM
//                        (
//                            SELECT slm_PositionName+' - '+staff.slm_StaffNameTH  AS TextField, staff.slm_UserName AS ValueField,staff.slm_StaffNameTH
//                            FROM " + SLMDBName + @".dbo.kkslm_ms_staff staff
//                            INNER JOIN " + SLMDBName + @".dbo.kkslm_ms_staff_group staffgroup ON staff.slm_StaffId = staffgroup.slm_StaffId
//                            WHERE staffgroup.slm_CampaignId IN ('" + campaignId + @"') AND staff.is_Deleted = 0  AND staff.slm_StaffTypeId <> " + SLMConstant.StaffType.ITAdministrator + @" 
//                                  and staff.slm_BranchCode = '" + branchcode + @"' AND staff.slm_UserName = '" + username + @"'
//                            UNION ALL
//                            SELECT slm_PositionName+' - '+staff.slm_StaffNameTH  AS TextField, staff.slm_UserName AS ValueField,staff.slm_StaffNameTH
//                            FROM " + SLMDBName + @".dbo.kkslm_ms_staff staff
//                            INNER JOIN " + SLMDBName + @".dbo.kkslm_ms_staff_group staffgroup ON staff.slm_StaffId = staffgroup.slm_StaffId
//                            WHERE staff.is_Deleted = 0  AND staff.slm_StaffTypeId = "+ SLMConstant.StaffType.Marketing + @" and staff.slm_BranchCode = '" + branchcode + @"' 
//                            AND staff.slm_UserName = '" + username + @"'    
            //               ) AS A ) AS Z ";
            #endregion

            sql = @"SELECT staff.slm_StaffNameTH AS TextField ,staff.slm_UserName AS ValueField
                    FROM    kkslm_ms_branch branch inner join kkslm_ms_staff staff on staff.slm_BranchCode = branch.slm_BranchCode 
	                        INNER JOIN (
	                                    SELECT DISTINCT Z.slm_BranchCode,Z.slm_StaffTypeId  
                                        FROM (
			                                    SELECT AR.slm_BranchCode,AR.slm_StaffTypeId 
			                                    FROM kkslm_ms_access_right AR INNER JOIN kkslm_ms_campaign CAM ON CAM.slm_CampaignId = AR.slm_CampaignId 
			                                    WHERE CAM.slm_CampaignId = '{1}' 
			                                    UNION ALL
			                                    SELECT AR.slm_BranchCode,AR.slm_StaffTypeId 
			                                    FROM kkslm_ms_access_right AR INNER JOIN CMT_CAMPAIGN_PRODUCT CP ON CP.PR_ProductId = AR.slm_Product_Id
			                                    WHERE CP.PR_ProductId = (SELECT PR_ProductId FROM CMT_CAMPAIGN_PRODUCT 
			                                    WHERE PR_CAMPAIGNID = '{1}')
			                                 ) AS Z
		                              ) AS A ON A.slm_BranchCode = STAFF.slm_BranchCode AND A.slm_StaffTypeId = STAFF.slm_StaffTypeId     
	                    WHERE {0} STAFF.slm_UserName = '{2}' AND STAFF.slm_StaffTypeId NOT IN ({3})               
                    ";

            string wh = "";

            if (flag == SLMConstant.Branch.Active)
            {
                wh = " branch.is_Deleted = 0 AND ";
            }
            else if (flag == SLMConstant.Branch.InActive)
            {
                wh = " branch.is_Deleted = 1 AND";
            }
            else if (flag == SLMConstant.Branch.All)
            {
                wh = "";
            }
            else
                wh = "";

            sql = string.Format(sql, wh, campaignId, username, SLMConstant.StaffType.ITAdministrator);

            var list = slmdb.ExecuteStoreQuery<ControlListData>(sql).ToList();
            if (list.Count > 0)
                return true;
            else
                return false;
        }

        //==================================================== SLM 5 ===============================================================
        public string GetBrachCodeByStaffId(int staffId)
        {
            var branchCode = slmdb.kkslm_ms_staff.Where(p => p.slm_StaffId == staffId).Select(p => p.slm_BranchCode).FirstOrDefault();
            return branchCode != null ? branchCode : "";
        }

        public List<StaffAmountJobOnHand> GetAmountJobOnHandList(string branchCode)
        {
            string sql = @"SELECT staff.slm_UserName AS Username, 
	                            (
		                            SELECT COUNT(*) AS NUM
		                            FROM " + SLMDBName + @".dbo.kkslm_tr_lead lead
		                            WHERE lead.is_Deleted = 0 AND lead.slm_AssignedFlag = '1' AND lead.slm_Status NOT IN ('08','09','10') AND lead.slm_Owner = staff.slm_UserName AND lead.slm_Delegate IS NULL) AS AmountOwner,
	                            (
		                            SELECT COUNT(*) AS NUM
		                            FROM " + SLMDBName + @".dbo.kkslm_tr_lead lead
		                            WHERE lead.is_Deleted = 0 AND lead.slm_Delegate_Flag = '0' AND lead.slm_Status NOT IN ('08','09','10') AND lead.slm_Delegate = staff.slm_UserName) AS AmountDelegate
                            FROM " + SLMDBName + @".dbo.kkslm_ms_staff staff
                            WHERE staff.is_Deleted = 0 AND staff.slm_StaffTypeId <> '" + SLMConstant.StaffType.ITAdministrator.ToString() + "' AND staff.slm_BranchCode = '" + branchCode + "'";

            return slmdb.ExecuteStoreQuery<StaffAmountJobOnHand>(sql).ToList();
        }

        public StaffAmountJobOnHand GetAmountJobOnHand(string username)
        {
            string sql = @"SELECT staff.slm_UserName AS Username, 
                            (
                                SELECT COUNT(*) AS NUM
                                FROM " + SLMDBName + @".dbo.kkslm_tr_lead lead
                                WHERE lead.is_Deleted = 0 AND lead.slm_AssignedFlag = '1' AND lead.slm_Status NOT IN ('08','09','10') AND lead.slm_Owner = staff.slm_UserName AND lead.slm_Delegate IS NULL) AS AmountOwner,
                            (
                                SELECT COUNT(*) AS NUM
                                FROM " + SLMDBName + @".dbo.kkslm_tr_lead lead
                                WHERE lead.is_Deleted = 0 AND lead.slm_Delegate_Flag = '0' AND lead.slm_Status NOT IN ('08','09','10') AND lead.slm_Delegate = staff.slm_UserName) AS AmountDelegate
                        FROM SLMDB.dbo.kkslm_ms_staff staff
                        WHERE staff.slm_UserName = '" + username + "'";

            return slmdb.ExecuteStoreQuery<StaffAmountJobOnHand>(sql).FirstOrDefault();
        }

        public int? GetPositionId(string username)
        {
            return slmdb.kkslm_ms_staff.Where(p => p.slm_UserName == username).Select(p => p.slm_Position_id).FirstOrDefault();
        }

        public List<string> GetRecursiveStaffList(string username)
        {
            KKSlmMsStaffModel sModel = new KKSlmMsStaffModel();
            List<StaffData> staffList = sModel.GetStaffList();

            StaffData sData = sModel.GetStaff(username);
            int? staffId = sData.StaffId;

            List<string> list = new List<string>();
            list.Add(username.ToLower());    //เก็บ login staff

            FindStaffRecusive(staffId, list, staffList);

            return list;
        }

        private void FindStaffRecusive(int? headId, List<string> list, List<StaffData> staffList)
        {
            foreach (StaffData staff in staffList)
            {
                if (staff.HeadStaffId == headId)
                {
                    if (!string.IsNullOrEmpty(staff.UserName)) list.Add(staff.UserName.ToLower());
                    FindStaffRecusive(staff.StaffId, list, staffList);
                }
            }
        }

        public void SetCollapse(string username, bool isCollapse)
        {
            var staff = slmdb.kkslm_ms_staff.Where(p => p.slm_UserName == username).FirstOrDefault();
            if (staff != null)
            {
                staff.slm_Collapse = isCollapse;
                slmdb.SaveChanges();
            }
        }

        public string GetActiveStatusByAvailableConfig(string username)
        {
            string sql = @"SELECT staff.*
                            FROM " + SLMConstant.SLMDBName + @".dbo.kkslm_ms_staff staff
                            WHERE staff.slm_StaffTypeId IN (SELECT slm_StaffTypeId FROM " + SLMConstant.SLMDBName + @".dbo.kkslm_ms_config_available WHERE slm_SetAvailable = '1') 
                            AND staff.slm_UserName = '" + username + "'";

            var staff = slmdb.ExecuteStoreQuery<kkslm_ms_staff>(sql).FirstOrDefault();
            if (staff != null)
                return staff.slm_IsActive.ToString().Trim();
            else
                return "";
        }

        public void SetActiveStatus(string username, int status)
        {
            var staff = slmdb.kkslm_ms_staff.Where(p => p.slm_UserName == username).FirstOrDefault();
            if (staff != null)
            {
                staff.slm_IsActive = status;
                slmdb.SaveChanges();
            }
        }

        public bool CheckEmployeeInPosition(int positionId)
        {
            var count = slmdb.kkslm_ms_staff.Where(p => p.slm_Position_id == positionId && p.is_Deleted == 0).Count();
            return count > 0 ? true : false;
        }
    }
}

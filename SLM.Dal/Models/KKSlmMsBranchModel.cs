using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using SLM.Resource.Data;
using SLM.Resource;


namespace SLM.Dal.Models
{
    public class KKSlmMsBranchModel
    {
        private SLM_DBEntities slmdb = null;

        public KKSlmMsBranchModel()
        {
            slmdb = new SLM_DBEntities();
        }

        /// <summary>
        /// GetBranchList
        /// </summary>
        /// <param name="flag">Flag 1=Active Branch, 2=Inactive Branch, 3=All</param>
        /// <returns></returns>
        public List<ControlListData> GetBranchList(int flag)
        {
            List<ControlListData> list = null;

            if (flag == SLMConstant.Branch.Active)
                list = slmdb.kkslm_ms_branch.Where(p => p.is_Deleted == false).OrderBy(p => p.slm_BranchName).Select(p => new ControlListData { TextField = p.slm_BranchName, ValueField = p.slm_BranchCode }).ToList();
            else if (flag == SLMConstant.Branch.InActive)
                list = slmdb.kkslm_ms_branch.Where(p => p.is_Deleted == true).OrderBy(p => p.slm_BranchName).Select(p => new ControlListData { TextField = p.slm_BranchName, ValueField = p.slm_BranchCode }).ToList();
            else if (flag == SLMConstant.Branch.All)
                list = slmdb.kkslm_ms_branch.OrderBy(p => p.slm_BranchName).Select(p => new ControlListData { TextField = p.slm_BranchName, ValueField = p.slm_BranchCode }).ToList();
            else
                list = new List<ControlListData>();

            return list;
        }


        /// <summary>
        /// GetBranchList
        /// </summary>
        /// <param name="flag">Flag 1=Active Branch, 2=Inactive Branch, 3=All</param>
        /// <returns></returns>
        public List<ControlListData> GetBranchAccessRightList(int flag, string campaignId)
        {
            string sql = @"SELECT branch.slm_BranchName AS TextField, branch.slm_BranchCode AS ValueField 
                            FROM kkslm_ms_branch branch  INNER JOIN (
                                            SELECT DISTINCT Z.slm_BranchCode  
                                            FROM (
                                                    SELECT AR.slm_BranchCode
                                                    FROM kkslm_ms_access_right AR INNER JOIN kkslm_ms_campaign CAM ON CAM.slm_CampaignId = AR.slm_CampaignId 
                                                    WHERE CAM.slm_CampaignId = '{1}' 
                                                    UNION ALL
                                                    SELECT AR.slm_BranchCode
                                                    FROM kkslm_ms_access_right AR INNER JOIN CMT_CAMPAIGN_PRODUCT CP ON CP.PR_ProductId = AR.slm_Product_Id
                                                    WHERE CP.PR_ProductId = (SELECT PR_ProductId FROM CMT_CAMPAIGN_PRODUCT 
													                            WHERE PR_CAMPAIGNID = '{1}')
                                                 ) AS Z
                                          ) AS A ON A.slm_BranchCode = branch.slm_BranchCode
                            {0}  
                            ORDER BY slm_BranchName  ";
            string wh = "";

            if (flag == SLMConstant.Branch.Active)
            {
                wh = " WHERE branch.is_Deleted = 0 ";
            }
            else if (flag == SLMConstant.Branch.InActive)
            {
                wh = " WHERE branch.is_Deleted = 1 ";
            }
            else if (flag == SLMConstant.Branch.All)
            {
                wh = "";
            }
            else
                wh = "";

            sql = string.Format(sql, wh, campaignId);

            return slmdb.ExecuteStoreQuery<ControlListData>(sql).ToList();
        }

        /// <summary>
        /// GetBranchListByRole
        /// </summary>
        /// <param name="flag">Flag 1=Active Branch, 2=Inactive Branch, 3=All</param>
        /// <param name="staffTypeId"></param>
        /// <returns></returns>
        public List<ControlListData> GetBranchListByRole(int flag, string staffTypeId)
        {
            string sql = @"SELECT branch.slm_BranchName AS TextField, branch.slm_BranchCode AS ValueField 
                            FROM " + SLMConstant.SLMDBName + @".dbo.kkslm_ms_branch branch
                            WHERE branch.slm_BranchCode IN (
						                            SELECT DISTINCT slm_BranchCode FROM " + SLMConstant.SLMDBName + @".dbo.kkslm_ms_branch_role br
						                            WHERE br.slm_StaffTypeId = '" + staffTypeId + @"')
                            {0}
                            ORDER BY branch.slm_BranchName ";

            string condition = "";
            if (flag == SLMConstant.Branch.Active)
                condition = " AND branch.is_Deleted = '0' ";
            else if (flag == SLMConstant.Branch.InActive)
                condition = " AND branch.is_Deleted = '1' ";
            else if (flag == SLMConstant.Branch.All)
                condition = "";
            else
                condition = "";

            sql = string.Format(sql, condition);

            return slmdb.ExecuteStoreQuery<ControlListData>(sql).ToList();
        }

        public List<ControlListData> GetBranchData()
        {
            return slmdb.kkslm_ms_branch.Where(p => p.is_Deleted == false).OrderBy(p => p.slm_BranchName).Select(p => new ControlListData { TextField = p.slm_BranchName, ValueField = p.slm_BranchCode }).ToList();
        }

        public string GetBranchName(string branchCode)
        {
            var name = slmdb.kkslm_ms_branch.Where(p => p.slm_BranchCode == branchCode).Select(p => p.slm_BranchName).FirstOrDefault();
            return name != null ? name : "";
        }

        public bool CheckBranchActive(string branchCode)
        {
            var branch = slmdb.kkslm_ms_branch.Where(p => p.slm_BranchCode == branchCode).FirstOrDefault();
            return branch != null ? (branch.is_Deleted ? false : true) : false;
        }

        public string GetChannelStaffData(string username)
        {
            string sql = @" SELECT  BRANCH.slm_ChannelId AS ChannelId
                            FROM    kkslm_ms_staff staff inner join kkslm_ms_branch branch on branch.slm_BranchCode = staff.slm_BranchCode 
                            WHERE   branch.is_Deleted = 0 AND STAFF.is_Deleted = 0 AND STAFF.slm_UserName = '" + username + "'";

            return slmdb.ExecuteStoreQuery<string>(sql).FirstOrDefault();
        }

        public BranchData GetBranch(string branchCode)
        {
            string sql = @"SELECT br.slm_BranchCode AS BranchCode, br.slm_BranchName AS BranchName, slm_StartTime_Hour AS StartTimeHour, slm_StartTime_Minute AS StartTimeMinute
                            , slm_EndTime_Hour AS EndTimeHour, slm_EndTime_Minute AS EndTimeMinute, br.slm_ChannelId AS ChannelId
                            , CASE WHEN br.is_Deleted = '0' THEN 'Y'
                                   WHEN br.is_Deleted = '1' THEN 'N'
                                   ELSE '' END AS [Status]
                            FROM " + SLMConstant.SLMDBName + @".dbo.kkslm_ms_branch br 
                            WHERE br.slm_BranchCode = '" + branchCode + "' ";

            return slmdb.ExecuteStoreQuery<BranchData>(sql).FirstOrDefault();
        }

        public List<BranchData> SearchBranch(string branchCode, string branchName, string channelId, bool statusActive, bool statusInActive)
        {
            string sql = @"SELECT br.slm_BranchCode AS BranchCode, br.slm_BranchName AS BranchName, slm_StartTime_Hour AS StartTimeHour, slm_StartTime_Minute AS StartTimeMinute
                            , slm_EndTime_Hour AS EndTimeHour, slm_EndTime_Minute AS EndTimeMinute, br.slm_ChannelId AS ChannelId, ch.slm_ChannelDesc AS ChannelDesc
                            , CASE WHEN br.is_Deleted = '0' THEN 'ใช้งาน'
                                  WHEN br.is_Deleted = '1' THEN 'ปิดสาขา'
                                  ELSE '' END AS StatusDesc
                            , CASE WHEN br.is_Deleted = '0' THEN 'Y'
                                   WHEN br.is_Deleted = '1' THEN 'N'
                                   ELSE '' END AS [Status]
                            FROM " + SLMConstant.SLMDBName + @".dbo.kkslm_ms_branch br
                            LEFT JOIN " + SLMConstant.SLMDBName + @".dbo.kkslm_ms_channel ch ON br.slm_ChannelId = ch.slm_ChannelId ";

            string whr = "";

            whr += (branchCode == "" ? "" : (whr == "" ? "" : " AND ") + " br.slm_BranchCode LIKE @branchcode ");
            whr += (branchName == "" ? "" : (whr == "" ? "" : " AND ") + " br.slm_BranchName LIKE @branchname ");
            whr += (channelId == "" ? "" : (whr == "" ? "" : " AND ") + " br.slm_ChannelId = '" + channelId + "' ");

            if (statusActive == true && statusInActive == false)
                whr += (whr == "" ? "" : " AND ") + " br.is_Deleted = '0' ";
            else if (statusActive == false && statusInActive == true)
                whr += (whr == "" ? "" : " AND ") + " br.is_Deleted = '1' ";

            if (whr != "")
                sql += " WHERE " + whr;

            sql += " ORDER BY br.slm_BranchCode ";

            object[] param = new object[] 
            { 
                new SqlParameter("@branchcode", "%" + branchCode + "%"),
                new SqlParameter("@branchname", "%" + branchName + "%")
            };

            return slmdb.ExecuteStoreQuery<BranchData>(sql, param).ToList();
        }

        public void InsertData(string branchCode, string branchName, string startTimeHour, string startTimeMin, string endTimeHour, string endTimeMin, string channelId, bool isActive, string createby)
        {
            try
            {
                var count = slmdb.kkslm_ms_branch.Where(p => p.slm_BranchCode == branchCode).Count();
                if (count > 0)
                    throw new Exception("รหัสสาขา " + branchCode + " มีในระบบแล้ว");

                count = slmdb.kkslm_ms_branch.Where(p => p.slm_BranchName == branchName).Count();
                if (count > 0)
                    throw new Exception(branchName + " มีในระบบแล้ว");

                DateTime createdDate = DateTime.Now;
                kkslm_ms_branch branch = new kkslm_ms_branch()
                {
                    slm_BranchCode = branchCode,
                    slm_BranchName = branchName,
                    slm_Branch_CreatedBy = branchName,
                    slm_StartTime_Hour = startTimeHour,
                    slm_StartTime_Minute = startTimeMin,
                    slm_EndTime_Hour = endTimeHour,
                    slm_EndTime_Minute = endTimeMin,
                    slm_ChannelId = (string.IsNullOrEmpty(channelId) ? null : channelId),
                    slm_CreatedBy = createby,
                    slm_CreatedDate = createdDate,
                    slm_UpdatedBy = createby,
                    slm_UpdatedDate = createdDate,
                    is_Deleted = !isActive
                };
                slmdb.kkslm_ms_branch.AddObject(branch);
                slmdb.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateData(string branchCode, string branchName, string startTimeHour, string startTimeMin, string endTimeHour, string endTimeMin, string channelId, bool isActive, string updateBy)
        {
            try
            {
                var count = slmdb.kkslm_ms_branch.Where(p => p.slm_BranchName == branchName && p.slm_BranchCode != branchCode).Count();
                if (count > 0)
                    throw new Exception(branchName + " มีในระบบแล้ว");

                var branch = slmdb.kkslm_ms_branch.Where(p => p.slm_BranchCode == branchCode).FirstOrDefault();
                if (branch != null)
                {
                    branch.slm_BranchName = branchName;
                    branch.slm_Branch_CreatedBy = branchName;
                    branch.slm_StartTime_Hour = startTimeHour;
                    branch.slm_StartTime_Minute = startTimeMin;
                    branch.slm_EndTime_Hour = endTimeHour;
                    branch.slm_EndTime_Minute = endTimeMin;
                    branch.slm_ChannelId = (string.IsNullOrEmpty(channelId) ? null : channelId);
                    branch.slm_UpdatedBy = updateBy;
                    branch.slm_UpdatedDate = DateTime.Now;
                    branch.is_Deleted = !isActive;

                    slmdb.SaveChanges();
                }
                else
                    throw new Exception("ไม่พบรหัสสาขา " + branchCode + " ในระบบ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool CheckEmployeeInBranch(string branchCode)
        {
            var count = slmdb.kkslm_ms_staff.Where(p => p.slm_BranchCode == branchCode && p.is_Deleted == 0).Count();
            return count > 0 ? true : false;
        }
        public bool CheckBranchAccessRightExist(int flag, string campaignId, string branchcode)
        {
            string sql = "";

            sql = @"SELECT branch.slm_BranchName AS TextField, branch.slm_BranchCode AS ValueField 
                    FROM kkslm_ms_branch branch  INNER JOIN (
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
                                  ) AS A ON A.slm_BranchCode = branch.slm_BranchCode
                     WHERE  branch.slm_BranchCode = '{2}' {0}
                    ORDER BY slm_BranchName                 
                    ";

            string wh = "";

            if (flag == SLMConstant.Branch.Active)
            {
                wh = " AND branch.is_Deleted = 0 ";
            }
            else if (flag == SLMConstant.Branch.InActive)
            {
                wh = " AND branch.is_Deleted = 1 ";
            }
            else if (flag == SLMConstant.Branch.All)
            {
                wh = "";
            }
            else
                wh = "";

            sql = string.Format(sql, wh, campaignId,branchcode);

            var list = slmdb.ExecuteStoreQuery<ControlListData>(sql).ToList();
            if (list.Count > 0)
                return true;
            else
                return false;
        }

        public List<ControlListData> GetMonitoringBranchList(int flag, string username)
        {
            string sql = @"SELECT branch.slm_BranchName AS TextField, branch.slm_BranchCode AS ValueField 
                            FROM kkslm_ms_branch branch  
                            WHERE {0} slm_BranchCode IN 
                                (
                                    SELECT DISTINCT Z.slm_BranchCode 
									FROM 
									(
										select slm_BranchCode from kkslm_ms_staff  
										where {2} is_deleted =  0 and slm_HeadStaffId = 
                                        (select slm_StaffId from kkslm_ms_staff where slm_UserName = '{1}')
                                        UNION ALL
										select distinct slm_BranchCode from kkslm_ms_staff  
										where {2} slm_UserName = '{1}') AS Z)";
            string wh = "";

            if (flag == SLMConstant.Branch.Active)
            {
                wh = " branch.is_Deleted = 0 AND ";
            }
            else if (flag == SLMConstant.Branch.InActive)
            {
                wh = " branch.is_Deleted = 1 AND ";
            }
            else if (flag == SLMConstant.Branch.All)
            {
                wh = "";
            }
            else
                wh = "";

            sql = string.Format(sql, wh, username, wh);

            return slmdb.ExecuteStoreQuery<ControlListData>(sql).ToList();
        }
    }
}

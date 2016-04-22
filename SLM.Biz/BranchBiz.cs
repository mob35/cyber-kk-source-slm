using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SLM.Dal.Models;
using SLM.Resource.Data;

namespace SLM.Biz
{
    public class BranchBiz
    {
        public static string GetBranchName(string branchCode)
        {
            KKSlmMsBranchModel branch = new KKSlmMsBranchModel();
            return branch.GetBranchName(branchCode);
        }

        public static bool CheckBranchActive(string branchCode)
        {
            KKSlmMsBranchModel branch = new KKSlmMsBranchModel();
            return branch.CheckBranchActive(branchCode);
        }

        /// <summary>
        /// GetBranchList Flag 1=Active Branch, 2=Inactive Branch, 3=All
        /// </summary>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static List<ControlListData> GetBranchList(int flag)
        {
            KKSlmMsBranchModel branch = new KKSlmMsBranchModel();
            return branch.GetBranchList(flag);
        }

        /// <summary>
        /// GetBranchListByRole
        /// </summary>
        /// <param name="flag">Flag 1=Active Branch, 2=Inactive Branch, 3=All</param>
        /// <param name="staffTypeId"></param>
        /// <returns></returns>
        public static List<ControlListData> GetBranchListByRole(int flag, string staffTypeId)
        {
            KKSlmMsBranchModel branch = new KKSlmMsBranchModel();
            return branch.GetBranchListByRole(flag, staffTypeId);
        }

        public static List<ControlListData> GetBranchList(int flag,string[] branchcode)
        {
            KKSlmMsBranchModel branch = new KKSlmMsBranchModel();
            return branch.GetBranchList(flag);
        }

        public static List<BranchData> SearchBranch(string branchCode, string branchName, string channelId, bool statusActive, bool statusInActive)
        {
            KKSlmMsBranchModel branch = new KKSlmMsBranchModel();
            return branch.SearchBranch(branchCode, branchName, channelId, statusActive, statusInActive);
        }

        public static void InsertData(string branchCode, string branchName, string startTimeHour, string startTimeMin, string endTimeHour, string endTimeMin, string channelId, bool isActive, string createby)
        {
            KKSlmMsBranchModel branch = new KKSlmMsBranchModel();
            branch.InsertData(branchCode, branchName, startTimeHour, startTimeMin, endTimeHour, endTimeMin, channelId, isActive, createby);
        }

        public static void UpdateData(string branchCode, string branchName, string startTimeHour, string startTimeMin, string endTimeHour, string endTimeMin, string channelId, bool isActive, string updateby)
        {
            KKSlmMsBranchModel branch = new KKSlmMsBranchModel();
            branch.UpdateData(branchCode, branchName, startTimeHour, startTimeMin, endTimeHour, endTimeMin, channelId, isActive, updateby);
        }

        public static BranchData GetBranch(string branchCode)
        {
            KKSlmMsBranchModel branch = new KKSlmMsBranchModel();
            return branch.GetBranch(branchCode);
        }

        public static bool CheckEmployeeInBranch(string branchCode)
        {
            KKSlmMsBranchModel branch = new KKSlmMsBranchModel();
            return branch.CheckEmployeeInBranch(branchCode);
        }

        public static List<ControlListData> GetMonitoringBranchList(int flag,string username)
        {
            KKSlmMsBranchModel branch = new KKSlmMsBranchModel();
            return branch.GetMonitoringBranchList(flag, username);
        }
    }
}

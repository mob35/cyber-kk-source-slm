using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SLM.Dal.Models;
using SLM.Resource.Data;

namespace SLM.Biz
{
    public class StaffBiz
    {
        public static string GetBranchCode(int staffId)
        {
            KKSlmMsStaffModel staff = new KKSlmMsStaffModel();
            return staff.GetBrachCodeByStaffId(staffId);
        }

        public static string GetBranchCode(string username)
        {
            KKSlmMsStaffModel staff = new KKSlmMsStaffModel();
            return staff.GetBrachCode(username);
        }

        public static string GetBranchCodeByEmpCode(string empCode)
        {
            KKSlmMsStaffModel staff = new KKSlmMsStaffModel();
            return staff.GetBrachCodeByEmpCode(empCode);
        }

        public static string GetUsernameByEmpCode(string empCode)
        {
            KKSlmMsStaffModel staff = new KKSlmMsStaffModel();
            return staff.GetUsernameByEmpCode(empCode);
        }

        public static List<StaffAmountJobOnHand> GetAmountJobOnHandList(string branchCode)
        {
            KKSlmMsStaffModel staff = new KKSlmMsStaffModel();
            return staff.GetAmountJobOnHandList(branchCode);
        }

        public static List<ControlListData> GetStaffTypeList()
        {
            KKSlmMsStaffTypeModel st = new KKSlmMsStaffTypeModel();
            return st.GetStaffTyeData();
        }

        public static List<ControlListData> GetStaffList(string branchCode)
        {
            KKSlmMsStaffModel staff = new KKSlmMsStaffModel();
            return staff.GetStaffList(branchCode);
        }

        public static List<ControlListData> GetHeadStaffList(string branchCode)
        {
            KKSlmMsStaffModel staff = new KKSlmMsStaffModel();
            return staff.GetHeadStaffList(branchCode);
        }

        public static List<ControlListData> GetStaffAllDataByAccessRight(string campaignId, string branch)
        {
            KKSlmMsStaffModel staff = new KKSlmMsStaffModel();
            return staff.GetStaffAllDataByAccessRight(campaignId, branch);
        }

        public static void SetCollapse(string username, bool isCollapse)
        {
            KKSlmMsStaffModel staff = new KKSlmMsStaffModel();
            staff.SetCollapse(username, isCollapse);
        }

        public static List<string> GetRecursiveStaffList(string username)
        {
            KKSlmMsStaffModel staff = new KKSlmMsStaffModel();
            return staff.GetRecursiveStaffList(username);
        }

        public static string GetActiveStatusByAvailableConfig(string username)
        {
            KKSlmMsStaffModel staff = new KKSlmMsStaffModel();
            return staff.GetActiveStatusByAvailableConfig(username);
        }

        public static void SetActiveStatus(string username, int status)
        {
            KKSlmMsStaffModel staff = new KKSlmMsStaffModel();
            staff.SetActiveStatus(username, status);
        }

        public static bool CheckEmployeeInPosition(int positionId)
        {
            KKSlmMsStaffModel staff = new KKSlmMsStaffModel();
            return staff.CheckEmployeeInPosition(positionId);
        }

        public static List<ControlListData> GetStaffTypeAllList()
        {
            KKSlmMsStaffTypeModel st = new KKSlmMsStaffTypeModel();
            return st.GetStaffTyeAllData();
        }

        public static List<ControlListData> GetStaffBranchAndRecursiveList(string branchCode,string recursivelist)
        {
            KKSlmMsStaffModel staff = new KKSlmMsStaffModel();
            return staff.GetStaffBranchAndRecursiveList(branchCode, recursivelist);
        }

        public static StaffData GetStaff(string username)
        {
            KKSlmMsStaffModel staff = new KKSlmMsStaffModel();
            return staff.GetStaff(username);
        }

        public static StaffData GetDefaultSearch(string username)
        {
            KKSlmMsStaffModel staff = new KKSlmMsStaffModel();
            return staff.GetDefaultSearch(username);
        }
    }
}

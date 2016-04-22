using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SLM.Dal.Models;
using SLM.Resource.Data;
using SLM.Resource;

namespace SLM.Biz
{
    public class SlmScr019Biz
    {
        public static decimal? GetStaffTypeData(string username)
        {
            KKSlmMsStaffModel staff = new KKSlmMsStaffModel();
            return staff.GetStaffType(username);
        }

        public static int? GetDeptData(string username)
        {
            KKSlmMsStaffModel staff = new KKSlmMsStaffModel();
            return staff.GetDeptData(username);
        }

        public static bool CheckUsernameExist(string username,int? staffid)
        {
            KKSlmMsStaffModel staff = new KKSlmMsStaffModel();
            return staff.CheckUsernameExist(username, staffid);
        }

        public static bool CheckEmpCodeExist(string empCode, int? staffid)
        {
            KKSlmMsStaffModel staff = new KKSlmMsStaffModel();
            return staff.CheckEmpCodeExist(empCode, staffid);
        }

        public static bool CheckMarketingCodeExist(string marketingCode, int? staffid)
        {
            KKSlmMsStaffModel staff = new KKSlmMsStaffModel();
            return staff.CheckMarketingCodeExist(marketingCode, staffid);
        }

        public static string InsertStaff(StaffDataManagement data, string username)
        {
            KKSlmMsStaffModel staff = new KKSlmMsStaffModel();
            return staff.InsertStaff(data, username);
        }

        public static string UpdateStaff(StaffDataManagement data, string username,int flag)
        {
            KKSlmMsStaffModel staff = new KKSlmMsStaffModel();
            return staff.UpdateStaff(data, username,flag);
        }

        public static List<ControlListData> GetStaffHeadData(string branch)
        {
            KKSlmMsStaffModel staff = new KKSlmMsStaffModel();
            return staff.GetStaffHeadData(branch);
        }
    }
}

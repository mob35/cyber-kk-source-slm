using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SLM.Dal.Models;
using SLM.Resource.Data;

namespace SLM.Biz
{
    public class SlmScr015Biz
    {
        public static string GetNumOfUnassignLead(string username,decimal? stafftype)
        {
            SearchLeadModel search = new SearchLeadModel();
            return search.GetNumOfUnassignLead(username, stafftype);
        }

        public static List<SearchLeadResult> GetUnassignLeadList(string username)
        {
            SearchLeadModel search = new SearchLeadModel();
            return search.GetUnassignLeadList(username);
        }

        public static List<UserMonitoringData> SearchUserMonitoring(string userList, string active, string assignDateFrom, string assignDateTo)
        {
            SearchLeadModel search = new SearchLeadModel();
            return search.SearchUserMonitoring(userList, active, assignDateFrom, assignDateTo);
        }

        public static List<UserMonitoringMKTData> SearchUserMonitoringMKT(string userList,string productId,string campaign,string branchcode, string active, string assignDateFrom, string assignDateTo)
        {
            SearchLeadModel search = new SearchLeadModel();
            return search.SearchUserMonitoringMKT(userList, productId, campaign, branchcode, active, assignDateFrom, assignDateTo);
        }

        public static List<SearchLeadResult> SearchUserMonitoringList(string username)
        {
            SearchLeadModel search = new SearchLeadModel();
            return search.GetUserMonitoringList(username);
        }

        public static List<SearchLeadResult> GetUserMonitoringMKTListByUser(string productId, string campaign, string branchcode, string assignDateFrom, string assignDateTo, string username, string statuscode)
        {
            SearchLeadModel search = new SearchLeadModel();
            return search.GetUserMonitoringMKTListByUser(productId, campaign, branchcode, assignDateFrom, assignDateTo, username,statuscode);
        }
        public static List<StaffData> GetStaffList()
        {
            KKSlmMsStaffModel staffmodel = new KKSlmMsStaffModel();
            return staffmodel.GetStaffList();
        }

        public static string GetStaffId(string username)
        {
            KKSlmMsStaffModel staffmodel = new KKSlmMsStaffModel();
            return staffmodel.GetStaffIdData(username);
        }

        public static decimal? GetStaffType(string username)
        {
            KKSlmMsStaffModel staffmodel = new KKSlmMsStaffModel();
            return staffmodel.GetStaffType(username);
        }

        public static List<ControlListData> GetProductList()
        {
            CmtMappingProductModel product = new CmtMappingProductModel();
            return product.GetProductList();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SLM.Dal.Models;
using SLM.Resource.Data;

namespace SLM.Biz
{
    public class UploadLeadBiz
    {
        ////Reference UploadLeadBiz.cs


        public static List<SearchUploadLeadResult> SearchUploadLeadData(SearchUploadLeadCondition suld)
        {
            KKSlmTrUploadLeadModel search = new KKSlmTrUploadLeadModel();
            //string createDate = data.CreatedDate.Year != 1 ? data.CreatedDate.Year + data.CreatedDate.ToString("-MM-dd") : string.Empty;
            //string assignDate = data.AssignedDate.Year != 1 ? data.AssignedDate.Year + data.AssignedDate.ToString("-MM-dd") : string.Empty;

            return search.SearchUploadLeadData(suld);
        }

        public static UploadLeadData SearchUploadLeadData(string UploadLeadId)
        {
            KKSlmTrUploadLeadModel search = new KKSlmTrUploadLeadModel();
            //string createDate = data.CreatedDate.Year != 1 ? data.CreatedDate.Year + data.CreatedDate.ToString("-MM-dd") : string.Empty;
            //string assignDate = data.AssignedDate.Year != 1 ? data.AssignedDate.Year + data.AssignedDate.ToString("-MM-dd") : string.Empty;

            return search.SearchUploadLeadData(UploadLeadId);
        }

        public static List<UploadLeadDetailData> GetUploadLeadDetailList(string UploadLeadId)
        {
            KKSlmTrUploadLeadModel search = new KKSlmTrUploadLeadModel();

            return search.GetUploadLeadDetailList(UploadLeadId);
        }


        public static int AddUploadLead(UploadLeadData UploadLead, List<UploadLeadDetailData> UploadLeadDetail, string username)
        {
            KKSlmTrUploadLeadModel search = new KKSlmTrUploadLeadModel();
            return search.AddUploadLead(UploadLead, UploadLeadDetail, username);
        }

        public static void EditUploadLead(UploadLeadData UploadLead, List<UploadLeadDetailData> UploadLeadDetail, string username)
        {
            KKSlmTrUploadLeadModel search = new KKSlmTrUploadLeadModel();
            search.EditUploadLead(UploadLead, UploadLeadDetail, username);
        }

        

        public static void DeleteUploadLead(int? UploadLeadID ,string username) {
            KKSlmTrUploadLeadModel search = new KKSlmTrUploadLeadModel();
            search.DeleteUploadLead(UploadLeadID ,username);
        }

        public static bool CheckFileExist(string Filename, int? UploadLeadId)
        {
            //
            KKSlmTrUploadLeadModel search = new KKSlmTrUploadLeadModel();

            return search.CheckFileExist(Filename, UploadLeadId);

            
        }

        public static List<ControlListData> GetStatusList()
        {
            List<ControlListData> cds = new List<ControlListData>();

            ControlListData cd1 = new ControlListData();
            cd1.ValueField = "Complete";
            cd1.TextField = "Complete";

            ControlListData cd2 = new ControlListData();
            cd2.ValueField = "Submit";
            cd2.TextField = "Submit";

            cds.Add(cd1);
            cds.Add(cd2);

            return cds;
        }
        
    }
}

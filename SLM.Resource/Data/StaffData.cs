using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLM.Resource.Data
{
    public class StaffData
    {
        public string StaffNameTH { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public int? StaffId { get; set; }
        public decimal? StaffTypeId { get; set; }
        public string StaffTypeDesc { get; set; }
        public string EmpCode { get; set; }
        public string  UserName { get; set; }
        public int? HeadStaffId { get; set; }
        public string ChannelId { get; set; }
        public string  ChannelDesc { get; set; }
        public bool? Collapse { get; set; }
        public int? PositionId { get; set; }
        public string DefaultSearch { get; set; }
    }

    public class StaffDataManagement
    {
        public int? StaffId { get; set; }
        public string Username { get; set; }
        public string EmpCode { get; set; }
        public string MarketingCode { get; set; }
        public string StaffNameTH { get; set; }
        public string TelNo { get; set; }
        public string StaffEmail { get; set; }
        public int? PositionId { get; set; }
        public string PositionName { get; set; }
        public decimal? StaffTypeId { get; set; }
        public string StaffTypeDesc { get; set; }
        public string Team { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public int? HeadStaffId { get; set; }
        public int? DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public decimal? Is_Deleted { get; set; }
        public int? PageIndex { get; set; }
        public DateTime? UpdateStatusDate { get; set; }
        public string UpdateStatusBy { get; set; }
    }

    public class StaffAmountJobOnHand
    {
        public string Username { get; set; }
        public int? AmountOwner { get; set; }
        public int? AmountDelegate { get; set; }
    }


    public class CheckStaffData {
        public string Staff { get; set; }
    }
}

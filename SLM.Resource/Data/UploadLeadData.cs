using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;

namespace SLM.Resource.Data
{
    [Serializable]
    public class UploadLeadData
    {
        public int slm_UploadLeadId { get; set; }
        public string slm_FileName { get; set; }
        public string slm_CampaignCode { get; set; }
        public int? slm_LeadCount { get; set; }
        public int? slm_LeadAssignedCount { get; set; }
        public string slm_Status { get; set; }
        public DateTime? slm_AssignedDate { get; set; }
        public int? Is_Deleted { get; set; }
        public string slm_UpdatedBy { get; set; }
        public DateTime slm_UpdatedDate { get; set; }
        public string slm_CreatedBy { get; set; }
        public DateTime slm_CreatedDate { get; set; }

    }
    [Serializable]
    public class UploadLeadDetailData
    {
        public int slm_UploadLeadDetailId{ get; set; }
      public int slm_UploadLeadId{ get; set; }
      public string slm_Remark{ get; set; }
      public string slm_AssignStatus{ get; set; }
      public string slm_AssignRemark{ get; set; }
      public string slm_UpdatedBy{ get; set; }
      public DateTime? slm_UpdatedDate { get; set; }
      public string slm_CreatedBy{ get; set; }
      public DateTime? slm_CreatedDate{ get; set; }
      public string slm_ContractBranchName{ get; set; }
      public string slm_OwnerLead{ get; set; }
      public string slm_ThaiFirstName{ get; set; }
      public string slm_ThaiLastName{ get; set; }
      public string slm_CardIdType{ get; set; }
      public string slm_CardId{ get; set; }
      public string slm_CustTelephoneMobile{ get; set; }
      public string slm_CustTelephoneHome{ get; set; }
      public string slm_CustTelephoneOther{ get; set; }
      public string slm_BrandName{ get; set; }
      public string slm_ModelName{ get; set; }
      public string slm_ModelYear{ get; set; }
      public string slm_HpBscodeXsell{ get; set; }
      public string slm_TicketID { get; set; }
       
    }

    [Serializable]
    public class SearchUploadLeadCondition
    {

        public string slm_FileName { get; set; }
        public string slm_Status { get; set; }

        public string UploadedDateForm { get; set; }
        public string UploadedDateTo { get; set; }
    }


    [Serializable]
    public class SearchUploadLeadResult
    {
        public int slm_UploadLeadId { get; set; }
        public string slm_FileName { get; set; }
        public int? slm_LeadCount { get; set; }
        public int? slm_LeadAssignedCount { get; set; }
        public string slm_Status { get; set; }
        public DateTime? slm_AssignedDate { get; set; }
        public string slm_UpdatedBy { get; set; }
        public DateTime? slm_UpdatedDate { get; set; }
        public string slm_CreatedBy { get; set; }
        public DateTime? slm_CreatedDate { get; set; }
    }

}

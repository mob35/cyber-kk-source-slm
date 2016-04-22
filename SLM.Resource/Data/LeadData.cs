using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLM.Resource.Data
{
    public class LeadData
    {
        public string TicketId { get; set; }
        public string ChannelId { get; set; }

        //**************************************TableName : kkslm_tr_lead **********************************************
        public string Name { get; set; }
        public string TelNo_1 { get; set; }
        public string Ext_1 { get; set; }
        public string CampaignId { get; set; }
        public string Owner { get; set; }
        public string Delegate { get; set; }
        public string Status { get; set; }
        public DateTime? StatusDate { get; set; }
        public string StatusBy { get; set; }
        public DateTime? AssignedDate { get; set; }
        public string AssignedBy { get; set; }
        public string AvailableTime { get; set; }
        public string AssignedFlag { get; set; }
        public int? StaffId { get; set; }
        public decimal? counting { get; set; }
        public string EmailFlag { get; set; }
        public string SmsFlag { get; set; }
        public DateTime? LeadCreateDate { get; set; }
        public decimal? Delegate_Flag { get; set; }
        public string NoteFlag { get; set; }
        public DateTime? Delegate_Date { get; set; }
        public string DealerCode { get; set; }
        public string DealerName { get; set; }
        public string Reference { get; set; }
        public string ProductGroupId { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string slmOldOwner { get; set; }
        public bool HasAdamsUrl { get; set; }
        public string CalculatorUrl { get; set; }
        public string AppNo { get; set; }
        public string ContractNoRefer { get; set; }
        public string ExternalSubStatusDesc { get; set; }

        //**************************************TableName : kkslm_tr_cusinfo **********************************************
        public string CusInfo_Id { get; set; }
        public string CusCode { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string TelNo_2 { get; set; }
        public string TelNo_3 { get; set; }
        public string Ext_2 { get; set; }
        public string Ext_3 { get; set; }
        public string BuildingName { get; set; }
        public string AddressNo { get; set; }
        public string Floor { get; set; }
        public string Soi { get; set; }
        public string Street { get; set; }
        public int? Tambon { get; set; }
        public int? Amphur { get; set; }
        public int? Province { get; set; }
        public string PostalCode { get; set; }
        public int? Occupation { get; set; }
        public string OccupationCode { get; set; }
        public decimal? BaseSalary { get; set; }
        public string IsCustomer { get; set; }
        public DateTime? Birthdate { get; set; }
        public int? CardType { get; set; }
        public string CardTypeDesc { get; set; }
        public string CitizenId { get; set; }
        public string Topic { get; set; }
        public string Detail { get; set; }
        public string Note { get; set; }
        public string PathLink { get; set; }
        public string TelesaleId { get; set; }
        public string TelesaleName { get; set; }
        public string ContactTime { get; set; }
        public string ContactBranch { get; set; }

        //**************************************TableName : kkslm_tr_channelinfo **********************************************
        public string ChannelInfo_Id { get; set; }
        public string IPAddress { get; set; }
        public string Company { get; set; }
        public string Branch { get; set; }
        public string BranchNo { get; set; }
        public string MachineNo { get; set; }
        public string ClientServiceType { get; set; }
        public string DocumentNo { get; set; }
        public string CommPaidCode { get; set; }
        public string Zone { get; set; }
        public DateTime? RequestDate { get; set; }
        public string RequestTime { get; set; }
        public string RequestBy { get; set; }
        public DateTime? ResponseDate { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public string TransId { get; set; }

        //**************************************TableName : kkslm_tr_productinfo **********************************************
        public string ProductInfo_Id { get; set; }
        public string InterestedProd { get; set; }
        public string LicenseNo { get; set; }
        public string YearOfCar { get; set; }
        public string YearOfCarRegis { get; set; }
        public int? Brand { get; set; }
        public int? Model { get; set; }
        public string ModelFamily { get; set; }
        public int? Submodel { get; set; }
        public string SubModelCode { get; set; }
        public decimal? DownPayment { get; set; }
        public decimal? DownPercent { get; set; }
        public decimal? CarPrice { get; set; }
        public decimal? FinanceAmt { get; set; }
        public string PaymentTerm { get; set; }
        public string PaymentType { get; set; }
        public string PaymentTypeCode { get; set; }
        public decimal? BalloonAmt { get; set; }
        public decimal? BalloonPercent { get; set; }
        public string CoverageDate { get; set; }
        public int? ProvinceRegis { get; set; }
        public string PlanType { get; set; }
        public int? AccType { get; set; }
        public string AccTypeCode { get; set; }
        public int? AccPromotion { get; set; }
        public string AccPromotionCode { get; set; }
        public string AccTerm { get; set; }
        public string Interest { get; set; }
        public string Invest { get; set; }
        public string LoanOd { get; set; }
        public string LoanOdTerm { get; set; }
        public string Ebank { get; set; }
        public string Atm { get; set; }
        public string OtherDetail_1 { get; set; }
        public string OtherDetail_2 { get; set; }
        public string OtherDetail_3 { get; set; }
        public string OtherDetail_4 { get; set; }
        public string CarType { get; set; }

        //**************************************TableName : kkslm_tr_activity **********************************************
        public string  NewOwner { get; set; }
        public string NewStatus { get; set; }
        public string OldOwner { get; set; }
        public string OldStatus { get; set; }
        public string Type { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string WorkId { get; set; }
        public string WorkDesc { get; set; }

        //**************************************TableName : kkslm_tr_activity 2 (ใช้กรณีมีการเปลี่ยน owner ที่หน้าจอ Edit Lead********************************
        public string NewOwner2 { get; set; }
        public string OldOwner2 { get; set; }
        public string Type2 { get; set; }
        public string CreatedBy2 { get; set; }
        public DateTime? CreatedDate2 { get; set; }
        public string UpdatedBy2 { get; set; }
        public DateTime? UpdatedDate2 { get; set; }
        public string WorkId2 { get; set; }
        public string WorkDesc2 { get; set; }
        //************************************** View Data **********************************************
        public string  StatusName { get; set; }
        public string CampaignName { get; set; }
        public string ChannelDesc { get; set; }
        public string BranchName { get; set; }
        public string OwnerName { get; set; }
        public string DelegateName { get; set; }
        public DateTime? CreatedDateView { get; set; }
        public DateTime? AssignedDateView { get; set; }
        public string TambolName { get; set; }
        public string AmphurName { get; set; }
        public string ProvinceName { get; set; }
        public string OccupationName { get; set; }
        public string BrandName { get; set; }
        public string ModelName { get; set; }
        public string SubModelName { get; set; }
        public string ProvinceRegisName { get; set; }
        public string AccTypeName { get; set; }
        public string PromotionName { get; set; }
        public DateTime? ContactLatestDate { get; set; }
        public DateTime? ContactFirstDate { get; set; }
        public string Branchprod { get; set; }
        public string ProvinceCode { get; set; }
        public string AmphurCode { get; set; }
        public string TambolCode { get; set; }
        public string ProvinceRegisCode { get; set; }
        public string  BrandCode { get; set; }
        public string Family { get; set; }
        public string DelegatebranchName { get; set; }
        public string PaymentName { get; set; }
        public string  PlanBancName { get; set; }
        public string  LeadCreateBy { get; set; }
        //public string DelegateOwnerBranch {get; set; }
        public string  Description { get; set; }
        public string OldDelegate { get; set; }
        public string  NewDelegate { get; set; }
        public string Owner_Branch { get; set; }
        public string Delegate_Branch { get; set; }
        public string CreatedBy_Branch { get; set; }
        public int? CreatedByPositionId { get; set; }
        public string OwnerBranchName { get; set; }
        public int? OwnerPositionId { get; set; }

        //COC
        public string ISCOC { get; set; }
        public string COCCurrentTeam { get; set; }
        public string MarketingOwnerName { get; set; }
        public string LastOwnerName { get; set; }
        public string CocStatusDesc { get; set; }
        public DateTime? CocAssignedDate { get; set; }
    }

    public class LeadOwnerDelegateData
    {
        public string TicketId { get; set; }
        public string OwnerName { get; set; }
        public string DelegateName { get; set; }
    }

    public class EmailTemplateData
    {
        public string TicketId { get; set; }
        public string CampaignName { get; set; }
        public string Channel { get; set; }
        public string StatusDesc { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string OwnerName { get; set; }
        public DateTime? AssignedDate { get; set; }
        public string DelegateName { get; set; }
        public DateTime? DelegateDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string AvailableTime { get; set; }
        public string ProductGroupName { get; set; }
        public string ProductName { get; set; }
        public string TelNo1 { get; set; }
        public string LicenseNo { get; set; }
        public string CocStatusDesc { get; set; }
        public string MarketingOwnerName { get; set; }
        public string LastOwnerName { get; set; }
    }

    public class LeadDataForAdam
    {
        //LeadInfo
        public string TicketId { get; set; }
        public string Firstname { get; set; }
        public string TelNo1 { get; set; }
        public string Campaign { get; set; }
        public string ProductGroupId { get; set; }
        public string ProductId { get; set; }

        //CustomerInfo
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string TelNo2 { get; set; }
        public string TelNo3 { get; set; }
        public string ExtNo1 { get; set; }
        public string ExtNo2 { get; set; }
        public string ExtNo3 { get; set; }
        public string BuildingName { get; set; }
        public string AddrNo { get; set; }
        public string Floor { get; set; }
        public string Soi { get; set; }
        public string Street { get; set; }
        public int? Tambol { get; set; }
        public string TambolCode { get; set; }
        public int? Amphur { get; set; }
        public string AmphurCode { get; set; }
        public int? Province { get; set; }
        public string ProvinceCode { get; set; }
        public string PostalCode { get; set; }
        public int? Occupation { get; set; }
        public string OccupationCode { get; set; }
        public decimal? BaseSalary { get; set; }
        public string IsCustomer { get; set; }
        public string CustomerCode { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Cid { get; set; }
        public string Status { get; set; }
        public string StatusDesc { get; set; }

        //CustomerDetail
        public string Topic { get; set; }
        public string Detail { get; set; }
        public string PathLink { get; set; }
        public string TelesaleName { get; set; }
        public string AvailableTime { get; set; }
        public string ContactBranch { get; set; }

        //ProductInfo
        public string InterestedProdAndType { get; set; }
        public string LicenseNo { get; set; }
        public string YearOfCar { get; set; }
        public string YearOfCarRegis { get; set; }
        public int? RegisterProvince { get; set; }          //ProvinceId
        public string RegisterProvinceCode { get; set; }    //ProvinceCode
        public int? Brand { get; set; }
        public string BrandCode { get; set; }
        public int? Model { get; set; }
        public string ModelFamily { get; set; }
        public int? Submodel { get; set; }
        public string SubmodelRedBookNo { get; set; }
        public decimal? DownPayment { get; set; }
        public decimal? DownPercent { get; set; }
        public decimal? CarPrice { get; set; }
        public string CarType { get; set; }
        public decimal? FinanceAmt { get; set; }
        public string Term { get; set; }
        public string PaymentType { get; set; }
        public string PaymentTypeCode { get; set; }
        public decimal? BalloonAmt { get; set; }
        public decimal? BalloonPercent { get; set; }
        public string Plantype { get; set; }
        public string CoverageDate { get; set; }
        public int? AccType { get; set; }
        public string AccTypeCode { get; set; }
        public int? AccPromotion { get; set; }
        public string AccPromotionCode { get; set; }
        public string AccTerm { get; set; }
        public string Interest { get; set; }
        public string Invest { get; set; }
        public string LoanOd { get; set; }
        public string LoanOdTerm { get; set; }
        public string SlmBank { get; set; }
        public string SlmAtm { get; set; }
        public string OtherDetail1 { get; set; }
        public string OtherDetail2 { get; set; }
        public string OtherDetail3 { get; set; }
        public string OtherDetail4 { get; set; }

        //ChannelInfo
        public string ChannelId { get; set; }
        public DateTime? RequestDate { get; set; }
        public string Time { get; set; }
        public string CreateUser { get; set; }
        public string Ipaddress { get; set; }
        public string Company { get; set; }
        public string Branch { get; set; }
        public string BranchNo { get; set; }
        public string MachineNo { get; set; }
        public string ClientServiceType { get; set; }
        public string DocumentNo { get; set; }
        public string CommPaidCode { get; set; }
        public string Zone { get; set; }
        public string TransId { get; set; }

        //Others
        public string AdamsUrl { get; set; }
    }

    public class LeadDataPhoneCallHistory
    {
        public string TicketId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string CampaignId { get; set; }
        public string CampaignName { get; set; }
        public string OwnerBranch { get; set; }
        public string Owner { get; set; }
        public string DelegateBranch { get; set; }
        public string Delegate { get; set; }
        public string TelNo1 { get; set; }
        public string LeadStatus { get; set; }
        public int? CardType { get; set; }
        public string CitizenId { get; set; }
        public string AssignedFlag { get; set; }
        public decimal? DelegateFlag { get; set; }
        public string ProductId { get; set; }
    }
}

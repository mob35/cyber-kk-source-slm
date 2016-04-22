using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SLM.Resource.Data;
using SLM.Dal.Models;
using SLM.Dal;
using System.Transactions;
using SLM.Resource;

namespace SLM.Biz
{
    public class LeadInfoBiz
    {
        private string _Error = "";
        public string ErrorMessage
        {
            get { return _Error; }
        }

        private Dictionary<string, string> _errList = new Dictionary<string, string>();
        public Dictionary<string, string> ErrorList
        {
            get { return _errList; }
        }

        public static string InsertLeadData(LeadData leadData,CampaignWSData camData, string createbyUsername)
        {
            string ticketId = "";
            try
            {
                StoreProcedure store = new StoreProcedure();
                ticketId = store.GenerateTicketId();
                leadData.TicketId = ticketId;
                camData.TicketId = ticketId;

                DateTime createDate = DateTime.Now;

                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
                {
                    KKSlmTrLeadModel lead = new KKSlmTrLeadModel();
                    lead.InsertData(leadData, createbyUsername, createDate);

                    KKSlmTrCusInfoModel customerInfo = new KKSlmTrCusInfoModel();
                    customerInfo.InsertData(leadData, createbyUsername, createDate);

                    KKSlmTrProductInfoModel productInfo = new KKSlmTrProductInfoModel();
                    productInfo.InsertData(leadData, createbyUsername);

                    KKSlmTrChannelInfoModel channelInfo = new KKSlmTrChannelInfoModel();
                    channelInfo.InsertData(leadData, createbyUsername, createDate);

                    KKSLMTrCampaignFinalModel camFinal = new KKSLMTrCampaignFinalModel();
                    camFinal.InsertData(camData, createbyUsername, createDate);

                    KKSlmTrHistoryModel history = new KKSlmTrHistoryModel();
                    history.InsertData(ticketId, SLMConstant.HistoryTypeCode.CreateLead, "", "", createbyUsername, createDate);

                    ts.Complete();
                }
                return ticketId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string UpdateLeadData(LeadData leadData, string username, bool actStatus, bool actDelegate, bool actOwner)
        {
            try
            {
                DateTime updateDate = DateTime.Now;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
                {
                    KKSlmTrLeadModel lead = new KKSlmTrLeadModel();
                    lead.UpdateData(leadData, username, actDelegate, actOwner, updateDate);

                    KKSlmTrCusInfoModel customerInfo = new KKSlmTrCusInfoModel();
                    customerInfo.UpdateData(leadData, username, updateDate);

                    KKSlmTrProductInfoModel productInfo = new KKSlmTrProductInfoModel();
                    productInfo.UpdateData(leadData, username);

                    KKSlmTrChannelInfoModel channelInfo = new KKSlmTrChannelInfoModel();
                    channelInfo.UpdateData(leadData, username);

                    if (actStatus == true || actDelegate == true)
                    {
                        KKSlmTrActivityModel Activity = new KKSlmTrActivityModel();
                        Activity.InsertData(leadData, username, updateDate);

                        KKSlmTrHistoryModel historydel = new KKSlmTrHistoryModel();
                        historydel.InsertData(leadData.TicketId, SLMConstant.HistoryTypeCode.UpdateDelegate, leadData.OldDelegate, leadData.NewDelegate, username, updateDate);
                    }

                    if (actOwner == true)
                    {
                        KKSlmTrActivityModel Activity = new KKSlmTrActivityModel();
                        Activity.InsertDataChangeOwner(leadData, username, updateDate);

                        KKSlmTrHistoryModel historydel = new KKSlmTrHistoryModel();
                        historydel.InsertData(leadData.TicketId, SLMConstant.HistoryTypeCode.UpdateOwner, leadData.OldOwner2, leadData.NewOwner2, username, updateDate);
                    }

                    KKSlmTrHistoryModel history = new KKSlmTrHistoryModel();
                    history.InsertData(leadData.TicketId, SLMConstant.HistoryTypeCode.UpdateLead, "", "", username, updateDate);

                    ts.Complete();
                }
                return leadData.TicketId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public bool InsertLeadData(LeadData leadData, List<CampaignWSData> camDataList, string createbyUsername)
        //{
        //    string ticketId = "";
        //    try
        //    {
        //        StoreProcedure store = new StoreProcedure();
        //        ticketId = store.GenerateTicketId();
        //        leadData.TicketId = ticketId;

        //        foreach (CampaignWSData cpdata in camDataList)
        //        {
        //            cpdata.TicketId = ticketId;
        //        }

        //        using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
        //        {
        //            KKSlmTrLeadModel lead = new KKSlmTrLeadModel();
        //            lead.InsertData(leadData, createbyUsername);

        //            KKSlmTrCusInfoModel customerInfo = new KKSlmTrCusInfoModel();
        //            customerInfo.InsertData(leadData, createbyUsername);

        //            KKSlmTrProductInfoModel productInfo = new KKSlmTrProductInfoModel();
        //            productInfo.InsertData(leadData, createbyUsername);

        //            KKSlmTrChannelInfoModel channelInfo = new KKSlmTrChannelInfoModel();
        //            channelInfo.InsertData(leadData, createbyUsername);

        //            KKSLMTrCampaignFinalModel camFinal = new KKSLMTrCampaignFinalModel();
        //            camFinal.InsertCampaignList(camDataList, createbyUsername);

        //            ts.Complete();
        //        }
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        _Error = ex.Message.ToString();
        //        return false;
        //    }
        //}

        public string InsertLeadSuggestCampaign(LeadData leadData, CampaignWSData cpdata, string createByUsername)
        {
            string ticketId = "";
            try
            {
                DateTime createDate = DateTime.Now;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
                {
                    StoreProcedure store = new StoreProcedure();
                    ticketId = store.GenerateTicketId();
                    leadData.TicketId = ticketId;
                    cpdata.TicketId = ticketId;

                    List<ProductData> prodList = SlmScr016Biz.GetProductCampaignDataForSuggestCampaign(cpdata.CampaignId);
                    if (prodList.Count > 0)
                    {
                        leadData.ProductGroupId = prodList[0].ProductGroupId;
                        leadData.ProductId = prodList[0].ProductId;
                        leadData.ProductName = prodList[0].ProductName;
                    }                   

                    KKSlmTrLeadModel lead = new KKSlmTrLeadModel();
                    lead.InsertData(leadData, createByUsername, createDate);

                    KKSlmTrCusInfoModel customerInfo = new KKSlmTrCusInfoModel();
                    customerInfo.InsertData(leadData, createByUsername, createDate);

                    KKSlmTrProductInfoModel productInfo = new KKSlmTrProductInfoModel();
                    productInfo.InsertData(leadData, createByUsername);

                    KKSlmTrChannelInfoModel channelInfo = new KKSlmTrChannelInfoModel();
                    channelInfo.InsertData(leadData, createByUsername, createDate);

                    KKSLMTrCampaignFinalModel camFinal = new KKSLMTrCampaignFinalModel();
                    camFinal.InsertData(cpdata, createByUsername, createDate);

                    KKSlmTrHistoryModel history = new KKSlmTrHistoryModel();
                    history.InsertData(ticketId, SLMConstant.HistoryTypeCode.CreateLead, "", "", createByUsername, createDate);

                    ts.Complete();
                }
                return ticketId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public bool InsertLeadData2(LeadData leadData, List<CampaignWSData> camDataList, string UserId, List<string> ticketIdList)
        //{
        //    string ticketId = "";
        //    try
        //    {
        //        using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
        //        {
        //            foreach (CampaignWSData cpdata in camDataList)
        //            {
        //                StoreProcedure store = new StoreProcedure();
        //                ticketId = store.GenerateTicketId();
        //                leadData.TicketId = ticketId;
        //                ticketIdList.Add(ticketId);

        //                List<ProductData> prodList = SlmScr016Biz.GetProductCampaignDataForSuggestCampaign(cpdata.CampaignId);
        //                if (prodList.Count > 0)
        //                {
        //                    leadData.ProductGroupId = prodList[0].ProductGroupId;
        //                    leadData.ProductId = prodList[0].ProductId;
        //                    leadData.ProductName = prodList[0].ProductName;
        //                }

        //                leadData.CampaignId = cpdata.CampaignId;
        //                cpdata.TicketId = ticketId;

        //                KKSlmTrLeadModel lead = new KKSlmTrLeadModel();
        //                lead.InsertData(leadData, UserId);

        //                KKSlmTrCusInfoModel customerInfo = new KKSlmTrCusInfoModel();
        //                customerInfo.InsertData(leadData, UserId);

        //                KKSlmTrProductInfoModel productInfo = new KKSlmTrProductInfoModel();
        //                productInfo.InsertData(leadData, UserId);

        //                KKSlmTrChannelInfoModel channelInfo = new KKSlmTrChannelInfoModel();
        //                channelInfo.InsertData(leadData, UserId);

        //                KKSLMTrCampaignFinalModel camFinal = new KKSLMTrCampaignFinalModel();
        //                camFinal.InsertData(cpdata, UserId);
        //            }

        //            ts.Complete();
        //        }
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        _Error = ex.Message.ToString();
        //        return false;
        //    }
        //}

        public static int? GetProvinceId(string provinceCode)
        {
            KKSlmMsProvinceModel province = new KKSlmMsProvinceModel();
            return province.GetProvinceId(provinceCode);
        }

        public static int? GetAmphurId(string provinceCode, string amphurCode)
        {
            KKSlmMsAmphurModel amphur = new KKSlmMsAmphurModel();
            return amphur.GetAmphurId(provinceCode, amphurCode);
        }

        public static int? GetBrandId(string BrandCode)
        {
            KKSlmMsBrandModel brand = new KKSlmMsBrandModel();
            return brand.GetBrandId(BrandCode);
        }
        public static int? GetModelId(string BrandCode, string Family)
        {
            KKSlmMsModelModel model = new KKSlmMsModelModel();
            return model.GetModelId(BrandCode, Family);
        }
        public static string GetChannelId(string ChannelDesc)
        {
            KKSlmMsChannelModel channel = new KKSlmMsChannelModel();
            return channel.GetChannelId(ChannelDesc);
        }

        public List<SaveResultData> InsertNewLeads(string ticketId, List<ProductData> productList, string username, string staffNameTH, string channelId, string channelDesc)
        {
            List<SaveResultData> resultList = new List<SaveResultData>();
            DateTime createdDate = DateTime.Now;

            string jobOnHand = "";
            KKSlmMsStaffModel staff = new KKSlmMsStaffModel();
            StaffAmountJobOnHand tmp = staff.GetAmountJobOnHand(username);
            if (tmp != null)
            {
                jobOnHand = " (" + (tmp.AmountOwner + tmp.AmountDelegate).ToString() + " งาน)";
            }

            foreach (ProductData product in productList)
            {
                try
                {
                    SLM_DBEntities slmdb = new SLM_DBEntities();
                    StoreProcedure store = new StoreProcedure();
                    string new_ticketId = store.GenerateTicketId();

                    KKSlmTrLeadModel lead = new KKSlmTrLeadModel(slmdb);
                    lead.InsertData(ticketId, new_ticketId, product, username, createdDate, channelId);

                    KKSlmTrCusInfoModel customerInfo = new KKSlmTrCusInfoModel(slmdb);
                    customerInfo.InsertData(ticketId, new_ticketId, username, createdDate);

                    KKSlmTrProductInfoModel productInfo = new KKSlmTrProductInfoModel(slmdb);
                    productInfo.InsertData(ticketId, new_ticketId, username, createdDate);

                    KKSlmTrChannelInfoModel channelInfo = new KKSlmTrChannelInfoModel(slmdb);
                    channelInfo.InsertData(ticketId, new_ticketId, username, createdDate, channelId);

                    KKSLMTrCampaignFinalModel camFinal = new KKSLMTrCampaignFinalModel(slmdb);
                    camFinal.InsertData(new_ticketId, product, username, createdDate);

                    KKSlmTrHistoryModel.InsertHistory(slmdb, new_ticketId, SLMConstant.HistoryTypeCode.CreateLead, "", "", username, createdDate);

                    slmdb.SaveChanges();

                    SaveResultData data = new SaveResultData()
                    {
                        TicketId = new_ticketId,
                        CampaignName = product.CampaignName,
                        ChannelDesc = channelDesc,
                        Ownername = staffNameTH + jobOnHand
                    };
                    resultList.Add(data);
                }
                catch (Exception ex)
                {
                    _errList.Add(product.CampaignId, ex.Message);
                }
            }

            return resultList;
        }

        public static LeadDataPhoneCallHistory GetLeadDataPhoneCallHistory(string ticketId)
        {
            KKSlmTrLeadModel lead = new KKSlmTrLeadModel();
            return lead.GetLeadDataPhoneCallHistory(ticketId);
        }

        public static string GetAssignedFlag(string ticketId)
        {
            KKSlmTrLeadModel lead = new KKSlmTrLeadModel();
            return lead.GetAssignFlag(ticketId);
        }

        public static List<string> GetAssignedFlagAndDelegateFlag(string ticketId)
        {
            KKSlmTrLeadModel lead = new KKSlmTrLeadModel();
            return lead.GetAssignedFlagAndDelegateFlag(ticketId);
        }

        public static bool CheckRequireCardId(string statusCode)
        {
            KKSlmMsConfigCloseJobModel config = new KKSlmMsConfigCloseJobModel();
            return config.CheckRequireCardId(statusCode);
        }
    }
}

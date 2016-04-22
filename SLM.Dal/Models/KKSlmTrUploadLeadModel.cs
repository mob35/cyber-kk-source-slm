using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using SLM.Resource.Data;
using System.Configuration;
using SLM.Resource;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Transactions;

namespace SLM.Dal.Models
{
    public class KKSlmTrUploadLeadModel
    {
        private SLM_DBEntities slmdb = null;
        private string SLMDBName = SLMConstant.SLMDBName;

        public KKSlmTrUploadLeadModel()
        {
            slmdb = new SLM_DBEntities();
        }

        //Reference : SlmScr003Biz,RoleBiz
        public List<SearchUploadLeadResult> SearchUploadLeadData(SearchUploadLeadCondition suld)
        {
            string sql = "";

            sql = @"  SELECT ul.[slm_UploadLeadId]
                      ,ul.[slm_CampaignCode]
                      ,ul.[slm_FileName]
                      ,ul.[slm_LeadCount]
                      ,ul.[slm_LeadAssignedCount]
                      ,ul.[slm_Status]
                      ,ul.[slm_AssignedDate]
                      ,ul.[Is_Deleted]
                      ,isnull(su.slm_PositionName +'-','')+isnull(su.slm_StaffNameTH,'') [slm_UpdatedBy]
                      ,ul.[slm_UpdatedDate]
                      ,isnull(sc.slm_PositionName +'-','')+isnull(sc.slm_StaffNameTH,'') [slm_CreatedBy]
                      ,ul.[slm_CreatedDate] from kkslm_tr_upload_lead ul
                 left join kkslm_ms_staff su on ul.slm_UpdatedBy = su.slm_UserName
                 left join kkslm_ms_staff sc on ul.slm_CreatedBy = sc.slm_UserName
                 ";



            string whr = "";

            whr += (whr == "" ? "" : " AND ") + @"ul.Is_Deleted is null or ul.Is_Deleted = 0 ";

            if (suld.slm_FileName != "")
            {
                whr += (whr == "" ? "" : " AND ") + @"ul.slm_FileName like '%" + suld.slm_FileName + "%'";
            }

            if (suld.slm_Status != "0"){
                whr += (whr == "" ? "" : " AND ") + @"ul.slm_Status = '" + suld.slm_Status + "'";
            }

            if (suld.UploadedDateForm != "1-01-01")
            {
                whr += (whr == "" ? "" : " AND ") + @" CONVERT(DATE,ul.slm_UpdatedDate) >= '" + suld.UploadedDateForm + "'";
            }

            if (suld.UploadedDateTo != "1-01-01")
            {
                whr += (whr == "" ? "" : " AND ") + @" CONVERT(DATE,ul.slm_UpdatedDate) <= '" + suld.UploadedDateTo + "'";
            }

            sql += (whr == "" ? "" : " WHERE " + whr);

            sql += " ORDER BY ul.slm_UpdatedDate";

            return slmdb.ExecuteStoreQuery<SearchUploadLeadResult>(sql).ToList();
        }

        public UploadLeadData SearchUploadLeadData(string UploadLeadId)
        {

            string sql = @"select * from kkslm_tr_upload_lead";
            string whr = @"slm_UploadLeadId = '" + UploadLeadId + "'";
            sql += (whr == "" ? "" : " WHERE " + whr);
            //sql += " ORDER BY seq";

            return slmdb.ExecuteStoreQuery<UploadLeadData>(sql).FirstOrDefault();
        }


        public List<UploadLeadDetailData> GetUploadLeadDetailList(string UploadLeadId)
        {
            string sql = @"select * from kkslm_tr_upload_lead_detail";
            string whr = @"slm_UploadLeadId = '" + UploadLeadId + "'";
            sql += (whr == "" ? "" : " WHERE " + whr);
            //sql += " ORDER BY seq";

            return slmdb.ExecuteStoreQuery<UploadLeadDetailData>(sql).ToList();
        }

        //public bool isLastUploadLeadData(int? seq)
        //{

        //    string sql = @"select max(Seq) from UploadLead";
        //    //sql += " ORDER BY seq";

        //    int? UploadLeaddata = slmdb.ExecuteStoreQuery<int?>(sql).FirstOrDefault();

        //    if (UploadLeaddata != null && UploadLeaddata == seq)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        //public int? maxSeqUploadLeadData()
        //{

        //    string sql = @"select max(Seq) from UploadLead";
        //    //sql += " ORDER BY seq";

        //    return slmdb.ExecuteStoreQuery<int?>(sql).FirstOrDefault();
        //}


        public bool CheckFileExist(string FileName, int? UploadLeadId)
        {
            try
            {
                if (slmdb.kkslm_tr_upload_lead.Where(r => r.slm_FileName == FileName && r.slm_UploadLeadId != UploadLeadId).Count() == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                };

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //public void UpdateSeq(List<UploadLeadData> UploadLeaddatas)
        //{
        //    try
        //    {
        //        using (TransactionScope scope = new TransactionScope())
        //        {
        //            foreach (UploadLeadData UploadLeaddata in UploadLeaddatas)
        //            {
        //                UploadLead rd = slmdb.UploadLead.Where(r => r.UploadLeadId == UploadLeaddata.UploadLeadId).FirstOrDefault();
        //                rd.Seq = UploadLeaddata.Seq;
        //                slmdb.SaveChanges();
        //            }
        //            //Save and discard changes

        //            //if we get here things are looking good.
        //            scope.Complete();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public int AddUploadLead(UploadLeadData UploadLead, List<UploadLeadDetailData> UploadLeadDetailData, string username)
        {
            try
            {
                int key = 0;
                using (TransactionScope scope = new TransactionScope())
                {


                    kkslm_tr_upload_lead rd = new kkslm_tr_upload_lead();



                    rd.slm_LeadCount = UploadLead.slm_LeadCount;
                    rd.slm_FileName = UploadLead.slm_FileName;
                    rd.slm_Status = UploadLead.slm_Status;
                    rd.slm_CampaignCode = UploadLead.slm_CampaignCode;
                    
                    rd.slm_AssignedDate = null;
                    rd.slm_UpdatedBy = username;
                    rd.slm_UpdatedDate = DateTime.Now;
                    rd.slm_CreatedBy = username;
                    rd.slm_CreatedDate = DateTime.Now;



                    slmdb.kkslm_tr_upload_lead.AddObject(rd);
                    slmdb.SaveChanges();


                    //delete old UploadLead_Campaign
                    List<kkslm_tr_upload_lead_detail> oldUploadLeadDetail = slmdb.kkslm_tr_upload_lead_detail.Where(r => r.slm_UploadLeadId == UploadLead.slm_UploadLeadId).ToList();

                    if (oldUploadLeadDetail != null)
                    {
                        foreach (kkslm_tr_upload_lead_detail orc in oldUploadLeadDetail)
                        {
                            slmdb.kkslm_tr_upload_lead_detail.DeleteObject(orc);
                            slmdb.SaveChanges();
                        }

                    }


                    //add UploadLeadcampaigns
                    if (UploadLeadDetailData != null)
                    {
                        foreach (UploadLeadDetailData rcd in UploadLeadDetailData)
                        {
                            //if (slmdb.UploadLeadDetail.Where(r => r.CampaignCode == rcd.CampaignCode && r.CampaignCode == rcd.CampaignName).Count() == 0)
                            //{
                            kkslm_tr_upload_lead_detail rc = new kkslm_tr_upload_lead_detail();

                            rc.slm_UploadLeadId = rd.slm_UploadLeadId;
                            rc.slm_ContractBranchName = rcd.slm_ContractBranchName;
                            rc.slm_OwnerLead = rcd.slm_OwnerLead;
                            rc.slm_ThaiFirstName = rcd.slm_ThaiFirstName;
                            rc.slm_ThaiLastName = rcd.slm_ThaiLastName;
                            rc.slm_CardIdType = rcd.slm_CardIdType;
                            rc.slm_CardId = rcd.slm_CardId;
                            rc.slm_CustTelephoneMobile = rcd.slm_CustTelephoneMobile;
                            rc.slm_CustTelephoneHome = rcd.slm_CustTelephoneHome;
                            rc.slm_CustTelephoneOther = rcd.slm_CustTelephoneOther;
                            rc.slm_BrandName = rcd.slm_BrandName;
                            rc.slm_ModelName = rcd.slm_ModelName;
                            rc.slm_ModelYear = rcd.slm_ModelYear;
                            rc.slm_HpBscodeXsell = rcd.slm_HpBscodeXsell;
                            rc.slm_Remark = rcd.slm_Remark;
                            rc.slm_TicketID = rcd.slm_TicketID;

                            slmdb.kkslm_tr_upload_lead_detail.AddObject(rc);
                            slmdb.SaveChanges();
                            //}
                        }
                    }

                    key = rd.slm_UploadLeadId;


                    scope.Complete();
                    return key;


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void EditUploadLead(UploadLeadData UploadLead, List<UploadLeadDetailData> UploadLeadDetailData, string username)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    kkslm_tr_upload_lead rk = slmdb.kkslm_tr_upload_lead.Where(r => r.slm_UploadLeadId == UploadLead.slm_UploadLeadId).FirstOrDefault();


                    rk.slm_LeadCount = UploadLead.slm_LeadCount;
                    //rk.Status = UploadLead.Status;
                    //rk.AssignedDate = UploadLead.AssignedDate;
                    rk.slm_CampaignCode = UploadLead.slm_CampaignCode;
                    
                    rk.slm_UpdatedBy= username;
                    rk.slm_UpdatedDate = DateTime.Now;



                    //slmdb.UploadLead.AddObject(rk);
                    slmdb.SaveChanges();


                    //delete old UploadLead_Campaign
                    List<kkslm_tr_upload_lead_detail> oldUploadLeadDetail = slmdb.kkslm_tr_upload_lead_detail.Where(r => r.slm_UploadLeadId == UploadLead.slm_UploadLeadId).ToList();

                    if (oldUploadLeadDetail != null)
                    {
                        foreach (kkslm_tr_upload_lead_detail orc in oldUploadLeadDetail)
                        {
                            slmdb.kkslm_tr_upload_lead_detail.DeleteObject(orc);
                            slmdb.SaveChanges();
                        }

                    }





                    //add UploadLeadcampaigns
                    if (UploadLeadDetailData != null)
                    {
                        foreach (UploadLeadDetailData rcd in UploadLeadDetailData)
                        {
                            //if (slmdb.UploadLeadDetail.Where(r => r.CampaignCode == rcd.CampaignCode && r.CampaignCode == rcd.CampaignName).Count() == 0)
                            //{
                            kkslm_tr_upload_lead_detail rc = new kkslm_tr_upload_lead_detail();

                            rc.slm_UploadLeadId = rcd.slm_UploadLeadId;
                            rc.slm_ContractBranchName = rcd.slm_ContractBranchName;
                            rc.slm_OwnerLead = rcd.slm_OwnerLead;
                            rc.slm_ThaiFirstName = rcd.slm_ThaiFirstName;
                            rc.slm_ThaiLastName = rcd.slm_ThaiLastName;
                            rc.slm_CardIdType = rcd.slm_CardIdType;
                            rc.slm_CardId = rcd.slm_CardId;
                            rc.slm_CustTelephoneMobile = rcd.slm_CustTelephoneMobile;
                            rc.slm_CustTelephoneHome = rcd.slm_CustTelephoneHome;
                            rc.slm_CustTelephoneOther = rcd.slm_CustTelephoneOther;
                            rc.slm_BrandName = rcd.slm_BrandName;
                            rc.slm_ModelName = rcd.slm_ModelName;
                            rc.slm_ModelYear = rcd.slm_ModelYear;
                            rc.slm_HpBscodeXsell = rcd.slm_HpBscodeXsell;
                            rc.slm_Remark = rcd.slm_Remark;
                            rc.slm_TicketID = rcd.slm_TicketID;

                            slmdb.kkslm_tr_upload_lead_detail.AddObject(rc);
                            slmdb.SaveChanges();
                            //}
                        }
                    }



                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void DeleteUploadLead(int? UploadLeadId, string username)
        {

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    DeleteUploadLeadDetailData(UploadLeadId);
                    kkslm_tr_upload_lead rd = slmdb.kkslm_tr_upload_lead.Where(r => r.slm_UploadLeadId == UploadLeadId).FirstOrDefault();

                   // slmdb.kkslm_tr_upload_lead.DeleteObject(rd);
                    rd.Is_Deleted = 1;
                    rd.slm_UpdatedBy = username;
                    rd.slm_UpdatedDate = DateTime.Now;
                    slmdb.SaveChanges();

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public void DeleteUploadLeadDetailData(int? UploadLeadId)
        {
            try
            {
                var sortConList = slmdb.kkslm_tr_upload_lead_detail.Where(p => p.slm_UploadLeadId == UploadLeadId).ToList();
                foreach (kkslm_tr_upload_lead_detail obj in sortConList)
                {
                    slmdb.kkslm_tr_upload_lead_detail.DeleteObject(obj);
                }

                slmdb.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



    }
}

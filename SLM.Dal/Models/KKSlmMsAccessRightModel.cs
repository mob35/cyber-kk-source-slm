﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using SLM.Resource.Data;
using SLM.Resource;

namespace SLM.Dal.Models
{
    public class KKSlmMsAccessRightModel
    {
        private SLM_DBEntities slmdb = null;

        public KKSlmMsAccessRightModel()
        {
            slmdb = new SLM_DBEntities();
        }

        public List<AccessRightData> SearchAccessRight(string productId, string campaignId, string staffTypeId, string branchCode)
        {
            string SLMDBName = SLMConstant.SLMDBName;
            string sql = @"SELECT access.slm_Product_Id AS ProductId, pd.sub_product_name AS ProductName, access.slm_CampaignId AS CampaignId, cam.slm_CampaignName AS CampaignName
                            , access.slm_StaffTypeId AS StaffTypeId, st.slm_StaffTypeDesc AS StaffTypeDesc, access.slm_BranchCode AS BranchCode, branch.slm_BranchName AS BranchName
                            FROM " + SLMDBName + @".dbo.kkslm_ms_access_right access
                            LEFT JOIN " + SLMDBName + @".dbo.CMT_MAPPING_PRODUCT pd ON pd.sub_product_id = access.slm_Product_Id
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_campaign cam ON cam.slm_CampaignId = access.slm_CampaignId
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff_type st ON st.slm_StaffTypeId = access.slm_StaffTypeId
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_branch branch ON branch.slm_BranchCode = access.slm_BranchCode
                            WHERE access.is_Deleted = 0 {0} 
                            ORDER BY cam.slm_CampaignName, pd.sub_product_name, branch.slm_BranchName, st.slm_StaffTypeDesc ";

            string whr = "";
            whr += (productId == "" ? "" : (whr == "" ? "" : " AND ") + " access.slm_Product_Id = '" + productId + "' ");
            whr += (campaignId == "" ? "" : (whr == "" ? "" : " AND ") + " access.slm_CampaignId = '" + campaignId + "' ");
            whr += (staffTypeId == "" ? "" : (whr == "" ? "" : " AND ") + " access.slm_StaffTypeId = '" + staffTypeId + "' ");
            whr += (branchCode == "" ? "" : (whr == "" ? "" : " AND ") + " access.slm_BranchCode = '" + branchCode + "' ");

            whr = (whr == "" ? "" : " AND " + whr);
            sql = string.Format(sql, whr);

            return slmdb.ExecuteStoreQuery<AccessRightData>(sql).ToList();
        }

        public void UpdateData(string productId, string campaignId, int staffTypeId, List<string> branchCodeList, string createdBy)
        {
            try
            {
                string del = "";

                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
                {
                    if (!string.IsNullOrEmpty(productId))
                        del = @"DELETE FROM " + SLMConstant.SLMDBName + ".dbo.kkslm_ms_access_right WHERE slm_Product_Id = '" + productId + "' AND slm_StaffTypeId = '" + staffTypeId.ToString() + "' ";
                    else if (!string.IsNullOrEmpty(campaignId))
                        del = @"DELETE FROM " + SLMConstant.SLMDBName + ".dbo.kkslm_ms_access_right WHERE slm_CampaignId = '" + campaignId + "' AND slm_StaffTypeId = '" + staffTypeId.ToString() + "' ";
                    else
                        throw new Exception("Cannot find productId or campaignId");

                    if (!string.IsNullOrEmpty(del))
                        slmdb.ExecuteStoreCommand(del);

                    DateTime createdDate = DateTime.Now;
                    foreach (string branchcode in branchCodeList)
                    {
                        kkslm_ms_access_right obj = new kkslm_ms_access_right()
                        {
                            slm_Product_Id = string.IsNullOrEmpty(productId) ? null : productId,
                            slm_CampaignId = string.IsNullOrEmpty(campaignId) ? null : campaignId,
                            slm_StaffTypeId = staffTypeId,
                            slm_BranchCode = branchcode,
                            slm_CreatedBy = createdBy,
                            slm_CreatedDate = createdDate,
                            slm_UpdatedBy = createdBy,
                            slm_UpdatedDate = createdDate,
                            is_Deleted = false
                        };
                        slmdb.kkslm_ms_access_right.AddObject(obj);
                    }

                    slmdb.SaveChanges();

                    ts.Complete();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsertData(string productId, string campaignId, int staffTypeId, List<string> branchCodeList, string createdBy)
        {
            try
            {
                DateTime createdDate = DateTime.Now;
                List<string> dbBranchCodeList = null;

                //หา branch ที่มีอยู่ในเบส
                if (!string.IsNullOrEmpty(productId))
                    dbBranchCodeList = slmdb.kkslm_ms_access_right.Where(p => p.slm_Product_Id == productId && p.slm_StaffTypeId == staffTypeId && p.is_Deleted == false).Select(p => p.slm_BranchCode).ToList();
                else if (!string.IsNullOrEmpty(campaignId))
                    dbBranchCodeList = slmdb.kkslm_ms_access_right.Where(p => p.slm_CampaignId == campaignId && p.slm_StaffTypeId == staffTypeId && p.is_Deleted == false).Select(p => p.slm_BranchCode).ToList();
                else
                    throw new Exception("Cannot find productId or campaignId");

                //เอา branch ที่มีอยู่ในเบสแล้ว ออกจาก list ที่ต้องการ insert
                if (dbBranchCodeList.Count > 0)
                    branchCodeList = branchCodeList.Except<string>(dbBranchCodeList).ToList();

                if (branchCodeList.Count > 0)
                {
                    foreach (string branchcode in branchCodeList)
                    {
                        kkslm_ms_access_right obj = new kkslm_ms_access_right()
                        {
                            slm_Product_Id = string.IsNullOrEmpty(productId) ? null : productId,
                            slm_CampaignId = string.IsNullOrEmpty(campaignId) ? null : campaignId,
                            slm_StaffTypeId = staffTypeId,
                            slm_BranchCode = branchcode,
                            slm_CreatedBy = createdBy,
                            slm_CreatedDate = createdDate,
                            slm_UpdatedBy = createdBy,
                            slm_UpdatedDate = createdDate,
                            is_Deleted = false
                        };
                        slmdb.kkslm_ms_access_right.AddObject(obj);
                    }

                    slmdb.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

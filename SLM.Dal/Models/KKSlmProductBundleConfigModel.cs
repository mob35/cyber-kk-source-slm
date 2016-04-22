﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SLM.Resource.Data;
using SLM.Resource;

namespace SLM.Dal.Models
{
    public class KKSlmProductBundleConfigModel
    {
        private SLM_DBEntities slmdb = null;
        private string SLMDBName = System.Configuration.ConfigurationManager.AppSettings["SLMDBName"].ToString();

        public KKSlmProductBundleConfigModel()
        {
            slmdb = new SLM_DBEntities();
        }

        public List<ProductData> GetBundleProduct(string campaignId)
        {
            string sql = @"SELECT BUN.slm_Product_Bundle_Id AS ProductBundleId, BUN.slm_Product_Id AS ProductId, BUN.slm_Product_Bundle AS ProductBundle
                            , BUN.slm_Description AS BundleDescription
                            FROM " + SLMDBName + @".dbo.CMT_CAMPAIGN_PRODUCT CP
                            INNER JOIN " + SLMDBName + @".dbo.kkslm_product_bundle_config BUN ON BUN.slm_Product_Id = CP.PR_ProductId AND BUN.is_Deleted = 0
                            WHERE CP.PR_CampaignId = '" + campaignId + "'";

            var bundleList = slmdb.ExecuteStoreQuery<ProductBundleConfigData>(sql).ToList();

            string productIdList = "";
            foreach (ProductBundleConfigData data in bundleList)
            {
                if (!string.IsNullOrEmpty(data.ProductBundle))
                {
                    string[] idList = data.ProductBundle.Split(',');
                    foreach (string productId in idList)
                    {
                        productIdList += (productIdList != "" ? "," : "") + "'" + productId + "'";
                    }
                }
            }

            if (productIdList != "")    //มี Bundle
            {
                sql = @"SELECT PG.product_id AS ProductGroupId, PG.product_name AS ProductGroupName, MP.sub_product_id AS ProductId, MP.sub_product_name AS ProductName
                        , CAM.slm_CampaignId AS CampaignId, CAM.slm_CampaignName AS CampaignName, CAM.slm_Offer + ' : ' + CAM.slm_Criteria AS CampaignDesc, CAM.slm_StartDate AS StartDate, CAM.slm_EndDate AS EndDate
                        , CAM.slm_CampaignType AS CampaignType
                        FROM SLMDB.dbo.CMT_MS_PRODUCT_GROUP PG
                        INNER JOIN SLMDB.dbo.CMT_MAPPING_PRODUCT MP ON PG.product_id = MP.product_id
                        INNER JOIN SLMDB.dbo.CMT_CAMPAIGN_PRODUCT CP ON MP.sub_product_id = CP.PR_ProductId AND MP.product_id = CP.PR_ProductGroupId
                        INNER JOIN SLMDB.dbo.kkslm_ms_campaign CAM ON CP.PR_CampaignId = CAM.slm_CampaignId AND CAM.is_Deleted = 0 AND CAM.slm_Status = 'A' 
                        WHERE MP.sub_product_id IN (" + productIdList + ")";

                return slmdb.ExecuteStoreQuery<ProductData>(sql).ToList();
            }
            else
            {
                return new List<ProductData>();
            }
        }
    }
}

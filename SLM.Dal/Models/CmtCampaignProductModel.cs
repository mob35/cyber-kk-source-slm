using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SLM.Resource;
using SLM.Resource.Data;

namespace SLM.Dal.Models
{
    public class CmtCampaignProductModel
    {
        private SLM_DBEntities slmdb = null;

        public CmtCampaignProductModel()
        {
            slmdb = new SLM_DBEntities();
        }

        //INNER JOIN เพื่อเอาแต่ข้อมูลแคมเปญที่ยังไม่หมดอายุ(ใช้หน้าสร้าง Lead)
        public List<ControlListData> GetCampaignData(string productGroupId, string productId)
        {
            string sql = @"SELECT CAM.slm_CampaignId AS ValueField, CAM.slm_CampaignName AS TextField
                            FROM SLMDB.dbo.CMT_CAMPAIGN_PRODUCT PRO
                            INNER JOIN SLMDB.dbo.kkslm_ms_campaign CAM ON CAM.slm_CampaignId = PRO.PR_CampaignId AND CAM.is_Deleted = 0 AND CAM.slm_Status = 'A' AND CAM.slm_CampaignType = '" + SLMConstant.CampaignType.Mass + @"'
                            WHERE PRO.PR_ProductGroupId = '" + productGroupId + "' AND PRO.PR_ProductId = '" + productId + @"' 
                            ORDER BY CAM.slm_CampaignName";

            return slmdb.ExecuteStoreQuery<ControlListData>(sql).ToList();
        }

        //INNER JOIN เพื่อเอาแต่ข้อมูลแคมเปญที่ยังไม่หมดอายุ
        //Reference: SlmScr003Biz.cs (ใช้หน้าสร้าง Lead)
        public List<ProductData> SearchCampaign(string productGroupId, string productId, string campaignId)
        {
//            string sql = @"SELECT PG.product_id AS ProductGroupId, PG.product_name AS ProductGroupName, MP.sub_product_id AS ProductId, MP.sub_product_name AS ProductName
//                            , CAM.slm_CampaignId AS CampaignId, CAM.slm_CampaignName AS CampaignName, CAM.slm_Offer + ' : ' + CAM.slm_Criteria AS CampaignDesc, CAM.slm_StartDate AS StartDate, CAM.slm_EndDate AS EndDate
//                            FROM SLMDB.dbo.CMT_MS_PRODUCT_GROUP PG
//                            INNER JOIN SLMDB.dbo.CMT_MAPPING_PRODUCT MP ON PG.product_id = MP.product_id
//                            INNER JOIN SLMDB.dbo.CMT_CAMPAIGN_PRODUCT CP ON MP.sub_product_id = CP.PR_ProductId AND MP.product_id = CP.PR_ProductGroupId
//                            INNER JOIN SLMDB.dbo.kkslm_ms_campaign CAM ON CP.PR_CampaignId = CAM.slm_CampaignId
//                            WHERE CAM.is_Deleted = 0 AND CAM.slm_Status = 'A' AND CAM.slm_CampaignType = '" + SLMConstant.CampaignType.Mass + @"'
//                            AND CP.PR_ProductGroupId = '" + productGroupId + "' AND CP.PR_ProductId = '" + productId + "' AND CP.PR_CampaignId = '" + campaignId + "' ORDER BY CAM.slm_CampaignName ASC ";

            string sql = @"SELECT PG.product_id AS ProductGroupId, PG.product_name AS ProductGroupName, MP.sub_product_id AS ProductId, MP.sub_product_name AS ProductName
                            , CAM.slm_CampaignId AS CampaignId, CAM.slm_CampaignName AS CampaignName, CAM.slm_Offer + ' : ' + CAM.slm_Criteria AS CampaignDesc, CAM.slm_StartDate AS StartDate, CAM.slm_EndDate AS EndDate
                            FROM SLMDB.dbo.CMT_MS_PRODUCT_GROUP PG
                            INNER JOIN SLMDB.dbo.CMT_MAPPING_PRODUCT MP ON PG.product_id = MP.product_id
                            INNER JOIN SLMDB.dbo.CMT_CAMPAIGN_PRODUCT CP ON MP.sub_product_id = CP.PR_ProductId AND MP.product_id = CP.PR_ProductGroupId
                            INNER JOIN SLMDB.dbo.kkslm_ms_campaign CAM ON CP.PR_CampaignId = CAM.slm_CampaignId
                            WHERE CAM.is_Deleted = 0 AND CAM.slm_Status = 'A' AND CAM.slm_CampaignType = '" + SLMConstant.CampaignType.Mass + @"'
                            AND CP.PR_ProductGroupId = '" + productGroupId + "' ";

            string whr = "";
            whr += (productId == "0" ? "" : (whr == "" ? "" : " AND ") + " CP.PR_ProductId = '" + productId + "' ");
            whr += (campaignId == "" ? "" : (whr == "" ? "" : " AND ") + " CP.PR_CampaignId = '" + campaignId + "' ");

            sql += (whr != "" ? " AND " + whr : "");
            sql += " ORDER BY CAM.slm_CampaignName ASC ";

            return slmdb.ExecuteStoreQuery<ProductData>(sql).ToList();
        }

        //INNER JOIN เพื่อเอาแต่ข้อมูลแคมเปญที่ยังไม่หมดอายุ
        //Reference: SlmScr003Biz.cs(ใช้หน้าสร้าง Lead)
        public List<ProductData> SearchCampaign(string searchWord)
        {
            string sql = @"SELECT PG.product_id AS ProductGroupId, PG.product_name AS ProductGroupName, MP.sub_product_id AS ProductId, MP.sub_product_name AS ProductName
                            , CAM.slm_CampaignId AS CampaignId, CAM.slm_CampaignName AS CampaignName, CAM.slm_Offer + ' : ' + CAM.slm_Criteria AS CampaignDesc, CAM.slm_StartDate AS StartDate, CAM.slm_EndDate AS EndDate
                            FROM SLMDB.dbo.CMT_MS_PRODUCT_GROUP PG
                            INNER JOIN SLMDB.dbo.CMT_MAPPING_PRODUCT MP ON PG.product_id = MP.product_id
                            INNER JOIN SLMDB.dbo.CMT_CAMPAIGN_PRODUCT CP ON MP.sub_product_id = CP.PR_ProductId AND MP.product_id = CP.PR_ProductGroupId
                            INNER JOIN SLMDB.dbo.kkslm_ms_campaign CAM ON CP.PR_CampaignId = CAM.slm_CampaignId AND CAM.is_Deleted = 0 AND CAM.slm_Status = 'A' AND CAM.slm_CampaignType = '" + SLMConstant.CampaignType.Mass + @"'
                            WHERE (PG.product_name LIKE '%" + searchWord + "%' OR MP.sub_product_name LIKE '%" + searchWord + "%' OR CAM.slm_CampaignName LIKE '%" + searchWord + "%')  ORDER BY CAM.slm_CampaignName ASC ";

            return slmdb.ExecuteStoreQuery<ProductData>(sql).ToList();
        }

        //Reference: SlmScr003Biz.cs(ใช้หน้าสร้าง Lead)
        public List<ProductData> GetProductCampaignData(string campaignId)
        {
            string sql = @"SELECT PG.product_id AS ProductGroupId, PG.product_name AS ProductGroupName, MP.sub_product_id AS ProductId, MP.sub_product_name AS ProductName
                            , CAM.slm_CampaignId AS CampaignId, CAM.slm_CampaignName AS CampaignName, CAM.slm_Offer + ' : ' + CAM.slm_Criteria AS CampaignDesc, CAM.slm_StartDate AS StartDate, CAM.slm_EndDate AS EndDate
                            , ISNULL(MP.HasADAMUrl, 0) AS HasAdamsUrl
                            FROM SLMDB.dbo.CMT_MS_PRODUCT_GROUP PG
                            INNER JOIN SLMDB.dbo.CMT_MAPPING_PRODUCT MP ON PG.product_id = MP.product_id
                            INNER JOIN SLMDB.dbo.CMT_CAMPAIGN_PRODUCT CP ON MP.sub_product_id = CP.PR_ProductId AND MP.product_id = CP.PR_ProductGroupId
                            INNER JOIN SLMDB.dbo.kkslm_ms_campaign CAM ON CP.PR_CampaignId = CAM.slm_CampaignId AND CAM.is_Deleted = 0 AND CAM.slm_Status = 'A' AND CAM.slm_CampaignType = '" + SLMConstant.CampaignType.Mass + @"'
                            WHERE CAM.slm_CampaignId = '" + campaignId + "'";

            return slmdb.ExecuteStoreQuery<ProductData>(sql).ToList();
        }

        //ใช้หน้าแนะนำแคมเปญ
        public List<ProductData> GetProductCampaignDataForSuggestCampaign(string campaignId)
        {
            string sql = @"SELECT PG.product_id AS ProductGroupId, PG.product_name AS ProductGroupName, MP.sub_product_id AS ProductId, MP.sub_product_name AS ProductName
                            , CAM.slm_CampaignId AS CampaignId, CAM.slm_CampaignName AS CampaignName, CAM.slm_Offer + ' : ' + CAM.slm_Criteria AS CampaignDesc, CAM.slm_StartDate AS StartDate, CAM.slm_EndDate AS EndDate
                            FROM SLMDB.dbo.CMT_MS_PRODUCT_GROUP PG
                            INNER JOIN SLMDB.dbo.CMT_MAPPING_PRODUCT MP ON PG.product_id = MP.product_id
                            INNER JOIN SLMDB.dbo.CMT_CAMPAIGN_PRODUCT CP ON MP.sub_product_id = CP.PR_ProductId AND MP.product_id = CP.PR_ProductGroupId
                            INNER JOIN SLMDB.dbo.kkslm_ms_campaign CAM ON CP.PR_CampaignId = CAM.slm_CampaignId AND CAM.is_Deleted = 0 AND CAM.slm_Status = 'A' 
                            WHERE CAM.slm_CampaignId = '" + campaignId + "'";

            return slmdb.ExecuteStoreQuery<ProductData>(sql).ToList();
        }

        //================== ใช้หน้า View Lead ==============================================================================================================================================================================

        public List<ControlListData> GetCampaignDataViewPage(string productGroupId, string productId, string CmtCampaignIdList)
        {
//            string sql = @"SELECT CAM.slm_CampaignId AS ValueField, CAM.slm_CampaignName AS TextField
//                            FROM SLMDB.dbo.CMT_CAMPAIGN_PRODUCT PRO
//                            INNER JOIN SLMDB.dbo.kkslm_ms_campaign CAM ON CAM.slm_CampaignId = PRO.PR_CampaignId AND CAM.is_Deleted = 0 AND CAM.slm_Status = 'A' 
//                            WHERE PRO.PR_ProductGroupId = '" + productGroupId + "' AND PRO.PR_ProductId = '" + productId + @"' 
//                            ORDER BY CAM.slm_CampaignName";
            string sql = "";
            if (CmtCampaignIdList.Trim() != "")
            {
                sql = @"SELECT LIST.ValueField, LIST.TextField 
                        FROM(
                            SELECT CAM.slm_CampaignId AS ValueField, CAM.slm_CampaignName AS TextField
                            FROM SLMDB.dbo.CMT_CAMPAIGN_PRODUCT PRO
                            INNER JOIN SLMDB.dbo.kkslm_ms_campaign CAM ON CAM.slm_CampaignId = PRO.PR_CampaignId AND CAM.is_Deleted = 0 AND CAM.slm_Status = 'A' AND CAM.slm_CampaignType = '" + SLMConstant.CampaignType.Mass + @"'
                            WHERE PRO.PR_ProductGroupId = '" + productGroupId + "' AND PRO.PR_ProductId = '" + productId + @"' 
                            UNION
                            SELECT CAM.slm_CampaignId AS ValueField, CAM.slm_CampaignName AS TextField
                            FROM SLMDB.dbo.CMT_CAMPAIGN_PRODUCT PRO
                            INNER JOIN SLMDB.dbo.kkslm_ms_campaign CAM ON CAM.slm_CampaignId = PRO.PR_CampaignId AND CAM.is_Deleted = 0 AND CAM.slm_Status = 'A' AND CAM.slm_CampaignType = '02'
                            WHERE PRO.PR_ProductGroupId = '" + productGroupId + "' AND PRO.PR_ProductId = '" + productId + @"' 
                            AND CAM.slm_CampaignId IN (" + CmtCampaignIdList + @")) LIST
                        ORDER BY LIST.TextField";
            }
            else
            {
                sql = @"SELECT CAM.slm_CampaignId AS ValueField, CAM.slm_CampaignName AS TextField
                        FROM SLMDB.dbo.CMT_CAMPAIGN_PRODUCT PRO
                        INNER JOIN SLMDB.dbo.kkslm_ms_campaign CAM ON CAM.slm_CampaignId = PRO.PR_CampaignId AND CAM.is_Deleted = 0 AND CAM.slm_Status = 'A' AND CAM.slm_CampaignType = '" + SLMConstant.CampaignType.Mass + @"'
                        WHERE PRO.PR_ProductGroupId = '" + productGroupId + "' AND PRO.PR_ProductId = '" + productId + @"' 
                        ORDER BY CAM.slm_CampaignName ";
            }

            return slmdb.ExecuteStoreQuery<ControlListData>(sql).ToList();
        }

        public List<ProductData> SearchCampaignViewPage(string productGroupId, string productId, string campaignId, string bundleCamIdList)
        {
//            string sql = @"SELECT PG.product_id AS ProductGroupId, PG.product_name AS ProductGroupName, MP.sub_product_id AS ProductId, MP.sub_product_name AS ProductName
//                            , CAM.slm_CampaignId AS CampaignId, CAM.slm_CampaignName AS CampaignName, CAM.slm_Offer + ' : ' + CAM.slm_Criteria AS CampaignDesc, CAM.slm_StartDate AS StartDate, CAM.slm_EndDate AS EndDate
//                            FROM SLMDB.dbo.CMT_MS_PRODUCT_GROUP PG
//                            INNER JOIN SLMDB.dbo.CMT_MAPPING_PRODUCT MP ON PG.product_id = MP.product_id
//                            INNER JOIN SLMDB.dbo.CMT_CAMPAIGN_PRODUCT CP ON MP.sub_product_id = CP.PR_ProductId AND MP.product_id = CP.PR_ProductGroupId
//                            INNER JOIN SLMDB.dbo.kkslm_ms_campaign CAM ON CP.PR_CampaignId = CAM.slm_CampaignId
//                            WHERE CAM.is_Deleted = 0 AND CAM.slm_Status = 'A' 
//                            AND CP.PR_ProductGroupId = '" + productGroupId + "' AND CP.PR_ProductId = '" + productId + "' AND CP.PR_CampaignId = '" + campaignId + "' ORDER BY CAM.slm_CampaignName ASC ";

            string sql = @"SELECT PG.product_id AS ProductGroupId, PG.product_name AS ProductGroupName, MP.sub_product_id AS ProductId, MP.sub_product_name AS ProductName
                            , CAM.slm_CampaignId AS CampaignId, CAM.slm_CampaignName AS CampaignName, CAM.slm_Offer + ' : ' + CAM.slm_Criteria AS CampaignDesc, CAM.slm_StartDate AS StartDate, CAM.slm_EndDate AS EndDate
                            , CAM.slm_CampaignType AS CampaignType
                            FROM SLMDB.dbo.CMT_MS_PRODUCT_GROUP PG
                            INNER JOIN SLMDB.dbo.CMT_MAPPING_PRODUCT MP ON PG.product_id = MP.product_id
                            INNER JOIN SLMDB.dbo.CMT_CAMPAIGN_PRODUCT CP ON MP.sub_product_id = CP.PR_ProductId AND MP.product_id = CP.PR_ProductGroupId
                            INNER JOIN SLMDB.dbo.kkslm_ms_campaign CAM ON CP.PR_CampaignId = CAM.slm_CampaignId
                            WHERE CAM.is_Deleted = 0 AND CAM.slm_Status = 'A' 
                            AND CP.PR_ProductGroupId = '" + productGroupId + "' ";

            string whr = "";
            whr += (productId == "0" ? "" : (whr == "" ? "" : " AND ") + " CP.PR_ProductId = '" + productId + "' ");
            whr += (campaignId == "" ? "" : (whr == "" ? "" : " AND ") + " CP.PR_CampaignId = '" + campaignId + "' ");
            whr += (bundleCamIdList == "" ? "" : (whr == "" ? "" : " AND ") + " CP.PR_CampaignId NOT IN (" + bundleCamIdList + ") ");

            sql += (whr != "" ? " AND " + whr : "");
            sql += " ORDER BY CAM.slm_CampaignName ASC ";

            return slmdb.ExecuteStoreQuery<ProductData>(sql).ToList();
        }

        //Reference: SlmScr004Biz.cs(ใช้หน้า View Lead)
        public List<ProductData> GetProductCampaignDataForCmt(string campaignId)
        {
            string sql = @"SELECT PG.product_id AS ProductGroupId, PG.product_name AS ProductGroupName, MP.sub_product_id AS ProductId, MP.sub_product_name AS ProductName
                            , CAM.slm_CampaignId AS CampaignId, CAM.slm_CampaignName AS CampaignName, CAM.slm_Offer + ' : ' + CAM.slm_Criteria AS CampaignDesc, CAM.slm_StartDate AS StartDate, CAM.slm_EndDate AS EndDate
                            FROM SLMDB.dbo.CMT_MS_PRODUCT_GROUP PG
                            INNER JOIN SLMDB.dbo.CMT_MAPPING_PRODUCT MP ON PG.product_id = MP.product_id
                            INNER JOIN SLMDB.dbo.CMT_CAMPAIGN_PRODUCT CP ON MP.sub_product_id = CP.PR_ProductId AND MP.product_id = CP.PR_ProductGroupId
                            INNER JOIN SLMDB.dbo.kkslm_ms_campaign CAM ON CP.PR_CampaignId = CAM.slm_CampaignId 
                            WHERE CAM.slm_CampaignId = '" + campaignId + "'";

            return slmdb.ExecuteStoreQuery<ProductData>(sql).ToList();
        }

        //INNER JOIN เพื่อเอาแต่ข้อมูลแคมเปญที่ยังไม่หมดอายุ
        //Reference: SlmScr004Biz.cs(ใช้หน้า View Lead)
        public List<ProductData> SearchCampaign(string searchWord, string bundleCamIdList)
        {
            string sql = @"SELECT PG.product_id AS ProductGroupId, PG.product_name AS ProductGroupName, MP.sub_product_id AS ProductId, MP.sub_product_name AS ProductName
                            , CAM.slm_CampaignId AS CampaignId, CAM.slm_CampaignName AS CampaignName, CAM.slm_Offer + ' : ' + CAM.slm_Criteria AS CampaignDesc, CAM.slm_StartDate AS StartDate, CAM.slm_EndDate AS EndDate
                            , CAM.slm_CampaignType AS CampaignType
                            FROM SLMDB.dbo.CMT_MS_PRODUCT_GROUP PG
                            INNER JOIN SLMDB.dbo.CMT_MAPPING_PRODUCT MP ON PG.product_id = MP.product_id
                            INNER JOIN SLMDB.dbo.CMT_CAMPAIGN_PRODUCT CP ON MP.sub_product_id = CP.PR_ProductId AND MP.product_id = CP.PR_ProductGroupId
                            INNER JOIN SLMDB.dbo.kkslm_ms_campaign CAM ON CP.PR_CampaignId = CAM.slm_CampaignId AND CAM.is_Deleted = 0 AND CAM.slm_Status = 'A' 
                            WHERE (PG.product_name LIKE '%" + searchWord + "%' OR MP.sub_product_name LIKE '%" + searchWord + "%' OR CAM.slm_CampaignName LIKE '%" + searchWord + "%') ";

            if (bundleCamIdList != "")
                sql += " AND CP.PR_CampaignId NOT IN (" + bundleCamIdList + ") ";

            sql += " ORDER BY CAM.slm_CampaignName ASC ";

            return slmdb.ExecuteStoreQuery<ProductData>(sql).ToList();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SLM.Resource.Data;

namespace SLM.Dal.Models
{
    public class CmtMappingProductModel
    {
        private SLM_DBEntities slmdb = null;

        public CmtMappingProductModel()
        {
            slmdb = new SLM_DBEntities();
        }

        public List<ControlListData> GetProductData(string productGroupId)
        {
            string sql = @"SELECT sub_product_id AS ValueField, sub_product_name AS TextField FROM SLMDB.dbo.CMT_MAPPING_PRODUCT
                            WHERE (is_deleted = 0 OR is_deleted IS NULL) AND product_id = '" + productGroupId + @"'
                            ORDER BY sub_product_name";

            return slmdb.ExecuteStoreQuery<ControlListData>(sql).ToList();
        }

        public List<ControlListData> GetProductList()
        {
            string sql = @"SELECT sub_product_id AS ValueField, sub_product_name AS TextField FROM SLMDB.dbo.CMT_MAPPING_PRODUCT
                            WHERE (is_deleted = 0 OR is_deleted IS NULL) 
                            ORDER BY sub_product_name";

            return slmdb.ExecuteStoreQuery<ControlListData>(sql).ToList();
        }
    }
}

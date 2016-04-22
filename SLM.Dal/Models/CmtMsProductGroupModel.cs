using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SLM.Resource.Data;

namespace SLM.Dal.Models
{
    public class CmtMsProductGroupModel
    {
        private SLM_DBEntities slmdb = null;

        public CmtMsProductGroupModel()
        {
            slmdb = new SLM_DBEntities();
        }

        public List<ControlListData> GetProductGroupData()
        {
            string sql = @"SELECT product_id AS ValueField, product_name AS TextField FROM SLMDB.dbo.CMT_MS_PRODUCT_GROUP 
                            WHERE (is_deleted = 0 OR is_deleted IS NULL)
                            ORDER BY product_name";

            return slmdb.ExecuteStoreQuery<ControlListData>(sql).ToList();
        }
    }
}

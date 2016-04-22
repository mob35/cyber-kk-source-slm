using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SLM.Resource.Data;

namespace SLM.Dal.Models
{
    public class KKSlmMsBrandModel
    {
       private SLM_DBEntities slmdb = null;

       public KKSlmMsBrandModel()
        {
            slmdb = new SLM_DBEntities();
        }
        public List<ControlListData> GetBrandData()
        {
            return slmdb.kkslm_ms_brand.OrderBy(p => p.slm_BrandName).AsEnumerable().Select(p => new ControlListData { TextField = p.slm_BrandName, ValueField = p.slm_BrandCode }).ToList();
        }
        public int? GetBrandId(string BrandCode)
        {
            return slmdb.kkslm_ms_brand.Where(p => p.slm_BrandCode == BrandCode).Select(p => p.slm_BrandId).FirstOrDefault();
        }
    }
}

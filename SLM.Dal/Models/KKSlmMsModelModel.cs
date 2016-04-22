using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SLM.Resource.Data;

namespace SLM.Dal.Models
{
    public class KKSlmMsModelModel
    {
        private SLM_DBEntities slmdb = null;

        public KKSlmMsModelModel()
        {
            slmdb = new SLM_DBEntities();
        }
        public List<ControlListData> GetModelData(string brandcode)
        {
            return slmdb.kkslm_ms_model.Where(p => p.slm_BrandCode == brandcode).OrderBy(p => p.slm_FamilyDesc).AsEnumerable().Select(p => new ControlListData { TextField = p.slm_FamilyDesc, ValueField = p.slm_Family.ToString() }).ToList();
        }
        public int? GetModelId(string BrandCode, string Family)
        {
            return slmdb.kkslm_ms_model.Where(p => p.slm_BrandCode == BrandCode && p.slm_Family == Family).Select(p => p.slm_ModelId).FirstOrDefault();
        }
    }
}

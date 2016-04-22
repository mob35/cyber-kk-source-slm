using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SLM.Resource.Data;

namespace SLM.Dal.Models
{
    public class KKSlmMsAmphurModel
    {
        private SLM_DBEntities slmdb = null;

        public KKSlmMsAmphurModel()
        {
            slmdb = new SLM_DBEntities();
        }
        public List<ControlListData> GetAmphurData(string provinceCode)
        {
            return slmdb.kkslm_ms_amphur.Where(p => p.slm_ProvinceCode == provinceCode).OrderBy(p => p.slm_AmphurNameTH).Select(p => new ControlListData { TextField = p.slm_AmphurNameTH, ValueField = p.slm_AmphurCode }).ToList();
        }

        public int? GetAmphurId(string provinceCode, string amphurCode)
        {
            return slmdb.kkslm_ms_amphur.Where(p => p.slm_ProvinceCode == provinceCode && p.slm_AmphurCode == amphurCode).Select(p => p.slm_AmphurId).FirstOrDefault();
        }
    }
}

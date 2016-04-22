using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SLM.Resource.Data;

namespace SLM.Dal.Models
{
    public class KKSlmMsTambolModel
    {
        private SLM_DBEntities slmdb = null;

        public KKSlmMsTambolModel()
        {
            slmdb = new SLM_DBEntities();
        }
        public List<ControlListData> GetTambolData(string provinceCode, string amphurCode, bool useWebservice)
        {
            //เฉพาะตำบลให้ใช้ id เป็น value เพื่อที่จะ insert ลงเบสได้เลย
            return slmdb.kkslm_ms_tambol.Where(p => p.slm_ProvinceCode == provinceCode && p.slm_AmphurCode == amphurCode).OrderBy(p => p.slm_TambolNameTH).AsEnumerable().Select(p => new ControlListData { TextField = p.slm_TambolNameTH, ValueField = useWebservice ? p.slm_TambolCode.ToString() : p.slm_TambolId.ToString() }).ToList();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SLM.Resource.Data;

namespace SLM.Dal.Models
{
    public class KKSlmMsCardTypeModel
    {
        private SLM_DBEntities slmdb = null;

        public KKSlmMsCardTypeModel()
        {
            slmdb = new SLM_DBEntities();
        }

        public List<ControlListData> GetCardTypeList()
        {
            return slmdb.kkslm_ms_cardtype.Where(p => p.is_Deleted == false).OrderBy(p => p.slm_CardTypeId).AsEnumerable().Select(p => new ControlListData { TextField = p.slm_CardTypeName, ValueField = p.slm_CardTypeId.ToString() }).ToList();
        }
    }
}

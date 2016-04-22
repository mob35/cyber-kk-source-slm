using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SLM.Resource.Data;

namespace SLM.Dal.Models
{
    public class KKSlmMsPromotionModel
    {
        private SLM_DBEntities slmdb = null;

        public KKSlmMsPromotionModel()
        {
            slmdb = new SLM_DBEntities();
        }
        public List<ControlListData> GetPromotionData(bool useWebservice)
        {
            return slmdb.kkslm_ms_promotion.OrderBy(p => p.slm_PromotionName).AsEnumerable().Select(p => new ControlListData { TextField = p.slm_PromotionName, ValueField = useWebservice ? p.slm_PromotionCode.ToString() : p.slm_PromotionId.ToString() }).ToList();
        }
    }
}

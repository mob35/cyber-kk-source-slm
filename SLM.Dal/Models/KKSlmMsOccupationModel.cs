using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SLM.Resource.Data;

namespace SLM.Dal.Models
{
    public class KKSlmMsOccupationModel
    {
        private SLM_DBEntities slmdb = null;

        public KKSlmMsOccupationModel()
        {
            slmdb = new SLM_DBEntities();
        }
        public List<ControlListData> GetOccupationData(bool useWebservice)
        {
            return slmdb.kkslm_ms_occupation.OrderBy(p => p.slm_OccupationNameTH).AsEnumerable().Select(p => new ControlListData { TextField = p.slm_OccupationNameTH, ValueField = useWebservice ? p.slm_OccupationCode.ToString() : p.slm_OccupationId.ToString() }).ToList();
        }
    }
}

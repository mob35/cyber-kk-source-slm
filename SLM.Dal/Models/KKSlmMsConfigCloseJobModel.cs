using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLM.Dal.Models
{
    public class KKSlmMsConfigCloseJobModel
    {
        private SLM_DBEntities slmdb = null;

        public KKSlmMsConfigCloseJobModel()
        {
            slmdb = new SLM_DBEntities();
        }

        public bool CheckRequireCardId(string statusCode)
        {
            var require = slmdb.kkslm_ms_config_close_job.Where(p => p.slm_Status == statusCode && p.is_Deleted == false).Select(p => p.slm_RequireCardId).FirstOrDefault();
            return require != null ? require.Value : false;
        }
    }
}

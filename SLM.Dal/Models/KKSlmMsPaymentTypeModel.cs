using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SLM.Resource.Data;

namespace SLM.Dal.Models
{
    public class KKSlmMsPaymentTypeModel
    {
        private SLM_DBEntities slmdb = null;

        public KKSlmMsPaymentTypeModel()
        {
            slmdb = new SLM_DBEntities();
        }
        public List<ControlListData> GetPaymentTypeData(bool useWebservice)
        {
            return slmdb.kkslm_ms_paymenttype.OrderBy(p => p.slm_PaymentNameTH).AsEnumerable().Select(p => new ControlListData { TextField = p.slm_PaymentNameTH, ValueField = useWebservice ? p.slm_PaymentCode.ToString() : p.slm_PaymentId.ToString() }).ToList();
        }
    }
}

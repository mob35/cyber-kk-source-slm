using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SLM.Dal.Models;
using SLM.Resource.Data;

namespace SLM.Biz
{
    public class CardTypeBiz
    {
        public static List<ControlListData> GetCardTypeList()
        {
            KKSlmMsCardTypeModel cardtype = new KKSlmMsCardTypeModel();
            return cardtype.GetCardTypeList();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SLM.Dal.Models;
using SLM.Resource.Data;

namespace SLM.Biz
{
    public class OptionBiz
    {
        public static List<ControlListData> GetOptionList(string optionType)
        {
            KKSlmMsOptionModel option = new KKSlmMsOptionModel();
            return option.GetOptionList(optionType);
        }

        public static List<ControlListData> GetOptionListForActivityConfig(string optionType)
        {
            KKSlmMsOptionModel option = new KKSlmMsOptionModel();
            return option.GetOptionListForActivityConfig(optionType);
        }

        public static List<ControlListData> GetStatusListByActivityConfig(string productId, string leadStatus)
        {
            KKSlmMsOptionModel option = new KKSlmMsOptionModel();
            return option.GetStatusListByActivityConfig(productId, leadStatus);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SLM.Resource.Data;
using SLM.Dal.Models;
using SLM.Resource;

namespace SLM.Biz
{
    public class RoleBiz
    {
        public static ScreenPrivilegeData GetScreenPrivilege(string username, string screenDesc)
        {
            KKSlmScreenModel screen = new KKSlmScreenModel();
            return screen.GetScreenPrivilege(username, screenDesc);
        }

        public static bool GetTicketIdPrivilege(string ticketId, string username, decimal? staffType)
        {
            SearchLeadModel search = new SearchLeadModel();
            List<SearchLeadResult> list = search.SearchLeadData(ticketId, "", "", "", "", "", "", "", "", "", "", username, staffType, "", "", "", "", "", "", SLMConstant.SearchOrderBy.None);
            return list.Count > 0 ? true : false;
        }
    }
}

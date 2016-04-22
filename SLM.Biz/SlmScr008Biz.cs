using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SLM.Resource.Data;
using SLM.Dal.Models;

namespace SLM.Biz
{
    public class SlmScr008Biz
    {
        public static List<ControlListData> GetLeadStatus(string optionType)
        {
            KKSlmMsOptionModel option = new KKSlmMsOptionModel();
            return option.GetOptionList(optionType);
        }

        //public static List<ControlListData> GetLeadStatusForActivity(string optionType)
        //{
        //    KKSlmMsOptionModel option = new KKSlmMsOptionModel();
        //    return option.GetOptionListForActivity(optionType);
        //}

        public static List<PhoneCallHistoryData> SearchPhoneCallHistory(string citizenId, string ticketid, bool thisLead)
        {
            SearchLeadModel search = new SearchLeadModel();
            return search.SearchPhoneCallHistory(citizenId, ticketid, thisLead);
        }

        public static void InsertPhoneCallHistory(string ticketId, string cardType, string cardId, string leadStatusCode, string oldstatus, string ownerBranch, string owner, string oldOwner, string delegateLeadBranch, string delegateLead, string oldDelegateLead, string contactPhone, string contactDetail, string createBy)
        {
            KKSlmPhoneCallModel phone = new KKSlmPhoneCallModel();
            phone.InsertPhoneCallHistory(ticketId, cardType, cardId, leadStatusCode, oldstatus, ownerBranch, owner, oldOwner, delegateLeadBranch, delegateLead, oldDelegateLead, contactPhone, contactDetail, createBy);
        }

        public static string GetLeadStatusByTicketId(string ticketId)
        {
            KKSlmTrLeadModel lead = new KKSlmTrLeadModel();
            return lead.GetLeadStatus(ticketId);
        }

        public static LeadData GetStatusAndAssignFlag(string ticketId)
        {
            KKSlmTrLeadModel lead = new KKSlmTrLeadModel();
            return lead.GetStatusAndAssignFlag(ticketId);
        }
    }
}

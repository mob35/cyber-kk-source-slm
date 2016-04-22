using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SLM.Resource.Data;
using SLM.Dal.Models;

namespace SLM.Biz
{
    public class SlmScr009Biz
    {
        public static List<NoteHistoryData> SearchNoteHistory(string ticketId)
        {
            SearchLeadModel search = new SearchLeadModel();
            return search.SearchNoteHistory(ticketId);
        }

        public static void InsertNoteHistory(string ticketId, bool sendEmail, string emailSubject, string noteDetail, List<string> emailList, string createBy)
        {
            KKSlmNoteModel note = new KKSlmNoteModel();
            note.InsertNoteHistory(ticketId, sendEmail, emailSubject, noteDetail, emailList, createBy);
        }

        public static void ChangeNoteFlag(string ticketId, bool noteFlag, string updateBy)
        {
            KKSlmTrLeadModel lead = new KKSlmTrLeadModel();
            lead.ChangeNoteFlag(ticketId, noteFlag, updateBy);
        }

        public static bool HasOwnerOrDelegate(string ticketId)
        {
            KKSlmTrLeadModel lead = new KKSlmTrLeadModel();
            return lead.HasOwnerOrDelegate(ticketId);
        }
    }
}

using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLM.Resource
{
    public class SLMConstant
    {
        public const string SystemName = "SLM";

        public static string SLMDBName
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["SLMDBName"] != null ? ConfigurationManager.AppSettings["SLMDBName"] : "SLMDB";
                }
                catch
                {
                    return "SLMDB";
                }
            }
        }

        public static class StaffType
        {
            public const decimal Manager = 1;
            public const decimal Supervisor = 2;
            public const decimal UserAdministrator = 3;
            public const decimal Telesales = 4;
            public const decimal CallCenter = 5;
            public const decimal Leader = 6;
            public const decimal ITAdministrator = 7;
            public const decimal Marketing = 8;
            public const decimal ManagerOper = 10;
            public const decimal SupervisorOper = 11;
            public const decimal Oper = 12;
        }
        public static class StatusCode
        {
            public const string Interest = "00";        //สนใจ
            public const string NoContact = "01";       //ติดต่อไม่ได้ >>> ยกเลิก...ให้ใช้ 15 แทน
            public const string ContactDoc = "02";      //ติดต่อได้ รอเอกสาร >>>ยกเลิก...ให้ใช้ 14 แทน
            public const string ContactCall = "03";     //ติดต่อได้ใ ห้โทรกลับ >>>ยกเลิก...ให้ใช้ 14 แทน
            public const string FollowDoc = "04";       //ติดตามเอกสาร >>>ยกเลิก...ให้ใช้ 14 แทน
            public const string WaitConsider = "05";    //รอผลการพิจารณา 
            public const string ApproveAccept = "06";   //อนุมัติ - ลูกค้าตกลง
            public const string ApproveEdit = "07";     //อนุมัติ - ส่งกลับแก้ไข >>>ยกเลิก...ให้ใช้ 11 แทน
            public const string Reject = "08";          //Reject
            public const string Cancel = "09";          //Cancel
            public const string Close = "10";           //ปิดงาน
            public const string RoutebackEdit = "11";           //ส่งกลับแก้ไข
            public const string COCWaitingConsider = "12";        //รอผลพิจารณา COC
            public const string COCReturn = "13";        //ส่งกลับแก้ไข COC
            public const string OnProcess = "14";       //อยู่ระหว่างดำเนินการ
            public const string WaitContact = "15";     //รอติดต่อลูกค้า 
        }
        public static class CampaignType 
        {
            public const string Mass = "01";
            public const string BelowTheLine = "02";
        }
        public static class ChannelId
        {
            public const string Branch = "BRANCH";
            public const string CallCenter = "CALLCENTER";
            public const string Telesales = "TELESALES";
            public const string PriorityBanking = "PB";
        }
        public static class ActionType
        {
            public const string SystemAssign = "01";
            public const string ChangeStatus = "02";
            public const string Delegate = "03";
            public const string Transfer = "04";
            public const string UserAssign = "05";
            public const string Consolidate = "06";
            public const string ResetOwner = "07";
            public const string UpdateOwner = "08";
            public const string EODUpdateCurrent = "09";
            public const string ChangeOwner = "10";
            public const string EODHistoryLogs = "11";
            public const string UserError = "12";
        }

        public static class Branch
        {
            public const int Active = 1;
            public const int InActive = 2;
            public const int All = 3;
        }

        public static class Position
        {
            public const int Active = 1;
            public const int InActive = 2;
            public const int All = 3;
        }

        public static class COCTeam
        {
            public const string Marketing = "MARKETING";
        }

        public static class HistoryTypeCode
        {
            public const string CreateLead = "001";
            public const string UpdateOwner = "002";
            public const string UpdateDelegate = "003";
            public const string AddCampaignFinal = "004";
            public const string UpdateStatus = "005";
            public const string UpdateLead = "006";
            public const string InsertNote = "007";
            public const string UpdateCardId = "008";
        }

        public static class SearchOrderBy
        {
            public const string None = "";
            public const string SLA = "SLA";
            public const string Note = "Note";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace SLM.Application.Utilities
{
    public static class AppConstant
    {
        public static int TextMaxLength
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["TextMaxLength"] != null ? Convert.ToInt32(ConfigurationManager.AppSettings["TextMaxLength"]) : 4000;
                }
                catch 
                { 
                    return 4000; 
                }
            }
        }

        public static int MaximumImageUploadSize
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["MaximumImageUploadSize"] != null ? Convert.ToInt32(ConfigurationManager.AppSettings["MaximumImageUploadSize"]) : 5242880;
                }
                catch
                {
                    return 5242880; //5MB
                }
            }
        }

        public static int MaximumFileUploadSize
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["MaximumFileUploadSize"] != null ? Convert.ToInt32(ConfigurationManager.AppSettings["MaximumFileUploadSize"]) : 5242880;
                }
                catch
                {
                    return 5242880; //5MB
                }
            }
        }

        public static string NoticeFolder
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["NoticeFolder"] != null ? ConfigurationManager.AppSettings["NoticeFolder"] : "";
                }
                catch
                {
                    return "";
                }
            }
        }

        public static int CMTTimeout
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["CMTTimeout"] != null ? Convert.ToInt32(ConfigurationManager.AppSettings["CMTTimeout"]) : 10;
                }
                catch
                {
                    return 10;
                }
            }
        }

        public static class Campaign
        {
            public static int DisplayCampaignDescMaxLength
            {
                get 
                {
                    try
                    {
                        return ConfigurationManager.AppSettings["DisplayCampaignDescMaxLength"] != null ? Convert.ToInt32(ConfigurationManager.AppSettings["DisplayCampaignDescMaxLength"]) : 100;
                    }
                    catch { return 100; }
                }
            }
        }

        public static class CardType
        {
            public const string Person = "1";
            public const string JuristicPerson = "2";
            public const string Foreigner = "3";
        }

        public static class OptionType
        {
            public const string LeadStatus = "lead status";
        }

    }
}
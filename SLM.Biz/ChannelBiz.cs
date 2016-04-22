using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SLM.Dal.Models;
using SLM.Resource.Data;

namespace SLM.Biz
{
    public class ChannelBiz
    {
        public static List<ControlListData> GetChannelList()
        {
            KKSlmMsChannelModel chann = new KKSlmMsChannelModel();
            return chann.GetChannelData();
        }

        public static bool CheckUserErrorInUse(string username)
        {
            KKSlmMsChannelModel chann = new KKSlmMsChannelModel();
            return chann.CheckUserErrorInUse(username);
        }

        public static bool CheckHeadStaff(string UserId,string HeadStaffId,out string  desc)
        {
            if (HeadStaffId == "0"){
                desc = null;
                return true;
            }
            else
            {
                KKSlmMsChannelModel chann = new KKSlmMsChannelModel();
                return chann.CheckHeadStaff(UserId, HeadStaffId, out   desc);
            }
        }

        //public static bool CheckUnderStaff(string UserId,string UnderStaffId ,out string  desc)
        //{
        //    KKSlmMsChannelModel chann = new KKSlmMsChannelModel();
        //    return chann.CheckUnderStaff(UserId, UnderStaffId, out   desc);
        //}
    }
}

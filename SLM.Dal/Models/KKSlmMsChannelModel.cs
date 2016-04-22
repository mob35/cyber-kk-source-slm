using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SLM.Resource.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Data.Objects;

namespace SLM.Dal.Models
{
    public class KKSlmMsChannelModel
    {
        private SLM_DBEntities slmdb = null;

        public KKSlmMsChannelModel()
        {
            slmdb = new SLM_DBEntities();
        }

        public List<ControlListData> GetChannelData()
        {
            return slmdb.kkslm_ms_channel.Where(p => p.is_Deleted == 0).OrderBy(p => p.slm_ChannelDesc).Select(p => new ControlListData { TextField = p.slm_ChannelDesc, ValueField = p.slm_ChannelId }).ToList();
        }
        public string GetChannelId(string channeldesc)
        {
            return slmdb.kkslm_ms_channel.Where(p => p.slm_ChannelDesc == channeldesc && p.is_Deleted == 0).Select(p => p.slm_ChannelId).FirstOrDefault();
        }

        public bool CheckUserErrorInUse(string username)
        {
            var count = slmdb.kkslm_ms_channel.Where(p => p.slm_UserError == username && p.is_Deleted == 0).Count();
            return count > 0 ? true : false;
        }


        public bool CheckHeadStaff(string UserId, string HeadStaffId, out string desc)
        {
            try
            {

                //SqlParameter param1 = new SqlParameter("level",System.Data.SqlDbType.BigInt, 0);
                //SqlParameter param2 = new SqlParameter("StaffId", UserId);
                //SqlParameter param3 = new SqlParameter("FindStaffId", HeadStaffId);
                //var result = slmdb.ExecuteStoreQuery<IEnumerable<string>>("exec SP_FINDHEADRECURSION 0, '" + UserId + "', '" + HeadStaffId + "'");

                //foreach (DbDataRecord rec in new ObjectQuery<DbDataRecord>("exec SP_FINDHEADRECURSION 0, '" + UserId + "', '" + HeadStaffId + "'", slmdb))
                //{
                //    string value  = rec.GetString(0);
                //}

                var result = slmdb.SP_FINDHEADERRECURSION(UserId, HeadStaffId).FirstOrDefault();

                desc = result.STAFF;

                if (desc == null)
                {
                    return true;
                }
                else {
                    return false;
                }
            }
            catch (Exception ex)
            {
                desc = ex.Message;
                return false;
            }
        }

        //public bool CheckUnderStaff(string UserId, string UnderStaffId, out string desc)
        //{
        //    var result = slmdb.SP_FINDUNDERRECURSION(0, UserId, null).FirstOrDefault();
        //    try
        //    {
        //        desc = result.Column1;
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        desc = ex.Message;
        //        return false;
        //    }
        //}


    }
}

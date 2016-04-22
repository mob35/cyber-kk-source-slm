﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SLM.Resource.Data;
using SLM.Resource;

namespace SLM.Dal.Models
{
    public class KKSlmMsStaffTypeModel
    {
        private SLM_DBEntities slmdb = null;
        private string SLMDBName = System.Configuration.ConfigurationManager.AppSettings["SLMDBName"].ToString();

        public KKSlmMsStaffTypeModel()
        {
            slmdb = new SLM_DBEntities();
        }

        public List<ControlListData> GetStaffTyeData()
        {
            decimal[] dec = { SLMConstant.StaffType.ITAdministrator };
            return slmdb.kkslm_ms_staff_type.Where(p => p.is_Deleted == 0 && dec.Contains(p.slm_StaffTypeId) == false).OrderBy(p => p.slm_StaffTypeDesc).AsEnumerable().Select(p => new ControlListData { TextField = p.slm_StaffTypeDesc, ValueField = p.slm_StaffTypeId.ToString() }).ToList();
        }

        public List<ControlListData> GetStaffTyeAllData()
        {
            return slmdb.kkslm_ms_staff_type.Where(p => p.is_Deleted == 0).OrderBy(p => p.slm_StaffTypeDesc).AsEnumerable().Select(p => new ControlListData { TextField = p.slm_StaffTypeDesc, ValueField = p.slm_StaffTypeId.ToString() }).ToList();
        }

    }
}

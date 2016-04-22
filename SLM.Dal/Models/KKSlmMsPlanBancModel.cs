﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SLM.Resource.Data;


namespace SLM.Dal.Models
{
    public class KKSlmMsPlanBancModel
    {
        private SLM_DBEntities slmdb = null;

        public KKSlmMsPlanBancModel()
        {
            slmdb = new SLM_DBEntities();
        }
        public List<ControlListData> GetPlanBancData()
        {
            return slmdb.kkslm_ms_plan_banc.OrderBy(p => p.slm_Plan_Banc_T_Desc).AsEnumerable().Select(p => new ControlListData { TextField = p.slm_Plan_Banc_T_Desc, ValueField = p.slm_Plan_Banc_Code.ToString() }).ToList();
        }
    }
}

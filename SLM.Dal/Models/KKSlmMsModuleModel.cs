﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SLM.Resource.Data;

namespace SLM.Dal.Models
{
    public class KKSlmMsModuleModel
    {
        private SLM_DBEntities slmdb = null;

        public KKSlmMsModuleModel()
        {
            slmdb = new SLM_DBEntities();
        }
        public List<ControlListData> GetModuleData(bool useWebservice)
        {
            return slmdb.kkslm_ms_module.OrderBy(p => p.slm_ModuleNameTH).AsEnumerable().Select(p => new ControlListData { TextField = p.slm_ModuleNameTH, ValueField = useWebservice ? p.slm_ModuleCode.ToString() : p.slm_ModuleId.ToString() }).ToList();
        }
    }
}

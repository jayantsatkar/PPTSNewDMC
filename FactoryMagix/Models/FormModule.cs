using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FactoryMagix.Models
{
    public class FormModule
    {
        public long Sub_Module_ID { get; set; }
        public string Sub_Module_Name { get; set; }
        public string Frm_Name { get; set; }
        public string Frm_URL { get; set; }
        public long Frm_ID { get; set; }
        public int Flag_Visible { get; set; }
        public long Frm_Order { get; set; }
        public string Module_Name { get; set; }
        public long Module_ID { get; set; }
        public Nullable<int> Module_Order { get; set; }
    }
}
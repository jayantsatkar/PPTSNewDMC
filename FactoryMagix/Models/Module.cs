using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FactoryMagix.Models
{
    public class Module
    {
        public long Module_ID { get; set; }
        public string Module_Name { get; set; }
        public string Module_Description { get; set; }
        public Nullable<int> Module_Order { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public System.DateTime Created_On { get; set; }
        public long Created_By { get; set; }
        public Nullable<System.DateTime> Modified_On { get; set; }
        public Nullable<long> Modified_By { get; set; }
    }
}
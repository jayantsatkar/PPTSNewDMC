using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FactoryMagix.Models
{
    public class Role
    {
        public long Role_ID { get; set; }
        public string Role_Name { get; set; }
        public string Role_Desc { get; set; }
        public bool IsActive { get; set; }
        public long Created_By { get; set; }
        public System.DateTime Created_On { get; set; }
        public Nullable<long> Modified_By { get; set; }
        public Nullable<System.DateTime> Modified_On { get; set; }
    }
}
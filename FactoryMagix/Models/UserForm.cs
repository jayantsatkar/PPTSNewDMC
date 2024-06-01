using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FactoryMagix.Models
{
    public class UserForm
    {
        public long Sr_No { get; set; }
        public long User_Id { get; set; }
        public long Frm_Id { get; set; }
        public bool Flag_Visible { get; set; }
        public long Created_By { get; set; }
        public DateTime Created_On { get; set; }
        public long? Modified_By { get; set; }
        public DateTime? Modified_On { get; set; }
    }
}
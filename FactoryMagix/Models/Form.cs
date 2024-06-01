using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FactoryMagix.Models
{
    public class Form
    {
        public long Frm_Id { get; set; }
        public long? Module_ID { get; set; }
        public long? Sub_Module_ID { get; set; }
        public string Frm_Name { get; set; }
        public string Frm_URL { get; set; }
        public long Frm_Order { get; set; }
        public long Created_By { get; set; }
        public DateTime? Created_On { get; set; }
    }
}
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FactoryMagix.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class MST_SubModule
    {
        public long Sub_Module_ID { get; set; }
        public string Sub_Module_Name { get; set; }
        public string Sub_Module_Description { get; set; }
        public int Sub_Module_Order { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<long> Module_ID { get; set; }
        public System.DateTime Created_On { get; set; }
        public long Created_By { get; set; }
        public Nullable<System.DateTime> Modified_On { get; set; }
        public Nullable<long> Modified_By { get; set; }
    
        public virtual MST_Module MST_Module { get; set; }
        public virtual MST_User MST_User { get; set; }
        public virtual MST_User MST_User1 { get; set; }
    }
}

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
    
    public partial class MST_Machine
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MST_Machine()
        {
            this.TRN_InvoiceDetails = new HashSet<TRN_InvoiceDetails>();
            this.TRN_ScanPartBarcode = new HashSet<TRN_ScanPartBarcode>();
        }
    
        public long MST_Machine_ID { get; set; }
        public string Machine_No { get; set; }
        public string Machine_Name { get; set; }
        public string IPAddress { get; set; }
        public Nullable<long> Created_By { get; set; }
        public Nullable<System.DateTime> Created_On { get; set; }
        public Nullable<long> Modified_By { get; set; }
        public Nullable<System.DateTime> Modified_On { get; set; }
    
        public virtual MST_User MST_User { get; set; }
        public virtual MST_User MST_User1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TRN_InvoiceDetails> TRN_InvoiceDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TRN_ScanPartBarcode> TRN_ScanPartBarcode { get; set; }
    }
}

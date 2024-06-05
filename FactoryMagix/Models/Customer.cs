using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FactoryMagix.Models
{
    public class Customer
    {
        public long MST_Customer_ID { get; set; }
        public string Customer_Code { get; set; }
        public string Customer_Name { get; set; }
        public string Customer_Index { get; set; }
        public string Address_Line1 { get; set; }
        public string Address_Line2 { get; set; }
        public string PhoneNo { get; set; }
        public string FaxNo { get; set; }
        public string EmailId { get; set; }
        public Nullable<long> Created_By { get; set; }
        public Nullable<System.DateTime> Created_On { get; set; }
        public Nullable<long> Modified_By { get; set; }
        public Nullable<System.DateTime> Modified_On { get; set; }

        public Customer()
        {
            Customer_Code = string.Empty;
            Customer_Name = string.Empty;
            Customer_Index = string.Empty;
            Address_Line1 = string.Empty;
            Address_Line2 = string.Empty;
            PhoneNo = string.Empty;
            FaxNo = string.Empty;
            EmailId = string.Empty;
        }
    }
    
    
}
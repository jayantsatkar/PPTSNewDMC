using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FactoryMagix.Models
{
    public class User
    {
        public long User_ID { get; set; }
        public long Role_ID { get; set; }
        public string Login_ID { get; set; }
        public string Password { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Middle_Name { get; set; }
        public string EmailId { get; set; }
        public byte[] User_Image { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Pincode { get; set; }
        public string Mobile_No { get; set; }
        public string EmployeeId { get; set; }
        public Nullable<bool> IsFingerPrint { get; set; }
        public bool IsActive { get; set; }
        public long Created_By { get; set; }
        public System.DateTime Created_On { get; set; }
        public Nullable<long> Modified_By { get; set; }
        public Nullable<System.DateTime> Modified_On { get; set; }

    }
}
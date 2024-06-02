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

        public string Role_Name { get; set; }
        
        public string Login_ID { get; set; }
        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Middle_Name { get; set; }
        public string EmailId { get; set; }
       // public byte[] User_Image { get; set; }
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

        public User()
        {
            Role_Name = string.Empty;
            Login_ID = string.Empty;
            Password = string.Empty;
            ConfirmPassword = string.Empty;
            First_Name = string.Empty;
            Last_Name = string.Empty;
            Middle_Name = string.Empty;
            EmailId = string.Empty;
             Address = string.Empty;
            City = string.Empty;
            State = string.Empty;
            Country = string.Empty;
            Pincode = string.Empty;
            Mobile_No = string.Empty;
            EmployeeId = string.Empty;
        }

    }
}
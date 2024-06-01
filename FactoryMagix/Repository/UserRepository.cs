using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using FactoryMagix.Models;
using System.Data;

namespace FactoryMagix
{
    public class UserRepository
    {
        public static IList<User> GetUsersWithRole(int RoleId)
        {

            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            DBHelper.AddSqlParameter("@RoleId", RoleId, ref sqlParameters);
            
              
            var dtUsers = DBHelper.ExecuteProcedure("usp_GetUserWithRole", sqlParameters);
            List<User> users = new List<User>();

            if (dtUsers.Rows.Count > 0)
            {
                foreach (DataRow dr in dtUsers.Rows)
                {
                    users.Add(new User
                    {
                        User_ID = Convert.ToInt64(dr["User_ID"]),
                        Role_ID = Convert.ToInt64(dr["Role_ID"]),
                        Login_ID = Convert.ToString(dr["Login_ID"]),
                        Password = Convert.ToString(dr["Password"]),
                        First_Name = Convert.ToString(dr["First_Name"]),
                        Last_Name = Convert.ToString(dr["Last_Name"]),
                        Middle_Name = Convert.ToString(dr["Middle_Name"]),
                        EmailId = Convert.ToString(dr["EmailId"]),
                        //User_Image = (Byte[])(dr["User_Image"]),
                        Address = Convert.ToString(dr["Address"]),
                        City = Convert.ToString(dr["City"]),
                        State = Convert.ToString(dr["State"]),
                        Country = Convert.ToString(dr["Country"]),
                        Pincode = Convert.ToString(dr["Pincode"]),
                        Mobile_No = Convert.ToString(dr["Mobile_No"]),
                        EmployeeId = Convert.ToString(dr["EmployeeId"]),
                        IsFingerPrint = Convert.ToBoolean(dr["IsFingerPrint"]),
                        IsActive = Convert.ToBoolean(dr["IsActive"]),
                        Created_By = Convert.ToInt64(dr["Created_By"]),
                        Created_On = Convert.ToDateTime(dr["Created_On"]),
                    });
                }

            }

            return users;
          
        }
    }
}
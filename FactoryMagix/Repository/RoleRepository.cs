using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using FactoryMagix.Models;
using System.Data;
using Newtonsoft.Json;

namespace FactoryMagix
{
    public class RoleRepository
    {
        public static IList<Role> GetRoles()
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
                     
            var dtRoles = DBHelper.ExecuteProcedure("usp_GetRoles", sqlParameters);
            List<Role> roles = new List<Role>();

            if (dtRoles.Rows.Count > 0)
            {
                foreach (DataRow dr in dtRoles.Rows)
                {
                    roles.Add(new Role{
                        Role_ID = Convert.ToInt64(dr["Role_ID"]),
                        Role_Name = Convert.ToString(dr["Role_Name"]),
                        Role_Desc = Convert.ToString(dr["Role_Desc"]),
                        IsActive = Convert.ToBoolean(dr["IsActive"]),
                        Created_By = Convert.ToInt64(dr["Created_By"]),
                        Created_On = Convert.ToDateTime(dr["Created_On"])});
                }
               
            }

            return roles;
        }

        public static Role GetRole(long RoleId)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();

            DBHelper.AddSqlParameter("@RoleId", RoleId, ref sqlParameters);
            var dtRoles = DBHelper.ExecuteProcedure("usp_GetRole", sqlParameters);
            Role role = new Role();

            if (dtRoles.Rows.Count > 0)
            {
                foreach (DataRow dr in dtRoles.Rows)
                {

                    role.Role_ID = Convert.ToInt64(dr["Role_ID"]);
                    role.Role_Name = Convert.ToString(dr["Role_Name"]);
                    role.Role_Desc = Convert.ToString(dr["Role_Desc"]);
                    role.IsActive = Convert.ToBoolean(dr["IsActive"]);
                    role.Created_By = Convert.ToInt64(dr["Created_By"]);
                    role.Created_On = Convert.ToDateTime(dr["Created_On"]);
                }

            }

            return role;
        }

        public static void AddUpdateRole(Role role)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            
            DBHelper.AddSqlParameter("@JSON", JsonConvert.SerializeObject(role, Formatting.Indented), ref sqlParameters);
            DBHelper.ExecuteNonQuery("usp_AddUpdateRole", sqlParameters);
        }


        


    }
}
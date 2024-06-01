using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using FactoryMagix.Models;
using System.Data;

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
    }
}
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using FactoryMagix.Models;
//using System.Data.SqlClient;
//using System.Data;
//namespace FactoryMagix
//{
//    public class test
//    {
//    }

//    public class RoleRepository
//    {
//        public static IList<Role> GetRoles()
//        {
//            List<SqlParameter> sqlParameters = new List<SqlParameter>();

//            var dataTableUser = DBHelper.ExecuteProcedure("usp_GetRoles", sqlParameters);
//            List<Role> roles = new List<Role>();

//            return roles;
//        }
//    }
//}
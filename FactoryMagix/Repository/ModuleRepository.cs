using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using FactoryMagix.Models;
using System.Data;

namespace FactoryMagix.Repository
{
    public class ModuleRepository
    {
        public static IList<Module> GetModules()
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();

            var dtModules = DBHelper.ExecuteProcedure("usp_GetModules", sqlParameters);
            List<Module> modules = new List<Module>();

            if (dtModules.Rows.Count > 0)
            {
                foreach (DataRow dr in dtModules.Rows)
                {
                    modules.Add(new Module
                    {
                        Module_ID = Convert.ToInt64(dr["Module_ID"]),
                        Module_Name = Convert.ToString(dr["Module_Name"]),
                        Module_Order = Convert.ToInt32(dr["Module_Order"]),
                        IsActive = Convert.ToBoolean(dr["IsActive"]),
                        Created_By = Convert.ToInt64(dr["Created_By"]),
                        Created_On = Convert.ToDateTime(dr["Created_On"])
                    });
                }

            }

            return modules;
        }

        public static IList<UserForm> GetUserForms()
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();

            var dtUserForm = DBHelper.ExecuteProcedure("usp_GetUserForms", sqlParameters);
            List<UserForm> userForms = new List<UserForm>();

            if (dtUserForm.Rows.Count > 0)
            {
                foreach (DataRow dr in dtUserForm.Rows)
                {
                    userForms.Add(new UserForm
                    {
                        Sr_No = Convert.ToInt64(dr["Sr_No"]),
                        User_Id = Convert.ToInt64(dr["User_Id"]),
                        Frm_Id = Convert.ToInt64(dr["Frm_Id"]),
                        Flag_Visible = Convert.ToBoolean(dr["Flag_Visible"]),
                        Created_By = Convert.ToInt64(dr["Created_By"]),
                        Created_On = Convert.ToDateTime(dr["Created_On"])

                    });
                }

            }

            return userForms;
        }

        public static IList<UserForm> GetUserForm(long UserId, long FormId)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            DBHelper.AddSqlParameter("@UserId", UserId, ref sqlParameters);
            DBHelper.AddSqlParameter("@FormId", FormId, ref sqlParameters);
            var dtUserForm = DBHelper.ExecuteProcedure("usp_GetUserForm", sqlParameters);
            List<UserForm> userForms = new List<UserForm>();

            if (dtUserForm.Rows.Count > 0)
            {
                foreach (DataRow dr in dtUserForm.Rows)
                {
                    userForms.Add(new UserForm
                    {
                        Sr_No = Convert.ToInt64(dr["Sr_No"]),
                        User_Id = Convert.ToInt64(dr["User_Id"]),
                        Frm_Id = Convert.ToInt64(dr["Frm_Id"]),
                        Flag_Visible = Convert.ToBoolean(dr["Flag_Visible"]),
                        Created_By = Convert.ToInt64(dr["Created_By"]),
                        Created_On = Convert.ToDateTime(dr["Created_On"])

                    });
                }

            }

            return userForms;
        }


        public static IList<Form> GetForms()
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();

            var dtForms = DBHelper.ExecuteProcedure("usp_GetForms", sqlParameters);
            List<Form> forms = new List<Form>();

            if (dtForms.Rows.Count > 0)
            {
                foreach (DataRow dr in dtForms.Rows)
                {
                    forms.Add(new Form
                    {
                        Frm_Id = Convert.ToInt64(dr["Frm_Id"]),
                        Module_ID = Convert.ToInt64(dr["Module_ID"]),
                        Sub_Module_ID = Convert.ToInt64(dr["Sub_Module_ID"]),
                        Frm_Name = Convert.ToString(dr["Frm_Name"]),
                        Frm_URL = Convert.ToString(dr["Frm_URL"]),
                        Frm_Order = Convert.ToInt64(dr["Frm_Order"]),
                        Created_By = Convert.ToInt64(dr["Created_By"]),
                        Created_On = Convert.ToDateTime(dr["Created_On"])
                    });
                }

            }

            return forms;
        }
    }
}
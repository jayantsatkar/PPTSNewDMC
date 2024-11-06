using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using FactoryMagix.Models;
using System.Data;
using Newtonsoft.Json;

namespace FactoryMagix.Repository
{
    public class CustomerRepository
    {
        public static IList<Customer> GetCustomers()
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();

            var dtCustomers = DBHelper.ExecuteProcedure("usp_GetCustomers", sqlParameters);
            List<Customer> customers = new List<Customer>();

            if (dtCustomers.Rows.Count > 0)
            {
                foreach (DataRow dr in dtCustomers.Rows)
                {
                    customers.Add(new Customer
                    {
                        //MST_Customer_ID = Convert.ToInt64(dr["MST_Customer_ID"]),
                        //Customer_Code = Convert.ToString(dr["Customer_Code"]),
                        //Customer_Name = Convert.ToString(dr["Customer_Name"]),
                        //Customer_Index = Convert.ToString(dr["Customer_Index"]),
                        //Address_Line1 = Convert.ToString(dr["Address_Line1"]),
                        //Address_Line2 = Convert.ToString(dr["Address_Line2"]),
                        //PhoneNo = Convert.ToString(dr["PhoneNo"]),
                        //FaxNo = Convert.ToString(dr["FaxNo"]),
                        //EmailId = Convert.ToString(dr["EmailId"]),
                        //Created_By = Convert.ToInt64(dr["Created_By"])

                        MST_Customer_ID = dr["MST_Customer_ID"] == DBNull.Value ? 0 : Convert.ToInt64(dr["MST_Customer_ID"]),
                        Customer_Code = dr["Customer_Code"] == DBNull.Value ? string.Empty : Convert.ToString(dr["Customer_Code"]),
                        Customer_Name = dr["Customer_Name"] == DBNull.Value ? string.Empty : Convert.ToString(dr["Customer_Name"]),
                        Customer_Index = dr["Customer_Index"] == DBNull.Value ? string.Empty : Convert.ToString(dr["Customer_Index"]),
                        Address_Line1 = dr["Address_Line1"] == DBNull.Value ? string.Empty : Convert.ToString(dr["Address_Line1"]),
                        Address_Line2 = dr["Address_Line2"] == DBNull.Value ? string.Empty : Convert.ToString(dr["Address_Line2"]),
                        PhoneNo = dr["PhoneNo"] == DBNull.Value ? string.Empty : Convert.ToString(dr["PhoneNo"]),
                        FaxNo = dr["FaxNo"] == DBNull.Value ? string.Empty : Convert.ToString(dr["FaxNo"]),
                        EmailId = dr["EmailId"] == DBNull.Value ? string.Empty : Convert.ToString(dr["EmailId"]),
                        Created_By = dr["Created_By"] == DBNull.Value ? 0 : Convert.ToInt64(dr["Created_By"])

                    });
                }

            }

            return customers;
        }


        public static Customer GetCustomer(long CustomerId)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();

            DBHelper.AddSqlParameter("@CustomerId", CustomerId, ref sqlParameters);
            var dtCustomer = DBHelper.ExecuteProcedure("usp_GetCustomer", sqlParameters);
            Customer customer = new Customer();

            if (dtCustomer.Rows.Count > 0)
            {
                foreach (DataRow dr in dtCustomer.Rows)
                {

                    customer.MST_Customer_ID = Convert.ToInt64(dr["MST_Customer_ID"]);
                    customer.Customer_Code = Convert.ToString(dr["Customer_Code"]);
                    customer.Customer_Name = Convert.ToString(dr["Customer_Name"]);
                    customer.Customer_Index = Convert.ToString(dr["Customer_Index"]);
                    customer.Address_Line1 = Convert.ToString(dr["Address_Line1"]);
                    customer.Address_Line2 = Convert.ToString(dr["Address_Line2"]);
                    customer.PhoneNo = Convert.ToString(dr["PhoneNo"]);
                    customer.FaxNo = Convert.ToString(dr["FaxNo"]);
                    customer.EmailId = Convert.ToString(dr["EmailId"]);
                    customer.Created_By = Convert.ToInt64(dr["Created_By"]);
                }

            }

            return customer;
        }


        public static void AddUpdateCustomer(Customer customer)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();

            DBHelper.AddSqlParameter("@JSON", JsonConvert.SerializeObject(customer, Formatting.Indented), ref sqlParameters);
            DBHelper.ExecuteNonQuery("usp_AddUpdateCustomer", sqlParameters);
        }


    }
}
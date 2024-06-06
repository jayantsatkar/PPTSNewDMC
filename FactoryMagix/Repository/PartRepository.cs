using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using FactoryMagix.Models;
using System.Data;
using Newtonsoft.Json;
using System.IO;
using System.Configuration;
using System.Text;

namespace FactoryMagix.Repository
{
    public class PartRepository
    {
        public static DataTable GetPartsWithKeyValue()
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            var dtUsers = DBHelper.ExecuteProcedure("spGetAllPartNowithIndex", sqlParameters);

            return dtUsers;

        }

        public static List<PartConfiguration> GetParts()
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            var dtUsers = DBHelper.ExecuteProcedure("usp_GetParts", sqlParameters);

          
            List<PartConfiguration> partConfigurations = new List<PartConfiguration>();

            if (dtUsers.Rows.Count > 0)
            {
                foreach (DataRow dr in dtUsers.Rows)
                {
                    partConfigurations.Add(new PartConfiguration
                    {
                        MST_PartConfiguration_ID = Convert.ToInt32(dr["MST_PartConfiguration_ID"]),
                        BoschPart_No = Convert.ToString(dr["BoschPart_No"]),
                        Customer_Index = Convert.ToString(dr["Customer_Index"]),
                        MST_Customer_ID = Convert.ToInt32(dr["MST_Customer_ID"]),
                        BoschPart_Desc = Convert.ToString(dr["BoschPart_Desc"]),
                        CustPart_No = Convert.ToString(dr["CustPart_No"]),
                        NoOfPartQy_Box = Convert.ToInt32(dr["NoOfPartQy_Box"]),
                        Packed_In = Convert.ToString(dr["Packed_In"]),
                        Line_Id = Convert.ToString(dr["Line_Id"]),
                        Line_No = Convert.ToString(dr["Line_No"]),
                        Created_By = Convert.ToInt32(dr["Created_By"]),
                        Customer_Name= Convert.ToString(dr["Customer_Name"]),
                        Customer_Code = Convert.ToString(dr["Customer_Code"])
                        //Modified_By = Convert.ToInt32(dr["Modified_By"])

                    });
                }

            }

            return partConfigurations;
           
        }

        public static PartConfiguration GetPartDetails(int PartId)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            DBHelper.AddSqlParameter("@PartId", PartId, ref sqlParameters);
            var dtUsers = DBHelper.ExecuteProcedure("usp_GetPartDetails", sqlParameters);


            PartConfiguration partConfiguration = new PartConfiguration();

            if (dtUsers.Rows.Count > 0)
            {
                foreach (DataRow dr in dtUsers.Rows)
                {

                    partConfiguration.MST_PartConfiguration_ID = Convert.ToInt32(dr["MST_PartConfiguration_ID"]);
                    partConfiguration.BoschPart_No = Convert.ToString(dr["BoschPart_No"]);
                    partConfiguration.Customer_Index = Convert.ToString(dr["Customer_Index"]);
                    partConfiguration.MST_Customer_ID = Convert.ToInt32(dr["MST_Customer_ID"]);
                    partConfiguration.BoschPart_Desc = Convert.ToString(dr["BoschPart_Desc"]);
                    partConfiguration.CustPart_No = Convert.ToString(dr["CustPart_No"]);
                    partConfiguration.NoOfPartQy_Box = Convert.ToInt32(dr["NoOfPartQy_Box"]);
                    partConfiguration.Packed_In = Convert.ToString(dr["Packed_In"]);
                    partConfiguration.Line_Id = Convert.ToString(dr["Line_Id"]);
                    partConfiguration.Line_No = Convert.ToString(dr["Line_No"]);
                    partConfiguration.Created_By = Convert.ToInt32(dr["Created_By"]);
                    partConfiguration.Customer_Name = Convert.ToString(dr["Customer_Name"]);
                    partConfiguration.Customer_Code = Convert.ToString(dr["Customer_Code"]);
                }

            }

            return partConfiguration;

        }

        public static DataTable GetPart(int PartId)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            DBHelper.AddSqlParameter("@pPartConfiguration_Id", PartId, ref sqlParameters);
            

            var dtUsers = DBHelper.ExecuteProcedure("spGetPartDetails", sqlParameters);

            return dtUsers;

        }

        public static DataTable ValidatePartNumber(string PartNo, string BatchNo, string SerialNo,int PartId)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            DBHelper.AddSqlParameter("@PartNo", PartNo, ref sqlParameters);
            DBHelper.AddSqlParameter("@BatchNo", BatchNo, ref sqlParameters);
            DBHelper.AddSqlParameter("@SerialNo", SerialNo, ref sqlParameters);
            DBHelper.AddSqlParameter("@PartConfigID", PartId, ref sqlParameters);


            var dtPart = DBHelper.ExecuteProcedure("spCheckPartNoScaned", sqlParameters);

            return dtPart;

        }

        public static PartResult ValidatePartNumberNewDMC(string ScannedBarcode, string PartNo)
        {
            PartResult result = new PartResult();

            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            DBHelper.AddSqlParameter("@SCANNEDBARCODE", ScannedBarcode, ref sqlParameters);
            DBHelper.AddSqlParameter("@PartNo", PartNo, ref sqlParameters);
            var dt = DBHelper.ExecuteProcedure("usp_CheckPartNoScaned", sqlParameters);


            if (dt != null && dt.Rows.Count > 0)
            {
                result.RESULT = Convert.ToInt32(dt.Rows[0]["Result"]);
                result.PartNo = Convert.ToString(dt.Rows[0]["PartNo"]);
                result.PartBatchCode = Convert.ToString(dt.Rows[0]["PartBatchCode"]);
                result.PartSerialNo = Convert.ToString(dt.Rows[0]["PartSerialNo"]);
            }
            return result;

        }

        public static DataTable ValidatePartwithCustomer (string custName, string custIndex, string custCode, string partNo)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            DBHelper.AddSqlParameter("@CustName", custName, ref sqlParameters);
            DBHelper.AddSqlParameter("@CustIndex", custIndex, ref sqlParameters);
            DBHelper.AddSqlParameter("@CustCode", custCode, ref sqlParameters);
            DBHelper.AddSqlParameter("@PartNo", partNo, ref sqlParameters);

            var dt = DBHelper.ExecuteProcedure("spCheckPartconfiguration", sqlParameters);

            return dt;
        }

        public static void AddUpdatePartConfiguration(PartConfiguration partConfiguration)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();

            DBHelper.AddSqlParameter("@JSON", JsonConvert.SerializeObject(partConfiguration, Formatting.Indented), ref sqlParameters);
            DBHelper.ExecuteNonQuery("usp_AddUpdatePartConfiguration", sqlParameters);
        }

        public static void ImportCSV(string BoschPartNo, string CustIndex,int MST_Customer_Id, string PartDesc, string PartSerialNo , string CustPartNo,int NoOfParts, string PackedIn, string LineId, string LineNo, int CreatedBy)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            DBHelper.AddSqlParameter("@BoschPartNo", BoschPartNo, ref sqlParameters);
            DBHelper.AddSqlParameter("@CustIndex", CustIndex, ref sqlParameters);
            DBHelper.AddSqlParameter("@MST_Customer_Id", MST_Customer_Id, ref sqlParameters);
            DBHelper.AddSqlParameter("@partdesc", PartDesc, ref sqlParameters);
            DBHelper.AddSqlParameter("@PartSerialNo", PartSerialNo, ref sqlParameters);
            DBHelper.AddSqlParameter("@NoOfParts", NoOfParts, ref sqlParameters);
            DBHelper.AddSqlParameter("@PackedIn", PackedIn, ref sqlParameters);
            DBHelper.AddSqlParameter("@LineId", LineId, ref sqlParameters);
            DBHelper.AddSqlParameter("@LineNo", LineNo, ref sqlParameters);
            DBHelper.AddSqlParameter("@created_By", CreatedBy, ref sqlParameters);
            DBHelper.AddSqlParameter("@CustIndex", CustIndex, ref sqlParameters);

            DBHelper.ExecuteNonQuery("spInsertPartConfig", sqlParameters);

        }

        public static DataTable CheckPartConfiguration (string CustName, string CustIndex, string CustCode, string PartNo)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            DBHelper.AddSqlParameter("@CustName", CustName, ref sqlParameters);
            DBHelper.AddSqlParameter("@CustIndex", CustIndex, ref sqlParameters);
            DBHelper.AddSqlParameter("@CustCode", CustCode, ref sqlParameters);
            DBHelper.AddSqlParameter("@PartNo", PartNo, ref sqlParameters);
            DataTable dt = DBHelper.ExecuteProcedure("spCheckPartconfiguration", sqlParameters);
            return dt;
        }


    }
}
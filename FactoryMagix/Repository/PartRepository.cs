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
        public static List<PartResult> GetPartsWithKeyValue()
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            var dataTable = DBHelper.ExecuteProcedure("spGetAllPartNowithIndex", sqlParameters);
            List<PartResult> partResults = new List<PartResult>();
            
            if(dataTable != null && dataTable.Rows.Count > 0)
            {
                foreach(DataRow dr in dataTable.Rows)
                {
                    partResults.Add(new PartResult
                    {
                        MST_PartConfiguration_ID = Convert.ToInt32(dr["MST_PartConfiguration_ID"]),
                        PartNo = Convert.ToString(dr["PartNo"])
                    });
                }
                
            }
            return partResults;

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
                    partConfiguration.Customer_Index = Convert.ToString(dr["Customer_Index"]);
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
                result.Result = Convert.ToInt32(dt.Rows[0]["Result"]);
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

        public static DataTable SaveInTemp(int PartConfigId, int Partqty,int MachineId, int UserId,string PartNo, string PartBatchCode, string PartSerialNo,string Code)
        {

            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            DBHelper.AddSqlParameter("@PartConfig_Id", PartConfigId, ref sqlParameters);
            DBHelper.AddSqlParameter("@PartActualQty", Partqty, ref sqlParameters);
            DBHelper.AddSqlParameter("@MachineId", MachineId, ref sqlParameters);
            DBHelper.AddSqlParameter("@created_By", UserId, ref sqlParameters);
            DBHelper.AddSqlParameter("@BoschPartNo", PartNo, ref sqlParameters);
            DBHelper.AddSqlParameter("@PartBatchCode", PartBatchCode, ref sqlParameters);
            DBHelper.AddSqlParameter("@PartSerialNo", PartSerialNo, ref sqlParameters);
            DBHelper.AddSqlParameter("@Code", Code, ref sqlParameters);
            DataTable dt = DBHelper.ExecuteProcedure("spInsertBoxLable_Verify", sqlParameters);
            return dt;
        }

        public static List<ToyotaPart> GetBoschPartNoFromCustPartNo(string ToyotaPartInDB, string Kanban)
        {

            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            DBHelper.AddSqlParameter("@custPartNo", ToyotaPartInDB, ref sqlParameters);
            DBHelper.AddSqlParameter("@kanbanId", Kanban, ref sqlParameters);
            List<ToyotaPart> toyotaParts = new List<ToyotaPart>();
             DataTable dataTable = DBHelper.ExecuteProcedure("spGetBoschPartNoFromCustPartNo", sqlParameters);

            if(dataTable != null && dataTable.Rows.Count>0){
                toyotaParts.Add(new ToyotaPart
                {
                    Bosch_PartNo = Convert.ToString(dataTable.Rows[0]["Bosch_PartNo"]),
                    Customer_PartNo = Convert.ToString(dataTable.Rows[0]["Customer_PartNo"]),
                    Kanban_ID = Convert.ToString(dataTable.Rows[0]["Kanban_ID"])
                });
            }
            return toyotaParts;
        }

        public static DataTable SaveBoxErrorLogs(string LoginId,int PartConfigNo, string ApprovedBy, string ErrorDescription)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
      
            DBHelper.AddSqlParameter("@LoginUserId", LoginId, ref sqlParameters);
            DBHelper.AddSqlParameter("@PartConfigNo", PartConfigNo, ref sqlParameters);
            DBHelper.AddSqlParameter("@Approved_By", ApprovedBy, ref sqlParameters);
            DBHelper.AddSqlParameter("@ErrorDescription", ErrorDescription, ref sqlParameters);
           
            DataTable dataTable = DBHelper.ExecuteProcedure("spInsertUserErrorLog", sqlParameters);

           
            return dataTable;
        }

        public static DataTable SavePalletErrorLogs(string LoginId, int PartConfigNo, string ApprovedBy, string ErrorDescription, string invoiceNo, string invoiceDate, int invoiceQty)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();

            DBHelper.AddSqlParameter("@LoginUserId", LoginId, ref sqlParameters);
            DBHelper.AddSqlParameter("@PartConfigNo", PartConfigNo, ref sqlParameters);
            DBHelper.AddSqlParameter("@Approved_By", ApprovedBy, ref sqlParameters);
            DBHelper.AddSqlParameter("@ErrorDescription", ErrorDescription, ref sqlParameters);
            DBHelper.AddSqlParameter("@InvoiceNo", invoiceNo, ref sqlParameters);
            DBHelper.AddSqlParameter("@InvoiceDate", invoiceDate, ref sqlParameters);
            DBHelper.AddSqlParameter("@InvoiceQty", invoiceQty, ref sqlParameters);

            DataTable dataTable = DBHelper.ExecuteProcedure("spInsertUserErrorLog_2", sqlParameters);


            return dataTable;
        }




    }
}
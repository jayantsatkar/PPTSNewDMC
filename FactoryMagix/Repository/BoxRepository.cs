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
    public class BoxRepository
    {
        public static DataTable GetBoxReprintDetails(string serialNo, int flag)
        {

            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            DBHelper.AddSqlParameter("@SerialNo", serialNo, ref sqlParameters);
            DBHelper.AddSqlParameter("@FlagforPalletorBox", flag, ref sqlParameters);


            var dtUsers = DBHelper.ExecuteProcedure("spGetBarcodeDataprint", sqlParameters);


            return dtUsers;

        }

        public static void CreatePRNFile(string DestinationFolderPath, DataTable dt, int labeltype, string UserName)
        {
            if (!Directory.Exists(DestinationFolderPath))
            {
                Directory.CreateDirectory(DestinationFolderPath);
            }


            if ((int)LabelType.Box == labeltype && dt.Rows.Count > 0)
            {

                string CustomerName1;
                string CustomerName2;

                //User user = (User)Session["UserInfo"];
                string CustomerName = Convert.ToString(dt.Rows[0]["Customer_Name"]),
                    Createddate = Convert.ToString(dt.Rows[0]["Created_On"]),
                    BoxSerialNo = Convert.ToString(dt.Rows[0]["BoxSerial_No"]),
                    PartDesc = Convert.ToString(dt.Rows[0]["BoschPart_Desc"]),
                    PartNo = Convert.ToString(dt.Rows[0]["partNo"]),
                    boxbarcode = Convert.ToString(dt.Rows[0]["BoxBarcode"]),

                    PartQty = Convert.ToString(dt.Rows[0]["ActualPart_Qty"]),
                    UserLoginId = UserName,//Convert.ToString(dt.Rows[0]["UserLoginId"]),
                    Shift = GetShift(); //Convert.ToString(dt.Rows[0]["Shift"]);


                try
                {
                    long result = 0;

                    string prnPath = @ConfigurationManager.AppSettings["BoxLabelPrn"].ToString();
                    if (File.Exists(prnPath))
                    {
                        // string strVehicleNo = string.Empty;
                        StringBuilder newFile = new StringBuilder();
                        StringBuilder strbuild = new StringBuilder();
                        TextReader tr = new StreamReader(prnPath);
                        long stringlen = CustomerName.Length;
                        string strleftstring = string.Empty;
                        if (stringlen <= (long)23)
                        {
                            CustomerName1 = CustomerName.Substring(0);
                            CustomerName2 = "";
                        }
                        else if (stringlen == (long)35)
                        {
                            CustomerName1 = CustomerName.Substring(0, 22);
                            CustomerName2 = CustomerName.Substring(22).Substring(0);
                        }
                        else if (stringlen != (long)34)
                        {
                            CustomerName1 = CustomerName.Substring(0, 23);
                            CustomerName2 = CustomerName.Substring(23).Substring(0);
                        }
                        else
                        {
                            CustomerName1 = CustomerName.Substring(0, 17);
                            CustomerName2 = CustomerName.Substring(17).Substring(0);
                        }
                        // OperShift = GetShift();
                        do
                        {
                            string strtxt = ReplaceAllValues("Customer Name1", CustomerName1, tr.ReadLine());
                            strtxt = ReplaceAllValues("Customer Name2", CustomerName2, strtxt);
                            strtxt = ReplaceAllValues("DateTime", Createddate, strtxt);
                            strtxt = ReplaceAllValues("BoxSrNo", BoxSerialNo, strtxt);
                            strtxt = ReplaceAllValues("PartDesc", PartDesc, strtxt);
                            strtxt = ReplaceAllValues("PartNo", PartNo, strtxt);
                            strtxt = ReplaceAllValues("BarCode", boxbarcode, strtxt);
                            strtxt = ReplaceAllValues("qty", PartQty.ToString(), strtxt);
                            strtxt = ReplaceAllValues("PackerName", UserLoginId, strtxt);
                            strtxt = ReplaceAllValues("sft", Shift, strtxt);
                            strbuild.Append(strtxt);
                            strbuild.Append(Environment.NewLine);
                        }
                        while (tr.Peek() != -1);
                        tr.Close();
                        //string newfile = //@ConfigurationManager.AppSettings["TempBoxLabelPrn"].ToString();
                       
                        string newfile = Path.Combine(DestinationFolderPath, "BoxLabel.prn");

                        if (File.Exists(newfile))
                        {
                            File.Delete(newfile);
                        }
                        StreamWriter w = (new FileInfo(newfile)).CreateText();
                        w.WriteLine(strbuild);
                        w.Close();
                        //if (File.Exists(newfile))
                        //{
                        //    //result = Print_Old(newfile);
                        //    if (result == 1)
                        //    {
                        //        Console.ForegroundColor = ConsoleColor.Green;
                        //        Console.WriteLine("Box Label printed successfully");
                        //    }

                        //    else
                        //    {
                        //        Console.ForegroundColor = ConsoleColor.Red;
                        //        Console.WriteLine("Error: Box Label printed successfully");
                        //    }
                        //   // File.Delete(newfile);
                        //}
                    }
                }
                catch (Exception exception)
                {
                    //  Logger.Fatal("Error", exception);
                }

            }
        }

        private static string ReplaceAllValues(string NameOfTag, string DbColumnName, string StrLine)
        {
            string empty;
            try
            {
                if (StrLine.Contains(NameOfTag))
                {
                    if (DbColumnName == "")
                    {
                        DbColumnName = "";
                    }
                    StrLine = StrLine.Replace(NameOfTag, DbColumnName);
                }
            }
            catch (Exception exception)
            {
                empty = string.Empty;
                return empty;
            }
            empty = StrLine;
            return empty;
        }

        private static string GetShift()
        {
            string shift = "";
            if (DateTime.Now.Hour >= 7 && DateTime.Now.Hour < 15)
            {
                shift = "A";
            }

            else if (DateTime.Now.Hour >= 15 && DateTime.Now.Hour < 23)
            {
                shift = "B";
            }

            else if (DateTime.Now.Hour < 7)
            {
                shift = "C";
            }
            return shift;



        }

        public static DataTable SaveBoxDetailsTemporaryNewDMC(long PartConfigId, long partqty, int MachineId, int UserId, string PartNo, string partbatchcode, string partserialno, string Code)
        {
            //var partConfig_IdParameter = partConfig_Id.HasValue ?
            //    new ObjectParameter("PartConfig_Id", partConfig_Id) :
            //    new ObjectParameter("PartConfig_Id", typeof(long));

            //var partActualQtyParameter = partActualQty.HasValue ?
            //    new ObjectParameter("PartActualQty", partActualQty) :
            //    new ObjectParameter("PartActualQty", typeof(long));

            //var machineIdParameter = machineId.HasValue ?
            //    new ObjectParameter("MachineId", machineId) :
            //    new ObjectParameter("MachineId", typeof(long));

            //var created_ByParameter = created_By.HasValue ?
            //    new ObjectParameter("created_By", created_By) :
            //    new ObjectParameter("created_By", typeof(long));

            //var boschPartNoParameter = boschPartNo != null ?
            //    new ObjectParameter("BoschPartNo", boschPartNo) :
            //    new ObjectParameter("BoschPartNo", typeof(string));

            //var partBatchCodeParameter = partBatchCode != null ?
            //    new ObjectParameter("PartBatchCode", partBatchCode) :
            //    new ObjectParameter("PartBatchCode", typeof(string));

            //var partSerialNoParameter = partSerialNo != null ?
            //    new ObjectParameter("PartSerialNo", partSerialNo) :
            //    new ObjectParameter("PartSerialNo", typeof(string));

            //var codeParameter = code != null ?
            //    new ObjectParameter("Code", code) :
            //    new ObjectParameter("Code", typeof(string));

            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            DBHelper.AddSqlParameter("@PartConfig_Id", PartConfigId, ref sqlParameters);
            DBHelper.AddSqlParameter("@PartActualQty", partqty, ref sqlParameters);
            DBHelper.AddSqlParameter("@MachineId", MachineId, ref sqlParameters);
            DBHelper.AddSqlParameter("@created_By", UserId, ref sqlParameters);
            DBHelper.AddSqlParameter("@BoschPartNo", PartNo, ref sqlParameters);

            DBHelper.AddSqlParameter("@BoschPartNo", partbatchcode, ref sqlParameters);
            DBHelper.AddSqlParameter("@PartSerialNo", partserialno, ref sqlParameters);
            DBHelper.AddSqlParameter("@Code", Code, ref sqlParameters);

            DataTable dt = DBHelper.ExecuteProcedure("spInsertBoxLable_Verify", sqlParameters);

            return dt;

        }

        public static DataTable SaveBoxDetailsNewDMC(int PartId, int Qty, int MachineId, int UserId) {
            //{
            //    new ObjectParameter("PartConfig_Id", partConfig_Id) :
            //        new ObjectParameter("PartConfig_Id", typeof(long));

            //    var partActualQtyParameter = partActualQty.HasValue ?
            //        new ObjectParameter("PartActualQty", partActualQty) :
            //        new ObjectParameter("PartActualQty", typeof(long));

            //    var machineIdParameter = machineId.HasValue ?
            //        new ObjectParameter("MachineId", machineId) :
            //        new ObjectParameter("MachineId", typeof(long));

            //    var created_ByParameter = created_By.HasValue ?
            //        new ObjectParameter("created_By", created_By) :
            //        new ObjectParameter("created_By", typeof(long));

            //    return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spInsertBoxSerialData_Result>("spInsertBoxSerialData

            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            DBHelper.AddSqlParameter("@PartConfig_Id", PartId, ref sqlParameters);
            DBHelper.AddSqlParameter("@PartActualQty", Qty, ref sqlParameters);
            DBHelper.AddSqlParameter("@MachineId", MachineId, ref sqlParameters);
            DBHelper.AddSqlParameter("@created_By", UserId, ref sqlParameters);
           
            DataTable dt = DBHelper.ExecuteProcedure("spInsertBoxSerialData", sqlParameters);

            return dt;
        }


        public static DataTable SaveBoxDetails(int PartId, string BoschPartNo, int tRN_BoxSerialNo_ID, string partBatchCode, string partSerialNo,int actualQty, int machineId, int created_By)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            DBHelper.AddSqlParameter("@PartConfig_Id", PartId, ref sqlParameters);
            DBHelper.AddSqlParameter("@BoschPartNo", BoschPartNo, ref sqlParameters);
            DBHelper.AddSqlParameter("@TRN_BoxSerialNo_ID", tRN_BoxSerialNo_ID, ref sqlParameters);
            DBHelper.AddSqlParameter("@partBatchCode", partBatchCode, ref sqlParameters);
            DBHelper.AddSqlParameter("@PartSerialNo", partSerialNo, ref sqlParameters);

            DBHelper.AddSqlParameter("@ActualQty", actualQty, ref sqlParameters);
            DBHelper.AddSqlParameter("@MachineId", machineId, ref sqlParameters);
            DBHelper.AddSqlParameter("@created_By", created_By, ref sqlParameters);

            DataTable dt = DBHelper.ExecuteProcedure("spInsertBoxDetails", sqlParameters);

            return dt;

        }

        public static DataTable SaveErrorLogs( string LoginId, int PartId, string Approved_By, string ErrorDescription)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            DBHelper.AddSqlParameter("@LoginUserId", LoginId, ref sqlParameters);
            DBHelper.AddSqlParameter("@PartConfigNo", PartId, ref sqlParameters);
            DBHelper.AddSqlParameter("@Approved_By", Approved_By, ref sqlParameters);
            DBHelper.AddSqlParameter("@ErrorDescription", ErrorDescription, ref sqlParameters);
            

            DataTable dt = DBHelper.ExecuteProcedure("spInsertUserErrorLog", sqlParameters);

            return dt;

        }

        public static DataTable GetBoschPartNoFromCustPartNo(string ToyotaPartInDB, string Kanban)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            DBHelper.AddSqlParameter("@custPartNo", ToyotaPartInDB, ref sqlParameters);
            DBHelper.AddSqlParameter("@kanbanId", Kanban, ref sqlParameters);
            DataTable dt = DBHelper.ExecuteProcedure("spGetBoschPartNoFromCustPartNo", sqlParameters);

            return dt;

        }

        public static DataTable CreateBox(int UserId)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            DBHelper.AddSqlParameter("@UserId", UserId, ref sqlParameters);
            
            DataTable dt = DBHelper.ExecuteProcedure("usp_CreateBoxByUser", sqlParameters);

            return dt;
        }

        public static DataTable ValidateBoxSerialNo(string Boxbatchcode, string BoxSerialNo, int PartId)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            DBHelper.AddSqlParameter("@BoxBatchNo", Boxbatchcode, ref sqlParameters);
            DBHelper.AddSqlParameter("@BoxSerialNo", BoxSerialNo, ref sqlParameters);
            DBHelper.AddSqlParameter("@PartConfigID", PartId, ref sqlParameters);
            DataTable dt = DBHelper.ExecuteProcedure("spCheckBoxScanedSrNo", sqlParameters);
            return dt;

        }
    }
}
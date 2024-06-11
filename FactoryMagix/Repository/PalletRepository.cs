using FactoryMagix.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace FactoryMagix.Repository
{
    public class PalletRepository
    {
        public static DataTable GetPalletReprintDetails(string serialNo, int flag)
        {

            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            DBHelper.AddSqlParameter("@SerialNo", serialNo, ref sqlParameters);
            DBHelper.AddSqlParameter("@FlagforPalletorBox", flag, ref sqlParameters);


            var dtUsers = DBHelper.ExecuteProcedure("spGetPalletBarcodeDataprint", sqlParameters);


            return dtUsers;

        }

        public static void CreatePRNFile(string DestinationFolderPath, DataTable dt, int labeltype, string UserName)
        {
            if (!Directory.Exists(DestinationFolderPath))
            {
                Directory.CreateDirectory(DestinationFolderPath);
            }


            if ((int)LabelType.Pallet == labeltype && dt.Rows.Count > 0)
            {

                string CustomerName1;
                string CustomerName2;

                string CustomerName = dt.Rows[0]["Customer_Name"].ToString(),

                    PartNo = Convert.ToString(dt.Rows[0]["BoschPart_No"]),
                    PalletSerialNo = Convert.ToString(dt.Rows[0]["PalletSrNo"]),
                    PartDesc = Convert.ToString(dt.Rows[0]["BoschPart_Desc"]),
                    Createddate = Convert.ToString(dt.Rows[0]["createdon"]),
                    CustAddress = Convert.ToString(dt.Rows[0]["custaddress"]),
                    CustPartNo = Convert.ToString(dt.Rows[0]["CustPart_No"]),
                    InvoiceNo = Convert.ToString(dt.Rows[0]["InvoiceNo"]),
                    InvoiceQty = Convert.ToString(dt.Rows[0]["InvoiceQty"]),
                    InvoiceDate = Convert.ToString(dt.Rows[0]["InvoiceDate"]),
                    pbarcode = Convert.ToString(dt.Rows[0]["PalletBarcode"]),
                    PartQty= Convert.ToString(dt.Rows[0]["NoOfPartQy_Box"]),
                    UserLoginId = UserName.ToUpper(),
                    Shift = GetShift();


                try
                {
                    long result = 0;

                    string prnPath = @ConfigurationManager.AppSettings["PalletLabelPrn"].ToString();
                    if (File.Exists(prnPath))
                    {
                        // string strVehicleNo = string.Empty;
                        StringBuilder newFile = new StringBuilder();
                        StringBuilder strbuild = new StringBuilder();
                        TextReader tr = new StreamReader(prnPath);

                        string strleftstring = string.Empty;
                        long stringlen = CustomerName.Length;
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
                            string strtxt = ReplaceAllValues("Name", CustomerName, tr.ReadLine());
                            strtxt = ReplaceAllValues("Address Line 1", CustAddress, strtxt);
                            strtxt = ReplaceAllValues("PalletNo", PalletSerialNo, strtxt);
                            strtxt = ReplaceAllValues("PalletBarcode", pbarcode, strtxt);
                            strtxt = ReplaceAllValues("Datetime", Createddate, strtxt);
                            strtxt = ReplaceAllValues("CustomerPartNo", CustPartNo, strtxt);
                            strtxt = ReplaceAllValues("BoschPartNo", PartNo, strtxt);
                            strtxt = ReplaceAllValues("PartDesc", PartDesc, strtxt);
                            strtxt = ReplaceAllValues("QtyB", PartQty, strtxt);
                            strtxt = ReplaceAllValues("InvoiceNo", string.Concat(InvoiceNo, " ", InvoiceDate), strtxt);
                            strtxt = ReplaceAllValues("InvoiceQty", InvoiceQty, strtxt);
                            strtxt = ReplaceAllValues("PackerNM", UserLoginId, strtxt);
                            strtxt = ReplaceAllValues("sft", Shift, strtxt);
                            strbuild.Append(strtxt);
                            strbuild.Append(Environment.NewLine);
                        }
                        while (tr.Peek() != -1);
                        tr.Close();
                        //string newfile = //@ConfigurationManager.AppSettings["TempBoxLabelPrn"].ToString();
                        string newfile = Path.Combine(DestinationFolderPath, "PalletLabel.prn");
                        if(File.Exists(newfile))
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
                        //    // File.Delete(newfile);
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

        public static DataTable SaveInTemp(string Invoice_No, int PartConfig_Id, int InvoiceQty, string InvoiceDate, int MachineId,int UserId, string BoxBatchCode, string BoxSerialNo, string Code)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            DBHelper.AddSqlParameter("@Invoice_No", Invoice_No, ref sqlParameters);
            DBHelper.AddSqlParameter("@PartConfig_Id", PartConfig_Id, ref sqlParameters);
            DBHelper.AddSqlParameter("@InvoiceQty", InvoiceQty, ref sqlParameters);
            DBHelper.AddSqlParameter("@InvoiceDate", InvoiceQty, ref sqlParameters);
            DBHelper.AddSqlParameter("@Machine_ID", MachineId, ref sqlParameters);
            DBHelper.AddSqlParameter("@Created_By", UserId, ref sqlParameters);
            DBHelper.AddSqlParameter("@BoxBatchCode", BoxBatchCode, ref sqlParameters);
            DBHelper.AddSqlParameter("@BoxSerialNo", BoxSerialNo, ref sqlParameters);
            DBHelper.AddSqlParameter("@Code", Code, ref sqlParameters);

            var dtUsers = DBHelper.ExecuteProcedure("spInsertPalletLable_Verify", sqlParameters);

            return dtUsers;
        }

        public static DataTable Save(string Invoice_No, int PartConfig_Id, int InvoiceQty, string InvoiceDate, int MachineId, int UserId)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            DBHelper.AddSqlParameter("@Invoice_No", Invoice_No, ref sqlParameters);
            DBHelper.AddSqlParameter("@PartConfig_Id", PartConfig_Id, ref sqlParameters);
            DBHelper.AddSqlParameter("@InvoiceQty", InvoiceQty, ref sqlParameters);
            DBHelper.AddSqlParameter("@InvoiceDate", InvoiceQty, ref sqlParameters);
            DBHelper.AddSqlParameter("@Machine_ID", MachineId, ref sqlParameters);
            DBHelper.AddSqlParameter("@Created_By", UserId, ref sqlParameters);
           

            var dtUsers = DBHelper.ExecuteProcedure("spInsertPalletSerialData", sqlParameters);

            return dtUsers;
        }

        public static DataTable InsertPalletDetails(int TRN_PalletSerialNo_ID, string BoxBatchCode, string serialno, int MachineId, int UserId)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            DBHelper.AddSqlParameter("@TRN_PalletSerialNo_ID", TRN_PalletSerialNo_ID, ref sqlParameters);
            DBHelper.AddSqlParameter("@BoxBatchCode", BoxBatchCode, ref sqlParameters);
            DBHelper.AddSqlParameter("@BoxSerialNo", serialno, ref sqlParameters);
            DBHelper.AddSqlParameter("@MachineId", MachineId, ref sqlParameters);
            DBHelper.AddSqlParameter("@created_By", UserId, ref sqlParameters);


            DataTable dtUsers = DBHelper.ExecuteProcedure("spInsertPalletDetails", sqlParameters);

            return dtUsers;
            
        }

        public static DataTable CreatePallet(int UserId)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            DBHelper.AddSqlParameter("@UserId", UserId, ref sqlParameters);

            DataTable dt = DBHelper.ExecuteProcedure("usp_CreatePalletByUser", sqlParameters);

            return dt;
        }

    }
}
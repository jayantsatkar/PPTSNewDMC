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
           
            List <SqlParameter> sqlParameters = new List<SqlParameter>();
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

            
            if( (int)LabelType.Box == labeltype && dt.Rows.Count>0)
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
            if(DateTime.Now.Hour >= 7 && DateTime.Now.Hour < 15)
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
    }
}
using System;
using System.Linq;
using System.Web.Mvc;
using FactoryMagix.Models;
using PRNPrintFile;
using System.IO;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using Newtonsoft.Json;
using FactoryMagix;
using FactoryMagix.Repository;
using System.Collections.Generic;
using NLog;

namespace FactoryMagix.Controllers
{
    public class BoxLabelController : Controller
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        //BOSCH_PPTSEntities context = new BOSCH_PPTSEntities();
        // GET: BoxLabel
        public ActionResult BoxLabel()
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public JsonResult GetPartNos()
        {
            var query = PartRepository.GetPartsWithKeyValue(); //context.spGetAllPartNowithIndex().ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetuserData()
        {
            User objUserSession = (User)Session["UserInfo"];
            return Json(objUserSession.User_ID + ";" + objUserSession.Login_ID);
        }

        [HttpPost]
        public ActionResult Action(string code)
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                List<PartConfiguration> query = new List<PartConfiguration>();
                PartConfiguration partConfiguration = PartRepository.GetPartDetails(Convert.ToInt32(code));// context.spGetPartDetails(Convert.ToInt64(code)).ToList();
                query.Add(partConfiguration); // Code to be optimised. as single part is returned in list

                var CustCode = System.Configuration.ConfigurationManager.AppSettings["Customer_Code"].ToString();

                return Json(new { query, Customer = CustCode }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult ValidatePartNo(string PartNo, string batchcode, string partSerialNo, long partconfigid, string PartBarcode)
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                int result = 0;
                //var query = context.spCheckPartNoScaned(PartNo, batchcode, partSerialNo, partconfigid).ToList();
                //var result = query[0].Value;
                DataTable dt = PartRepository.ValidatePartNumber(PartNo, batchcode, partSerialNo, Convert.ToInt32(partconfigid));
                if(dt != null && dt.Rows.Count > 0)
                {
                    result = Convert.ToInt32(dt.Rows[0][0]);
                }
                return Json(result);
            }

        }

        [HttpPost]
        public ActionResult ValidatePartNoNewDMC(string ScannedBarcode, string PartNo)
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                //DataTable dt = new DataTable();
                //SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["SQLConnection"].ToString());
                //SqlCommand cmd = new SqlCommand("usp_CheckPartNoScaned", conn);
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@SCANNEDBARCODE", ScannedBarcode);
                //cmd.Parameters.AddWithValue("@PartNo", PartNo);
                //conn.Open();

                //SqlDataAdapter sda = new SqlDataAdapter(cmd);
                //sda.Fill(dt);
                //conn.Close();
                //PartResult result = new PartResult();

                //if (dt!=null && dt.Rows.Count>0)
                //{

                //    result.Result = Convert.ToInt32(dt.Rows[0]["Result"]);
                //    result.PartNo = Convert.ToString(dt.Rows[0]["PartNo"]);
                //    result.PartBatchCode= Convert.ToString(dt.Rows[0]["PartBatchCode"]);
                //    result.PartSerialNo = Convert.ToString(dt.Rows[0]["PartSerialNo"]);
                //}
                PartResult result = PartRepository.ValidatePartNumberNewDMC(ScannedBarcode, PartNo);

                return Json(result);
            }

        }


        [HttpPost]
        public ActionResult UniqueNum()
        {
            long i = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray())
            {
                i *= ((int)b + 1);
            }
            string number = String.Format("{0:d9}", (DateTime.Now.Ticks / 10) % 1000000000);
            return Json(number);
        }

        [HttpPost]
        public ActionResult SaveInTemp(long PartConfigId, long partqty, string partbatchcode, string partserialno, string partno, string Code)
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                int result = 0;
                User objuser = (User)Session["UserInfo"];
                //var query = context.spInsertBoxLable_Verify(PartConfigId, partqty, 1, objuser.User_ID, partno, partbatchcode, partserialno, Code).ToList();
                var query = PartRepository.SaveInTemp(Convert.ToInt32(PartConfigId), Convert.ToInt32(partqty), 1, Convert.ToInt32(objuser.User_ID), partno, partbatchcode, partserialno, Code);
                if(query != null && query.Rows.Count > 0)
                {
                     result = Convert.ToInt32(query.Rows[0][0]);
                }
                    
                return Json(result);
            }
        }

        // BELOW  METHODS NOT IN USE SO IT IS COMMENTED
        //[HttpPost]
        public ActionResult Save(long PartConfigId, long partqty)
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                User objuser = (User)Session["UserInfo"];
                //var query = context.spInsertBoxSerialData(PartConfigId, partqty, 1, objuser.User_ID).ToList();
                var query = BoxRepository.InsertBoxSerialData(Convert.ToInt32( PartConfigId),Convert.ToInt32( partqty), 1, Convert.ToInt32( objuser.User_ID)) ;
                return Json(query);
            }
        }

        [HttpPost]
        public ActionResult SaveBarCodeDetails(long PartConfigId, long partqty, string partbatchcode, string partserialno, string partno, long boxserialno)
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                User objMST_User = new User();
                objMST_User = (User)Session["UserInfo"];
                //var query = context.spInsertBoxDetails(PartConfigId, partno, boxserialno, partbatchcode, partserialno, partqty, 1, objMST_User.User_ID);
                var query = BoxRepository.SaveBoxDetails(Convert.ToInt32(PartConfigId), partno, Convert.ToInt32(boxserialno), partbatchcode, partserialno, Convert.ToInt32(partqty), 1, Convert.ToInt32(objMST_User.User_ID));
                return Json(query);
            }
        }

        [HttpPost]
        public ActionResult PrintBarcode(string CustomerName, string PartNo, long PartQty, string BoxSerialNo, string ParTDesc, string Createddate)
        {
            try
            {
                if (Session["UserInfo"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    long result = 0;
                    string prnPath = (Server.MapPath(@"~/PrnFiles/Box Label.prn"));

                    if (System.IO.File.Exists(prnPath))
                    {
                        string strtxt;
                        string strVehicleNo = string.Empty;
                        StringBuilder newFile = new StringBuilder();
                        StringBuilder strbuild = new StringBuilder();
                        TextReader tr = new StreamReader(prnPath);

                        do
                        {
                            strtxt = tr.ReadLine();

                            strtxt = ReplaceAllValues("Customer Name", CustomerName, strtxt);
                            strtxt = ReplaceAllValues("DateTime", Createddate, strtxt);
                            strtxt = ReplaceAllValues("BoxSrNo", BoxSerialNo, strtxt);

                            strtxt = ReplaceAllValues("PartDesc", ParTDesc, strtxt);
                            strtxt = ReplaceAllValues("PartNo", PartNo, strtxt);
                            strtxt = ReplaceAllValues("BarCode", BoxSerialNo, strtxt);
                            strtxt = ReplaceAllValues("qty", PartQty.ToString(), strtxt);
                            strbuild.Append(strtxt);
                            strbuild.Append(Environment.NewLine);
                        } while (tr.Peek() != -1);
                        tr.Close();

                        string newfile;
                        newfile = (Server.MapPath(@"~/PrnFiles/Box Label1.prn"));


                        FileInfo f = new FileInfo(newfile);
                        StreamWriter w = f.CreateText();
                        w.WriteLine(strbuild);
                        w.Close();

                        if (System.IO.File.Exists(newfile))
                        {
                            result = Print(newfile);

                            System.IO.File.Delete(newfile);
                        }

                    }
                    return Json(result);
                }
            }
            catch (Exception ex)
            {
                string path = @"E:\Project\Bosch\PublishWebSite\New\PPTS17\PPTS08\ErrorLog\errorlog.txt";

                using (StreamWriter sw = System.IO.File.AppendText(path))
                {
                    sw.WriteLine("$Message:" + ex.Message + "\t" + "Error" + "\t" + "$Datetime:" + DateTime.Now);
                }
                throw;
            }
        }

        public long Print(string filename)
        {
            try
            {
                string str;
                bool sts = false;
                StringBuilder sbprn = new StringBuilder();

                str = (Server.MapPath(@"~/PrnFiles/Box Label1.prn"));
                sts = RawPrinterHelper.SendFileToPrinter("Intermec_PD43_(203_dpi)_#2", str);

                if (sts == false)
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }

        private string ReplaceValues(string NameOfTag, string DbColumnName, string StrLine)
        {
            try
            {
                if (StrLine.Contains(NameOfTag))
                {
                    if (DbColumnName == "")
                    {
                        DbColumnName = "--------";
                    }
                    string str11;
                    int i = StrLine.IndexOf(NameOfTag);
                    i = i + NameOfTag.Length;
                    str11 = StrLine.Substring(i, DbColumnName.Length);
                    StrLine = StrLine.Replace(str11, DbColumnName);
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }
            return StrLine;
        }

        private string ReplaceAllValues(string NameOfTag, string DbColumnName, string StrLine)
        {
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
            catch (Exception)
            {
                return string.Empty;
            }
            return StrLine;
        }

        [HttpPost]
        public ActionResult SaveUserLogs(Int64 PartConfigNo, string ErrorDescription)
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                User objuser = (User)Session["UserInfo"];
                //var query = context.spInsertUserErrorLog(objuser.Login_ID, PartConfigNo, "", ErrorDescription).ToList();
                var query = PartRepository.SaveBoxErrorLogs(objuser.Login_ID, Convert.ToInt32(PartConfigNo), "", ErrorDescription);
                var result = query.Rows[0][0];
                return Json(query);
            }
        }

        [HttpPost]
        public ActionResult VerifyCustomerPartNo(string ToyotaPartInDB, string Kanban)
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                var query = PartRepository.GetBoschPartNoFromCustPartNo(ToyotaPartInDB, Kanban); // context.spGetBoschPartNoFromCustPartNo(ToyotaPartInDB, Kanban);
                return Json(query, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult PrintBarcodeAndCreatePRN()
        {
            string BoxNumber = "";
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                User user = (User)Session["UserInfo"];
                DataTable dtBox = BoxRepository.CreateBox(Convert.ToInt32(user.User_ID));
                if (dtBox != null && dtBox.Rows.Count > 0)
                {

                    BoxNumber = Convert.ToString(dtBox.Rows[0]["BoxSerial_No"]);
               // dt = BoxRepository.GetBoxReprintDetails(SerialNo, Convert.ToInt32(flag)); ///db.spGetBarcodeDataprint(SerialNo, flag).ToList();
                string clientIp = IpHelper.GetClientIpAddress(Request).Replace(':', '.');
                Logger.Info("IP of Request:" + clientIp);
                string path = ConfigurationManager.AppSettings["SharedDrive"].ToString();
                string folderPath = Path.Combine(path, clientIp);
                string shareName = clientIp;
                //FolderHelper.CreateAndShareFolder(folderPath, shareName);

                
                    BoxRepository.CreatePRNFile(folderPath, dtBox, (int)LabelType.Box, user.Login_ID);
                }
                
            }
            return Json(BoxNumber);
        }
    }

    //public class PartResult
    //{
    //    public int Result { get; set; }
    //    public string PartNo { get; set; }
    //    public string PartBatchCode { get; set; }
    //    public string PartSerialNo { get; set; }

    //    public PartResult()
    //    {
    //        PartNo = string.Empty;
    //        PartBatchCode = string.Empty;
    //        PartSerialNo = string.Empty;
    //    }
    //}
}

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
namespace FactoryMagix.Controllers
{
    public class BoxLabelController : Controller
    {
        BOSCH_PPTSEntities context = new BOSCH_PPTSEntities();
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
            var query = context.spGetAllPartNowithIndex().ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetuserData()
        {
            MST_User objUserSession = (MST_User)Session["UserInfo"];
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
                var query = context.spGetPartDetails(Convert.ToInt64(code)).ToList();
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
                var query = context.spCheckPartNoScaned(PartNo, batchcode, partSerialNo, partconfigid).ToList();
                var result = query[0].Value;
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
                DataTable dt = new DataTable();
                SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["SQLConnection"].ToString());
                SqlCommand cmd = new SqlCommand("usp_CheckPartNoScaned", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SCANNEDBARCODE", ScannedBarcode);
                cmd.Parameters.AddWithValue("@PartNo", PartNo);
                conn.Open();

                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
                conn.Close();
                PartResult result = new PartResult();
                
                if (dt!=null && dt.Rows.Count>0)
                {
                    
                    result.Result = Convert.ToInt32(dt.Rows[0]["Result"]);
                    result.PartNo = Convert.ToString(dt.Rows[0]["PartNo"]);
                    result.PartBatchCode= Convert.ToString(dt.Rows[0]["PartBatchCode"]);
                    result.PartSerialNo = Convert.ToString(dt.Rows[0]["PartSerialNo"]);
                }
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
                MST_User objuser = (MST_User)Session["UserInfo"];
                var query = context.spInsertBoxLable_Verify(PartConfigId, partqty, 1, objuser.User_ID, partno, partbatchcode, partserialno, Code).ToList();
                var result = query[0].Value;
                return Json(query);
            }
        }

        [HttpPost]
        public ActionResult Save(long PartConfigId, long partqty)
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                MST_User objuser = (MST_User)Session["UserInfo"];
                var query = context.spInsertBoxSerialData(PartConfigId, partqty, 1, objuser.User_ID).ToList();

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
                MST_User objMST_User = new MST_User();
                objMST_User = (MST_User)Session["UserInfo"];
                var query = context.spInsertBoxDetails(PartConfigId, partno, boxserialno, partbatchcode, partserialno, partqty, 1, objMST_User.User_ID);

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
                MST_User objuser = (MST_User)Session["UserInfo"];
                var query = context.spInsertUserErrorLog(objuser.Login_ID, PartConfigNo, "", ErrorDescription).ToList();
                var result = query[0].Value;
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
                var query = context.spGetBoschPartNoFromCustPartNo(ToyotaPartInDB, Kanban);
                return Json(query, JsonRequestBehavior.AllowGet);
            }
        }
    }

    public class PartResult
    {
        public int Result { get; set; }
        public string PartNo { get; set; }
        public string PartBatchCode { get; set; }
        public string PartSerialNo { get; set; }

        public PartResult()
        {
            PartNo = string.Empty;
            PartBatchCode = string.Empty;
            PartSerialNo = string.Empty;
        }
    }
}

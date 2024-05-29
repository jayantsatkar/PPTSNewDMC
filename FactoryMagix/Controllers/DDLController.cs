using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FactoryMagix.Models;
using PRNPrintFile;
using System.IO;
using System.Text;
using System.Drawing.Printing;


namespace FactoryMagix.Controllers
{
    public class DDLController : Controller
    {
        BOSCH_PPTSEntities context = new BOSCH_PPTSEntities();
        // GET: BoxLabel
        public ActionResult DDL()
        {
            
                ViewBag.MST_PartConfiguration_ID = new SelectList(context.spGetAllPartNowithIndex(), "MST_PartConfiguration_ID", "PartNo");
               return View();
           
        }
        //kirti
        [HttpPost]
        public ActionResult GetuserData()
        {


            MST_User objUserSession = (MST_User)Session["UserInfo"];

            return Json(objUserSession.User_ID + ";" + objUserSession.Login_ID);


        }
        //kirti
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

                return Json(query);
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
                // System.Net.IPHostEntry obj = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName().ToString());

                var query = context.spCheckPartNoScaned(PartNo, batchcode, partSerialNo, partconfigid).ToList();

                var result = query[0].Value;
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

                // string userIpAddress = this.Request.UserHostAddress;

                MST_User objuser = (MST_User)Session["UserInfo"];
                //need to replace machineid and userid


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

                // string userIpAddress = this.Request.UserHostAddress;

                MST_User objuser = (MST_User)Session["UserInfo"];
                //need to replace machineid and userid
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
                string strLine;
                bool sts = false;
                StringBuilder sbprn = new StringBuilder();
                StreamReader sr = null;
                str = (Server.MapPath(@"~/PrnFiles/Box Label1.prn"));
                //if (System.IO.File.Exists(str))
                //{
                //    FileStream fs = new FileStream(str, FileMode.Open);
                //    sr = new StreamReader(fs);
                //    strLine = sr.ReadLine();
                //    if (strLine != null)
                //    {

                //        while (strLine != null)
                //        {
                //            sbprn.Append(sbprn);
                //            strLine = sr.ReadLine();
                //        }
                //    }
                //    if (sbprn != null || sbprn.Length > 0)
                //    {
                //        // Send a printer-specific to the printer.
                //         sts= RawPrinterHelper.SendFileToPrinter(new PrinterSettings().PrinterName, sbprn.ToString());
                //    }
                //    sr.Close();
                //    sr.Dispose();
                //    fs.Close();
                //    fs.Dispose();

                //}

                sts = RawPrinterHelper.SendFileToPrinter("Intermec_PD43_(203_dpi)_#2", str);
                // RawPrinterHelper.SendFileToPrinter("Intermec PD43 (203 dpi)", @"E:\Project\Bosch\PPTS\Code\PPTS application\FactoryMagix\FactoryMagix\PrnFiles\Box Label.prn");
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

                // string userIpAddress = this.Request.UserHostAddress;

                MST_User objuser = (MST_User)Session["UserInfo"];
                //need to replace machineid and userid


                var query = context.spInsertUserErrorLog(objuser.Login_ID, PartConfigNo, "", ErrorDescription).ToList();
                var result = query[0].Value;
                return Json(query);
            }
        }
    }
}

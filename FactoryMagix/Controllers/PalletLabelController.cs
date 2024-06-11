using System;
using System.Linq;
using System.Web.Mvc;
using FactoryMagix.Models;
using PRNPrintFile;
using System.IO;
using System.Text;
using FactoryMagix.Repository;
using System.Collections.Generic;
using System.Data;
using NLog;
using System.Configuration;

namespace FactoryMagix.Controllers
{
    public class PalletLabelController : Controller
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        // BOSCH_PPTSEntities context = new BOSCH_PPTSEntities();
        public ActionResult PalletLabel()
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

        //// Added by 92293
        [HttpPost]
        public JsonResult GetPartNos()
        {
            var query = PartRepository.GetPartsWithKeyValue();  //context.spGetAllPartNowithIndex().ToList();
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
                //var query = context.spGetPartDetails(Convert.ToInt64(code)).ToList();
                List<PartConfiguration> query = new List<PartConfiguration>();
                PartConfiguration partConfiguration = PartRepository.GetPartDetails(Convert.ToInt32(code));
                query.Add(partConfiguration);
                var CustCode = System.Configuration.ConfigurationManager.AppSettings["Customer_Code"].ToString();

                // return Json(query);
                return Json(new { query, Customer = CustCode }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult ValidateBoxSerialNo(string Boxbatchcode, string BoxSerialNo, long partconfigid)
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                var query = BoxRepository.ValidateBoxSerialNo(Boxbatchcode, BoxSerialNo, Convert.ToInt32(partconfigid)); // context.spCheckBoxScanedSrNo(Boxbatchcode, BoxSerialNo, partconfigid).ToList();

                var result = Convert.ToInt32(query.Rows[0][0]);
                return Json(result);
            }

        }

        /// <summary>
        ///to save Pallet serial data
        /// </summary>
        /// <param name="PartConfigId"></param>
        /// <param name="partqty"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        //public ActionResult SaveInTemp(string Invoice_No, long PartConfig_Id, long InvoiceQty, string InvoiceDate)
        public ActionResult SaveInTemp(string Invoice_No, long PartConfig_Id, long InvoiceQty, string InvoiceDate, string BoxBatchCode, string BoxSerialNo, string Code)
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                // string userIpAddress = this.Request.UserHostAddress;
                User user = (User)Session["UserInfo"];
                //need to replace machineid and userid

                //var query = context.spInsertPalletLable_Verify(Invoice_No, PartConfig_Id, InvoiceQty, InvoiceDate, 1, user.User_ID, BoxBatchCode, BoxSerialNo, Code).ToList();
                var query = PalletRepository.SaveInTemp(Invoice_No, Convert.ToInt32(PartConfig_Id), Convert.ToInt32(InvoiceQty), InvoiceDate, 1, Convert.ToInt32(user.User_ID), BoxBatchCode, BoxSerialNo, Code);
                var result = Convert.ToInt32(query.Rows[0][0]);
                return Json(result);
            }
        }

        [HttpPost]
        public ActionResult Save(long PartConfigId, string Invoice_No, long invoiceQty, string InvoiceDate)
        {
            //need to replace machineid and userid
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                User user = new User();
                user = (User)Session["UserInfo"];
                var query = PalletRepository.Save(Invoice_No, Convert.ToInt32(PartConfigId),Convert.ToInt32(invoiceQty), InvoiceDate, 1, Convert.ToInt32(user.User_ID)); // context.spInsertPalletSerialData(Invoice_No, PartConfigId, invoiceQty, InvoiceDate, 1, user.User_ID).ToList();

                return Json(query);
            }
        }

        [HttpPost]
        public ActionResult SaveBarCodeDetails(long TRN_PalletSerialNo_ID, string BoxBatchCode, string serialno, string partno)
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                User user = new User();
                user = (User)Session["UserInfo"];
                var query = PalletRepository.InsertPalletDetails( Convert.ToInt32(TRN_PalletSerialNo_ID), BoxBatchCode, serialno, 1, Convert.ToInt32(user.User_ID));// context.spInsertPalletDetails(TRN_PalletSerialNo_ID, BoxBatchCode, serialno, 1, user.User_ID);

                return Json( Convert.ToInt16(query.Rows[0][0]));
            }
        }

        [HttpPost]
        public ActionResult PrintBarcode(string CustomerName, string PartNo, long PartQty, string PalletSerialNo,
                                         string ParTDesc, string Createddate, string CustAddress, string CustPartNo,
                                         string InvoiceNo, string InvoiceQty, string InvoiceDate)
        {
            long result = 0;
            string prnPath = (Server.MapPath(@"~/PrnFiles/Pallet Label.prn"));

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

                    strtxt = ReplaceAllValues("Name", CustomerName, strtxt);
                    strtxt = ReplaceAllValues("Address Line 1", CustAddress, strtxt);
                    strtxt = ReplaceAllValues("PalletNo", PalletSerialNo, strtxt);

                    strtxt = ReplaceAllValues("Datetime", Createddate, strtxt);
                    strtxt = ReplaceAllValues("CustomerPartNo", CustPartNo, strtxt);
                    strtxt = ReplaceAllValues("BoschPartNo", PartNo, strtxt);
                    strtxt = ReplaceAllValues("PartDesc", ParTDesc, strtxt);

                    strtxt = ReplaceAllValues("QtyB", PartQty.ToString(), strtxt);
                    strtxt = ReplaceAllValues("InvoiceNo", InvoiceNo + " " + InvoiceDate, strtxt);
                    strtxt = ReplaceAllValues("InvoiceQty", InvoiceQty, strtxt);
                    strbuild.Append(strtxt);
                    strbuild.Append(Environment.NewLine);
                } while (tr.Peek() != -1);
                tr.Close();

                string newfile;
                newfile = (Server.MapPath(@"~/PrnFiles/Pallet Label1.prn"));

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


        [HttpPost]
        public ActionResult SaveUserLogs(Int64 PartConfigNo, string ErrorDescription, string invoiceNo, string invoiceDate, Nullable<long> invoiceQty)
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                // string userIpAddress = this.Request.UserHostAddress;
                User objuser = (User)Session["UserInfo"];
                //need to replace machineid and userid

                var query = PartRepository.SavePalletErrorLogs(objuser.Login_ID, Convert.ToInt32(PartConfigNo), "", ErrorDescription, invoiceNo, invoiceDate, Convert.ToInt32(invoiceQty));// context.spInsertUserErrorLog_2(objuser.Login_ID, PartConfigNo, "", ErrorDescription, invoiceNo, invoiceDate, invoiceQty).ToList();
                var result = Convert.ToInt32(query.Rows[0][0]);
                return Json(query);
            }
        }

        public long Print(string filename)
        {
            try
            {
                string str;
                str = (Server.MapPath(@"~/PrnFiles/Pallet Label1.prn"));
                RawPrinterHelper.SendFileToPrinter("Intermec PD43 (203 dpi)", str);
                // RawPrinterHelper.SendFileToPrinter("Intermec PD43 (203 dpi)", @"E:\Project\Bosch\PPTS\Code\PPTS application\FactoryMagix\FactoryMagix\PrnFiles\Box Label.prn");
                return 1;
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
            string PalletNumber = "";
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                User user = (User)Session["UserInfo"];
                DataTable dtBox = PalletRepository.CreatePallet(Convert.ToInt32(user.User_ID));
                if (dtBox != null && dtBox.Rows.Count > 0)
                {

                    PalletNumber = Convert.ToString(dtBox.Rows[0]["PalletSrNo"]);
                    // dt = BoxRepository.GetBoxReprintDetails(SerialNo, Convert.ToInt32(flag)); ///db.spGetBarcodeDataprint(SerialNo, flag).ToList();
                    string clientIp = IpHelper.GetClientIpAddress(Request).Replace(':', '.');
                    Logger.Info("IP of Request:" + clientIp);
                    string path = ConfigurationManager.AppSettings["SharedDrive"].ToString();
                    string folderPath = Path.Combine(path, clientIp);
                    string shareName = clientIp;
                    //FolderHelper.CreateAndShareFolder(folderPath, shareName);


                    PalletRepository.CreatePRNFile(folderPath, dtBox, (int)LabelType.Pallet, user.Login_ID);
                }

            }
            return Json(PalletNumber);
        }


    }
}
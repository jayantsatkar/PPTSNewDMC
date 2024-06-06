using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using FactoryMagix.Models;
using PagedList;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FactoryMagix.Controllers
{
    public class ReportsController : Controller
    {
        private FactoryMagix.Models.BOSCH_PPTSEntities db = new FactoryMagix.Models.BOSCH_PPTSEntities();
        const int pageSize = 6;
        public ActionResult UserErrorReport()
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

        public ActionResult _UserErrorReportLoginIdWise(string LoginID)
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {

                IList<spUserErrorLogReport_Result> userError = db.spUserErrorLogReport(LoginID).ToList();
                ViewBag.RecordsCount = userError.Count();
                int? page = null;
                var listPaged = userError.ToPagedList(page ?? 1, pageSize);

                return View("UserErrorReport", listPaged);

            }
        }

        public ActionResult _UserErrorReportPage(string LoginID, int? page)
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                IList<spUserErrorLogReport_Result> userError = db.spUserErrorLogReport(LoginID).ToList();
                ViewBag.RecordsCount = userError.Count();
                
                var listPaged = userError.ToPagedList(page ?? 1, pageSize);

                return View("UserErrorReport", listPaged);
            }
        }

        public ActionResult _UserErrorReportGotoPage(string LoginID, int? page)
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                var usererrorlidt = db.spUserErrorLogReport(LoginID).ToList();
                var listPaged = usererrorlidt.ToPagedList(page ?? 1, pageSize); 
                ViewBag.RecordsCount = usererrorlidt.Count();
                if (listPaged.PageNumber != 1 && page.HasValue && page > listPaged.PageCount)
                {
                    ViewBag.PageCount = listPaged.PageCount;
                    var currentlistPaged = usererrorlidt.ToPagedList(1, pageSize);
                    ViewData["GotoPageMessage"] = "Please enter page number between 1 to";
                    return View("UserErrorReport", currentlistPaged);
                }
                return View("UserErrorReport", listPaged);
            }
           
        }

        public ActionResult UserErrorReportExportToExcel(string LoginID)
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                // BoxSrNo = "B00000047";
                var UserErrorReportDetails = (from e in db.spUserErrorLogReport(LoginID)
                                            select new
                                            {
                                                Login_ID = e.Login_ID,
                                                Login_Name = e.Login_Name,
                                                Error_Description = e.Error_Description,
                                                Part_Config__No = e.Part_Config__No,
                                                CustPart_No = e.CustPart_No,
                                                Approved_By = e.Approved_By,
                                                Approved_By_Name = e.Approved_By_Name,
                                                Created_On = e.Created_On,
                                                
                                            }).ToList();

                GridView gv = new GridView();
                gv.DataSource = UserErrorReportDetails.ToList();
                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment; filename=UserErrorReportBox.xls");
                //Response.ContentType = "application/ms-excel";
                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                gv.RenderControl(htw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();

                return RedirectToAction("UserErrorReport", "Reports");
            }
        }



        public ActionResult UserErrorReport_2()
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

        public ActionResult _UserErrorReportLoginIdWise_2(string LoginID)
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {

                IList<spUserErrorLogReport_2_Result> userError = db.spUserErrorLogReport_2(LoginID).ToList();
                ViewBag.RecordsCount = userError.Count();
                int? page = null;
                var listPaged = userError.ToPagedList(page ?? 1, pageSize);

                return View("UserErrorReport_2", listPaged);

            }
        }

        public ActionResult _UserErrorReportPage_2(string LoginID, int? page)
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                IList<spUserErrorLogReport_2_Result> userError = db.spUserErrorLogReport_2(LoginID).ToList();
                ViewBag.RecordsCount = userError.Count();

                var listPaged = userError.ToPagedList(page ?? 1, pageSize);

                return View("UserErrorReport_2", listPaged);
            }
        }

        public ActionResult _UserErrorReportGotoPage_2(string LoginID, int? page)
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                var usererrorlidt = db.spUserErrorLogReport_2(LoginID).ToList();
                var listPaged = usererrorlidt.ToPagedList(page ?? 1, pageSize);
                ViewBag.RecordsCount = usererrorlidt.Count();
                if (listPaged.PageNumber != 1 && page.HasValue && page > listPaged.PageCount)
                {
                    ViewBag.PageCount = listPaged.PageCount;
                    var currentlistPaged = usererrorlidt.ToPagedList(1, pageSize);
                    ViewData["GotoPageMessage"] = "Please enter page number between 1 to";
                    return View("UserErrorReport_2", currentlistPaged);
                }
                return View("UserErrorReport_2", listPaged);
            }

        }

        public ActionResult UserErrorReportExportToExcel_2(string LoginID)
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                // BoxSrNo = "B00000047";
                var UserErrorReportDetails = (from e in db.spUserErrorLogReport_2(LoginID)
                                              select new
                                              {
                                                  Login_ID = e.Login_ID,
                                                  Login_Name = e.Login_Name,
                                                  Error_Description = e.Error_Description,
                                                  Part_Config__No = e.Part_Config__No,
                                                  CustPart_No = e.CustPart_No,
                                                  Approved_By = e.Approved_By,
                                                  Approved_By_Name = e.Approved_By_Name,
                                                  Created_On = e.Created_On,

                                              }).ToList();

                GridView gv = new GridView();
                gv.DataSource = UserErrorReportDetails.ToList();
                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment; filename=UserErrorReportBox.xls");
                //Response.ContentType = "application/ms-excel";
                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                gv.RenderControl(htw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();

                return RedirectToAction("UserErrorReport_2", "Reports");
            }
        }


        public ActionResult PalletWiseReport()
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

        public ActionResult _PalletReport(string PalletNo)
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                IList<spGetPalletWiseReport_Result> invoicelist = db.spGetPalletWiseReport(PalletNo, DateTime.Now, DateTime.Now, false).ToList();
                ViewBag.RecordsCount = invoicelist.Count();
                int? page = null;
                var listPaged = invoicelist.ToPagedList(page ?? 1, pageSize);

                return View("PalletWiseReport", listPaged);
            }
        }

        public ActionResult _PalletReportPage(int? page, string PalletNo)
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                IList<spGetPalletWiseReport_Result> invoicelist = db.spGetPalletWiseReport(PalletNo, DateTime.Now, DateTime.Now, false).ToList();
                ViewBag.RecordsCount = invoicelist.Count();
                //int? page = null;
                var listPaged = invoicelist.ToPagedList(page ?? 1, pageSize);

                return View("PalletWiseReport", listPaged);
            }
        }

        public ActionResult PalletWiseReportGotoPage(string PalletNo, int? page)
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {

                var invoicelist = db.spGetPalletWiseReport(PalletNo, DateTime.Now, DateTime.Now, false).ToList();


                var listPaged = invoicelist.ToPagedList(page ?? 1, pageSize);

                ViewBag.RecordsCount = invoicelist.Count();
                if (listPaged.PageNumber != 1 && page.HasValue && page > listPaged.PageCount)
                {
                    ViewBag.PageCount = listPaged.PageCount;
                    var currentlistPaged = invoicelist.ToPagedList(1, pageSize);
                    ViewData["GotoPageMessage"] = "Please enter page number between 1 to";
                    return View("PalletWiseReport", currentlistPaged);
                }

                return View("PalletWiseReport", listPaged);

            }
        }


        public ActionResult BoxSerialWise()
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

        public ActionResult _BoxSerialDateWise(string BoxSrNo)
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                IList<spGetBoxSerialNoReport_Result> invoicelist = db.spGetBoxSerialNoReport(BoxSrNo, DateTime.Now, DateTime.Now, false).ToList();
                ViewBag.RecordsCount = invoicelist.Count();
                int? page = null;
                var listPaged = invoicelist.ToPagedList(page ?? 1, pageSize);

                return View("BoxSerialWise", listPaged);
            }
        }

        public ActionResult _BoxSerialDateWisePage(int? page, string BoxSrNo)
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                IList<spGetBoxSerialNoReport_Result> invoicelist = db.spGetBoxSerialNoReport(BoxSrNo, DateTime.Now, DateTime.Now, false).ToList();
                ViewBag.RecordsCount = invoicelist.Count();
                //int? page = null;
                var listPaged = invoicelist.ToPagedList(page ?? 1, pageSize);

                return View("BoxSerialWise", listPaged);
            }
        }

        public ActionResult BoxSerialWiseGotoPage(string BoxSrNo, int? page)
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                var invoicelist = db.spGetBoxSerialNoReport(BoxSrNo, DateTime.Now, DateTime.Now, false).ToList();


                var listPaged = invoicelist.ToPagedList(page ?? 1, pageSize);

                ViewBag.RecordsCount = invoicelist.Count();
                if (listPaged.PageNumber != 1 && page.HasValue && page > listPaged.PageCount)
                {
                    ViewBag.PageCount = listPaged.PageCount;
                    var currentlistPaged = invoicelist.ToPagedList(1, pageSize);
                    ViewData["GotoPageMessage"] = "Please enter page number between 1 to";
                    return View("BoxSerialWise", currentlistPaged);
                }

                return View("BoxSerialWise", listPaged);

            }
        }



        public ActionResult InvoiceReport()
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

        public ActionResult _InvoiceReport(string InvoiceNo)
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
               
                if (InvoiceNo != String.Empty || InvoiceNo != "")
                { 
                    IList<spGetInvoiceReport_Result> invoicelist = db.spGetInvoiceReport(InvoiceNo, DateTime.Now, DateTime.Now, false).ToList();
                    ViewBag.RecordsCount = invoicelist.Count();
                    int? page = null;
                    var listPaged = invoicelist.ToPagedList(page ?? 1, pageSize);

                    return View("InvoiceReport", listPaged);
                }
                else
                {
                    ModelState.AddModelError("ErrorEmail", "Error Message");
                    return View("_InvoiceDatewiseReport");
                    
                    
                }
                
            }
        }

        public ActionResult _InvoiceReportPage(int? page, string InvoiceNo)
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                IList<spGetInvoiceReport_Result> invoicelist = db.spGetInvoiceReport(InvoiceNo, DateTime.Now, DateTime.Now, false).ToList();
                ViewBag.RecordsCount = invoicelist.Count();
                //int? page = null;
                var listPaged = invoicelist.ToPagedList(page ?? 1, pageSize);

                return View("InvoiceReport", listPaged);
            }
        }

        public ActionResult InvoiceReportGotoPage(string InvoiceNo, int? page)
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                var invoicelist = db.spGetInvoiceReport(InvoiceNo, DateTime.Now, DateTime.Now, false).ToList();


                var listPaged = invoicelist.ToPagedList(page ?? 1, pageSize);

                ViewBag.RecordsCount = invoicelist.Count();
                if (listPaged.PageNumber != 1 && page.HasValue && page > listPaged.PageCount)
                {
                    ViewBag.PageCount = listPaged.PageCount;
                    var currentlistPaged = invoicelist.ToPagedList(1, pageSize);
                    ViewData["GotoPageMessage"] = "Please enter page number between 1 to";
                    return View("InvoiceReport", currentlistPaged);
                }

                return View("InvoiceReport", listPaged);

            }
        }


        public ActionResult DateWiseReport()
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                
                ViewBag.Boxlist = null;
                ViewBag.Invoice = null;
                ViewBag.Pallet = null;
                return View();
            }
        }
        
        public ActionResult PartSerialNoWise()
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

        
        public ActionResult _PartSerialNoWise(string PartSrNo,string Type)
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                bool type=false;
                if(Type=="1")
                {
                    type = true;
                }
                else if(Type=="0")
                {
                    type = false;
                }
                else
                {
                    return View("PartSerialNoWise");
                }
                IList<spGetPartSerialNoReport_Result> list = db.spGetPartSerialNoReport(PartSrNo, type).ToList();
                ViewBag.RecordsCount = list.Count();
                int? page = null;
                var listPaged = list.ToPagedList(page ?? 1, pageSize);

                return View("PartSerialNoWise",listPaged);
            }
        }

        public ActionResult PartSerialNoWisepage(int? page,string PartSrNo, string Type)
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                bool type = false;
                if (Type == "1")
                {
                    type = true;
                }
                else if (Type == "0")
                {
                    type = false;
                }
                else
                {
                    return View("PartSerialNoWise");
                }

                IList<spGetPartSerialNoReport_Result> list = db.spGetPartSerialNoReport(PartSrNo, type).ToList();
                ViewBag.RecordsCount = list.Count();
                
                var listPaged = list.ToPagedList(page ?? 1, pageSize);

                if (listPaged.PageNumber != 1 && page.HasValue && page > listPaged.PageCount)
                {
                    ViewBag.PageCount = listPaged.PageCount;
                    var currentlistPaged = list.ToPagedList(1, pageSize);
                    ViewData["GotoPageMessage"] = "Please enter page number between 1 to";
                    return View("PartSerialNoWise", currentlistPaged);
                }


                return View("PartSerialNoWise",listPaged);
            }
        }

        public ActionResult TypeDatewiseReport(string datedata)
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                string[] date = datedata.Split(';');
                DateTime fromdate = Convert.ToDateTime(date[0] + " 00:00:00");
                DateTime todate = Convert.ToDateTime(date[1] + " 23:59:59");
                ViewBag.Boxlist = null;
                ViewBag.Invoice = null;
                ViewBag.Pallet = null;
                int? page = null;
                if (date[2].ToString() == "I")
                {
                    var invoicelist = db.spGetInvoiceReport("", fromdate, todate, true).ToList();

                    IList<spGetInvoiceReport_Result> olist = (IList<spGetInvoiceReport_Result>)invoicelist;
                    
                   
                    //int? page = null;
                    var listPaged = invoicelist.ToPagedList(page ?? 1, pageSize);
                    ViewBag.Invoice = listPaged;
                }
                else if (date[2].ToString() == "P")
                {
                    var Palletlist = db.spGetPalletWiseReport("", fromdate, todate, true).ToList();
                    
                  //  int? page = null;
                    var listPaged = Palletlist.ToPagedList(page ?? 1, pageSize);
                    ViewBag.Pallet = listPaged;
                }
                else if (date[2].ToString() == "B")
                {
                    var Boxlist = db.spGetBoxSerialNoReport("", fromdate, todate, true).ToList();
                 
                   
                    var listPaged = Boxlist.ToPagedList(page ?? 1, pageSize);
                    ViewBag.Boxlist = listPaged;
                }
                else
                {
                    ViewBag.Message = 1;
                }
                return View("DateWiseReport");

            }
        }

        public ActionResult TypeDatewiseReportpage(int? page, string datedata)
        {
            if (Session["UserInfo"] == null)
            {
                return Redirect("/Account/Login");
            }
            else
            {
                string[] date = datedata.Split(';');
                DateTime fromdate = Convert.ToDateTime(date[0] + " 00:00:00");
                DateTime todate = Convert.ToDateTime(date[1] + " 23:59:59");
                ViewBag.Boxlist = null;
                ViewBag.Invoice = null;
                ViewBag.Pallet = null;
               // int? page = null;
                if (date[2].ToString() == "I")
                {
                    var invoicelist = db.spGetInvoiceReport("", fromdate, todate, true).ToList();

                    IList<spGetInvoiceReport_Result> olist = (IList<spGetInvoiceReport_Result>)invoicelist;


                    //int? page = null;
                    var listPaged = invoicelist.ToPagedList(page ?? 1, pageSize);

                    if (listPaged.PageNumber != 1 && page.HasValue && page > listPaged.PageCount)
                    {
                        ViewBag.PageCount = listPaged.PageCount;
                        var currentlistPaged = invoicelist.ToPagedList(1, pageSize);
                        ViewData["GotoPageMessage"] = "Please enter page number between 1 to";
                        ViewBag.Invoice = currentlistPaged;
                         return View("DateWiseReport");
                    }


                    ViewBag.Invoice = listPaged;
                }
                else if (date[2].ToString() == "P")
                {
                    var Palletlist = db.spGetPalletWiseReport("", fromdate, todate, true).ToList();

                    //  int? page = null;
                    var listPaged = Palletlist.ToPagedList(page ?? 1, pageSize);
                    if (listPaged.PageNumber != 1 && page.HasValue && page > listPaged.PageCount)
                    {
                        ViewBag.PageCount = listPaged.PageCount;
                        var currentlistPaged = Palletlist.ToPagedList(1, pageSize);
                        ViewData["GotoPageMessage"] = "Please enter page number between 1 to";
                        ViewBag.Pallet = currentlistPaged;
                        return View("DateWiseReport");
                    }
                    ViewBag.Pallet = listPaged;
                }
                else if (date[2].ToString() == "B")
                {
                    var Boxlist = db.spGetBoxSerialNoReport("", fromdate, todate, true).ToList();

                    var listPaged = Boxlist.ToPagedList(page ?? 1, pageSize);
                    if (listPaged.PageNumber != 1 && page.HasValue && page > listPaged.PageCount)
                    {
                        ViewBag.PageCount = listPaged.PageCount;
                        var currentlistPaged = Boxlist.ToPagedList(1, pageSize);
                        ViewData["GotoPageMessage"] = "Please enter page number between 1 to";
                        ViewBag.Boxlist = currentlistPaged;
                        return View("DateWiseReport");
                    }
                    ViewBag.Boxlist = listPaged;
                }
                else
                {
                    ViewBag.Message = 1;
                }
                return View("DateWiseReport");

            }
        }

        public ActionResult BoxSerialWiseExportToExcel(string BoxSrNo)
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                //BoxSrNo = "B00000047";
                var BoxSerialWiseDetails = (from e in db.spGetBoxSerialNoReport(BoxSrNo, DateTime.Now, DateTime.Now, false)
                                            select new
                                            {
                                                SrNo = e.SrNo,
                                                Invoiceno=e.InvoiceNo,
                                                PalletserialNo=e.PalletSerial_No,
                                                PalletBatchCode=e.PartBatchCode,
                                                BoxSerialNo=e.BoxSerialNo,
                                                BoxBatchCode=e.BoxBatchCode,
                                                Boschpart_No = e.Boschpart_No,
                                                PartSerialNo = e.PartSerialNo,
                                                PartBatchCode = e.PartBatchCode,

                                            }).ToList();

                GridView gv = new GridView();
                gv.DataSource = BoxSerialWiseDetails.ToList();  // db.MST_Role.ToList();
                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment; filename=BoxSerialReport.xls");
                //Response.ContentType = "application/ms-excel";
                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                gv.RenderControl(htw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();

                return RedirectToAction("BoxSerialWise", "Reports");
            }
        }

        public ActionResult PalletWiseExportToExcel(string PalletNo)
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                var PalletWiseWiseDetails = (from e in db.spGetPalletWiseReport(PalletNo, DateTime.Now, DateTime.Now, false)
                                             select new
                                             {
                                                 SrNo = e.SrNo,
                                                 Invoiceno=e.InvoiceNo,
                                                 PalletSrNo=e.PalletSerial_No,
                                                 PalletBatchCode=e.PartBatchCode,
                                                 BoxSerialNo = e.BoxSerialNo,
                                                 BoxBatchCode = e.BoxBatchCode,
                                                 Boschpart_No = e.Boschpart_No,
                                                 PartSerialNo = e.PartSerialNo,
                                                 PartBatchCode = e.PartBatchCode,
                                                 Paker=e.Login_ID
                                             }).ToList();

                GridView gv = new GridView();
                gv.DataSource = PalletWiseWiseDetails.ToList();  // db.MST_Role.ToList();
                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment; filename=PalletSrNo.xls");
                //Response.ContentType = "application/ms-excel";
                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                gv.RenderControl(htw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();

                return RedirectToAction("PalletWiseReport", "Reports");
            }
        }

        public ActionResult InvoiceReportExportToExcel(string InvoiceNo)
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                // BoxSrNo = "B00000047";
                var InvoiceReportDetails = (from e in db.spGetInvoiceReport(InvoiceNo, DateTime.Now, DateTime.Now, false)
                                            select new
                                            {
                                                SrNo = e.SrNo,
                                                InvoiceNo = e.InvoiceNo,
                                                PalletSerial_No = e.PalletSerial_No,
                                                PalletBatchCode = e.PalletBatchCode,
                                                BoxSerialNo = e.BoxSerialNo,
                                                BoxBatchCode = e.BoxBatchCode,
                                                Boschpart_No = e.Boschpart_No,
                                                PartSerialNo = e.PartSerialNo,
                                                PartBatchCode = e.PartBatchCode
                                            }).ToList();

                GridView gv = new GridView();
                gv.DataSource = InvoiceReportDetails.ToList();
                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment; filename=InvoiceReport.xls");
                //Response.ContentType = "application/ms-excel";
                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                gv.RenderControl(htw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();

                return RedirectToAction("InvoiceReport", "Reports");
            }
        }

        public ActionResult PartSerialWiseExportToExcel(string PartSrNo,string Type)
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                bool type = false;
                if (Type == "1")
                {
                    type = true;
                }
                else if (Type == "0")
                {
                    type = false;
                }
                else
                {
                    return View("PartSerialNoWise");
                }
                //BoxSrNo = "B00000047";
                var PartSerialWiseDetails = (from e in db.spGetPartSerialNoReport(PartSrNo, type)
                                            select new
                                            {
                                                SrNo = e.SrNo,
                                                InvoiceNo = e.InvoiceNo,
                                                PalletSrNo = e.PalletSerial_No,
                                                PalletBatchCode = e.PalletBatchCode,
                                                BoxSrNo = e.BoxSerialNo,
                                                BoxBatchCode = e.BoxBatchCode,
                                                PartNo=e.Boschpart_No,
                                                PartSrNoNo = e.PartSerialNo,
                                                partBatchCode=e.PartBatchCode
                                            }).ToList();

                GridView gv = new GridView();
                gv.DataSource = PartSerialWiseDetails.ToList();  // db.MST_Role.ToList();
                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment; filename=PartSrNoWiseReport.xls");
                //Response.ContentType = "application/ms-excel";
                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                gv.RenderControl(htw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();

                return RedirectToAction("PartSerialNoWise", "Reports");
            }
        }

        public ActionResult DateWiseReportExportToExcel(string datedata)
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                string[] date = datedata.Split(';');
                DateTime fromdate = Convert.ToDateTime(date[0] + " 00:00:00");
                DateTime todate = Convert.ToDateTime(date[1] + " 23:59:59");
                
              
                if (date[2].ToString() == "I")
                {
                    
                      var invoicelist = (from e in db.spGetInvoiceReport("", fromdate, todate, true)
                                                select new
                                                {
                                                    SrNo = e.SrNo,
                                                    InvoiceNo = e.InvoiceNo,
                                                    PalletSerial_No = e.PalletSerial_No,
                                                    PalletBatchCode = e.PalletBatchCode,
                                                    BoxSerialNo = e.BoxSerialNo,
                                                   // BoxBatchCode = e.BoxBatchCode,
                                                    Boschpart_No = e.Boschpart_No,
                                                    PartSerialNo = e.PartSerialNo,
                                                    PartBatchCode = e.PartBatchCode
                                                }).ToList();

                    GridView gv = new GridView();
                    gv.DataSource = invoicelist.ToList();
                    gv.DataBind();
                    Response.ClearContent();
                    Response.Buffer = true;
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment; filename=DateWiseInvoiceReport.xls");
                    //Response.ContentType = "application/ms-excel";
                    Response.Charset = "";
                    StringWriter sw = new StringWriter();
                    HtmlTextWriter htw = new HtmlTextWriter(sw);
                    gv.RenderControl(htw);
                    Response.Output.Write(sw.ToString());
                    Response.Flush();
                    Response.End();

                

                }
                else if (date[2].ToString() == "P")
                {
                    var Palletlist = (from e in db.spGetPalletWiseReport("", fromdate, todate, true)
                                       select new
                                       {
                                           SrNo = e.SrNo,
                                           InvoiceNo = e.InvoiceNo,
                                           PalletSerial_No = e.PalletSerial_No,
                                           PalletBatchCode = e.PalletBatchCode,
                                           BoxSerialNo = e.BoxSerialNo,
                                           BoxBatchCode = e.BoxBatchCode,
                                           Boschpart_No = e.Boschpart_No,
                                           PartSerialNo = e.PartSerialNo,
                                           PartBatchCode = e.PartBatchCode
                                       }).ToList();

                    GridView gv = new GridView();
                    gv.DataSource = Palletlist.ToList();
                    gv.DataBind();
                    Response.ClearContent();
                    Response.Buffer = true;
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment; filename=DatewisePalletReport.xls");
                    //Response.ContentType = "application/ms-excel";
                    Response.Charset = "";
                    StringWriter sw = new StringWriter();
                    HtmlTextWriter htw = new HtmlTextWriter(sw);
                    gv.RenderControl(htw);
                    Response.Output.Write(sw.ToString());
                    Response.Flush();
                    Response.End();

                    

                }
                else if (date[2].ToString() == "B")
                {
                     var Boxlist = (from e in db.spGetBoxSerialNoReport("", fromdate, todate, true)
                                       select new
                                       {
                                           SrNo = e.SrNo,
                                           InvoiceNo = e.InvoiceNo,
                                           PalletSerial_No = e.PalletSerial_No,
                                           PalletBatchCode = e.PalletBatchCode,
                                           BoxSerialNo = e.BoxSerialNo,
                                           BoxBatchCode = e.BoxBatchCode,
                                           Boschpart_No = e.Boschpart_No,
                                           PartSerialNo = e.PartSerialNo,
                                           PartBatchCode = e.PartBatchCode
                                       }).ToList();

                    GridView gv = new GridView();
                    gv.DataSource = Boxlist.ToList();
                    gv.DataBind();
                    Response.ClearContent();
                    Response.Buffer = true;
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment; filename=DatewiseBoxReport.xls");
                    //Response.ContentType = "application/ms-excel";
                    Response.Charset = "";
                    StringWriter sw = new StringWriter();
                    HtmlTextWriter htw = new HtmlTextWriter(sw);
                    gv.RenderControl(htw);
                    Response.Output.Write(sw.ToString());
                    Response.Flush();
                    Response.End();

                    

                }

                return RedirectToAction("InvoiceReport", "Reports");

            }
        }

       
    }
}

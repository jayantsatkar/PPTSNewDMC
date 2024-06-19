using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FactoryMagix.Models;
using System.Data.Entity.Validation;
using PagedList;
using PagedList.Mvc;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using FactoryMagix.Repository;
namespace FactoryMagix.Controllers
{
   // [Authorize(Roles = "Super Administrator, Administrator")]
    public class PartConfigurationController : Controller
    {
        // private BOSCH_PPTSEntities db = new BOSCH_PPTSEntities();
        const int pageSize = 10;


        public ActionResult Index(string option, string search, int? page)
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                if (option == "BoschPart_No")
                {
                    List<PartConfiguration> dataExist = PartRepository.GetParts().Where(x => x.BoschPart_No.Contains(search)).ToList(); //db.MST_PartConfiguration.Where(x => x.BoschPart_No.Contains(search)).ToList();
                    if (dataExist.Count == 0)
                    {

                        var partConfiguration = PartRepository.GetParts();// db.MST_PartConfiguration.Include(m => m.MST_Customer).Include(m => m.MST_User).Include(m => m.MST_User1);

                        partConfiguration = partConfiguration.OrderBy(c => c.MST_PartConfiguration_ID).ToList();
                        ViewBag.RecordsCount = partConfiguration.Count();
                        var listPaged = partConfiguration.ToPagedList(page ?? 1, pageSize);
                        TempData["NoDatavalidationMessage"] = "No records found please enter valid data..!";

                        return View(listPaged);
                    }
                    return View(PartRepository.GetParts().Where(x => x.BoschPart_No.Contains(search) || search == null).ToList().ToPagedList(page ?? 1, pageSize));

                }
                else if (option == "CustPart_No")
                {
                    List<PartConfiguration> dataExist = PartRepository.GetParts().Where(x => x.CustPart_No.Contains(search)).ToList();
                    if (dataExist.Count == 0)
                    {

                        var partConfiguration = PartRepository.GetParts();//.Include(m => m.MST_Customer).Include(m => m.MST_User).Include(m => m.MST_User1);

                        partConfiguration = partConfiguration.OrderBy(c => c.MST_PartConfiguration_ID).ToList();
                        ViewBag.RecordsCount = partConfiguration.Count();
                        var listPaged = partConfiguration.ToPagedList(page ?? 1, pageSize);
                        TempData["NoDatavalidationMessage"] = "No records found please enter valid data..!";

                        return View(listPaged);
                    }

                    return View(PartRepository.GetParts().Where(x => x.CustPart_No.Contains(search) || search == null).ToList().ToPagedList(page ?? 1, pageSize));
                }

                else
                {

                    var partConfiguration = PartRepository.GetParts();// .Include(m => m.MST_Customer).Include(m => m.MST_User).Include(m => m.MST_User1);


                    partConfiguration = partConfiguration.OrderBy(c => c.MST_PartConfiguration_ID).ToList();
                    ViewBag.RecordsCount = partConfiguration.Count();
                    var listPaged = partConfiguration.ToPagedList(page ?? 1, pageSize);

                    return View(listPaged);
                }
            }
        }


        // GET: MST_PartConfiguration/Details/5
        public ActionResult Details(long? id)
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                PartConfiguration partConfiguration = PartRepository.GetPartDetails(Convert.ToInt32(32)); //db.MST_PartConfiguration.Find(id);
                if (partConfiguration == null)
                {
                    return HttpNotFound();
                }
                return View(partConfiguration);
            }
        }



        // GET: MST_PartConfiguration/Create
        public ActionResult Create()
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {

                ViewBag.MST_Customer_ID = CustomerRepository.GetCustomers().Select(c => new SelectListItem //db.MST_Customer.ToList().Select(c => new SelectListItem
                {
                    Value = c.MST_Customer_ID.ToString(),
                    Text = string.Format("{0} -{1} -{2}", c.Customer_Name, c.Customer_Index, c.Customer_Code)
                });

                ViewBag.Cust_Code = new SelectList(CustomerRepository.GetCustomers(), "MST_Customer_ID", "Customer_Code"); //new SelectList(db.MST_Customer, "MST_Customer_ID", "Customer_Code");
                ViewBag.Cust_Index = new SelectList(CustomerRepository.GetCustomers(), "MST_Customer_ID", "Customer_Index"); //new SelectList(db.MST_Customer, "MST_Customer_ID", "Customer_Index");
                PartConfiguration partConfiguration = new PartConfiguration();
                return View(partConfiguration);
            }
        }

        // POST: MST_PartConfiguration/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MST_PartConfiguration_ID,BoschPart_No,Customer_Index,MST_Customer_ID,BoschPart_Desc,CustPart_No,NoOfPartQy_Box,Packed_In,Line_Id,Line_No,Created_By,Created_On,Modified_By,Modified_On")] PartConfiguration partConfiguration)
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                try
                {
                    User user = (User)Session["UserInfo"];

                    if (ModelState.IsValid)
                    {
                        partConfiguration.Created_By = Convert.ToInt32(user.User_ID);
                        partConfiguration.Created_On = DateTime.Now;

                        IList<Customer> customers = CustomerRepository.GetCustomers();//db.MST_Customer.ToList();
                        var str = customers.Where(m => m.MST_Customer_ID == partConfiguration.MST_Customer_ID).Select(c => c.Customer_Index).ToArray();
                        partConfiguration.Customer_Index = str[0].ToString();
                        Customer objcustomer = CustomerRepository.GetCustomer(partConfiguration.MST_Customer_ID); // db.MST_Customer.Find(partConfiguration.MST_Customer_ID);
                        var result = PartRepository.ValidatePartwithCustomer(objcustomer.Customer_Name, objcustomer.Customer_Index, objcustomer.Customer_Code, partConfiguration.BoschPart_No); //db.spCheckPartconfiguration(objcustomer.Customer_Name, objcustomer.Customer_Index, objcustomer.Customer_Code, partConfiguration.BoschPart_No).ToList();
                        if (result != null && result.Rows.Count > 0 && Convert.ToInt32(result.Rows[0]["result"]) == 0 && Convert.ToInt32(result.Rows[0]["customer"]) == 1)
                        {
                            Session["CreateMessage"] = "Part Configured already";
                            return RedirectToAction("Create");
                        }
                        else if (result != null && result.Rows.Count > 0 && Convert.ToInt32(result.Rows[0]["result"]) == 0 && Convert.ToInt32(result.Rows[0]["customer"]) == 0)
                        {
                            Session["CreateMessage"] = "Customer not exists";
                            return RedirectToAction("Create");
                        }

                        //db.MST_PartConfiguration.Add(partConfiguration);

                        //db.SaveChanges();

                        PartRepository.AddUpdatePartConfiguration(partConfiguration);
                        TempData["CreateMessage"] = "Part Configured Successfully!";

                        return RedirectToAction("Index");
                    }

                    //ViewBag.MST_Customer_ID = new SelectList(db.MST_Customer, "MST_Customer_ID", "Customer_Code", partConfiguration.MST_Customer_ID);
                    ViewBag.MST_Customer_ID = new SelectList(CustomerRepository.GetCustomers(), "MST_Customer_ID", "Customer_Code", partConfiguration.MST_Customer_ID);

                    return View(partConfiguration);
                }
                catch (DbEntityValidationException ex)
                {
                    throw ex;

                    // return View();
                }
            }
        }

        // GET: MST_PartConfiguration/Edit/5
        public ActionResult Edit(long? id)
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                PartConfiguration partConfiguration = PartRepository.GetPartDetails(Convert.ToInt32(id)); // db.MST_PartConfiguration.Find(id);
                if (partConfiguration == null)
                {
                    return HttpNotFound();
                }
                ViewBag.MST_Customer_ID = CustomerRepository.GetCustomers().Select(c => new SelectListItem //db.MST_Customer.ToList().Select(c => new SelectListItem
                {
                    Value = c.MST_Customer_ID.ToString(),
                    Text = string.Format("{0} -{1} -{2}", c.Customer_Name, c.Customer_Index, c.Customer_Code)
                });

                ViewBag.Cust_Code = new SelectList(CustomerRepository.GetCustomers(), "MST_Customer_ID", "Customer_Code");
                ViewBag.Cust_Index = new SelectList(CustomerRepository.GetCustomers(), "MST_Customer_ID", "Customer_Index");

                TempData["EditMessage"] = "Part edited Successfully!";

                return View(partConfiguration);
            }
        }

        // public ActionResult Edit( MST_PartConfiguration partConfiguration)

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MST_PartConfiguration_ID,BoschPart_No,Customer_Index,MST_Customer_ID,BoschPart_Desc,CustPart_No,NoOfPartQy_Box,Packed_In,Line_Id,Line_No,Created_By,Created_On,Modified_By,Modified_On")] PartConfiguration partConfiguration)

        {

            try
            {
                if (Session["UserInfo"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                else
                {


                    if (ModelState.IsValid)
                    {
                        partConfiguration.Modified_On = DateTime.Now;
                        User objUserSession = (User)Session["UserInfo"];
                        partConfiguration.Modified_By = Convert.ToInt32(objUserSession.User_ID);

                        //db.Entry(partConfiguration).State = System.Data.Entity.EntityState.Modified;
                        IList<Customer> olist = CustomerRepository.GetCustomers(); //db.MST_Customer.ToList();
                        var str = olist.Where(m => m.MST_Customer_ID == partConfiguration.MST_Customer_ID).Select(c => c.Customer_Index).ToArray();
                        partConfiguration.Customer_Index = str[0].ToString();

                        // db.SaveChanges();
                        PartRepository.AddUpdatePartConfiguration(partConfiguration);
                        TempData["EditMessage"] = "Part edited Successfully!";
                        return RedirectToAction("Index");
                    }

                    var errors = ModelState
                        .Where(x => x.Value.Errors.Count > 0)
                        .Select(x => new { x.Key, x.Value.Errors })
                        .ToArray();


                    partConfiguration = PartRepository.GetPartDetails(partConfiguration.MST_PartConfiguration_ID); //db.MST_PartConfiguration.Find(partConfiguration.MST_PartConfiguration_ID);
                    ViewBag.MST_Customer_ID = CustomerRepository.GetCustomers().Select(c => new SelectListItem //db.MST_Customer.ToList().Select(c => new SelectListItem
                    {
                        Value = c.MST_Customer_ID.ToString(),
                        Text = string.Format("{0} -{1} -{2}", c.Customer_Name, c.Customer_Index, c.Customer_Code)
                    });

                    ViewBag.Cust_Code = new SelectList(CustomerRepository.GetCustomers(), "MST_Customer_ID", "Customer_Code");
                    ViewBag.Cust_Index = new SelectList(CustomerRepository.GetCustomers(), "MST_Customer_ID", "Customer_Index");


                    return View(partConfiguration);
                }
            }
            catch (DbEntityValidationException ex)
            {
                var error = ex.EntityValidationErrors.First().ValidationErrors.First();
                //this.ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

                return View();
            }

        }


        public JsonResult IsBoschPart_No(string BoschPart_No)
        {

            //return Json(!db.MST_PartConfiguration.Any(partCon => partCon.BoschPart_No == BoschPart_No), JsonRequestBehavior.AllowGet);
            return Json(!PartRepository.GetParts().Any(partCon => partCon.BoschPart_No == BoschPart_No), JsonRequestBehavior.AllowGet);

        }

        public JsonResult IsCustPart_No(string CustPart_No)
        {

            //return Json(!db.MST_PartConfiguration.Any(partCon => partCon.CustPart_No == CustPart_No), JsonRequestBehavior.AllowGet);
            return Json(!PartRepository.GetParts().Any(partCon => partCon.CustPart_No == CustPart_No), JsonRequestBehavior.AllowGet);

        }


        public JsonResult GetPartConfigurationData(int MST_Customer_ID)
        {

            //BOSCH_PPTSEntities objBOSCH_PPTSEntities = new BOSCH_PPTSEntities();
            // MST_Customer objMST_Customer = objBOSCH_PPTSEntities.MST_Customer.Find(MST_Customer_ID);
            Customer customer = CustomerRepository.GetCustomer(MST_Customer_ID);
            return Json((new { Customer_Code = customer.Customer_Code, Customer_Index = customer.Customer_Index }), JsonRequestBehavior.AllowGet);
        }


        public ActionResult GotoPage(int? page)
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {

                var partConfiguration = from c in PartRepository.GetParts() //db.MST_PartConfiguration
                                        select c;
                partConfiguration = partConfiguration.OrderBy(c => c.MST_PartConfiguration_ID);
                var listPaged = partConfiguration.ToPagedList(page ?? 1, pageSize);
                ViewBag.RecordsCount = partConfiguration.Count();
                if (listPaged.PageNumber != 1 && page.HasValue && page > listPaged.PageCount)
                {
                    ViewBag.PageCount = listPaged.PageCount;
                    var currentlistPaged = partConfiguration.ToPagedList(1, pageSize);
                    ViewData["GotoPageMessage"] = "Please enter page number between 1 to";
                    return View("Index", currentlistPaged);
                }

                return View("Index", listPaged);

            }

        }


        public ActionResult ExportToExcel()
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {

                var PartConfigurationDetail = (from e in PartRepository.GetParts() //db.MST_PartConfiguration
                                               select new
                                               {
                                                   BoschPart_No = e.BoschPart_No,
                                                   CustPart_No = e.CustPart_No,
                                                   BoschPart_Desc = e.BoschPart_Desc,
                                                   Customer_Name = e.Customer_Name,
                                                   Customer_Code = e.Customer_Code,
                                                   Customer_Index = e.Customer_Index,
                                                   Line_Id = e.Line_Id,
                                                   NoOfPartQy_Box = e.NoOfPartQy_Box,
                                                   Line_No = e.Line_No

                                               }).ToList();

                GridView gv = new GridView();
                gv.DataSource = PartConfigurationDetail.ToList();
                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment; filename=PartConfigurationData.xls");
                //Response.ContentType = "application/ms-excel";
                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                gv.RenderControl(htw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();

                return RedirectToAction("Index", "MST_PartConfiguration");
            }
        }


        // GET: MST_PartConfiguration/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PartConfiguration partConfiguration = PartRepository.GetPartDetails(Convert.ToInt32(id)); //db.MST_PartConfiguration.Find(id);
            if (partConfiguration == null)
            {
                return HttpNotFound();
            }
            return View(partConfiguration);
        }

        // POST: MST_PartConfiguration/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            PartConfiguration partConfiguration = PartRepository.GetPartDetails(Convert.ToInt32(id)); //db.MST_PartConfiguration.Find(id);
            //db.MST_PartConfiguration.Remove(partConfiguration);
            //db.SaveChanges();
            PartRepository.AddUpdatePartConfiguration(partConfiguration);
            return RedirectToAction("Index");
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        public ActionResult ParconfigCSV()
        {

            return View();
        }

        [HttpPost]
        public ActionResult ParconfigCSV(HttpPostedFileBase file)
        {
            System.IO.FileStream aFile = null;
            StreamReader sr = null;

            var fileName = Path.GetFileName(file.FileName);
            // store the file inside ~/App_Data/uploads folder
            var path = Path.Combine(Server.MapPath(@"~//Content//CSVFileUpload"), fileName);

            try
            {

                if (Session["UserInfo"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                else
                {

                    string filepath = file.FileName;


                    User user = (User)Session["UserInfo"];

                    Session["csvresult"] = "";
                    if (file != null && file.ContentLength > 0)
                    {
                        file.SaveAs(path);

                        string strFileExtn;
                        strFileExtn = System.IO.Path.GetExtension(path);
                        DataSet Ds = new System.Data.DataSet();
                        if (strFileExtn.ToString().ToUpper() != ".CSV")
                        {
                            Session["csvresult"] = "Please select .csv files ";
                            return RedirectToAction("ParconfigCSV");
                        }
                        else
                        {

                            string lineId;
                            string modelfamily;
                            string itemClass;
                            string strLine;
                            string[] strArray;
                            string[] StrHeader;
                            char[] charArray = new char[] { ',' };

                            aFile = new FileStream(path, FileMode.Open);

                            sr = new StreamReader(aFile);
                            strLine = sr.ReadLine();
                            StrHeader = strLine.Split(charArray);

                            strLine = sr.ReadLine();
                            List<PartConfiguration> ilistMST_PartConfiguration = new List<PartConfiguration>();
                            PartConfiguration objMST_PartConfiguration = null;
                            if (StrHeader.Length == 11)
                            {
                                if (strLine != null)
                                {
                                    while (strLine != null)
                                    {
                                        objMST_PartConfiguration = new PartConfiguration();
                                        #region while loop

                                        strArray = strLine.Split(charArray);
                                        if (strArray.Length == 11)
                                        {
                                            if (strArray[1] != string.Empty || strArray[1] != "")
                                            {
                                                string[] boschpart = strArray[1].Split('-');

                                                if (boschpart.Length > 1)
                                                {
                                                    objMST_PartConfiguration.BoschPart_No = boschpart[0];
                                                    objMST_PartConfiguration.Customer_Index = boschpart[1];
                                                }
                                                else
                                                {
                                                    objMST_PartConfiguration.BoschPart_No = strArray[1];
                                                    objMST_PartConfiguration.Customer_Index = strArray[5];
                                                }
                                            }
                                            else
                                            {
                                                Session["csvresult"] = "Row No " + strArray[0] + " Error: Bosch part no null. please fill part no";
                                                return RedirectToAction("ParconfigCSV");
                                            }

                                            if (strArray[2] != "" && strArray[3] != "" && strArray[4] != "" && strArray[7] != "")
                                            {
                                                objMST_PartConfiguration.BoschPart_Desc = strArray[2];
                                                //objMST_PartConfiguration.MST_Customer.Customer_Name = strArray[3];
                                                // objMST_PartConfiguration.MST_Customer.Customer_Code = strArray[4];
                                                objMST_PartConfiguration.CustPart_No = strArray[6];
                                                objMST_PartConfiguration.NoOfPartQy_Box = Convert.ToInt32(strArray[7]);

                                                objMST_PartConfiguration.Line_Id = strArray[9];
                                                objMST_PartConfiguration.Line_No = strArray[10];
                                                objMST_PartConfiguration.Packed_In = strArray[8];

                                                if (strArray[3] != string.Empty && strArray[4] != string.Empty)
                                                {
                                                    //var result = db.spCheckPartconfiguration(strArray[3],
                                                    //                                        objMST_PartConfiguration.Customer_Index,
                                                    //                                        strArray[4],
                                                    //                                        objMST_PartConfiguration.BoschPart_No).ToList();

                                                    var result = PartRepository.CheckPartConfiguration(strArray[3],
                                                                                           objMST_PartConfiguration.Customer_Index,
                                                                                           strArray[4],
                                                                                           objMST_PartConfiguration.BoschPart_No);

                                                    if ( Convert.ToInt32(result.Rows[0]["result"]) == 0 && Convert.ToInt32(result.Rows[0]["customer"]) == 1)
                                                    {
                                                        Session["csvresult"] = "Row No " + strArray[0] + " - Part No" + objMST_PartConfiguration.BoschPart_No + " -Part Configured already";
                                                        return RedirectToAction("ParconfigCSV");
                                                    }
                                                    else if (Convert.ToInt32(result.Rows[0]["result"]) == 0 && Convert.ToInt32(result.Rows[0]["customer"]) == 0)
                                                    {
                                                        Session["csvresult"] = "Row No " + strArray[0] + "- Part No" + objMST_PartConfiguration.BoschPart_No + " -Customer not exists";
                                                        return RedirectToAction("ParconfigCSV");
                                                    }
                                                    else if (Convert.ToInt32(result.Rows[0]["result"]) > 1 && Convert.ToInt32(result.Rows[0]["customer"]) == 1)
                                                    {
                                                        objMST_PartConfiguration.MST_Customer_ID = Convert.ToInt32(result.Rows[0]["customer"]);
                                                    }

                                                }
                                                else
                                                {
                                                    Session["csvresult"] = "Some of the data blank on row no-" + strArray[0];
                                                    return RedirectToAction("ParconfigCSV");
                                                }

                                            }
                                            else
                                            {
                                                Session["csvresult"] = "Some of the data blank on row no-" + strArray[0];
                                                return RedirectToAction("ParconfigCSV");
                                            }




                                        }
                                        else
                                        {
                                            Session["csvresult"] = "Wrong file selected. Please select correct file.";
                                            return RedirectToAction("ParconfigCSV");
                                        }

                                        ilistMST_PartConfiguration.Add(objMST_PartConfiguration);


                                        strLine = sr.ReadLine();
                                        #endregion
                                    }



                                    foreach (PartConfiguration partConfiguration in ilistMST_PartConfiguration)
                                    {
                                        //var queryresult = db.spInsertPartConfig(partConfiguration.BoschPart_No, partConfiguration.Customer_Index,
                                        //                                        partConfiguration.MST_Customer_ID, partConfiguration.BoschPart_Desc, "",
                                        //                                        partConfiguration.CustPart_No, partConfiguration.NoOfPartQy_Box,
                                        //                                        partConfiguration.Packed_In, partConfiguration.Line_Id,
                                        //                                        partConfiguration.Line_No, user.User_ID

                                        //                                    );


                                        
                                        PartRepository.ImportCSV(partConfiguration.BoschPart_No, partConfiguration.Customer_Index,
                                                                     Convert.ToInt32(partConfiguration.MST_Customer_ID), partConfiguration.BoschPart_Desc, "",
                                       partConfiguration.CustPart_No, Convert.ToInt32(partConfiguration.NoOfPartQy_Box),
                                                                                partConfiguration.Packed_In, partConfiguration.Line_Id,
                                                                                partConfiguration.Line_No, Convert.ToInt32(user.User_ID)

                                                                            );
                                    }

                                    Session["csvresult"] = "File imported successfully";

                                }
                            }
                            else
                            {

                                Session["csvresult"] = "Wrong file selected. Please select correct file.";
                                return RedirectToAction("ParconfigCSV");
                            }
                            sr.Close();
                            sr.Dispose();
                            aFile.Close();
                            aFile.Dispose();
                        }

                    }
                    else
                    {
                        Session["csvresult"] = "Please select file";
                        return RedirectToAction("ParconfigCSV");
                    }

                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                if (aFile != null)
                {
                    sr.Close();
                    sr.Dispose();
                    aFile.Close();
                    aFile.Dispose();
                }
                Session["csvresult"] = "Error while importing file  " + Session["csvresult"].ToString() + ex.Message;
                return RedirectToAction("ParconfigCSV");

            }
            finally
            {
                if (aFile != null)
                {
                    sr.Close();
                    sr.Dispose();
                    aFile.Close();
                    aFile.Dispose();
                }
                if (System.IO.File.Exists(path))
                {


                    System.IO.File.Delete(path);

                }
            }
        }
    }
}

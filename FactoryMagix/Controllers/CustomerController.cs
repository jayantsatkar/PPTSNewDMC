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
    public class CustomerController : Controller
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
                if (option == "CustomerCode")
                {
                    List<Customer> dataExist = CustomerRepository.GetCustomers().Where(x => x.Customer_Code.Contains(search)).ToList(); // db.MST_Customer.Where(x => x.Customer_Code.Contains(search)).ToList();
                    if (dataExist.Count == 0)
                    {

                        var customer = from c in  CustomerRepository.GetCustomers()//db.MST_Customer
                                       select c;
                        customer = customer.OrderBy(c => c.Customer_Name);
                        ViewBag.RecordsCount = customer.Count();
                        var listPaged = customer.ToPagedList(page ?? 1, pageSize);
                        TempData["NoDatavalidationMessage"] = "No records found please enter valid data..!";

                        return View(listPaged);
                    }

                    // return View(db.MST_Customer.Where(x => x.Customer_Code.Contains(search) || search == null).ToList().ToPagedList(page ?? 1, pageSize));
                    return View(CustomerRepository.GetCustomers().Where(x => x.Customer_Code.Contains(search) || search == null).ToList().ToPagedList(page ?? 1, pageSize));

                }
                else if (option == "CustomerName")
                {
                    List<Customer> dataExist = CustomerRepository.GetCustomers().Where(x => x.Customer_Name.Contains(search)).ToList();
                    if (dataExist.Count == 0)
                    {
                        var customer = from c in CustomerRepository.GetCustomers()//db.MST_Customer
                                       select c;
                        customer = customer.OrderBy(c => c.Customer_Name);
                        ViewBag.RecordsCount = customer.Count();
                        var listPaged = customer.ToPagedList(page ?? 1, pageSize);
                        TempData["NoDatavalidationMessage"] = "No records found please enter valid data..!";

                        return View(listPaged);
                    }
                    return View(CustomerRepository.GetCustomers().Where(x => x.Customer_Name.Contains(search) || search == null).ToList().ToPagedList(page ?? 1, pageSize));
                }
                else if(option== "CustomerIndex")
                {
                    List<Customer> dataExist = CustomerRepository.GetCustomers().Where(x => x.Customer_Index.Contains(search)).ToList();
                    if (dataExist.Count == 0)
                    {
                        var customer = from c in CustomerRepository.GetCustomers() //db.MST_Customer
                                       select c;
                        customer = customer.OrderBy(c => c.Customer_Name);
                        ViewBag.RecordsCount = customer.Count();
                        var listPaged = customer.ToPagedList(page ?? 1, pageSize);
                        TempData["NoDatavalidationMessage"] = "No records found plesae enter valid data..!";

                        return View(listPaged);
                    }

                    return View(CustomerRepository.GetCustomers().Where(x => x.Customer_Index.Contains(search) || search == null).ToList().ToPagedList(page ?? 1, pageSize));
                
                //return View(db.MST_Customer.Where(x => x.Customer_Index.Contains(search) || search == null).ToList().ToPagedList(page ?? 1, pageSize));
            }

                else
                { 

                    var customer = from c in CustomerRepository.GetCustomers()//db.MST_Customer
                                   select c;
                customer = customer.OrderBy(c => c.Customer_Name);
                ViewBag.RecordsCount = customer.Count();
                var listPaged = customer.ToPagedList(page ?? 1, pageSize);

                return View(listPaged);
                }
            }
        }

        public ActionResult SearchText(string strSearchText, string strToSearch, int? page)
        {
            var customer = from c in CustomerRepository.GetCustomers()//db.MST_Customer
                           select c;
            // customer = customer.OrderBy(c => c.Customer_Name);

           List<Customer>  SearchResultRoles = customer.AsQueryable().Where(c => c.Customer_Name.ToLower().Contains(strSearchText.ToLower())).ToList();
            ViewBag.RecordsCount = SearchResultRoles.Count();
            var listPaged = SearchResultRoles.ToPagedList(page ?? 1, pageSize);

            return View(listPaged);
            
        }


        // GET: Customer/Details/5
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
                // MST_Customer customer = db.MST_Customer.Find(id);
                Customer customer = CustomerRepository.GetCustomer(Convert.ToInt64(id));
                if (customer == null)
                {
                    return HttpNotFound();
                }
                return View(customer);
            }
        }

        // GET: Customer/Create
        public ActionResult Create()
        {
            Customer customer = new Customer();
            return View(customer);
        }

        // POST: Customer/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MST_Customer_ID,Customer_Code,Customer_Name,Customer_Index,Address_Line1,Address_Line2,PhoneNo,FaxNo,EmailId,Created_By,Created_On,Modified_By,Modified_On")] Customer customer)
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                User mST_User = (User)Session["UserInfo"];

                if (ModelState.IsValid)
                {
                    //  bool doesExistAlready = db.MST_Customer.Where(c => c.Customer_Code = customer.Customer_Code && c.Customer_Index= customer.Customer_Index && c.Customer_Name = customer.Customer_Name );
                    if (CustomerRepository.GetCustomers().Any(cust => cust.Customer_Code == customer.Customer_Code && cust.Customer_Index == customer.Customer_Index && cust.Customer_Name == customer.Customer_Name))
                    {
                        TempData["validationMessage"] = "Customer Code, Name, Index must be different..!";

                        return RedirectToAction("Create");

                    }

                    if (customer.Address_Line2 == null)
                    {
                        customer.Address_Line2 = string.Empty;
                    }

                   // if(db.MST_Customer.Any(cust => cust.Customer_Code == customer.Customer_Code))




                    customer.Created_By = mST_User.User_ID;
                    customer.Created_On = DateTime.Now;

                    CustomerRepository.AddUpdateCustomer(customer);
                    //db.MST_Customer.Add(customer);
                    TempData["CreateMessage"] = "Customer created Successfully!";

                   // db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(customer);
            }
        }

        // GET: Customer/Edit/5
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
                Customer customer = CustomerRepository.GetCustomer(Convert.ToInt64(id));
                //MST_Customer customer = db.MST_Customer.Find(id);
                if (customer == null)
                {
                    return HttpNotFound();
                }
                return View(customer);
            }
        }

        // POST: Customer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MST_Customer_ID,Customer_Code,Customer_Name,Customer_Index,Address_Line1,Address_Line2,PhoneNo,FaxNo,EmailId,Created_By,Created_On,Modified_By,Modified_On")] Customer customer)
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                ModelState.Remove("Modified_By");
                ModelState.Clear();
                if (ModelState.IsValid)
                {
                    
                    customer.Modified_On = DateTime.Now;
                    User objUserSession = (User)Session["UserInfo"];
                    customer.Modified_By = objUserSession == null ? 0 : objUserSession.User_ID;

                    CustomerRepository.AddUpdateCustomer(customer);
                   // db.Entry(customer).State = System.Data.Entity.EntityState.Modified;
                   // db.SaveChanges();
                    TempData["EditMessage"] = "Customer edited Successfully!";

                    return RedirectToAction("Index");
                }
                return View(customer);
            }
        }
        
        public ActionResult GotoPage(int? page)
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {

                var customer = from c in CustomerRepository.GetCustomers() //db.MST_Customer
                               select c;
                customer = customer.OrderBy(c => c.MST_Customer_ID);
                var listPaged = customer.ToPagedList(page ?? 1, pageSize);
                ViewBag.RecordsCount = customer.Count();
                if (listPaged.PageNumber != 1 && page.HasValue && page > listPaged.PageCount)
                {
                    ViewBag.PageCount = listPaged.PageCount;
                    var currentlistPaged = customer.ToPagedList(1, pageSize);
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

                var CustomerDetail = (from e in CustomerRepository.GetCustomers() //db.MST_Customer
                                      select new
                                      {
                                          CustomerCode = e.Customer_Code,
                                          CustomerName = e.Customer_Name,
                                          CustomerIndex = e.Customer_Index,
                                          AddressLine1 = e.Address_Line1,
                                          AddressLine2 = e.Address_Line2
                                      }).ToList();

                GridView gv = new GridView();
                gv.DataSource = CustomerDetail.ToList();  
                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment; filename=CustomerData.xls");
                //Response.ContentType = "application/ms-excel";
                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                gv.RenderControl(htw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();

                return RedirectToAction("Index", "Customer");
            }
        }

        // GET: Customer/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = CustomerRepository.GetCustomer(Convert.ToInt64(id));
            //MST_Customer customer = db.MST_Customer.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Customer customer = CustomerRepository.GetCustomer(Convert.ToInt64(id));
            // MST_Customer customer = db.MST_Customer.Find(id);
            // db.MST_Customer.Remove(customer);
           // customer.IsActive = false;
            CustomerRepository.AddUpdateCustomer(customer);
           // db.SaveChanges();
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
    }
}

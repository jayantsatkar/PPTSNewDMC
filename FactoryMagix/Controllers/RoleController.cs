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
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
using System.Drawing;
using PagedList;
using PagedList.Mvc;

namespace FactoryMagix.Controllers
{

    public class RoleController : Controller
    {

        private BOSCH_PPTSEntities db = new BOSCH_PPTSEntities();
        const int pageSize = 10;
       

        public ActionResult Index(string option, string search, int? page)
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                if (option == "Role_Name")
                {
                    List<MST_Role> dataExist = db.MST_Role.Where(x => x.Role_Name.Contains(search)).ToList();
                    if (dataExist.Count == 0)
                    {
                        var role = from c in db.MST_Role
                                   select c;
                        role = role.OrderBy(c => c.Role_ID);
                        // const int pageSize = 2;
                        ViewBag.RecordsCount = role.Count();
                        var listPaged = role.ToPagedList(page ?? 1, pageSize);
                        TempData["NoDatavalidationMessage"] = "No records found please enter valid data..!";

                        return View(listPaged);

                    }
                    return View(db.MST_Role.Where(x => x.Role_Name.Contains(search) || search == null).ToList().ToPagedList(page ?? 1, pageSize));

                }
                else if (option == "Role_Desc")
                {
                    List<MST_Role> dataExist = db.MST_Role.Where(x => x.Role_Desc.Contains(search)).ToList();
                    if (dataExist.Count == 0)
                    {
                        var role = from c in db.MST_Role
                               select c;
                    role = role.OrderBy(c => c.Role_ID);
                    // const int pageSize = 2;
                    ViewBag.RecordsCount = role.Count();
                    var listPaged = role.ToPagedList(page ?? 1, pageSize);
                    TempData["NoDatavalidationMessage"] = "No records found please enter valid data..!";

                        return View(listPaged);
                    } 
                    return View(db.MST_Role.Where(x => x.Role_Desc.Contains(search) || search == null).ToList().ToPagedList(page ?? 1, pageSize));
                }


                else
                {
                    var role = from c in db.MST_Role
                               select c;
                    role = role.OrderBy(c => c.Role_ID);
                    // const int pageSize = 2;
                    ViewBag.RecordsCount = role.Count();
                    var listPaged = role.ToPagedList(page ?? 1, pageSize);

                    return View(listPaged);
                }
            }
        }

        // GET: Role/Details/5
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
                MST_Role mST_Role = db.MST_Role.Find(id);
                if (mST_Role == null)
                {
                    return HttpNotFound();
                }
                return View(mST_Role);
            }
        }


        // GET: Role/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Role/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Role_ID,Role_Name,Role_Desc,IsActive")] MST_Role mST_Role)
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                MST_User mST_User = (MST_User)Session["UserInfo"];

                if (ModelState.IsValid)
                {
                    mST_Role.Created_On = DateTime.Now;
                    // MST_User objUserSession = (MST_User)Session["MST_User"];
                    mST_Role.Created_By = mST_User.User_ID;
                    db.MST_Role.Add(mST_Role);
                    db.SaveChanges();
                    TempData["CreateMessage"] = "Role created Successfully!";

                    return RedirectToAction("Index");
                }

                return View(mST_Role);
            }
        }

        // GET: Role/Edit/5
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
                MST_Role mST_Role = db.MST_Role.Find(id);
                if (mST_Role == null)
                {
                    return HttpNotFound();
                }
                return View(mST_Role);
            }
        }

        // POST: Role/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MST_Role mST_Role)
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                try
                {
                    MST_User mST_User = (MST_User)Session["UserInfo"];

                    if (ModelState.IsValid)
                    {
                        db.Entry(mST_Role).State = System.Data.Entity.EntityState.Modified;

                        mST_Role.Modified_By = mST_User.User_ID;
                        mST_Role.Modified_On = DateTime.Now;
                        db.SaveChanges();
                        TempData["EditMessage"] = "Role edited Successfully!";

                        return RedirectToAction("Index");
                    }
                    return View(mST_Role);
                }

                catch (DbEntityValidationException ex)
                {
                    var error = ex.EntityValidationErrors.First().ValidationErrors.First();
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

                    return View();
                }
            }

        }


        public JsonResult IsRoleAvailable(string Role_Name)
        {

            return Json(!db.MST_Role.Any(role => role.Role_Name == Role_Name), JsonRequestBehavior.AllowGet);

        }

        public ActionResult GotoPage(int? page)
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                var role = from c in db.MST_Role
                           select c;
                role = role.OrderBy(c => c.Role_ID);
                var listPaged = role.ToPagedList(page ?? 1, pageSize);
                ViewBag.RecordsCount = role.Count();

                if (listPaged.PageNumber != 1 && page.HasValue && page > listPaged.PageCount)
                {
                    ViewBag.PageCount = listPaged.PageCount;
                    var currentlistPaged = role.ToPagedList(1, pageSize);
                    ViewData["GotoPageMessage"] = "Please enter page number between 1 to";
                    return View("Index", currentlistPaged);
                }

                return View("Index", listPaged);

            }
        }


        // GET: Role/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MST_Role mST_Role = db.MST_Role.Find(id);
            if (mST_Role == null)
            {
                return HttpNotFound();
            }
            return View(mST_Role);
        }

        // POST: Role/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            MST_Role mST_Role = db.MST_Role.Find(id);
            db.MST_Role.Remove(mST_Role);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ExportToExcel()
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                var RoleDetail = (from e in db.MST_Role
                                  select new
                                  {
                                      SrNo = e.Role_ID,
                                      RoleName = e.Role_Name,
                                      RoleDesc = e.Role_Desc,
                                      Status = e.IsActive


                                  }).ToList();

                GridView gv = new GridView();
                gv.DataSource = RoleDetail.ToList();  // db.MST_Role.ToList();
                gv.DataBind();



                Response.ClearContent();
                Response.Buffer = true;
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment; filename=RoleData.xls");
                //Response.ContentType = "application/ms-excel";
                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                gv.RenderControl(htw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();

                return RedirectToAction("Index", "Role");
            }
        }




        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

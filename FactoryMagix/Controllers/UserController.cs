using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FactoryMagix.Models;

using System.Text;
using System.IO;
using System.Web.UI.WebControls;
using System.Web.UI;
using PagedList;
using PagedList.Mvc;
using System.Data.Entity.Validation;

namespace FactoryMagix.Controllers
{
    public class UserController : Controller
    {
        //private BOSCH_PPTSEntities db = new BOSCH_PPTSEntities();
        const int pageSize = 10;

        public ActionResult Index(string option, string search, int? page)
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                if (option == "Login_ID")
                {
                    List<User> dataExist = UserRepository.GetUsers().Where(x => x.Login_ID.Contains(search)).ToList();// db.MST_User.Where(x => x.Login_ID.Contains(search)).ToList();
                    if (dataExist.Count == 0)
                    {
                        var user = from c in UserRepository.GetUsers()// db.MST_User
                                   select c;
                        user = user.OrderBy(c => c.User_ID);
                        ViewBag.RecordsCount = user.Count();
                        var listPaged = user.ToPagedList(page ?? 1, pageSize);
                        TempData["NoDatavalidationMessage"] = "No records found please enter valid data..!";

                        return View(listPaged);

                    }

                    // return View(db.MST_User.Where(x => x.Login_ID.Contains(search) || search == null).ToList().ToPagedList(page ?? 1, pageSize));
                    return View(UserRepository.GetUsers().Where(x => x.Login_ID.Contains(search) || search == null).ToList().ToPagedList(page ?? 1, pageSize));
                    
                }
                else if (option == "First_Name")
                {
                    List<User> dataExist = UserRepository.GetUsers().Where(x => x.First_Name.Contains(search)).ToList();
                    // db.MST_User.Where(x => x.First_Name.Contains(search)).ToList();
                    if (dataExist.Count == 0)
                    {
                        var user = from c in UserRepository.GetUsers()//db.MST_User
                                   select c;
                        user = user.OrderBy(c => c.User_ID);
                        ViewBag.RecordsCount = user.Count();
                        var listPaged = user.ToPagedList(page ?? 1, pageSize);
                        TempData["NoDatavalidationMessage"] = "No records found please enter valid data..!";

                        return View(listPaged);

                    }
                    //return View(db.MST_User.Where(x => x.First_Name.Contains(search) || search == null).ToList().ToPagedList(page ?? 1, pageSize));
                    return View(UserRepository.GetUsers().Where(x => x.First_Name.Contains(search) || search == null).ToList().ToPagedList(page ?? 1, pageSize));
                    
                }
                //else if (option == "Role_Name")
                //{
                //    List<Role> dataExist = RoleRepository.GetRoles().Where(x => x.Role_Name.Contains(search)).ToList();

                //    //db.MST_Role.Where(x => x.Role_Name.Contains(search)).ToList();
                //    if (dataExist.Count == 0)
                //    {
                //        var user = from c in UserRepository.GetUsers()//db.MST_User
                //                   select c;
                //        user = user.OrderBy(c => c.User_ID);
                //        ViewBag.RecordsCount = user.Count();
                //        var listPaged = user.ToPagedList(page ?? 1, pageSize);
                //        TempData["NoDatavalidationMessage"] = "No records found please enter valid data..!";

                //        return View(listPaged);
                //    }
                //    //return View(db.MST_Role.Where(x => x.Role_Name.Contains(search) || search == null).ToList().ToPagedList(page ?? 1, pageSize));
                //    return View(RoleRepository.GetRoles().Where(x => x.Role_Name.Contains(search) || search == null).ToList().ToPagedList(page ?? 1, pageSize));
                    
                //}

                else
                {
                    var user = from c in UserRepository.GetUsers() //db.MST_User
                               select c;
                    user = user.OrderBy(c => c.User_ID);
                    ViewBag.RecordsCount = user.Count();
                    var listPaged = user.ToPagedList(page ?? 1, pageSize);

                    return View(listPaged);
                }
            }
        }

        // GET: User/Details/5
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
                User mST_User = UserRepository.GetUser(Convert.ToInt32(id)); //db.MST_User.Find(id);
                if (mST_User == null)
                {
                    return HttpNotFound();
                }
                return View(mST_User);
            }
        }

        // GET: User/Create
        public ActionResult Create()
        {
            ViewBag.Role_ID = new SelectList(RoleRepository.GetRoles(), "Role_ID", "Role_Name");//db.MST_Role
            User user = new User();
            //  var model = new MST_User();
            return View(user);
        }

        // POST: User/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        // public ActionResult Create([Bind(Exclude = "User_ID")] MST_User mST_User, HttpPostedFileBase User_Image)
        public ActionResult Create([Bind(Exclude = "User_ID")] User mST_User)
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                try
                {
                    string strPassword = Encryptdata(mST_User.Password);
                   //string strConfPassword = strPassword; // Encryptdata(mST_User.ConfirmPassword);
                    if (ModelState.IsValid)
                    {
                        mST_User.Created_On = DateTime.Now;
                        mST_User.Password = strPassword;
                        // mST_User.ConfirmPassword = strPassword;

                        User objUserSession = (User)Session["UserInfo"];
                        mST_User.Created_By = objUserSession == null ? 0 : objUserSession.User_ID;

                        //db.MST_User.Add(mST_User);
                        //db.SaveChanges();
                        UserRepository.AddUpdateUser(mST_User);
                        TempData["CreateMessage"] = "User created Successfully!";

                        return RedirectToAction("Index");
                    }

                    //ViewBag.Role_ID = new SelectList(db.MST_Role, "Role_ID", "Role_Name", mST_User.Role_ID);
                    ViewBag.Role_ID = new SelectList(RoleRepository.GetRoles(), "Role_ID", "Role_Name", mST_User.Role_ID);
                }
                catch (DbEntityValidationException dbEx)
                {
                    Exception raise = dbEx;
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            string message = string.Format("{0}:{1}",
                                validationErrors.Entry.Entity.ToString(),
                                validationError.ErrorMessage);
                            // raise a new exception nesting  
                            // the current instance as InnerException  
                            raise = new InvalidOperationException(message, raise);
                        }
                    }
                    throw raise;
                }
                catch (Exception ex)
                {
                    return View("Error", new HandleErrorInfo(ex, "MST_User", "Create"));
                }
                return View(mST_User);
            }
        }

        private string Encryptdata(string password)
        {
            string strmsg = string.Empty;
            byte[] encode = new byte[password.Length];
            encode = Encoding.UTF8.GetBytes(password);
            strmsg = Convert.ToBase64String(encode);
            return strmsg;
        }

        public ActionResult BioMetric(long? id)
        {
            return RedirectToAction("Index", "BioMetric", new { Id = id });
        }

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
                User mST_User = UserRepository.GetUser(Convert.ToInt32(id)); //db.MST_User.Find(id);
                if (mST_User == null)
                {
                    return HttpNotFound();
                }
                if (mST_User.Password != null || mST_User.Password != "")
                {
                    string oldpassword = mST_User.Password;
                    mST_User.Password = Decryptdata(oldpassword);
                    // mST_User.ConfirmPassword = Decryptdata(oldpassword);
                }
                else
                {
                    mST_User.Password = "";
                    // mST_User.ConfirmPassword = "";
                }

                //ViewBag.Role_ID = new SelectList(db.MST_Role, "Role_ID", "Role_Name", mST_User.Role_ID);
                ViewBag.Role_ID = new SelectList(RoleRepository.GetRoles(), "Role_ID", "Role_Name", mST_User.Role_ID);
                return View(mST_User);
            }
        }

        public string Decryptdata(string password)
        {
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            System.Text.Decoder strDecoder = encoder.GetDecoder();
            byte[] to_DecodeByte = Convert.FromBase64String(password);
            int charCount = strDecoder.GetCharCount(to_DecodeByte, 0, to_DecodeByte.Length);
            char[] decoded_char = new char[charCount];
            strDecoder.GetChars(to_DecodeByte, 0, to_DecodeByte.Length, decoded_char, 0);
            string Name = new string(decoded_char);

            return Name;
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        // public ActionResult Edit([Bind(Include = "User_ID,Role_ID,Login_ID,Password,First_Name,Last_Name,Middle_Name,EmailId,User_Image,Address,City,State,Country,Pincode,Mobile_No,EmployeeId,IsActive,Created_By,Created_On,Modified_By,Modified_On")] MST_User mST_User)
        public ActionResult Edit(User user)
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                try
                {
                    string strPassword = user.Password;

                    user.Password = Encryptdata(strPassword);
                    // mST_User.ConfirmPassword = Encryptdata(strPassword);

                    user.Modified_On = DateTime.Now;
                    User objUserSession = (User)Session["UserInfo"];
                    user.Modified_By = objUserSession == null ? 0 : objUserSession.User_ID;
                    //db.Entry(mST_User).State = System.Data.Entity.EntityState.Modified;
                    //db.SaveChanges();
                    UserRepository.AddUpdateUser(user);
                    TempData["EditMessage"] = "User edited Successfully!";

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    var error = ex.Message;
                    return View();
                }
            }
        }

        public ActionResult GotoPage(int? page)
        {
            if (Session["UserInfo"] == null)
            {
                return Redirect("/Account/Login");
            }
            else
            {
                var user = from c in UserRepository.GetUsers() //db.MST_User
                           select c;
                user = user.OrderBy(c => c.User_ID);
                var listPaged = user.ToPagedList(page ?? 1, pageSize);
                ViewBag.RecordsCount = user.Count();
                if (listPaged.PageNumber != 1 && page.HasValue && page > listPaged.PageCount)
                {
                    ViewBag.PageCount = listPaged.PageCount;
                    var currentlistPaged = user.ToPagedList(1, pageSize);
                    ViewData["GotoPageMessage"] = "Please enter page number between 1 to";
                    return View("Index", currentlistPaged);
                }
                return View("Index", listPaged);
            }
        }

        // GET: User/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User mST_User = UserRepository.GetUser(Convert.ToInt32(id)); //db.MST_User.Find(id);
            if (mST_User == null)
            {
                return HttpNotFound();
            }
            return View(mST_User);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            User user = UserRepository.GetUser(Convert.ToInt32(id)); // db.MST_User.Find(id);
            //db.MST_User.Remove(mST_User);
            //db.SaveChanges();
            user.IsActive = false;
            UserRepository.AddUpdateUser(user);
            return RedirectToAction("Index");
        }

        public JsonResult IsUserAvailable(string Login_ID)
        {
            // return Json(!db.MST_User.Any(user => user.Login_ID == Login_ID), JsonRequestBehavior.AllowGet);
            return Json(!UserRepository.GetUsers().Any(user => user.Login_ID == Login_ID), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExportToExcel()
        {
            if (Session["UserInfo"] == null)
            {
                return Redirect("/Account/Login");
            }
            else
            {
                var UserDetail = (from e in  UserRepository.GetUsers() //db.MST_User
                                  select new
                                  {
                                      SrNo = e.User_ID,
                                      LoginName = e.Login_ID,
                                      UserName = e.First_Name + e.Last_Name,
                                      //Role = e.Role_Name,
                                      Status = e.IsActive

                                  }).ToList();

                GridView gv = new GridView();
                gv.DataSource = UserDetail.ToList();
                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment; filename=UserData.xls");
                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                gv.RenderControl(htw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();

                return RedirectToAction("Index", "User");
            }
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

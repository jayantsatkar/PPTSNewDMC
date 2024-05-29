using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FactoryMagix.Models;
using FactoryMagix.ViewModel;
using System.Data.SqlClient;
using System.Data;

namespace FactoryMagix.Controllers
{
    public class AssignFormsToUserController : Controller
    {
        private BOSCH_PPTSEntities db = new BOSCH_PPTSEntities();

        // GET: AssignFormsToUser
        public ActionResult Index()
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                ViewBag.MST_Role = db.MST_Role.ToList();
                AssignFormsToUseViewModel AssignFormsToUseViewModel = new AssignFormsToUseViewModel();

                //ViewBag.MST_Forms = db.MST_Form.ToList();
                //ViewBag.REL_UserForm = db.REL_UserForm.ToList();

                return View(AssignFormsToUseViewModel);
            }
        }


        private IList<MST_User> GetUser(int roleId)
        {
            return db.MST_User.Where(m => m.Role_ID == roleId).ToList();
        }


        private IList<MST_Module> GetModule()
        {
            return db.MST_Module.ToList();
        }


        private IList<MST_Form> GetForm(int moduleid)
        {
            return db.MST_Form.Where(m => m.Module_ID == moduleid).ToList();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult LoadUserByRoletId(string roleid)
        {
            
                var userList = this.GetUser(Convert.ToInt32(roleid));
                var userData = userList.Select(m => new SelectListItem()
                {
                    Text = m.Login_ID,
                    Value = m.User_ID.ToString(),
                });
                return Json(userData, JsonRequestBehavior.AllowGet);
           
        }


        
        public JsonResult LoadModuleByUserId(string userid)
        {
            var moduleList = this.GetModule();
            var moduleData = moduleList.Select(c => new SelectListItem()
            {
                Text = c.Module_Name,
                Value = c.Module_ID.ToString(),
            });

            return Json(new { modules = moduleData }, JsonRequestBehavior.AllowGet);
        }



        public ActionResult GetFormsAccess(long userid,long FormmId)
        {

            var moduleList = db.REL_UserForm.ToList();

            var moduleListresult= from moduleListitem in moduleList
            where (moduleListitem.Frm_Id == FormmId) && (moduleListitem.User_Id == userid) 
            select moduleListitem.Flag_Visible;
            
            return Json(moduleListresult);
        }




        public JsonResult LoadFormsByModuleId(int moduleid)
        {
            List<MST_Form> FormList = new List<MST_Form>();

            var Forms = db.MST_Form.Where(m => m.Module_ID == moduleid).ToList();
            foreach (MST_Form objMST_Form in Forms)
            {
                MST_Form form = new MST_Form();
                form.Frm_Name = objMST_Form.Frm_Name;
                form.Frm_Id = objMST_Form.Frm_Id;
                FormList.Add(form);
            }

            return Json(FormList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Index(AssignFormsToUseViewModel AssignFormsToUser)
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                MST_User objMST_User = (MST_User)Session["UserInfo"];

                ViewBag.MST_Role = db.MST_Role.ToList();
                foreach (var item in AssignFormsToUser.FormList)
                {

                    db.spAddRelationalFormUser(AssignFormsToUser.UserID, item.Frm_Id, item.Flag_Visible, objMST_User.User_ID, objMST_User.User_ID);

                }

                return View();
            }
        }

        
    }
}

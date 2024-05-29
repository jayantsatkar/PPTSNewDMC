using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FactoryMagix.Models;

namespace FactoryMagix.Controllers
{
    public class PartialController : Controller
    {
        // GET: Partial

        BOSCH_PPTSEntities context = new BOSCH_PPTSEntities();

     
        [ChildActionOnly]
        public ActionResult _MenuLayout()
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                MST_User objMST_User = new MST_User();

                objMST_User = (MST_User)Session["UserInfo"];

                IList<spGetFormsandModuleforUser_Result> sp = context.spGetFormsandModuleforUser(objMST_User.User_ID).ToList();
                return View(sp);
            }
        }

        public ActionResult _LayoutHeader()
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
       

        
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FactoryMagix.Models;
using FactoryMagix.Repository;
namespace FactoryMagix.Controllers
{
    public class PartialController : Controller
    {
        // GET: Partial

    //    BOSCH_PPTSEntities context = new BOSCH_PPTSEntities();

     
        [ChildActionOnly]
        public ActionResult _MenuLayout()
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                User objMST_User = new User();

                objMST_User = (User)Session["UserInfo"];

                //  IList<spGetFormsandModuleforUser_Result> sp = context.spGetFormsandModuleforUser(objMST_User.User_ID).ToList();
                IList<FormModule> sp = ModuleRepository.GetFormModule(Convert.ToInt32(objMST_User.User_ID));

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
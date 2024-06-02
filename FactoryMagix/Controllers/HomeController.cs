using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FactoryMagix.Models;
using System.Data.SqlClient;
using System.Web.Security;
using System.Diagnostics;
using NLog; 

namespace FactoryMagix.Controllers
{
    public class HomeController : Controller
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();


        public ActionResult Index()
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                Process[] runingProcess = Process.GetProcesses();
                for (int i = 0; i < runingProcess.Length; i++)
                {
                    // compare equivalent process by their name
                    if (runingProcess[i].ProcessName == "ZK7500")
                    {
                        // kill  running process
                        runingProcess[i].Kill();
                    }

                }

                ViewBag.Message = "Your application description page.";
                return View();
            }
        }

        public ActionResult About()
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                ViewBag.Message = "Your application description page.";

                return View();
            }
        }

        public ActionResult Contact()
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                ViewBag.Message = "Your contact page.";

                return View();
            }
        }

        public ActionResult LogOut()
        {
            //Session.Clear();
            //FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
            //  Redirect("http://AnotherApplicaton/Home/LogOut");
        }
    }
}
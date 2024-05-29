using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FactoryMagix.Models;

namespace FactoryMagix.Controllers
{
    public class PartailBoxSerialNoController : Controller
    {

        private FactoryMagix.Models.BOSCH_PPTSEntities db = new FactoryMagix.Models.BOSCH_PPTSEntities();
        // GET: PartailBoxSerialNo
        public ActionResult Index()
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

        [ChildActionOnly]
        // GET: PartailBoxSerialNo/Details/5
        public ActionResult Details(int id)
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


        // GET: PartailBoxSerialNo/Create
        public ActionResult Create()
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

        // POST: PartailBoxSerialNo/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {

            try
            {
                if (Session["UserInfo"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    // TODO: Add insert logic here

                    return RedirectToAction("Index");
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: PartailBoxSerialNo/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PartailBoxSerialNo/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                if (Session["UserInfo"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: PartailBoxSerialNo/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PartailBoxSerialNo/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}

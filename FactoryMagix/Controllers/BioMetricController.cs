using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FactoryMagix.Models;
using System.Data.SqlClient;

namespace FactoryMagix.Controllers
{
    public class BioMetricController : Controller
    {
        private BOSCH_PPTSEntities db = new BOSCH_PPTSEntities();

        // GET: BioMetric
        public ActionResult Index(long? Id)
        {
            Session["UserIdfingerPrint"] = null;
            Session["UserIdfingerPrint"] = Id;            
            return View(db.MST_BioMetric.Where(m => m.User_ID == Id).ToList());
        }


        public ActionResult GetuserData()
        {
            
            
            MST_User objUserSession = (MST_User)Session["UserInfo"];

            return Json(objUserSession.User_ID+";" +Convert.ToString( Session["UserIdfingerPrint"]));


        }
        // GET: BioMetric/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MST_BioMetric mST_BioMetric = db.MST_BioMetric.Find(id);
            if (mST_BioMetric == null)
            {
                return HttpNotFound();
            }
            return View(mST_BioMetric);
        }

        // GET: BioMetric/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BioMetric/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MST_BioMetric_ID,User_ID,FingerPrint1,IsActive,Created_On,Created_By,Modified_By")] MST_BioMetric mST_BioMetric)
        {
            if (ModelState.IsValid)
            {
                db.MST_BioMetric.Add(mST_BioMetric);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mST_BioMetric);
        }

        // GET: BioMetric/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MST_BioMetric mST_BioMetric = db.MST_BioMetric.Find(id);
            if (mST_BioMetric == null)
            {
                return HttpNotFound();
            }
            return View(mST_BioMetric);
        }

        // POST: BioMetric/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MST_BioMetric_ID,User_ID,FingerPrint1,IsActive,Created_On,Created_By,Modified_By")] MST_BioMetric mST_BioMetric)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mST_BioMetric).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mST_BioMetric);
        }

        // GET: BioMetric/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MST_BioMetric mST_BioMetric = db.MST_BioMetric.Find(id);
            if (mST_BioMetric == null)
            {
                return HttpNotFound();
            }
            return View(mST_BioMetric);
        }

        // POST: BioMetric/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            MST_BioMetric mST_BioMetric = db.MST_BioMetric.Find(id);
            db.MST_BioMetric.Remove(mST_BioMetric);
            db.SaveChanges();
            return RedirectToAction("Index");
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

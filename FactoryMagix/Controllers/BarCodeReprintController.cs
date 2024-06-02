﻿using FactoryMagix.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FactoryMagix.Repository;

namespace FactoryMagix.Controllers
{
    public class BarCodeReprintController : Controller
    {
      // private FactoryMagix.Models.BOSCH_PPTSEntities db = new FactoryMagix.Models.BOSCH_PPTSEntities();
        // GET: BarCodeReprint
        public ActionResult BarCodeReprint()
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
        [HttpPost]
        public ActionResult GetBarCodeReprint(String SerialNo, long flag)
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                try
                {
                    //var loginUser = ((MST_User)Session["UserInfo"]).Login_ID;  
                    if (flag == 0)
                    {
                        var query = BoxRepository.GetBoxReprintDetails(SerialNo, Convert.ToInt32(flag)); ///db.spGetBarcodeDataprint(SerialNo, flag).ToList();
                        return Json(query);
                    }
                    else
                    {
                        var query = PalletRepository.GetPalletReprintDetails(SerialNo, Convert.ToInt32(flag));//db.spGetPalletBarcodeDataprint(SerialNo, flag).ToList();
                        return Json(query);
                    }
                }
                catch (Exception)
                {

                    throw;
                }

            }
        }

        
    }
}

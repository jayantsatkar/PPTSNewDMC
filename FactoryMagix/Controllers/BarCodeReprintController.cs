using FactoryMagix.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FactoryMagix.Repository;
using NLog;
using System.IO;
using System.Data;
using System.Configuration;

namespace FactoryMagix.Controllers
{
    public class BarCodeReprintController : Controller
    {
        // private FactoryMagix.Models.BOSCH_PPTSEntities db = new FactoryMagix.Models.BOSCH_PPTSEntities();
        // GET: BarCodeReprint
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        //User user;//= (User)Session["UserInfo"];
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
            DataTable dt = new DataTable();
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                try
                {
                    User user = (User)Session["UserInfo"];
                    //var loginUser = ((MST_User)Session["UserInfo"]).Login_ID;  
                    if (flag == 0)
                    {
                        dt = BoxRepository.GetBoxReprintDetails(SerialNo, Convert.ToInt32(flag)); ///db.spGetBarcodeDataprint(SerialNo, flag).ToList();
                        string clientIp = IpHelper.GetClientIpAddress(Request).Replace(':', '.');
                        Logger.Info("IP of Request:" + clientIp);
                        string path = ConfigurationManager.AppSettings["SharedDrive"].ToString();
                        string folderPath = Path.Combine(path, clientIp);
                        string shareName = clientIp;
                        //FolderHelper.CreateAndShareFolder(folderPath, shareName);

                        if(dt!= null && dt.Rows.Count > 0)
                        {
                            BoxRepository.CreatePRNFile(folderPath, dt, (int)LabelType.Box, user.Login_ID);
                        }
                        


                        // return Json(query);
                    }
                    else
                    {
                        dt = PalletRepository.GetPalletReprintDetails(SerialNo, Convert.ToInt32(flag));//db.spGetPalletBarcodeDataprint(SerialNo, flag).ToList();
                        string clientIp = IpHelper.GetClientIpAddress(Request).Replace(':', '.');
                        Logger.Info("IP of Request:" + clientIp);
                        string path = ConfigurationManager.AppSettings["SharedDrive"].ToString();
                        string folderPath = Path.Combine(path, clientIp);
                        string shareName = clientIp;
                        //FolderHelper.CreateAndShareFolder(folderPath, shareName);

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            PalletRepository.CreatePRNFile(folderPath, dt, (int)LabelType.Pallet, user.Login_ID);
                        }

                       // query = PalletRepository.GetPalletReprintDetails(SerialNo, Convert.ToInt32(flag));//db.spGetPalletBarcodeDataprint(SerialNo, flag).ToList();
                       // return Json(query);
                    }
                    //var json = "Success";
                    //return Json(json);
                }
                catch (Exception ex)
                {

                    Logger.Error("GetBarCodeReprint", ex);
                }
                
            }
            var json = "Success";
            return Json(json);
        }

        
    }
}

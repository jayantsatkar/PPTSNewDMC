using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FactoryMagix.Models;
using System.Web.Security;
using System.Data;
using System.Data.Entity;
using System.Net;
using System.Text;
using System.IO;
using System.Web.UI.WebControls;
using System.Web.UI;
using PagedList;
using PagedList.Mvc;
using System.ServiceModel.Channels;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Diagnostics;

namespace FactoryMagix.Controllers
{
    public class AccountController : Controller
    {


        BOSCH_PPTSEntities db = new BOSCH_PPTSEntities();
        // GET: Account

        public void EndExe()
        {
            Process[] runingProcess = Process.GetProcesses();
            for (int i = 0; i < runingProcess.Length; i++)
            {
                if (runingProcess[i].ProcessName == "ZK7500")
                {
                    runingProcess[i].Kill();
                }

            }
        }
        public ActionResult Login()
        {
            // TempData["WronguserName"] = "1";
            return View();
        }

        private string Encryptdata(string password)
        {
            string strmsg = string.Empty;
            byte[] encode = new byte[password.Length];
            encode = Encoding.UTF8.GetBytes(password);
            strmsg = Convert.ToBase64String(encode);
            return strmsg;
        }

        public ActionResult LoginDetails(string UserId)
        {
            string[] logindetails = UserId.Split(';');
            string userid, password;
            userid = logindetails[0];

            password = Encryptdata(logindetails[1]);
            spGetUserInfo_Result objspGetUserInfo_Result = new spGetUserInfo_Result();
            MST_User objMST_User = new MST_User();


            var result = db.spValidateUserforLogin(userid, password).ToList();
            if (result.Count > 0)
            {
                if (Convert.ToInt64(result[0]) > 0)
                {
                    objspGetUserInfo_Result = (db.spGetUserInfo(userid, password).ToList())[0];
                    objMST_User.User_ID = objspGetUserInfo_Result.User_ID;
                    objMST_User.Login_ID = objspGetUserInfo_Result.Login_ID;
                    objMST_User.Role_ID = objspGetUserInfo_Result.Role_ID;
                    objMST_User.First_Name = objspGetUserInfo_Result.First_Name;
                    //HttpCookie myCookie = new HttpCookie("UDID");
                    //Response.Cookies.Clear();
                    Session.Add("UserId", result[0]);
                    Session.Add("UserInfo", objMST_User);

                    FormsAuthentication.SetAuthCookie(objMST_User.Login_ID.ToUpper(), false);

                    TempData["WronguserName"] = "2";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    //TempData["WronguserName"] = "0";
                    Session["UserId"] = null;
                    Session["UserInfo"] = null;
                    return View("Login");
                }
            }
            else
            {
                //TempData["WronguserName"] = "0";
                Session["UserId"] = null;
                Session["UserInfo"] = null;
                return View("Login");
            }
        }

        public ActionResult LogOut()
        {
            Session.Clear();
            //Process[] runingProcess = Process.GetProcesses();
            //for (int i = 0; i < runingProcess.Length; i++)
            //{
            //    // compare equivalent process by their name
            //    if (runingProcess[i].ProcessName == "ZK7500")
            //    {
            //        // kill  running process
            //        runingProcess[i].Kill();
            //    }

            //}
            EndExe();
            FormsAuthentication.SignOut();
          
            return RedirectToAction("Login", "Account");
        }

        public ActionResult ChangePassword()
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                EndExe();
                MST_User objMST_User = (MST_User)Session["UserInfo"];
                var user = db.MST_User.Find(objMST_User.User_ID);

                return View(user);
            }
        }


        public ActionResult UpdatePassword(MST_User mST_User)
        {
            if (Session["UserInfo"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                return RedirectToAction("Index", "Home");
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



    }
}

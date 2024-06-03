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
using System.Data.SqlClient;
using NLog;

namespace FactoryMagix.Controllers
{
    public class AccountController : Controller
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();



        // BOSCH_PPTSEntities db = new BOSCH_PPTSEntities();
        List<SqlParameter> sqlParameters = new List<SqlParameter>();
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
            try
            {
                return View();// throw new Exception("Test exception");
            }

            catch (Exception ex)
            {
                Logger.Error("Error",ex);
            }

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
            // spGetUserInfo_Result objspGetUserInfo_Result = new spGetUserInfo_Result();
            User user = new User();

            sqlParameters.Clear();
            DBHelper.AddSqlParameter("@pLogin_Id", userid, ref sqlParameters);
            DBHelper.AddSqlParameter("@pUser_PWD", password, ref sqlParameters);

            var result = DBHelper.ExecuteProcedure("spValidateUserforLogin", sqlParameters);
            //var result = db.spValidateUserforLogin(userid, password).ToList();
            //DBHelper.ExecuteProcedure("ExecuteQuery",)
            if (result.Rows.Count > 0)
            {
                if (Convert.ToInt64(result.Rows[0][0]) > 0)
                {

                    //objspGetUserInfo_Result = (db.spGetUserInfo(userid, password).ToList())[0];
                    sqlParameters.Clear();
                    DBHelper.AddSqlParameter("@pLoginID", userid, ref sqlParameters);
                    DBHelper.AddSqlParameter("@pPass", password, ref sqlParameters);

                    var dataTableUser = DBHelper.ExecuteProcedure("spGetUserInfo", sqlParameters);
                    if (dataTableUser.Rows.Count > 0)
                    {
                        user.User_ID = Convert.ToInt64(dataTableUser.Rows[0]["User_ID"]);
                        user.Login_ID = Convert.ToString(dataTableUser.Rows[0]["Login_ID"]);
                        user.Role_ID = Convert.ToInt64(dataTableUser.Rows[0]["Role_ID"]);  
                        user.First_Name = Convert.ToString(dataTableUser.Rows[0]["First_Name"]);
                        //HttpCookie myCookie = new HttpCookie("UDID");
                        //Response.Cookies.Clear();
                        Session.Add("UserId", Convert.ToInt64(result.Rows[0][0]));
                        Session.Add("UserInfo", user);

                        FormsAuthentication.SetAuthCookie(user.Login_ID.ToUpper(), false);

                        TempData["WronguserName"] = "2";
                        return RedirectToAction("Index", "Home");
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
            else
            {
                //TempData["WronguserName"] = "0";
                Session["UserId"] = null;
                Session["UserInfo"] = null;
                return View("Login");
            }
            return View("Login"); //Not accessible
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

        //public ActionResult ChangePassword()
        //{
        //    if (Session["UserInfo"] == null)
        //    {
        //        return RedirectToAction("Login", "Account");
        //    }
        //    else
        //    {
        //        EndExe();
        //        MST_User objMST_User = (MST_User)Session["UserInfo"];


        //        var user = db.MST_User.Find(objMST_User.User_ID);

        //        return View(user);
        //    }
        //}


        public ActionResult UpdatePassword(User mST_User)
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

        public ActionResult Unauthorized()
        {
            return View();
        }

    }
}

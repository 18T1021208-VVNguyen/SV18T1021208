using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SV18T1021208.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
     /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
        // GET: Account
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            // ToDo: Kiem tra Code de kiem tra dung tai khoan.
            if (username == "admin@gmail.com" && password == "admin")
            {
                // Ghi cookie ghi nhan phien dang nhap
                FormsAuthentication.SetAuthCookie(username, false);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Username = username;
                ViewBag.Message = "Đăng nhập thất bại";

                return View();
            }
        }
        public ActionResult Change_Password()
        {
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Login");
        }


    }
}
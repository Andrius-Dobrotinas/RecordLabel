using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using RecordLabel.Web;

namespace RecordLabel.Web.Controllers
{
    public class AdminController : BaseController
    {
        [Authorize]
        public override ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginData data)
        {
           
            if (FormsAuthentication.Authenticate(data.UserName, data.Password))
            {
                FormsAuthentication.SetAuthCookie(data.UserName, data.RememberMe);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(String.Empty, "Bad user name or password");
                return View();
            }
            
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}
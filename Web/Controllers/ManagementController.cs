using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace RecordLabel.Web.Controllers
{
    public class ManagementController : BaseController
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
        public ActionResult Login(LoginData data, string ReturnUrl)
        {
            if (FormsAuthentication.Authenticate(data.UserName, data.Password))
            {
                FormsAuthentication.SetAuthCookie(data.UserName, data.RememberMe);
                if (!String.IsNullOrEmpty(ReturnUrl))
                {
                    return Redirect(ReturnUrl);
                }
                
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError(String.Empty, Localization.ManagementApplicationLocalization.Auth_BadCredentials);
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
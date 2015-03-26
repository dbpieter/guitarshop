using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using GuitarShopper_admin.Models;
using System.Web.Security;

namespace GuitarShopper_admin.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/
        public ActionResult Index()
        {
            return RedirectToAction("Login");
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(UserLoginModel ulm)
        {
            if (ModelState.IsValid)
            {
                user usr = user.CheckAuth(ulm);
                if (usr != null)
                {
                    FormsAuthentication.SetAuthCookie(usr.id.ToString(), true);
                    return RedirectToAction("Manage", "Products");
                }
            }

            ModelState.AddModelError("", "Het emailadres en/of wachtwoord is incorrect.");
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Admin");
        }

    }
}

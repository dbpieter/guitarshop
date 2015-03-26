using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using GuitarShopper_admin.Models;
using GuitarShopper_admin.Utilities;

namespace GuitarShopper_admin.Controllers
{
    [CustomAuthorize]
    public class CustomersController : Controller
    {
        //
        // GET: /Users/

        public ActionResult Index()
        {
            return RedirectToAction("Manage", "Users");
        }

        public ActionResult Manage(int ? id)
        {
            //ViewBag.customers = customer.GetAllUsersInfo(id);
            return View();
        }

    }
}

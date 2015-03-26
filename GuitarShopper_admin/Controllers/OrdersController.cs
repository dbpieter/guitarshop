using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using GuitarShopper_admin.Utilities;
using GuitarShopper_admin.Models;

namespace GuitarShopper_admin.Controllers
{
    [CustomAuthorize]
    public class OrdersController : Controller
    {
        //
        // GET: /Orders/

        public ActionResult Index()
        {
            return RedirectToAction("Manage");
        }

        public ActionResult Manage()
        {
            ViewBag.orders = order.GetAllOrders();
            return View();
        }

        public ActionResult ViewProducts(int ? orderid){

            return View();
        }

        [HttpPost]
        public JsonResult ChangeState(int? id, string state)
        {
            bool ok = order.ChangeOrderState(id, state);
            if (ok) return Json("Order state changed");
            return Json("Error changing order state");
        }

        // DELETE: /Orders/Delete
        [HttpDelete]
        public JsonResult Delete(int ? id)
        {
            order.DeleteFromId(id);
            return Json("Aaaaand it's gone, probably, if nothing went wrong");
        }

    }
}

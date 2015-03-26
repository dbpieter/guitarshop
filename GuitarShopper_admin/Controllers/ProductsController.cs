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
    public class ProductsController : Controller
    { 
        //Default stub
        
        public ActionResult Index()
        {
            return RedirectToAction("Manage");
        }

        
        public ActionResult Manage()
        {
            if(TempData.ContainsKey("message")) 
            {
                string test = (string) TempData["message"];
                ViewBag.message = TempData["message"];
            }
            ViewBag.products = product.GetAllProductsInfo();
            return View();
        }

        
        public ActionResult NewProduct(int ? category_id)
        {
            PopulateViewBag(category_id);

            return View();
        }

        [HttpPost]
        public ActionResult NewProduct(AddProductModel npm)
        {
            PopulateViewBag(null);
            if (ModelState.IsValid)
            {
                string error = "";
                if (!product.AddProduct(npm, ref error))
                {
                    ViewBag.error = error;
                }
                else
                {
                    TempData["message"] = " Nieuw product toegevoegd!";
                    return RedirectToAction("Manage");
                }
            }
            return View(npm);
        }

        public void PopulateViewBag(int ? category_id)
        {

            List<SelectListItem> catList = new List<SelectListItem>();
            foreach (category cat in category.GetAllCategories())
            {
                catList.Add(new SelectListItem { Text = cat.name, Value = cat.id.ToString() });
            }

            if (category_id != null)
            {
                SelectListItem itm = catList.FirstOrDefault(m => m.Value == category_id.ToString());
                itm.Selected = true;
            }
            else
            {
                catList[0].Selected = true;
                category_id = 1;
            }

            List<SelectListItem> subCatList = new List<SelectListItem>();
            foreach (subcategory subcat in subcategory.GetSubCatsFromCats(category_id.Value))
            {
                subCatList.Add(new SelectListItem { Text = subcat.name, Value = subcat.id.ToString() });
            }

            List<SelectListItem> brandList = new List<SelectListItem>();
            foreach (brand b in brand.GetAllBrands())
            {
                brandList.Add(new SelectListItem { Text = b.name, Value = b.id.ToString() });
            }

            ViewBag.brands = brandList;
            ViewBag.categories = catList;
            ViewBag.subcategories = subCatList;
        }

        public ActionResult EditProduct(int ? id)
        {
            EditProductModel emp = product.GetEditProductModel(id);
            ViewBag.pictures = product.GetAllImagesForProduct(id,false);
            if(emp == null){
                ViewBag.error = "Dit product bestaat niet";
                return View();
            }
            PopulateViewBag(emp.CategoryID);
            return View(emp);
        }

        [HttpPost]
        public ActionResult EditProduct(EditProductModel epm)
        {
            PopulateViewBag(category.GetCatIdFromSubCat(epm.SubcategoryID));
            if (ModelState.IsValid)
            {
                string error = "";
                if (!product.EditProduct(epm, ref error))
                {
                    ViewBag.error = error;
                    //System.Diagnostics.Debug.WriteLine(error);
                    return View(epm);
                }
                TempData["message"] = " Product is bijgewerkt";
                return RedirectToAction("Manage");
            }
            return View(epm);
        }

        public JsonResult FeatureProduct(int? id, bool featured)
        {
            product.FeatureProduct(id, featured);
            return Json("Feature attr changed");
        }

        // DELETE: /Product/Delete
        [HttpDelete]
        public JsonResult Delete(int id)
        {
            product.DeleteFromID(id);
            return Json("Aaaaand it's gone, probably, if nothing went wrong");
        }

        [Utilities.MyError]
        [HttpDelete]
        public JsonResult DeleteImage(int ? id, string imgname)
        {
            if (id == null || imgname == null || imgname.Length == 0 || !product.DeleteImage(id.Value,imgname))
            {
                return Json("Could not delete image");
            }
            return Json("Image deleted"); 
        }

        [HttpGet]
        public JsonResult GetSubCategories(int ? id)
        {
            if (id != null)
            {
                return Json(subcategory.GetSubCatFromCatsCustom(id.Value),JsonRequestBehavior.AllowGet);
            }
            else { 
                return Json("fail");
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public JsonResult Pictures(int? id)
        {
            return Json(product.GetAllImagesForProduct(id,false),JsonRequestBehavior.AllowGet);
        }

        //this is used by the shop to get the filenames of the pictures
        [HttpGet]
        [AllowAnonymous]
        public JsonResult GetImageNames(int? id)
        {
            return Json(product.GetAllImagesForProduct(id,true),JsonRequestBehavior.AllowGet);
        }
        }


    
}

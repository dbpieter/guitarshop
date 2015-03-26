using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

using GuitarShopper_admin.Utilities;
using System.IO;

namespace GuitarShopper_admin.Models
{
    /*
     * Model for the products table overview 
     */
    public class ProductForTable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string Color { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public bool Featured { get; set; }
    }

    /*
     * Model for adding a new product, includes validation 
     */
    public class AddProductModel
    {
        [Required(ErrorMessage = "Gelieve een naam op te geven.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Gelieve een merknaam te kiezen.")]
        public int BrandID { get; set; }

        [Required(ErrorMessage = "Gelieve een beschrijving op te geven.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Gelieve een subcategorie te kiezen.")]
        public int SubcategoryID { get; set; }

        [Required(ErrorMessage = "Gelieve een prijs op te geven.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Gelieve het aantal producten in stock op te geven")]
        [Range(0, Int32.MaxValue, ErrorMessage = "Ongeldig aantal items")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "Gelieve minstens 1 foto toe te voegen")]
        public IEnumerable<HttpPostedFileBase> Images { get; set; }

        public bool Featured { get; set; }

        public bool LeftHanded { get; set; }

        public string Color { get; set; }
    }

    /*
     * Model for editing a product, includes validation 
     */
    public class EditProductModel
    {
        [Required(ErrorMessage = "Uhm blijf daar eens af")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Gelieve een naam op te geven.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Gelieve een merknaam te kiezen.")]
        public int BrandID { get; set; }

        [Required(ErrorMessage = "Gelieve een beschrijving op te geven.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Gelieve een subcategorie te kiezen.")]
        public int SubcategoryID { get; set; }
        
        public int CategoryID { get; set; }

        [Required(ErrorMessage = "Gelieve een prijs op te geven.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Gelieve het aantal producten in stock op te geven")]
        [Range(0, Int32.MaxValue, ErrorMessage = "Ongeldig aantal items")]
        public int Stock { get; set; }

        public IEnumerable<HttpPostedFileBase> Images { get; set; }

        public bool Featured { get; set; }

        public bool LeftHanded { get; set; }

        public string Color { get; set; }
    }

    public partial class brand
    {

        public static IEnumerable<brand> GetAllBrands()
        {
            DataClassesDataContext dc = new DataClassesDataContext();
            return dc.brands.ToList();
        }
    }

    

    public partial class product
    {

        public static bool ProductExists(int ? id){
            if(id == null) return false;
            DataClassesDataContext dc = new DataClassesDataContext();
            return dc.products.FirstOrDefault(pro => pro.id == id.Value) != null;   
        }

        public static product GetProduct(int ? id){
            if(id == null) return null;
            DataClassesDataContext dc = new DataClassesDataContext();
            return dc.products.FirstOrDefault(pro => pro.id == id.Value);
        }

        public static IEnumerable<ProductForTable> GetAllProductsInfo()
        {
            DataClassesDataContext dc = new DataClassesDataContext();

            var query = from prods in dc.products
                        join subcats in dc.subcategories on prods.subcategory_id equals subcats.id
                        join cats in dc.categories on subcats.category_id equals cats.id
                        select new ProductForTable {Name = prods.name, Featured = prods.featured, Id= prods.id, Category = cats.name, SubCategory = subcats.name, Price = prods.price, Stock = prods.stock,Color = prods.color };

            return query.ToList();
        }

        public static bool FeatureProduct(int? id, bool featured)
        {
            DataClassesDataContext dc = new DataClassesDataContext();
            product p = dc.products.FirstOrDefault(pro => pro.id == id.Value);
            if (p == null) return false;

            p.featured = featured;

            try
            {
                dc.SubmitChanges();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public static bool DeleteFromID(int id)
        {
            DataClassesDataContext dc = new DataClassesDataContext();
            product p = dc.products.FirstOrDefault(prod => prod.id == id);

            if (p == null) return false;

            try
            {
                dc.products.DeleteOnSubmit(p);
                dc.SubmitChanges();

            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public static bool EditProduct(EditProductModel epm, ref string errorMsg)
        {
            if(!ProductExists(epm.Id)){
                errorMsg = "Kon product niet aanpassen";
                return false;
            }

            bool imageUploaded = false;
            foreach (HttpPostedFileBase file in epm.Images)
            {
                if (file != null)
                {
                    if (file != null && !Utils.IsImage(file))
                    {
                        errorMsg = "Ongeldig bestand toegevoegd";
                        return false;
                    }
                    imageUploaded = true;
                }
            }

            try
            {
                if (!imageUploaded && Directory.GetFiles(Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Content/images/products"), epm.Id.ToString() + "\\"), "*.png").Length == 0)
                {
                    errorMsg = "Dit product heeft geen afbeeldingen";
                    return false;
                }
            }
            catch (DirectoryNotFoundException ex)
            {
                string path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Content/images/products"), epm.Id + "\\");
                Directory.CreateDirectory(path);
            }

            DataClassesDataContext dc = new DataClassesDataContext();
            product p = dc.products.FirstOrDefault(prod => prod.id == epm.Id);

            p.name = epm.Name;
            p.description = epm.Description;
            p.price = Convert.ToDecimal(epm.Price);
            p.stock = epm.Stock;
            p.subcategory_id = epm.SubcategoryID;
            //p.dateadded = DateTime.Now;
            p.brand_id = epm.BrandID;
            p.color = epm.Color;

            try
            {
                dc.SubmitChanges();
            }
            catch (Exception ex)
            {
                errorMsg = "Er ging iets fout bij het aanpassen van het product";
                return false;
            }

            foreach (HttpPostedFileBase file in epm.Images)
            {
                if (file != null)
                {
                    Bitmap resizedImg = Utils.ResizeImage(Image.FromStream(file.InputStream), 266, 381);
                    string path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Content/images/products"), p.id.ToString() + "\\");
                    Directory.CreateDirectory(path);
                    resizedImg.Save(path + Path.GetFileNameWithoutExtension(file.FileName) + ".png");
                }
            }

            return true;
        }

        public static EditProductModel GetEditProductModel(int? id)
        {
            if (id == null) return null;
            DataClassesDataContext dc = new DataClassesDataContext();

            var query = from prods in dc.products
                        where prods.id == id.Value
                        join subcats in dc.subcategories on prods.subcategory_id equals subcats.id
                        join cats in dc.categories on subcats.category_id equals cats.id
                        select new EditProductModel { Id = prods.id, Color= prods.color, Name = prods.name, BrandID = prods.brand_id,CategoryID= cats.id, Description = prods.description, SubcategoryID = subcats.id, Price = prods.price, Stock = prods.stock };

            return query.FirstOrDefault();
        }

        public static List<string> GetAllImagesForProduct(int ? id, bool fileNameOnly)
        {
            List<string> lst = new List<string>();
            if (!ProductExists(id)) return lst;
            try
            {
                foreach (var file in Directory.GetFiles(Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Content/images/products"), id.Value.ToString() + "\\"), "*.png"))
                {
                    Console.WriteLine(Path.GetFileName(file));
                    if (!fileNameOnly)
                    {
                        lst.Add("~/Content/images/products/" + id.Value + "/" + Path.GetFileName(file));
                    }
                    else
                    {
                        lst.Add(Path.GetFileName(file));
                    }
                }
            }
            catch (Exception ex)
            {
                return new List<string>();
            }
            return lst;
        }

        public static bool DeleteImage(int id, string fileName)
        {
            bool imgDeleted = false;

            try
            {
                //keep at least one image
                if (Directory.GetFiles(Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Content/images/products"), id.ToString() + "\\"), "*.png").Length == 1)
                {
                    return false;
                }

                DirectoryInfo di = new DirectoryInfo(Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Content/images/products"), id.ToString() + "\\"));
                foreach (FileInfo file in di.GetFiles())
                {
                    if (file.Name == fileName)
                    {
                        file.Delete();
                        imgDeleted = true;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return imgDeleted;
        }

        public static bool AddProduct(AddProductModel npm, ref string errorMsg)
        {
            foreach (HttpPostedFileBase file in npm.Images)
            {
                if (!Utils.IsImage(file))
                {
                    errorMsg = "Ongeldig bestand";
                    return false;
                }
            }

            product p = new product();
            p.name = npm.Name;
            p.description = npm.Description;
            p.price = npm.Price;
            p.stock = npm.Stock;
            p.subcategory_id = npm.SubcategoryID;
            p.dateadded = DateTime.Now;
            p.brand_id = npm.BrandID;
            p.color = npm.Color;

            try
            {
                DataClassesDataContext dc = new DataClassesDataContext();
                dc.products.InsertOnSubmit(p);
                dc.SubmitChanges();
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return false;
            }

            foreach (HttpPostedFileBase file in npm.Images)
            {
                if (file != null)
                {
                    Bitmap resizedImg = Utils.ResizeImage(Image.FromStream(file.InputStream), 266, 381);
                    string path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Content/images/products"), p.id.ToString() + "\\");
                    Directory.CreateDirectory(path);
                    resizedImg.Save(path + Path.GetFileNameWithoutExtension(file.FileName) + ".png");
                }
            }
            return true;
        }
    }
}
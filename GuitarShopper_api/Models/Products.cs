using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace GuitarShopper_api.Models
{

    public class MyProduct
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public double Price { get; set; }
        public int Stock { get; set; }
    }

    public partial class product
    {

        private static string IMAGEPATH = "http://shop.pieterdebruyne.ikdoeict.net/productimages/";

        public static bool ProductExists(int ? id){
            if(id == null) return false;
            DataClassesDataContext dc = new DataClassesDataContext();
            return dc.products.FirstOrDefault(pro => pro.id == id.Value) != null;   
        }   

        public static IEnumerable<MyProduct> GetAllProductsInfo()
        {
            DataClassesDataContext dc = new DataClassesDataContext();

            var query = from prods in dc.products
                        join subcats in dc.subcategories on prods.subcategory_id equals subcats.id
                        join cats in dc.categories on subcats.category_id equals cats.id
                        select new MyProduct { Brand = prods.brand.name, Name = prods.name, Id = prods.id, Category = cats.name, SubCategory = subcats.name, Price = Convert.ToDouble(prods.price), Stock = prods.stock };

            return query.ToList();
        }

        public static MyProduct GetProductInfo(int id)
        {
            DataClassesDataContext dc = new DataClassesDataContext();

            var query = from prods in dc.products
                        where prods.id == id
                        join subcats in dc.subcategories on prods.subcategory_id equals subcats.id
                        join cats in dc.categories on subcats.category_id equals cats.id
                        select new MyProduct { Brand = prods.brand.name, Name = prods.name, Id = prods.id, Category = cats.name, SubCategory = subcats.name, Price = Convert.ToDouble(prods.price), Stock = prods.stock };

            try
            {
                return query.FirstOrDefault();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static bool OrderProduct(int ? id)
        {
            if(id == null) return false;
            DataClassesDataContext dc = new DataClassesDataContext();

            product p = dc.products.FirstOrDefault(pr => pr.id == id.Value);
            if (p == null) return false;

            if (p.stock > 0) p.stock--;
            try
            {
                dc.SubmitChanges();
            }
            catch(Exception ex)
            {
                return false;
            }
            return true;
        }

        public static List<string> GetAllImagesForProductRemote(int? id)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://admin.pieterdebruyne.ikdoeict.net/products/GetImageNames/" + id.Value);
            httpWebRequest.ContentType = "text/json";
            httpWebRequest.Method = "GET";

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            try
            {
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var responseText = streamReader.ReadToEnd();
                    //System.Diagnostics.Debug.WriteLine(responseText);

                    List<string> imageNames = ParseJsonArray(responseText);
                    List<string> images = new List<string>();

                    foreach (string s in imageNames)
                    {
                        images.Add(IMAGEPATH + id.Value + "/" + s);
                    }


                    return images;
                }
            }
            catch (Exception ex)
            {
                return new List<string>();
            }

        }

        /*
         * Kzalt zelf wel parsen gvd !!!
         */
        public static List<string> ParseJsonArray(string responseText)
        {
            //remove square brackets
            responseText = responseText.Trim();
            responseText = responseText.Remove(responseText.Length - 1);
            responseText = responseText.Remove(0, 1);

            //split
            string[] splitted = responseText.Split(',');

            //remove quotes
            for (int i = 0; i < splitted.Length; i++)
            {
                splitted[i] = splitted[i].Remove(splitted[i].Length - 1);
                splitted[i] = splitted[i].Remove(0, 1);
            }
            return splitted.ToList();
        }

        public static IEnumerable<MyProduct> FindProducts(string query)
        {
            List<MyProduct> found = new List<MyProduct>();
            if(query.Length == 0) return found;

            string[] querySplit = query.ToLower().Split(' ');
            foreach (MyProduct p in GetAllProductsInfo())
            {
                string[] nameSplit = p.Name.ToLower().Split(' ');
                foreach (string s in querySplit)
                {
                    if (nameSplit.Contains(s))
                    {
                        found.Add(p);
                        break; //adding once is enough
                    }
                }
            }
            return found;
        }

    }
}
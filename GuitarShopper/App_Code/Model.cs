using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

/// <summary>
/// Summary description for Model
/// </summary>
/// 
public static class Extensions
{
    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, Random rng)
    {
        T[] elements = source.ToArray();
        for (int i = elements.Length - 1; i >= 0; i--)
        {
            // Swap element "i" with a random earlier element it (or itself)
            // ... except we don't really need to swap it fully, as we can
            // return it immediately, and afterwards it's irrelevant.
            int swapIndex = rng.Next(i + 1);
            yield return elements[swapIndex];
            elements[swapIndex] = elements[i];
        }
    }
}

public class Model
{
    public Model() { }

    private static string IMAGEPATH = "http://shop.pieterdebruyne.ikdoeict.net/productimages/";
    private static bool LOCALIMAGES = false;
    private static string REMOTEIMAGEURL = "http://admin.pieterdebruyne.ikdoeict.net/products/GetImageNames/";

    private static string GetImagesPath()
    {
        return IMAGEPATH;
    }

    public static MyProduct GetMostExpensiveProduct()
    {
        DataClassesDataContext dc = new DataClassesDataContext();
        var query = from prods in dc.products
                    join subcats in dc.subcategories on prods.subcategory_id equals subcats.id
                    join cats in dc.categories on subcats.category_id equals cats.id
                    select new MyProduct { CategoryId = cats.id, BrandName = prods.brand.name, ImageURL = GetFirstImageForProduct(prods.id), Name = prods.name, Id = prods.id, CategoryName = cats.name, SubCategoryName = subcats.name, Price = Convert.ToDouble(prods.price), Stock = prods.stock };

        return query.OrderByDescending(p => p.Price).FirstOrDefault();
    }

    public static bool PlaceOrder(string email, string firstname, string lastname, string address, string postalcode, List<CartProduct> cart)
    {
        DataClassesDataContext dc = new DataClassesDataContext();

        //first insert and submit the customer
        customer c = new customer();
        c.address = address;
        c.email = email;
        c.postalcode = postalcode;
        c.lastname = lastname;
        c.firstname = firstname;

        try
        {
            dc.customers.InsertOnSubmit(c);
            dc.SubmitChanges();
        }
        catch (Exception ex)
        {
            return false;
        }

        //create the order
        order order = new order();
        order.date_added = DateTime.Now;
        order.customer_id = c.id;
        order.state = "verwerking"; //cfr opgave

        try
        {
            dc.orders.InsertOnSubmit(order);
            dc.SubmitChanges();
        }
        catch (Exception ex)
        {
            return false;
        }

        //create the order_has_products entries
        foreach (CartProduct cp in cart)
        {
            if (cp.Origin == ProductOrigin.local)
            {
                DecrementStock(cp.Id);
                orders_has_product ohp = new orders_has_product();
                ohp.orders_id = order.id;
                ohp.products_id = cp.Id;
                ohp.productorigin = cp.Origin.ToString();
                dc.orders_has_products.InsertOnSubmit(ohp);
            }
        }

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

    public static void DecrementStock(int productId)
    {
        DataClassesDataContext dc = new DataClassesDataContext();
        product p = dc.products.FirstOrDefault(pr => pr.id == productId);
        if (p != null) p.stock--;
        try
        {
            dc.SubmitChanges();
        }
        catch (Exception ex) { }
    }

    public static List<MyProduct> GetRandomProducts(int limit)
    {
        DataClassesDataContext dc = new DataClassesDataContext();
        var query = from prods in dc.products
                    join subcats in dc.subcategories on prods.subcategory_id equals subcats.id
                    join cats in dc.categories on subcats.category_id equals cats.id
                    select new MyProduct { CategoryId = cats.id, BrandName = prods.brand.name, ImageURL = GetFirstImageForProduct(prods.id), Name = prods.name, Id = prods.id, CategoryName = cats.name, SubCategoryName = subcats.name, Price = Convert.ToDouble(prods.price), Stock = prods.stock };

        return query.Shuffle(new Random()).Take(limit).ToList();
    }

    public static List<MyProduct> GetProductsFromCategory(int category_id)
    {
        DataClassesDataContext dc = new DataClassesDataContext();
        var query = from prods in dc.products
                    join subcats in dc.subcategories on prods.subcategory_id equals subcats.id
                    join cats in dc.categories on subcats.category_id equals cats.id
                    where cats.id == category_id
                    select new MyProduct { CategoryId = cats.id, BrandName = prods.brand.name, ImageURL = GetFirstImageForProduct(prods.id), Name = prods.name, Id = prods.id, CategoryName = cats.name, SubCategoryName = subcats.name, Price = Convert.ToDouble(prods.price), Stock = prods.stock };

        return query.ToList();
    }

    public static List<MyProduct> GetSimilarProducts(MyProduct p, int? limit)
    {
        DataClassesDataContext dc = new DataClassesDataContext();
        var query = from prods in dc.products
                    join subcats in dc.subcategories on prods.subcategory_id equals subcats.id
                    join cats in dc.categories on subcats.category_id equals cats.id
                    where cats.id == p.CategoryId
                    select new MyProduct { CategoryId = cats.id, BrandName = prods.brand.name, ImageURL = GetFirstImageForProduct(prods.id), Name = prods.name, Id = prods.id, CategoryName = cats.name, SubCategoryName = subcats.name, Price = Convert.ToDouble(prods.price), Stock = prods.stock };

        if (limit == null || limit < 1) return query.ToList();
        return query.Take(limit.Value).ToList();
    }

    public static string GetFirstImageForProduct(int? id)
    {
        List<string> images = GetAllImagesForProduct(id);
        if (images.Count > 0) return images[0];
        else return "";
    }

    public static List<string> GetAllImagesForProduct(int? id)
    {
        if (!ProductExists(id)) return new List<string>();
        if (LOCALIMAGES) return GetAllImagesForProductLocal(id);
        else return GetAllImagesForProductRemote(id);
    }

    public static List<string> GetAllImagesForProductLocal(int? id)
    {
        List<string> lst = new List<string>();

        try
        {
            var path = System.Web.HttpContext.Current.Server.MapPath(GetImagesPath() + id.Value.ToString() + "/");
            //System.Diagnostics.Debug.WriteLine(path);
            foreach (var file in Directory.GetFiles(path, "*.png"))
            {
                Console.WriteLine(Path.GetFileName(file));
                lst.Add(GetImagesPath() + id.Value + "/" + Path.GetFileName(file));
                //lst.Add(path);
            }
        }
        catch (Exception ex)
        {
            return new List<string>();
        }
        GetAllImagesForProductRemote(id);
        return lst;
    }

    //fetch image urls from admin part (can't scan remote virtual dir)
    public static List<string> GetAllImagesForProductRemote(int? id)
    {
        var httpWebRequest = (HttpWebRequest)WebRequest.Create(REMOTEIMAGEURL + id.Value);
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

    public static bool ProductExists(int? id)
    {
        if (id == null) return false;
        DataClassesDataContext dc = new DataClassesDataContext();
        return dc.products.FirstOrDefault(pro => pro.id == id.Value) != null;
    }

    public class BrandInfo
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public int Count { get; set; }
    }

    public static List<category> GetAllCategories()
    {
        return (new DataClassesDataContext()).categories.ToList();
    }

    public static List<subcategory> GetAllSubsFromCat(int category_id)
    {
        DataClassesDataContext dc = new DataClassesDataContext();
        return dc.subcategories.Where(sub => sub.category_id == category_id).ToList();
    }

    public static List<brand> GetAllBrands()
    {
        return (new DataClassesDataContext()).brands.ToList();
    }

    public static List<BrandInfo> GetAllBrandsWithProductCount()
    {
        DataClassesDataContext dc = new DataClassesDataContext();
        return dc.products.GroupBy(p => p.brand).Select(group => new BrandInfo { Id = group.Key.id, Name = group.Key.name, Count = group.Count() }).OrderBy(x => x.Count).ToList();
    }

    public static List<MyProduct> GetAllProductsInfo()
    {
        DataClassesDataContext dc = new DataClassesDataContext();

        var query = from prods in dc.products
                    join subcats in dc.subcategories on prods.subcategory_id equals subcats.id
                    join cats in dc.categories on subcats.category_id equals cats.id
                    select new MyProduct { Color = prods.color, CategoryId = cats.id, BrandName = prods.brand.name, ImageURL = GetFirstImageForProduct(prods.id), Name = prods.name, Id = prods.id, CategoryName = cats.name, SubCategoryName = subcats.name, Price = Convert.ToDouble(prods.price), Stock = prods.stock, SubCategoryId = subcats.id };

        return query.ToList();
    }

    public static List<MyProduct> GetAllProductsFromCart(List<CartProduct> cart)
    {
        List<int> idList = new List<int>();

        foreach(CartProduct cp in cart){
            idList.Add(cp.Id);
        }
        idList = idList.Distinct().ToList();

        DataClassesDataContext dc = new DataClassesDataContext();

        var query = from prods in dc.products
                    join subcats in dc.subcategories on prods.subcategory_id equals subcats.id
                    join cats in dc.categories on subcats.category_id equals cats.id
                    select new MyProduct { Color = prods.color, CategoryId = cats.id, BrandName = prods.brand.name, ImageURL = GetFirstImageForProduct(prods.id), Name = prods.name, Id = prods.id, CategoryName = cats.name, SubCategoryName = subcats.name, Price = Convert.ToDouble(prods.price), Stock = prods.stock, Description = prods.description, SubCategoryId = subcats.id };

        return query.Where(p => idList.Contains(p.Id)).ToList();
    }

    public static MyProduct GetProductInfo(int id)
    {
        DataClassesDataContext dc = new DataClassesDataContext();

        var query = from prods in dc.products
                    where prods.id == id
                    join subcats in dc.subcategories on prods.subcategory_id equals subcats.id
                    join cats in dc.categories on subcats.category_id equals cats.id
                    select new MyProduct { Color = prods.color, CategoryId = cats.id, BrandName = prods.brand.name, ImageURL = GetFirstImageForProduct(prods.id), Name = prods.name, Id = prods.id, CategoryName = cats.name, SubCategoryName = subcats.name, Price = Convert.ToDouble(prods.price), Stock = prods.stock, Description = prods.description, SubCategoryId = subcats.id };

        return query.FirstOrDefault();
    }

    public static List<MyProduct> GetFeaturedProducts()
    {
        DataClassesDataContext dc = new DataClassesDataContext();

        var query = from prods in dc.products
                    where prods.featured
                    join subcats in dc.subcategories on prods.subcategory_id equals subcats.id
                    join cats in dc.categories on subcats.category_id equals cats.id
                    select new MyProduct { Color = prods.color, CategoryId = cats.id, BrandName = prods.brand.name, ImageURL = GetFirstImageForProduct(prods.id), Name = prods.name, Id = prods.id, CategoryName = cats.name, SubCategoryName = subcats.name, Price = Convert.ToDouble(prods.price), Stock = prods.stock, SubCategoryId = subcats.id };

        return query.ToList();
    }

    public static List<MyProduct> FindProducts(string category_id, string subcategory_id, string brand_id,string minprice,string maxprice)
    {
        DataClassesDataContext dc = new DataClassesDataContext();
        var query = from prods in dc.products
                    join subcats in dc.subcategories on prods.subcategory_id equals subcats.id
                    join cats in dc.categories on subcats.category_id equals cats.id
                    select new MyProduct { Color = prods.color, SubCategoryId = subcats.id, BrandId = prods.brand_id, CategoryId = cats.id, BrandName = prods.brand.name, ImageURL = GetFirstImageForProduct(prods.id), Name = prods.name, Id = prods.id, CategoryName = cats.name, SubCategoryName = subcats.name, Price = Convert.ToDouble(prods.price), Stock = prods.stock };

        if (subcategory_id != null)
        {
            try
            {
                int sid = Convert.ToInt32(subcategory_id);
                if (sid > 0)
                {
                    query = query.Where(p => p.SubCategoryId == sid);
                }
            }
            catch (Exception ex) { } //if Convert fails don't filter
        }
        else if (category_id != null) // no need to filter cats if already filtering on subcats
        {
            try
            {
                int cid = Convert.ToInt32(category_id);
                if (cid >= 0)
                {
                    query = query.Where(p => p.CategoryId == cid);
                }

            }
            catch (Exception ex) { } //if Convert fails don't filter
        }
        if (brand_id != null)
        {
            try
            {
                int bid = Convert.ToInt32(brand_id);
                if (bid >= 0)
                {
                    query = query.Where(p => p.BrandId == bid);
                }
            }
            catch (Exception ex) { } //if Convert fails don't filter
        }
        if (minprice != null)
        {
            try
            {
                double min = Convert.ToDouble(minprice);
                if (min >= 0)
                {
                    query = query.Where(p => p.Price >= min);
                }
            }
            catch (Exception ex) { } //if Convert fails don't filter
        }
        if (maxprice != null)
        {
            try
            {
                double max = Convert.ToDouble(maxprice);
                query = query.Where(p => p.Price <= max);
            }
            catch (Exception ex) { } //if Convert fails don't filter
        }
            
        return query.ToList();
    }

    public static List<MyProduct> FindProducts(string query)
    {
        List<MyProduct> found = new List<MyProduct>();
        if (query.Length == 0) return GetAllProductsInfo();

        string[] querySplit = query.ToLower().Split(' ');
        foreach (MyProduct p in GetAllProductsInfo())
        {
            string[] nameSplit = p.Name.ToLower().Split(' ');
            foreach (string s in querySplit)
            {
                if (nameSplit.Contains(s))
                {
                    found.Add(p);
                    break; //add only once
                }
            }
        }
        return found;
    }

    public static List<ProductResponse> GetRemoteProducts(string baseAdr,string query)
    {
        List<ProductResponse> remoteBooks = null;
        using (HttpClient client = new HttpClient())
        {
            client.BaseAddress = new Uri(baseAdr);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.GetAsync("api/Products?searchquery="+query).Result;
            if (response.IsSuccessStatusCode)
            {
                remoteBooks = response.Content.ReadAsAsync<List<ProductResponse>>().Result;
            }
            else
            {
                return new List<ProductResponse>();
            }
        }
        return remoteBooks;
    }

    public static bool ProductIsAvailable(int productId)
    {
        DataClassesDataContext dc = new DataClassesDataContext();
        product p = dc.products.FirstOrDefault(pr => pr.id == productId);
        if (p == null) return false;
        if (p.stock > 0) return true;
        return false;
    }

}

public class ProductResponse
    {
        public string name { get; set; }
        public string price { get; set; }
        public string picture { get; set; }
        public int id { get; set; }
    }

public enum ProductOrigin
{
    local,gilles,michiel
}

public class CartProduct
{
    public int Id { get; set; }
    public ProductOrigin Origin { get; set; }
}

/*
 * Custom Product model
 */
public class MyProduct
{
    public int Id { get; set; }
    public string Color { get; set; }
    public string Description { get; set; }
    public string Name { get; set; }
    public string CategoryName { get; set; }
    public int CategoryId { get; set; }
    public string SubCategoryName { get; set; }
    public int SubCategoryId { get; set; }
    public double Price { get; set; }
    public int Stock { get; set; }
    public string ImageURL { get; set; }
    public string BrandName { get; set; }
    public int BrandId { get; set; }
    public string Availablity
    {
        get
        {
            if (Stock <= 0) return "Uit stock";
            return Stock.ToString() + " stuks beschikbaar";
        }
    }
}


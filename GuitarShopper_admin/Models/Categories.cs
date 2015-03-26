using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GuitarShopper_admin.Models
{
    public partial class category
    {
        public static List<category> GetAllCategories()
        {
            return (new DataClassesDataContext()).categories.ToList();
        }

        public static int ? GetCatIdFromSubCat(int subcatid)
        {
            DataClassesDataContext dc = new DataClassesDataContext();
            return dc.subcategories.FirstOrDefault(subcat => subcat.id == subcatid).category_id;
        }
    }


    public class MySubCat
    {
        public string ID { get; set; }
        public string Name { get; set; }
    }

    public partial class subcategory {

        public static List<subcategory> GetSubCatsFromCats(int cat_id)
        {
            DataClassesDataContext dc = new DataClassesDataContext();
            return dc.subcategories.Where(sub => sub.category_id == cat_id).ToList();
        }

        public static List<MySubCat> GetSubCatFromCatsCustom(int cat_id)
        {
            DataClassesDataContext dc = new DataClassesDataContext();
            return dc.subcategories.Where(sub => sub.category_id == cat_id).Select(subcat=> new MySubCat{Name = subcat.name, ID = subcat.id.ToString()}).ToList();
        }
    }

}
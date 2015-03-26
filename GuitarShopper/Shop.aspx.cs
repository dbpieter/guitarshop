using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Shop : System.Web.UI.Page
{
    private class PaginationObj
    {
        public int PageNr { get; set; }
        public string CssClass { get; set; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            List<MyProduct> products = null;

            //parse filter parameters
            if (Request.Params["search"] != null)
            {
                products = Model.FindProducts(Request.Params["search"]);
            }
            else
            {
                products = Model.FindProducts(Request.Params["category_id"], Request.Params["subcategory_id"], Request.Params["brand_id"],Request.Params["minprice"],Request.Params["maxprice"]);
            }

            int productsperpage = 6;
            int pageNr = 1;

            //pagination logic
            if (Request.Params["page"] != null)
            {
                try
                {
                    pageNr = Convert.ToInt32(Request.Params["page"]);
                }
                catch (Exception ex) { } //Convert exception
            }
            int productCount = products.Count();
            double d = (double)productCount / (double)productsperpage;
            int nrOfPages = (int)Math.Ceiling(d);

            if (pageNr <= nrOfPages)
            {
                products = Paginate(products, productsperpage, pageNr);
            }
            else
            {
                pageNr = 1;
            }

            ProductsRepeater.DataSource = products;
            ProductsRepeater.DataBind();

            List<PaginationObj> lst = new List<PaginationObj>();

            for (int i = 1; i <= nrOfPages; i++)
            {
                PaginationObj po = new PaginationObj();
                po.PageNr = i;
                if (i == pageNr) po.CssClass = "active";
                lst.Add(po);
            }

            if (nrOfPages <= 1) pagination.Visible = false;
            if (pageNr == 1) GoBack.Visible = false;
            if (pageNr == nrOfPages) GoForward.Visible = false;

            PaginationRepeater.DataSource = lst;
            PaginationRepeater.DataBind();

        }
    }

    //page numbering starts on 1
    private static List<MyProduct> Paginate(List<MyProduct> products, int productsperpage, int page)
    {
        return products.Skip((page - 1) * productsperpage).Take(productsperpage).ToList();
    }

    protected void linkBtn_Command(object sender, CommandEventArgs e)
    {
        if (e.CommandName == "OrderLocal")
        {
            if (Session["Cart"] != null)
            {
                List<CartProduct> cart = (List<CartProduct>)Session["Cart"];
                CartProduct cp = new CartProduct { Id = Convert.ToInt32(e.CommandArgument), Origin = ProductOrigin.local };
                if (!cart.Contains(cp) && Model.ProductIsAvailable(Convert.ToInt32(e.CommandArgument)))
                {
                    cart.Add(cp);
                }
            }
        }
    }
}
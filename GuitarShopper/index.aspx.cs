using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class index : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //FeaturedItems.DataSource = Model.GetFeaturedProducts();
        //FeaturedItems.DataBind();
        if (!IsPostBack)
        {
            FeaturedItems.DataSource = Model.GetFeaturedProducts().Take(6);
            FeaturedItems.DataBind();

            ElekGtrRepeater.DataSource = Model.GetProductsFromCategory(2).Take(4);
            ElekGtrRepeater.DataBind();

            AcousticRepeater.DataSource = Model.GetProductsFromCategory(3).Take(4);
            AcousticRepeater.DataBind();

            BassRepeater.DataSource = Model.GetProductsFromCategory(4).Take(4);
            BassRepeater.DataBind();

            AccessoriesRepeater.DataSource = Model.GetProductsFromCategory(1).Take(4);
            AccessoriesRepeater.DataBind();

            List<MyProduct> products = Model.GetRandomProducts(6);

            RecommendedRepeater1.DataSource = products.Take(3).ToList();
            RecommendedRepeater1.DataBind();

            RecommendedRepeater2.DataSource = products.Skip(3).ToList();
            RecommendedRepeater2.DataBind();
        }
        

        
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
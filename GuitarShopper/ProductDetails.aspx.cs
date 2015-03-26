using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ProductDetails : System.Web.UI.Page
{
    public MyProduct Product { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
            var test = Request.QueryString["id"];
            if (test == null)
            {
                Response.Redirect("index.aspx");
            }
            try
            {
                Product = Model.GetProductInfo(Convert.ToInt32(Request.QueryString["id"]));
            }
            catch (Exception ex)
            {
                Response.Redirect("index.aspx");
            }
            if (Product == null)
            {
                Response.Redirect("index.aspx");
            }

            
            ImagesRepeater.DataSource = Model.GetAllImagesForProduct(Product.Id).Select(x => new { Value = x }).ToList();
            ImagesRepeater.DataBind();

            List<MyProduct> products = Model.GetSimilarProducts(Product, 6);

            SimilarRepeater1.DataSource = products.Take(3).ToList();
            SimilarRepeater1.DataBind();

            var sim2 = products.Skip(3).ToList();
            if (sim2.Count == 0) similar2.Visible = false;
            SimilarRepeater2.DataSource = sim2;
            SimilarRepeater2.DataBind();

            AddToCartButton.CommandArgument = Product.Id.ToString();

            List<ProductResponse> r1 = Model.GetRemoteProducts("http://api.michieldedeyne.ikdoeict.net/", Product.Name).Take(3).ToList();
            List<ProductResponse> r2 = Model.GetRemoteProducts("http://api.gillesdeblock.ikdoeict.net/",Product.Name).Take(3).ToList();

            if (r1.Count == 0) RemoteProducts1.Visible = false;
            if (r2.Count == 0) RemoteProducts2.Visible = false;
            if (r1.Count == 0 && r2.Count == 0) RemoteProductsDiv.Visible = false;

            RemoteRepeater1.DataSource = r1;
            RemoteRepeater1.DataBind();

            RemoteRepeater2.DataSource = r2;
            RemoteRepeater2.DataBind();
        
    }

    protected void ImagesRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemIndex == 0)
        {
            e.Item.FindControl("tzeDiv").Visible = false;
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
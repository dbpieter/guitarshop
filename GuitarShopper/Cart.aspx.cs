using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Cart : System.Web.UI.Page
{
    public double TotalPrice { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            List<CartProduct> cart = (List<CartProduct>)Session["cart"];

            if (cart != null)
            {
                TotalPrice = 0;
                List<MyProduct> products = Model.GetAllProductsFromCart(cart);
                foreach (MyProduct p in products)
                {
                    TotalPrice += p.Price;
                }
                cartRowRepeater.DataSource = products;
                cartRowRepeater.DataBind();
            }

        }
    }

    protected void DeleteFromCartBtn_Command(object sender, CommandEventArgs e)
    {
        List<CartProduct> cart = (List<CartProduct>)Session["cart"];
        int id = Convert.ToInt32(e.CommandArgument);
        CartProduct cp = cart.Where(c => c.Id == id).FirstOrDefault(); ;
        cart.Remove(cp);
        Response.Redirect(Request.RawUrl);
    }
}
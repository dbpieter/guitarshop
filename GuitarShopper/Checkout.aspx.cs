using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Checkout : System.Web.UI.Page
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
        List<CartProduct> cart = (List<CartProduct>)Session["Cart"];
        int id = Convert.ToInt32(e.CommandArgument);
        CartProduct cp = cart.Where(c => c.Id == id).FirstOrDefault(); ;
        cart.Remove(cp);
        Response.Redirect(Request.RawUrl);
    }


    protected void CheckoutButton_Click(object sender, EventArgs e)
    {
        string email = this.EmailField.Text;
        string firstName = this.FirstNameField.Text;
        string lastName = this.LastNameField.Text;
        string address = this.AddressField1.Text + " " + this.AddressField2.Text;
        string postalcode = this.PostalCodeField.Text;

        bool hasError = false;

        //manual error rendering, requiredfieldvalidators suck
        if (email.Length == 0)
        {
            EmailField.CssClass += "errorinput";
            hasError = true;
        }
        if (firstName.Length == 0)
        {
            FirstNameField.CssClass += "errorinput";
            hasError = true;
        }
        if (lastName.Length == 0)
        {
            LastNameField.CssClass += "errorinput";
            hasError = true;
        }
        if (AddressField1.Text.Length == 0)
        {
            AddressField1.CssClass += "errorinput";
            hasError = true;
        }
        if (postalcode.Length == 0)
        {
            PostalCodeField.CssClass += "errorinput";
            hasError = true;
        }

        List<CartProduct> cart = (List<CartProduct>)Session["Cart"];

        if (cart == null || cart.Count == 0)
        {
            cartError.Visible = true;
            hasError = true;
        }

        if (hasError)
        {
            errorMsg.Visible = true;
            return;
        }

        if (Model.PlaceOrder(email, firstName, lastName, address, postalcode, cart))
        {
            checkoutFinished.Visible = true;
            checkoutUnfinished.Visible = false;
            Session["Cart"] = new List<CartProduct>();
        }
        else
        {
            //well shit
        }

    }
}
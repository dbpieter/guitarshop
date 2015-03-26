using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterPage : System.Web.UI.MasterPage
{
    public static int MaxPrice { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            CategoryRepeater.DataSource = Model.GetAllCategories();
            CategoryRepeater.DataBind();

            brandsRepeater.DataSource = Model.GetAllBrandsWithProductCount();
            brandsRepeater.DataBind();

            if (Session["Cart"] == null)
            {
                Session["Cart"] = new List<CartProduct>();
            }

            MyProduct exp = Model.GetMostExpensiveProduct();
            MaxPrice = (int)Math.Ceiling(exp.Price);
        }
    }

    protected void CategoryRepeater_ItemDataBound(object sender, RepeaterItemEventArgs args)
    {
        if (args.Item.ItemType == ListItemType.Item || args.Item.ItemType == ListItemType.AlternatingItem)
        {
            HiddenField hiddenField = (HiddenField)args.Item.FindControl("hiddenField");
            int id = Convert.ToInt32(hiddenField.Value);
        
        Repeater childRepeater = (Repeater)args.Item.FindControl("SubCatRepeater");
        childRepeater.DataSource = Model.GetAllSubsFromCat(id);
        childRepeater.DataBind();
        }
    }
}

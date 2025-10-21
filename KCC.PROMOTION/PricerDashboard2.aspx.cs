using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PricerDashboard : System.Web.UI.Page
{
    protected void lnklogout_Click(object sender, EventArgs e) {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        this.Response.Redirect("login.aspx");
    }

    protected void Page_Load(object sender, EventArgs e) {
        Response.Redirect("PricerDashboard.aspx");

        //if (Session["username"] == null) {
        //    Response.Redirect("login.aspx");
        //} else {
        //    string userRole = Session["rolename"].ToString();

        //    if (userRole == "KCC_PM_BYR") {
        //        Response.Redirect("buyerDashboard.aspx");
        //    } else if (userRole == "KCC_PM_SA") {
        //        Response.Redirect("sellingAreaDashboard.aspx");
        //    } else if (userRole == "KCC_PM_UM") {
        //        Response.Redirect("");
        //    }


        //    if (userRole == "KCC_PM_RM") {
        //        //lnkReprintManager.Visible = true;
        //    }

        //}

    }
}
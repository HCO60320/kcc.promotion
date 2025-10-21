namespace KCC.PROMOTION
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Data;

    public partial class reprintmanager : System.Web.UI.Page
    {
        protected void lnklogout_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            this.Response.Redirect("login.aspx");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["username"] == null)
            {
                Response.Redirect("login.aspx");
            }
            else
            {
                int userID = Convert.ToInt32(Session["userId"]);
                DataTable dtUserRole = new DataTable();
                userModelClass userModel = new userModelClass();
                dtUserRole = userModel.get_user_role(Convert.ToInt32(Session["applicationId"]), userID);

                if (dtUserRole.Rows.Count < 1)
                {
                    Response.Redirect("login.aspx");
                }
                else
                {
                    string userRole = dtUserRole.Rows[0]["ROLE_NAME"].ToString();
                    HttpContext.Current.Session.Add("rolename", userRole);

                    if (userRole == "KCC_PM_SA")
                    {
                        Response.Redirect("SellingAreaDashboard.aspx");
                    }
                    else if (userRole == "KCC_PM_BYR")
                    {
                        Response.Redirect("buyerDashboard.aspx");
                    }
                    else if (userRole == "KCC_PM_PRC")
                    {
                        Response.Redirect("pricerDashboard.aspx");
                    }
                }
            }
        }
    }
}
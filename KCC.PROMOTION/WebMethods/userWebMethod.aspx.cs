namespace KCC.PROMOTION.WebMethods
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using Oracle.ManagedDataAccess.Client;
    using System.Data;
    using System.Web.Services;
    using promotions;
    using System.Web.Script.Serialization;
    using Oracle.ManagedDataAccess;

    public partial class userWebMethod : System.Web.UI.Page
    {
        [WebMethod()]
        public static string check_user_access(string username, string password, int applicationId, string connectionType)
        {
            object resp = null;
            connection.username = username;
            connection.password = password;
            connection.port = 1521;
            if (connectionType == "2")
            {
                connection.host = "192.168.32.101";
                connection.userSchema = "rms13prd";
                connection.sID = "rmsprd";
                connection.connectionType = "PRODUCTION";
            }
            else
            {
                connection.host = "192.168.32.101";
                connection.userSchema = "rms13prd";
                connection.sID = "rmsprd";
                connection.connectionType = "DEVELOPMENT";
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            try
            {
                connection.instantiate();
                OracleConnection cn = new OracleConnection(connection.connStr);

                cn.Open();
                cn.Close();

                if (applicationId == 193 || applicationId == 173)
                {
                    //resp = new { status = "1", url = "http://192.168.33.197/KCCMDPrep/Login.aspx?u=" + username + "&p=" + password + "&connType=" + connectionType + "&appID=" + applicationId };
                    resp = new { status = "1", url = "http://192.168.32.178/KCCMDPrep/Login.aspx?u=" + username + "&p=" + password + "&connType=" + connectionType + "&appID=" + applicationId };
                    return serializer.Serialize(resp);
                }
                else if (applicationId == 112)
                {
                    //resp = new { status = "1", url = "http://192.168.33.197/KCCMDPrep/Login.aspx?u=" + username + "&p=" + password + "&connType=" + connectionType + "&appID=" + applicationId };
                    resp = new { status = "1", url = "http://192.168.32.178/KCCMDPrep/Login.aspx?u=" + username + "&p=" + password + "&connType=" + connectionType + "&appID=" + applicationId };
                    return serializer.Serialize(resp);
                }

                DataTable dtUser = new DataTable();
                userModelClass userModel = new userModelClass();

                dtUser = userModel.getUser(username.ToUpper(), password.ToUpper());

                //checks user role 
                int userID = Convert.ToInt32(dtUser.Rows[0]["USER_ID"]);

                HttpContext.Current.Session.Add("username", dtUser.Rows[0]["username"]);
                HttpContext.Current.Session.Add("userid", userID);
                HttpContext.Current.Session.Add("applicationId", applicationId);

                DataTable dtUserRole = new DataTable();

                dtUserRole = userModel.get_user_role(applicationId, userID);
                // KCC_MD_SYSTEM = appID: 112
                // KCC_PROMO_SYSTEM = appID: 152

                if (dtUserRole.Rows.Count < 1)
                {
                    resp = new { status = "0", message = "User has no Role,Please contact the Administrator" };
                    return serializer.Serialize(resp);
                }
                else
                {
                    string userRole = dtUserRole.Rows[0]["ROLE_NAME"].ToString();
                    HttpContext.Current.Session.Add("rolename", userRole);

                    if (applicationId == 49)
                    {
                        //promotion pages
                        if (userRole == "KCC_PM_SA")
                        {
                            resp = new { status = "1", url = "SellingAreaDashboard.aspx" };
                        }
                        else if (userRole == "KCC_PM_BYR")
                        {
                            resp = new { status = "1", url = "BuyerDashboard.aspx" };
                        }
                        else if (userRole == "KCC_PM_PRC")
                        {
                            resp = new { status = "1", url = "PricerDashboard.aspx" };
                        }
                        else if (userRole == "KCC_PM_UM")
                        {
                            resp = new { status = "1", url = "reprintmanager.aspx" };
                        }
                        else if (userRole == "KCC_PM_RM")
                        {
                            resp = new { status = "1", url = "reprintmanager.aspx" };
                        }
                    }
                    else
                    {
                        //markdown pages
                        if (userRole == "KCC_MD_SA")
                        {
                            resp = new { status = "1", url = "SellingAreaDashboard.aspx" };
                        }
                        else if (userRole == "KCC_MD_BYR")
                        {
                            resp = new { status = "1", url = "BuyerDashBoard.aspx" };
                        }
                        else if (userRole == "KCC_MD_PRC")
                        {
                            resp = new { status = "1", url = "PricerDashboard.aspx" };
                        }
                        else if (userRole == "KCC_MD_UM")
                        {
                            resp = new { status = "1", url = "UserManager.aspx" };
                        }
                        else if (userRole == "KCC_MD_RM")
                        {
                            resp = new { status = "1", url = "reprintmanager.aspx" };
                        }
                        else if (userRole == "KCC_AGECODETRACKER")
                        {
                            resp = new { status = "1", url = "AgeCodeTracker.aspx" };
                        }

                    }

                    return serializer.Serialize(resp);
                }
            }
            catch (Exception ex)
            {
                resp = new { status = "2", message = ex.Message };
                return serializer.Serialize(resp);
            }
        }
    }
}
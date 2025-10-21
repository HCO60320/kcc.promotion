using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Oracle.ManagedDataAccess;
using Oracle.ManagedDataAccess.Client;
using Newtonsoft;
using promotions;
using System.Web.Services;

public partial class WebMethods_adminWebMethod : System.Web.UI.Page
{
    [WebMethod()]
    public static string getReprintTransactions()
    {
        object resp = null;
        try
        {
            DataTable dtReprintTransaction = new DataTable();
            adminModelClass adminModelObj = new adminModelClass();
            dtReprintTransaction = adminModelObj.getReprintTransactions();
            resp = new { status = "1", transactions = dtReprintTransaction };        
        }
        catch (Exception ex)
        {
            resp = new { status = "2", message = ex.Message };
        }
        resp = Newtonsoft.Json.JsonConvert.SerializeObject(resp, Newtonsoft.Json.Formatting.Indented);
        return resp.ToString();
    }

    [WebMethod()]
    public static string getReprintTransaction(int headId) {
        object resp = null;
        try {
            DataTable dtReprintTransaction = new DataTable();
            adminModelClass adminModelObj = new adminModelClass();
            dtReprintTransaction = adminModelObj.getReprintTransaction(headId);

            if (dtReprintTransaction.Rows.Count == 0) {
                resp = new { status = "0", message = "No Reprint Request Detail" };
            } else {
                resp = new { status = "1", reprintTransaction = dtReprintTransaction };
            }
           
        } catch (Exception ex) {
            resp = new { status = "2", message = ex.Message };
        }

        resp = Newtonsoft.Json.JsonConvert.SerializeObject(resp, Newtonsoft.Json.Formatting.Indented);
        return resp.ToString();
    }

    [WebMethod()]
    public static string getPurgeTransactions()
    {
        object resp = null;
        try
        {
            DataTable dtPurgeTransaction = new DataTable();
            adminModelClass adminModelObj = new adminModelClass();
            dtPurgeTransaction = adminModelObj.getPurgeTransactions();
            resp = new { status = "1", transactions = dtPurgeTransaction };
        }
        catch (Exception ex)
        {
            resp = new { status = "2", message = ex.Message };
        }
        resp = Newtonsoft.Json.JsonConvert.SerializeObject(resp, Newtonsoft.Json.Formatting.Indented);
        return resp.ToString();
    }

    [WebMethod()]
    public static string grantRequest(int headId,string pinCode) {
        object resp = null;
        basegeneral obj = new basegeneral();
        try {
            DataTable dtRequestTransaction = new DataTable();
            adminModelClass adminModelObj = new adminModelClass();
            string username = HttpContext.Current.Session["username"].ToString();

            dtRequestTransaction = adminModelObj.getReprintTransaction(headId,pinCode.ToUpper());
            if (dtRequestTransaction.Rows.Count <= 0) {
                resp = new { status = "0", message = "Pin Code is incorrect!" };
            } else {
                adminModelObj.updateReprintStatus(headId,"GRANTED", ref obj);
                adminModelObj.updateReprintApprover(headId, username, ref obj);
                resp = new { status = "1", message = "Request has been Granted", transaction = dtRequestTransaction };
                obj.Commit();

            }
        } catch (Exception ex) {
            obj.Rollback();
            resp = new { status = "2", message = ex.Message };
        }
        resp = Newtonsoft.Json.JsonConvert.SerializeObject(resp, Newtonsoft.Json.Formatting.Indented);
        return resp.ToString();
    }

    [WebMethod()]
    public static string purgeTransaction(string headId, string barcode, int purge)
    {
        object resp = null;
        basegeneral obj = new basegeneral();
        try
        {

            remove_item(headId, barcode, purge, ref obj);
            resp = new { status = "1", message = "Transaction has been successfully purged." };
            obj.Commit();
        }
        catch (Exception ex)
        {
            obj.Rollback();
            resp = new { status = "2", message = ex.Message };
        }
        return Newtonsoft.Json.JsonConvert.SerializeObject(resp, Newtonsoft.Json.Formatting.Indented);
    }

    private static void remove_item(string headId, string barcode, int rewardApplication, ref basegeneral obj)
    {
        OracleParameter[] param = new OracleParameter[2];
        OracleParameter parameter = new OracleParameter();

        parameter = new OracleParameter();

        parameter.ParameterName = "I_HEADID";
        parameter.OracleDbType = OracleDbType.Int32;
        parameter.Value = headId;
        parameter.Direction = ParameterDirection.Input;
        param[0] = parameter;

        parameter = new OracleParameter();
        parameter.ParameterName = "I_BARCODE";
        parameter.OracleDbType = OracleDbType.Varchar2;
        parameter.Value = barcode;
        parameter.Direction = ParameterDirection.Input;
        param[1] = parameter;

        parameter = new OracleParameter();
        parameter.ParameterName = "I_REWARDAPPLICATION";
        parameter.OracleDbType = OracleDbType.Int16;
        parameter.Value = rewardApplication;
        parameter.Direction = ParameterDirection.Input;
        param[2] = parameter;

        obj.GetDataTable(connection.userSchema + ".KCC_PROMOLIST_DATALAYER_CUM.remove_item", CommandType.StoredProcedure, param);

    }

}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Oracle.ManagedDataAccess;
using Oracle.ManagedDataAccess.Client;
using System.Web.Services;
using promotions;


public partial class WebMethods_generalWebMethod : System.Web.UI.Page
{
    [WebMethod()]
    public static string[] getLocs(string prefixText, int count) {

        DataTable dtLocs = new DataTable();
        int page_index = 0;
        int page_size = 50;
        string username = HttpContext.Current.Session["username"].ToString();
        prefixText = prefixText.ToUpper();
        dtLocs =getLocation(prefixText,page_index,page_size,username.ToUpper());
        List<string> listLocs = new List<String>();
        foreach (DataRow dr in dtLocs.Rows)
        {
            listLocs.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(dr[1].ToString() , dr[0].ToString()));
        }
        return listLocs.ToArray();
    }

    [WebMethod()]
    public static string[] getDeps(string prefixText, int count)
    {
      
        DataTable dtDeps = new DataTable();
        int page_index = 0;
        int page_size = 50;
        string username = HttpContext.Current.Session["username"].ToString();
        prefixText = prefixText.ToUpper();
        dtDeps = getDepartment(prefixText, page_index, page_size, username.ToUpper());
        List<string> listDeps = new List<String>();
        foreach (DataRow dr in dtDeps.Rows)
        {
            listDeps.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(dr[1].ToString(), dr[0].ToString()));
        }
        return listDeps.ToArray();
    }

    [WebMethod()]
    public static string updateTransactionStatus(int headId) {
        object resp = null;
        basegeneral obj = new basegeneral();
        try {
            string currentStatus;
            DataTable dtTransaction = new DataTable();
            generalModelClass generalModelObj = new generalModelClass();
           
            dtTransaction = generalModelObj.get_transaction_detail(headId);
            if (dtTransaction.Rows.Count > 0) {
                currentStatus = dtTransaction.Rows[0]["status"].ToString();
                if (currentStatus == "WORKSHEET") {
                    generalModelObj.update_transaction_status(headId, "", ref obj);
                } else if (currentStatus == "SUBMITTED") {
                    generalModelObj.update_transaction_status(headId, "WORKSHEET", ref obj);
                } else { 
                    //PRINTED
                    generalModelObj.update_transaction_status(headId, "SUBMITTED", ref obj);
                }
                obj.Commit();

                dtTransaction = generalModelObj.get_transaction_detail(headId);
                string newStatus = dtTransaction.Rows[0]["status"].ToString();

                resp = new { status = "1", message = "Status has been Updated", transactionStatus = newStatus };
            } else {
                resp = new { status = "0", message = "No Details found." };
            }
        } catch (Exception ex) {
            obj.Rollback();
            resp = new { status = "2", message = ex.Message };
        }
        resp = Newtonsoft.Json.JsonConvert.SerializeObject(resp, Newtonsoft.Json.Formatting.Indented);
        return resp.ToString(); 
    }

    [WebMethod()]
    public static void keepSessionAlive() {
        string a = "KEEP SESSION ALIVE";
    }

    private static DataTable getDepartment(string prefixText, int pageIndex, int pageSize,string username)
    {
        OracleParameter[] param = new OracleParameter[5];
        OracleParameter parameter = new OracleParameter();

        parameter = new OracleParameter();

        parameter.ParameterName = "I_page_index";
        parameter.OracleDbType = OracleDbType.Varchar2;
        parameter.Value = pageIndex;
        parameter.Direction = ParameterDirection.Input;
        param[0] = parameter;

        parameter = new OracleParameter();
        parameter.ParameterName = "I_page_size";
        parameter.OracleDbType = OracleDbType.Varchar2;
        parameter.Value = pageSize;
        parameter.Direction = ParameterDirection.Input;
        param[1] = parameter;

        parameter = new OracleParameter();
        parameter.ParameterName = "I_name";
        parameter.OracleDbType = OracleDbType.Varchar2;
        parameter.Value = prefixText;
        parameter.Direction = ParameterDirection.Input;
        param[2] = parameter;

        parameter = new OracleParameter();
        parameter.ParameterName = "I_username";
        parameter.OracleDbType = OracleDbType.Varchar2;
        parameter.Value = username.ToString();
        parameter.Direction = ParameterDirection.Input;
        param[3] = parameter;

        parameter = new OracleParameter();
        parameter.ParameterName = "O_data";
        parameter.OracleDbType = OracleDbType.RefCursor;
        parameter.Direction = ParameterDirection.Output;
        param[4] = parameter;

        DataTable dtLocation = new DataTable();
        basegeneral obj = new basegeneral();
        dtLocation = obj.GetDataTable(connection.userSchema + ".KCC_PROMOLIST_DATALAYER_CUM.GETDEPS", CommandType.StoredProcedure, param);
        return dtLocation;
    }

    private static DataTable getLocation(string prefixText,int pageIndex,int pageSize,string username) {
        OracleParameter[] param = new OracleParameter[5];
        OracleParameter parameter = new OracleParameter();

        parameter = new OracleParameter();

        parameter.ParameterName = "I_page_index";
        parameter.OracleDbType = OracleDbType.Varchar2;
        parameter.Value = pageIndex;
        parameter.Direction = ParameterDirection.Input;
        param[0] = parameter;

        parameter = new OracleParameter();
        parameter.ParameterName = "I_page_size";
        parameter.OracleDbType = OracleDbType.Varchar2;
        parameter.Value = pageSize ;
        parameter.Direction = ParameterDirection.Input;
        param[1] = parameter;

         parameter = new OracleParameter();
        parameter.ParameterName = "I_name";
        parameter.OracleDbType = OracleDbType.Varchar2;
        parameter.Value = prefixText;
        parameter.Direction = ParameterDirection.Input;
        param[2] = parameter;

        parameter = new OracleParameter();
        parameter.ParameterName = "I_username";
        parameter.OracleDbType = OracleDbType.Varchar2;
        parameter.Value = username;
        parameter.Direction = ParameterDirection.Input;
        param[3] = parameter;

         parameter = new OracleParameter();
        parameter.ParameterName = "O_data";
        parameter.OracleDbType = OracleDbType.RefCursor;
        parameter.Direction = ParameterDirection.Output;
        param[4] = parameter;

        DataTable dtLocation = new DataTable();
        basegeneral obj = new basegeneral();
        dtLocation = obj.GetDataTable(connection.userSchema + ".KCC_PROMOLIST_DATALAYER_CUM.GETLOCS", CommandType.StoredProcedure, param);
        return dtLocation;
    }


}
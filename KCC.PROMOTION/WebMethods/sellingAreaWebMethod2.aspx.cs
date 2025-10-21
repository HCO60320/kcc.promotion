using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using promotions;
using System.Web.Script.Serialization;
using Newtonsoft;
using System.Data;
using Oracle.ManagedDataAccess;
using Oracle.ManagedDataAccess.Client;


public class sa_promo_dates {
    public string startDate;
    public string endDate;
}

public partial class WebMethods_sellingAreaWebMethod : System.Web.UI.Page
{
    [WebMethod()]
    public static string getAllTransactions() {
        object resp = null;
        try {
            DataTable dtTransaction = new DataTable();
            string username = HttpContext.Current.Session["username"].ToString();
            string roleName = HttpContext.Current.Session["roleName"].ToString();

            generalModelClass generalModelObj = new generalModelClass();
            dtTransaction = generalModelObj.get_all_transactions(username, roleName);

            resp = new { status = "1", transactions = dtTransaction };
        }catch(Exception ex){
            resp = new { status = "2", message = ex.Message };
        }
        resp = Newtonsoft.Json.JsonConvert.SerializeObject(resp, Newtonsoft.Json.Formatting.Indented);
        return resp.ToString();
    }

    [WebMethod()]
    public static string getReprintTransactions()
    {   object resp = null;
        try{
            DataTable dtReprintTransaction = new DataTable();
            string username = HttpContext.Current.Session["username"].ToString();
            dtReprintTransaction = get_reprint_transactions(username);
            resp = new { status = "1", transactions = dtReprintTransaction };
        }catch (Exception ex) {
            resp = new { status = "2", message = ex.Message };
        }
        resp = Newtonsoft.Json.JsonConvert.SerializeObject(resp, Newtonsoft.Json.Formatting.Indented);
        return resp.ToString();
    }

    [WebMethod()]
    public static string insertItem(string barcode, string headId, int location, int department, int promoType, int hasFreeList, int rewardApplication, sa_promo_dates[] promo_datesObj) { 
       basegeneral obj = new basegeneral();
       object resp = null;
       string username = HttpContext.Current.Session["username"].ToString();

       try {
           DataTable dtItem = new DataTable();
           dtItem = get_item_detail(barcode, location, department);
           if (dtItem.Rows.Count < 1) {
               //throws this warning if the the barcode 
               //has no equivalent orin and other details
               //or the item status is Inactive
               resp = new { status = "0", message = "Item is Invalid or Does not belong to the specified filter"};
           }else {
               Int32 orin = Convert.ToInt32(dtItem.Rows[0]["orin"]);
               DataTable dtPromoConflicts = new DataTable();
               DataTable dtMarkdownConflicts = new DataTable();
               bool conflictCntr = false;
               //DateTime _earliestDate = Convert.ToDateTime(earliestDate);
               //DateTime _latestDate = Convert.ToDateTime(latestDate);
               for (int y = 0; y < promo_datesObj.Count(); y++) {
                   DateTime startDate = Convert.ToDateTime(promo_datesObj[y].startDate);
                   DateTime endDate = Convert.ToDateTime(promo_datesObj[y].endDate);
                   dtPromoConflicts = new DataTable();
                   dtMarkdownConflicts = new DataTable();

                   dtPromoConflicts = get_promotion_conflict(headId, barcode, location, department, startDate.ToString("dd-MMM-yy"), endDate.ToString("dd-MMM-yy"));
                   dtMarkdownConflicts = get_markdown_conflict(barcode, location, department, startDate.ToString("dd-MMM-yy"), endDate.ToString("dd-MMM-yy"));

                   if (dtPromoConflicts.Rows.Count > 0) {
                       resp = new { status = "0", message = "PROMOTION CONFLICT <BR/>" + get_conflict_message(dtPromoConflicts) };
                       conflictCntr = true;
                       y = promo_datesObj.Count();
                   } else if (dtMarkdownConflicts.Rows.Count > 0) {
                       resp = new { status = "0", message = "MARKDOWN CONFLICT <BR/>" + get_conflict_message(dtMarkdownConflicts) };
                       conflictCntr = true;
                       y = promo_datesObj.Count();
                   }

               }
               
               if (conflictCntr == false){
                   if (headId == null || headId == "null" || headId == "") {
                       //this means that the item is the very first item of the transaction
                       //so,must create a head/transaction first
                       headId = create_transaction("", location, department,promoType,hasFreeList, ref obj);
                       if (promo_datesObj.Count() < 1) {
                           resp = new { status = "0", message = "Promotion must have initial Promo Dates" };
                       } else {
                           int x = 0;
                           generalModelClass generalModelObj = new generalModelClass();

                           for (x = 0; x < promo_datesObj.Count(); x++) {
                               DateTime startDate = Convert.ToDateTime(promo_datesObj[x].startDate);
                               DateTime endDate = Convert.ToDateTime(promo_datesObj[x].endDate);
                               generalModelObj.insertPromoDate(Convert.ToInt32(headId), startDate.ToString("dd-MMM-yy"), endDate.ToString("dd-MMM-yy"), ref obj);
                           }
                           insert_item(headId, orin, barcode, rewardApplication, username, ref obj);
                       }
                   } else {
                       insert_item(headId, orin, barcode, rewardApplication, username, ref obj);                          
                   }
                   //if it's all ok
                   obj.Commit();
                   dtItem = new DataTable();
                   dtItem = get_item_detail2(Convert.ToInt32(headId), barcode,rewardApplication);
                   resp = new { status = "1", headId = headId, item = dtItem };
                  
               }
           }
       }catch(Exception ex){
           obj.Rollback();
           resp = new { status = "2", message = ex.Message };
       }
        resp = Newtonsoft.Json.JsonConvert.SerializeObject(resp, Newtonsoft.Json.Formatting.Indented);
        return resp.ToString();
    }

    [WebMethod()]
    public static string getItemsTransaction(string headId,int rewardApplication){
        object resp = null;
        try { 
            DataTable dtItems = new DataTable();
            dtItems = get_items_transaction(headId,rewardApplication);
            if (dtItems.Rows.Count < 0){
                resp = new { status = "0", message = "This Transaction has no Items" };
            } else {
                resp = new { status = "1", item = dtItems };
            }
        }catch(Exception ex){
            resp = new { status = "2", message = ex.Message };
        }
        return Newtonsoft.Json.JsonConvert.SerializeObject(resp, Newtonsoft.Json.Formatting.None);
    }

    [WebMethod()]
    public static string getPromoTypes() {
        object resp = null;
        try {
            DataTable dtPromoType = new DataTable();
            dtPromoType = get_promo_types();
            if (dtPromoType.Rows.Count <= 0) {
                resp = new { status = "0", message = "There is no Promo Types Available,Please Contact The Administrator" };
            } else {
                resp = new { status = "1", types = dtPromoType };
            }
        } catch (Exception ex) {
            resp = new { status = "2", message = ex.Message };
        }
        return Newtonsoft.Json.JsonConvert.SerializeObject(resp, Newtonsoft.Json.Formatting.Indented);
    }

    [WebMethod()]
    public static string getPromoDate(string headId) {
        object resp = null;
        try {
            DataTable dtDates = new DataTable();
            dtDates = get_promo_date(headId);
            if (dtDates.Rows.Count <= 0) {
                resp = new { status = "0", message = "This Transaction has no Dates" };
            } else {
                resp = new { status = "1", dates = dtDates };
            }
        } catch (Exception ex) {
            resp = new { status = "2", message = ex.Message };
        }
        return Newtonsoft.Json.JsonConvert.SerializeObject(resp, Newtonsoft.Json.Formatting.Indented);
    }
    
    [WebMethod()]
    public static string getTransactionDetail(int headId) {
        object resp = null;
        try {
            DataTable dtTransaction = new DataTable();
            generalModelClass generalModelObj = new generalModelClass();

            dtTransaction = generalModelObj.get_transaction_detail(headId);
            if (dtTransaction.Rows.Count < 0) {
                resp = new { status = "0", message = "Transaction has no Detail" };
            }else {
                resp = new { status = "1", transactionDetail = dtTransaction };
            }
        }catch(Exception ex){
            resp = new { status = "2", message = ex.Message };
        }
        return Newtonsoft.Json.JsonConvert.SerializeObject(resp, Newtonsoft.Json.Formatting.Indented);
    }

    [WebMethod()]
    public static string removeItem(string headId,string barcode,int rewardApplication)
    {
        object resp = null;
        basegeneral obj = new basegeneral();
        try {

            remove_item(headId, barcode,rewardApplication, ref obj);
            resp = new { status = "1", message = "Item has successfully removed from list"};
            obj.Commit();
        }catch (Exception ex) {
            obj.Rollback();
            resp = new { status = "2", message = ex.Message };
        }
        return Newtonsoft.Json.JsonConvert.SerializeObject(resp, Newtonsoft.Json.Formatting.Indented);
    }

    [WebMethod()]
    public static string cancelTransaction(string headId) {
        object resp = null;
        basegeneral obj = new basegeneral();
        try {
            generalModelClass generalModelObj = new generalModelClass();
            DataTable dtTransaction  = new DataTable();

           dtTransaction = generalModelObj.get_transaction_detail(Convert.ToInt32(headId));
           if (dtTransaction.Rows.Count > 0) {
               if (dtTransaction.Rows[0]["status"].ToString() == "" || dtTransaction.Rows[0]["status"].ToString() == null) {
                   delete_head(headId, ref obj);
                   delete_items(headId, ref obj);
                   generalModelObj.deletePromoDates(Convert.ToInt32(headId), ref obj);

                   resp = new { status = "1", message = "Transaction has successfully Cancelled/Removed" };
                   obj.Commit();
               } else {
                   resp = new { status = "0", message = "Cannot cancel transaction that have already saved" };
               }
              
           } else {
               resp = new { status = "0", message = "Cant find Transaction Details" };
           }
        } catch (Exception ex) {
            obj.Rollback();
            resp = new { status = "2", message = ex.Message };
        }
        return Newtonsoft.Json.JsonConvert.SerializeObject(resp, Newtonsoft.Json.Formatting.Indented);
    }

    [WebMethod()]
    public static string generateTransactionNumber(int headId,string description)
    {
       description =  HttpUtility.UrlDecode(description);
        object resp = null;
        basegeneral obj = new basegeneral();
        try
        {
            string refNo;
            DataTable dtTransaction = new DataTable();
            generalModelClass generalModelObj = new generalModelClass();
            string username = HttpContext.Current.Session["username"].ToString();


            dtTransaction = generalModelObj.get_transaction_detail(headId);
            if (dtTransaction.Rows.Count < 1) {
                resp = new { status = "0", message = "There is no Detail for this Transaction" };
            } else {
                refNo = dtTransaction.Rows[0]["tran_id"].ToString();
                if (refNo == "") {
                    refNo = generate_transaction_number(headId, description, "WORKSHEET", ref obj);
                } else {
                    generalModelObj.update_transaction_status(headId, "WORKSHEET", ref obj);          
                }
                update_preparer(headId.ToString(), username, ref obj);
                resp = new { status = "1", tranId = refNo, message = "Transaction has been successfully saved" };
                obj.Commit();
            }
        }
        catch (Exception ex) {
            obj.Rollback();
            resp = new { status = "2", message = ex.Message };
        }
        return Newtonsoft.Json.JsonConvert.SerializeObject(resp, Newtonsoft.Json.Formatting.Indented);
    }

    [WebMethod()]
    public static string requestReprint(string headId,string message)
    {
        object resp = null;
        basegeneral obj = new basegeneral();
        try{
            string msgBody = HttpUtility.UrlDecode(message);
            string username = HttpContext.Current.Session["username"].ToString();
            string requestCode = generateRequestCode(headId);
            string pinCode = generatePinCode(headId);
            insert_reprint_request(headId, "PENDING", requestCode, pinCode, username, ref obj);
            send_IR(msgBody, requestCode, pinCode, username);
            obj.Commit();
            resp = new { status = "1",message = "Reprint Request has been sent"};
        }catch (Exception ex){
            obj.Rollback();
            resp = new { status = "2", message = ex.Message };
        }
        resp = Newtonsoft.Json.JsonConvert.SerializeObject(resp, Newtonsoft.Json.Formatting.Indented);
        return resp.ToString();
    }

    [WebMethod()]
    public static string updateAgeCode(string headId, string barcode,int rewardApplication,string ageCode) {
        object resp = null;
        basegeneral obj = new basegeneral();
        try {
            update_AgeCode(headId,barcode,rewardApplication,ageCode,ref obj);
            obj.Commit();
            resp = new { status = "1", message = "Age Code has been updated" };
        } catch (Exception ex) {
            obj.Rollback();
            resp = new { status = "2", message = ex.Message };
        }
        resp = Newtonsoft.Json.JsonConvert.SerializeObject(resp, Newtonsoft.Json.Formatting.Indented);
        return resp.ToString();
    }

    [WebMethod()]
    public static string updateQty(string headId, string barcode, int rewardApplication, int qty) {
        object resp = null;
        basegeneral obj = new basegeneral();
        try {
            update_qty(headId, barcode, rewardApplication, qty, ref obj);
            obj.Commit();
            resp = new { status = "1", message = "Quantity has been updated" };
        } catch (Exception ex) {
            obj.Rollback();
            resp = new { status = "2", message = ex.Message };
        }
        resp = Newtonsoft.Json.JsonConvert.SerializeObject(resp, Newtonsoft.Json.Formatting.Indented);
        return resp.ToString();
    }

    private static void send_IR(string message,string requestCode,string pinCode,string username) {
        System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
        System.Net.Mail.MailMessage msgObj = new System.Net.Mail.MailMessage();

        string body = message + "<br/> Request Code: " + requestCode + "<br/>Pin Code: " + pinCode + "<br/>From: " + username;
        string subject = "Promotion Incident Report";
        int port = 25;
        string host = "192.168.37.37";
        string sender = "KCC.MIS.ORACLE.TEAM@kccmalls.net";
        string userCredPass = "larrylloyd";
        client.Credentials = new System.Net.NetworkCredential(sender, userCredPass);
        client.Port = port;
        client.Host = host;
        msgObj.To.Clear();
        msgObj.CC.Clear();
        msgObj.Attachments.Clear();
        msgObj.AlternateViews.Clear();
        msgObj.From = new System.Net.Mail.MailAddress(sender);
        msgObj.To.Add("abdul.malik.m.abtahi@kccmalls.net");
        msgObj.CC.Add("mary.joyce.s.perales@kccmalls.net");
        msgObj.CC.Add("karl.cedrick.a.pelagio@kccmalls.net");
        msgObj.CC.Add("alpha.grace.m.talledo@kccmalls.net");

        msgObj.Subject = subject;
        msgObj.Body = body;
        msgObj.IsBodyHtml = true;
        System.Net.Mail.AlternateView alt = System.Net.Mail.AlternateView.CreateAlternateViewFromString(msgObj.Body, null, "text/html");
        msgObj.AlternateViews.Add(alt);
        client.Send(msgObj);
    }
    private static string generateRequestCode(string tranId) {
        string requestCode = null;
        requestCode = "R" + tranId + "C";
        return requestCode;
    }
    private static string generatePinCode(string tranId) {
        string pinCode = null;
        pinCode = "P" + tranId + "N";
        return pinCode;
    }
    private static string get_conflict_message(DataTable dtConflict) {

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        foreach (DataRow dr in dtConflict.Rows) {
            sb.Append("REF#: " + dr["tran_id"] + "|" + dr["head_id"] + ": " + dr["start_date"] + " -  " + dr["end_date"] + "<BR/>");
        }
        return sb.ToString();
    }

    private static void remove_item(string headId,string barcode,int rewardApplication,ref basegeneral obj) {
        OracleParameter[] param = new OracleParameter[3];
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
    private static void delete_items(string headId, ref basegeneral obj) {
        OracleParameter[] param = new OracleParameter[1];
        OracleParameter parameter = new OracleParameter();

        parameter = new OracleParameter();

        parameter.ParameterName = "I_HEADID";
        parameter.OracleDbType = OracleDbType.Int32;
        parameter.Value = headId;
        parameter.Direction = ParameterDirection.Input;
        param[0] = parameter;

        obj.GetDataTable(connection.userSchema + ".KCC_PROMOLIST_DATALAYER_CUM.delete_items", CommandType.StoredProcedure, param);

    }
    private static void delete_head(string headId, ref basegeneral obj) {
        OracleParameter[] param = new OracleParameter[1];
        OracleParameter parameter = new OracleParameter();

        parameter = new OracleParameter();

        parameter.ParameterName = "I_HEADID";
        parameter.OracleDbType = OracleDbType.Int32;
        parameter.Value = headId;
        parameter.Direction = ParameterDirection.Input;
        param[0] = parameter;

        obj.GetDataTable(connection.userSchema + ".KCC_PROMOLIST_DATALAYER_CUM.delete_head", CommandType.StoredProcedure, param);

    }
    private static void update_preparer(string headId,string username, ref basegeneral obj) {
        OracleParameter[] param = new OracleParameter[2];
        OracleParameter parameter = new OracleParameter();

        parameter = new OracleParameter();

        parameter.ParameterName = "I_HEADID";
        parameter.OracleDbType = OracleDbType.Int32;
        parameter.Value = headId;
        parameter.Direction = ParameterDirection.Input;
        param[0] = parameter;

        parameter = new OracleParameter();
        parameter.ParameterName = "I_USERNAME";
        parameter.OracleDbType = OracleDbType.Varchar2;
        parameter.Value = username;
        parameter.Direction = ParameterDirection.Input;
        param[1] = parameter;

        obj.GetDataTable(connection.userSchema + ".KCC_PROMOLIST_DATALAYER_CUM.UPDATE_PREPARER", CommandType.StoredProcedure, param);

    }
    private static string create_transaction(string status, int location, int department,int promoType,int hasFreeList, ref basegeneral obj)
    {
        string headId;

        OracleParameter[] param = new OracleParameter[6];
        OracleParameter parameter = new OracleParameter();

        parameter = new OracleParameter();

        parameter.ParameterName = "I_LOCATION";
        parameter.OracleDbType = OracleDbType.Int32;
        parameter.Value = location;
        parameter.Direction = ParameterDirection.Input;
        param[0] = parameter;

        parameter = new OracleParameter();
        parameter.ParameterName = "I_DEPARTMENT";
        parameter.OracleDbType = OracleDbType.Int32;
        parameter.Value = department;
        parameter.Direction = ParameterDirection.Input;
        param[1] = parameter;

        parameter = new OracleParameter();
        parameter.ParameterName = "I_STATUS";
        parameter.OracleDbType = OracleDbType.Varchar2;
        parameter.Value = status;
        parameter.Direction = ParameterDirection.Input;
        param[2] = parameter;

        parameter = new OracleParameter();
        parameter.ParameterName = "I_PROMOTYPE";
        parameter.OracleDbType = OracleDbType.Int32;
        parameter.Value = promoType;
        parameter.Direction = ParameterDirection.Input;
        param[3] = parameter;

        parameter = new OracleParameter();
        parameter.ParameterName = "I_HASFREELIST";
        parameter.OracleDbType = OracleDbType.Int32;
        parameter.Value = hasFreeList;
        parameter.Direction = ParameterDirection.Input;
        param[4] = parameter;


        parameter = new OracleParameter();
        parameter.ParameterName = "O_HEAD_ID";
        parameter.OracleDbType = OracleDbType.Int32;
        parameter.Direction = ParameterDirection.Output;
        param[5] = parameter;

        obj.executeNoCommit(connection.userSchema + ".KCC_PROMOLIST_DATALAYER_CUM.create_transaction",CommandType.StoredProcedure, param);
        headId = param[5].Value.ToString();
        return headId;
    }
    private static string updateTransaction(string headId) {
        string tranId = null;


        return tranId;
    }
    private static void insert_item(string headId, int orin, string barcode,int rewardApplication, string username, ref basegeneral obj)
    {
        OracleParameter[] param = new OracleParameter[5];
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
        parameter.ParameterName = "I_ORIN";
        parameter.OracleDbType = OracleDbType.Int32;
        parameter.Value = orin;
        parameter.Direction = ParameterDirection.Input;
        param[2] = parameter;

        parameter = new OracleParameter();
        parameter.ParameterName = "I_REWARDAPPLICATION";
        parameter.OracleDbType = OracleDbType.Int32;
        parameter.Value = rewardApplication;
        parameter.Direction = ParameterDirection.Input;
        param[3] = parameter;

        parameter = new OracleParameter();
        parameter.ParameterName = "I_USERNAME";
        parameter.OracleDbType = OracleDbType.Varchar2;
        parameter.Value = username;
        parameter.Direction = ParameterDirection.Input;
        param[4] = parameter;

        DataTable dtItem = new DataTable();
        obj.executeNoCommit(connection.userSchema + ".KCC_PROMOLIST_DATALAYER_CUM.insert_item", CommandType.StoredProcedure, param);
        headId = param[3].Value.ToString();
    }
    private static void update_AgeCode(string headId,string barcode, int rewardApplication, string ageCode, ref basegeneral obj) {
        OracleParameter[] param = new OracleParameter[4];
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
        parameter.OracleDbType = OracleDbType.Int32;
        parameter.Value = rewardApplication;
        parameter.Direction = ParameterDirection.Input;
        param[2] = parameter;

        parameter = new OracleParameter();
        parameter.ParameterName = "I_AGECODE";
        parameter.OracleDbType = OracleDbType.Varchar2;
        parameter.Value = ageCode;
        parameter.Direction = ParameterDirection.Input;
        param[3] = parameter;

        DataTable dtItem = new DataTable();
        obj.executeNoCommit(connection.userSchema + ".KCC_PROMOLIST_DATALAYER_CUM.update_agecode", CommandType.StoredProcedure, param);
    }
    private static void update_qty(string headId, string barcode, int rewardApplication, int qty, ref basegeneral obj) {
        OracleParameter[] param = new OracleParameter[4];
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
        parameter.OracleDbType = OracleDbType.Int32;
        parameter.Value = rewardApplication;
        parameter.Direction = ParameterDirection.Input;
        param[2] = parameter;

        parameter = new OracleParameter();
        parameter.ParameterName = "I_QTY";
        parameter.OracleDbType = OracleDbType.Int32;
        parameter.Value = qty;
        parameter.Direction = ParameterDirection.Input;
        param[3] = parameter;

        DataTable dtItem = new DataTable();
        obj.executeNoCommit(connection.userSchema + ".KCC_PROMOLIST_DATALAYER_CUM.update_qty", CommandType.StoredProcedure, param);
    }


    private static string generate_transaction_number(int headId,string description,string status,ref basegeneral obj) {
        OracleParameter[] param = new OracleParameter[4];
        OracleParameter parameter = new OracleParameter();
        string transactionNumber = null;

        parameter = new OracleParameter();
        parameter.ParameterName = "I_HEADID";
        parameter.OracleDbType = OracleDbType.Int32;
        parameter.Value = headId;
        parameter.Direction = ParameterDirection.Input;
        param[0] = parameter;

        parameter = new OracleParameter();
        parameter.ParameterName = "I_DESCRIPTION";
        parameter.OracleDbType = OracleDbType.Varchar2;
        parameter.Value = description;
        parameter.Direction = ParameterDirection.Input;
        param[1] = parameter;

        parameter = new OracleParameter();
        parameter.ParameterName = "I_STATUS";
        parameter.OracleDbType = OracleDbType.Varchar2;
        parameter.Value = status;
        parameter.Direction = ParameterDirection.Input;
        param[2] = parameter;

        parameter = new OracleParameter();
        parameter.ParameterName = "O_REFID";
        parameter.OracleDbType = OracleDbType.Int32;
        parameter.Direction = ParameterDirection.Output;
        param[3] = parameter;

        obj.executeNoCommit(connection.userSchema + ".KCC_PROMOLIST_DATALAYER_CUM.generate_transation_number", CommandType.StoredProcedure, param);
        transactionNumber = param[3].Value.ToString();
        return transactionNumber;
    }
    private static void insert_reprint_request(string headId, string status, string requestCode, string pinCode, string username, ref basegeneral obj) {
        OracleParameter[] param = new OracleParameter[5];
        OracleParameter parameter = new OracleParameter();

        parameter = new OracleParameter();

        parameter.ParameterName = "I_TRANID";
        parameter.OracleDbType = OracleDbType.Int32;
        parameter.Value = headId;
        parameter.Direction = ParameterDirection.Input;
        param[0] = parameter;

        parameter = new OracleParameter();
        parameter.ParameterName = "I_STATUS";
        parameter.OracleDbType = OracleDbType.Varchar2;
        parameter.Value = status;
        parameter.Direction = ParameterDirection.Input;
        param[1] = parameter;

        parameter = new OracleParameter();
        parameter.ParameterName = "I_REQUESTCODE";
        parameter.OracleDbType = OracleDbType.Varchar2;
        parameter.Value = requestCode;
        parameter.Direction = ParameterDirection.Input;
        param[2] = parameter;

        parameter = new OracleParameter();
        parameter.ParameterName = "I_PINCODE";
        parameter.OracleDbType = OracleDbType.Varchar2;
        parameter.Value = pinCode;
        parameter.Direction = ParameterDirection.Input;
        param[3] = parameter;

        parameter = new OracleParameter();
        parameter.ParameterName = "I_USERNAME";
        parameter.OracleDbType = OracleDbType.Varchar2;
        parameter.Value = username;
        parameter.Direction = ParameterDirection.Input;
        param[4] = parameter;

        obj.executeNoCommit(connection.userSchema + ".KCC_PROMOLIST_DATALAYER_CUM.insert_reprint_request", CommandType.StoredProcedure, param);
    }

    private static DataTable get_promo_types() {
        DataTable dtPromoTypes = new DataTable();
        OracleParameter[] param = new OracleParameter[1];
        OracleParameter parameter = new OracleParameter();

        parameter = new OracleParameter();
        parameter.ParameterName = "O_DATA";
        parameter.OracleDbType = OracleDbType.RefCursor;
        parameter.Direction = ParameterDirection.Output;
        param[0] = parameter;
        basegeneral obj = new basegeneral();
        dtPromoTypes= obj.GetDataTable(connection.userSchema + ".KCC_PROMOLIST_DATALAYER_CUM.get_promo_types", CommandType.StoredProcedure, param);
        return dtPromoTypes;
    }

    private static DataTable get_promotion_conflict(string headId,string barcode, int location, int department, string startDate, string endDate) {
        //start date is the earliest among the list of start dates(if multiple)
        //end date is the latest among the list of end dates (if multiple)

        OracleParameter[] param = new OracleParameter[7];
        OracleParameter parameter = new OracleParameter();

        parameter = new OracleParameter();
        parameter.ParameterName = "I_HEADID";
        parameter.OracleDbType = OracleDbType.Varchar2;
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
        parameter.ParameterName = "I_LOCATION";
        parameter.OracleDbType = OracleDbType.Int16;
        parameter.Value = location;
        parameter.Direction = ParameterDirection.Input;
        param[2] = parameter;

        parameter = new OracleParameter();
        parameter.ParameterName = "I_DEPARTMENT";
        parameter.OracleDbType = OracleDbType.Int16;
        parameter.Value = department;
        parameter.Direction = ParameterDirection.Input;
        param[3] = parameter;

        parameter = new OracleParameter();
        parameter.ParameterName = "I_STARTDATE";
        parameter.OracleDbType = OracleDbType.Varchar2;
        parameter.Value = startDate;
        parameter.Direction = ParameterDirection.Input;
        param[4] = parameter;

        parameter = new OracleParameter();
        parameter.ParameterName = "I_ENDDATE";
        parameter.OracleDbType = OracleDbType.Varchar2;
        parameter.Value = endDate;
        parameter.Direction = ParameterDirection.Input;
        param[5] = parameter;

        parameter = new OracleParameter();
        parameter = new OracleParameter();
        parameter.ParameterName = "O_DATA";
        parameter.OracleDbType = OracleDbType.RefCursor;
        parameter.Direction = ParameterDirection.Output;
        param[6] = parameter;

        DataTable dtPromoConflict = new DataTable();
        basegeneral obj = new basegeneral();

        dtPromoConflict = obj.GetDataTable(connection.userSchema + ".KCC_PROMOLIST_DATALAYER_CUM.get_promotion_conflict", CommandType.StoredProcedure, param);
        return dtPromoConflict;
    }
    private static DataTable get_markdown_conflict(string barcode, int location, int department, string startDate, string endDate) {
        //start date is the earliest among the list of start dates(if multiple)
        //end date is the latest among the list of end dates (if multiple)

        OracleParameter[] param = new OracleParameter[6];
        OracleParameter parameter = new OracleParameter();

        parameter = new OracleParameter();
        parameter.ParameterName = "I_BARCODE";
        parameter.OracleDbType = OracleDbType.Varchar2;
        parameter.Value = barcode;
        parameter.Direction = ParameterDirection.Input;
        param[0] = parameter;

        parameter = new OracleParameter();
        parameter.ParameterName = "I_LOCATION";
        parameter.OracleDbType = OracleDbType.Int16;
        parameter.Value = location;
        parameter.Direction = ParameterDirection.Input;
        param[1] = parameter;

        parameter = new OracleParameter();
        parameter.ParameterName = "I_DEPARTMENT";
        parameter.OracleDbType = OracleDbType.Int16;
        parameter.Value = department;
        parameter.Direction = ParameterDirection.Input;
        param[2] = parameter;

        parameter = new OracleParameter();
        parameter.ParameterName = "I_STARTDATE";
        parameter.OracleDbType = OracleDbType.Varchar2;
        parameter.Value = startDate;
        parameter.Direction = ParameterDirection.Input;
        param[3] = parameter;

        parameter = new OracleParameter();
        parameter.ParameterName = "I_ENDDATE";
        parameter.OracleDbType = OracleDbType.Varchar2;
        parameter.Value = endDate;
        parameter.Direction = ParameterDirection.Input;
        param[4] = parameter;

        parameter = new OracleParameter();
        parameter = new OracleParameter();
        parameter.ParameterName = "O_DATA";
        parameter.OracleDbType = OracleDbType.RefCursor;
        parameter.Direction = ParameterDirection.Output;
        param[5] = parameter;

        DataTable dtMarkdownConflict = new DataTable();
        basegeneral obj = new basegeneral();

        dtMarkdownConflict = obj.GetDataTable(connection.userSchema + ".KCC_PROMOLIST_DATALAYER_CUM.get_markdown_conflict", CommandType.StoredProcedure, param);
        return dtMarkdownConflict;
    }
    private static DataTable get_promo_date(string headId) {
        OracleParameter[] param = new OracleParameter[2];
        OracleParameter parameter = new OracleParameter();

        parameter = new OracleParameter();

        parameter.ParameterName = "I_HEADID";
        parameter.OracleDbType = OracleDbType.Int32;
        parameter.Value = headId;
        parameter.Direction = ParameterDirection.Input;
        param[0] = parameter;

        parameter = new OracleParameter();
        parameter.ParameterName = "O_DATA";
        parameter.OracleDbType = OracleDbType.RefCursor;
        parameter.Direction = ParameterDirection.Output;
        param[1] = parameter;

        DataTable dtDates = new DataTable();
        basegeneral obj = new basegeneral();

        dtDates = obj.GetDataTable(connection.userSchema + ".KCC_PROMOLIST_DATALAYER_CUM.get_promo_date", CommandType.StoredProcedure, param);
        return dtDates;
    }
   
    private static DataTable get_reprint_transactions(string username)
    {
        OracleParameter[] param = new OracleParameter[2];
        OracleParameter parameter = new OracleParameter();
        parameter = new OracleParameter();

        parameter.ParameterName = "I_USERNAME";
        parameter.OracleDbType = OracleDbType.Varchar2;
        parameter.Value = username;
        parameter.Direction = ParameterDirection.Input;
        param[0] = parameter;

        parameter = new OracleParameter();
        parameter.ParameterName = "O_DATA";
        parameter.OracleDbType = OracleDbType.RefCursor;
        parameter.Direction = ParameterDirection.Output;
        param[1] = parameter;

        DataTable dtTransactions = new DataTable();
        basegeneral obj = new basegeneral();

        dtTransactions = obj.GetDataTable(connection.userSchema + ".KCC_PROMOLIST_DATALAYER_CUM.get_reprint_transactions", CommandType.StoredProcedure, param);
        return dtTransactions;
    }
    private static DataTable get_items_transaction(string headId,int rewardApplication)
    {
        OracleParameter[] param = new OracleParameter[3];
        OracleParameter parameter = new OracleParameter();

        parameter = new OracleParameter();

        parameter.ParameterName = "I_HEADID";
        parameter.OracleDbType = OracleDbType.Int32;
        parameter.Value = headId;
        parameter.Direction = ParameterDirection.Input;
        param[0] = parameter;

        parameter = new OracleParameter();
        parameter.ParameterName = "I_REWARDAPPLICATION";
        parameter.OracleDbType = OracleDbType.Int32;
        parameter.Value = rewardApplication;
        parameter.Direction = ParameterDirection.Input;
        param[1] = parameter;

        parameter = new OracleParameter();
        parameter.ParameterName = "O_DATA";
        parameter.OracleDbType = OracleDbType.RefCursor;
        parameter.Direction = ParameterDirection.Output;
        param[2] = parameter;

        DataTable dtItems = new DataTable();
        basegeneral obj = new basegeneral();

        dtItems = obj.GetDataTable(connection.userSchema + ".KCC_PROMOLIST_DATALAYER_CUM.get_items_transaction", CommandType.StoredProcedure, param);
        return dtItems;
    }
    private static DataTable get_item_detail(string barcode, int location, int department)
    {
        

        OracleParameter[] param = new OracleParameter[4];
        OracleParameter parameter = new OracleParameter();

        parameter = new OracleParameter();

        parameter.ParameterName = "I_BARCODE";
        parameter.OracleDbType = OracleDbType.Varchar2;
        parameter.Value = barcode;
        parameter.Direction = ParameterDirection.Input;
        param[0] = parameter;

        parameter = new OracleParameter();
        parameter.ParameterName = "I_LOC";
        parameter.OracleDbType = OracleDbType.Int16;
        parameter.Value = location;
        parameter.Direction = ParameterDirection.Input;
        param[1] = parameter;

        parameter = new OracleParameter();
        parameter.ParameterName = "I_DEPT";
        parameter.OracleDbType = OracleDbType.Int16;
        parameter.Value = department;
        parameter.Direction = ParameterDirection.Input;
        param[2] = parameter;


        parameter = new OracleParameter();
        parameter.ParameterName = "O_DATA";
        parameter.OracleDbType = OracleDbType.RefCursor;
        parameter.Direction = ParameterDirection.Output;
        param[3] = parameter;

        DataTable dtItem = new DataTable();
        basegeneral obj = new basegeneral();
        dtItem = obj.GetDataTable(connection.userSchema + ".KCC_PROMOLIST_DATALAYER_CUM.get_item_detail", CommandType.StoredProcedure, param);
        return dtItem;
    }
    private static DataTable get_item_detail2(int headId, string barcode, int rewardApplication) {

        OracleParameter[] param = new OracleParameter[4];
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
        parameter.OracleDbType = OracleDbType.Int32;
        parameter.Value = rewardApplication;
        parameter.Direction = ParameterDirection.Input;
        param[2] = parameter;

        parameter = new OracleParameter();
        parameter.ParameterName = "O_DATA";
        parameter.OracleDbType = OracleDbType.RefCursor;
        parameter.Direction = ParameterDirection.Output;
        param[3] = parameter;

        DataTable dtItem = new DataTable();
        basegeneral obj = new basegeneral();
        dtItem = obj.GetDataTable(connection.userSchema + ".KCC_PROMOLIST_DATALAYER_CUM.get_item_detail2", CommandType.StoredProcedure, param);
        return dtItem;
    }
}
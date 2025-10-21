
namespace KCC.PROMOTION.WebMethods
{
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
    using KCC.PROMOTION.Model;

    public partial class generalWebMethod : System.Web.UI.Page
    {

        // CHECK CONFLICT IN MARKDOWN AND PROMOTION
        [WebMethod()]
        public static string checkOngoingTran(int tranId, int location, int department, string item)
        {
            object resp = null;
            try
            {
                DataTable dtPromoConflict = new DataTable();
                DataTable dtMarkdownConflicts = new DataTable();
                generalModelClass generalModelObj = new generalModelClass();

                dtPromoConflict = generalModelObj.get_item_promotion_conflict(item, tranId, location, department);
                dtMarkdownConflicts = generalModelObj.get_item_markdown_conflict(item, location, department);

                if (dtPromoConflict.Rows.Count > 0)
                {
                    resp = new { status = "1", message = "PROMOTION CONFLICT <BR/>" + get_conflict_message(dtPromoConflict), data = dtPromoConflict };
                }
                else if (dtMarkdownConflicts.Rows.Count > 0)
                {
                    resp = new { status = "0", message = "CANNOT ADD ITEM <BR/> ON GOING MARKDOWN CONFLICT <BR/>" + get_conflict_message(dtMarkdownConflicts), data = dtMarkdownConflicts };
                }
                else
                {
                    resp = new { status = "1" };
                }

            }
            catch (Exception ex)
            {
                resp = new { status = "2", message = ex.Message };
            }
            resp = Newtonsoft.Json.JsonConvert.SerializeObject(resp, Newtonsoft.Json.Formatting.Indented);
            return resp.ToString();
        }

        // CHECK EXISTING IN MARKDOWN AND PROMOTION
        [WebMethod()]
        public static string checkExistingTran(int tranId, int location, int department, string item)
        {
            object resp = null;
            try
            {
                DataTable dtPromoConflict = new DataTable();
                DataTable dtMarkdownConflicts = new DataTable();
                generalModelClass generalModelObj = new generalModelClass();

                dtPromoConflict = generalModelObj.get_item_promotion_existing(item, tranId, location, department);
                dtMarkdownConflicts = generalModelObj.get_item_markdown_existing(item, location, department);

                if (dtPromoConflict.Rows.Count > 0)
                {
                    resp = new { status = "1", message = "EXISTING PROMOTION <BR/>" + get_conflict_message(dtPromoConflict), data = dtPromoConflict };
                }
                else if (dtMarkdownConflicts.Rows.Count > 0)
                {
                    resp = new { status = "0", message = "EXISTING MARKDOWN <BR/>" + get_conflict_message(dtMarkdownConflicts), data = dtMarkdownConflicts };
                }
                else
                {
                    resp = new { status = "1" };
                }

            }
            catch (Exception ex)
            {
                resp = new { status = "2", message = ex.Message };
            }
            resp = Newtonsoft.Json.JsonConvert.SerializeObject(resp, Newtonsoft.Json.Formatting.Indented);
            return resp.ToString();
        }

        private static string get_conflict_message(DataTable dtConflict)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (DataRow dr in dtConflict.Rows)
            {
                sb.Append("REF#: " + dr["tran_id"] + "|" + dr["start_date"] + " -  " + dr["end_date"] + "<BR/>");
            }
            return sb.ToString();
        }

        [WebMethod()]
        public static string[] getLocs(string prefixText, int count)
        {

            DataTable dtLocs = new DataTable();
            int page_index = 0;
            int page_size = 50;
            string username = HttpContext.Current.Session["username"].ToString();
            prefixText = prefixText.ToUpper();
            dtLocs = getLocation(prefixText, page_index, page_size, username.ToUpper());
            List<string> listLocs = new List<String>();
            foreach (DataRow dr in dtLocs.Rows)
            {
                listLocs.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(dr[1].ToString(), dr[0].ToString()));
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
        public static string updateTransactionStatus(int headId)
        {
            object resp = null;
            basegeneral obj = new basegeneral();
            JsonResponse appManagerResponse = new JsonResponse();
            MerchAppManager.AutoEmailLogsClient autoEmail = new MerchAppManager.AutoEmailLogsClient();

            try
            {
                string currentStatus;
                DataTable dtTransaction = new DataTable();
                generalModelClass generalModelObj = new generalModelClass();

                MerchAppManager.ConnectionModel credential = new MerchAppManager.ConnectionModel();
                MerchAppManager.AutoEmailModel emailModel = new MerchAppManager.AutoEmailModel();

                
                
                    credential.Username = "merch_app_manager";
                    credential.Password = "m3rchappmanag3r";
                

                dtTransaction = generalModelObj.get_transaction_detail(headId);
                if (dtTransaction.Rows.Count > 0)
                {
                    emailModel.ApplicationId = Convert.ToInt32(HttpContext.Current.Session["applicationId"].ToString());
                    emailModel.TranNo = Convert.ToInt32(dtTransaction.Rows[0]["TRAN_ID"].ToString());

                    currentStatus = dtTransaction.Rows[0]["status"].ToString();
                    if (currentStatus == "WORKSHEET")
                    {
                        generalModelObj.update_transaction_status(headId, "", ref obj);
                        obj.Commit();

                        dtTransaction = generalModelObj.get_transaction_detail(headId);
                        string newStatus = dtTransaction.Rows[0]["status"].ToString();

                        resp = new { status = "1", message = "Status has been Updated", transactionStatus = newStatus };
                    }
                    else if (currentStatus == "SUBMITTED")
                    {
                        generalModelObj.update_transaction_status(headId, "WORKSHEET", ref obj);
                        obj.Commit();

                        dtTransaction = generalModelObj.get_transaction_detail(headId);
                        string newStatus = dtTransaction.Rows[0]["status"].ToString();

                        resp = new { status = "1", message = "Status has been Updated", transactionStatus = newStatus };
                    }
                    else
                    {
                        //PRINTED
                        appManagerResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonResponse>(autoEmail.GetAppTranRequest(credential, emailModel.ApplicationId, emailModel.TranNo));
                        if (appManagerResponse.Data.Rows.Count > 0)
                        {
                            if (appManagerResponse.Data.Rows[0]["STATUS"].ToString() != "SENT")
                            {
                                generalModelObj.update_transaction_status(headId, "SUBMITTED", ref obj);
                                obj.Commit();

                                dtTransaction = generalModelObj.get_transaction_detail(headId);
                                string newStatus = dtTransaction.Rows[0]["status"].ToString();

                                resp = new { status = "1", message = "Status has been Updated", transactionStatus = newStatus };
                            }
                            else
                            {
                                resp = new { status = "0", message = "This Transaction needs Approval" };
                            }
                        }
                        else
                        {
                            generalModelObj.update_transaction_status(headId, "SUBMITTED", ref obj);
                            obj.Commit();

                            dtTransaction = generalModelObj.get_transaction_detail(headId);
                            string newStatus = dtTransaction.Rows[0]["status"].ToString();

                            resp = new { status = "1", message = "Status has been Updated", transactionStatus = newStatus };
                        }
                    }
                }
                else
                {
                    resp = new { status = "0", message = "No Details found." };
                }
            }
            catch (Exception ex)
            {
                obj.Rollback();
                resp = new { status = "2", message = ex.Message };
            }
            resp = Newtonsoft.Json.JsonConvert.SerializeObject(resp, Newtonsoft.Json.Formatting.Indented);
            return resp.ToString();
        }

        [WebMethod()]
        public static void keepSessionAlive()
        {
            string a = "KEEP SESSION ALIVE";
        }

        private static DataTable getDepartment(string prefixText, int pageIndex, int pageSize, string username)
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

        private static DataTable getLocation(string prefixText, int pageIndex, int pageSize, string username)
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

        //SENDS EMAIL IF THERE IS NEGATIVE MARK UP
        //[WebMethod()]
        //public static string emailTrigger(int tranNo)
        //{
        //    object resp = null;
        //    try
        //    {
        //        generalModelClass generalModelObj = new generalModelClass();
        //       emailTriggerSend(tranNo);
        //       resp = new { status = "1" , message = "Email succesfully sent."};                

        //    }
        //    catch (Exception ex)
        //    {
        //        resp = new { status = "2", message = ex.Message };
        //    }
        //    resp = Newtonsoft.Json.JsonConvert.SerializeObject(resp, Newtonsoft.Json.Formatting.Indented);
        //    return resp.ToString();
        //}

        //private static void emailTriggerSend(int tranNo)
        //{
        //    //this function sends email to sir arvin if there is negative markup 


        //    OracleParameter[] param = new OracleParameter[1];
        //    OracleParameter parameter = new OracleParameter();

        //    parameter = new OracleParameter();

        //    parameter.ParameterName = "I_TRANID";
        //    parameter.OracleDbType = OracleDbType.Int32;
        //    parameter.Value = tranNo;
        //    parameter.Direction = ParameterDirection.Input;
        //    param[0] = parameter;


        //    basegeneral obj = new basegeneral();
        //    obj.GetDataTable(connection.userSchema + ".KCC_PROMOLIST_DATALAYER_CUM.NEG_MRKUP_EMAIL", CommandType.StoredProcedure, param);
        //}

    }
}
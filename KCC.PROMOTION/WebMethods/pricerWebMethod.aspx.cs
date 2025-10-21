
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
    using Newtonsoft;
    using promotions;
    using System.Web.Services;
    using KCC.PROMOTION.Model;

    //public class body
    //{
    //    public int body_id;
    //    public double srp;
    //    public string orin;
    //    public string barcode;
    //    public double? promo_srp = 0;
    //    public string discount;
    //    public string vpn;
    //    public double unit_cost;
    //    public int clr_type;
    //    public string clr_type_desc;
    //    public double? promo_mark_up = 0;
    //    public double? regular_mark_up = 0;
    //}

    public class prc_promo_dates
    {
        public string startDate;
        public string endDate;
    }

    public partial class pricerWebMethod : System.Web.UI.Page
    {
        [WebMethod()]
        public static string checkConflict(string headId, int location, int department, string startDate, string endDate)
        {
            object resp = null;
            try
            {
                DataTable dtPromoConflict = new DataTable();
                DataTable dtMarkdownConflicts = new DataTable();
                generalModelClass generalModelObj = new generalModelClass();
                DateTime startDate2 = Convert.ToDateTime(startDate);
                DateTime endDate2 = Convert.ToDateTime(endDate);

                dtPromoConflict = generalModelObj.get_promotion_conflict(headId, location, department, startDate2.ToString("dd-MMM-yy"), endDate2.ToString("dd-MMM-yy"));
                dtMarkdownConflicts = generalModelObj.get_markdown_conflict(headId, startDate2.ToString("dd-MMM-yy"), endDate2.ToString("dd-MMM-yy"));

                if (dtPromoConflict.Rows.Count > 0)
                {
                    resp = new { status = "0", message = "PROMOTION CONFLICT <BR/>" + get_conflict_message(dtPromoConflict) };
                }
                else if (dtMarkdownConflicts.Rows.Count > 0)
                {
                    resp = new { status = "0", message = "MARKDOWN CONFLICT <BR/>" + get_conflict_message(dtMarkdownConflicts) };
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

        [WebMethod()]
        public static string modifyDate(int headId, prc_promo_dates[] promoDatesObj)
        {
            object resp = null;
            basegeneral obj = new basegeneral();

            try
            {
                generalModelClass generalModelObj = new generalModelClass();
                generalModelObj.deletePromoDates(headId, ref obj);
                for (int x = 0; x < promoDatesObj.Count(); x++)
                {
                    DateTime startDate = Convert.ToDateTime(promoDatesObj[x].startDate);
                    DateTime endDate = Convert.ToDateTime(promoDatesObj[x].endDate);
                    generalModelObj.insertPromoDate(headId, startDate.ToString("dd-MMM-yy"), endDate.ToString("dd-MMM-yy"), ref obj);
                }

                obj.Commit();
                resp = new { status = "1", message = "Date has been modified" };
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
        public static string forceEndDate(int headId, string newEndDate)
        {
            object resp = null;
            basegeneral obj = new basegeneral();
            DateTime newEndDate2 = Convert.ToDateTime(newEndDate);
            try
            {
                generalModelClass generalModelObj = new generalModelClass();
                generalModelObj.forceEndTransaction(headId, newEndDate2.ToString("dd-MMM-yy"), ref obj);                

                obj.Commit();
                resp = new { status = "1", message = "Successfully forced end transaction." };
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
        public static string ReclassTran(int headId)
        {
            object resp = null;
            basegeneral obj = new basegeneral();
            pricerModelClass pricerModelObj = new pricerModelClass();

           // DateTime newEndDate2 = Convert.ToDateTime(headId);
            try
            {
                string username = HttpContext.Current.Session["username"].ToString();
                pricerModelObj.ReclassTran(headId, username);
                obj.Commit();
                resp = new { status = "1", message = "Successfully reclass transaction." };
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
        public static string approve(int headId)
        {
            object resp = null;
            basegeneral obj = new basegeneral();
            DataTable dtTransactionDetail = new DataTable();
            JsonResponse appManagerResponse = new JsonResponse();
            MerchAppManager.AutoEmailLogsClient autoEmail = new MerchAppManager.AutoEmailLogsClient();

            try
            {
                pricerModelClass pricerModelObj = new pricerModelClass();
                generalModelClass generalModelObj = new generalModelClass();
                string username = HttpContext.Current.Session["username"].ToString();

                MerchAppManager.ConnectionModel credential = new MerchAppManager.ConnectionModel();
                MerchAppManager.AutoEmailModel emailModel = new MerchAppManager.AutoEmailModel();

                
                    credential.Username = "merch_app_manager";
                    credential.Password = "m3rchappmanag3r";
                

                dtTransactionDetail = generalModelObj.get_transaction_detail(headId);
                if (dtTransactionDetail.Rows.Count < 1)
                {
                    resp = new { status = "0", message = "No Transaction Detail" };
                }
                else if (dtTransactionDetail.Rows[0]["status"].ToString() == "APPROVED")
                {
                    resp = new { status = "0", message = "cannot reapprove any transaction" };
                }
                else
                {
                    emailModel.ApplicationId = Convert.ToInt32(HttpContext.Current.Session["applicationId"].ToString());
                    emailModel.TranNo = Convert.ToInt32(dtTransactionDetail.Rows[0]["TRAN_ID"].ToString());

                    appManagerResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonResponse>(autoEmail.GetAppTranRequest(credential, emailModel.ApplicationId, emailModel.TranNo));
                    if (appManagerResponse.Data.Rows.Count > 0)
                    {
                        if (appManagerResponse.Data.Rows[0]["STATUS"].ToString() != "SENT")
                        {
                            if (appManagerResponse.Data.Rows[0]["STATUS"].ToString() == "APPROVED")
                            {
                                pricerModelObj.update_approver(headId, username, ref obj);
                                generalModelObj.update_transaction_status(headId, "APPROVED", ref obj);
                                string itemListNo = pricerModelObj.insert_itemlist(headId, username, 1, ref obj);
                                string itemListNoFreelist = null;
                                if (dtTransactionDetail.Rows[0]["has_freelist"].ToString() == "1")
                                {
                                    itemListNoFreelist = pricerModelObj.insert_itemlist(headId, username, 0, ref obj);
                                }
                                obj.Commit();
                                resp = new { status = "1", itemListNo = itemListNo, itemListNoFreelist = itemListNoFreelist, message = "Transaction has been Approved" };
                            }
                            else
                            {
                                resp = new { status = "0", message = "This Transaction has been Rejected" };
                            }
                        }
                        else
                        {
                            resp = new { status = "0", message = "This Transaction needs Approval" };
                        }
                    }
                    else
                    {
                        pricerModelObj.update_approver(headId, username, ref obj);
                        generalModelObj.update_transaction_status(headId, "APPROVED", ref obj);
                        string itemListNo = pricerModelObj.insert_itemlist(headId, username, 1, ref obj);
                        string itemListNoFreelist = null;
                        if (dtTransactionDetail.Rows[0]["has_freelist"].ToString() == "1")
                        {
                            itemListNoFreelist = pricerModelObj.insert_itemlist(headId, username, 0, ref obj);
                        }
                        obj.Commit();
                        resp = new { status = "1", itemListNo = itemListNo, itemListNoFreelist = itemListNoFreelist, message = "Transaction has been Approved" };
                    }
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
        public static string getAllTransactions()
        {
            object resp = null;
            try
            {
                DataTable dtTransaction = new DataTable();
                pricerModelClass pricerModelObj = new pricerModelClass();

                string username = HttpContext.Current.Session["username"].ToString();
                string rolename = HttpContext.Current.Session["rolename"].ToString();

                dtTransaction = pricerModelObj.get_all_transactions(username, rolename);
                resp = new { status = "1", transactions = dtTransaction };
            }
            catch (Exception ex)
            {
                resp = new { status = "2", message = ex.Message };
            }
            resp = Newtonsoft.Json.JsonConvert.SerializeObject(resp, Newtonsoft.Json.Formatting.Indented);
            return resp.ToString();
        }

        [WebMethod()]
        public static string getTransactionDetail(string headId)
        {
            object resp = null;
            try
            {
                DataTable dtTransaction = new DataTable();
                generalModelClass generalModelObj = new generalModelClass();

                dtTransaction = generalModelObj.get_transaction_detail(Convert.ToInt32(headId));

                if (dtTransaction.Rows.Count < 0)
                {
                    resp = new { status = "0", message = "Transaction has no Detail" };
                }
                else
                {
                    resp = new { status = "1", transactionDetail = dtTransaction };
                }
            }
            catch (Exception ex)
            {
                resp = new { status = "2", message = ex.Message };
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(resp, Newtonsoft.Json.Formatting.Indented);
        }

        [WebMethod()]
        public static string GetReClassItemsWDept(string headId)
        {
            object resp = null;
            try
            {
                DataTable dt = new DataTable();
                pricerModelClass pricerModelObj = new pricerModelClass();

                dt = pricerModelObj.GetReClassItemsWDept(Convert.ToInt32(headId));

                if (dt.Rows.Count < 0)
                {
                    resp = new { status = "0", message = "No Record Found." };
                }
                else
                {
                    resp = new { status = "1", databody = dt };
                }
            }
            catch (Exception ex)
            {
                resp = new { status = "2", message = ex.Message };
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(resp, Newtonsoft.Json.Formatting.Indented);
        }

        [WebMethod()]
        public static string getItemsTransaction(string headId)
        {
            object resp = null;
            try
            {
                DataTable dtItems = new DataTable();
                pricerModelClass pricerModelObj = new pricerModelClass();
                dtItems = pricerModelObj.get_prc_items_transaction(headId);
                if (dtItems.Rows.Count < 0)
                {
                    resp = new { status = "0", message = "This Transaction has no Items" };
                }
                else
                {
                    resp = new { status = "1", item = dtItems };
                }
            }
            catch (Exception ex)
            {
                resp = new { status = "2", message = ex.Message };
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(resp, Newtonsoft.Json.Formatting.Indented);

        }

        [WebMethod()]
        public static string getItemsTransaction(string headId, int rewardApplication)
        {
            object resp = null;
            try
            {
                DataTable dtItems = new DataTable();
                buyerModelClass buyerModelObj = new buyerModelClass();
                dtItems = buyerModelObj.get_byr_items_transaction(headId, rewardApplication);
                if (dtItems.Rows.Count < 0)
                {
                    resp = new { status = "0", message = "This Transaction has no Items" };
                }
                else
                {
                    resp = new { status = "1", item = dtItems };
                }
            }
            catch (Exception ex)
            {
                resp = new { status = "2", message = ex.Message };
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(resp, Newtonsoft.Json.Formatting.Indented);
        }

        [WebMethod()]
        public static string checkReclass(string headId)
        {
            object resp = null;
            try
            {
                DataTable dtItems = new DataTable();
                pricerModelClass pricerModelObj = new pricerModelClass();

                string _reclass_ind = "";

                dtItems = pricerModelObj.checkReclass(headId);
                if (dtItems.Rows.Count < 0)
                {
                    resp = new { status = "0", message = "This Transaction has no Items" };
                }
                else
                {
                    _reclass_ind = dtItems.Rows[0][0].ToString();
                    resp = new { status = "1", item = _reclass_ind };
                }
            }
            catch (Exception ex)
            {
                resp = new { status = "2", message = ex.Message };
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(resp, Newtonsoft.Json.Formatting.Indented);
        }

        [WebMethod()]
        public static string checkAdminAccount(string username, string password)
        {
            object resp = null;
            basegeneral obj = new basegeneral();

            try
            {
                userModelClass userModelObj = new userModelClass();
                DataTable dtuser = new DataTable();
                DataTable dtUserRole = new DataTable();
                dtuser = userModelObj.getUser(username, password);
                if (dtuser.Rows.Count <= 0)
                {
                    resp = new { status = "0", message = "Username or Password is incorrect" };
                }
                else
                {
                    dtUserRole = userModelObj.get_user_role(Convert.ToInt32(HttpContext.Current.Session["applicationId"]), Convert.ToInt32(dtuser.Rows[0]["USER_ID"]));
                    string userRole = dtUserRole.Rows[0]["ROLE_NAME"].ToString();
                    if (userRole == "KCC_PM_RM")
                    {
                        resp = new { status = "1", message = "Access Granted" };
                    }
                    else
                    {
                        resp = new { status = "0", message = "only User with administrative account can Approve modification" };
                    }
                }
            }
            catch (Exception ex)
            {
                obj.Rollback();
                resp = new { status = "2", message = ex.Message };
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(resp, Newtonsoft.Json.Formatting.Indented);
            //return resp.ToString();
        }

        private static string get_conflict_message(DataTable dtConflict)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (DataRow dr in dtConflict.Rows)
            {
                sb.Append("REF#: " + dr["tran_id"] + " |  " + dr["start_date"] + " -  " + dr["end_date"] + "<BR/>");
            }
            return sb.ToString();
        }

        [WebMethod()]
        public static string getPromoDate(string headId)
        {
            object resp = null;
            try
            {
                DataTable dtDates = new DataTable();
                dtDates = get_promo_date(headId);
                if (dtDates.Rows.Count <= 0)
                {
                    resp = new { status = "0", message = "This Transaction has No Dates" };
                }
                else
                {
                    resp = new { status = "1", dates = dtDates };
                }
            }
            catch (Exception ex)
            {
                resp = new { status = "2", message = ex.Message };
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(resp, Newtonsoft.Json.Formatting.Indented);
        }
        //Add New Function
        [WebMethod()]
        //public static string CheckPromoCanReclass()
        //{
        //    object resp = null;
        //    try
        //    {
        //        DataTable dt = new DataTable();
        //        pricerModelClass pricerModelObj = new pricerModelClass();
        //        string username = HttpContext.Current.Session["username"].ToString();

        //        dt = pricerModelObj.CheckPromoCanReclass(username);
        //        if (dt.Rows.Count < 0)
        //        {
        //            resp = new { status = "0", message = "No Record Found!"};
        //        }
        //        else
        //        {
        //            resp = new { status = "1", databody = dt };
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        resp = new { status = "2", message = ex.Message };
        //    }
        //    return Newtonsoft.Json.JsonConvert.SerializeObject(resp, Newtonsoft.Json.Formatting.Indented);
        //}

        public static string CheckPromoCanReclass()
        {
            object resp = null;
            try
            {
                DataTable dt = new DataTable();
                pricerModelClass pricerModelObj = new pricerModelClass();

                string username = HttpContext.Current.Session["username"].ToString();

                dt = pricerModelObj.CheckPromoCanReclass(username);
                if (dt.Rows.Count > 0)
                {
                    resp = new { status = "1", message = "Can reclass!" };
                }
                else
                {
                    resp = new { status = "0", message = "No Record Found!" };
                }
            }
            catch (Exception ex)
            {
                resp = new { status = "2", message = ex.Message };
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(resp, Newtonsoft.Json.Formatting.Indented);
        }


        [WebMethod()]
        public static string RemoveReclassItems(int headId)
        {
            object resp = null;
            basegeneral obj = new basegeneral();
            pricerModelClass pricerModelObj = new pricerModelClass();
            try
            {
                string username = HttpContext.Current.Session["username"].ToString();
                pricerModelObj.RemoveReclassItems(headId, username);
                obj.Commit();
                resp = new { status = "1", message = "Successfully remove reclass item/s." };
            }
            catch (Exception ex)
            {
                obj.Rollback();
                resp = new { status = "2", message = ex.Message };
            }

            resp = Newtonsoft.Json.JsonConvert.SerializeObject(resp, Newtonsoft.Json.Formatting.Indented);
            return resp.ToString();
        }
        //End
        private static void update_promo_date(int headId, string startDate, string endDate, string status, ref basegeneral obj)
        {

            OracleParameter[] param = new OracleParameter[4];
            OracleParameter parameter = new OracleParameter();

            parameter = new OracleParameter();

            parameter.ParameterName = "I_HEADID";
            parameter.OracleDbType = OracleDbType.Int32;
            parameter.Value = headId;
            parameter.Direction = ParameterDirection.Input;
            param[0] = parameter;

            parameter = new OracleParameter();
            parameter.ParameterName = "I_STARTDATE";
            parameter.OracleDbType = OracleDbType.Varchar2;
            parameter.Value = startDate;
            parameter.Direction = ParameterDirection.Input;
            param[1] = parameter;

            parameter = new OracleParameter();
            parameter.ParameterName = "I_ENDDATE";
            parameter.OracleDbType = OracleDbType.Varchar2;
            parameter.Value = endDate;
            parameter.Direction = ParameterDirection.Input;
            param[2] = parameter;

            parameter = new OracleParameter();
            parameter.ParameterName = "I_STATUS";
            parameter.OracleDbType = OracleDbType.Varchar2;
            parameter.Value = status;
            parameter.Direction = ParameterDirection.Input;
            param[3] = parameter;
            obj.executeNoCommit(connection.userSchema + ".KCC_PROMOLIST_DATALAYER_CUM.UPDATE_PROMO_DATE", CommandType.StoredProcedure, param);
        }

        private static void update_item_discount(int bodyid, double discount, double promoSrp, double promoMarkUp, double regularMarkUp, int type, string username, ref basegeneral obj)
        {
            OracleParameter[] param = new OracleParameter[7];
            OracleParameter parameter = new OracleParameter();

            parameter = new OracleParameter();

            parameter.ParameterName = "I_BODYID";
            parameter.OracleDbType = OracleDbType.Int32;
            parameter.Value = bodyid;
            parameter.Direction = ParameterDirection.Input;
            param[0] = parameter;

            parameter = new OracleParameter();
            parameter.ParameterName = "I_DISCOUNT";
            parameter.OracleDbType = OracleDbType.Double;
            parameter.Value = discount;
            parameter.Direction = ParameterDirection.Input;
            param[1] = parameter;

            parameter = new OracleParameter();
            parameter.ParameterName = "I_SRP";
            parameter.OracleDbType = OracleDbType.Double;
            parameter.Value = promoSrp;
            parameter.Direction = ParameterDirection.Input;
            param[2] = parameter;

            parameter = new OracleParameter();
            parameter.ParameterName = "I_CLRTYPE";
            parameter.OracleDbType = OracleDbType.Int32;
            parameter.Value = type;
            parameter.Direction = ParameterDirection.Input;
            param[3] = parameter;

            parameter = new OracleParameter();
            parameter.ParameterName = "I_PROMOMARKUP";
            parameter.OracleDbType = OracleDbType.Double;
            parameter.Value = promoMarkUp;
            parameter.Direction = ParameterDirection.Input;
            param[4] = parameter;

            parameter = new OracleParameter();
            parameter.ParameterName = "I_REGULARMARKUP";
            parameter.OracleDbType = OracleDbType.Double;
            parameter.Value = regularMarkUp;
            parameter.Direction = ParameterDirection.Input;
            param[5] = parameter;

            parameter = new OracleParameter();
            parameter.ParameterName = "I_USERNAME";
            parameter.OracleDbType = OracleDbType.Varchar2;
            parameter.Value = username;
            parameter.Direction = ParameterDirection.Input;
            param[6] = parameter;

            obj.executeNoCommit(connection.userSchema + ".KCC_PROMOLIST_DATALAYER_CUM.update_item_discount", CommandType.StoredProcedure, param);

        }

        private static DataTable get_promo_date(string headId)
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
            parameter.ParameterName = "O_DATA";
            parameter.OracleDbType = OracleDbType.RefCursor;
            parameter.Direction = ParameterDirection.Output;
            param[1] = parameter;

            DataTable dtDates = new DataTable();
            basegeneral obj = new basegeneral();

            dtDates = obj.GetDataTable(connection.userSchema + ".KCC_PROMOLIST_DATALAYER_CUM.get_promo_date", CommandType.StoredProcedure, param);
            return dtDates;
        }

        [WebMethod()]
        public static string getfilterTransactions(int searchType, string searchValue)
        {
            object resp = null;
            try
            {
                DataTable dtTransaction = new DataTable();
                pricerModelClass pricerModelObj = new pricerModelClass();

                string username = HttpContext.Current.Session["username"].ToString();
                string rolename = HttpContext.Current.Session["rolename"].ToString();

                dtTransaction = pricerModelObj.get_filter_transactions(username, rolename, searchType, searchValue);
                if (dtTransaction.Rows.Count <= 0)
                {
                    resp = new { status = 0 };
                }
                else
                {
                    resp = new { status = 1, transactions = dtTransaction };
                }

            }
            catch (Exception ex)
            {
                resp = new { status = 2, message = ex.Message };
            }
            resp = Newtonsoft.Json.JsonConvert.SerializeObject(resp, Newtonsoft.Json.Formatting.Indented);
            return resp.ToString();
        }

        [WebMethod()]
        public static string RemoveDate(int headId)
        {
            object resp = null;
            try
            {
                pricerModelClass pricerModelObj = new pricerModelClass();
                 pricerModelObj.RemoveDate(headId);

                resp = new { status = "1", message = "Successfully Remove Date" };



            }
            catch (Exception ex)
            {
                resp = new { status = "2", message = ex.Message };
            }
            resp = Newtonsoft.Json.JsonConvert.SerializeObject(resp, Newtonsoft.Json.Formatting.Indented);
            return resp.ToString();
        }
    }
}
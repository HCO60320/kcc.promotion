
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

    public class body
    {
        public int body_id;
        public double srp;
        public string orin;
        public string barcode;
        public double? promo_srp = 0;
        public string discount;
        public string vpn;
        public double unit_cost;
        public int clr_type;
        public string clr_type_desc;
        public double? promo_mark_up = 0;
        public double? regular_mark_up = 0;
    }

    public class byr_promo_dates
    {
        public string startDate;
        public string endDate;
    }

    public partial class buyerWebMethod : System.Web.UI.Page
    {
        [WebMethod()]
        public static string updateMultipleItemDiscount(body[] bodyObj)
        {

            object resp = null;
            basegeneral obj = new basegeneral();
            try
            {
                string username = HttpContext.Current.Session["username"].ToString();
                double promoMarkUp;
                //double regularMarkUp;
                double promoSRP;
                int x = 0;
                for (x = 0; x < bodyObj.Count(); x++)
                {
                    promoSRP = calculate_promo_srp(bodyObj[x].srp, Convert.ToDouble(bodyObj[x].discount), bodyObj[x].clr_type);
                    promoMarkUp = calculate_promo_markup(promoSRP, bodyObj[x].unit_cost);
                    // regularMarkUp = calculate_regular_markup(bodyObj[x].srp, bodyObj[x].unit_cost);
                    update_item_discount(bodyObj[x].body_id, Convert.ToDouble(bodyObj[x].discount), promoSRP, promoMarkUp, bodyObj[x].clr_type, username, ref obj);
                }
                obj.Commit();
                resp = new { status = "1", message = "Items have been updated" };
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
        public static string updateItemDiscount(body bodyObj)
        {
            object resp = null;
            basegeneral obj = new basegeneral();
            try
            {
                string username = HttpContext.Current.Session["username"].ToString();
                double promoMarkUp;
                //double regularMarkUp;
                double promoSRP;

                promoSRP = calculate_promo_srp(bodyObj.srp, Convert.ToDouble(bodyObj.discount), bodyObj.clr_type);
                promoMarkUp = calculate_promo_markup(promoSRP, bodyObj.unit_cost);
                //regularMarkUp = calculate_regular_markup(bodyObj.srp, bodyObj.unit_cost); 

                bodyObj.promo_mark_up = promoMarkUp;
                //bodyObj.regular_mark_up =regularMarkUp;
                bodyObj.promo_srp = promoSRP;

                update_item_discount(bodyObj.body_id, Convert.ToDouble(bodyObj.discount), promoSRP, promoMarkUp, bodyObj.clr_type, username, ref obj);

                //get the equivalent description of clr_type_id
                generalModelClass gmcObj = new generalModelClass();
                bodyObj.clr_type_desc = gmcObj.get_clr_type(bodyObj.clr_type);

                obj.Commit();
                resp = new { status = "1", item = bodyObj , message = "Item Price successfully updated!"};
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
        public static string getPromotionConflict(string headId, int location, int department, string startDate, string endDate)
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
        public static string submit(int headId, string description, byr_promo_dates[] promo_datesObj)
        {
            object resp = null;
            basegeneral obj = new basegeneral();
            generalModelClass generalModelObj = new generalModelClass();

            try
            {
                string username = HttpContext.Current.Session["username"].ToString();

                update_submitter(headId, username, description, ref obj);
                generalModelObj.update_transaction_status(headId, "SUBMITTED", ref obj);
                //generalModelObj.deletePromoDates(headId, ref obj);

                //for (int x = 0; x < promo_datesObj.Count(); x++)
                //{
                //    DateTime startDate = Convert.ToDateTime(promo_datesObj[x].startDate);
                //    DateTime endDate = Convert.ToDateTime(promo_datesObj[x].endDate);
                //    generalModelObj.insertPromoDate(headId, startDate.ToString("dd-MMM-yy"), endDate.ToString("dd-MMM-yy"), ref obj);
                //}

                obj.Commit();
                resp = new { status = "1", message = "Transaction has been Submitted" };
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
                // buyerModelClass buyerModelObj = new buyerModelClass();
                generalModelClass generalModelObj = new generalModelClass();

                string username = HttpContext.Current.Session["username"].ToString();
                string roleName = HttpContext.Current.Session["roleName"].ToString();
                dtTransaction = generalModelObj.get_all_transactions(username, roleName);
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
        public static string getTransactionDetail(int headId)
        {
            object resp = null;
            try
            {
                DataTable dtTransaction = new DataTable();
                generalModelClass generalModelObj = new generalModelClass();

                dtTransaction = generalModelObj.get_transaction_detail(headId);
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
            return Newtonsoft.Json.JsonConvert.SerializeObject(resp, Newtonsoft.Json.Formatting.None);
        }

        private static string get_conflict_message(DataTable dtConflict)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (DataRow dr in dtConflict.Rows)
            {
                sb.Append("REF#: " + dr["tran_id"] + "|" + dr["head_id"] + ": " + dr["start_date"] + " -  " + dr["end_date"] + "<BR/>");
            }
            return sb.ToString();
        }

        [WebMethod()]
        public static string removeItem(body[] bodyObj, int headId, int rewardApplication)
        {

            object resp = null;
            basegeneral obj = new basegeneral();
            generalModelClass generalModelObj = new generalModelClass();
            try
            {
                string UserId = HttpContext.Current.Session["userid"].ToString();
                int x = 0;
                for (x = 0; x < bodyObj.Count(); x++)
                {
                    remove_item(headId, bodyObj[x].barcode, rewardApplication, ref obj);
                    if (rewardApplication == 0)
                    {
                        generalModelObj.Create_Audit_Trail(headId, "WORKSHEET", UserId, "REMOVE IN FREELIST", bodyObj[x].orin, bodyObj[x].barcode, "", "", ref obj);
                    }
                    else
                    {
                        generalModelObj.Create_Audit_Trail(headId, "WORKSHEET", UserId, "REMOVE IN BUYLIST", bodyObj[x].orin, bodyObj[x].barcode, "", "", ref obj);
                    }

                }
                obj.Commit();
                resp = new { status = "1", message = "Items have been removed." };
            }
            catch (Exception ex)
            {
                obj.Rollback();
                resp = new { status = "2", message = ex.Message };
            }
            resp = Newtonsoft.Json.JsonConvert.SerializeObject(resp, Newtonsoft.Json.Formatting.Indented);
            return resp.ToString();
        }


        private static void update_submitter(int headId, string username, string description, ref basegeneral obj)
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
            parameter.ParameterName = "I_USERNAME";
            parameter.OracleDbType = OracleDbType.Varchar2;
            parameter.Value = username;
            parameter.Direction = ParameterDirection.Input;
            param[1] = parameter;

            parameter = new OracleParameter();
            parameter.ParameterName = "I_DESCRIPTION";
            parameter.OracleDbType = OracleDbType.Varchar2;
            parameter.Value = description;
            parameter.Direction = ParameterDirection.Input;
            param[2] = parameter;
            obj.executeNoCommit(connection.userSchema + ".KCC_PROMOLIST_DATALAYER_CUM.UPDATE_SUBMITTER", CommandType.StoredProcedure, param);
        }

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

        private static double calculate_promo_srp(double srp, double discount, int clr_type)
        {
            double _promoSRP;
            if (clr_type == 1)
            {
                _promoSRP = discount;
            }
            else if (clr_type == 2)
            {
                _promoSRP = Math.Round(srp - discount);
            }
            else if (clr_type == 3)
            {
                _promoSRP = Math.Round(srp - (srp * (discount / 100)), 2);
            }
            else
            {
                _promoSRP = 0;
            }
            return _promoSRP;
        }

        private static double calculate_regular_markup(double srp, double unitCost)
        {
            double _regularMarkUp;
            _regularMarkUp = Math.Round(((srp / unitCost) - 1) * 100, 2);
            return _regularMarkUp;
        }

        private static double calculate_promo_markup(double promoSRP, double unitCost)
        {
            double _promoMarkup;
            _promoMarkup = Math.Round(((promoSRP / unitCost) - 1) * 100, 2);
            return _promoMarkup;
        }

        private static void update_item_discount(int bodyid, double discount, double promoSrp, double promoMarkUp, int type, string username, ref basegeneral obj)
        {
            OracleParameter[] param = new OracleParameter[6];
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
            parameter.ParameterName = "I_USERNAME";
            parameter.OracleDbType = OracleDbType.Varchar2;
            parameter.Value = username;
            parameter.Direction = ParameterDirection.Input;
            param[5] = parameter;

            obj.executeNoCommit(connection.userSchema + ".KCC_PROMOLIST_DATALAYER_CUM.update_item_discount", CommandType.StoredProcedure, param);

        }

        private static void remove_item(int headId, string barcode, int rewardApplication, ref basegeneral obj)
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
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using promotions;
using Oracle.ManagedDataAccess;
using Oracle.ManagedDataAccess.Client;


/// <summary>
/// Summary description for generalModelClass
/// </summary>
public class generalModelClass
{
    public generalModelClass()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public string getSysdate()
    {
        string sql = "select sysdate from dual";
        basegeneral obj = new basegeneral();
        string sysDate = obj.GetValue(sql, CommandType.Text);
        return sysDate;
    }

    public DataTable get_transaction_detail(int headId)
    {
        //this function was used by selling Area and buyer


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

        DataTable dtTransaction = new DataTable();
        basegeneral obj = new basegeneral();

        dtTransaction = obj.GetDataTable(connection.userSchema + ".KCC_PROMOLIST_DATALAYER_CUM.get_transaction_detail", CommandType.StoredProcedure, param);
        return dtTransaction;
    }

    public DataTable get_all_transactions(string username, string roleName)
    {
        OracleParameter[] param = new OracleParameter[3];
        OracleParameter parameter = new OracleParameter();
        parameter = new OracleParameter();

        parameter.ParameterName = "I_USERNAME";
        parameter.OracleDbType = OracleDbType.Varchar2;
        parameter.Value = username;
        parameter.Direction = ParameterDirection.Input;
        param[0] = parameter;

        parameter = new OracleParameter();
        parameter.ParameterName = "I_ROLENAME";
        parameter.OracleDbType = OracleDbType.Varchar2;
        parameter.Value = roleName;
        parameter.Direction = ParameterDirection.Input;
        param[1] = parameter;


        parameter = new OracleParameter();
        parameter.ParameterName = "O_DATA";
        parameter.OracleDbType = OracleDbType.RefCursor;
        parameter.Direction = ParameterDirection.Output;
        param[2] = parameter;

        DataTable dtTransactions = new DataTable();
        basegeneral obj = new basegeneral();

        dtTransactions = obj.GetDataTable(connection.userSchema + ".KCC_PROMOLIST_DATALAYER_CUM.get_all_transactions", CommandType.StoredProcedure, param);
        return dtTransactions;
    }

    public string get_clr_type(int clr_type_id)
    {
        //gets the equivalent description of clr_type_id

        string clr_type_desc;
        if (clr_type_id == 1)
        {
            clr_type_desc = "FIXED PRICE";
        }
        else if (clr_type_id == 2)
        {
            clr_type_desc = "AMOUNT OFF";
        }
        else if (clr_type_id == 3)
        {
            clr_type_desc = "PERCENT OFF";
        }
        else
        {
            clr_type_desc = "";
        }

        return clr_type_desc;
    }

    public DataTable get_promotion_conflict(string headId, int location, int department, string startDate, string endDate)
    {

        OracleParameter[] param = new OracleParameter[6];
        OracleParameter parameter = new OracleParameter();

        parameter = new OracleParameter();
        parameter.ParameterName = "I_HEADID";
        parameter.OracleDbType = OracleDbType.Varchar2;
        parameter.Value = headId;
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

        DataTable dtPromoConflict = new DataTable();
        basegeneral obj = new basegeneral();

        dtPromoConflict = obj.GetDataTable(connection.userSchema + ".KCC_PROMOLIST_DATALAYER_CUM.get_promotion_conflict", CommandType.StoredProcedure, param);
        return dtPromoConflict;
    }

    public DataTable get_markdown_conflict(string headId, string startDate, string endDate)
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
        parameter = new OracleParameter();
        parameter.ParameterName = "O_DATA";
        parameter.OracleDbType = OracleDbType.RefCursor;
        parameter.Direction = ParameterDirection.Output;
        param[3] = parameter;

        DataTable dtMarkdownConflict = new DataTable();
        basegeneral obj = new basegeneral();

        dtMarkdownConflict = obj.GetDataTable(connection.userSchema + ".KCC_PROMOLIST_DATALAYER_CUM.get_markdown_conflict", CommandType.StoredProcedure, param);
        return dtMarkdownConflict;
    }

    public void update_transaction_status(int headId, string status, ref basegeneral obj)
    {
        //this function was used by selling Area and buyer

        OracleParameter[] param = new OracleParameter[2];
        OracleParameter parameter = new OracleParameter();

        parameter = new OracleParameter();

        parameter.ParameterName = "I_HEADID";
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
        obj.executeNoCommit(connection.userSchema + ".KCC_PROMOLIST_DATALAYER_CUM.update_transaction_status", CommandType.StoredProcedure, param);
    }

    public void insertPromoDate(int headId, string startDate, string endDate, ref basegeneral obj)
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

        obj.executeNoCommit(connection.userSchema + ".KCC_PROMOLIST_DATALAYER_CUM.insert_promo_date", CommandType.StoredProcedure, param);
    }

    public void deletePromoDates(int headId, ref basegeneral obj)
    {
        OracleParameter[] param = new OracleParameter[1];
        OracleParameter parameter = new OracleParameter();

        parameter = new OracleParameter();

        parameter.ParameterName = "I_HEADID";
        parameter.OracleDbType = OracleDbType.Int32;
        parameter.Value = headId;
        parameter.Direction = ParameterDirection.Input;
        param[0] = parameter;

        obj.executeNoCommit(connection.userSchema + ".KCC_PROMOLIST_DATALAYER_CUM.delete_promo_dates", CommandType.StoredProcedure, param);
    }

    public void forceEndTransaction(int headId,string newEndDate, ref basegeneral obj)
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

        parameter.ParameterName = "I_ENDDATE";
        parameter.OracleDbType = OracleDbType.Varchar2;
        parameter.Value = newEndDate;
        parameter.Direction = ParameterDirection.Input;
        param[1] = parameter;

        obj.executeNoCommit(connection.userSchema + ".KCC_PROMOLIST_DATALAYER_CUM.force_end_transaction", CommandType.StoredProcedure, param);
    }

    public void Create_Transaction_Disposed(string headId, string userId, ref basegeneral obj)
    {
        OracleParameter[] param = new OracleParameter[2];
        OracleParameter parameter = new OracleParameter();

        parameter = new OracleParameter();
        parameter.ParameterName = "I_HEAD_ID";
        parameter.OracleDbType = OracleDbType.Int32;
        parameter.Value = headId;
        parameter.Direction = ParameterDirection.Input;
        param[0] = parameter;

        parameter = new OracleParameter();
        parameter.ParameterName = "I_DISPOSED_BY";
        parameter.OracleDbType = OracleDbType.Varchar2;
        parameter.Value = userId;
        parameter.Direction = ParameterDirection.Input;
        param[1] = parameter;

        obj.GetDataTable(connection.userSchema + ".KCC_PROMOLIST_DATALAYER_CUM.CREATE_TRANSAC_HISTORY", CommandType.StoredProcedure, param);
    }

    public void Create_Transaction_Archived(string headId, ref basegeneral obj)
    {
        OracleParameter[] param = new OracleParameter[1];
        OracleParameter parameter = new OracleParameter();

        parameter = new OracleParameter();
        parameter.ParameterName = "I_HEAD_ID";
        parameter.OracleDbType = OracleDbType.Int32;
        parameter.Value = headId;
        parameter.Direction = ParameterDirection.Input;
        param[0] = parameter;

        obj.GetDataTable(connection.userSchema + ".KCC_PROMOLIST_DATALAYER_CUM.CREATE_TRANSACTION_ARC", CommandType.StoredProcedure, param);
    }

    public void Create_Audit_Trail(int headId, string status, string userId, string action, string orin, string barcode, string dateFrom, string dateTo, ref basegeneral obj)
    {
        OracleParameter[] param = new OracleParameter[8];
        OracleParameter parameter = new OracleParameter();

        parameter = new OracleParameter();
        parameter.ParameterName = "I_HEAD_ID";
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
        parameter.ParameterName = "I_USER_ID";
        parameter.OracleDbType = OracleDbType.Varchar2;
        parameter.Value = userId;
        parameter.Direction = ParameterDirection.Input;
        param[2] = parameter;

        parameter = new OracleParameter();
        parameter.ParameterName = "I_ACTION";
        parameter.OracleDbType = OracleDbType.Varchar2;
        parameter.Value = action;
        parameter.Direction = ParameterDirection.Input;
        param[3] = parameter;

        parameter = new OracleParameter();
        parameter.ParameterName = "I_ORIN";
        parameter.OracleDbType = OracleDbType.Varchar2;
        parameter.Value = orin;
        parameter.Direction = ParameterDirection.Input;
        param[4] = parameter;

        parameter = new OracleParameter();
        parameter.ParameterName = "I_BARCODE";
        parameter.OracleDbType = OracleDbType.Varchar2;
        parameter.Value = barcode;
        parameter.Direction = ParameterDirection.Input;
        param[5] = parameter;

        parameter = new OracleParameter();
        parameter.ParameterName = "I_DATE_FROM";
        parameter.OracleDbType = OracleDbType.Varchar2;
        parameter.Value = dateFrom;
        parameter.Direction = ParameterDirection.Input;
        param[6] = parameter;

        parameter = new OracleParameter();
        parameter.ParameterName = "I_DATE_TO";
        parameter.OracleDbType = OracleDbType.Varchar2;
        parameter.Value = dateTo;
        parameter.Direction = ParameterDirection.Input;
        param[7] = parameter;

        obj.GetDataTable(connection.userSchema + ".KCC_PROMOLIST_DATALAYER_CUM.CREATE_TRANSACTION_AUD", CommandType.StoredProcedure, param);
    }

    public DataTable get_item_markdown_conflict(string item, int location, int department)
    {

        OracleParameter[] param = new OracleParameter[4];
        OracleParameter parameter = new OracleParameter();

        parameter = new OracleParameter();
        parameter.ParameterName = "I_ITEM";
        parameter.OracleDbType = OracleDbType.Varchar2;
        parameter.Value = item;
        parameter.Direction = ParameterDirection.Input;
        param[0] = parameter;

        parameter = new OracleParameter();
        parameter.ParameterName = "I_LOCATION";
        parameter.OracleDbType = OracleDbType.Int32;
        parameter.Value = location;
        parameter.Direction = ParameterDirection.Input;
        param[1] = parameter;

        parameter = new OracleParameter();
        parameter.ParameterName = "I_DEPARTMENT";
        parameter.OracleDbType = OracleDbType.Int32;
        parameter.Value = department;
        parameter.Direction = ParameterDirection.Input;
        param[2] = parameter;


        parameter = new OracleParameter();
        parameter = new OracleParameter();
        parameter.ParameterName = "O_DATA";
        parameter.OracleDbType = OracleDbType.RefCursor;
        parameter.Direction = ParameterDirection.Output;
        param[3] = parameter;

        DataTable dtMarkdownConflict = new DataTable();
        basegeneral obj = new basegeneral();

        dtMarkdownConflict = obj.GetDataTable(connection.userSchema + ".KCC_PROMOLIST_DATALAYER_CUM.get_item_markdown_conflict", CommandType.StoredProcedure, param);
        return dtMarkdownConflict;
    }

    public DataTable get_item_promotion_conflict(string item, int tranId, int location, int department)
    {

        OracleParameter[] param = new OracleParameter[5];
        OracleParameter parameter = new OracleParameter();

        parameter = new OracleParameter();
        parameter.ParameterName = "I_ITEM";
        parameter.OracleDbType = OracleDbType.Varchar2;
        parameter.Value = item;
        parameter.Direction = ParameterDirection.Input;
        param[0] = parameter;

        parameter = new OracleParameter();
        parameter.ParameterName = "I_TRANID";
        parameter.OracleDbType = OracleDbType.Int32;
        parameter.Value = tranId;
        parameter.Direction = ParameterDirection.Input;
        param[1] = parameter;

        parameter = new OracleParameter();
        parameter.ParameterName = "I_LOCATION";
        parameter.OracleDbType = OracleDbType.Int32;
        parameter.Value = location;
        parameter.Direction = ParameterDirection.Input;
        param[2] = parameter;


        parameter = new OracleParameter();
        parameter.ParameterName = "I_DEPARTMENT";
        parameter.OracleDbType = OracleDbType.Int32;
        parameter.Value = department;
        parameter.Direction = ParameterDirection.Input;
        param[3] = parameter;


        parameter = new OracleParameter();
        parameter = new OracleParameter();
        parameter.ParameterName = "O_DATA";
        parameter.OracleDbType = OracleDbType.RefCursor;
        parameter.Direction = ParameterDirection.Output;
        param[4] = parameter;

        DataTable dtMarkdownConflict = new DataTable();
        basegeneral obj = new basegeneral();

        dtMarkdownConflict = obj.GetDataTable(connection.userSchema + ".KCC_PROMOLIST_DATALAYER_CUM.get_item_promotion_conflict", CommandType.StoredProcedure, param);
        return dtMarkdownConflict;
    }


    public DataTable get_item_markdown_existing(string item, int location, int department)
    {

        OracleParameter[] param = new OracleParameter[4];
        OracleParameter parameter = new OracleParameter();

        parameter = new OracleParameter();
        parameter.ParameterName = "I_ITEM";
        parameter.OracleDbType = OracleDbType.Varchar2;
        parameter.Value = item;
        parameter.Direction = ParameterDirection.Input;
        param[0] = parameter;

        parameter = new OracleParameter();
        parameter.ParameterName = "I_LOCATION";
        parameter.OracleDbType = OracleDbType.Int32;
        parameter.Value = location;
        parameter.Direction = ParameterDirection.Input;
        param[1] = parameter;

        parameter = new OracleParameter();
        parameter.ParameterName = "I_DEPARTMENT";
        parameter.OracleDbType = OracleDbType.Int32;
        parameter.Value = department;
        parameter.Direction = ParameterDirection.Input;
        param[2] = parameter;


        parameter = new OracleParameter();
        parameter = new OracleParameter();
        parameter.ParameterName = "O_DATA";
        parameter.OracleDbType = OracleDbType.RefCursor;
        parameter.Direction = ParameterDirection.Output;
        param[3] = parameter;

        DataTable dtMarkdownConflict = new DataTable();
        basegeneral obj = new basegeneral();

        dtMarkdownConflict = obj.GetDataTable(connection.userSchema + ".KCC_PROMOLIST_DATALAYER_CUM.get_item_markdown_existing", CommandType.StoredProcedure, param);
        return dtMarkdownConflict;
    }

    public DataTable get_item_promotion_existing(string item, int tranId, int location, int department)
    {

        OracleParameter[] param = new OracleParameter[5];
        OracleParameter parameter = new OracleParameter();

        parameter = new OracleParameter();
        parameter.ParameterName = "I_ITEM";
        parameter.OracleDbType = OracleDbType.Varchar2;
        parameter.Value = item;
        parameter.Direction = ParameterDirection.Input;
        param[0] = parameter;

        parameter = new OracleParameter();
        parameter.ParameterName = "I_TRANID";
        parameter.OracleDbType = OracleDbType.Int32;
        parameter.Value = tranId;
        parameter.Direction = ParameterDirection.Input;
        param[1] = parameter;

        parameter = new OracleParameter();
        parameter.ParameterName = "I_LOCATION";
        parameter.OracleDbType = OracleDbType.Int32;
        parameter.Value = location;
        parameter.Direction = ParameterDirection.Input;
        param[2] = parameter;


        parameter = new OracleParameter();
        parameter.ParameterName = "I_DEPARTMENT";
        parameter.OracleDbType = OracleDbType.Int32;
        parameter.Value = department;
        parameter.Direction = ParameterDirection.Input;
        param[3] = parameter;


        parameter = new OracleParameter();
        parameter = new OracleParameter();
        parameter.ParameterName = "O_DATA";
        parameter.OracleDbType = OracleDbType.RefCursor;
        parameter.Direction = ParameterDirection.Output;
        param[4] = parameter;

        DataTable dtMarkdownConflict = new DataTable();
        basegeneral obj = new basegeneral();

        dtMarkdownConflict = obj.GetDataTable(connection.userSchema + ".KCC_PROMOLIST_DATALAYER_CUM.get_item_promotion_existing", CommandType.StoredProcedure, param);
        return dtMarkdownConflict;
    }
}
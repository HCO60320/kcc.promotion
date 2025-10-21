using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Oracle.ManagedDataAccess;
using Oracle.ManagedDataAccess.Client;
using promotions;


/// <summary>
/// Summary description for pricerModelClass
/// </summary>
public class pricerModelClass
{
    public pricerModelClass()
    {   //
        // TODO: Add constructor logic here
        //
    }

    public DataTable get_prc_items_transaction(string headId)
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

        DataTable dtItems = new DataTable();
        basegeneral obj = new basegeneral();

        dtItems = obj.GetDataTable(connection.userSchema + ".KCC_PROMOLIST_DATALAYER_CUM.get_prc_items_transaction", CommandType.StoredProcedure, param);
        return dtItems;
    }

    public DataTable get_prc_items_freelist(string headId)
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

        DataTable dtItems = new DataTable();
        basegeneral obj = new basegeneral();

        dtItems = obj.GetDataTable(connection.userSchema + ".KCC_PROMOLIST_DATALAYER_CUM.get_prc_items_freelist", CommandType.StoredProcedure, param);
        return dtItems;
    }

    public DataTable get_prc_items_notfl(string headId)
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

        DataTable dtItems = new DataTable();
        basegeneral obj = new basegeneral();

        dtItems = obj.GetDataTable(connection.userSchema + ".KCC_PROMOLIST_DATALAYER_CUM.get_prc_items_notfl", CommandType.StoredProcedure, param);
        return dtItems;
    }

    public DataTable get_all_byr_transactions(string username)
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

        dtTransactions = obj.GetDataTable(connection.userSchema + ".KCC_PROMOLIST_DATALAYER_CUM.get_all_byr_transactions", CommandType.StoredProcedure, param);
        return dtTransactions;
    }

    public DataTable get_all_transactions(string username, string rolename)
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
        parameter.Value = rolename;
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

    public DataTable get_promo_dates(string headId)
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

        DataTable dtItems = new DataTable();
        basegeneral obj = new basegeneral();

        dtItems = obj.GetDataTable(connection.userSchema + ".KCC_PROMOLIST_DATALAYER_CUM.get_promo_date", CommandType.StoredProcedure, param);
        return dtItems;
    }

    public void update_approver(int headId, string username, ref basegeneral obj)
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
        parameter.ParameterName = "I_USERNAME";
        parameter.OracleDbType = OracleDbType.Varchar2;
        parameter.Value = username;
        parameter.Direction = ParameterDirection.Input;
        param[1] = parameter;

        obj.executeNoCommit(connection.userSchema + ".KCC_PROMOLIST_DATALAYER_CUM.UPDATE_APPROVER", CommandType.StoredProcedure, param);
    }

    public string insert_itemlist(int headId, string username, int rewardApplication, ref basegeneral obj)
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
        parameter.ParameterName = "I_USERNAME";
        parameter.OracleDbType = OracleDbType.Varchar2;
        parameter.Value = username;
        parameter.Direction = ParameterDirection.Input;
        param[1] = parameter;

        parameter = new OracleParameter();
        parameter.ParameterName = "I_REWARDAPPLICATION";
        parameter.OracleDbType = OracleDbType.Int16;
        parameter.Value = rewardApplication;
        parameter.Direction = ParameterDirection.Input;
        param[2] = parameter;

        parameter = new OracleParameter();
        parameter.ParameterName = "O_ITEMLISTNO";
        parameter.OracleDbType = OracleDbType.Int32;
        parameter.Direction = ParameterDirection.Output;
        param[3] = parameter;

        obj.executeNoCommit(connection.userSchema + ".KCC_PROMOLIST_DATALAYER_CUM.INSERT_ITEMLIST", CommandType.StoredProcedure, param);
        string itemListNo = param[3].Value.ToString();
        return itemListNo;
    }

    public void update_printedby(int headId, string username, ref basegeneral obj)
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
        parameter.ParameterName = "I_USERNAME";
        parameter.OracleDbType = OracleDbType.Varchar2;
        parameter.Value = username;
        parameter.Direction = ParameterDirection.Input;
        param[1] = parameter;


        obj.executeNoCommit(connection.userSchema + ".KCC_PROMOLIST_DATALAYER_CUM.UPDATE_PRC_PRINTEDBY", CommandType.StoredProcedure, param);
    }

    public DataTable get_item_promo_type(string headId)
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

        DataTable dtItems = new DataTable();
        basegeneral obj = new basegeneral();

        dtItems = obj.GetDataTable(connection.userSchema + ".KCC_PROMOLIST_DATALAYER_CUM.GET_ITEM_PROMO_TYPE", CommandType.StoredProcedure, param);
        return dtItems;
    }

    public DataTable get_filter_transactions(string username, string rolename, int searchType, string searchValue)
    {
        OracleParameter[] param = new OracleParameter[5];
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
        parameter.Value = rolename;
        parameter.Direction = ParameterDirection.Input;
        param[1] = parameter;

        parameter = new OracleParameter();
        parameter.ParameterName = "I_SEARCH_TYPE";
        parameter.OracleDbType = OracleDbType.Int64;
        parameter.Value = searchType;
        parameter.Direction = ParameterDirection.Input;
        param[2] = parameter;


        parameter = new OracleParameter();
        parameter.ParameterName = "I_SEARCH_VALUE";
        parameter.OracleDbType = OracleDbType.Varchar2;
        parameter.Value = searchValue;
        parameter.Direction = ParameterDirection.Input;
        param[3] = parameter;


        parameter = new OracleParameter();
        parameter.ParameterName = "O_DATA";
        parameter.OracleDbType = OracleDbType.RefCursor;
        parameter.Direction = ParameterDirection.Output;
        param[4] = parameter;


        DataTable dtTransactions = new DataTable();
        basegeneral obj = new basegeneral();

        dtTransactions = obj.GetDataTable(connection.userSchema + ".KCC_PROMOLIST_DATALAYER_CUM.GET_FILTER_TRANSACTIONS", CommandType.StoredProcedure, param);
        return dtTransactions;
    }

    public void RemoveDate(int headId)
    {
        OracleParameter[] param = new OracleParameter[1];
        OracleParameter parameter = new OracleParameter();

        parameter = new OracleParameter();

        parameter.ParameterName = "I_HEADID";
        parameter.OracleDbType = OracleDbType.Int32;
        parameter.Value = headId;
        parameter.Direction = ParameterDirection.Input;
        param[0] = parameter;




        basegeneral obj = new basegeneral();
        try
        {
            obj.executeNoCommit(connection.userSchema + ".KCC_PROMOLIST_DATALAYER_CUM.Remove_Date", CommandType.StoredProcedure, param);
            obj.Commit();
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    public DataTable checkReclass(string headId)
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
        parameter.ParameterName = "O_DATA";
        parameter.OracleDbType = OracleDbType.RefCursor;
        parameter.Direction = ParameterDirection.Output;
        param[1] = parameter;

        DataTable dtItems = new DataTable();
        basegeneral obj = new basegeneral();

        dtItems = obj.GetDataTable(connection.userSchema + ".KCC_PROMOLIST_DATALAYER_CUM.CHECK_RECLASS", CommandType.StoredProcedure, param);
        return dtItems;
    }
    public void ReclassTran(int headId, string username)
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
        parameter.ParameterName = "I_USERNAME";
        parameter.OracleDbType = OracleDbType.Varchar2;
        parameter.Value = username;
        parameter.Direction = ParameterDirection.Input;
        param[1] = parameter;
        basegeneral obj = new basegeneral();
        try
        {
            obj.executeNoCommit(connection.userSchema + ".KCC_PROMOLIST_DATALAYER_CUM.RECLASS_TRAN", CommandType.StoredProcedure, param);
            obj.Commit();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    // Add New Procedure Class
    public DataTable GetReClassItemsWDept(int headId)
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
        parameter.ParameterName = "O_DATA";
        parameter.OracleDbType = OracleDbType.RefCursor;
        parameter.Direction = ParameterDirection.Output;
        param[1] = parameter;

        DataTable dtuser = new DataTable();
        basegeneral obj = new basegeneral();

        dtuser = obj.GetDataTable(connection.userSchema + ".KCC_PROMOLIST_DATALAYER_CUM.GET_RECLASS_ITEMS_W_DEPT", CommandType.StoredProcedure, param);
        return dtuser;

    }

    public DataTable CheckPromoCanReclass(string username)
    {
        OracleParameter[] param = new OracleParameter[2];
        OracleParameter parameter = new OracleParameter();

        parameter = new OracleParameter();
        parameter.ParameterName = "I_username";
        parameter.OracleDbType = OracleDbType.Varchar2;
        parameter.Value = username;
        parameter.Direction = ParameterDirection.Input;
        param[0] = parameter;

        parameter = new OracleParameter();
        parameter.ParameterName = "O_DATA";
        parameter.OracleDbType = OracleDbType.RefCursor;
        parameter.Direction = ParameterDirection.Output;
        param[1] = parameter;

        DataTable dtuser = new DataTable();
        basegeneral obj = new basegeneral();

        dtuser = obj.GetDataTable(connection.userSchema + ".KCC_PROMOLIST_DATALAYER_CUM.CHECK_PROMO_CAN_RECLASS", CommandType.StoredProcedure, param);
        return dtuser;
    }

    public void RemoveReclassItems(int headId, string username)
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
        parameter.ParameterName = "I_USERNAME";
        parameter.OracleDbType = OracleDbType.Varchar2;
        parameter.Value = username;
        parameter.Direction = ParameterDirection.Input;
        param[1] = parameter;
        basegeneral obj = new basegeneral();
        try
        {
            obj.executeNoCommit(connection.userSchema + ".KCC_PROMOLIST_DATALAYER_CUM.REMOVE_RECLASS_ITEMS", CommandType.StoredProcedure, param);
            obj.Commit();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    //END
}

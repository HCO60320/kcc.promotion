using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Oracle.ManagedDataAccess;
using Oracle.ManagedDataAccess.Client;
using promotions;

/// <summary>
/// Summary description for adminModelClass
/// </summary>
public class adminModelClass
{
    public adminModelClass()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public DataTable getReprintTransactions()
    {
        OracleParameter[] param = new OracleParameter[1];
        OracleParameter parameter = new OracleParameter();

        parameter = new OracleParameter();
        parameter.ParameterName = "O_DATA";
        parameter.OracleDbType = OracleDbType.RefCursor;
        parameter.Direction = ParameterDirection.Output;
        param[0] = parameter;

        DataTable dtReprintTransactions = new DataTable();
        basegeneral obj = new basegeneral();

        dtReprintTransactions = obj.GetDataTable(connection.userSchema + ".KCC_PROMOLIST_DATALAYER_CUM.get_reprint_transactions", CommandType.StoredProcedure, param);
        return dtReprintTransactions;
    }

    public DataTable getReprintTransaction(int headId)
    {
        OracleParameter[] param = new OracleParameter[2];
        OracleParameter parameter = new OracleParameter();

        parameter = new OracleParameter();
        parameter.ParameterName = "I_HEADID";
        parameter.OracleDbType = OracleDbType.Int32;
        parameter.Direction = ParameterDirection.Input;
        parameter.Value = headId;
        param[0] = parameter;

        parameter = new OracleParameter();
        parameter.ParameterName = "O_DATA";
        parameter.OracleDbType = OracleDbType.RefCursor;
        parameter.Direction = ParameterDirection.Output;
        param[1] = parameter;

        DataTable dtReprintTransaction = new DataTable();
        basegeneral obj = new basegeneral();
        dtReprintTransaction = obj.GetDataTable(connection.userSchema + ".KCC_PROMOLIST_DATALAYER_CUM.get_reprint_transaction", CommandType.StoredProcedure, param);
        return dtReprintTransaction;
    }

    public DataTable getReprintTransaction(int headId, string pinCode)
    {
        OracleParameter[] param = new OracleParameter[3];
        OracleParameter parameter = new OracleParameter();

        parameter = new OracleParameter();
        parameter.ParameterName = "I_HEADID";
        parameter.OracleDbType = OracleDbType.Int32;
        parameter.Direction = ParameterDirection.Input;
        parameter.Value = headId;
        param[0] = parameter;

        parameter = new OracleParameter();
        parameter.ParameterName = "I_PINCODE";
        parameter.OracleDbType = OracleDbType.Varchar2;
        parameter.Direction = ParameterDirection.Input;
        parameter.Value = pinCode;
        param[1] = parameter;


        parameter = new OracleParameter();
        parameter.ParameterName = "O_DATA";
        parameter.OracleDbType = OracleDbType.RefCursor;
        parameter.Direction = ParameterDirection.Output;
        param[2] = parameter;

        DataTable dtReprintTransaction = new DataTable();
        basegeneral obj = new basegeneral();

        dtReprintTransaction = obj.GetDataTable(connection.userSchema + ".KCC_PROMOLIST_DATALAYER_CUM.get_reprint_transaction", CommandType.StoredProcedure, param);
        return dtReprintTransaction;
    }

    public DataTable getPurgeTransactions()
    {
        OracleParameter[] param = new OracleParameter[1];
        OracleParameter parameter = new OracleParameter();

        parameter = new OracleParameter();
        parameter.ParameterName = "O_DATA";
        parameter.OracleDbType = OracleDbType.RefCursor;
        parameter.Direction = ParameterDirection.Output;
        param[0] = parameter;

        DataTable dtReprintTransactions = new DataTable();
        basegeneral obj = new basegeneral();

        dtReprintTransactions = obj.GetDataTable(connection.userSchema + ".KCC_PROMOLIST_DATALAYER_CUM.get_purge_transactions", CommandType.StoredProcedure, param);
        return dtReprintTransactions;
    }

    public void updateReprintStatus(int headId, string status, ref basegeneral obj)
    {
        OracleParameter[] param = new OracleParameter[2];
        OracleParameter parameter = new OracleParameter();

        parameter = new OracleParameter();
        parameter.ParameterName = "I_HEADID";
        parameter.OracleDbType = OracleDbType.Int32;
        parameter.Direction = ParameterDirection.Input;
        parameter.Value = headId;
        param[0] = parameter;

        parameter = new OracleParameter();
        parameter.ParameterName = "I_STATUS";
        parameter.OracleDbType = OracleDbType.Varchar2;
        parameter.Direction = ParameterDirection.Input;
        parameter.Value = status;
        param[1] = parameter;

        obj.executeNoCommit(connection.userSchema + ".KCC_PROMOLIST_DATALAYER_CUM.update_reprint_status", CommandType.StoredProcedure, param);
    }

    public void updateReprintApprover(int headId, string username, ref basegeneral obj)
    {
        OracleParameter[] param = new OracleParameter[2];
        OracleParameter parameter = new OracleParameter();

        parameter = new OracleParameter();
        parameter.ParameterName = "I_HEADID";
        parameter.OracleDbType = OracleDbType.Int32;
        parameter.Direction = ParameterDirection.Input;
        parameter.Value = headId;
        param[0] = parameter;


        parameter = new OracleParameter();
        parameter.ParameterName = "I_USERNAME";
        parameter.OracleDbType = OracleDbType.Varchar2;
        parameter.Direction = ParameterDirection.Input;
        parameter.Value = username;
        param[1] = parameter;

        obj.executeNoCommit(connection.userSchema + ".KCC_PROMOLIST_DATALAYER_CUM.update_reprint_approver", CommandType.StoredProcedure, param);
    }

    public DataTable getPurgeTransactions_HeadID(string transactionID)
    {
        OracleParameter[] param = new OracleParameter[2];
        OracleParameter parameter = new OracleParameter();

        parameter = new OracleParameter();
        parameter.ParameterName = "I_TRAN_ID";
        parameter.OracleDbType = OracleDbType.Int32;
        parameter.Value = transactionID;
        parameter.Direction = ParameterDirection.Input;
        param[0] = parameter;

        parameter = new OracleParameter();
        parameter.ParameterName = "O_DATA";
        parameter.OracleDbType = OracleDbType.RefCursor;
        parameter.Direction = ParameterDirection.Output;
        param[1] = parameter;

        DataTable dtTransactions = new DataTable();
        basegeneral obj = new basegeneral();

        dtTransactions = obj.GetDataTable(connection.userSchema + ".KCC_PROMOLIST_DATALAYER_CUM.GET_TRANSACTION_HEAD_ID", CommandType.StoredProcedure, param);
        return dtTransactions;
    }

    public DataTable getTransaction(int tranNo)
    {
        OracleParameter[] param = new OracleParameter[2];
        OracleParameter parameter = new OracleParameter();

        parameter = new OracleParameter();
        parameter.ParameterName = "I_TRAN_ID";
        parameter.OracleDbType = OracleDbType.Int32;
        parameter.Direction = ParameterDirection.Input;
        parameter.Value = tranNo;
        param[0] = parameter;

        parameter = new OracleParameter();
        parameter.ParameterName = "O_DATA";
        parameter.OracleDbType = OracleDbType.RefCursor;
        parameter.Direction = ParameterDirection.Output;
        param[1] = parameter;


        DataTable dtTransaction = new DataTable();
        basegeneral obj = new basegeneral();

        dtTransaction = obj.GetDataTable(connection.userSchema + ".KCC_PROMOLIST_DATALAYER_CUM.GET_TRANSACTION_HEAD_ID", CommandType.StoredProcedure, param);
        return dtTransaction;
    }
    public DataTable RevertTransactionToWorksheet(int tranNo)
    {
        OracleParameter[] param = new OracleParameter[2];
        OracleParameter parameter = new OracleParameter();

        parameter = new OracleParameter();
        parameter.ParameterName = "I_TRAN_ID";
        parameter.OracleDbType = OracleDbType.Int32;
        parameter.Direction = ParameterDirection.Input;
        parameter.Value = tranNo;
        param[0] = parameter;

        DataTable dtTransaction = new DataTable();
        basegeneral obj = new basegeneral();

        dtTransaction = obj.GetDataTable(connection.userSchema + ".KCC_PROMOLIST_DATALAYER_CUM.modify_status_to_worksheet", CommandType.StoredProcedure, param);
        return dtTransaction;
    }
}
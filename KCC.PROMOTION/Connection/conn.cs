using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Oracle.ManagedDataAccess.Client;
using System.Data;


namespace promotions
{
    public class conn
    {   private OracleTransaction meTransaction;
        private bool transactionStatus = false;

        private OracleConnection cn = new OracleConnection(connection.connStr);
        private OracleDataAdapter da = new OracleDataAdapter();
        private OracleCommand cmd;
        private string connStr;
        private string rtnValue;

    public virtual DataTable GetDataTable(string strCmd, CommandType cmdType, OracleParameter[]  param = null) {
            DataTable dt = new DataTable();
            cmd = new OracleCommand();
            //cn = new OracleConnection(connection.connStr);
            try {
                if (!(param == null)) {
                    foreach (OracleParameter p in param) {
                        if (!(p == null)) {
                            cmd.Parameters.Add(p);
                        }
                    }
                }
               
                cmd.CommandText = strCmd;
                cmd.CommandType = cmdType;
                cmd.Connection = OpenConnection();
                da = new OracleDataAdapter(cmd);      
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex) {

                throw new Exception(ex.Message);
                // Throw New Exception("There was an error retrieving inmate record")
            }
            finally {
                da.Dispose();
                CloseConnection();
            }
    }

    public virtual string GetValue(string strCmd, CommandType cmdType, OracleParameter[] param = null) {
        object rtnVal;
        cmd = new OracleCommand();
        try {
            if (!(param == null)) {
                cmd.Parameters.AddRange(param);
            }
            cmd.CommandText = strCmd;
            cmd.CommandType = cmdType;
            cmd.Connection = OpenConnection();
            rtnVal = cmd.ExecuteScalar();
            return rtnVal.ToString();
        } catch (Exception ex) {
            throw new Exception(ex.Message);
        } finally {
            CloseConnection();
        }
    }


    //public virtual string Execute(string strCmd, CommandType cmdType, OracleParameter[] param = null) {
    //    cn = new OracleConnection(connection.connStr);
    //    try {
    //        if (!(param == null)) {
    //            if (!(param == null)) {
    //                foreach (OracleParameter p in param) {
    //                    cmd.Parameters.Add(p);
    //                }
    //            }
    //        }
    //        cmd.CommandText = strCmd;
    //        cmd.Connection = cn;
    //        cmd.CommandType = cmdType;
    //        cn.Open();
    //        cmd.ExecuteScalar();
    //        // rtrnValue = cmd.Parameters(rtnprm).Value
    //        return rtnValue;
    //    }
    //    catch (Exception ex) {
    //        throw new Exception(ex.Message);
    //    }
    //    finally {
    //        if ((cn.State == ConnectionState.Open) && (transactionStatus == false))
    //        {
    //            cn.Close();
    //            cmd.Parameters.Clear();
    //        }
    //    }
    //}

    public virtual void executeNoCommit(string strCmd, CommandType cmdType, OracleParameter[] param = null)
    {
        cmd = new OracleCommand();
        try
        {
            
            if (!(param == null))
            {
                cmd.Parameters.AddRange(param);
            }
            cmd.CommandText = strCmd;
            cmd.CommandType = cmdType;
            cmd.Connection = OpenConnection();
            cmd.Connection = cn;
            StartTransaction();
            cmd.Transaction = meTransaction;
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        finally
        {
            cmd.Dispose();
            CloseConnection();
        }
    }

    //public virtual DataTable executeNoCommit(string procedure_name, bool withReturns, CommandType cmdType, OracleParameter[] param = null)
    //{
    //    DataTable dt = new DataTable();
    //    cmd = new OracleCommand();
    //    //cn = new OracleConnection(connection.connStr);
    //    try
    //    {
    //        if (!(param == null))
    //        {
    //            if (!(param == null))
    //            {
    //                foreach (OracleParameter p in param)
    //                {
    //                    if (!(p == null))
    //                    {
    //                        cmd.Parameters.Add(p);
    //                    }
    //                }
    //            }
    //        }


    //        cmd.CommandText = procedure_name;
    //        cmd.CommandType = cmdType;
    //        cmd.Connection = OpenConnection();
    //        StartTransaction();
    //        cmd.Transaction = meTransaction;

    //        if ((withReturns == true))
    //        {
    //            da = new OracleDataAdapter(cmd);
    //            da.Fill(dt);
    //            return dt;
    //        }
    //        else
    //        {
    //            cmd.ExecuteNonQuery();
    //            return dt;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        throw new Exception(ex.Message);
    //    }
    //    finally
    //    {
    //        cmd.Parameters.Clear();
    //        CloseConnection();
    //    }
    //}

    public void Rollback(){
        if (((cn.State == ConnectionState.Open) && (transactionStatus == true))){
            transactionStatus = false;
            meTransaction.Rollback();
            cn.Close();
            meTransaction.Dispose();
        }
        else if ((((cn.State == ConnectionState.Closed) || (cn.State == ConnectionState.Broken))&& (transactionStatus == true))){
            transactionStatus = false;
            cn.Close();
            meTransaction.Dispose();
        }
    }

    private void StartTransaction() {
        if (transactionStatus == false) {
            transactionStatus = true;
            meTransaction = cn.BeginTransaction(IsolationLevel.ReadCommitted);
        }
    }
    
    public void Commit() {
        if (((cn.State == ConnectionState.Open) && (transactionStatus == true))) {
            transactionStatus = false;
            meTransaction.Commit();
            meTransaction.Dispose();
            cn.Close();
        }
    }

    protected OracleConnection OpenConnection() {
        try {
            if (((cn.State == ConnectionState.Closed)|| (cn.State == ConnectionState.Broken)))
            {
                cn.Open();
            }
        }
        catch (Exception ex) {
            throw new Exception(ex.Message);
        }
        return cn;
    }

    private void CloseConnection() {
        if (((cn.State == ConnectionState.Open) && (transactionStatus == false))){
            cn.Close();
        }
    }

    }
}
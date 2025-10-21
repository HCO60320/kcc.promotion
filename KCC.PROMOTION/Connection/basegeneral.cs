using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace promotions
{
    public class basegeneral:conn
    {
         public DataTable GetDataTable(string strCmd, System.Data.CommandType cmdType, Oracle.ManagedDataAccess.Client.OracleParameter[] param = null) {
            return base.GetDataTable(strCmd, cmdType, param);
         }

         public void executeNoCommit(string strCmd, System.Data.CommandType cmdType, Oracle.ManagedDataAccess.Client.OracleParameter[] param = null){
              base.executeNoCommit(strCmd, cmdType, param);
         }

         public string GetValue(string strCmd, CommandType cmdType, OracleParameter[] param = null) {
             return base.GetValue(strCmd, cmdType, param);
         }

         //public DataTable executeNoCommit(string strCmd, bool withReturns, System.Data.CommandType cmdType, Oracle.ManagedDataAccess.Client.OracleParameter[] param = null)
         //{
         //    return base.executeNoCommit(strCmd, withReturns, cmdType, param);
         //}

    }
}
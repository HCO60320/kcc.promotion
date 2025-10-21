using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using promotions;
using Oracle.ManagedDataAccess;
using Oracle.ManagedDataAccess.Client;

/// <summary>
/// Summary description for userModelClass
/// </summary>
public class userModelClass
{
    public userModelClass()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public DataTable getUser(string username, string password)
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
        parameter.ParameterName = "I_PASSWORD";
        parameter.OracleDbType = OracleDbType.Varchar2;
        parameter.Value = password;
        parameter.Direction = ParameterDirection.Input;
        param[1] = parameter;

        parameter = new OracleParameter();
        parameter.ParameterName = "O_DATA";
        parameter.OracleDbType = OracleDbType.RefCursor;
        parameter.Direction = ParameterDirection.Output;
        param[2] = parameter;

        DataTable dt = new DataTable();
        basegeneral obj = new basegeneral();
        dt = obj.GetDataTable(connection.userSchema + ".KCC_PROMOLIST_DATALAYER_CUM.GETUSER", CommandType.StoredProcedure, param);
        return dt;
    }

    public DataTable get_user_role(int appId, int userId)
    {
        DataTable dtRoles = new DataTable();
        basegeneral obj = new basegeneral();
        OracleParameter[] param = new OracleParameter[3];
        OracleParameter parameter = new OracleParameter();

        parameter = new OracleParameter();

        parameter.ParameterName = "I_APPID";
        parameter.OracleDbType = OracleDbType.Int32;
        parameter.Value = appId;
        parameter.Direction = ParameterDirection.Input;
        param[0] = parameter;

        parameter = new OracleParameter();
        parameter.ParameterName = "I_USERID";
        parameter.OracleDbType = OracleDbType.Int32;
        parameter.Value = userId;
        parameter.Direction = ParameterDirection.Input;
        param[1] = parameter;

        parameter = new OracleParameter();
        parameter.ParameterName = "O_DATA";
        parameter.OracleDbType = OracleDbType.RefCursor;
        parameter.Direction = ParameterDirection.Output;
        param[2] = parameter;

        dtRoles = obj.GetDataTable(connection.userSchema + ".KCC_PROMOLIST_DATALAYER_CUM.GET_USER_ROLE", CommandType.StoredProcedure, param);
        return dtRoles;
    }
}
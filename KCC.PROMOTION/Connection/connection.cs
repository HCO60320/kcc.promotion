using Oracle.ManagedDataAccess.Client;
using System.Data;
public static class connection {
    public static OracleConnection conn;
    public static OracleDataAdapter adap;
    public static OracleCommand comm;
    public static string connStr;

    public static string username { get;set; }
    public static string password {get; set; }
    public static string host{get; set;}
    public static int port { get; set; }
    public static string sID { get; set; }
    public static string userSchema { get; set; }
    public static string connectionType { get; set; }

    public static void instantiate()
    {        
        conn = new OracleConnection("Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST= "+ host +")(PORT="+ port +")))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME="+ sID +")));User Id="+ username +";Password="+ password +";");
        connStr = conn.ConnectionString;
        comm = new OracleCommand("", conn);
    }

}
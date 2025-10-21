using Oracle.ManagedDataAccess.Client;
using promotions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net;
using System.Configuration;

namespace KCC.PROMOTION.Class
{
    internal class IncidentReport
    {
        DataTable dt = new DataTable();
        basegeneral obj = new basegeneral();
        SmtpClient client = new SmtpClient();
        MailMessage msgObj = new MailMessage();
        public int TransactionId { get; set; }
        public string RequestCode { get; set; }
        public string PinCode { get; set; }
        public string Sender { get; set; }
        public string Reason { get; set; }
        public byte District { get; set; }


        string emailBody = "";
        string htmlCss = "<style>" +
        "#dtTable {" +
            "font-family: \"Trebuchet MS\", Arial, Helvetica, sans-serif;" +
            "border-collapse: collapse;" +
            "width: 90%;" +
        "}" +
        "#dtTable td," +
        "#customers th {" +
            "border: 1px solid #ddd;" +
            "padding: 5px;" +
        "}" +
        "#dtTable tr:nth-child(even) {" +
            "background-color: #f2f2f2;" +
        "}" +
        "#dtTable tr:hover {" +
            "background-color: #ddd;" +
        "}" +
        "#dtTable th {" +
            "padding-top: 10px;" +
            "padding-bottom: 10px;" +
            "text-align: left;" +
            "background-color: #4CAF50;" +
            "color: white;" +
        "}" +
    "</style>";
        public void SetEmailBody()
        {
            string htmlHead = "<head><title>PROMOTION SYSTEM INCIDENT REPORT</title>" + htmlCss + "</head>";
            string htmlTr = "";
            DataTable items = this.TransactionItem();
            items.Columns.Remove("IR_DTL_ID");//ir_dtl_id
            foreach (DataRow row in items.Rows)
            {
                htmlTr += "<tr>";
                foreach (DataColumn col in items.Columns)
                {
                    htmlTr += "<td>" + row[col] + "</td>";
                }
                htmlTr += "</tr>";
            }
            string htmlBody = "<body>" +
                                    "<p>Reason: "+ this.Reason +"</p>" +
                                    "<p>Request Code: " + this.RequestCode + "</p>" +
                                    "<p>PIN Code: " + this.PinCode + "</p>" +
                                    "<p>From: " + this.Sender + "</p>" +
                                    "<table id=\"dtTable\" style=\"width: 100%\">" +
                                         "<thead><tr>" +
                                             "<th>ORIN</th>" +
                                             "<th>BARCODE</th>" +
                                             "<th>VPN</th>" +
                                             "<th>ITEM DESCRIPTION</th>" +
                                             "<th>QUANTITY</th>" +
                                             "<th>ACTUAL QUANTITY</th>" +
                                         "</tr></thead><tbody>" +
                                         htmlTr +
                                    "</tbody></table>" +
                                "</body>";
            emailBody = "<html>" + htmlHead + htmlBody + "</html>";
        }

        internal void SendEmail()
        {
            string subject = "Promotion Incident Report";
            int port = Convert.ToInt16(ConfigurationManager.AppSettings["EmailPort"]);
            string host = ConfigurationManager.AppSettings["EmailHost"];
            string emailSender = ConfigurationManager.AppSettings["EmailSender"];
            string userCredPass = ConfigurationManager.AppSettings["EmailPassword"];
            client.Credentials = new NetworkCredential(emailSender, userCredPass);
            client.Port = port;
            client.Host = host;
            msgObj.Attachments.Clear();
            msgObj.AlternateViews.Clear();
            msgObj.From = new MailAddress(emailSender);

            msgObj.To.Clear();
            foreach (DataRow item in this.EmailTo().Rows)
            {
                msgObj.To.Add(item["EMAIL"].ToString());
            }

            msgObj.CC.Clear();
            foreach (DataRow item in this.EmailCC().Rows)
            {
                msgObj.CC.Add(item["EMAIL"].ToString());
            }

            msgObj.Bcc.Clear();
            foreach (DataRow item in this.EmailBCC().Rows)
            {
                msgObj.Bcc.Add(item["EMAIL"].ToString());
            }

            this.SetEmailBody();
            msgObj.Subject = subject;
            msgObj.Body = emailBody;
            msgObj.IsBodyHtml = true;
            AlternateView alt = AlternateView.CreateAlternateViewFromString(msgObj.Body, null, "text/html");
            msgObj.AlternateViews.Add(alt);
            this.SetReason();
            client.Send(msgObj);
        }
        
        private DataTable EmailTo()
        {
            try
            {
                OracleParameter[] param = new OracleParameter[1];
                OracleParameter parameter = new OracleParameter();

                parameter = new OracleParameter();
                parameter.ParameterName = "o_data";
                parameter.OracleDbType = OracleDbType.RefCursor;
                parameter.Direction = ParameterDirection.Output;
                param[0] = parameter;

                return obj.GetDataTable(connection.userSchema + ".KCC_PROMOLIST_DATALAYER_CUM.get_ir_email_to", CommandType.StoredProcedure, param);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private DataTable EmailCC()
        {
            try
            {
                OracleParameter[] param = new OracleParameter[3];
                OracleParameter parameter = new OracleParameter();

                parameter = new OracleParameter();
                parameter.ParameterName = "i_tran_id";
                parameter.Value = this.TransactionId;
                parameter.OracleDbType = OracleDbType.Int16;
                parameter.Direction = ParameterDirection.Input;
                param[0] = parameter;

                parameter = new OracleParameter();
                parameter.ParameterName = "i_district";
                parameter.Value = this.District;
                parameter.OracleDbType = OracleDbType.Int16;
                parameter.Direction = ParameterDirection.Input;
                param[1] = parameter;

                parameter = new OracleParameter();
                parameter.ParameterName = "o_data";
                parameter.OracleDbType = OracleDbType.RefCursor;
                parameter.Direction = ParameterDirection.Output;
                param[2] = parameter;

                return obj.GetDataTable(connection.userSchema + ".KCC_PROMOLIST_DATALAYER_CUM.get_ir_email_cc", CommandType.StoredProcedure, param);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private DataTable EmailBCC()
        {
            try
            {
                OracleParameter[] param = new OracleParameter[1];
                OracleParameter parameter = new OracleParameter();

                parameter = new OracleParameter();
                parameter.ParameterName = "o_data";
                parameter.OracleDbType = OracleDbType.RefCursor;
                parameter.Direction = ParameterDirection.Output;
                param[0] = parameter;

                return obj.GetDataTable(connection.userSchema + ".KCC_PROMOLIST_DATALAYER_CUM.get_ir_email_bcc", CommandType.StoredProcedure, param);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataTable TransactionItem()
        {
            try
            {
                OracleParameter[] param = new OracleParameter[2];
                OracleParameter parameter = new OracleParameter();

                parameter = new OracleParameter();
                parameter.ParameterName = "i_tran_id";
                parameter.Value = this.TransactionId;
                parameter.OracleDbType = OracleDbType.Int32;
                parameter.Direction = ParameterDirection.Input;
                param[0] = parameter;

                parameter = new OracleParameter();
                parameter.ParameterName = "o_data";
                parameter.OracleDbType = OracleDbType.RefCursor;
                parameter.Direction = ParameterDirection.Output;
                param[1] = parameter;

                return obj.GetDataTable(connection.userSchema + ".KCC_PROMOLIST_DATALAYER_CUM.get_ir_item", CommandType.StoredProcedure, param);

            }
            catch (Exception)
            {

                throw;
            }
        }

        private void SetReason()
        {
            try
            {
                OracleParameter[] param = new OracleParameter[6];
                OracleParameter parameter = new OracleParameter();

                parameter = new OracleParameter();
                parameter.ParameterName = "i_reason";
                parameter.Value = this.Reason;
                parameter.OracleDbType = OracleDbType.Varchar2;
                parameter.Direction = ParameterDirection.Input;
                param[0] = parameter;

                parameter = new OracleParameter();
                parameter.ParameterName = "i_request_code";
                parameter.Value = this.RequestCode;
                parameter.OracleDbType = OracleDbType.Varchar2;
                parameter.Direction = ParameterDirection.Input;
                param[1] = parameter;

                parameter = new OracleParameter();
                parameter.ParameterName = "i_pin_code";
                parameter.Value = this.PinCode;
                parameter.OracleDbType = OracleDbType.Varchar2;
                parameter.Direction = ParameterDirection.Input;
                param[2] = parameter;

                parameter = new OracleParameter();
                parameter.ParameterName = "i_tran_id";
                parameter.Value = this.TransactionId;
                parameter.OracleDbType = OracleDbType.Int32;
                parameter.Direction = ParameterDirection.Input;
                param[3] = parameter;

                parameter = new OracleParameter();
                parameter.ParameterName = "i_sender";
                parameter.Value = this.Sender;
                parameter.OracleDbType = OracleDbType.Varchar2;
                parameter.Direction = ParameterDirection.Input;
                param[4] = parameter;

                parameter = new OracleParameter();
                parameter.ParameterName = "o_data";
                parameter.OracleDbType = OracleDbType.RefCursor;
                parameter.Direction = ParameterDirection.Output;
                param[5] = parameter;

                obj.GetDataTable(connection.userSchema + ".KCC_PROMOLIST_DATALAYER_CUM.set_ir_reason", CommandType.StoredProcedure, param);


            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataTable GetHead()
        {
            try
            {
                OracleParameter[] param = new OracleParameter[2];
                OracleParameter parameter = new OracleParameter();

                parameter = new OracleParameter();
                parameter.ParameterName = "i_tran_id";
                parameter.Value = this.TransactionId;
                parameter.OracleDbType = OracleDbType.Int32;
                parameter.Direction = ParameterDirection.Input;
                param[0] = parameter;

                parameter = new OracleParameter();
                parameter.ParameterName = "o_data";
                parameter.OracleDbType = OracleDbType.RefCursor;
                parameter.Direction = ParameterDirection.Output;
                param[1] = parameter;

                return obj.GetDataTable(connection.userSchema + ".KCC_PROMOLIST_DATALAYER_CUM.ir_add_head", CommandType.StoredProcedure, param);

            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataTable AddItem(int tranId, string orin, int actualQuantity)
        {
            try
            {
                OracleParameter[] param = new OracleParameter[4];
                OracleParameter parameter = new OracleParameter();

                parameter = new OracleParameter();
                parameter.ParameterName = "i_tran_id";
                parameter.Value = tranId;
                parameter.OracleDbType = OracleDbType.Int32;
                parameter.Direction = ParameterDirection.Input;
                param[0] = parameter;

                parameter = new OracleParameter();
                parameter.ParameterName = "i_item";
                parameter.Value = orin;
                parameter.OracleDbType = OracleDbType.Varchar2;
                parameter.Direction = ParameterDirection.Input;
                param[1] = parameter;

                parameter = new OracleParameter();
                parameter.ParameterName = "i_qty";
                parameter.Value = actualQuantity;
                parameter.OracleDbType = OracleDbType.Int32;
                parameter.Direction = ParameterDirection.Input;
                param[2] = parameter;

                parameter = new OracleParameter();
                parameter.ParameterName = "o_data";
                parameter.OracleDbType = OracleDbType.RefCursor;
                parameter.Direction = ParameterDirection.Output;
                param[3] = parameter;

                return obj.GetDataTable(connection.userSchema + ".KCC_PROMOLIST_DATALAYER_CUM.ir_add_dtl", CommandType.StoredProcedure, param);

            }
            catch (Exception)
            {

                throw;
            }
        }


        public  void RemoveItem(int irDetailId)
        {
            OracleParameter[] param = new OracleParameter[2];
            OracleParameter parameter = new OracleParameter();

            parameter = new OracleParameter();
            parameter.ParameterName = "i_ir_dtl_id";
            parameter.Value = irDetailId;
            parameter.OracleDbType = OracleDbType.Int32;
            parameter.Direction = ParameterDirection.Input;
            param[0] = parameter;

            obj.GetDataTable(connection.userSchema + ".KCC_PROMOLIST_DATALAYER_CUM.remove_ir_item", CommandType.StoredProcedure, param);

        }

        public DataTable ModifyItem(int irDetailId, int actualQty)
        {
            OracleParameter[] param = new OracleParameter[3];
            OracleParameter parameter = new OracleParameter();

            parameter = new OracleParameter();
            parameter.ParameterName = "i_ir_dtl_id";
            parameter.Value = irDetailId;
            parameter.OracleDbType = OracleDbType.Int32;
            parameter.Direction = ParameterDirection.Input;
            param[0] = parameter;

            parameter = new OracleParameter();
            parameter.ParameterName = "i_actual_qty";
            parameter.Value = actualQty;
            parameter.OracleDbType = OracleDbType.Int32;
            parameter.Direction = ParameterDirection.Input;
            param[1] = parameter;

            parameter = new OracleParameter();
            parameter.ParameterName = "o_data";
            parameter.OracleDbType = OracleDbType.RefCursor;
            parameter.Direction = ParameterDirection.Output;
            param[2] = parameter;

            return obj.GetDataTable(connection.userSchema + ".KCC_PROMOLIST_DATALAYER_CUM.modify_ir_item", CommandType.StoredProcedure, param);

        }

        public void AutoUpdateQuantity(int promoHeadId)
        {
            OracleParameter[] param = new OracleParameter[2];
            OracleParameter parameter = new OracleParameter();

            parameter = new OracleParameter();
            parameter.ParameterName = "i_pin_code";
            parameter.Value = this.PinCode;
            parameter.OracleDbType = OracleDbType.Varchar2;
            parameter.Direction = ParameterDirection.Input;
            param[0] = parameter;

            parameter = new OracleParameter();
            parameter.ParameterName = "i_promo_head_id";
            parameter.Value = promoHeadId;
            parameter.OracleDbType = OracleDbType.Int32;
            parameter.Direction = ParameterDirection.Input;
            param[1] = parameter;

            obj.GetDataTable(connection.userSchema + ".KCC_PROMOLIST_DATALAYER_CUM.ir_modify_qty", CommandType.StoredProcedure, param);


        }
    }
}
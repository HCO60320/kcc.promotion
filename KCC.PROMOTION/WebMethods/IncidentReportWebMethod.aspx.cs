using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using KCC.PROMOTION.Class;
using System.Data;

namespace KCC.PROMOTION.WebMethods
{
    public partial class IncidentReportWebMethod : System.Web.UI.Page
    {

        [WebMethod()]
        public static string GetItem(int tranId)
        {
            object resp = null;
            DataTable dt = new DataTable();
            try
            {
                IncidentReport ir = new IncidentReport();
                ir.TransactionId = tranId;
                dt = ir.TransactionItem();
                if (dt.Rows.Count > 0)
                {
                    resp = new { status = 1, irItems = dt, irHead = ir.GetHead()};
                }
                else
                {
                    resp = new { status = 0, irHead = ir.GetHead()};
                }
            }
            catch (Exception ex)
            {
                resp = new {status = 2, message = ex.Message };
            }
            resp = JsonConvert.SerializeObject(resp, Formatting.Indented);
            return resp.ToString();
        }


        [WebMethod()]
        public static string AddItem(int tranId, string orin, int actualQuantity)
        {
            object resp = null;
            DataTable dt = new DataTable();
            try
            {
                IncidentReport ir = new IncidentReport();
                resp = new { status = 1, addedItem = ir.AddItem(tranId, orin, actualQuantity)};
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Item already added in this Incident Report"))
                {
                    resp = new { status = 0, message = "Item already added in this Incident Report" };
                }
                 else if (ex.Message.Contains("Incorrect Promotion Type"))
                {
                    resp = new { status = 0, message = "Incorrect Promotion Type" };
                }
                else if (ex.Message.Contains("Item does not exist in this transaction"))
                {
                    resp = new { status = 0, message = "Item does not exist in this transaction" };
                }
                else if (ex.Message.Contains("Actual Quantity is same with the recent Quantity"))
                {
                    resp = new { status = 0, message = "Actual Quantity is same with the recent Quantity" };
                }
                else
                {
                    resp = new { status = 2, message = ex.Message };
                }
            }
            resp = JsonConvert.SerializeObject(resp, Formatting.Indented);
            return resp.ToString();
        }

        [WebMethod()]
        public static string RemoveItem(int irDetailId)
        {
            object resp = null;
            try
            {
                IncidentReport ir = new IncidentReport();
                ir.RemoveItem(irDetailId);
                resp = new { status = 1, message = "Item successfully deleted" };
            }
            catch (Exception ex)
            {
                resp = new { status = 2, message = ex.Message };
            }
            resp = JsonConvert.SerializeObject(resp, Formatting.Indented);
            return resp.ToString();
        }

        [WebMethod()]
        public static string ModifyItem(int irDetailId, int actualQty)
        {
            object resp = null;
            try
            {
                IncidentReport ir = new IncidentReport();                
                resp = new { status = 1, message = "Item successfully updated", data = ir.ModifyItem(irDetailId, actualQty)};
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Actual Quantity is same with the recent Quantity"))
                {
                    resp = new { status = 0, message = "Actual Quantity is same with the recent Quantity" };
                }
                else
                {
                    resp = new { status = 2, message = ex.Message };
                }
            }
            resp = JsonConvert.SerializeObject(resp, Formatting.Indented);
            return resp.ToString();
        }
    }
}
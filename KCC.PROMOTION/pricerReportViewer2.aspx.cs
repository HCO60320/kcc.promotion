using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Data;
using promotions;
using System.IO;

public partial class pricerReportViewer : System.Web.UI.Page
{
    private ReportDocument rpt;
    private ReportDocument rpt1;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Redirect("pricerReportViewer.aspx");
     //   if (Session["username"] == null) {
     //       Response.Redirect("login.aspx");
     //   } else {
     //       string userRole = Session["rolename"].ToString();

     //       if (userRole == "KCC_PM_BYR") {
     //           Response.Redirect("buyerDashboard.aspx");
     //       } else if (userRole == "KCC_PM_SA") {
     //           Response.Redirect("sellingAreaDashboard.aspx");
     //       } else if (userRole == "KCC_PM_UM") {
     //           Response.Redirect("");
     //       }

     //       //-----------
     //       if (userRole == "KCC_PM_RM") {
     //           //lnkReprintManager.Visible = true;
     //       }
     //       //-----------
     //   }
        
     //  string headId = Request.QueryString["headId"];

     //  //hfHeadId.Value = headId;
     //// variable declaration
     //   DataTable dtTransactionDetail = new DataTable();
     //   DataTable dtTransactionItems = new DataTable();
     //   DataTable dtpromodate = new DataTable();
     //   DataTable dtTransactionItems_freelist = new DataTable();
     //   DataTable dtTransactionItems_multibuy = new DataTable();

     //   rpt = new ReportDocument();
     //   rpt1 = new ReportDocument();

     //   generalModelClass generalModelObj = new generalModelClass();
     //   pricerModelClass prcmodelobj = new pricerModelClass();

     ////datatable fillings
     //   dtTransactionDetail = generalModelObj.get_transaction_detail(Convert.ToInt32(headId));
     //   dtTransactionItems = prcmodelobj.get_prc_items_transaction(headId);
     //   dtpromodate = prcmodelobj.get_promo_dates(headId);
     //   string promo_type = "";

     ////identifying promotion type and setting its parameter
     //      promo_type = dtTransactionDetail.Rows[0]["PROMO_TYPE"].ToString();
     //       if (promo_type == "0"){
     //         promo_type = "Multi-buy";}
     //       else if (promo_type == "1"){ 
     //           promo_type = "Simple";}
     //       else if (promo_type == "2"){
     //           promo_type = "Threshold";}
                  
     //       if (promo_type == "Simple"){ 
     //           rpt.FileName = Server.MapPath("~/Reports/prcPrintOut_Simple.rpt"); }
     //       else if (promo_type == "Threshold"){ 
     //           rpt.FileName = Server.MapPath("~/Reports/prcPrintOut_Threshold.rpt"); }
     //       else if (promo_type == "Multi-buy"){ 
     //           rpt.FileName = Server.MapPath("~/Reports/prcPrintOut_Multibuy.rpt");
     //       //    rpt1.FileName = Server.MapPath("~/Reports/prcPrintOut_freelist.rpt");
     //       }
     ////populate items and promo dates
     //       if (promo_type == "Simple" || promo_type == "Threshold") {
     //          rpt.SetDataSource(dtTransactionItems);
     //          rpt.Subreports[0].DataSourceConnections.Clear();
     //          rpt.Subreports[0].SetDataSource(dtpromodate);  
        
     //       } else if (promo_type.ToUpper() == "MULTI-BUY") {
     //          dtTransactionItems_freelist = prcmodelobj.get_prc_items_freelist(headId);
     //          dtTransactionItems_multibuy = prcmodelobj.get_prc_items_notfl(headId);
               
     //          rpt.SetDataSource(dtTransactionItems_multibuy);
              

     //          rpt.Subreports["date"].DataSourceConnections.Clear();
     //          rpt.Subreports["date"].SetDataSource(dtpromodate);

     //          rpt.Subreports["freelist"].DataSourceConnections.Clear();
     //          rpt.Subreports["freelist"].SetDataSource(dtTransactionItems_freelist);
     //       }

     ////Head Parameter
     //   rpt.SetParameterValue("param_tranno", dtTransactionDetail.Rows[0]["TRAN_ID"].ToString());
     //    //report for simple and threshold have no 'param_itemlist_freelist' parameter
     //   if (dtTransactionDetail.Rows[0]["PROMO_TYPE"].ToString() == "0") {
     //       if (dtTransactionDetail.Rows[0]["HAS_FREELIST"].ToString() == "1") {
     //           rpt.SetParameterValue("param_itemlist_freelist", dtTransactionDetail.Rows[0]["ITEM_LIST_NO_FREELIST"].ToString());
     //       } else {
     //           rpt.SetParameterValue("param_itemlist_freelist","");
     //       }
     //   }
       
     //   rpt.SetParameterValue("param_itemlist", dtTransactionDetail.Rows[0]["ITEM_LIST_NO"].ToString());
     //   rpt.SetParameterValue("param_loc", dtTransactionDetail.Rows[0]["LOC_NAME"].ToString());
     //   rpt.SetParameterValue("param_dep", dtTransactionDetail.Rows[0]["DEPT_NAME"].ToString());
     //   rpt.SetParameterValue("param_prepared", dtTransactionDetail.Rows[0]["PREPARED_BY"].ToString());
     //   rpt.SetParameterValue("param_submitted", dtTransactionDetail.Rows[0]["SUBMITTED_BY"].ToString());
     //   rpt.SetParameterValue("param_approved", dtTransactionDetail.Rows[0]["APPROVED_BY"].ToString());
     //   rpt.SetParameterValue("param_promo_type", promo_type.ToString());
     //   rpt.SetParameterValue("param_remarks", dtTransactionDetail.Rows[0]["PROMOLIST_DESC"].ToString());
     //   //CrystalReportViewer1.ReportSource = rpt;
    }

    protected void printme_Click(object sender, EventArgs e) {
        basegeneral obj = new basegeneral();
        int headId = 0; 
        generalModelClass generalModelObj = new generalModelClass();
        adminModelClass adminModelObj = new adminModelClass();
        try {
            string userRole = Session["rolename"].ToString();
            headId = Convert.ToInt32(Request.QueryString["headId"]);
            string username = HttpContext.Current.Session["username"].ToString();

            DataTable dtReprintRequest = new DataTable();
            dtReprintRequest = adminModelObj.getReprintTransaction(headId);

            if (dtReprintRequest.Rows.Count > 0) {
                //this means that this transaction will be reprinted
                adminModelObj.updateReprintStatus(headId, "REPRINTED", ref obj);
            } else {
                //first print
                pricerModelClass pricerModelObj = new pricerModelClass();
                pricerModelObj.update_printedby(headId, username, ref obj);
            }

            obj.Commit();
        } catch (Exception ex) {
            obj.Rollback();
        } finally {
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/doc";
            Response.ClearContent();
            Response.ClearHeaders();
            DateTime sysDate = Convert.ToDateTime(generalModelObj.getSysdate());
            rpt.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "promotion(pricer)" + headId.ToString() + "-" + sysDate.ToString("ddMMyy"));

        }
    }
}
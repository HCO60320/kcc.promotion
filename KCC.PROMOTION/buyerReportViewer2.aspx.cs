using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using System.Data;
using CrystalDecisions.Shared;
using promotions;
using System.IO;

public partial class buyerReportViewer : System.Web.UI.Page
{
    private ReportDocument rpt;
    //private ReportDocument rpt1;

    protected void Page_Load(object sender, EventArgs e) {
        Response.Redirect("buyerReportViewer.aspx");

     //   string headId = Request.QueryString["headId"];
     //   //hfHeadId.Value = headId;
     //   // variable declaration
     //   DataTable dtTransactionDetail = new DataTable();
     //   DataTable dtTransactionItems = new DataTable();
     //   DataTable dtpromodate = new DataTable();
     //   DataTable dtTransactionItems_freelist = new DataTable();
     //   DataTable dtTransactionItems_multibuy = new DataTable();

     //  rpt = new ReportDocument();
     ////  rpt1 = new ReportDocument();

     //   generalModelClass generalModelObj = new generalModelClass();
     //   pricerModelClass prcmodelobj = new pricerModelClass();

     //   //datatable fillings
     //   dtTransactionDetail = generalModelObj.get_transaction_detail(Convert.ToInt32(headId));
     //   dtTransactionItems = prcmodelobj.get_prc_items_transaction(headId);
     //   dtpromodate = prcmodelobj.get_promo_dates(headId);
     //   string promo_type = "";

     //   //identifying promotion type and setting its parameter
     //   promo_type = dtTransactionDetail.Rows[0]["PROMO_TYPE"].ToString();
     //   if (promo_type == "0") {
     //       promo_type = "Multi-buy";
     //   } else if (promo_type == "1") {
     //       promo_type = "Simple";
     //   } else if (promo_type == "2") {
     //       promo_type = "Threshold";
     //   }

     //   if (promo_type == "Simple") {
     //       rpt.FileName = Server.MapPath("~/Reports/byrPrintOut_Simple.rpt");
     //   } else if (promo_type == "Threshold") {
     //       rpt.FileName = Server.MapPath("~/Reports/byrPrintOut_Threshold.rpt");
     //   } else if (promo_type == "Multi-buy") {
     //       rpt.FileName = Server.MapPath("~/Reports/byrPrintOut_Multibuy.rpt");
     //       //    rpt1.FileName = Server.MapPath("~/Reports/prcPrintOut_freelist.rpt");
     //   }
     //   //populate items and promo dates
     //   if (promo_type.ToUpper() == "SIMPLE" || promo_type.ToUpper() == "THRESHOLD") {
     //       rpt.SetDataSource(dtTransactionItems);
     //       rpt.Subreports[0].DataSourceConnections.Clear();
     //       rpt.Subreports[0].SetDataSource(dtpromodate);
     //   } else if (promo_type.ToUpper() == "MULTI-BUY") {
     //       // Identify if multi-buy has freelist
     //       string has_prm_list = "";
     //       has_prm_list = dtTransactionDetail.Rows[0]["HAS_FREELIST"].ToString();
     //       rpt.Subreports["date"].DataSourceConnections.Clear();
     //       rpt.Subreports["date"].SetDataSource(dtpromodate);
     //       dtTransactionItems_multibuy = prcmodelobj.get_prc_items_notfl(headId);
     //       dtTransactionItems_freelist = prcmodelobj.get_prc_items_freelist(headId);

     //        rpt.SetDataSource(dtTransactionItems_multibuy);
     //        rpt.Subreports["freelist"].DataSourceConnections.Clear();
     //        rpt.Subreports["freelist"].SetDataSource(dtTransactionItems_freelist);
     //   }

     //   //Head Parameter
     //   rpt.SetParameterValue("param_tranno", dtTransactionDetail.Rows[0]["TRAN_ID"].ToString());
     //   rpt.SetParameterValue("param_loc", dtTransactionDetail.Rows[0]["LOC_NAME"].ToString());
     //   rpt.SetParameterValue("param_dep", dtTransactionDetail.Rows[0]["DEPT_NAME"].ToString());
     //   rpt.SetParameterValue("param_prepared", dtTransactionDetail.Rows[0]["PREPARED_BY"].ToString());
     //   rpt.SetParameterValue("param_submitted", dtTransactionDetail.Rows[0]["SUBMITTED_BY"].ToString());
     //   rpt.SetParameterValue("param_promo_type", promo_type.ToString());
     //   rpt.SetParameterValue("param_remarks", dtTransactionDetail.Rows[0]["PROMOLIST_DESC"].ToString());
     //   //CrystalReportViewer1.ReportSource = rpt;

    }

    protected void printme_Click(object sender, EventArgs e) {
          basegeneral obj = new basegeneral();
          generalModelClass generalModelObj = new generalModelClass();
          int headId = 0; 
          try {
              headId = Convert.ToInt32(Request.QueryString["headId"]);
              string username = HttpContext.Current.Session["username"].ToString();
              //string pathName = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\promotions";
              buyerModelClass buyerModelObj = new buyerModelClass();
             
              buyerModelObj.update_printedby(headId, username, ref obj);
              generalModelObj.update_transaction_status(headId, "PRINTED", ref obj);

              //if (!Directory.Exists(pathName)) {
              //    Directory.CreateDirectory(pathName);
              //}

              //ExportOptions ex = new ExportOptions();
              //DiskFileDestinationOptions destinationURL = new DiskFileDestinationOptions();
              //PdfRtfWordFormatOptions formatOptionPDF = new PdfRtfWordFormatOptions();
              //destinationURL.DiskFileName = pathName + "\\promotion(buyer)" + headId + ".pdf";
              //ex.ExportDestinationType = ExportDestinationType.DiskFile;
              //ex.ExportFormatType = ExportFormatType.PortableDocFormat;
              //ex.ExportDestinationOptions = destinationURL;
              //ex.ExportFormatOptions = formatOptionPDF;
              //rpt.Export(ex);


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
              rpt.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "promotion(buyer)" + headId.ToString() + "-" + sysDate.ToString("ddMMyy"));
          }

       
    }
}
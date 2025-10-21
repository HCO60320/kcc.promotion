namespace KCC.PROMOTION
{
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
    using KCC.PROMOTION.Model;

    public partial class buyerReportViewer : System.Web.UI.Page
    {
        private ReportDocument rpt;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["username"] == null)
            {
                Response.Redirect("login.aspx");
            }
            else
            {
                int userID = Convert.ToInt32(Session["userId"]);
                DataTable dtUserRole = new DataTable();
                userModelClass userModel = new userModelClass();
                dtUserRole = userModel.get_user_role(Convert.ToInt32(Session["applicationId"]), userID);

                if (dtUserRole.Rows.Count < 1)
                {
                    Response.Redirect("login.aspx");
                }
                else
                {
                    string userRole = dtUserRole.Rows[0]["ROLE_NAME"].ToString();
                    HttpContext.Current.Session.Add("rolename", userRole);

                    if (userRole == "KCC_PM_SA")
                    {
                        Response.Redirect("SellingAreaDashboard.aspx");
                    }
                    else if (userRole == "KCC_PM_PRC")
                    {
                        Response.Redirect("PricerDashboard.aspx");
                    }
                    else if (userRole == "KCC_PM_UM")
                    {
                        Response.Redirect("reprintmanager.aspx");
                    }
                    else if (userRole == "KCC_PM_RM")
                    {
                        Response.Redirect("reprintmanager.aspx");
                    }
                }
            }

            string headId = Request.QueryString["headId"];
            hfHeadId.Value = headId;

            // variable declaration
            DataTable dtTransactionDetail = new DataTable();
            DataTable dtTransactionItems = new DataTable();
            DataTable dtpromodate = new DataTable();
            DataTable dtTransactionItems_freelist = new DataTable();
            DataTable dtTransactionItems_multibuy = new DataTable();

            rpt = new ReportDocument();
            //  rpt1 = new ReportDocument();

            generalModelClass generalModelObj = new generalModelClass();
            pricerModelClass prcmodelobj = new pricerModelClass();

            //datatable fillings
            dtTransactionDetail = generalModelObj.get_transaction_detail(Convert.ToInt32(headId));
            dtTransactionItems = prcmodelobj.get_prc_items_transaction(headId);
            dtpromodate = prcmodelobj.get_promo_dates(headId);
            string promo_type = "";

            //identifying promotion type and setting its parameter
            promo_type = dtTransactionDetail.Rows[0]["PROMO_TYPE"].ToString();

            if (promo_type == "0")
            {
                promo_type = "Multi-buy";
            }
            else if (promo_type == "1")
            {
                promo_type = "Simple";
            }
            else if (promo_type == "2")
            {
                promo_type = "Threshold";
            }

            if (promo_type == "Simple")
            {
                rpt.FileName = Server.MapPath("~/Reports/byrPrintOut_Simple.rpt");
            }
            else if (promo_type == "Threshold")
            {
                rpt.FileName = Server.MapPath("~/Reports/byrPrintOut_Threshold.rpt");
            }
            else if (promo_type == "Multi-buy")
            {
                rpt.FileName = Server.MapPath("~/Reports/byrPrintOut_Multibuy.rpt");
                //    rpt1.FileName = Server.MapPath("~/Reports/prcPrintOut_freelist.rpt");
            }

            //populate items and promo dates
            if (promo_type.ToUpper() == "SIMPLE" || promo_type.ToUpper() == "THRESHOLD")
            {
                rpt.SetDataSource(dtTransactionItems);
                rpt.Subreports[0].DataSourceConnections.Clear();
                rpt.Subreports[0].SetDataSource(dtpromodate);
            }
            else if (promo_type.ToUpper() == "MULTI-BUY")
            {
                // Identify if multi-buy has freelist
                string has_prm_list = "";
                has_prm_list = dtTransactionDetail.Rows[0]["HAS_FREELIST"].ToString();
                rpt.Subreports["date"].DataSourceConnections.Clear();
                rpt.Subreports["date"].SetDataSource(dtpromodate);
                dtTransactionItems_multibuy = prcmodelobj.get_prc_items_notfl(headId);
                dtTransactionItems_freelist = prcmodelobj.get_prc_items_freelist(headId);

                rpt.SetDataSource(dtTransactionItems_multibuy);
                rpt.Subreports["freelist"].DataSourceConnections.Clear();
                rpt.Subreports["freelist"].SetDataSource(dtTransactionItems_freelist);
            }

            //Head Parameter
            rpt.SetParameterValue("param_tranno", dtTransactionDetail.Rows[0]["TRAN_ID"].ToString());
            rpt.SetParameterValue("param_loc", dtTransactionDetail.Rows[0]["LOC_NAME"].ToString());
            rpt.SetParameterValue("param_dep", dtTransactionDetail.Rows[0]["DEPT_NAME"].ToString());
            rpt.SetParameterValue("param_prepared", dtTransactionDetail.Rows[0]["PREPARED_BY"].ToString());
            rpt.SetParameterValue("param_submitted", dtTransactionDetail.Rows[0]["SUBMITTED_BY"].ToString());
            rpt.SetParameterValue("param_promo_type", promo_type.ToString());
            rpt.SetParameterValue("param_remarks", dtTransactionDetail.Rows[0]["PROMOLIST_DESC"].ToString());

            // Footer Dates
            if (dtTransactionDetail.Rows[0]["PREPARED_DATE"].ToString() == string.Empty)
            {
                this.rpt.SetParameterValue("param_create_date", string.Empty);
            }
            else
            {
                this.rpt.SetParameterValue("param_create_date", Convert.ToDateTime(dtTransactionDetail.Rows[0]["PREPARED_DATE"]).ToString("dd-MMM-yyyy").ToUpper());
            }

            if (dtTransactionDetail.Rows[0]["SUBMITTED_DATE"].ToString() == string.Empty)
            {
                this.rpt.SetParameterValue("param_submitted_date", string.Empty);
            }
            else
            {
                this.rpt.SetParameterValue("param_submitted_date", Convert.ToDateTime(dtTransactionDetail.Rows[0]["SUBMITTED_DATE"]).ToString("dd-MMM-yyyy").ToUpper());
            }

            rpt.SetParameterValue("param_group_man", dtTransactionDetail.Rows[0]["GROUP_MANAGER"]);
            rpt.SetParameterValue("param_division_man", dtTransactionDetail.Rows[0]["DIVISION_MANAGER"]);
            this.rpt.SetParameterValue("param_print_date", Convert.ToDateTime(dtTransactionDetail.Rows[0]["VDATE"]).ToString("dd-MMM-yyyy").ToUpper());

            CrystalReportViewer1.ReportSource = rpt;
        }

        /// <summary>
        /// Page Unload
        /// </summary>
        /// <param name="sender">Parameter Sender</param>
        /// <param name="e">Parameter Event Args</param>
        protected void Page_Unload(object sender, EventArgs e)
        {
            this.rpt.Close();
            this.rpt.Dispose();
            this.CrystalReportViewer1.Dispose();
            GC.Collect();
        }

        protected void printme_Click(object sender, EventArgs e)
        {
            basegeneral obj = new basegeneral();
            DataTable dtTranHead = new DataTable();
            generalModelClass generalModelObj = new generalModelClass();
            buyerModelClass buyerModelObj = new buyerModelClass();
            pricerModelClass prcmodelobj = new pricerModelClass();

            JsonResponse appManagerResponse = new JsonResponse();
            MerchAppManager.AutoEmailLogsClient autoEmail = new MerchAppManager.AutoEmailLogsClient();
            MerchAppManager.OutTownModeClient outTownMode = new MerchAppManager.OutTownModeClient();

            int headId = 0;
            try
            {
                headId = Convert.ToInt32(Request.QueryString["headId"]);
                string username = HttpContext.Current.Session["username"].ToString();
                //string pathName = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\promotions";

                // Fetch transaction status
                DataTable dtTransactionDetail = generalModelObj.get_transaction_detail(headId);

                if (dtTransactionDetail.Rows.Count > 0)
                {
                    string transactionStatus = dtTransactionDetail.Rows[0]["Status"].ToString();

                    if (transactionStatus == "WORKSHEET")
                    {
                        try
                        {
                            // Try running the ExportFile method separately
                            ExportFile(generalModelObj.getSysdate(), headId.ToString());
                        }
                        catch (Exception exportEx)
                        {
                            Console.WriteLine("Error during ExportFile: " + exportEx.Message);
                            throw;  // Rethrow to capture in the main catch block if needed
                        }
                    }
                }

                MerchAppManager.ConnectionModel credential = new MerchAppManager.ConnectionModel();
                MerchAppManager.AutoEmailModel emailModel = new MerchAppManager.AutoEmailModel();

                
                    credential.Username = "merch_app_manager";
                    credential.Password = "m3rchappmanag3r";
                

                dtTranHead = generalModelObj.get_transaction_detail(headId);
                if (dtTranHead.Rows.Count > 0)
                {
                    int itemCount = 0;
                    string promo_type = "";

                    promo_type = dtTranHead.Rows[0]["PROMO_TYPE"].ToString();
                    if (promo_type == "0" || promo_type == "2")
                    {
                        if (promo_type == "0")
                        {
                            promo_type = "MULTI-BY";
                        }
                        else
                        {
                            promo_type = "THRESHOLD";
                        }

                        itemCount = prcmodelobj.get_prc_items_transaction(headId.ToString()).Rows.Count;
                        emailModel.MessageContent = string.Format("\t Location: {0} \n \t Department: {1} \n \t Promotion Type: {2} \n \t No. of Item(s): {3} \n \t Remarks: {4} ",
                                                dtTranHead.Rows[0]["LOC_NAME"].ToString(),
                                                dtTranHead.Rows[0]["DEPT_NAME"].ToString(),
                                                promo_type,
                                                itemCount,
                                                dtTranHead.Rows[0]["PROMOLIST_DESC"].ToString());
                    }
                    else if (promo_type == "1")
                    {
                        DataTable dtPromoItem = new DataTable();
                        dtPromoItem = prcmodelobj.get_item_promo_type(headId.ToString());
                        //emailModel.MessageContent = string.Format("\t Location: {0} \n \t Department: {1} \n \t Promotion Type: {2} \n \t Remarks: {3} ",
                        //                        dtTranHead.Rows[0]["LOC_NAME"].ToString(),
                        //                        dtTranHead.Rows[0]["DEPT_NAME"].ToString(),
                        //                        "SIMPLE",
                        //                        dtTranHead.Rows[0]["PROMOLIST_DESC"].ToString());
                        string items = string.Empty;
                        for (int x = 0; x < dtPromoItem.Rows.Count; x++)
                        {
                            items += string.Format("\n No. of Item(s): {0} - Discount Type: {1} - Discount: {2}% ",
                                                dtPromoItem.Rows[x]["ITEM_COUNT"].ToString(),
                                                dtPromoItem.Rows[x]["CLR_TYPE_DESC"].ToString(),
                                                dtPromoItem.Rows[x]["DISCOUNT"].ToString());
                        }
                        emailModel.MessageContent += items + " \n";
                    }

                    emailModel.TranNo = Convert.ToInt32(dtTranHead.Rows[0]["TRAN_ID"].ToString());
                    emailModel.ApplicationId = Convert.ToInt32(HttpContext.Current.Session["applicationId"].ToString());
                    emailModel.RequestedBy = HttpContext.Current.Session["username"].ToString();
                    emailModel.CreatedBy = Convert.ToInt32(HttpContext.Current.Session["userid"].ToString());

                    appManagerResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonResponse>(outTownMode.GetOutTownModeApp(credential, emailModel.ApplicationId));
                    if (appManagerResponse.Data.Rows.Count > 0)
                    {
                        buyerModelObj.update_printedby(headId, username, ref obj);
                        generalModelObj.update_transaction_status(headId, "PRINTED", ref obj);

                        if (appManagerResponse.Data.Rows[0]["STATUS"].ToString() == "1")
                        {
                           
                            appManagerResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonResponse>(autoEmail.GetAppTranRequest(credential, emailModel.ApplicationId, emailModel.TranNo));
                            if (appManagerResponse.Data.Rows.Count > 0)
                            {
                                if (appManagerResponse.Data.Rows[0]["STATUS"].ToString() != "SENT")
                                {
                                  
                                    appManagerResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonResponse>(autoEmail.AddAutoEmailLogs(credential, emailModel));
                                    if (appManagerResponse.Status == 1)
                                    {
                                        buyerModelObj.emailTriggerSend(emailModel.TranNo);
                                        obj.Commit();
                                        ExportFile(generalModelObj.getSysdate(), headId.ToString());
                                    }
                                    else
                                    {
                                        obj.Rollback();
                                    }
                                }
                            }
                            else
                            {
                               
                                appManagerResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonResponse>(autoEmail.AddAutoEmailLogs(credential, emailModel));
                                if (appManagerResponse.Status == 1)
                                {
                                    buyerModelObj.emailTriggerSend(emailModel.TranNo);
                                    obj.Commit();
                                    ExportFile(generalModelObj.getSysdate(), headId.ToString());
                                }
                                else
                                {
                                    obj.Rollback();
                                }
                            }
                        }
                        else
                        {
                            obj.Commit();
                            ExportFile(generalModelObj.getSysdate(), headId.ToString());
                        }
                    }
                }





                //buyerModelObj.update_printedby(headId, username, ref obj);
                //generalModelObj.update_transaction_status(headId, "PRINTED", ref obj);

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

                //obj.Commit();

            }
            catch (Exception ex)
            {
                obj.Rollback();
            }
            //finally
            //{
            //    Response.Clear();
            //    Response.Buffer = true;
            //    Response.ContentType = "application/doc";
            //    Response.ClearContent();
            //    Response.ClearHeaders();
            //    DateTime sysDate = Convert.ToDateTime(generalModelObj.getSysdate());
            //    rpt.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "promotion(buyer)" + headId.ToString() + "-" + sysDate.ToString("ddMMyy"));
            //}
        }

        private void ExportFile(string systemDate, string headId)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/doc";
            Response.ClearContent();
            Response.ClearHeaders();
            DateTime sysDate = Convert.ToDateTime(systemDate);
            rpt.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "promotion(buyer)" + headId + "-" + sysDate.ToString("ddMMyy"));
        }

    }
}
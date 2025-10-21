<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="buyerReportViewer.aspx.cs" Inherits="KCC.PROMOTION.buyerReportViewer" %>
<%@ Register assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="crystalreportviewers13/js/crviewer/crv.js" type="text/javascript"></script>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <ul id="breadcrumb">
          <li>
              <a href="buyerDashBoard.aspx"  title="Home"><img src="./images/home.png" alt="Home" class="home" /></a>
          </li>
          <li>
              <a id='lnkTransactionDetail' href="#">Transaction Detail</a>
          </li> 
          <li>
              <a href="#">Print Preview</a>
          </li>            
       </ul>
    </div>
     <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" 
            EnableDatabaseLogonPrompt = "false"
            AutoDataBind="true" BestFitPage="False" ToolPanelView="None" ViewStateMode="Enabled" 
            PrintMode= "ActiveX"
            Width="100%" Height="700"  />
    
    <asp:Button ID="printme" runat="server"  OnClick="printme_Click" Text= "" style="display:none;" />
    </form>

    <input type="hidden" id="hfHeadId" value="" runat="server"/>
    <script src="Scripts/jquery.min.js" type="text/javascript"></script>
    <script src="Scripts/buyerReportViewer.js" type="text/javascript"></script>
</body>
</html>

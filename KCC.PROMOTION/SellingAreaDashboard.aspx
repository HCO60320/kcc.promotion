<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="SellingAreaDashboard.aspx.cs" Inherits="SellingAreaDashboard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <title>Dashboard</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div id="Div1"  class="SellingOutline">
        <div id= "BuyerDashBoardBorder" class="SellingBack">    
            http://192.168.34.197/KCSRETAIL/dev/Dashboard/Dashboard
        </div>                  
        <div id="BuyerDashBoardAreaBody" class="SellingMainBody">
            <div class="BuyerDashBoardMainHead">
                <table width="100%">
                    <tr>
                        <td width="50%" colspan="4" style="text-align:left;" >
                            <img src="Images/promotion.png" alt="ERROR LOADING IMAGE">
                        </td>
                        <td style="width:50%;" >
                            <table style="text-align:right;width:100%;height:80px;">
                                <tr>
                                    <td style="text-align:right;padding-bottom:63px">
                                    <a id='lnkLogoutCntr' href="#" >Logout</a>
                                    <asp:LinkButton ID="lnklogout"  ClientIDMode="Static" runat="server" OnClick="lnklogout_Click" style="display:none;">Logout</asp:LinkButton>
                                    <%-- <asp:LinkButton ID="lnklogout" runat="server" OnClick="lnklogout_Click">Logout</asp:LinkButton>--%>
                                    <%--<a class="logout" href="#" onclick="Log_out();">Log-out</a>--%>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>                     
            </div>
            <div style="height:85%; ">
                <table width="100%" cellpadding="0" cellspacing="0">
<%--                    <tr style="text-align:center;">                          
                        <td>
                            <input id="txtfilter_md_sa" type="text"class="BarcodeSearch" placeholder="Search" style="text-align:left;" onkeyup="filter(this,'databody');get_total_items_sa();" tabindex = '1' />
                        </td>
                    </tr>--%>
                    <tr>
                        <td>
                            <div>
                                     <ul id="breadcrumb">
                                      <li>
                                        <%--<a href="http://192.168.33.197/KCSRETAIL/DEV/Dashboard/Dashboard"  title="KCSRMS">KCSRETAIL</a>--%>
                                          <a href="http://192.168.34.197/KCSRMS/dev/Dashboard/Dashboard"  title="KCSRMS">KCSRETAIL</a>
                                    </li>    

                                    <li>
                                        <a href="#" onclick ="page_redirect('SellingAreaDashboard.aspx')" title="Home"><img src="./images/home.png" alt="Home" class="home" /></a>
                                    </li>              
                                 </ul>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td> 
                               <select id="setID" style="float:right; height:36px; vertical-align: middle; ">
                            <option selected="true" disabled="disabled">SET</option>
                              <option id="btnSearchTran">Transaction</option>
                              <option id="btnSearchLocation">Location</option>
                              <option id="btnSearchDepartment">Department</option>
                              <option id="btnSearchOrin">ORIN</option>
                              <option id="btnSearchBarcode">BARCODE</option>
                            </select> 
                            <input id="txtFilterExecPR" type="text" tabindex="1" placeholder="Search" class="form-control" style="float: right;width: 200px; height: 30px; font-size: 1.5em; font-weight: 600;">
                            <%-- <div class="row" style="margin-top: -10px; margin-right: 8px;">
                            <div class="col-sm-9 col-md-9 col-lg-9"></div>
                            <div class="col-sm-3 col-md-3 col-lg-3">
                                <div class="icon-addon addon-md">
                                    <div class="input-group">
                                        <input id="txtFilterExecPR" type="text" tabindex="1" placeholder="Search" class="form-control" style="width: 100%; font-size: 0.76em; font-weight: 600;">
                                        <div class="input-group-btn">
                                            <button class="btn btn-primary dropdown-toggle" type="button" tabindex="2" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                                SET
                                            </button>
                                            <ul class="dropdown-menu dropdown-menu-right" style="left: auto;">
                                                <li>
                                                    <a id="btnSearchTran" tabindex="3" class="dropdown-item">Transaction</a>
                                                </li>
                                                <li>
                                                    <a id="btnSearchLocation" tabindex="4" class="dropdown-item">Location</a>
                                                </li>
                                                <li>
                                                    <a id="btnSearchDepartment" tabindex="5" class="dropdown-item">Department</a>
                                                </li>
                                                <li>
                                                    <a id="btnSearchOrin" tabindex="6" class="dropdown-item">ORIN</a>
                                                </li>
                                                <li>
                                                    <a id="btnSearchBarcode" tabindex="7" class="dropdown-item">BARCODE</a>
                                                </li>
                                            </ul>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>    --%>
                        <div style="height:435px;">  
                            <table id="tblTransactions" class="display" style="text-align:center" cellspacing="0" width="100%">
                                <thead>
                                    <tr>
                                        <th>TRAN NO</th>
                                        <th>LOCATION</th>
                                        <th>DEPARTMENT</th>
                                        <th>TYPE</th>
                                        <th>STATUS</th>
                                        <th>REMARKS</th>
                                    </tr>
                                </thead>
                            </table>
                        </div>
                         <div id="Div3" class="gridfoot" style="height:38px;" >
                                    <table width="100%">
                                        <tr>
                                            <td  style="text-align:left;padding-left:10px; font-size:15px;">
                                                <span>Total Promotions:</span>
                                                 <label id="lblTotalTransCount"></label>
                                            </td>
                                            <td  style="text-align:right;padding-left:10px; font-size:15px;">
                                                <input id="btnIR" type="button" value="Send IR" class="button-primary" tabindex='1'/>
                                                <input id="btnNew" type="button" value="New" class="button-primary" onclick="open_new_transaction();" tabindex='2'/>
                                                
                                            </td>
                                        </tr>
                                    </table>                                 
                         </div>
                            
                            <%--<div id="Div2" >
                                <table class="gridHead" width="100%"  cellpadding="0" cellspacing="0" style="height:35px;" >
                                    <tr>
                                        <td style="width:14%; padding:7px 0 7px 0;">
                                            <span>TRAN NO</span>
                                        </td>
                                        <td style="width:14%; padding:7px 0 7px 0;">
                                            <span>LOCATION</span>
                                        </td>
                                        <td style="width:14%; padding:7px 0 7px 0;">
                                            <span>DEPARTMENT</span>
                                        </td>
                                        <td style="width:14%; padding:7px 0 7px 0;">
                                            <span>SUPPLIER</span>    
                                        </td>
                                        <td style="width:14%; padding:7px 0 7px 0;">
                                            <span>BRAND</span>    
                                        </td>
                                        <td style="width:14%; padding:7px 0 7px 0;">
                                            <span>STATUS</span>    
                                        </td>     
										<td style="width:7%; padding:7px 0 7px 0;">
                                            <span>REMARKS</span>    
                                        </td> 										
                                    </tr>
                                </table>
                                <div style="height:358px;overflow:auto;">  
                                    <table id='databody' class='gridBody' cellpadding ="0" cellspacing = "0">
                                
                                    </table>         
                                </div>
                           </div>--%>
                        </td>                        
                    </tr>
                </table>
            </div>
        </div> 
    </div>
   
    <%--Remarks Popup--%>
   <div id="PopUpRemarks"  class="modalPage" >
        <div id= "PopBackground" class="modalBackground"></div>  
        <div id= "PopContainer" class="modalContainer" style="margin: 180px 27% 15%;">
            <div id='PopupContainerHead' class="modalhead" style="height:20%;" >
                <table>
                    <tr>
                        <td style="padding:15px 0 0 15px;">
                            <span>
                                PROMOTION REMARKS
                            </span>
                        </td>
                    </tr>
                </table>                
            </div>
            <div id='PopupContainerBody' class="modalbody" style="height:59%;" >
                <table>
                    <tr>
                        <td>
                            <textarea id="txtPromoDescription" 
                                   placeholder="Remarks"
                                   class="inputs"
                                   style="width: 387px; height: 113px; 
                                   font-family: Helvetica,sans-serif,Segoe UI,Helvetica Neue;
                                   font-size:14px;
								   resize: none;"
								   disabled="disabled"
                            ></textarea>
                        </td>   
                    </tr>                    
                </table>
            </div>
            <div id='PopupContainerFoot' class="modalfoot" style="height:20%;" >
                <table style="width:100%;" >
                    <tr>                                         
                        <td style="text-align:right;">
                            <input id="btnCloseRemarks" type="button" value="Close" class="button-primary" onclick="hideModal('PopUpRemarks');" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>  
    </div>  <%--end of Remarks Popup--%>

        <%--IR Dashboard--%>
    <div id="popupIncidentReport"  class="modalPage" >
        <div id= "IRbg" class="modalBackground"></div>  
        <div id= "IRcontainer" class="modalContainer" style="height: 350px;width:400px; margin: 150px 26% 15%;">
            <div id='IRhead' class="modalhead" style="height:13%;" >
                <table style="width:100%">
                    <tr >
                        <td style="width:30%;padding:0px 0 0 15px;">
                            <span>
                                REPRINT REQUEST
                            </span>
                        </td>
                        <td style="width:60%;padding:0px 20px 0 0px;" >
                            <input id="txtFilter_IR" type="text"class="BarcodeSearch" placeholder="Search" style="text-align:left;width:100%;height:16px;" tabindex = '1' />
                        </td>
                    </tr>
                </table>                
            </div>
            <div id='IRbody' class="modalbody" style="height:73%;" >
                <div style="height:100px;">
                     <table id="tblReprintTransactions" class="display" style="text-align:center" cellspacing="0" width="100%">
                        <thead>
                            <tr>
                                <th>TRAN NO</th>
                                <th>STATUS</th>
                            </tr>
                        </thead>
                    </table>
                </div>
            </div>
            <div id='IRFoot' class="modalfoot" style="height:13%;" >
                <table style="width:100%;" >
                    <tr>                                        
                        <td style="text-align:right;">
                            <input id="cur_head_id" type ="hidden"/>
                            <input id="cur_edate" type ="hidden"/>
                            <input id="cur_rdate" type ="hidden"/>

                            <%--<input id="btnReprint" type="button" value="Print" class="button-primary" style="display:none;" onclick="print_reprint_copy();get_user_mdlist_for_IR();" />--%>
                            <input id="btnCloseIR" type="button" value="Close" class="button-primary" onclick="hideModal('popupIncidentReport');" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>  
    </div>  

    <%--composer email--%>
    <div id="popupComposEmail"  class="modalPage" >
        <div id= "ComposEmailBG" class="modalBackground"></div>  
        <div id= "ComposEmailContainer" class="modalContainer" style="height: 350px;width:500px; margin: 150px 23% 15%;">
            <div id='ComposEmailHeadd' class="modalhead" style="height:13%;" >
                <table>
                    <tr>
                        <td style="padding:15px 0 0 15px;">
                            <span>
                                REASON
                            </span>
                        </td>
                    </tr>
                </table>                
            </div>
            <div id='ComposEmailBody' class="modalbody" style="height:73%;" >
                <table id='CEdatabody' class='gridBody' cellpadding="0" cellspacing= "0" style="height:100%;">
                    <tr>
                        <td>
                            <textarea id="txtIRBody" 
                                   class="inputs"
                                   style="width:  475px; height: 237px;
                                   font-family: Helvetica,sans-serif,Segoe UI,Helvetica Neue;
                                   font-size:14px;
								   resize: none;"
                            ></textarea>
                        </td>                        
                    </tr>                          
                </table>
            </div>
            <div id='ComposEmailFooT' class="modalfoot" style="height:13%;" >
                <table style="width:100%;" >
                    <tr>                                         
                        <td style="text-align:right;">
                            <input id="btnSendIR" type="button" value="Send" class="button-primary"/>
                            <input id="btncancelIR" type="button" value="Cancel" class="button-primary" onclick="hideModal('popupComposEmail'); revealModal('popupIncidentReport'); revealModal('modalIRdetails');" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>  
    </div>  


     <%--IR Details Modal--%>
    <div id="modalIRdetails"  class="modalPage" >
        <div id= "" class="modalBackground"></div>  
        <div id= "IRdetailContainer" class="modalContainer" style="height: 80vh;width:80vw; margin: auto;right:0px;left:0px;">
            <div id='' class="modalhead" style="height:20%;padding-top: 7px;" >
                <%--<p style="font-size: 12px">Incident Report</p>--%>
                <table style="width:100%">
                    <tr>
                        <td class="align-left" style="padding: 0px 5px 0 0;">
                            <span>
                                <b class="detail-font">Incident Report</b>
                            </span>
                        </td>
                        <td class="align-right"></td>
                    </tr>   
                    <tr>
                        <td class="detail-font align-left" style="">Location:<span id="spanLocation"></span></td>
                        <td class="detail-font align-right">Department:<span id="spanDept"></span></td>
                    </tr>
                    <tr style="">
                        <%--<td class="detail-font align-left">Supplier:</td>--%>
                        <td class="detail-font align-left" style="padding-bottom: 10px;">Transaction No.:<span id="spanTranId"></span></td>
                    </tr>
                    <tr style="height: 30px;">
                        <td class="align-right" style="">
                            <span style="font-size: 12px">Item: </span>
                            <input id="txtItem" maxlength="10" class="custom-txt" style="width: 200px" type="text" placeholder="Enter your item here..." />
                        </td>
                        <td class="align-left">
                            <span style="font-size: 12px">Actual Quantity: </span>
                            <input id="txtQty"  maxlength="4" class="custom-txt" style="width: 200px" type="text" placeholder="Enter actual quantity here..." />
                            <input id="btnAddIrItem" class="button-primary" style="" type="button" value="Add"/>
                            <input id="btnModifyIrItemQty" class="button-primary" style="display:none" type="button" value="Save"/>
                            <input id="btnCancelIrItemQty" class="button-primary" style="display:none" type="button" value="Cancel"/>
                        </td>
                    </tr>
                </table>                
            </div>
            <div id='' class="modalbody" style="height:70%;" >
                <table id='' class='gridBody' >
                    <tr>
                        <td>
                            <table id="tblIRdetail" class="dataTable" style="text-align: center" cellspacing="0" width="100%">
                                <thead>
                                    <tr> 
                                        <th>ID</th>
                                        <th>ORIN</th>
                                        <th>BARCODE</th>
                                        <th>VPN</th>
                                        <th>ITEM DESC</th>
                                        <th>QTY</th>
                                        <th>ACTUAL QTY</th>
                                    </tr>
                                </thead>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
            <div id='' class="modalfoot" style="height:10%;" >
                <table style="width:100%;" >
                    <tr>  
                        <td style="text-align:left;">
                            <input id="btnDeleteIrItem" class="button-primary" type="button" value="Delete"/>                            
                            <input id="btnEditIrItem" class="button-primary" type="button" value="Edit"/>
                        </td>                                       
                        <td style="text-align:right;">
                            <input id="btnIrOk" type="button" value="OK" class="button-primary"/>
                            <input id="" type="button" value="Cancel" class="button-primary" onclick="hideModal('modalIRdetails');" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>  
    </div>  

    <%--loading--%>  
        <div class="modalPage" id="loading">
            <div class="modalBackground" ></div>
           <%-- <div class="modalContainer">--%>
                 <div style="position:absolute;z-index:750; top:0; left:0; bottom:0; right:0; padding:0 45% 0 45%; -moz-opacity:80 opacity: 80;" id="loadingcontainer">
                    <img src="./images/ajax-loader-big.gif" alt="Loading"/>
                 </div> 
                 
           <%--</div>--%>
        </div> 

     <script src="Scripts/sellingAreaDashboard.js" type="text/javascript"></script>
    <script src="Scripts/plugins/datatable/extension/dataTables.select.min.js"></script>
</asp:Content>





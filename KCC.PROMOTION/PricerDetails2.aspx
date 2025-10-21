<%@ Page Language="C#"  MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="PricerDetails.aspx.cs" Inherits="BuyerDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <link href="Scripts/plugins/pikaday/css/pikaday.css" rel="stylesheet" type="text/css" />
    <title>Pricer Details</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div id="Outline" class="SellingOutline" style="height:100%;">
        <div id="SellingAreaBorder" class="SellingBack">
        </div>
        <div id="SellingAreaBody" class="SellingMainBody" style="height:100%;">
            <div class="SellingMainHead">
                <table width="100%">
                    <tr>
                        <td width="50%" colspan="4" style="text-align: left;">
                            <img src="Images/promotion.png" alt="ERROR LOADING IMAGE">
                        </td>
                        <td width="50%">
                            <table class="filterlabel" cellpadding="0">
                                <tr>
                                    <td>
                                        <input id="locId" type="hidden" value="" />
                                        <input id="locName" disabled="disabled" class="inputs" tabindex="1" placeholder="Location"/>                                                     
                                    </td>
                                    <td>
                                        <input id="deptId" type="hidden" value="" />
                                        <input id="deptName" disabled="disabled" class="inputs" tabindex="2" placeholder="Department"/>                                  
                                    </td>                                
                                </tr>
                                 <tr>
                     
                                </tr>
                                     <td>
                                      <%--  <input  type="text" class="inputs" placeholder="Remarks"/>--%>
                                      <textarea id="txtPromoDescription" disabled="disabled" rows="3" class="inputs" style="height: 20px; min-height: 20px; max-height:140px; width: 300px;  max-width: 300px; min-width: 300px; wrap="hard"" ></textarea>
                                    </td>
                                  <td>
                                       <%-- <input  type="text" class="inputs" placeholder="Prdomotion Type"/>--%>
                                       <select class="inputs" id='cbPromoTypes' disabled="disabled" style="height:23px;width:230px;">
                                            <option value="0">Multibuy</option>
                                            <option value="1">Simple</option>
                                            <option value="2">Threshold</option>
                                       </select>
                                       <span>Free List  </span><input type="checkbox" disabled="disabled" id="chkFreeList"/>
                                    </td>
                                      
                            </table>

                            <table id="tblDates" cellpadding="0">
                            </table>
                        </td> 
                        <td style="width:10%;" >
                            <table style="text-align:right;width:100%;height:80px;">
                                <tr>
                                    <td style="text-align:right;padding-bottom:63px">
                                        <a id='lnkLogoutCntr' href="#" >Logout</a>
                                        <asp:LinkButton ID="lnklogout"  ClientIDMode="Static" runat="server" OnClick="lnklogout_Click" style="display:none;">Logout</asp:LinkButton>
                                         <%-- <asp:LinkButton ID="lnklogout" runat="server" OnClick="lnklogout_Click">Logout</asp:LinkButton>--%>
                                    </td>
                                </tr>
                            </table>
                        </td>
                     
                    </tr>
                </table>                   
            </div>
            <div style="height:85%; ">
                <table width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td colspan="3">
                            <div>                             
                                 <ul id="breadcrumb">
                                    <li>
                                        <a href="PricerDashboard.aspx" title="Home"><img src="./images/home.png" alt="Home" class="home" /></a>
                                    </li>   
                                    <li id='lnkReprintManager' visible="false" runat="server">
                                        <a href="reprintmanager.aspx">Reprint Manager</a>
                                    </li> 
                                    <li>
                                        <a href="#">Pricer Details</a>
                                    </li>        
                                 </ul>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                             <div style="height:535px;">
                               <%--start tbl Items--%>
                                 <table id="tblItems" style="text-align:center" class="display" cellspacing="0" width="100%">
                                    <thead>
                                        <tr>
                                            <%--<th><input id="chkSet" type="checkbox" /></th>--%>
                                            <th>ORIN</th>
                                            <th>BARCODE</th>
                                            <th>VPN</th>
                                            <th>ITEM DESCRIPTION</th>
                                            <th>SRP</th>
                                            <th>AGE</th>
                                            <th>QTY</th>
                                            <th>TYPE</th>
                                            <th>DISCOUNT</th>
                                            <th>PROMO MUP</th>
                                        </tr>
                                    </thead>
                                </table>
                                <%--end tbl Items--%>
                             </div>
                     
                            <div id="divFreeList" style="width:100%;">
                            <div style="margin-top:20px;font-size:15px;box-sizing:border-box;padding:10px 15px;background-color:#eaeaea;border-color:#ddd;color:#333;"> FREE LIST</div>
                            <table width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td colspan="3">
                                         <div style="height:390px;">  
                                             <table id="tblItemsFreeList" style="text-align:center" class="display" cellspacing="0" width="100%">
                                                <thead>
                                                    <tr>
                                                        <th>ORIN</th>
                                                        <th>BARCODE</th>
                                                        <th>VPN</th>
                                                        <th>ITEM DESCRIPTION</th>
                                                        <th>AGE</th>
                                                        <th>QTY</th>
                                                        <th>SRP</th>
                                                    </tr>
                                                </thead>
                                            </table>
                                         </div>
                                    </td>                        
                                </tr>
                            </table>
                        </div> <%--end of divFreeList--%>  


                              <div id="Div2" class="gridfoot" style="height:38px;position: relative;">  
                                 <table width="100%" cellpadding="0" cellspacing="0" >
                                    <tr>
                                       <td style="text-align:left; width:40%; font-size:15px;">
                                            &nbsp;  
                                             <span style="text-align:left;padding-left:10px;">Tran No: </span>
                                             <label id="lblTranNo"></label>
                                
                                             <span style="text-align:left;padding-left:10px;">Total Items:</span>
                                             <label id="lblTotalItems">0</label>

							             </td>

                                         <td style="text-align:left; width:20%; font-size:15px;">
                                             <span style="text-align:left;padding-left:10px;">Itemlist No: </span>
                                             <label id="lblItemListNo"></label>
							             </td>


                                        <td style="text-align:right;  width:40%;">
                                            <input id="btnApprove" type="button" value="Approve" class="button-primary"/>
                                          <%--  <input id="btnPreview" type="button" value="Preview" class="button-primary" disabled="disabled"  />--%>  
                                           <input id="btnPreview" type="button" value="Preview" class="button-primary"/>  
                                           <input id="btnModifyDate" type="button"  value="Modify" class="button-primary" tabindex='10' />
                                        </td>
                                    </tr>                                        
                                 </table>                              
                              </div>
                             



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

  
  <div id="PopUpAdminAcctOutline"  class="modalPage" >
        <div id= "PopAdminAcctBackground" class="modalBackground"></div>  
        <div id= "PopAdminAcctContainer" class="modalContainer" style="height: 140px;width:250px; margin: 250px 29% 15%;">
            <div id='PopupAdminAcctContainerHead' class="modalhead" style="height:25%;" >
                <table>
                    <tr>
                        <td style="padding:15px 0 0 15px;">
                            <span>Enter Admin Account</span>
                        </td>
                    </tr>
                </table>                
            </div>
            <div id='PopupAdminAcctContainerBody' class="modalbody" style="height:auto;" >
                <table class="rowEditorbody">
                    <tr>
                        <td>
                            <input id='txtAdminUsername'type="text" style="height:25px; width :230px;" class="inputs" placeholder="Username"/>
                        </td>
                    </tr>
                    <tr>                        
                         <td>
                            <input id='txtAdminPassword'style="height:25px; width :230px;" class="inputs" type="password"placeholder="Password"/>
                        </td>
                    </tr>
                </table>
            </div>
            <div id='Div1' class="modalfoot" style="height:25%;" >
                <table style="width:100%;" >
                    <tr>                                         
                        <td style="text-align:right;">
                            <input id="btnApproveModification" type="button" value="Approve" class="button-primary"  tabindex='3' />
                            <input id="Button2" type="button" value="Cancel" class="button-primary" tabindex='4' onclick="hideModal('PopUpAdminAcctOutline');" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>  
    </div>

    <%--  <!-- Modal -->
          <div id="modalAdminVerification" class="modal fade" role="dialog">
            <div class="modal-dialog modal-sm">
              <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal">&times;</button>
                <h4 class="modal-title">Admin Verification Account</h4>
            </div>
                <div class="modal-body">
                      <div class="form-group text-left">
                         <input id="txtAdminUsername" type="text" placeholder="Username" class="form-control"/>
                      </div>

                       <div class="form-group">
                           <input id="txtAdminPassword" type="password" placeholder="Password" class="form-control"/>
                       </div>
                </div>
                <div class="modal-footer">
                  <button id="btnApproveModification" type="button" class="btn btn-success">Approve</button>
                </div>
              </div>

            </div>
         </div>--%>

      <%--hidden fields--%>
         <input id="hfHeadId" type="hidden" runat="server" clientidmode="Static"/>
      <%--end of hidden fields--%>

    <script src="Scripts/plugins/moment/moment.min.js" type="text/javascript"></script>
    <script src="Scripts/plugins/pikaday/js/pikaday.js" type="text/javascript"></script>
   <%-- <script src="Scripts/buyerDetails.js" type="text/javascript"></script>--%>
    <script src="Scripts/general.js" type="text/javascript"></script>
    <script src="Scripts/pricerDetails.js" type="text/javascript"></script>

</asp:Content>

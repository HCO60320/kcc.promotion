<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="reprintmanager.aspx.cs" Inherits="KCC.PROMOTION.reprintmanager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <title>Reprint Manager</title>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <link href="Styles/tabmenu.css" rel="stylesheet" type="text/css" />
    <div id="Outline"  class="SellingOutline"  >
        <div id= "BuyerDashBoardBorder" class="SellingBack">    
            
        </div>              
        <div id="AdminAreaBody" class="SellingMainBody">
            <div class="BuyerDashBoardMainHead">
                <table width="100%">
                    <tr>
                        <td colspan="4" style="text-align:left; width:50%;" >
                            <img src="Images/promotion.png" alt="ERROR LOADING IMAGE">
                        </td>
                        <td style="width:50%;" >
                            <table style="text-align:right;width:100%;height:80px;">
                                <tr>
                                    <td style="text-align:right;padding-bottom:63px">
                                        <a id='lnkLogoutCntr' href="#" >Logout</a>
                                        <asp:LinkButton ID="lnklogout"  ClientIDMode="Static" runat="server" OnClick="lnklogout_Click" style="display:none;">Logout</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>                     
            </div>
            <div style="height:85%; ">
                <table width="100%" cellpadding="0" cellspacing="0">
                    <tr  style="padding-bottom:0px;">
                        <td>
                            <div>
                                 <ul id="breadcrumb">
                                    <li>
                                        <a href="#" onclick ="page_redirect('reprintmanager.aspx')" title="Home"><img src="./images/home.png" alt="Home" class="home" /></a>
                                    </li>  
                                     <li>
                                        <a href="pricerDashboard.aspx">Pricer Dashboard</a>
                                    </li>             
                                 </ul>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div id="tabmenu">
                                <ul>
                                    <li id ="liReprintPromo" >
                                        <a href='#'>
                                            <span>REPRINT PROMO</span>
                                        </a>
                                    </li>
                                    <li id ="liPurgePromo" >
                                        <a href='#'>
                                            <span>PURGE PROMO</span>
                                        </a>
                                    </li>
                                    <li id ="liEnablePrint" >
                                          <input id="btnEnable" type="button" value="Enable Print Preview" class="button-primary" style="width: 140px !important"/>
                                    </li>
                                    <li id ="liRevertToWorksheet" >
                                          <input id="btnRevertToWorksheet" type="button" value="Revert To Worksheet" class="button-primary" style="width: 140px !important"/>
                                    </li>
                                </ul>
                            </div>    
                            <div id="gridviewReprintPromo" style="z-index:0;" >
                                <div style="height:422px; padding-top:8px;">  
                                   <table id="tblReprintTransaction" class="display" style="text-align:center" cellspacing="0" width="100%">
                                        <thead>
                                            <tr>
                                                <th>TRAN ID</th>
                                                <th>REPRINT CODE</th>
                                                <th>LOCATION</th>
                                                <th>DEPARTMENT</th>
                                                <%--<th>START DATE TO END DATE</th>--%>
                                                <th>STATUS</th>
                                                <th class="action">ACTION</th> 
                                            </tr>
                                        </thead>
                                    </table>        
                                </div>
                                <%--<div id="gridfooterNewMD" class="gridfoot" style="height:38px;" >
                                    <table width="100%" cellpadding="0" cellspacing="0" >
                                        <tr>
                                            <td colspan="3" style="text-align:left;padding-left:10px; font-size:15px;">
                                                <span id="sptotal_count_MD">Total Promotion:</span>
                                            </td>
                                        </tr>
                                    </table>                                          
                                </div>--%>
                            </div>
                            <div id="gridviewPurgePromo" style="z-index:0;">
                                <div style="height:382px; padding-top:8px;">  
                                    <table id="tblPurgeTransactions" class="display" style="text-align:center" cellspacing="0" width="100%">
                                        <thead>
                                            <tr>
                                                <th>TRAN ID</th>
                                                <th>LOCATION</th>
                                                <th>DEPARTMENT</th>
                                                <th>STATUS</th>
                                                <th>PREPARED BY</th>
                                                <th>LAST UPDATE DATE</th>
                                                <th class="action">PURGE</th>
                                            </tr>
                                        </thead>
                                    </table>            
                                </div>
                                <div id="gridfooterExeMD" class="gridfoot" style="height:38px;" >
                                    <table width="100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td colspan="3" style="text-align:left;padding-left:10px;padding-top:7px;font-size:15px;">
                                                <span>Total Transactions:</span>
                                                <span id="total_MD">0</span>
                                            </td>
                                            <td style="text-align:right;width:100px";">
                                                <input id="btnPurgeAll"  type="button" value="BULK" class="button-primary" tabindex='1' />                                              
                                            </td>
                                        </tr>
                                    </table>                          
                                </div>
                            </div>
                        </td>                        
                    </tr>
                </table>
            </div>
        </div> 
    </div>

	

         <%--popup enter pincode--%>
    <div id="PopUpOutline"  class="modalPage" >
        <div id= "Div1" class="modalBackground"></div>  
        <div id= "Div2" class="modalContainer" style="height: 130px;width:200px; margin: 250px 34% 15%;">
            <div id='Div3' class="modalhead" style="height:30%;" >
                <table>
                    <tr>
                        <td style="padding:12px 0 0 10px;">
                            <span>
                                ENTER PIN CODE
                            </span>
                        </td>
                    </tr>
                </table>                
            </div>
            <div id='Div4' class="modalbody" style="height:38%;" >
                <table>
                    <tr>
                        <td>
                            <input id='req_head_id' type="hidden" />
                            <input id='req_e_date' type="hidden" />
                            <input id='req_r_date' type="hidden" />
                            <input id='req_code' type="hidden" />
                        </td>
                    </tr>
                    <tr>    
                        <td>
                           
                            <%--<input id="btnReprint" type="button" value="Print" class="button-primary" style="display:none;" onclick="print_reprint_copy();get_user_mdlist_for_IR();" />--%>
                            <input id="txtPincode" 
                                   placeholder="PIN CODE"
                                   class="inputs"
                                   style="height:30px;
                                          width:187px;
                                          text-align:center;"
                            />
                        </td>
                    </tr>
                </table>
            </div>
            <div id='Div5' class="modalfoot" style="height:30%;" >
                <table style="width:100%;" >
                    <tr>                                         
                        <td style="text-align:right;">
                            <input id="btnOKPincode" type="button" value="OK" class="button-primary" enabled="enabled"/>
                            <input id="btnCancelPincode" type="button" value="Cancel" class="button-primary"/>
                        </td>
                    </tr>
                </table>
            </div>
        </div>  
    </div>


        <%--popup enter transaction Number--%>
    <div id="printPreview"  class="modalPage" >
        <div id= "Div1" class="modalBackground"></div>  
        <div id= "Div2" class="modalContainer" style="height: 160px;width:200px; margin: 250px 34% 15%;">
            <div id='Div3' class="modalhead" style="height:20%;" >
                <table>
                    <tr>
                        <td style="padding:12px 0 0 10px;">
                            <span>
                                ENTER TRANSACTION NUMBER
                            </span>
                        </td>
                    </tr>
                </table>                
            </div>
            <div id='Div4' class="modalbody" style="height:50%;" >
                <table>
                    <tr>    
                        <td>
                            <%--<input id="btnReprint" type="button" value="Print" class="button-primary" style="display:none;" onclick="print_reprint_copy();get_user_mdlist_for_IR();" />--%>
                              <select id="moduleID" style="text-align: center;float:left; height:26px;width: 195px; vertical-align: middle; ">           
                              <option id="buyer" value="BUYER" selected>BUYER</option>
                              <option id="pricer" value="PRICER">PRICER</option>
                            </select> 
                              <input id="tranNo" 
                                   placeholder="TRANSACTION NUMBER"
                                   class="inputs"
                                   style="height:30px;
                                          width:187px;
                                          text-align:center;"
                            />
                        </td>
                    </tr>
                </table>
            </div>
            <div id='Div5' class="modalfoot" style="height:30%;" >
                <table style="width:100%;" >
                    <tr>                                         
                        <td style="text-align:right;">
                            <input id="btnOKtran" type="button" value="OK" class="button-primary" enabled="enabled"/>
                            <input id="btnCanceltran" type="button" value="Cancel" class="button-primary"/>
                        </td>
                    </tr>
                </table>
            </div>
        </div>  
    </div>

    <%--Modal for Revert To Worksheet--%>
      <div id="divRevertToWorksheet"  class="modalPage" >
        <div id= "" class="modalBackground"></div>  
        <div id= "" class="modalContainer" style="height: 150px;width:200px; margin: 250px 34% 15%;">
            <div id='' class="modalhead" style="height:35%;" >
                <table>
                    <tr>
                        <td style="padding:0px 0 0 0px;">
                            <p><b>REVERT TO WORKSHEET</b></p>
                            <span>
                                ENTER TRANSACTION NUMBER
                            </span>
                        </td>
                    </tr>
                </table>                
            </div>
            <div id='' class="modalbody" style="" >
                <table>
                    <tr>
                        <td>
                            <input id="txtRevertToWorksheet"
                                placeholder="TRANSACTION NUMBER"
                                class="inputs"
                                maxlength="9"
                                style="height: 30px; width: 187px; text-align: center;" />
                        </td>
                    </tr>
                </table>
            </div>
            <div id='' class="modalfoot" style="height:30%;" >
                <table style="width:100%;" >
                    <tr>                                         
                        <td style="text-align:right;">
                            <input id="btnOKRevertToWorksheet" type="button" value="OK" class="button-primary"/>
                            <input id="btnCancelRevertToWorksheet" type="button" value="Cancel" class="button-primary"/>
                        </td>
                    </tr>
                </table>
            </div>
        </div>  
    </div>

     <script src="Scripts/ReprintScript.js" type="text/javascript"></script>
    <script src="Scripts/RevertTranToWorksheet.js?1.0.0" type="text/javascript"></script>
</asp:Content>
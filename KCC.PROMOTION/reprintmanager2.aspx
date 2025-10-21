<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/Site.master" CodeFile="reprintmanager.aspx.cs" Inherits="reprintmanager" %>

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
                                    <li id ="liPurgePromo");" >
                                        <a href='#'>
                                            <span>PURGE PROMO</span>
                                        </a>
                                    </li>
                                </ul>
                            </div>    
                            <div id="gridviewReprintPromo" style="z-index:0;" >
                                <div style="height:430px;">  
                                   <table id="tblReprintTransaction" class="display" style="text-align:center" cellspacing="0" width="100%">
                                        <thead>
                                            <tr>
                                                <th>TRAN ID</th>
                                                <th>REPRINT CODE</th>
                                                <th>LOCATION</th>
                                                <th>DEPARTMENT</th>
                                                <th>START DATE TO END DATE</th>
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
                                <div style="height:430px;">  
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
                                            <td colspan="3" style="text-align:left;padding-left:10px; font-size:15px;">
                                                <span id="sptotal_count_exeMD">Total MD:</span>
                                            </td>
                                            <td style="text-align:right;width:100px";">
                                                <input id="btnPurgeAll"  type="button" value="BULK" class="button-primary" onclick="approve_md_list_prc(this);" tabindex='1' />                                              
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
        <div id= "Div2" class="modalContainer" style="height: 130px;width:200px; margin: 250px 29% 15%;">
            <div id='Div3' class="modalhead" style="height:30%;" >
                <table>
                    <tr>
                        <td style="padding:15px 0 0 15px;">
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
                                          width:187px;"
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

     <script src="Scripts/ReprintScript.js" type="text/javascript"></script>

</asp:Content>
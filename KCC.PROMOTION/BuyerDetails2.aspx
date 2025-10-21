<%@ Page Language="C#"  MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="BuyerDetails.aspx.cs" Inherits="BuyerDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
  <link href="Scripts/plugins/pikaday/css/pikaday.css" rel="stylesheet" type="text/css" />

    <title>Buyer Details</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div id="Outline" class="SellingOutline" style="height:100%;">
        <div id="SellingAreaBorder" class="SellingBack">
        </div>
        <div id="SellingAreaBody" class="SellingMainBody" style="height:100%;">
       <%-- 718px--%>
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
                                        <textarea id="txtPromoDescription" rows="3" class="inputs" style="height: 20px; min-height: 20px; max-height:140px; width: 300px;  max-width: 300px; min-width: 300px; wrap="hard" "></textarea>                                
                                    </td>                                
                                </tr>
                                 <tr>
                                    <td>
                                        <input id="deptId" type="hidden" value="" />
                                        <input id="deptName" disabled="disabled" class="inputs" tabindex="2" placeholder="Department"/>                                                      
                                    </td>
                                    <td>
                                        <select class="inputs" id='cbPromoTypes' disabled="disabled" style="height:23px;width:230px;">
                                            <option value="0">Multibuy</option>
                                            <option value="1">Simple</option>
                                            <option value="2">Threshold</option>
                                       </select>
                                       <span>Free List  </span><input type="checkbox" disabled="disabled" id="chkFreeList"/>                          
                                    </td>                                
                                </tr>
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
                                          <%--<asp:LinkButton ID="lnklogout" runat="server" OnClick="lnklogout_Click">Logout</asp:LinkButton>--%>
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
                                        <a href="BuyerDashboard.aspx" title="Home"><img src="./images/home.png" alt="Home" class="home" /></a>
                                    </li>    
                                    <li>
                                        <a href="#">Buyer Details</a>
                                    </li>          
                                 </ul>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                          <%-- Row Editor body --%>
                                    <div id="dvRowEditorMain" style="position:relative;display:none;">  
                                        <div id="dvRowEditorBody" class="roweditor_bg">
                                            <table cellpadding="0" cellspacing="0" class="rowEditorbody" >
                                                <tbody>
                                                    <tr>
                                                        <td style="width:60px">
                                                             <span id ='lblOrin'></span>
                                                        </td>
                                                        <td style="width:87px">
                                                             <span id ='lblBarcode'></span>
                                                        </td>
                                                        <td style="width:60px">
                                                             <span id = 'lblVPN'></span>
                                                        </td>
                                                        <td style="width:225px">
                                                             <span id  = 'lblItemDesc'></span>
                                                        </td>
                                                        <td style="width:45px">
                                                             <span id  = 'lblUnitCost'></span>
                                                        </td>
                                                        <td style="width:45px">
                                                             <span id  = 'lblSRP'></span>
                                                        </td>
                                                        <td style="width:100px;"> 
                                                             <select id ='ddClrType' style="width:100px" >
                                                                 <option value='1'>FIXED AMOUNT</option>
                                                                 <option value='2'>AMOUNT OFF</option>
                                                                 <option value='3'>PERCENT OFF</option>
                                                             </select>
                                                        </td>
                                                        <td style="width:40px; ">
                                                             <input id='txtDiscount' type='text' class='inputs' />
                                                        </td>
                                                        <td style="width:40px; " >
                                                             <span >PROMO-SRP:</span>
                                                        </td>
                                                        <td style="width:40px;font-weight:bold;" >
                                                             <span id='lblPromoSRP'></span>
                                                        </td>
                                                        <td style="width:40px;">
                                                             <span>MRK-UP:</span>
                                                        </td>
                                                        <td style="width:40px;font-weight:bold; ">
                                                             <span id  = 'lblPromoMarkUp'></span>
                                                        </td>
                                                        <td>                                              
                                                            <input id="btnUpdateItem" type="button"  style=" width:80px;"  value="OK" class=" button-primary"/> 
                                                        </td>
                                                        <td >                      
                                                            <input id="btnCancelUpdate" type="button" style=" width:80px;"  value="Cancel"  class=" button-primary"/>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                    </div> <%--end of Row Editor body --%>

                             <div style="height:535px;">

                                    <%-- Row Editor buttons --%>
                                    <%--<div  id="dvRowEditorButtons" class="rowEditorbtn_bg" style="display:none;">
                                        <table cellpadding="0" cellspacing="0" >
                                            <tr> 
                                                <td>                                              
                                                    <input id="btnOK" type="button"  style=" width:80px;"  value="OK" class=" button-primary" onclick="update_md()"  /> 
                                                </td>
                                                <td >                      
                                                    <input id="btnCancel" type="button" style=" width:80px;"  value="Cancel"  class=" button-primary" onclick= "hideEditor('dvRowEditorMain','dvRowEditorButtons');" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>--%>


                               <%--start tbl Items--%>
                                 <table id="tblItems" style="text-align:center" class="display" cellspacing="0" width="100%">
                                    <thead>
                                        <tr>
                                            <th><input id="chkSet" type="checkbox" /></th>
                                            <th>ORIN</th>
                                            <th>BARCODE</th>
                                            <th>VPN</th>
                                            <th>ITEM DESCRIPTION</th>
                                            <th>U-COST</th>
                                            <th>SRP</th>
                                            <th>AGE</th>
                                            <th>QTY</th>
                                            <th>REG MUP</th>
                                            <th>TYPE</th>
                                            <th>DSCNT</th>
                                            <th>PROMO MUP</th>
                                            <th>PROMO SRP</th>
                                        </tr>
                                    </thead>
                                </table>
                                <%--end tbl Items--%>


                             </div>
                             <div id="Div3" class="gridfoot" style="height:38px;position: relative;">  
                                <table width="100%" cellpadding="0" cellspacing="0" >
                                   <tr>
                                      <td style="text-align:left; width:00%; font-size:15px;">
                                           <input id="btnSubmit" type="button" value="Submit" class="button-primary"/>
                                           <input id="btnPreview" type="button" value="Preview" class="button-primary" disabled="disabled"  />  

                                            <span style="text-align:left;padding-left:10px;">Tran No: </span>
                                            <label id="lblTranNo"></label>
                                           
                                            <span style="text-align:left;padding-left:10px;">Total Items:</span>
                                            <label id="lblTotalItems">0</label>

									    </td>
                                       <td style="text-align:right;">
                                            <input id="btnBack" type="button" value="Back" class="button-primary" tabindex='9' />
                                            <input id="btnSet" type="button" style="display:none;" value="Set" class="button-primary" tabindex='10' />
                                       </td>
                                   </tr>                                        
                                </table> 
                             </div>
                        </td>                        
                    </tr>
                </table>

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
                                 <div id="Div2" class="gridfoot" style="height:38px;">  
                                    <table width="100%" cellpadding="0" cellspacing="0" >
                                       <tr>
                                           <td style="text-align:right; width:40%; font-size:15px;">
                                                <span>Total Items:</span>
                                                <span id="total_freeList_items">0</span>
                                           </td>
                                           <td  style="text-align:center; font-size:15px;">
                                               
                                           </td>                            
                                       </tr>                                        
                                    </table> 
                                 </div>
                            </td>                        
                        </tr>
                    </table>
                </div> <%--end of divFreeList--%>  


            </div>
        </div> 
    </div>



    <div id="PopUpSetMultipleOutline"  class="modalPage" >
        <div id= "PopSetMultipleBackground" class="modalBackground"></div>  
        <div id= "PopSetMultipleContainer" class="modalContainer" style="height: 130px;width:200px; margin: 250px 29% 15%;">
            <div id='PopupSetMultipleContainerHead' class="modalhead" style="height:30%;" >
                <table>
                    <tr>
                        <td style="padding:15px 0 0 15px;">
                            <span>Set Promotion type</span>
                        </td>
                    </tr>
                </table>                
            </div>
            <div id='PopupSetMultipleContainerBody' class="modalbody" style="height:38%;" >
                <table class="rowEditorbody">
                    <tr>
                        <td>
                            <input id='edt_totchselect' type="hidden" value="0"/>
                        </td>
                    </tr>
                    <tr>                        
                        <td style="width:100px;padding: 5px 0px 0px;"> 
                             <select id ='ddClrTypeMultiple' style="width:100px" >
                                 <option value='1'>FIXED AMOUNT</option>
                                 <option value='2'>AMOUNT OFF</option>
                                 <option value='3'>PERCENT OFF</option>
                             </select>
                        </td>
                        <td style="width:40px;padding:5px 0px 0px;">
                             <input id='txtDiscountMultiple' type='text' class='inputs'  
                                    style="height:20px;width:85px;"/>
                        </td>
                    </tr>
                </table>
            </div>
            <div id='Div1' class="modalfoot" style="height:30%;" >
                <table style="width:100%;" >
                    <tr>                                         
                        <td style="text-align:right;">
                            <input id="btnSetMultipleDiscount" type="button" value="OK" class="button-primary"/>
                            <input id="btnCancelSet" type="button" value="Cancel" class="button-primary" onclick="hideModal('PopUpSetMultipleOutline');" />
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

  

    <div id="popupsearch" class="modalPage">
        <div id= "popupsearchbg" class="modalBackground"></div>
        <div id= "popupsearchcontainer" class="modalContainer" style="height: 42px;width:300px; margin: 250px 26% 15%;">
             <table style="width:100%;" border='0' cellpadding='0' cellspacing = '0' >
                <tr>
                    <td style="padding-left:5px;
                                padding-right:5px;">  
                        <input id ='txtsearchpopup'type="text" class="BarcodeSearch"
                         placeholder="Search Item..." tabindex = '1' 
                         style="width:278px;"
                         onkeyup="filter_prc_item(this,'databody',event)"
                          />                        
                    </td>
                </tr>
             </table>
        </div>
    </div>

      <%--hidden fields--%>
         <input id="hfHeadId" type="hidden" runat="server" clientidmode="Static"/>
      <%--end of hidden fields--%>
  
    <script src="Scripts/plugins/moment/moment.min.js" type="text/javascript"></script>
    <script src="Scripts/plugins/pikaday/js/pikaday.js" type="text/javascript"></script>
    <script src="Scripts/buyerDetails.js" type="text/javascript"></script>
    <script src="Scripts/plugins/keyboard-delimiter/js/keyboard-delimiter.min.js" type="text/javascript"></script>
</asp:Content>

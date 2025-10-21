<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master"  CodeFile="SellingAreaCreate.aspx.cs" Inherits="SellingAreaCreate" %>
<%@ Register assembly="AjaxControlToolkit" 
             namespace="AjaxControlToolkit" 
             TagPrefix="ajx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">

<link href="Scripts/plugins/pikaday/css/pikaday.css" rel="stylesheet" type="text/css" />

    <title>Create</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div id="Outline" style="height:100%" class="SellingOutline">
        <div id="SellingAreaBorder" class="SellingBack">
        </div>
        <div id="SellingAreaBody" style="height:100%" class="SellingMainBody">
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
                                        <asp:UpdatePanel ID="udploc" runat="server">
                                            <ContentTemplate>
                                                <asp:HiddenField runat="server" ID="locId" ClientIDMode="Static" Value="" />
                                                <asp:TextBox ID="locName" ClientIDMode="static" runat="server" placeholder="Location"
                                                    class="inputs" AutoPostBack="false" TabIndex='1'>
                                                </asp:TextBox>
                                                <ajx:autocompleteextender runat="server" BehaviorID="AutoCompleteExLoc" ID="acegetloc"
                                                    TargetControlID="locName" 
                                                    ServicePath="WebMethods/generalWebMethod.aspx" 
                                                    ServiceMethod="getLocs"
                                                    MinimumPrefixLength="1" 
                                                    ClientIDMode="Static" 
                                                    CompletionInterval="50" 
                                                    EnableCaching="false"
                                                    CompletionSetCount="20" 
                                                    ShowOnlyCurrentWordInCompletionListItem="true" 
                                                    OnClientItemSelected="locselected"
                                                    CompletionListCssClass="autocomplete_completionListElement" 
						                            CompletionListItemCssClass="autocomplete_listItem"
						                            CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem">
                                                    
                                                    <Animations>
														<OnShow>
															<Sequence>
																
																<OpacityAction Opacity="0" />
																<HideAction Visible="true" />
																
																<ScriptAction Script="
																	// Cache the size and setup the initial size
																	var behavior = $find('AutoCompleteExLoc');
																	if (!behavior._height) {
																		var target = behavior.get_completionList();
																		behavior._height = target.offsetHeight - 2;
																		target.style.height = '0px';
																	}" />
																
															
																<Parallel Duration=".1">
																	<FadeIn />
																	<Length PropertyKey="height" StartValue="0" EndValueScript="$find('AutoCompleteExLoc')._height" />
																</Parallel>
															</Sequence>
														</OnShow>
														<OnHide>
															<Parallel Duration=".1">
																<FadeOut />
																<Length PropertyKey="height" StartValueScript="$find('AutoCompleteExLoc')._height" EndValue="0" />
															</Parallel>
                                                            

														</OnHide>
                                                    </Animations>
                                                    
                                                </ajx:AutoCompleteExtender>
						                    </ContentTemplate>
						                	<Triggers>
						                	    <asp:AsyncPostBackTrigger ControlID="locName" EventName="TextChanged" />
						                	</Triggers>
						                </asp:UpdatePanel>                                                                            
                                    </td>
                                    <td>
                                      <%--  <input  type="text" class="inputs" placeholder="Remarks"/>--%>
                                      <textarea id="txtPromoDescription" rows="3" class="inputs" style="height: 20px; min-height: 20px; max-height:140px; width: 300px;  max-width: 300px; min-width: 300px; wrap="hard" "></textarea>
                                    </td>
                                                                 
                                </tr>

                                <tr>
                                  <td>
                                        <asp:UpdatePanel ID="udpdep" runat ="server" >
						                	<ContentTemplate>
                                                <asp:HiddenField runat="server" ID="deptId" ClientIDMode="Static" Value="" />
						                		<asp:TextBox ID="deptName"  
                                                             ClientIDMode = "static" 
                                                             runat="server"
                                                             placeholder="Department"
                                                             class="inputs"
						                			         AutoPostBack="false" 
                                                             TabIndex='2' >
                                                </asp:TextBox>
                                                <ajx:AutoCompleteExtender 
                                                             runat="server" 
						                                	 BehaviorID="AutoCompleteExDep"
						                                	 ID="acegetdep" 
						                                	 TargetControlID="deptName"
						                                	 ServicePath="WebMethods/generalWebMethod.aspx"
						                                	 ServiceMethod="getDeps"
						                                	 MinimumPrefixLength="1" 
                                                             ClientIDMode ="Static"
						                                	 CompletionInterval="100"
						                                	 EnableCaching="true"
						                                	 CompletionSetCount="20"
                                                             ShowOnlyCurrentWordInCompletionListItem = "true"
                                                             OnClientItemSelected="depselected"
                                                             CompletionListCssClass="autocomplete_completionListElement" 
						                                     CompletionListItemCssClass="autocomplete_listItem"
						                                     CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem">
                                                    <Animations>
														<OnShow>
															<Sequence>
																
																<OpacityAction Opacity="0" />
																<HideAction Visible="true" />
																
																<ScriptAction Script="
																	// Cache the size and setup the initial size
																	var behavior = $find('AutoCompleteExDep');
																	if (!behavior._height) {
																		var target = behavior.get_completionList();
																		behavior._height = target.offsetHeight - 2;
																		target.style.height = '0px';
																	}" />
																
															
																<Parallel Duration=".1">
																	<FadeIn />
																	<Length PropertyKey="height" StartValue="0" EndValueScript="$find('AutoCompleteExDep')._height" />
																</Parallel>
															</Sequence>
														</OnShow>
														<OnHide>
														
															<Parallel Duration=".1">
																<FadeOut />
																<Length PropertyKey="height" StartValueScript="$find('AutoCompleteExDep')._height" EndValue="0" />
															</Parallel>
														</OnHide>
													</Animations>
                                                    
                                                </ajx:AutoCompleteExtender>
						                	</ContentTemplate>
						                	<Triggers>
						                		<asp:AsyncPostBackTrigger ControlID="deptName" EventName="TextChanged" />
						                	</Triggers>
						                </asp:UpdatePanel>                                    
                                    </td> 
                                    <td>
                                       <%-- <input  type="text" class="inputs" placeholder="Prdomotion Type"/>--%>
                                       <select class="inputs" id='cbPromoTypes' style="height:23px;width:230px;">
                                            <option value="0">Multibuy</option>
                                            <option value="1">Simple</option>
                                            <option value="2">Threshold</option>
                                       </select>
                                       <span>Free List  </span><input type="checkbox" disabled="disabled" id="chkFreeList"/>
                                    </td>
                                
                                </tr>
                            </table>

                             <table id="tblDates" cellpadding="0">
                                <tr>
                                    <td>
                                         <input id='txtStartDate'  type="text"  class="inputs dateSelection" placeholder="Start Date"/>
                                    </td>
                                    <td>
                                         <input  id='txtEndDate' type="text" class="inputs dateSelection" placeholder="End Date"/>
                                    </td>
                                    <td>
                                         <input id="btnAddDate"  type="button" class="button-primary" value="Add"/>
                                    </td>
                                </tr>                            
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
                    <tr style="text-align:center;">  
                        <td style="width:90px;text-align:left; padding-left:10px;" class="labeltran">
                             <span>TRAN NO: </span>
                             <label id="lblTranNo"></label>
                        </td>                        
                        <td>
                            <input id="txtbarcode" type="text"class="BarcodeSearch" Placeholder="Barcode" maxlength="13" />

                        </td>
                        <td style="width:90px;text-align:right; padding-right:10px;">
                             <input id="txtopenMD" type="text" class="inputs" style="width:90px;display:none;"  placeholder="Search MD"  tabindex='10' />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <div>                             
                                 <ul id="breadcrumb">
                                    <li>
                                        <a href="SellingAreaDashboard.aspx" o title="Home"><img src="./images/home.png" alt="Home" class="home" /></a>
                                    </li>    
                                    <li>
                                        <a href="#">Create/Edit Transaction</a>
                                    </li>          
                                 </ul>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                             <div style="height:390px;">  
                                 <table id="tblItems" style="text-align:center" class="display" cellspacing="0" width="100%">
                                    <thead>
                                        <tr>
                                            <th>ORIN</th>
                                            <th>BARCODE</th>
                                            <th>VPN</th>
                                            <th>ITEM DESCRIPTION</th>
                                            <th>AGE CODE</th>
                                            <th>QTY</th>
                                            <th>SRP</th>
                                        </tr>
                                    </thead>
                                </table>
                             </div>
                             <div id="Div3" class="gridfoot" style="height:38px;">  
                                <table width="100%" cellpadding="0" cellspacing="0" >
                                   <tr>
                                      <td style="text-align:left; width:30%;">
                                           <input id="btnRemove" type="button" value="Remove" class="button-primary"  disabled="disabled" tabindex='6'  />
                                          <%--  <input id="btnNew" type="button" value="New" class="button-primary" onclick="new_tran();" tabindex='7'/>--%>   
									    </td>
                                       <td  style="text-align:left;width:30%; font-size:15px;">
                                           <span>Total Items:</span>
                                           <span id="total_items">0</span>
                                       </td>  
                                       <%-- <td style="text-align:left;">
                                          <input id="btnSubmit" type="button" value="Submit" class="button-primary"  disabled="disabled" style="display:none;"/>
                                           <input id="btnPrint" type="button" value="Print" class="button-primary" disabled="disabled" style="display:none;" />
                                       </td> --%>                                       
                                       <td style="text-align:right;width:40%;">
                                           <%--<input id="btnIR" type="button" value="Send IR" class="button-primary" onclick="revealModal('popupIncidentReport');get_user_mdlist_for_IR();" tabindex='8'/>--%>
                                           <%--<input id="btnApprove" type="button" value="Approve" class="button-primary"  disabled="disabled"  style="display:none;" />--%>
                                           <%--<input id="btnDone" type="button" value="Save/Done" class="button-primary" onclick="revealModal('PopUpOutline');" tabindex='9' />--%>
                                            <input id="btnDone" type="button" value="Save/Done" class="button-primary" tabindex='9' />
                                            <input id="btnCancelTransaction" type="button" value="Dispose" class="button-primary" tabindex='10' />
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
                        <tr style="text-align:center;">  
                            <td style="width:90px;text-align:left; padding-left:10px;" class="labeltran">

                            </td>                        
                            <td>
                                <input id="txtBarcodeFreeList" type="text"class="BarcodeSearch" Placeholder="Barcode" maxlength="13" />

                            </td>
                            <td style="width:90px;text-align:right; padding-right:10px;">
                                <%-- <input id="Text2" type="text" class="inputs" style="width:90px;display:none;"  placeholder="Search MD"  tabindex='10' />--%>
                            </td>
                        </tr>
                   
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
                                                <th>AGE CODE</th>
                                                <th>QTY</th>
                                                <th>SRP</th>
                                            </tr>
                                        </thead>
                                    </table>
                                 </div>
                                 <div id="Div2" class="gridfoot" style="height:38px;">  
                                    <table width="100%" cellpadding="0" cellspacing="0" >
                                       <tr>
                                          <td style="text-align:left;width:30%;">
                                              <input id="btnFreeListRemove" type="button" value="Remove" class="button-primary"  disabled="disabled" tabindex='6'  />
									      </td>
                                           <td  style="text-align:left; font-size:15px;width:30%;">
                                               <span>Total Items:</span>
                                               <span id="total_freeList_items">0</span>
                                           </td>  
                                                                          
                                           <td style="text-align:right;width:40%;">
                                         
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

    

    <div id="PopUpOutline"  class="modalPage" >
        <div id= "PopBackground" class="modalBackground"></div>  
        <div id= "PopContainer" class="modalContainer">
            <div id='PopupContainerHead' class="modalhead" style="height:20%;" >
                <table>
                    <tr>
                        <td style="padding:15px 0 0 15px;">
                            <span>
                                SAVE PROMOTION LIST
                            </span>
                        </td>                        
                        <td style="padding:15px 0 0 2px;">  
                            <span id="sp_rmk_cnt">(0/80)</span>
                        </td>
                    </tr>
                </table>                
            </div>
            <div id='PopupContainerBody' class="modalbody" style="height:59%;" >
                <table>
                    <tr>
                        <td>
                           <%-- <textarea id="txtPromoDescription2" 
                                   placeholder="Remarks"
                                   class="inputs"
                                   style="width: 387px; height: 103px; 
                                   font-family: Helvetica,sans-serif,Segoe UI,Helvetica Neue;
                                   font-size:14px;
								   resize: none;"
                                   maxlength="80"                                     
                                   onkeyup = "character_counter(this,'sp_rmk_cnt',80);"
                            ></textar--%>ea>
                        </td>   
                    </tr>
                     <tr>
                        <td>
                            <input id="txtPrepareby" 
                                   placeholder="Prepared By"
                                   class="inputs"
                                   style="height:30px;
                                          width:387px;
                                          display:none;"
                                   readonly = "readonly"
                            />
                        </td>
                    </tr>
                </table>
            </div>
            <div id='PopupContainerFoot' class="modalfoot" style="height:20%;" >
                <table style="width:100%;" >
                    <tr>                                         
                        <td style="text-align:right;">
                            <input id="btnSaveTransaction" type="button" value="OK" class="button-primary"/>
                            <input id="btnCancelSave" type="button" value="Cancel" class="button-primary" onclick="hideModal('PopUpOutline');" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>  
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


      <%--loading--%>  
        <div class="modalPage" id="loading">
            <div class="modalBackground" ></div>
           <%-- <div class="modalContainer">--%>
                 <div style="position:absolute;z-index:750; top:0; left:0; bottom:0; right:0; padding:0 45% 0 45%; -moz-opacity:80 opacity: 80;" id="loadingcontainer">
                    <img src="./images/ajax-loader-big.gif" alt="Loading"/>
                 </div> 
             <%--</div>--%>
        </div> 

      <%--hidden fields--%>
         <input id="hfHeadId" type="hidden" runat="server" clientidmode="Static"/>
         <input id="hfPromoType" type="hidden" runat="server" clientidmode="Static"/>
      <%--end of hidden fields--%>
       <script src="Scripts/plugins/moment/moment.min.js" type="text/javascript"></script>
       <script src="Scripts/plugins/pikaday/js/pikaday.js" type="text/javascript"></script>
       <script src="Scripts/general.js" type="text/javascript"></script>
      <script src="Scripts/sellingAreaCreate.js" type="text/javascript"></script>
    <script src="Scripts/plugins/keyboard-delimiter/js/keyboard-delimiter.min.js" type="text/javascript"></script>
    

</asp:Content>
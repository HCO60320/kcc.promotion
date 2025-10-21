<%@ Page Language="C#" AutoEventWireup="true" CodeFile="login.aspx.cs" Inherits="login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Initializing User Access</title>
    <link href="Styles/Site.css" rel="stylesheet" type="text/css" />
    <link href="Scripts/plugins/notifit/css/notifIt.min.css" rel="stylesheet" type="text/css" />
</head>
<body>
    
     <div style="position:absolute;z-index:750; top:0; left:0; bottom:0; right:0; padding:10% 45% 10% 45%; -moz-opacity:80 opacity: 80;" id="loadingcontainer">
                    <img src="./images/ajax-loader-big.gif" alt="Loading"/>
     </div> 
    
   <%--<form id="form1" runat="server">
            <div class="loginbody"> 
                <table style= "width:100%;">            
                    <tr>
                        <td style= "width:50%;height:140px;">
                        </td>
                        <td>
                        </td>
                        <td style= "width:50%;height:140px;">
                        </td>
                    </tr>
                    <tr>
                        <td style= "width:50%;">
                        </td>

                        <td>
                            <div id="Outline"  class="loginOutline"  >
                                <div id= "LoginBorder" class="loginBack">    
            
                                </div>                  
                                <div id="LoginBody" class="loginMainBody">
                                    <div id='PopupContainerHead' class="modalhead" style="height:15%;" >
                                        <table>
                                            <tr>
                                                <td style="padding:10px 0 0 15px;">
                                                    <span>
                                                        User Login
                                                    </span>
                                                </td>
												<td style="padding:10px 0 0 1px;">
                                                    <span id="spconntype"></span>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div id='PopupContainerBody' class="modalbody" style="height:69%;" >
                                        <table>
                                            <tr>
                                                <td>
                                                    <input id="txtusr" 
                                                           placeholder="User name"
                                                           class="inputs"
                                                           style="height:30px;
                                                                  width:296px;"
                                                           type="text"
                                                           tabindex = '1'

                                                    />
                                                </td>
                                            </tr>
                                             <tr>
                                                <td>
                                                    <input id="txtpwd" 
                                                           placeholder="Password"
                                                           class="inputs"
                                                           style="height:30px;
                                                                  width:296px;"
                                                            type="password"
                                                            tabindex = '2'
                                                    />
                                                </td>
                                            </tr>
                                             <tr>
                                                <td>
                                                    <select id="cmbApplication" style="width:100%;height:23px;" class="inputs"  tabindex = '3'>
      
                                                        <option value="152">Promotion</option>
                                    
                                                    </select>
                                                </td>
                                            </tr>
											<tr align="left">
                                                <td style="padding-top:40px">
                                                    <a id='lnkUserGuide' href="#">User Guide</a>  
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div id='PopupContainerFoot' class="modalfoot" style="height:15%;" >
                                        <table style="width:100%;" >
                                            <tr>                       
                                                <td>
                                           
                                                    <span>Version: 2.0.0.9</span>
                                                </td>                   
                                                <td style="text-align:right;">
                                                    <input id="btnOk" type="button" value="OK"  tabindex = '3'class="button-primary" />
                                                    <input id="btnClear" type="button" value="Clear"  tabindex = '4' class="button-primary"/>                                                    
                                                </td>
                                            </tr>
                                        </table>
                                    </div>          
                                </div> 
                            </div>
                        </td>
                        
                        <td style= "width:50%;">
                        </td>
                    </tr>
                    <tr>
                        <td style= "width:50%;height:140px;">
                        </td>
                        <td>
                        </td>
                        <td style= "width:50%;height:140px;">
                        </td>
                    </tr>
                </table>                
            </div>
			
			<div id="PopUpConnectionOutline"  class="modalPage" >
				<div id= "PopConnectioneBackground" class="modalBackground"></div>  
				<div id= "PopConnectionContainer" class="modalContainer" style="height: 130px;width:200px; margin: 16% 43%;">
					<div id='PopupConnectionContainerHead' class="modalhead" style="height:30%;" >
						<table>
							<tr>
								<td style="padding:15px 0 0 15px;">
									<span>Select Connection Type</span>
								</td>
							</tr>
						</table>                
					</div>
					<div id='PopupConnectionContainerBody' class="modalbody" style="height:50%;" >
						<table class="rowEditorbody">
							<tr>                        
								<td style="width:200px;padding: 5px 0px 0px;"> 
									<select id ='edt_setmultiple' style="width:180px">
										<option value='1'>DEVELOPMENT</option>
										<option value='2'>PRODUCTION</option>
									</select>
								</td>
							</tr>
						</table>
					</div>
					<div id='Div1' class="modalfoot" style="height:24px;" >
					
					</div>
				</div>  
			</div>
			
        </form>--%>
</body>

<script src="Scripts/jquery.min.js" type="text/javascript"></script>
<script src="Scripts/plugins/notifit/js/notifIt.min.js" type="text/javascript"></script>
<script src="Scripts/general.js" type="text/javascript"></script>
<script src="Scripts/login.js" type="text/javascript"></script>

</html>

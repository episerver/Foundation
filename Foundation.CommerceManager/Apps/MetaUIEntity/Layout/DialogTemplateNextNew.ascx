<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DialogTemplateNextNew.ascx.cs" Inherits="Mediachase.UI.Web.Modules.DialogTemplateNextNew" %>
<%@ Import Namespace="Mediachase.Commerce.Shared" %>
<%@ Register TagPrefix="mc" Namespace="Mediachase.BusinessFoundation" Assembly="Mediachase.BusinessFoundation" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
    <link rel="shortcut icon" id="iconIBN" runat="server" type='image/x-icon' />
    <script type="text/javascript"></script>    
    <style type="text/css">
        .ibn-BigTable {
          width: 100%;
          height: 100%;
          border: 0;
        }
    </style>
    <script type="text/javascript" defer="defer">
    	function pageLoad() {
    		var obj = document.getElementById('ibn_divWithLoadingRss');
    		if (obj) {
    			obj.style.display = 'none';
    		}
    	}
    </script>        
    <link type="text/css" rel="stylesheet" href="<%# CommerceHelper.GetAbsolutePath("~/Apps/Shell/Styles/css/FilterBuilder.css")%>" />
    <link type="text/css" rel="stylesheet" href="<%# CommerceHelper.GetAbsolutePath("~/Apps/MetaDataBase/Styles/Calendar.css")%>" />
    
    <!-- EPi Style-->
    <link href="<%# CommerceHelper.GetAbsolutePath("~/Apps/Shell/EPi/Shell/Light/Shell-ext.css") %>" rel="stylesheet" type="text/css" />
</head>
<body class="ibn-WhiteBg">	
    <form id="frmMain" runat="server" method="post">
	
	<asp:ScriptManager ID="ScriptManager1" runat="server" ScriptMode="Release" EnablePartialRendering="true" EnableScriptGlobalization="true" EnableScriptLocalization="true" LoadScriptsBeforeUI="false" EnableCdn="true"></asp:ScriptManager>
	<div id='ibn_divWithLoadingRss' style="position: absolute; left: 0px; top: 0px; height: 100%; width: 100%; background-color: White; z-index: 10000">
		<div style="left: 40%; top: 40%; height: 30px; width: 200px; position: absolute; z-index: 10001">
			<div style="position: relative;  z-index: 10002">
				<img style="position: absolute; left: 40%; top: 40%; z-index: 10003" src='<%= ResolveClientUrl("~/Apps/MetaDataBase/images/loading_rss.gif") %>' border='0' />
			</div>
		</div>
	</div>	    
    <asp:placeholder id="phMain" runat="server"></asp:placeholder>
    <mc:CommandManager ID="cm" runat="server" ContainerId="divContainer" />
	<div id="divContainer" runat="server"></div>
    </form>
</body>
</html>

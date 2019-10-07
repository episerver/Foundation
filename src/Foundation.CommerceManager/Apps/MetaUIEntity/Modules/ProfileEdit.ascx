<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProfileEdit.ascx.cs" Inherits="Mediachase.Ibn.Web.UI.MetaUIEntity.Modules.ProfileEdit" %>
<%@ Import Namespace="Mediachase.Commerce.Shared" %>
<%@ Register TagPrefix="ibn" Assembly="Mediachase.BusinessFoundation" Namespace="Mediachase.BusinessFoundation" %>

<link rel="stylesheet" type="text/css" href="<%= CommerceHelper.GetAbsolutePath("~/Apps/MetaDataBase/styles/Theme.css") %>" />
<link rel="stylesheet" type="text/css" href="<%= CommerceHelper.GetAbsolutePath("~/Apps/MetaDataBase/styles/calendar.css") %>" />

<!-- EPi Style -->
<link rel="stylesheet" type="text/css" href="<%= CommerceHelper.GetAbsolutePath("~/Apps/Shell/EPi/Shell/Light/Shell-ext.css") %>"/>

<style type='text/css'>
	.text, .ibn-nav td, .ibn-nav, .ibn-input 
	{
		font-family: "Lucida Grande","Lucida Sans Unicode",Arial,Verdana,Sans-Serif;
		font-size:1em;
	}
	.tab
	{
		background:#dcdcdc url(../../Shell/EPi/Shell/Resources/Gradients.png) repeat-x left -2300px;
		_background:#dcdcdc none;
		border-top:1px solid #4d4d4d;
		border-right:1px solid #4d4d4d;
		border-left:1px solid #4d4d4d;
		border-bottom:1px solid #4d4d4d;
		color:#1d1d1d;
		text-shadow:#f8f8f8 0 1px 0;
		-moz-border-radius-topleft: 3px;
		-moz-border-radius-topright: 3px;
		text-align: center;
		padding: 2px 5px 2px 5px;
		margin-top:10px;
	}
	.tabSelected
	{
		text-align: center;
		font-weight:bold;
		color: #CE3431;
		background: #F8F8F8 url(../../Shell/EPi/Shell/Resources/Gradients.png) repeat-x scroll left -2300px;
		_background:#F8F8F8 none;
		border-bottom-color:#ffffff;
		-moz-border-radius-topleft: 3px;
		-moz-border-radius-topright: 3px;
	}
	.tabGap
	{
		border-left: none;
		border-top: none;
		border-right: none;
	}
	a.tabLink,
	a.tabLinkSelected,
	a.tabLink:hover
	{	
		font-family: "Lucida Grande","Lucida Sans Unicode",Arial,Verdana,Sans-Serif;
		Color: #1D1D1D;
		Font-size:  1em;
		Font-weight: normal;
		Margin: 0 0 0.2em 0;
		Padding: 0.2em 0.6em;
		Line-height: 1.4;
		text-shadow: #fff 0 1px 0;
		text-decoration: none;
	}
	a.tabLink:hover
	{
		background: none;
		cursor: pointer;
	}
	a.tabLinkSelected
	{	
		Font-weight: bold;
	}
	
	
	table.padTable5 tbody tr td
	{
		padding: 5px ! important;
	}
	table.padTable3 tbody tr td
	{
		padding: 3px ! important;
	}
	table.padTable2 tbody tr td
	{
		padding: 2px ! important;
	}
</style>

<script type="text/javascript">
function ChangeTab(id)
{
	var hfValue = document.getElementById('<%= hfValue.ClientID %>');
	
	var panel1 = document.getElementById('<%= panel1.ClientID %>');
	var panel2 = document.getElementById('<%= panel2.ClientID %>');
	var panel3 = document.getElementById('<%= panel3.ClientID %>');
	var panel4 = document.getElementById('<%= panel4.ClientID %>');
	
	var td1 = document.getElementById('<%= tdTab1.ClientID %>');
	var td2 = document.getElementById('<%= tdTab2.ClientID %>');
	var td3 = document.getElementById('<%= tdTab3.ClientID %>');
	var td4 = document.getElementById('<%= tdTab4.ClientID %>');
	
	var a1 = document.getElementById('aTab1');
	var a2 = document.getElementById('aTab2');
	var a3 = document.getElementById('aTab3');
	var a4 = document.getElementById('aTab4');
	
	if(panel1)
		panel1.style.display = "none";
	if(panel2)
		panel2.style.display = "none";
	if(panel3)
		panel3.style.display = "none";
	if(panel4)
		panel4.style.display = "none";
	
	if(td1)
		td1.className = "tab";
	if(td2)
		td2.className = "tab";
	if(td3)
		td3.className = "tab";
	if(td4)
		td4.className = "tab";
	
	if(a1)
		a1.className = "tabLink";
	if(a2)
		a2.className = "tabLink";
	if(a3)
		a3.className = "tabLink";
	if(a4)
		a4.className = "tabLink";
		
	if(id == "Panel1")
	{
		td1.className = "tabSelected";
		a1.className = "tabLinkSelected";
		panel1.style.display = "";
		hfValue.value = "Panel1";
	}
	else if(id == "Panel2")
	{
		td2.className = "tabSelected";
		a2.className = "tabLinkSelected";
		panel2.style.display = "";
		hfValue.value = "Panel2";
	}
	else if(id == "Panel3")
	{
		td3.className = "tabSelected";
		a3.className = "tabLinkSelected";
		panel3.style.display = "";
		hfValue.value = "Panel3";
	}
	else if(id == "Panel4")
	{
		td4.className = "tabSelected";
		a4.className = "tabLinkSelected";
		panel4.style.display = "";
		hfValue.value = "Panel4";
	}
	resizeTable();
}

function resizeTable()
{
	var panel1 = document.getElementById('<%= panel1.ClientID %>');
	var panel2 = document.getElementById('<%= panel2.ClientID %>');
	var panel3 = document.getElementById('<%= panel3.ClientID %>');
	var panel4 = document.getElementById('<%= panel4.ClientID %>');
	
	var hfValue = document.getElementById('<%= hfValue.ClientID %>');
	
	var obj;
	if(hfValue.value == "Panel1")
		obj = panel1;
	else if(hfValue.value == "Panel2")
		obj = panel2;
	else if(hfValue.value == "Panel3")
		obj = panel3;
	else if(hfValue.value == "Panel4")
		obj = panel4;
		
	var toolbarRow = document.getElementById('tableHeader');

	var intHeight = 0;
	if (typeof(window.innerWidth) == "number")
	{
	  intHeight = window.innerHeight;
	}
	else if (document.documentElement && (document.documentElement.clientWidth || document.documentElement.clientHeight))
	{
	  intHeight = document.documentElement.clientHeight;
	}
	else if (document.body && (document.body.clientWidth || document.body.clientHeight))
	{
	  intHeight = document.body.clientHeight;
	}

	if(obj && toolbarRow && (intHeight - toolbarRow.offsetHeight - 40)>0)
		obj.style.height = (intHeight - toolbarRow.offsetHeight - 40) + "px";
} 
window.onresize=resizeTable; 
window.onload=resizeTable; 
</script>
<table id="tableHeader" cellspacing="0" cellpadding="0" border="0" style="border-width:0px;width:100%;border-collapse:collapse;">
	<tr>
		<td>
			<table cellspacing="0" cellpadding="0" border="0" style="border-width:0px;width:100%;margin-top:5px;">
				<tr>
					<td class="ibn-stylebox tabGap">&nbsp;</td>
					<td id="tdTab3" runat="server" class="tab" style="width:120px;white-space:nowrap;width:120px;">
						<nobr>&nbsp;<a id="aTab3" class='tabLink' href="javascript:ChangeTab('Panel3');"><%=GetGlobalResourceObject("Global", "GeneralTab").ToString()%></a>&nbsp;</nobr>
					</td>
					<td class="ibn-stylebox tabGap">&nbsp;</td>
					<td id="tdTab1" runat="server" class="tab" style="width:120px;white-space:nowrap;width:120px;">
						<nobr>&nbsp;<a id="aTab1" class='tabLink' href="javascript:ChangeTab('Panel1');"><%=GetGlobalResourceObject("Global", "FieldsTab").ToString()%></a>&nbsp;</nobr>
					</td>
					<td class="ibn-stylebox tabGap" id="tab4Gap" runat="server">&nbsp;</td>
					<td id="tdTab4" runat="server" class="tab" style="width:120px;white-space:nowrap;width:120px;">
						<nobr>&nbsp;<a id="a4" class='tabLink' href="javascript:ChangeTab('Panel4');"><%=GetGlobalResourceObject("Global", "GroupsTab").ToString()%></a>&nbsp;</nobr>
					</td>
					<td class="ibn-stylebox tabGap" id="tab2Gap" runat="server">&nbsp;</td>
					<td id="tdTab2" runat="server" class="tab" style="width:120px;white-space:nowrap;width:120px;">
						<nobr>&nbsp;<a id="aTab2" class='tabLink' href="javascript:ChangeTab('Panel2');"><%=GetGlobalResourceObject("Global", "FiltersTab").ToString()%></a>&nbsp;</nobr>
					</td>
					<td class="ibn-stylebox tabGap">&nbsp;</td>
					<td class="ibn-stylebox tabGap" width="100%">&nbsp;</td>
				</tr>
			</table>
		</td>
	</tr>
</table>
<asp:Panel ID="panel1" runat="server">
	<table width="100%" class="text">
		<tr>
			<td valign="top" align="center">
				<div style="padding-top:15px;">
					<ibn:ListToListSelector runat="server" ID="ListSelector" AutoPostBack="true"
						TargetListId="lbVisibleColumns"
						SourceListId="lbAllColumns"
						OneItemToSourceButtonId="btnHide"
						OneItemToTargetButtonId="btnShow"
						ItemUpButtonId="btnUp"
						ItemDownButtonId="btnDown"></ibn:ListToListSelector>
					<table border="0" class="tablePadding3">
						<tr>
							<td style="width: 250px"><asp:Literal ID="Literal1" runat="server" Text='<%$Resources : GlobalMetaInfo, AvailableColumns %>' />:</td>
							<td style="width: 25px"></td>
							<td style="width: 250px"><asp:Literal ID="Literal2" runat="server" Text='<%$Resources : GlobalMetaInfo, VisibleColumns %>' />:</td>
							<td style="width: 25px"></td>		
						</tr>
						<tr>
							<td style="width: 250px">
								<asp:ListBox runat="server" ID="lbAllColumns" Width="250px" Height="150px"></asp:ListBox>
							</td>
							<td align="center">
								<asp:Button runat="server" ID="btnShow" Text=">" OnClientClick="return false;" /> <br /><br />
								<asp:Button runat="server" ID="btnHide" Text="<" OnClientClick="return false;"/> 
							</td>		
							<td style="width: 250px">
								<asp:ListBox runat="server" ID="lbVisibleColumns" Width="250px" Height="150px"></asp:ListBox>
							</td>
							<td>
								<asp:Button runat="server" ID="btnUp" text="&uarr;"  OnClientClick="return false;" Width="16px" /> <br /><br />
								<asp:Button runat="server" ID="btnDown" text="&darr;" OnClientClick="return false;" Width="16px" />
							</td>
						</tr>
					</table>
				</div>
			</td>
		</tr>
	</table>
</asp:Panel>
<asp:Panel ID="panel2" runat="server">	
	<asp:UpdatePanel runat="server" ID="filterPanel" ChildrenAsTriggers="true" UpdateMode="Conditional">
		<ContentTemplate>
			<ibn:FilterExpressionBuilder runat="server" ID="ctrlFilter" />
		</ContentTemplate>
	</asp:UpdatePanel>
	<asp:UpdateProgress runat="server" ID="progressFilterPanel" AssociatedUpdatePanelID="filterPanel" DisplayAfter="800" DynamicLayout="false">
		<ProgressTemplate>
			<div style="position: absolute; left: 0px; top: 0px; height: 100%; width: 100%; background-color: White;">
				<div style="position: relative;  z-index: 10002; height: 100%;">
					<img alt="" style="position: absolute; left: 40%; top: 40%; z-index: 10003" src='<%= ResolveClientUrl("~/Apps/MetaDataBase/images/loading_rss.gif") %>' border='0' />
				</div>
			</div>
		</ProgressTemplate>
	</asp:UpdateProgress>
</asp:Panel>
<asp:Panel ID="panel3" runat="server">
	<table width="100%" class="text">
		<tr>
			<td valign="top" align="center">
				<div style="width:70%;padding-top:15px;" class="episerveroverwriteimage">
					<ibn:BlockHeaderLight ID="lgdFinish" runat="server" HeaderCssClass="ibn-toolbar-light episerverBorder"  />
					<table width="100%" class="ibn-stylebox-light padTable5">
						<tr>
							<td width="50%" valign="top">
								<asp:TextBox ID="txtTitle" runat="server" Width="200px" CssClass="text"></asp:TextBox><asp:RequiredFieldValidator ID="rfTitle" runat="server" ControlToValidate="txtTitle" CssClass="text" Display="Dynamic"></asp:RequiredFieldValidator>
								<br />
								<br />
								<asp:CheckBox ID="cbIsPublic" runat="server" />
							</td>
							<td valign="top" align="center" width="60">
								<img alt="" src='<% =ResolveClientUrl("~/Apps/MetaDataBase/images/quicktip.gif") %>' border="0" />
							</td>
							<td class="text" style="PADDING-RIGHT: 15px" valign="top"><asp:Literal ID="Literal3" runat="server" Text="<%$Resources: Global, NewViewGeneralTabComment%>" /></td>
						</tr>
					</table>
				</div>
			</td>
		</tr>
	</table>
</asp:Panel>
<asp:Panel ID="panel4" runat="server">
	<table width="100%" class="text">
		<tr>
			<td valign="top" align="center">
				Grouping here
			</td>
		</tr>
	</table>
</asp:Panel>
<asp:Panel Width="100%" runat="server" ID="divButtons" style="border-top:1px solid #cccccc;padding-top: 7px;">
	<table width="100%">
		<tr>
			<td style="width:200px;">
				<asp:ValidationSummary ID="valSum" runat="server" CssClass="text" />
			</td>
			<td align="left">
				<asp:Button ID="btnSave" runat="server" Text="Save" Width="90px" OnClick="btnSave_Click" />&nbsp;&nbsp;&nbsp;
				<asp:Button ID="btnClose" runat="server" Text="Close" Width="90px" />
			</td>
		</tr>
	</table>
</asp:Panel>
<asp:HiddenField ID="hfValue" runat="server" />
<script type="text/javascript" src="<%= CommerceHelper.GetAbsolutePath("~/Apps/Shell/EPi/Shell/Light/jquery.min.js") %>"></script>
<script type="text/javascript">
	$(document).ready(function(){
		var imgLeft = $('div.episerveroverwriteimage table:first td:first img');
		if (imgLeft){
			imgLeft.attr('src','../../Shell/EPi/Shell/Resources/leftCorner.GIF');
		}
		var imgRight = $('div.episerveroverwriteimage table:first td:last img');
		if (imgRight){
			imgRight.attr('src','../../Shell/EPi/Shell/Resources/rightCorner.GIF');
		}
		var imgLine = $('div.episerveroverwriteimage table:first td[background]');
		if (imgLine){
			imgLine.attr('background','../../Shell/EPi/Shell/Resources/linehz.GIF');
		}
	});
</script>

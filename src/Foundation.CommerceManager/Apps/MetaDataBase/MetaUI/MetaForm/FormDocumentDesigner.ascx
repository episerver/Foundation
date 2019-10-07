<%@ Control Language="C#" AutoEventWireup="true" Inherits="Mediachase.Ibn.Web.UI.MetaUI.FormDocumentDesigner" Codebehind="FormDocumentDesigner.ascx.cs" %>
<%@ Import Namespace="Mediachase.Commerce.Shared" %>
<%@ Reference Control="~/Apps/Core/Controls/MetaToolbar.ascx" %>
<%@ Register TagPrefix="ibn2" TagName="MetaToolbar" Src="~/Apps/Core/Controls/MetaToolbar.ascx" %>
<%@ Register TagPrefix="mc" Namespace="Mediachase.BusinessFoundation.MetaForm" Assembly="Mediachase.BusinessFoundation" %>
<%@ Register TagPrefix="mc2" Namespace="Mediachase.Commerce.Manager.Apps.Common.Design" Assembly="Mediachase.ConsoleManager" %>
<link type="text/css" rel="Stylesheet" href='<%= CommerceHelper.GetAbsolutePath("/Apps/MetaDataBase/styles/Theme.css") %>' />
<!-- EPi Style -->
<link href="<%= CommerceHelper.GetAbsolutePath("~/Apps/Shell/EPi/Shell/Light/Shell-ext.css") %>" rel="stylesheet" type="text/css" />
	
<style type="text/css">
	body{
		background-color: #f1f1f1;
	}
	table.imageMiddle tbody tr td em button
	{
		background-position: 3pt 2px ! important;
	}
	.filter
	{
	 background-color: #fbfbfb;
	}
	.whiteBg
	{
		background-color: #FFFFFF;
	}
	.bottomBorder
	{
		border-bottom: solid 2px #6B92CE;
	}
	.noBottomBorder
	{
	 border: solid 1px #6B92CE; 
	 border-bottom-width:0px;
	}
	.noTopBorder
	{
	 border: solid 1px #6B92CE; 
	 border-top-width:0px;
	}
</style>

<table cellspacing="0" cellpadding="0" border="0" width="100%" class="ibn-stylebox2noBottom">
<tr>
	<td colspan="2" class="bottomBorder">
		<ibn2:MetaToolbar runat="server" ID="topMetaBar" />
	</td>
</tr>
<tr>
	<td class="filter" style="padding: 9px 7px 3px 15px;" colspan="2">
		<table width="100%" cellspacing="0" cellpadding="7" border="0" class="ibn-propertysheet" style="table-layout:fixed">
		  <tr>
			<td style="width:120px;"><b><asp:Literal ID="TableLabel" runat="server" Text='<%$Resources : MetaForm, Table %>' />:</b></td>
			<td style="width:250px;">
				<asp:Label ID="lblTableName" runat="server"></asp:Label>
			</td>
			<td style="width:80px;"><b><asp:Literal ID="Literal6" runat="server" Text='<%$Resources : MetaForm, FormName %>' />:</b></td>
			<td>
				<asp:Label ID="lblFormName" runat="server"></asp:Label>
			</td>
		  </tr>
		 </table>
	</td>
</tr>
<tr>
	<td colspan="2" class="filter" style="padding:7px;padding-bottom:0px;">
		<div class="noBottomBorder">
			<ibn2:MetaToolbar runat="server" ID="MainMetaToolbar" />
		</div>
	</td>
</tr>
<tr>
	<td valign="top" style="padding:7px;padding-top:0px;" class="filter">
		<div class="whiteBg noTopBorder">
			<mc:FormRenderer ID="fRenderer" runat="server" TableLayoutMode="Designer">
				<SectionHeaderTemplate>
					<mc2:BlockHeaderLight HeaderCssClass="ibn-toolbar-light" id="bhl" runat="server" Title='<%# Eval("Title") %>'></mc2:BlockHeaderLight>
				</SectionHeaderTemplate>
			</mc:FormRenderer>
		</div>
		<mc:TableLayoutControlExtender SelectedCssClass="designTableLayoutItemSelected" HoverCssClass="designTableLayoutItemHover" ID="cntrlExt" runat="server" TargetControlID="fRenderer" />
	</td>
	<td class="rightCustomTd text" valign="top" id="tdRight" runat="server">
		<div class="rightCustomHeader"><asp:Literal ID="Literal1" runat="server" Text='<%$Resources : MetaForm, CommonTasks %>' /></div>
		<div class="rightCustomPanel ibn-propertysheet" style="height:100%">
			<table width="100%" cellspacing="0" cellpadding="2" border="0">
			  <tr>
				<td><b><asp:Literal ID="Literal2" runat="server" Text='<%$Resources : MetaForm, Table %>' />:</b></td>
				<td colspan="2">
					<asp:DropDownList CausesValidation="false" AutoPostBack="true" ID="ddClasses" runat="server" Width="135px" OnSelectedIndexChanged="ddClasses_SelectedIndexChanged"></asp:DropDownList>
					<asp:Label ID="lblTempClassName" runat="server"></asp:Label>
				</td>
			  </tr>
			  <tr>
				<td><b><asp:Literal ID="Literal3" runat="server" Text='<%$Resources : MetaForm, FormName %>' />:</b></td>
				<td colspan="2">
					<asp:DropDownList ID="ddFormName" runat="server" CausesValidation="false" AutoPostBack="true" Width="135px" OnSelectedIndexChanged="ddFormName_SelectedIndexChanged"></asp:DropDownList>
					<asp:Label ID="lblTempFormName" runat="server"></asp:Label>
				</td>
			  </tr>
			  <tr>
				<td></td>
				<td></td>
				<td align="right">
					<asp:Label ID="lblNewForm" runat="server"></asp:Label>
					<asp:LinkButton ID="lbNewForm" runat="server" OnClick="lbNewForm_Click"></asp:LinkButton>
				</td>
			  </tr>
			</table>
			<hr />
			<table cellspacing="0" cellpadding="0" align="center" id="tblMove" runat="server">
			  <tr>
				<td style="width:26px"></td>
				<td style="width:26px">
				  <asp:Image runat="server" ID="imgUp" />
				  <asp:LinkButton ID="lbTop" runat="server" OnClick="lbTop_Click"></asp:LinkButton>
				</td>
				<td style="width:26px"></td>
			  </tr>
			  <tr>
				<td>
				  <asp:Image runat="server" ID="imgLeft" />
				  <asp:LinkButton ID="lbLeft" runat="server" OnClick="lbLeft_Click"></asp:LinkButton>
				</td>
				<td></td>
				<td>
				  <asp:Image runat="server" ID="imgRight" />
				  <asp:LinkButton ID="lbRight" runat="server" OnClick="lbRight_Click"></asp:LinkButton>
				</td>
			  </tr>
			  <tr>
				<td></td>
				<td>
				  <asp:Image runat="server" ID="imgDown" />
				  <asp:LinkButton ID="lbDown" runat="server" OnClick="lbDown_Click"></asp:LinkButton>
				</td>
				<td></td>
			  </tr>
			</table>
			<hr />
			<table cellspacing="0" cellpadding="0" width="100%" id="tblLinks" runat="server">
				<tr>
					<td>
						<ul class="rightDesigner">
							<li><asp:Label ID="lblEditForm" runat="server"></asp:Label></li>
							<li><asp:Label ID="lblNewSection" runat="server"></asp:Label></li>
						</ul>
						<hr />
						<b><asp:Literal ID="Literal4" runat="server" Text='<%$Resources : MetaForm, SelectedElements %>' />:</b>
						<ul class="rightDesigner">
							<li><asp:Label ID="lblEditSection" runat="server"></asp:Label></li>
							<li><asp:Label ID="lblAddField" runat="server"></asp:Label></li>
							<li><asp:Label ID="lblEditField" runat="server"></asp:Label></li>
							<li><asp:LinkButton ID="lbRemoveSection" runat="server" Text='<%$Resources : MetaForm, RemoveSection %>' OnClick="lbRemoveSection_Click"></asp:LinkButton></li>
							<li><asp:LinkButton ID="lbRemoveField" runat="server" Text='<%$Resources : MetaForm, RemoveField %>' OnClick="lbRemoveField_Click"></asp:LinkButton></li>
						</ul>
						<hr />
						<ul class="rightDesigner">
							<li><asp:LinkButton ID="lbSave" runat="server" Text='<%$Resources : MetaForm, Save %>' OnClick="lbSave_Click"></asp:LinkButton></li>
							<li><asp:LinkButton ID="lbSaveClose" runat="server" Text='<%$Resources : MetaForm, SaveClose %>' OnClick="lbSaveClose_Click"></asp:LinkButton></li>
							<li><asp:LinkButton ID="lbReCreate" runat="server" Text='<%$Resources : MetaForm, RecreateForm %>' OnClick="lbReCreate_Click"></asp:LinkButton></li>
						</ul>
					</td>
				</tr>
			</table>
			<asp:LinkButton ID="lbAddSection" runat="server" OnClick="lbAddSection_Click"></asp:LinkButton>
		</div>
	</td>
</tr>
</table>
<asp:HiddenField ID="hFieldKey" runat="server" />
<%--<div style="padding-top:10px;">
	<asp:TextBox ID="txtXml" runat="server" TextMode="MultiLine" Width="1024px" Height="400px"></asp:TextBox>
</div>
<asp:Button ID="btnSave2" runat="server" Text="Save Xml" OnClick="btnSave2_Click" />--%>
<script type="text/javascript">
	function OpenFDDPopUp(link, w, h, sel)
	{
		var sUid;
		if(sel)
		{
			sUid = $find(sel).getSelection();
			if(sUid && sUid != "0")
				link = link + "&itemUid=" + sUid;
			else
				return;
		}
		if(!w)
			w = 350;
		if(!h)
			h = 300;
		var l = (screen.width - w) / 2;
		var t = (screen.height - h) / 2;
		winprops = 'scrollbars=0, resizable=0, height='+h+',width='+w+',top='+t+',left='+l;
		var f = window.open(link, '_blank', winprops);
	}
	
	function BeforeDelete()
	{
		var sel = $find('<%=fRenderer.TableContainer %>');
		if(sel && sel.storage)
			sel.storage.value = "";
	}
	
	function onSaveClick(item)
	{
		<%=this.Page.ClientScript.GetPostBackClientHyperlink(lbSave, "") %>
	}
	
	function onSaveCloseClick(item)
	{
		<%=this.Page.ClientScript.GetPostBackClientHyperlink(lbSaveClose, "") %>
	}
	
	function onReCreateClick(item)
	{
		if(confirm('<%=GetGlobalResourceObject("MetaForm", "RecreateFormWarning").ToString() %>'))
		{
			$find('<%=fRenderer.TableContainer %>').get_storage().value = "";
			<%=this.Page.ClientScript.GetPostBackClientHyperlink(lbReCreate, "") %>
		}
		else 
			return false;
	}
	
	function onRemoveClick(item)
	{
		var uid = $find('<%=fRenderer.TableContainer %>').getSelection();
		var classname = $find('<%=fRenderer.TableContainer %>').getClassName();
		if(classname == "TableLayout")
		{
			if(confirm('<%=GetGlobalResourceObject("MetaForm", "WarningSection").ToString() %>'))
			{
				BeforeDelete();
				__doPostBack('<%=lbRemoveSection.UniqueID %>', uid);
			}
			else 
				return false;
		}
		else if(classname == "SmartTableLayout")
		{
			if(confirm('<%=GetGlobalResourceObject("MetaForm", "WarningField").ToString() %>'))
			{
				BeforeDelete();
				__doPostBack('<%=lbRemoveField.UniqueID %>', uid);
			}
			else 
				return false;
		}
		else
			return false;
		
	}
	
	function onAddFieldClick(item)
	{
		var link = '<%=this.ResolveClientUrl("~/Apps/MetaDataBase/MetaUI/Pages/Public/FormItemEdit.aspx") %>';
		link = link + '?add=1&uid=';
		var obj = document.getElementById('<%=hFieldKey.ClientID %>');
		link = link + obj.value;
		link = link + '&btn=<%=lbAddSection.UniqueID %>';
		OpenFDDPopUp(link, 350, 550, '<%=fRenderer.TableContainer %>');
	}
	
	function onAddSectionClick(item)
	{
		var link = '<%=this.ResolveClientUrl("~/Apps/MetaDataBase/MetaUI/Pages/Public/FormSectionEdit.aspx") %>';
		link = link + '?uid=';
		var obj = document.getElementById('<%=hFieldKey.ClientID %>');
		link = link + obj.value;
		link = link + '&btn=<%=lbAddSection.UniqueID %>';
		OpenFDDPopUp(link, 350, 520, null);
	}
	
	function onEditClick(item)
	{
		var classname = $find('<%=fRenderer.TableContainer %>').getClassName();
		if(classname == "TableLayout")
		{
			var link = '<%=this.ResolveClientUrl("~/Apps/MetaDataBase/MetaUI/Pages/Public/FormSectionEdit.aspx") %>';
			link = link + '?uid=';
			var obj = document.getElementById('<%=hFieldKey.ClientID %>');
			link = link + obj.value;
			link = link + '&btn=<%=lbAddSection.UniqueID %>';
			OpenFDDPopUp(link, 350, 520, '<%=fRenderer.TableContainer %>');
		}
		else if(classname == "SmartTableLayout")
		{
			var link = '<%=this.ResolveClientUrl("~/Apps/MetaDataBase/MetaUI/Pages/Public/FormItemEdit.aspx") %>';
			link = link + '?uid=';
			var obj = document.getElementById('<%=hFieldKey.ClientID %>');
			link = link + obj.value;
			link = link + '&btn=<%=lbAddSection.UniqueID %>';
			OpenFDDPopUp(link, 350, 550, '<%=fRenderer.TableContainer %>');
		}
		else
			return false;
	}
	
	function onEditFormClick(item)
	{
		var link = '<%=this.ResolveClientUrl("~/Apps/MetaDataBase/MetaUI/Pages/Public/FormDocumentEdit.aspx") %>';
		link = link + '?uid=';
		var obj = document.getElementById('<%=hFieldKey.ClientID %>');
		link = link + obj.value;
		link = link + '&btn=<%=lbAddSection.UniqueID %>&class=<%=MetaClassName %>';
		OpenFDDPopUp(link, 350, 460, null);
	}
	
	function onArrowClick(item)
	{
		var btnId = "";
		if(item.id == "leftButton")
			btnId = '<%=lbLeft.UniqueID %>';
		else if(item.id == "upButton")
			btnId = '<%=lbTop.UniqueID %>';
		else if(item.id == "rightButton")
			btnId = '<%=lbRight.UniqueID %>';
		else if(item.id == "downButton")
			btnId = '<%=lbDown.UniqueID %>';
		var uid = $find('<%=fRenderer.TableContainer %>').getSelection();
		if(uid && btnId != "")
			__doPostBack(btnId, uid);
		else
			return false;
	}
</script>
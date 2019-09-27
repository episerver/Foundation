<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EntitySelect.ascx.cs" Inherits="Mediachase.Ibn.Web.UI.MetaUIEntity.Modules.EntitySelect" %>
<%@ Import Namespace="Mediachase.Commerce.Shared" %>
<%@ Register TagPrefix="mc" TagName="EntityGrid" Src="~/Apps/MetaUIEntity/Grid/EntityGrid.ascx" %>
<%@ Register TagPrefix="mc" TagName="MCGridAction" Src="~/Apps/MetaUIEntity/Grid/MetaGridServerEventAction.ascx" %>
<%@ Register TagPrefix="mc2" Assembly="Mediachase.BusinessFoundation" Namespace="Mediachase.BusinessFoundation" %>
<%@ Register TagPrefix="mc" TagName="FormView" Src="~/Apps/MetaDataBase/MetaUI/MetaForm/FormDocumentView.ascx" %>
<link rel="stylesheet" type="text/css" href='<%= CommerceHelper.GetAbsolutePath("~/Apps/MetaUIEntity/styles/grid.css") %>' />
 <!-- EPi Style-->
<link href="<%= CommerceHelper.GetAbsolutePath("~/Apps/Shell/EPi/Shell/Light/Shell-ext.css") %>" rel="stylesheet" type="text/css" />
<script type="text/javascript">
	var resizeFlag = false;
	function LayoutResizeHandler(sender, eventArgs)
	{
		var intHeight = 0;
		if (typeof(window.innerHeight) == "number")
		{
			intHeight = window.innerHeight;
		}
		else if (document.documentElement && document.documentElement.clientHeight)
		{
			intHeight = document.documentElement.clientHeight;
		}
		else if (document.body && document.body.clientHeight)
		{
			intHeight = document.body.clientHeight;
		}
		var obj = document.getElementById('<%=tblNew.ClientID %>');
		if(obj)
		{	
			var divFV = document.getElementById("divDormView");
			if(intHeight)
				divFV.style.height = (intHeight - 50) + "px";
		}
		var divBT = document.getElementById("divBottom");
		if(divBT)
		{
			divBT.style.top = (intHeight - 50) + "px";
		}
	}
	
	function CheckSelected()
	{
		var obj = $find('<%= grdMain.GridClientContainerId %>');
		var fl = true;
		if(obj)
		{
			var hdn = document.getElementById('<%=hdnValue.ClientID %>');
			if(obj.isCheckboxes())
			{
				if(!obj.isChecked())
				{
					if(obj.getSelectedElement() == "")
						fl = false;
					else
						hdn.value = obj.getSelectedElement();
				}
				else
					hdn.value = obj.getCheckedCollection();
			}
			else
			{
				if(obj.getSelectedElement() == "")
					fl = false;
				else
					hdn.value = obj.getSelectedElement();
			}
		}
		else
			fl = false;
		if(fl)
			<%=Page.ClientScript.GetPostBackClientHyperlink(lbSave, "") %>;
		else
			return false;
	}
</script>
<mc2:McDock ID="DockTop" runat="server" Anchor="top" EnableSplitter="False" DefaultSize="33">
	<DockItems>
		<table cellspacing="0" cellpadding="0" border="0" width="100%" class="filter">
			<tr>
				<td style="padding:7px;">
					<asp:UpdatePanel ID="upTop" runat="server" UpdateMode="Conditional">
						<ContentTemplate>
							<table class="ibn-propertysheet" width="100%" border="0" cellpadding="5" cellspacing="0">
								<tr>
									<td class="ibn-value">
										<asp:DropDownList runat="server" ID="ddFilter" AutoPostBack="true" Width="200px" />&nbsp;
										<asp:Button ID="btnNew" runat="server" Width="70px" CssClass="text" OnClick="btnNew_Click" />
									</td>
									<td class="ibn-value" align="right" >
										<asp:TextBox runat="server" ID="tbSearchString" Width="200px"></asp:TextBox>
										<asp:ImageButton Runat="server" id="btnSearch" Width="16" Height="16" ImageUrl="~/Apps/MetaDataBase/images/search.gif" ImageAlign="AbsMiddle" OnClick="btnSearch_Click" />
										<asp:ImageButton runat="server" ID="btnClear" Width="19" Height="17" ImageUrl="~/Apps/MetaDataBase/images/reset17.gif" ImageAlign="AbsMiddle" OnClick="btnClear_Click" />
									</td>
								</tr>
							</table>
						</ContentTemplate>
						<Triggers>
							<asp:PostBackTrigger ControlID="btnNew" />
						</Triggers>
					</asp:UpdatePanel> 
				</td>
			</tr>
		</table>
	</DockItems>
</mc2:McDock>	
<table id="tblSelect" runat="server" style="margin-top:0px; padding-top: 0px; table-layout: fixed;" cellspacing="0" cellpadding="0" border="0" width="100%" class="ibn-propertysheet">
	<tr>
		<td style="padding-left: 5px;" class="filter">
			<asp:UpdatePanel ID="grdMainPanel" runat="server" UpdateMode="Conditional">
				<ContentTemplate>
					<mc:EntityGrid ID="grdMain" runat="server" />	
					<mc:MCGridAction runat="server" ID="ctrlGridEventUpdater" PlaceName="" ViewName="" ClassName=""  />
				</ContentTemplate>
			</asp:UpdatePanel>
		</td>
	</tr>
</table>
<table id="tblNew" runat="server" style="margin-top:0px; padding-top: 0px; table-layout: fixed;" cellspacing="0" cellpadding="0" border="0" width="100%" class="ibn-propertysheet">
	<tr>
		<td class="filter">
			<div id="divDormView" class="NormalPriority" style="overflow:auto;height:360px;">
				<mc:FormView ID="docFormView" runat="server" />
			</div>
		</td>
	</tr>
</table>
<div style="position:absolute; width:100%; height:50px; top:365px;z-index:10;" id="divBottom">
	<table cellspacing="0" cellpadding="0" border="0" width="100%" class="filter">
		<tr>
			<td style="padding:10px; text-align:right;">
				<mc2:IMButton id="btnSave" runat="server" style="width:105px;" OnServerClick="btnSave_ServerClick"></mc2:IMButton>&nbsp;
				<mc2:IMButton id="btnCancel" runat="server" style="width:105px;" OnServerClick="btnCancel_ServerClick" CausesValidation="false"></mc2:IMButton>
			</td>
		</tr>
	</table>
</div>
<mc2:McDock ID="DockBottom" runat="server" Anchor="Bottom" EnableSplitter="false" DefaultSize="50">
	<DockItems>
		<div>&nbsp;</div>
	</DockItems>
</mc2:McDock>
<asp:HiddenField ID="hdnValue" runat="server" />
<asp:LinkButton ID="lbSave" runat="server" Visible="false" OnClick="lbSave_Click"></asp:LinkButton>

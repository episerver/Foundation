<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Publish.ascx.cs" Inherits="Mediachase.Commerce.Manager.Apps.Customization.Modules.Publish" %>
<%@ Register TagPrefix="ibn" Assembly="Mediachase.BusinessFoundation" Namespace="Mediachase.BusinessFoundation" %>
<%@ Register TagPrefix="mc" Namespace="Mediachase.FileUploader.Web.UI" Assembly = "Mediachase.FileUploader" %>
<style type="text/css">
#DataTable td
{
	padding:5px;
}
#DataTable td span.rightColumn
{
	display: none;
}
.rightColumn
{
	position: absolute;
	right: 15px;
	color: #666666!important;
}
.nodeCls, .x-panel-bwrap .x-panel-body
{
	position: relative;
}
* 
{
	zoom: 1;	
}
.MenuTree
{
	display: none;
}
</style>
<script type="text/javascript">
function SelectNode(node, args)
{
	var path = node.id;
	var text = node.text;
	while (node.parentNode && node.parentNode.id && node.parentNode.id != "FL_root_id")
	{
		node = node.parentNode;
		path = node.id + "/" + path;
	}
	
	var hiddenField = document.getElementById('<%= NodeIdField.ClientID%>');
	if (hiddenField)
		hiddenField.value = path;
		
	var selectedItemLabel = document.getElementById('<%= SelectedItemLabel.ClientID %>');
	if (selectedItemLabel)
	{
		selectedItemLabel.innerHTML = text;
		selectedItemLabel.className = "";
	}
}

function InitMenuTree()
{
	var menuTree = $find('<%=MenuTree.ClientID%>');
	if (menuTree && menuTree.extTreePanel)
		menuTree.extTreePanel.on('click', SelectNode);
}
</script>
<table cellspacing="0" cellpadding="0" border="0" width="100%" style="margin-top:5px;">
	<tr>
		<td style="width:300px; padding-left:8px;">
			<div class="ibn-stylebox2" style="height:415px; position:relative; top:1px;bottom: 1px;">
				<ibn:JsTreePanel FillToContainer="true" AutoScroll="true" id="MenuTree" runat="server" IconCss="iconStdCls" NodeTextCss="nodeCls" CssClass="MenuTree"></ibn:JsTreePanel>				
			</div>
			
		</td>
		<td valign="top" style="padding-right:8px; padding-left:8px;">
			<table cellpadding="0" cellspacing="3" width="100%" id="DataTable">
				<tr>
					<td class="ibn-label" style="width:120px;">
						<asp:Literal ID="Literal1" runat="server" Text="<%$ Resources: GlobalMetaInfo, DisplayRegion %>"></asp:Literal>:
					</td>
					<td> 
						<asp:Label runat="server" ID="SelectedItemLabel" Text="<%$ Resources: GlobalMetaInfo, ItemNotSelected %>" CssClass="ibn-error"></asp:Label>
					</td>
				</tr>
				<tr>
					<td class="ibn-label">
						<asp:Literal ID="Literal2" runat="server" Text="<%$ Resources: GlobalMetaInfo, DisplayText %>"></asp:Literal>:
					</td>
					<td>
						<asp:TextBox runat="server" ID="ItemText" Width="280"></asp:TextBox>
						<asp:RequiredFieldValidator runat="server" ID="ItemTextRequiredValidator" ControlToValidate="ItemText" Display="static" ErrorMessage="*" Font-Bold="true"></asp:RequiredFieldValidator>
					</td>
				</tr>
				<tr>
					<td class="ibn-label">
						<asp:Literal ID="Literal3" runat="server" Text="<%$ Resources: GlobalMetaInfo, DisplayOrder %>"></asp:Literal>:
					</td>
					<td>
						<asp:TextBox runat="server" ID="ItemOrder" Width="280" Text="10000"></asp:TextBox>
						<asp:RequiredFieldValidator runat="server" ID="ItemOrderRequiredValidator" ControlToValidate="ItemOrder" Display="dynamic" ErrorMessage="*" Font-Bold="true">
						</asp:RequiredFieldValidator><asp:RangeValidator runat="server" ID="ItemOrderRangeValidator" ControlToValidate="ItemOrder" CultureInvariantValues="true" Type="Integer" MinimumValue="0" MaximumValue="999999" ErrorMessage="*" Display="dynamic" Font-Bold="true"></asp:RangeValidator>
					</td>
				</tr>
				<tr>
					<td class="ibn-label">
						<asp:Literal ID="Literal4" runat="server" Text="<%$ Resources: GlobalMetaInfo, ClientScript %>"></asp:Literal>:
					</td>
					<td>
						<asp:TextBox runat="server" ID="ClientScript" Width="280"></asp:TextBox>
						<asp:RequiredFieldValidator runat="server" ID="ClientScriptRequiredFieldValidator" ControlToValidate="ClientScript" Display="static" ErrorMessage="*" Font-Bold="true"></asp:RequiredFieldValidator>
					</td>
				</tr>
				<tr>
					<td class="ibn-label">
						<asp:Literal ID="Literal6" runat="server" Text="<%$ Resources: GlobalMetaInfo, Permissions %>"></asp:Literal>:
					</td>
					<td>
						<asp:TextBox runat="server" ID="ItemPermissions" Width="280"></asp:TextBox>
					</td>
				</tr>
				<tr>
					<td class="ibn-label">
						<asp:Literal ID="Literal5" runat="server" Text="<%$ Resources: GlobalMetaInfo, MenuItemIcon %>"></asp:Literal>:
					</td>
					<td>
						<mc:mchtmlinputfile id="fAssetFile" CssClass="text" runat="server" Width="280px" />
					</td>
				</tr>
				<tr>
					<td colspan="2" align="center">
						<br /><br /><br /><br />
						<ibn:IMButton ID="PublishButton" runat="server" class="text" Text="<%$ Resources: GlobalMetaInfo, Publish %>" OnServerClick="PublishButton_ServerClick" width="150px"/>
						<br /><br />
						<ibn:IMButton ID="CloseButton" runat="server" class="text" Text="<%$ Resources: GlobalMetaInfo, Close %>" IsDecline="true" width="150px"/>
					</td>
				</tr>
			</table>
		</td>
	</tr>
</table>
<asp:HiddenField runat="server" ID="NodeIdField" />
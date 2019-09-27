<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LeftMenuItemEdit.ascx.cs" Inherits="Mediachase.Commerce.Manager.Apps.Customization.Modules.LeftMenuItemEdit" %>
<%@ Register TagPrefix="ibn" Namespace="Mediachase.BusinessFoundation" Assembly="Mediachase.BusinessFoundation" %>
<%@ Register TagPrefix="mc" Namespace="Mediachase.FileUploader.Web.UI" Assembly = "Mediachase.FileUploader" %>
<table cellpadding="0" cellspacing="7" width="100%" style="table-layout: fixed;" border="0">
	<tr>
		<td class="ibn-label" valign="top">
			<asp:Literal ID="Literal2" runat="server" Text="<%$ Resources: GlobalMetaInfo, DisplayText %>"></asp:Literal>:
		</td>
		<td style="width:300px;">
			<asp:TextBox runat="server" ID="ItemText" Width="280"></asp:TextBox>
			<asp:RequiredFieldValidator runat="server" ID="ItemTextRequiredValidator" ControlToValidate="ItemText" Display="static" ErrorMessage="*" Font-Bold="true"></asp:RequiredFieldValidator>
		</td>
	</tr>
	<tr runat="server" id="ItemUrlRow">
		<td class="ibn-label">
			<asp:Literal ID="ItemUrlLiteral" runat="server" Text="<%$ Resources: GlobalMetaInfo, MenuItemLink %>"></asp:Literal>:
		</td>
		<td>
			<asp:TextBox runat="server" ID="ItemUrl" Width="280"></asp:TextBox>
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
			<asp:Literal ID="Literal6" runat="server" Text="<%$ Resources: GlobalMetaInfo, Permissions %>"></asp:Literal>:
		</td>
		<td>
			<asp:TextBox runat="server" ID="ItemPermissions" Width="280"></asp:TextBox>
		</td>
	</tr>
	<tr>
		<td class="ibn-label" valign="top">
			<asp:Literal ID="Literal5" runat="server" Text="<%$ Resources: GlobalMetaInfo, MenuItemIcon %>"></asp:Literal>:
		</td>
		<td>
			<mc:mchtmlinputfile id="fAssetFile" CssClass="text" runat="server" Width="280px" />
			
			<table cellpadding="0" cellspacing="0">
				<tr style="padding-top:10px">
					<td align="center" valign="middle">
						<div style="text-align:left;">
							<img alt="" id="imgPhoto" runat="server" style="border-width:0;" src="" />
						</div>
					</td>
					<td valign="top" style="padding-left:10px; width:25px">
						<asp:LinkButton ID="DeleteButton" Runat="server" Visible="false" CausesValidation="False" onclick="DeleteButton_Click"></asp:LinkButton>
					</td>
				</tr>	
			</table>
		</td>
	</tr>
	<tr>
	    <td align="right" style="padding-right:10px;" colspan="2">
            <asp:Button runat="server" ID="SaveButton" Text="<%$ Resources:Common, btnOk %>" Width="80px" OnClick="SaveButton_ServerClick" />
            &nbsp;&nbsp;&nbsp;
            <asp:Button runat="server" ID="CancelButton" Text="<%$ Resources:Common, btnCancel %>" Width="80px" CausesValidation="false" />
        </td>
	</tr>
</table>
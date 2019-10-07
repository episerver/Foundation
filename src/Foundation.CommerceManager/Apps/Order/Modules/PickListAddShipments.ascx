<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PickListAddShipments.ascx.cs" Inherits="Mediachase.Commerce.Manager.Apps.Order.Modules.PickListAddShipments" %>
<%@ Register TagPrefix="mc2" Assembly="Mediachase.BusinessFoundation" Namespace="Mediachase.BusinessFoundation" %>
<%-- TODO: Move style to *.css --%>
<style type="text/css">
	#contentPanel
	{
		padding: 10px;
	}
	.option-title td
	{
		font-weight: bold;
	}
	.option-value td
	{
		padding-top: 5px;
	}
	.option-value input
	{
		padding: 2px;
		width: 200px;
		color: Black;
	}
	.option-value select
	{
		padding: 2px;
		width: 204px;
	}
	.separator td
	{
		height: 10px;
	}
	.buttons
	{
		margin-top: 20px;
		text-align: center;
		vertical-align: bottom;
	}
	.buttons button
	{
		width: 90px;
	}
</style>
<table>
	<tr class="option-title">
		<td>
			<asp:RadioButton runat="server" ID="createNewOption" GroupName="PickList" Checked="true" />
		</td>
		<td>
			<asp:Label ID="Label1" runat="server" AssociatedControlID="createNewOption" Text="<%$ Resources:OrderStrings, Create_New_Pick_List %>"></asp:Label>
		</td>
	</tr>
	<tr class="option-value">
		<td>
			&nbsp;
		</td>
		<td>
			<asp:Label ID="Label2" runat="server" AssociatedControlID="listName" Text="<%$ Resources:OrderStrings, List_Name %>"></asp:Label>:
			<asp:TextBox runat="server" ID="listName"></asp:TextBox>
		</td>
	</tr>
	<tr class="separator">
		<td>
		</td>
	</tr>
	<tr class="option-title">
		<td>
			<asp:RadioButton runat="server" ID="addToExistingOption" GroupName="PickList" />
		</td>
		<td>
			<asp:Label ID="Label3" runat="server" AssociatedControlID="addToExistingOption" Text="<%$ Resources:OrderStrings, Add_To_Pick_List %>"></asp:Label>
		</td>
	</tr>
	<tr class="option-value">
		<td>
			&nbsp;
		</td>
		<td>
			<asp:Label ID="Label4" runat="server" AssociatedControlID="list" Text="<%$ Resources:OrderStrings, List_Name %>"></asp:Label>:
			<asp:DropDownList runat="server" ID="list">
			</asp:DropDownList>
		</td>
	</tr>
    <tr>
        <td colspan="2">
            <asp:CustomValidator runat="server" ID="listNameValidator" ControlToValidate="listName" ValidateEmptyText="True" OnServerValidate="listNameCheck" 
            Font-Size="9pt" Font-Names="verdana" Display="Dynamic"
            ErrorMessage="<%$ Resources:OrderStrings, List_Name_Empty_Error %>" />
        </td>
    </tr>
</table>


<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrderGroupQuickEdit.ascx.cs" Inherits="Mediachase.Commerce.Manager.Apps.Order.Modules.OrderGroupQuickEdit" %>
<%@ Register TagPrefix="mc2" Assembly="Mediachase.BusinessFoundation" Namespace="Mediachase.BusinessFoundation" %>
<div style="padding: 5px;">
	<asp:Label runat="server" ID="lblErrorInfo" Style="color: Red"></asp:Label>
	<table width="100%" class="DataForm">
		<tr>
			<td class="FormLabelCell">
				<asp:Label ID="Label7" runat="server" Text="<%$ Resources:SharedStrings, Currency %>"></asp:Label>:
			</td>
			<td class="FormFieldCell">
				<asp:DropDownList runat="server" ID="ddOrderCurrencyList" DataMember="Currency" DataTextField="Name"
					DataValueField="CurrencyCode" Width="250px">
				</asp:DropDownList>
			</td>
		</tr>
        <tr>
            <td>
                <asp:Label runat="server" Text="<%$ Resources:OrderStrings, Currency_Change_Warning %>" />
            </td>
        </tr>
		<tr>
			<td align="right" colspan="2" style="padding-top: 10px; padding-right: 10px;">
				<mc2:IMButton ID="btnSave" runat="server" style="width: 105px;" OnServerClick="btnSave_ServerClick">
				</mc2:IMButton>
				&nbsp;
				<mc2:IMButton ID="btnCancel" runat="server" style="width: 105px;" CausesValidation="false">
				</mc2:IMButton>
			</td>
		</tr>
	</table>
</div>
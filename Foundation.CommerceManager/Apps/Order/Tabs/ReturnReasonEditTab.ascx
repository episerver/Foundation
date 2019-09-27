<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="ReturnReasonEditTab.ascx.cs" Inherits="Mediachase.Commerce.Manager.Order.Tabs.ReturnReasonEditTab" %>
<%@ Register Src="~/Apps/Core/Controls/BooleanEditControl.ascx" TagName="BooleanEditControl" TagPrefix="ecf" %>
<div id="DataForm">
    <table class="DataForm">
        <tr>
            <td class="FormLabelCell">
                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:SharedStrings, Return_Reason %>"></asp:Label>:
            </td>
            <td class="FormFieldCell">
                <asp:TextBox runat="server" ID="tbReturnReason" Width="300px" MaxLength="50"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ID="rfvReturnReason" ControlToValidate="tbReturnReason" Display="Dynamic" Text="<%$ Resources:OrderStrings, Return_Reason_Name_Required %>"></asp:RequiredFieldValidator>
                <asp:CustomValidator runat="server" ID="ReturnReasonCheckCustomValidator" ControlToValidate="tbReturnReason" OnServerValidate="ReturnReasonCheck" Display="Dynamic" ErrorMessage="<%$ Resources:OrderStrings, Return_Reason_Exists %>" />
                <asp:RegularExpressionValidator runat="server" id="ReasonNameValidator" EnableClientScript="false" ControlToValidate="tbReturnReason" ErrorMessage="<%$ Resources:OrderStrings, Invalid_Return_Reason_Name %>" Display="Dynamic" ValidationExpression="^[-' \w]{1,50}$" />
            </td>
        </tr>
        <tr>
            <td colspan="2" class="FormSpacerCell">
            </td>
        </tr>
        <tr>
            <td class="FormLabelCell">
                <asp:Label ID="Label11" runat="server" Text="<%$ Resources:SharedStrings, Sort_Order %>"></asp:Label>:
            </td>
            <td class="FormFieldCell">
                <asp:TextBox runat="server" Width="50" ID="SortOrder" Text="0"></asp:TextBox>
                <asp:RequiredFieldValidator ID="SortOrderRequiredValidator" runat="server" ControlToValidate="SortOrder" ErrorMessage="<%$ Resources:SharedStrings, Sort_Order_Required %>" Display="Dynamic"></asp:RequiredFieldValidator>
                <asp:RangeValidator ID="SortOrderRangeValidator" runat="server" Type="Integer" ControlToValidate="SortOrder" MinimumValue="-2147483648" MaximumValue="2147483647" ErrorMessage="<%$ Resources:SharedStrings, Sort_Order_Invalid %>" Display="Dynamic"></asp:RangeValidator>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="FormSpacerCell">
            </td>
        </tr>
        <tr>
            <td class="FormLabelCell">
                <asp:Label ID="Label5" runat="server" Text="<%$ Resources:SharedStrings, Visible %>"></asp:Label>:
            </td>
            <td class="FormFieldCell">
                <ecf:BooleanEditControl ID="IsVisible" runat="server"></ecf:BooleanEditControl>
            </td>
        </tr>
    </table>
</div>

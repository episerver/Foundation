<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="CountryEditTab.ascx.cs" Inherits="Mediachase.Commerce.Manager.Order.Tabs.CountryEditTab" %>
<%@ Register Src="~/Apps/Core/Controls/BooleanEditControl.ascx" TagName="BooleanEditControl" TagPrefix="ecf" %>
<div id="DataForm">
    <table class="DataForm">
        <tr>
            <td class="FormLabelCell">
                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:SharedStrings, Country_Name %>"></asp:Label>:
            </td>
            <td class="FormFieldCell">
                <asp:TextBox runat="server" ID="tbCountryName" Width="300px" MaxLength="100"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ID="rfvCountryName" ControlToValidate="tbCountryName" Display="Dynamic" Text="<%$ Resources:OrderStrings, Order_Country_Name_Required %>"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="FormSpacerCell">
            </td>
        </tr>
        <tr>
            <td class="FormLabelCell">
                <asp:Label ID="Label9" runat="server" Text="<%$ Resources:SharedStrings, Code %>"></asp:Label>:
            </td>
            <td class="FormFieldCell">
                <asp:TextBox runat="server" Width="50" ID="CodeText" MaxLength="3"></asp:TextBox>
                <asp:RequiredFieldValidator ID="CodeRequiredValidator" runat="server" ControlToValidate="CodeText" ErrorMessage="<%$ Resources:SharedStrings, Code_Required %>" Display="Dynamic"></asp:RequiredFieldValidator>
                <asp:CustomValidator runat="server" ID="CodeCheckCustomValidator" ControlToValidate="CodeText" OnServerValidate="CountryCodeCheck" Display="Dynamic" ErrorMessage="<%$ Resources:OrderStrings, Country_With_Code_Exists %>" />
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

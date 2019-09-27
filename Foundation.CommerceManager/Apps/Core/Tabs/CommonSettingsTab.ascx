<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CommonSettingsTab.ascx.cs" Inherits="Mediachase.Commerce.Manager.Core.Tabs.CommonSettingsTab" %>
<%@ Register Src="~/Apps/Core/Controls/BooleanEditControl.ascx" TagName="BooleanEditControl" TagPrefix="ecf" %>
<div id="DataForm">
    <table class="DataForm">
        <tr>
            <td class="FormLabelCell">
                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:SharedStrings, Default_Language %>"></asp:Label>:
            </td>
            <td class="FormFieldCell">
                <asp:DropDownList runat="server" ID="LanguagesList" AutoPostBack="false">
                    <asp:ListItem Value="" Text="<%$ Resources:SharedStrings, select_language %>"></asp:ListItem>
                </asp:DropDownList>
                <asp:RequiredFieldValidator runat="server" ID="LanguageRequired" ControlToValidate="LanguagesList" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                <br /><asp:Label ID="Label2" CssClass="FormFieldDescription" Text="<%$ Resources:CoreStrings, CommonSettings_Language_Description %>" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="FormSpacerCell">
            </td>
        </tr>
        <tr>
            <td class="FormLabelCell">
                <asp:Label ID="Label3" runat="server" Text="<%$ Resources:SharedStrings, Default_Currency %>"></asp:Label>:
            </td>
            <td class="FormFieldCell">
                <asp:DropDownList runat="server" ID="CurrenciesList" AutoPostBack="false">
                    <asp:ListItem Value="" Text="<%$ Resources:SharedStrings, select_currency %>"></asp:ListItem>
                </asp:DropDownList>
                <asp:RequiredFieldValidator runat="server" ID="CurrencyRequired" ControlToValidate="CurrenciesList" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                <br /><asp:Label ID="Label5" CssClass="FormFieldDescription" Text="<%$ Resources:CoreStrings, CommonSettings_Currency_Description %>" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="FormSpacerCell">
            </td>
        </tr>
        <tr>
            <td class="FormLabelCell">
                <asp:Label ID="Label6" runat="server" Text="<%$ Resources:SharedStrings, Default_Length_Unit %>"></asp:Label>:
            </td>
            <td class="FormFieldCell">
                <asp:DropDownList runat="server" ID="LengthUnitList" AutoPostBack="false">
                    <asp:ListItem Value="" Text="<%$ Resources:SharedStrings, select_length_unit %>"></asp:ListItem>
                </asp:DropDownList>
                <asp:RequiredFieldValidator runat="server" ID="LengthRequired" ControlToValidate="LengthUnitList" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                <br /><asp:Label ID="Label8" CssClass="FormFieldDescription" Text="<%$ Resources:CoreStrings, CommonSettings_Length_Unit_Description %>" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="FormSpacerCell">
            </td>
        </tr>
        <tr>
            <td class="FormLabelCell">
                <asp:Label ID="Label4" runat="server" Text="<%$ Resources:SharedStrings, Default_Weight_Unit %>"></asp:Label>:
            </td>
            <td class="FormFieldCell">
                <asp:DropDownList runat="server" ID="WeightUnitList" AutoPostBack="false">
                    <asp:ListItem Value="" Text="<%$ Resources:SharedStrings, select_weight_unit %>"></asp:ListItem>
                </asp:DropDownList>
                <asp:RequiredFieldValidator runat="server" ID="WeightRequired" ControlToValidate="WeightUnitList" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                <br /><asp:Label ID="Label7" CssClass="FormFieldDescription" Text="<%$ Resources:CoreStrings, CommonSettings_Weight_Unit_Description %>" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
</div>
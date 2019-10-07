<%@ Control Language="c#" Inherits="Mediachase.Commerce.Manager.Order.Payments.Tabs.PaymentMethodEditBaseTab" CodeBehind="PaymentMethodEditBaseTab.ascx.cs" %>
<%@ Register Src="~/Apps/Core/Controls/BooleanEditControl.ascx" TagName="BooleanEditControl" TagPrefix="ecf" %>
<%@ Register TagPrefix="console" Namespace="Mediachase.Web.Console.Controls" Assembly="Mediachase.WebConsoleLib" %>
<div id="DataForm">
    <table class="DataForm">
        <tr>
            <td class="FormLabelCell">
                <asp:Label ID="lblPaymentMethodIdText" runat="server" Text="<%$ Resources:SharedStrings, Id %>"></asp:Label>:
            </td>
            <td class="FormFieldCell">
                <asp:Label runat="server" ID="lblPaymentMethodId" CssClass="text"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="FormSpacerCell"></td>
        </tr>
        <tr>
            <td class="FormLabelCell">
                <asp:Label ID="lblName" runat="server" Text="<%$ Resources:SharedStrings, Name %>"></asp:Label>:
            </td>
            <td class="FormFieldCell">
                <asp:TextBox runat="server" ID="tbName" CssClass="text" Width="200px" MaxLength="120"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfName" runat="server" ErrorMessage="<%$ Resources:SharedStrings, Name_Required %>"
                    Font-Size="9pt" Font-Names="verdana" Display="Dynamic" ControlToValidate="tbName" />
            </td>
        </tr>
        <tr>
            <td colspan="2" class="FormSpacerCell"></td>
        </tr>
        <tr>
            <td class="FormLabelCell">
                <asp:Label ID="lblDescription" runat="server" Text="<%$ Resources:SharedStrings, Description %>"></asp:Label>:
            </td>
            <td class="FormFieldCell">
                <asp:TextBox runat="server" ID="tbDescription" Width="350px" Rows="4" TextMode="MultiLine"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="FormSpacerCell"></td>
        </tr>
        <tr>
            <td class="FormLabelCell" style="vertical-align: middle; width: 120px">
                <asp:Label ID="lblSystemKeyword" runat="server" Text="<%$ Resources:SharedStrings, System_Keyword %>"></asp:Label>:
            </td>
            <td class="FormFieldCell">
                <asp:TextBox runat="server" ID="tbSystemName" CssClass="text" Width="200px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfSystemName" runat="server" ErrorMessage="<%$ Resources:SharedStrings, System_Name_Required %>"
                    Font-Size="9pt" Font-Names="verdana" Display="Dynamic" ControlToValidate="tbSystemName" />
                <asp:RegularExpressionValidator runat="server" ID="revSystemName" ControlToValidate="tbSystemName" Display="Dynamic"
                    ValidationExpression="^[a-zA-Z]\S*$" ErrorMessage="<%$ Resources:SharedStrings, Latin_Symbols_Only %>" />
                <asp:CustomValidator runat="server" ID="SystemNameCustomValidator" ControlToValidate="tbSystemName" OnServerValidate="PaymentMethodSystemNameCheck" Display="Dynamic" ErrorMessage="<%$ Resources:SharedStrings, Payment_Method_Exists %>" />
            </td>
        </tr>
        <tr>
            <td colspan="2" class="FormSpacerCell"></td>
        </tr>
        <tr>
            <td class="FormLabelCell" style="vertical-align: middle; width: 120px">
                <asp:Label ID="lblLanguage" runat="server" Text="<%$ Resources:SharedStrings, Language %>"></asp:Label>:
            </td>
            <td class="FormFieldCell">
                <asp:DropDownList runat="server" ID="ddlLanguage">
                    <asp:ListItem Value="" Text="<%$ Resources:SharedStrings, select_language %>"></asp:ListItem>
                </asp:DropDownList>
                <asp:RequiredFieldValidator runat="server" ID="LanguageRequired" ControlToValidate="ddlLanguage" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="FormSpacerCell"></td>
        </tr>
        <tr>
            <td class="FormLabelCell" style="vertical-align: middle; width: 120px">
                <asp:Label ID="Label2" runat="server" Text="<%$ Resources:SharedStrings, Class_Name %>"></asp:Label>:
            </td>
            <td class="FormFieldCell">
                <asp:DropDownList runat="server" ID="ddlClassName">
                    <asp:ListItem Value="" Text="<%$ Resources:OrderStrings, Select %>"></asp:ListItem>
                </asp:DropDownList>
                <asp:RequiredFieldValidator runat="server" ID="ClassNameRequired" ControlToValidate="ddlClassName" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="FormSpacerCell"></td>
        </tr>
        <tr>
            <td class="FormLabelCell" style="vertical-align: middle; width: 120px">
                <asp:Label ID="Label6" runat="server" Text="<%$ Resources:OrderStrings, Payment_Implementation_Class_Name %>"></asp:Label>:
            </td>
            <td class="FormFieldCell">
                <asp:DropDownList runat="server" ID="ddlPaymentImplementationClassName">
                    <asp:ListItem Value="" Text="<%$ Resources:OrderStrings, Select %>"></asp:ListItem>
                </asp:DropDownList>
                <asp:RequiredFieldValidator runat="server" ID="ImplementationClassNameRequired" ControlToValidate="ddlPaymentImplementationClassName" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="FormSpacerCell"></td>
        </tr>
        <tr>
            <td class="FormLabelCell">
                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:SharedStrings, Sort_Order %>"></asp:Label>:</td>
            <td class="FormFieldCell">
                <asp:TextBox runat="server" ID="tbSortOrder"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ID="SortOrderRequiredValidator" ControlToValidate="tbSortOrder" ErrorMessage="<%$ Resources:SharedStrings, Sort_Order_Required %>" Display="Dynamic"></asp:RequiredFieldValidator>
                <asp:RangeValidator runat="server" ID="SortOrderRangeValidator" ControlToValidate="tbSortOrder" MinimumValue="0" MaximumValue="1000000000" Type="Integer" ErrorMessage="<%$ Resources:SharedStrings, Sort_Order_Invalid %>" Display="Dynamic"></asp:RangeValidator>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="FormSpacerCell"></td>
        </tr>
        <tr>
            <td class="FormLabelCell">
                <asp:Label ID="Label3" runat="server" Text="<%$ Resources:SharedStrings, IsActive %>"></asp:Label>:</td>
            <td class="FormFieldCell">
                <ecf:BooleanEditControl id="IsActive" runat="server"></ecf:BooleanEditControl>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="FormSpacerCell"></td>
        </tr>
        <tr>
            <td class="FormLabelCell">
                <asp:Label ID="Label4" runat="server" Text="<%$ Resources:SharedStrings, IsDefault %>"></asp:Label>:</td>
            <td class="FormFieldCell">
                <ecf:BooleanEditControl id="IsDefault" runat="server"></ecf:BooleanEditControl>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="FormSpacerCell"></td>
        </tr>
        <tr>
            <td class="FormLabelCell">
                <asp:Label ID="Label5" runat="server" Text="<%$ Resources:SharedStrings, Supports_Recurring %>"></asp:Label>:</td>
            <td class="FormFieldCell">
                <ecf:BooleanEditControl id="SupportsRecurring" runat="server"></ecf:BooleanEditControl>
            </td>
        </tr>

        <tr>
            <td class="FormSectionCell" colspan="2">
                <asp:Label runat="server" ID="lblShippingMethods" Text="<%$ Resources:SharedStrings, Restricted_Shipping_Methods %>"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="FormFieldCell" colspan="2">
                <console:DualList ID="ShippingMethodsList" runat="server" ListRows="6" EnableMoveAll="True" CssClass="text"
                    LeftDataTextField="Name" LeftDataValueField="ShippingMethodId" RightDataTextField="Name" RightDataValueField="ShippingMethodId"
                    ItemsName="Shipping Methods">
                    <RightListStyle Font-Bold="True" Width="200px" Height="150px"></RightListStyle>
                    <ButtonStyle Width="100px"></ButtonStyle>
                    <LeftListStyle Width="200px" Height="150px"></LeftListStyle>
                </console:DualList>
            </td>
        </tr>
    </table>
</div>

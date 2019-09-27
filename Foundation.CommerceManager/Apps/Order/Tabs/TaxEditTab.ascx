<%@ Control Language="c#" Inherits="Mediachase.Commerce.Manager.Order.Tabs.TaxEditTab" Codebehind="TaxEditTab.ascx.cs" %>
<%@ Register Src="~/Apps/Core/Controls/CalendarDatePicker.ascx" TagName="CalendarDatePicker" TagPrefix="ecf" %>
<div id="DataForm">
    <table class="DataForm">
        <tr>
            <td class="FormLabelCell">
                <asp:Label ID="lblName" runat="server" text="<%$ Resources:SharedStrings, Name %>"></asp:Label>:
            </td>
            <td class="FormFieldCell">
                <asp:TextBox runat="server" ID="tbName" MaxLength="50" Width="350px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfName" runat="server" ErrorMessage="<%$ Resources:SharedStrings, Name_Required %>"
                    Font-Size="9pt" Font-Names="verdana" Display="Dynamic" ControlToValidate="tbName" />
                <asp:CustomValidator runat="server" ID="NameCheckCustomValidator" ControlToValidate="tbName" OnServerValidate="NameCheck" Display="Dynamic" ErrorMessage="<%$ Resources:OrderStrings, Tax_With_Name_Exists %>" />
            </td>
        </tr>
        <tr>
            <td colspan="2" class="FormSpacerCell"></td>
        </tr>
        <tr>
            <td class="FormLabelCell">
                <asp:Label ID="Label4" runat="server" text="<%$ Resources:SharedStrings, Type %>"></asp:Label>:</td>
            <td class="FormFieldCell">
                <asp:DropDownList runat="server" ID="TaxTypeList">
                    <asp:ListItem Value="1" Text="<%$ Resources:SharedStrings, Sales %>"></asp:ListItem>
                    <asp:ListItem Value="2" Text="<%$ Resources:SharedStrings, Shipping%>"></asp:ListItem>
                </asp:DropDownList><br />
                <asp:Label ID="Label13" CssClass="FormFieldDescription" runat="server"><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:SharedStrings, Tax_Type_Description %>"/></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="FormSpacerCell"></td>
        </tr>
        <tr>  
            <td class="FormLabelCell">
                <asp:Label ID="Label6" runat="server" text="<%$ Resources:SharedStrings, Sort_Order %>"></asp:Label>:
            </td>
            <td class="FormFieldCell">
                <asp:TextBox runat="server" ID="tbSortOrder"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$ Resources:SharedStrings, Sort_Order_Required %>"
                    Font-Size="9pt" Font-Names="verdana" Display="Dynamic" ControlToValidate="tbSortOrder" />
                <asp:RangeValidator runat="server" ID="RangeValidator1" Display="Dynamic" ControlToValidate="tbSortOrder" MinimumValue="0" MaximumValue="1000000000" Type="Integer" ErrorMessage="<%$ Resources:SharedStrings, Sort_Order_Invalid %>"></asp:RangeValidator>
            </td>
        </tr>
        <tr>
            <td class="FormSectionCell" colspan="2">
                <asp:Label runat="server" ID="lblDisplayName" text="<%$ Resources:SharedStrings, Display_Name %>"></asp:Label>:
            </td>
        </tr>
		<asp:Repeater ID="LanguagesList" runat="server">
		    <ItemTemplate>
		        <tr>
                    <td class="FormLabelCell">
                        <asp:Label ID="lblLanguageCode" runat="server" Text='<%# Eval("FriendlyName") %>'></asp:Label>
                    </td>
			        <td class="FormFieldCell" colspan="2">
			            <asp:HiddenField runat="server" ID="hfLangCode" Value='<%# Eval("LanguageCode") %>' />
			            <asp:TextBox runat="server" ID="tbDisplayName" Text='<%# Eval("DisplayName") %>' Width="350px"></asp:TextBox>
			        </td>
		        </tr>
		    </ItemTemplate>
		</asp:Repeater>
    </table>
</div>
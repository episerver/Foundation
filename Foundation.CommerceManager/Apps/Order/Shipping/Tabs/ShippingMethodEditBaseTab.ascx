<%@ Control Language="c#" Inherits="Mediachase.Commerce.Manager.Order.Shipping.Tabs.ShippingMethodEditBaseTab" Codebehind="ShippingMethodEditBaseTab.ascx.cs" %>
<%@ Register Src="~/Apps/Core/Controls/BooleanEditControl.ascx" TagName="BooleanEditControl" TagPrefix="ecf" %>
<div id="DataForm">
    <table class="DataForm">
        <tr>
            <td class="FormLabelCell">
                <asp:Label ID="lblShippingMethodIdText" runat="server" Text="<%$ Resources:SharedStrings, ID %>"></asp:Label>:
            </td>
            <td class="FormFieldCell">
                <asp:Label runat="server" ID="lblShippingMethodId" CssClass="text"></asp:Label>
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
                <asp:TextBox runat="server" ID="tbName" CssClass="text" Width="200px" MaxLength="100"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfName" runat="server" ErrorMessage="<%$ Resources:SharedStrings, Name_Required %>"
                    Font-Size="9pt" Font-Names="verdana" Display="Dynamic" ControlToValidate="tbName" />
                    <asp:RegularExpressionValidator runat="server" ID="revName" ControlToValidate="tbName" Display="Dynamic"
                    ValidationExpression="^[a-zA-Z0-9\s]*$" ErrorMessage="<%$ Resources:SharedStrings, Latin_Symbols_Only %>" />
                <asp:RegularExpressionValidator ID="regexTextBox1" Text="<%$ Resources:SharedStrings, Name_MaxLength %>" 
                     ControlToValidate="tbName" runat="server" ValidationExpression="^[\s\S]{0,100}$"
                     Font-Size="9pt" Font-Names="verdana" Display="Dynamic"/>
                <asp:CustomValidator runat="server" ID="SystemNameCustomValidator" ControlToValidate="tbName" OnServerValidate="ShippingMethodNameCheck" Display="Dynamic" ErrorMessage="<%$ Resources:SharedStrings, Shipping_Method_Exists %>" />
            </td>
        </tr>
        <tr>
            <td colspan="2" class="FormSpacerCell"></td>
        </tr>
        <tr>
            <td class="FormLabelCell" style="vertical-align: middle; width: 120px">
                <asp:Label ID="lblFriendlyName" runat="server" Text="<%$ Resources:SharedStrings, Friendly_Name %>"></asp:Label>:
            </td>
            <td class="FormFieldCell">
                <asp:TextBox runat="server" ID="tbFriendlyName" CssClass="text" Width="200px" MaxLength="200"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfFriendlyName" runat="server" ErrorMessage="<%$ Resources:SharedStrings, Friendly_Name_Required %>"
                    Font-Size="9pt" Font-Names="verdana" Display="Dynamic" ControlToValidate="tbFriendlyName" />
                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" Text="<%$ Resources:SharedStrings, Friendly_Name_MaxLength %>" 
                    ControlToValidate="tbFriendlyName" runat="server" ValidationExpression="^[\s\S]{0,200}$"
                    Font-Size="9pt" Font-Names="verdana" Display="Dynamic"/>
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
                <asp:TextBox runat="server" ID="tbDescription" Width="350px" Rows="4" TextMode="MultiLine" MaxLength="225"></asp:TextBox>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" Text="<%$ Resources:SharedStrings, Description_MaxLength %>" 
                     ControlToValidate="tbDescription" runat="server" ValidationExpression="^[\s\S]{0,225}$"
                     Font-Size="9pt" Font-Names="verdana" Display="Dynamic"/>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="FormSpacerCell"></td>
        </tr>
        <tr>
            <td class="FormLabelCell" style="vertical-align: middle; width: 120px">
                <asp:Label ID="Label2" runat="server" Text="<%$ Resources:SharedStrings, Provider %>"></asp:Label>:
            </td>
            <td class="FormFieldCell">
                <asp:UpdatePanel UpdateMode="Conditional" ID="UpdatePanel1" runat="server" RenderMode="Inline">
                    <ContentTemplate>
                        <asp:DropDownList runat="server" id="ddlShippingOption" DataValueField="ShippingOptionId" DataTextField="Name">
                            <asp:ListItem Value="" Text="<%$ Resources:SharedStrings, select_shipping_option %>"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator runat="server" ID="ShippingOptionRequired" ControlToValidate="ddlShippingOption" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <!-- BEGIN: shipping option parameters -->
        <tr>
            <td class="FormFieldCell" colspan="2">
                <asp:UpdatePanel UpdateMode="Conditional" ID="ShippingOptionParametersContentPanel" runat="server" RenderMode="Inline">
                    <ContentTemplate>
                        <asp:placeholder id="phShippingOptionPatameters" Runat="server"></asp:placeholder>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <!-- END: shipping option parameters -->
        <tr>
            <td class="FormLabelCell" style="vertical-align: middle; width: 120px">
                <asp:Label ID="lblLanguage" runat="server" Text="<%$ Resources:SharedStrings, Language %>"></asp:Label>:
            </td>
            <td class="FormFieldCell">
                <asp:DropDownList runat="server" id="ddlLanguage">
                    <asp:ListItem Value="" Text="<%$ Resources:SharedStrings, select_language %>"></asp:ListItem>
                </asp:DropDownList>
                <asp:RequiredFieldValidator runat="server" ID="LanguageRequired" ControlToValidate="ddlLanguage" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="FormSpacerCell"></td>
        </tr>
        <tr>  
            <td class="FormLabelCell"><asp:Label ID="Label1" runat="server" Text="<%$ Resources:SharedStrings, Base_Price %>"></asp:Label>:</td> 
            <td class="FormFieldCell">
                <asp:TextBox runat="server" ID="tbBasePrice"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfBasePrice" runat="server" ErrorMessage="<%$ Resources:SharedStrings, Base_Price_Required %>"
                    Font-Size="9pt" Font-Names="verdana" Display="Dynamic" ControlToValidate="tbBasePrice" />
                <asp:RangeValidator runat="server" ID="rvBasePrice" Display="Dynamic" ControlToValidate="tbBasePrice" MinimumValue="0" MaximumValue="1000000000" Type="Currency" ErrorMessage="<%$ Resources:SharedStrings, Base_Price_Invalid %>"></asp:RangeValidator>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="FormSpacerCell"></td> 
        </tr>
        <tr>  
            <td class="FormLabelCell"><asp:Label ID="Label7" runat="server" Text="<%$ Resources:SharedStrings, Currency %>"></asp:Label>:</td> 
            <td class="FormFieldCell">
                <asp:DropDownList runat="server" id="ddlCurrency">
                    <asp:ListItem Value="" Text="<%$ Resources:SharedStrings, select_currency %>"></asp:ListItem>
                </asp:DropDownList>
                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="ddlCurrency" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="FormSpacerCell"></td>
        </tr>
        <tr>
            <td class="FormLabelCell"><asp:Label ID="Label3" runat="server" Text="<%$ Resources:SharedStrings, IsActive %>"></asp:Label>:</td> 
            <td class="FormFieldCell">
                <ecf:BooleanEditControl id="IsActive" runat="server"></ecf:BooleanEditControl>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="FormSpacerCell"></td>
        </tr>
        <tr>
            <td class="FormLabelCell"><asp:Label ID="Label5" runat="server" Text="<%$ Resources:SharedStrings, IsDefault %>"></asp:Label>:</td> 
            <td class="FormFieldCell">
                <ecf:BooleanEditControl id="IsDefault" runat="server"></ecf:BooleanEditControl>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="FormSpacerCell"></td>
        </tr>
        <tr>  
            <td class="FormLabelCell"><asp:Label ID="Label4" runat="server" Text="<%$ Resources:SharedStrings, Sort_Order %>"></asp:Label>:</td> 
            <td class="FormFieldCell">
                <asp:TextBox runat="server" ID="tbSortOrder"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ID="SortOrderRequiredValidator" ControlToValidate="tbSortOrder" ErrorMessage="<%$ Resources:SharedStrings, Sort_Order_Required %>" Display="Dynamic"></asp:RequiredFieldValidator>
                <asp:RangeValidator runat="server" ID="SortOrderRangeValidator" ControlToValidate="tbSortOrder" MinimumValue="0" MaximumValue="1000000000" Type="Integer" ErrorMessage="<%$ Resources:SharedStrings, Sort_Order_Invalid %>" Display="Dynamic"></asp:RangeValidator>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="FormSpacerCell"></td>
        </tr>
    </table>
</div>
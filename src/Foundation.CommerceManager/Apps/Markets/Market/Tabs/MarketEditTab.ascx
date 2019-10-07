<%@ Control Language="C#" AutoEventWireup="true" Inherits="Mediachase.Commerce.Manager.Market.Tabs.MarketEditTab" Codebehind="MarketEditTab.ascx.cs" %>
<%@ Register Assembly="Mediachase.WebConsoleLib" Namespace="Mediachase.Web.Console.Controls" TagPrefix="console" %>
<%@ Register Src="~/Apps/Core/Controls/HtmlEditControl.ascx" TagName="HtmlEditControl" TagPrefix="ecf" %>
<%@ Register Src="~/Apps/Core/Controls/BooleanEditControl.ascx" TagName="BooleanEditControl" TagPrefix="ecf" %>
<%--<script runat="server" language="c#">
    protected void CountryList_CountryMoved(Object sender, EventArgs e)
    {
        foreach (ListItem item in CountryList.RightItems)
        {
 			ddlCountryList.Items.Add(new ListItem(item.Value, item.Text));
		}
}
</script>--%>
<%--<script type="text/javascript">
        for (var li in CountryList.RightItems)
        {
 			ddlCountryList.Items.Add(new ListItem(item.Value, item.Text));
		}
</script>--%>

<div id="DataForm">
    <table>
        <tr>
            <td class="FormLabelCell">
                *<asp:Label runat="server" Text="<%$ Resources:SharedStrings, Market_ID %>"></asp:Label>:</td>
            <td class="FormFieldCell">
                <asp:TextBox runat="server" Width="250" MaxLength="8" ID="MarketID"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ID="MarketIDRequiredValidator" ControlToValidate="MarketID"
                    ErrorMessage="<%$ Resources:SharedStrings, Market_ID_Required %>" Display="Dynamic"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator id="MarketIDLengthValidator" runat="server"  ControlToValidate="MarketID" 
                    ErrorMessage="<%$ Resources:SharedStrings, Market_ID_Length_Exceeded %>" Display="Dynamic" ValidationExpression=".{1,8}"></asp:RegularExpressionValidator>
                <asp:RegularExpressionValidator runat="server" id="MarketIDValidator" ControlToValidate="MarketID" 
                    ErrorMessage="<%$ Resources:SharedStrings, Market_ID_Invalid %>" Display="Dynamic" ValidationExpression="^[a-zA-Z0-9_]*$" />
                <asp:CustomValidator runat="server" ID="MarketIDUniqueCustomValidator" ControlToValidate="MarketID"
                    OnServerValidate="MarketIDCheck" Display="Dynamic" ErrorMessage="<%$ Resources:SharedStrings, Market_ID_Already_Exists %>" />    
            </td>
        </tr>
        <tr>
            <td colspan="2" class="FormSpacerCell">
            </td>
        </tr>
        <tr>
            <td class="FormLabelCell">
                <asp:Label runat="server" Text="<%$ Resources:SharedStrings, Market_Name %>"></asp:Label>:</td>
            <td class="FormFieldCell">
                <asp:TextBox runat="server" Width="250" MaxLength="50" ID="MarketName"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ID="MarketNameRequiredValidator" ControlToValidate="MarketName"
                        ErrorMessage="<%$ Resources:SharedStrings, Market_Name_Required %>" Display="Dynamic"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator runat="server" id="MarketNameValidator" ControlToValidate="MarketName" 
                        ErrorMessage="<%$ Resources:SharedStrings, Market_Name_Invalid %>" Display="Dynamic" ValidationExpression="^[- \w]*$" />
                <asp:CustomValidator runat="server" ID="MarketNameUniqueCustomValidator" ControlToValidate="MarketName"
                    OnServerValidate="MarketNameCheck" Display="Dynamic" ErrorMessage="<%$ Resources:SharedStrings, Market_Name_Already_Exists %>" /> 
            </td>
        </tr>
        <tr>
            <td class="FormLabelCell">
                <asp:Label  runat="server" Text="<%$ Resources:SharedStrings, Market_Description %>"></asp:Label>:</td>
            <td class="FormFieldCell">
                <asp:TextBox runat="server" TextMode="MultiLine" Width="250" MaxLength="255" ID="MarketDescription"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="FormSpacerCell">
            </td>
        </tr>
        <tr>
            <td class="FormLabelCell">
                <asp:Label runat="server" Text="<%$ Resources:SharedStrings, Is_Active %>"></asp:Label>:</td>
            <td class="FormFieldCell">
                <ecf:BooleanEditControl ID="IsMarketActive" runat="server"></ecf:BooleanEditControl>
            </td>
        </tr>
        <tr>
            <td class="FormLabelCell">
                <asp:Label runat="server" Text="<%$ Resources:SharedStrings, Prices_Include_Tax %>"></asp:Label>:</td>
            <td class="FormFieldCell">
                <ecf:BooleanEditControl ID="IsPricesIncludeTax" runat="server"></ecf:BooleanEditControl>&nbsp;
                <asp:Label runat="server" CssClass="FormFieldDescription" Text="<%$ Resources:SharedStrings, Prices_Include_Tax_Information %>"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="FormSpacerCell">
            </td>
        </tr>
        <%-- Countries --%>
        <tr>
            <td colspan="2" class="FormSectionCell"><asp:Literal ID="Literal2" runat="server" 
                Text="Countries"/></td>
        </tr>        
        <tr>
            <td class="FormFieldCell" colspan="2">
                <console:DualList ID="CountryList" runat="server" ListRows="6" EnableMoveAll="True"
                    CssClass="text" LeftDataTextField="Text" LeftDataValueField="Value"
                    RightDataTextField="Text" RightDataValueField="Value" ItemsName="Countries" OnItemsMoved="CountryList_CountryMoved">
                    <RightListStyle Font-Bold="True" Width="200px" Height="150px"></RightListStyle>
                    <ButtonStyle Width="100px"></ButtonStyle>
                    <LeftListStyle Width="200px" Height="150px"></LeftListStyle>
                </console:DualList>
            </td>
        </tr>
        <tr runat="server" id="CountryTableRow" visible="false">
            <td class="FormLabelCell">
                <asp:Label ID="Label3" runat="server" Text="<%$ Resources:SharedStrings, Default_Country %>"></asp:Label>:</td>
            <td class="FormFieldCell">
                <asp:DropDownList runat="server" ID="ddlCountryList">
                    <asp:ListItem Value="" Text="<%$ Resources:SharedStrings, Empty_Parenthesis %>"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <%-- Languages --%>
        <tr>
            <td colspan="2" class="FormSectionCell"><asp:Literal ID="Literal5" runat="server" 
                Text="Languages"/></td>
        </tr>        
        <tr>
            <td class="FormFieldCell" colspan="2">
                <console:DualList ID="LanguageList" runat="server" ListRows="6" EnableMoveAll="True"
                    CssClass="text" LeftDataTextField="Text" LeftDataValueField="Value"
                    RightDataTextField="Text" RightDataValueField="Value" ItemsName="Languages">
                    <RightListStyle Font-Bold="True" Width="200px" Height="150px"></RightListStyle>
                    <ButtonStyle Width="100px"></ButtonStyle>
                    <LeftListStyle Width="200px" Height="150px"></LeftListStyle>
                </console:DualList>
            </td>
        </tr>
        <tr runat="server" id="LanguageTableRow">
            <td class="FormLabelCell">
                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:SharedStrings, Default_Language %>"></asp:Label>:</td>
            <td class="FormFieldCell">
                <asp:DropDownList runat="server" ID="ddlLanguageList">
                    <asp:ListItem Value="" Text="<%$ Resources:SharedStrings, Empty_Parenthesis %>"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <%-- Currencies --%>
        <tr>
            <td colspan="2" class="FormSectionCell"><asp:Literal ID="Literal1" runat="server" 
                Text="Currencies"/></td>
        </tr>        
        <tr>
            <td class="FormFieldCell" colspan="2">
                <console:DualList ID="CurrencyList" runat="server" ListRows="6" EnableMoveAll="True"
                    CssClass="text" LeftDataTextField="Text" LeftDataValueField="Value"
                    RightDataTextField="Text" RightDataValueField="Value" ItemsName="Currencies">
                    <RightListStyle Font-Bold="True" Width="200px" Height="150px"></RightListStyle>
                    <ButtonStyle Width="100px"></ButtonStyle>
                    <LeftListStyle Width="200px" Height="150px"></LeftListStyle>
                </console:DualList>
            </td>
        </tr>
        <tr runat="server" id="CurrencyTableRow">
            <td class="FormLabelCell">
                <asp:Label ID="Label2" runat="server" Text="<%$ Resources:SharedStrings, Default_Currency %>"></asp:Label>:</td>
            <td class="FormFieldCell">
                <asp:DropDownList runat="server" ID="ddlCurrencyList">
                    <asp:ListItem Value="" Text="<%$ Resources:SharedStrings, Empty_Parenthesis %>"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>   
    </table>
</div>

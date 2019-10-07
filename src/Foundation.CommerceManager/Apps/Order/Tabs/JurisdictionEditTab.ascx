<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="JurisdictionEditTab.ascx.cs" Inherits="Mediachase.Commerce.Manager.Order.Tabs.JurisdictionEditTab" %>
<div id="DataForm">
    <table class="DataForm">
        <tr>
            <td class="FormLabelCell">
                <asp:Label ID="Label5" runat="server" Text="<%$ Resources:SharedStrings, Display_Name %>"></asp:Label>:
            </td>
            <td class="FormFieldCell">
                <asp:TextBox runat="server" ID="DisplayName"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator8" ControlToValidate="DisplayName" Display="Dynamic" ErrorMessage="*" />
            </td>
        </tr>
        <tr>
            <td class="FormLabelCell">
                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:SharedStrings, Code %>"></asp:Label>:
            </td>
            <td class="FormFieldCell">
                <asp:TextBox runat="server" ID="Code"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator0" ControlToValidate="Code" Display="Dynamic" ErrorMessage="*" />
                <asp:CustomValidator runat="server" ID="CodeCheckCustomValidator" ControlToValidate="Code" OnServerValidate="CodeCheck" Display="Dynamic" ErrorMessage="<%$ Resources:OrderStrings, Jurisdiction_With_Code_Exists %>" />
            </td>
        </tr>
        <tr>
            <td class="FormLabelCell">
                <asp:Label ID="Label2" runat="server" Text="<%$ Resources:SharedStrings, Country_Code %>"></asp:Label>:
            </td>
            <td class="FormFieldCell">
                <asp:TextBox runat="server" ID="CountryCode" AutoPostBack="true"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="CountryCode" Display="Dynamic" ErrorMessage="*" />
            </td>
        </tr>
        <tr>
            <td class="FormLabelCell">
                <asp:Label ID="Label3" runat="server" Text="<%$ Resources:SharedStrings, State_Province_Code %>"></asp:Label>:
            </td>
            <td class="FormFieldCell">
                <asp:TextBox runat="server" ID="StateProvinceCode"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="FormLabelCell">
                <asp:Label ID="Label6" runat="server" Text="<%$ Resources:SharedStrings, Zip_Code %>"></asp:Label>:
            </td>
            <td class="FormFieldCell">
                <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:SharedStrings, From_Lowercase %>"/>&nbsp;
                <asp:TextBox runat="server" ID="ZipCodeStart"></asp:TextBox>
                <asp:RegularExpressionValidator runat="server" ID="ZipCodeStartRegexValidator" ControlToValidate="ZipCodeStart"
                    Display="Dynamic" ValidationExpression=".*">Invalid ZIP code format</asp:RegularExpressionValidator>
                &nbsp;&nbsp;&nbsp;
                <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:SharedStrings, To_Lowercase %>"/>&nbsp;
                <asp:TextBox runat="server" ID="ZipCodeEnd"></asp:TextBox>
                <asp:RegularExpressionValidator runat="server" ID="ZipCodeEndRegexValidator" ControlToValidate="ZipCodeEnd"
                    Display="Dynamic" ValidationExpression=".*">Invalid ZIP code format</asp:RegularExpressionValidator>

            </td>
        </tr>
        <tr>
            <td class="FormLabelCell">
                <asp:Label ID="Label7" runat="server" Text="<%$ Resources:SharedStrings, City %>"></asp:Label>:
            </td>
            <td class="FormFieldCell">
                <asp:TextBox runat="server" ID="City"></asp:TextBox>
            </td>
        </tr>                                                                                        
        <tr>
            <td class="FormLabelCell">
                <asp:Label ID="Label8" runat="server" Text="<%$ Resources:SharedStrings, District %>"></asp:Label>:
            </td>
            <td class="FormFieldCell">
                <asp:TextBox runat="server" ID="District"></asp:TextBox>
            </td>
        </tr>    
        <tr>
            <td class="FormLabelCell">
                <asp:Label ID="Label9" runat="server" Text="<%$ Resources:SharedStrings, County %>"></asp:Label>:
            </td>
            <td class="FormFieldCell">
                <asp:TextBox runat="server" ID="County"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="FormLabelCell">
                <asp:Label ID="Label10" runat="server" Text="<%$ Resources:SharedStrings, Geo_Code %>"></asp:Label>:
            </td>
            <td class="FormFieldCell">
                <asp:TextBox runat="server" ID="GeoCode"></asp:TextBox>
            </td>
        </tr>
    </table>
</div>
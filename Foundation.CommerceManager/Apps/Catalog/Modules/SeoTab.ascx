<%@ Control Language="C#" AutoEventWireup="true" Inherits="Mediachase.Commerce.Manager.Catalog.Modules.SeoTab" CodeBehind="SeoTab.ascx.cs" %>
<tr>
    <td class="FormLabelCell">
        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:SharedStrings, Title %>"></asp:Label>&nbsp;(<%#LanguageCode %>):
    </td>
    <td class="FormFieldCell">
        <asp:TextBox runat="server" Width="250" ID="Title" MaxLength="150"></asp:TextBox><br />
    </td>
    <td>
		<asp:RegularExpressionValidator id="RegularExpressionValidator3" runat="server"  ControlToValidate="Title" 
		                                ErrorMessage="* Title cannot be longer than 150 characters." ValidationExpression=".{0,150}"></asp:RegularExpressionValidator>
	</td>

</tr>
<tr>
    <td colspan="2" class="FormSpacerCell">
    </td>
</tr>
<tr>
    <td class="FormLabelCell">
        <asp:Label ID="Label10" runat="server" Text="<%$ Resources:SharedStrings, Url %>"></asp:Label>&nbsp;(<%#LanguageCode %>):
    </td>
    <td class="FormFieldCell">
        <asp:TextBox runat="server" Width="350" ID="UrlText" MaxLength="255"></asp:TextBox><br />
        <asp:CustomValidator runat="server" id="UrlValidator"
                             ControlToValidate="UrlText" OnServerValidate="SeoUrlValidatorCheck" Display="Dynamic" />
		<asp:RegularExpressionValidator id="RegularExpressionValidator2" runat="server"  ControlToValidate="UrlText" Display="Dynamic"
		                                ErrorMessage="* Url cannot be longer than 255 characters." ValidationExpression=".{0,255}"></asp:RegularExpressionValidator>
        <asp:CustomValidator runat="server" id="duplicateUrlCheck"
            ControlToValidate="UrlText"
            OnServerValidate="SeoUrlCheck" Display="Dynamic"
            ErrorMessage="SEO Url is already in use." />
    </td>
    <td>
	</td>

</tr>
<tr>
    <td colspan="2" class="FormSpacerCell">
    </td>
</tr>
<tr>
    <td class="FormLabelCell">
        <asp:Label ID="Label9" runat="server" Text="<%$ Resources:SharedStrings, Description %>"></asp:Label>&nbsp;(<%#LanguageCode %>):
    </td>
    <td class="FormFieldCell">
        <asp:TextBox runat="server" Width="350" TextMode="MultiLine" ID="Description" MaxLength="355" ></asp:TextBox><br />
    </td>
    
    <td>
		<asp:RegularExpressionValidator id="description_validation" runat="server"  ControlToValidate="Description" 
		                                ErrorMessage="* Description cannot be longer than 355 characters." ValidationExpression=".{0,355}"></asp:RegularExpressionValidator>
	</td>

</tr>
<tr>
    <td colspan="2" class="FormSpacerCell">
    </td>
</tr>
<tr>
    <td class="FormLabelCell">
        <asp:Label ID="Label11" runat="server" Text="<%$ Resources:SharedStrings, Keywords %>"></asp:Label>&nbsp;(<%#LanguageCode %>):
    </td>
    <td class="FormFieldCell">
        <asp:TextBox runat="server" Width="350" TextMode="MultiLine" ID="Keywords" MaxLength="355"></asp:TextBox><br />
    </td>
    <td>
		<asp:RegularExpressionValidator id="RegularExpressionValidator1" runat="server"  ControlToValidate="Keywords" 
		                                ErrorMessage="* Keywords cannot be longer than 355 characters." ValidationExpression=".{0,355}"></asp:RegularExpressionValidator>
	</td>

</tr>
<tr>
    <td colspan="2" class="FormSpacerCell">
    </td>
</tr>
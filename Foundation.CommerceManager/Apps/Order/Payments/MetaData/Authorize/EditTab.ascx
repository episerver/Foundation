<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditTab.ascx.cs" Inherits="Mediachase.Commerce.Manager.Apps.Order.Payments.MetaData.Authorize.EditTab" %>
<tr>
    <td>
    </td>
    <td>
        <asp:Image runat="server" ID="Image1" AlternateText="Visa" ImageUrl="images/visa.gif">
        </asp:Image>
        <asp:Image runat="server" ID="Image2" AlternateText="Discover" ImageUrl="images/discover.gif">
        </asp:Image>
        <asp:Image runat="server" ID="Image3" AlternateText="Mastercard" ImageUrl="images/mastercard.gif">
        </asp:Image>
        <asp:Image runat="server" ID="Image4" AlternateText="American Express" ImageUrl="images/amex.gif">
        </asp:Image>
    </td>
</tr>
<tr>
    <td class="FormLabelCell">
        <asp:Label ID="Label7" runat="server" Text="<%$ Resources:OrderStrings, Cardholders_Name %>"></asp:Label>:
    </td>
    <td class="FormFieldCell">
        <asp:TextBox MaxLength="255" Width="350" ID="creditCardName" runat="server"></asp:TextBox><br />
        <asp:Label CssClass="FormFieldDescription" ID="Label8" runat="server" Text="<%$ Resources:OrderStrings, AsItAppearsOnTheCard %>"></asp:Label>
        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator5" ControlToValidate="creditCardName"
            ErrorMessage="*" EnableClientScript="False" Display="Dynamic"></asp:RequiredFieldValidator>
    </td>
</tr>
<tr>
    <td class="FormLabelCell">
        <asp:Label ID="Label9" runat="server" Text="<%$ Resources:OrderStrings, Credit_Card_Number %>"></asp:Label>:
    </td>
    <td class="FormFieldCell">
        <asp:TextBox MaxLength="16" Width="350" ID="creditCardNumber" runat="server"></asp:TextBox><br />
        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator6" ControlToValidate="creditCardNumber"
            ErrorMessage="*" EnableClientScript="False" Display="Dynamic"></asp:RequiredFieldValidator>
        <cms:CreditCardValidator ControlToValidate="creditCardNumber" runat="server" ID="CCValidator"
            ErrorMessage="Invalid Credit Card Number" EnableClientScript="False" Display="Dynamic"></cms:CreditCardValidator>
    </td>
</tr>
<tr>
    <td class="FormLabelCell">
        <asp:Label ID="Label10" runat="server" Text="<%$ Resources:OrderStrings, Credit_Card_Expiration_Date %>"></asp:Label>:
    </td>
    <td class="FormFieldCell">
        <asp:DropDownList ID="creditCardExpireMonth" runat="server">
            <asp:ListItem Value="1">1</asp:ListItem>
            <asp:ListItem Value="2">2</asp:ListItem>
            <asp:ListItem Value="3">3</asp:ListItem>
            <asp:ListItem Value="4">4</asp:ListItem>
            <asp:ListItem Value="5">5</asp:ListItem>
            <asp:ListItem Value="6">6</asp:ListItem>
            <asp:ListItem Value="7">7</asp:ListItem>
            <asp:ListItem Value="8">8</asp:ListItem>
            <asp:ListItem Value="9">9</asp:ListItem>
            <asp:ListItem Value="10">10</asp:ListItem>
            <asp:ListItem Value="11">11</asp:ListItem>
            <asp:ListItem Value="12">12</asp:ListItem>
        </asp:DropDownList>
        /
        <asp:DropDownList ID="creditCardExpireYear" runat="server">
            <asp:ListItem Value="2011">2011</asp:ListItem>
            <asp:ListItem Value="2012">2012</asp:ListItem>
            <asp:ListItem Value="2013">2013</asp:ListItem>
            <asp:ListItem Value="2014">2014</asp:ListItem>
            <asp:ListItem Value="2015">2015</asp:ListItem>
            <asp:ListItem Value="2016">2016</asp:ListItem>
            <asp:ListItem Value="2017">2017</asp:ListItem>
            <asp:ListItem Value="2018">2018</asp:ListItem>
            <asp:ListItem Value="2019">2019</asp:ListItem>
            <asp:ListItem Value="2020">2020</asp:ListItem>
            <asp:ListItem Value="2021">2021</asp:ListItem>
        </asp:DropDownList>
    </td>
</tr>
<tr>
    <td class="FormLabelCell">
        <asp:Label ID="Label11" runat="server" Text="<%$ Resources:OrderStrings, Credit_Card_Code %>"></asp:Label>:
    </td>
    <td class="FormFieldCell">
        <asp:TextBox ID="creditCardCSC" CssClass="required" runat="server" MaxLength="4"></asp:TextBox>
        <asp:RequiredFieldValidator runat="server" ID="CIDValidator" ControlToValidate="creditCardCSC"
            ErrorMessage="*" EnableClientScript="False" Display="Dynamic"></asp:RequiredFieldValidator>
    </td>
</tr>

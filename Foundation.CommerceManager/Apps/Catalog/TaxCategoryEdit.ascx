<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TaxCategoryEdit.ascx.cs" Inherits="Mediachase.Commerce.Manager.Catalog.TaxCategoryEdit" %>
<asp:Panel ID="Panel1" runat="server" DefaultButton="btnOK">
    <table width="100%" cellspacing="10" border="0">
        <tr>
            <td>
                <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:CatalogStrings, Tax_Category_Name %>" />:
            </td>
            <td style="width: 300px;">
                <asp:TextBox runat="server" ID="TaxCategoryName" ValidationGroup="vgTaxCategory" MaxLength="50" Width="290px"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ID="TaxCategoryRequired" ControlToValidate="TaxCategoryName" ErrorMessage="*" Display="Dynamic" ValidationGroup="vgTaxCategory"></asp:RequiredFieldValidator>
                <asp:CustomValidator runat="server" ID="TaxCategoryNameCheckCustomValidator" ControlToValidate="TaxCategoryName" OnServerValidate="TaxCategoryNameCheck" Display="Dynamic" ValidationGroup="vgTaxCategory" ErrorMessage="<%$ Resources:CatalogStrings, Tax_Category_With_Name_Exists %>" />
            </td>
        </tr>
        <tr>
            <td align="right" style="padding-right:10px;" colspan="2">
                <asp:Button runat="server" ID="btnOK" Text="<%$ Resources:SharedStrings, OK %>" Width="80px" OnClick="btnOK_Click" ValidationGroup="vgTaxCategory" />
                &nbsp;&nbsp;&nbsp;
                <asp:Button runat="server" ID="btnClose" Text="<%$ Resources:SharedStrings, Close %>" Width="80px" CausesValidation="false" />
            </td>
        </tr>
    </table>
</asp:Panel>
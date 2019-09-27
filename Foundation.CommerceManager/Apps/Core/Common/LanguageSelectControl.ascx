<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LanguageSelectControl.ascx.cs" Inherits="Mediachase.Commerce.Manager.Core.Common.LanguageSelectControl" %>
<asp:Panel ID="Panel1" runat="server" DefaultButton="btnOK">
    <table width="100%" cellspacing="10" border="0">
        <tr>
            <td>
                <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:SharedStrings, Language %>"/>:
            </td>
            <td>
                <asp:DropDownList runat="server" ID="LanguagesList" AutoPostBack="false" ValidationGroup="vgLanguage">
                    <asp:ListItem Value="" Text="<%$ Resources:SharedStrings, Select_Language %>"></asp:ListItem>
                </asp:DropDownList>
                <asp:RequiredFieldValidator runat="server" ID="LanguageRequired" ControlToValidate="LanguagesList" ErrorMessage="*" Display="Dynamic" ValidationGroup="vgLanguage"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td align="right" style="padding-right:10px;" colspan="2">
                <asp:Button runat="server" ID="btnOK" Text="<%$ Resources:SharedStrings, OK %>" Width="80px" OnClick="btnOK_Click" ValidationGroup="vgLanguage" />
                &nbsp;&nbsp;&nbsp;
                <asp:Button runat="server" ID="btnClose" Text="<%$ Resources:SharedStrings, Close %>" Width="80px" CausesValidation="false" />
            </td>
        </tr>
    </table>
</asp:Panel>
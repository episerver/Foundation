<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Integer.Edit.CreditCard.ExpirationYear.ascx.cs"
    Inherits="Mediachase.Commerce.Manager.Apps.Customer.Primitives.Integer_Edit_CreditCard_ExpirationYear" %>
<%@ Import Namespace="Mediachase.Ibn.Web.UI" %>
<table cellpadding="0" cellspacing="0" border="0" width="100%" class="ibn-propertysheet">
    <tr>
        <td>
            <table cellpadding="0" cellspacing="0" width="100%" style="table-layout: fixed;">
                <tr>
                    <td style="padding-top: 1px;" width="100%">
                        <asp:DropDownList ID="ddlValue" runat="server" Width="99%">
                        </asp:DropDownList>
                    </td>
                    <td width="20px">
                        <asp:RequiredFieldValidator runat="server" id="ExpireYearRequired" ControlToValidate="ddlValue"
                            Display="Static" ErrorMessage="*"></asp:RequiredFieldValidator>
                    </td>
                </tr>
            </table>
        </td>
        <td width="20px">
        </td>
    </tr>
</table>
<asp:Button ID="btnRefresh" runat="server" CausesValidation="False" Style="display: none;"
    OnClick="btnRefresh_Click"></asp:Button>

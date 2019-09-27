<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Text.Edit.Contact.PreferredCurrency.ascx.cs" Inherits="Mediachase.Commerce.Manager.Apps.Customer.Primitives.Text_Edit_Contact_PreferredCurrency" %>
<%@ Import Namespace="Mediachase.Ibn.Web.UI" %>
<table cellpadding="0" cellspacing="0" border="0" width="100%" class="ibn-propertysheet">
	<tr>
		<td>
			<table cellpadding="0" cellspacing="0" width="100%" style="table-layout: fixed;">
				<tr>
					<td style="padding-top:1px;" width="100%">
						<asp:DropDownList ID="ddlValue" runat="server" Width="99%"></asp:DropDownList>
					</td>
				</tr>
			</table>
		</td>
		<td width="20px"></td>
	</tr>
</table>
<asp:Button id="btnRefresh" runat="server" CausesValidation="False" style="display:none;" OnClick="btnRefresh_Click"></asp:Button>


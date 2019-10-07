<%@ Control Language="C#" AutoEventWireup="true" Inherits="Mediachase.Ibn.Web.UI.MetaUI.Primitives.LongText_Edit" Codebehind="LongText.Edit.ascx.cs" %>
<table cellpadding="0" cellspacing="0" border="0" width="100%" class="ibn-propertysheet">
	<tr>
		<td>
			<asp:TextBox id="txtValue" runat="server" CssClass="text" Wrap="True" Width="99%" TextMode="MultiLine" Height="20px"></asp:TextBox>
		</td>
		<td style="width:20px;" valign="top">
			<asp:RequiredFieldValidator id="vldValue_Required" runat="server" ErrorMessage="*" ControlToValidate="txtValue"	Display="Dynamic"></asp:RequiredFieldValidator>
		</td>
	</tr>
</table>

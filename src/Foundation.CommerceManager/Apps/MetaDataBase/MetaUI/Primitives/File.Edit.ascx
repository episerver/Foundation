<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="File.Edit.ascx.cs" Inherits="Mediachase.Ibn.Web.UI.MetaUI.Primitives.File_Edit" %>
<%@ Register TagPrefix="mc" Namespace="Mediachase.FileUploader.Web.UI" Assembly="Mediachase.FileUploader" %>
<table border="0" cellpadding="0" cellspacing="0" class="ibn-propertysheet">
	<tr>
		<td valign="top" class="text">
			<mc:mchtmlinputfile id="fAssetFile" CssClass="text" runat="server" Width="320" />
			<asp:RequiredFieldValidator ID="rfFile" ControlToValidate="fAssetFile" Display="Dynamic" ErrorMessage="*" Runat="server" Visible="False"></asp:RequiredFieldValidator>
		</td>
	</tr>
	<tr style="padding-top:10px">
		<td align="left">
			<asp:Label ID="lblLink" Runat="server" CssClass="text" Visible="False" TabIndex="-1"></asp:Label>&nbsp;&nbsp;
			<asp:LinkButton ID="btnDelete" Runat="server" Visible="False" onclick="btnDelete_Click" TabIndex="-1"></asp:LinkButton>
		</td>
	</tr>
</table>
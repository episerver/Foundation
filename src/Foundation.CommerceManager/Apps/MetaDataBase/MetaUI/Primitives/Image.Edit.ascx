<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Image.Edit.ascx.cs" Inherits="Mediachase.Ibn.Web.UI.MetaUI.Primitives.Image_Edit" %>
<%@ Register TagPrefix="mc" Namespace="Mediachase.FileUploader.Web.UI" Assembly = "Mediachase.FileUploader" %>
<table border="0" cellpadding="0" cellspacing="0">
	<tr>
		<td valign="top" class="text">
			<mc:mchtmlinputfile id="fAssetFile" CssClass="text" runat="server" Width="320px" />
			<asp:RequiredFieldValidator ID="rfPhoto" ControlToValidate="fAssetFile" Display="Dynamic" ErrorMessage="*" Runat="server" Visible="False"></asp:RequiredFieldValidator>
		</td>
		<td></td>
	</tr>
	<tr style="padding-top:10px">
		<td align="center" valign="middle">
			<div style="text-align:left;">
				<img alt="" id="imgPhoto" runat="server" style="border-width:0;" src="" />
			</div>
		</td>
		<td valign="top" style="padding-left:10px; width:25px">
			<asp:LinkButton ID="btnDelete" Runat="server" Visible="false" CausesValidation="False" onclick="btnDelete_Click" TabIndex="-1"></asp:LinkButton>
		</td>
	</tr>
</table>

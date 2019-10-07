<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GeneralView.ascx.cs" Inherits="Mediachase.Commerce.Manager.Apps.Order.Modules.GeneralView" %>
<%@ Register TagPrefix="mc2" Namespace="Mediachase.Commerce.Manager.Apps.Common.Design"
	Assembly="Mediachase.ConsoleManager" %>
<table cellpadding="0" cellspacing="0" width="100%">
	<tr>
		<td style="padding: 7px">
			<mc2:BlockHeaderLight HeaderCssClass="ibn-toolbar-light" ID="bhl" runat="server"
				Title="Overview"></mc2:BlockHeaderLight>
			<table class="orderform-blockheaderlight-datatable">
				<tr>
					<td style="vertical-align:top; width:50%;">
						&nbsp;
					</td>
					<td style="vertical-align:top; width:50%;">
						<asp:Button ID="post" runat="server" Text="Post" />
					</td>
				</tr>
			</table>
		</td>
	</tr>
</table>
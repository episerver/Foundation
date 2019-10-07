<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MembershipAccountView.ascx.cs"
	Inherits="Mediachase.Commerce.Manager.Apps.Customer.Modules.MembershipAccountView" %>
<%@ Register TagPrefix="mc2" Namespace="Mediachase.Commerce.Manager.Apps.Common.Design"
	Assembly="Mediachase.ConsoleManager" %>
<style type="text/css">
#accountTable td
{
	padding: 5px;
}
</style>
<table cellpadding="0" cellspacing="0" width="100%">
	<tr>
		<td style="padding: 7px">
			<mc2:BlockHeaderLight HeaderCssClass="ibn-toolbar-light" ID="bhl" runat="server"
				Title="<%$ Resources:Customer, Account %>"></mc2:BlockHeaderLight>
			<table border="0" style="width: 100%;" class="borderType1TableLayoutSection" id="accountTable">
				<tbody>
					<tr runat="server" id="trUsername">
						<td valign="top" style="width: 120px;" class="labelSmartTableLayoutItem">
							<asp:Label runat="server" ID="UserNameLabel" Text="<%$ Resources:SharedStrings, User_Name %>"></asp:Label>:
						</td>
						<td>
							<asp:Literal runat="server" Mode="Encode" ID="lblUserName" Text=""></asp:Literal>
						</td>
						<td valign="top" style="width: 120px;" class="labelSmartTableLayoutItem">
							<asp:Label runat="server" ID="Label11" Text="<%$ Resources:Customer, MembershipUser_IsLockedOut %>"></asp:Label>
						</td>
						<td>
							<asp:Label runat="server" ID="IsLockedOut"></asp:Label>
						</td>
					</tr>
					<tr runat="server" id="trDescription">
						<td valign="top" style="width: 120px;" class="labelSmartTableLayoutItem">
							<asp:Label runat="server" ID="lblDescr" Text="<%$ Resources:SharedStrings, Description %>"></asp:Label>:
						</td>
						<td>
                            <asp:Literal runat="server" Mode="Encode" ID="lblDecr" Text=""></asp:Literal>
						</td>
						<td valign="top" style="width: 120px;" class="labelSmartTableLayoutItem">
							<asp:Label runat="server" ID="Label8" Text="<%$ Resources:Customer, MembershipUser_LastActivity %>"></asp:Label>
						</td>
						<td>
							<asp:Label runat="server" ID="LastActivityDate"></asp:Label>
						</td>
					</tr>
					<tr runat="server" id="trEmail">
						<td valign="top" style="width: 120px;" class="labelSmartTableLayoutItem">
							<asp:Label runat="server" ID="Label3" Text="<%$ Resources:SharedStrings, Email %>"></asp:Label>:
						</td>
						<td>
                            <asp:Literal runat="server" Mode="Encode" ID="lblEmail" Text=""></asp:Literal>
						</td>
						<td valign="top" style="width: 120px;" class="labelSmartTableLayoutItem">
							<asp:Label runat="server" ID="Label12" Text="<%$ Resources:Customer, MembershipUser_LastLogin%>"></asp:Label>
						</td>
						<td>
							<asp:Label runat="server" ID="LastLoginDate"></asp:Label>
						</td>
					</tr>
					<tr runat="server" id="trPasswordChanged">
						<td valign="top" style="width: 120px;" class="labelSmartTableLayoutItem" colspan="2" />
						<td valign="top" style="width: 120px;" class="labelSmartTableLayoutItem">
							<asp:Label runat="server" ID="Label13" Text="<%$ Resources:Customer, MembershipUser_LastPasswordChanged%>"></asp:Label>
						</td>
						<td>
							<asp:Label runat="server" ID="LastPasswordChangedDate"></asp:Label>
						</td>
					
					</tr>
					<tr runat="server" id="trNoAccount">
						<td valign="top" style="width: 120px;" class="labelSmartTableLayoutItem">
							<asp:Label runat="server" ID="Label2" Text="<%$ Resources:Customer, MembershipUser_Account_Not_Found%>"></asp:Label>
						</td>
					</tr>
				</tbody>
			</table>
		</td>
	</tr>
</table>
<asp:Button runat="server" ID="btnDeleteAccount" Visible="false" CausesValidation="false" Text="Cancel" />
<asp:LinkButton runat="server" text="refresh" ID="btnRefresh" Visible="false" OnClick="btnRefresh_OnClick"></asp:LinkButton>
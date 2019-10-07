<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomPageEdit.ascx.cs" Inherits="Mediachase.Commerce.Manager.Apps.Customization.Modules.CustomPageEdit" %>
<%@ Reference VirtualPath="~/Apps/Common/Design/BlockHeader2.ascx" %>
<%@ Register TagPrefix="ibn" TagName="BlockHeader" src="~/Apps/Common/Design/BlockHeader2.ascx" %>
<%@ Register TagPrefix="btn" namespace="Mediachase.BusinessFoundation" Assembly="Mediachase.BusinessFoundation" %>

<table class="ibn-stylebox" cellspacing="0" cellpadding="0" width="100%" border="0" style="margin-top:1px">
	<tr>
		<td colspan="2"><ibn:blockheader id="MainHeader" runat="server"></ibn:blockheader></td>
	</tr>
	<tr>
		<td valign="top">
			<table class="text" cellspacing="7" cellpadding="0" border="0" style="table-layout:fixed;">
				<tr>
					<td class="ibn-label" style="width:150px;">
						<asp:Literal runat="server" ID="TitleLiteral" Text="<%$ Resources: Global, _mc_Title %>"></asp:Literal>:
					</td>
					<td>
						<asp:TextBox runat="server" ID="TitleText" Width="480"></asp:TextBox>
						<asp:RequiredFieldValidator runat="server" ID="TitleTextRFValidator" ControlToValidate="TitleText" Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator>
					</td>
				</tr>
				<tr>
					<td class="ibn-label" valign="top">
						<asp:Literal runat="server" ID="DescriptionLiteral" Text="<%$ Resources: Global, _mc_Description %>"></asp:Literal>:
					</td>
					<td>
						<asp:TextBox runat="server" ID="DescriptionText" Width="480" TextMode="MultiLine" Rows="5"></asp:TextBox>
					</td>
				</tr>
				<tr>
					<td class="ibn-label">
						<asp:Literal runat="server" ID="TemplateLiteral" Text="<%$ Resources: Common, PageTemplate %>"></asp:Literal>:
					</td>
					<td>
						<asp:DropDownList runat="server" ID="TemplateList" Width="480"></asp:DropDownList>
					</td>
				</tr>
				<tr>
					<td>&nbsp;</td>
					<td style="padding-top:10px;">
						<btn:imbutton class="text" id="SaveButton" style="width:110px;" 
							Runat="server" onserverclick="SaveButton_ServerClick" />&nbsp;&nbsp;
						<btn:imbutton class="text" id="CancelButton" style="width:110px;" 
							Runat="server" IsDecline="true" CausesValidation="false" onserverclick="CancelButton_ServerClick"/>
					</td>
				</tr>
			</table>
		</td>
	</tr>
</table>

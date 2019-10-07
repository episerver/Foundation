<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrderNoteEdit.ascx.cs" Inherits="Mediachase.Commerce.Manager.Apps.Order.Modules.OrderNoteEdit" %>
<%@ Register TagPrefix="mc2" Assembly="Mediachase.BusinessFoundation" Namespace="Mediachase.BusinessFoundation" %>
<div style="padding: 5px;">
	<asp:Label runat="server" ID="lblErrorInfo" Style="color: Red"></asp:Label>
	<table width="100%" class="DataForm">
	    <asp:UpdatePanel UpdateMode="Conditional" ID="MainContentPanel" runat="server" RenderMode="Block">
		    <ContentTemplate>
		        <tr>
			        <td class="FormLabelCell">
				        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:SharedStrings, Title %>"></asp:Label>:
			        </td>
			        <td class="FormFieldCell">
				        <asp:TextBox runat="server" ID="tbNoteTitle" Width="300" MaxLength="255"></asp:TextBox>
				        <asp:RequiredFieldValidator runat="server" ID="NoteTitleRequired" ControlToValidate="tbNoteTitle" Display="Dynamic" ErrorMessage="*" />
			        </td>
		        </tr>
		        <tr>
					<td class="FormLabelCell">
						<asp:Label ID="Label2" runat="server" Text="<%$ Resources:SharedStrings, Text %>"></asp:Label>:
					</td>
					<td class="FormFieldCell">
						<asp:TextBox runat="server" ID="tbNoteText" Width="300" TextMode="MultiLine"></asp:TextBox>
					</td>
				</tr>
		        <tr runat="server" id="CreatorRow">
			        <td class="FormLabelCell">
				        <asp:Label ID="Label7" runat="server" Text="<%$ Resources:SharedStrings, Created_By %>"></asp:Label>:
			        </td>
			        <td class="FormFieldCell">
				        <asp:Label ID="CreatedByLabel" runat="server"></asp:Label>
			        </td>
		        </tr>
		        <tr runat="server" id="CreateDateRow">
			        <td class="FormLabelCell">
				        <asp:Label ID="Label8" runat="server" Text="<%$ Resources:SharedStrings, Created_On %>"></asp:Label>:
			        </td>
			        <td class="FormFieldCell">
				        <asp:Label ID="CreateDateLabel" runat="server"></asp:Label>
			        </td>
		        </tr>
		    </ContentTemplate>
	    </asp:UpdatePanel>
	</table>
</div>
<%@ Control Language="c#" Inherits="Mediachase.Commerce.Manager.Core.MetaData.MetaControls.DicSingleValueControl" CodeBehind="DicSingleValueControl.ascx.cs" AutoEventWireup="True" %>
<tr>
	<td class="FormLabelCell"><strong><asp:Label id="MetaLabelCtrl" runat="server" Text="<%$ Resources:SharedStrings, Label %>"></asp:Label>:</strong></td>
	<td class="FormFieldCell">
		<asp:DropDownList id="DicSingleValueCtrl" runat="server" Width="250px"></asp:DropDownList>
		<asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ErrorMessage="*" Display="Dynamic" ControlToValidate="DicSingleValueCtrl"></asp:RequiredFieldValidator>		
		<br /><asp:Label CssClass="FormFieldDescription" id="MetaDescriptionCtrl" runat="server" Text="<%$ Resources:SharedStrings, Label %>"></asp:Label>
	</td>
</tr>

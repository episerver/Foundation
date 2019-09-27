<%@ Control Language="c#" Inherits="Mediachase.Commerce.Manager.Core.MetaData.MetaControls.MoneyControl" Codebehind="MoneyControl.ascx.cs" %>
<tr>
	<td class="FormLabelCell"><asp:Label id="MetaLabelCtrl" runat="server" Text="<%$ Resources:SharedStrings, Label %>"></asp:Label>:</td>
	<td class="FormFieldCell">
		<asp:TextBox id="MetaValueCtrl" runat="server"></asp:TextBox>
		<asp:RangeValidator Runat="server" ErrorMessage="*" ControlToValidate="MetaValueCtrl" Type="Double"
			MinimumValue="0" MaximumValue="922337203685477.5807" id="RangeValidator1"></asp:RangeValidator>
		<asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ErrorMessage="*" Display="Dynamic" ControlToValidate="MetaValueCtrl"></asp:RequiredFieldValidator>
		<asp:CompareValidator ID="CompareValidator1" runat="Server" ErrorMessage="*" Display="Dynamic" ControlToValidate="MetaValueCtrl" Operator="DataTypeCheck" Type="Double"></asp:CompareValidator>
		<br/><asp:Label CssClass="FormFieldDescription" id="MetaDescriptionCtrl" runat="server" Text="<%$ Resources:SharedStrings, Label %>"></asp:Label>
	</td>
</tr>

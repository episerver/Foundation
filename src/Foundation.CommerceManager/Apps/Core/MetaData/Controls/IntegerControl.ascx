<%@ Control Language="c#" Inherits="Mediachase.Commerce.Manager.Core.MetaData.MetaControls.IntegerControl" Codebehind="IntegerControl.ascx.cs" %>
<tr>
	<td class="FormLabelCell"><asp:Label id="MetaLabelCtrl" runat="server" Text="<%$ Resources:SharedStrings, Label %>"></asp:Label>:</td>
	<td class="FormFieldCell">
		<asp:TextBox id="MetaValueCtrl" runat="server"></asp:TextBox>
		<asp:RangeValidator Runat="server" ErrorMessage="*" ControlToValidate="MetaValueCtrl" Type="Integer"
			MinimumValue="-2147483648" MaximumValue="2147483647" id="RangeValidator1"></asp:RangeValidator>
		<asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ErrorMessage="*" Display="Dynamic" ControlToValidate="MetaValueCtrl"></asp:RequiredFieldValidator>
		<br><asp:Label CssClass="FormFieldDescription" id="MetaDescriptionCtrl" runat="server" Text="<%$ Resources:SharedStrings, Label %>"></asp:Label>
	</td>
</tr>

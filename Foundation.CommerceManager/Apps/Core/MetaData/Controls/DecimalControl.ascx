<%@ Control Language="c#" Inherits="Mediachase.Commerce.Manager.Core.MetaData.MetaControls.DecimalControl" Codebehind="DecimalControl.ascx.cs" %>
<tr>
	<td class="FormLabelCell"><asp:Label id="MetaLabelCtrl" runat="server" Text="<%$ Resources:SharedStrings, Label %>"></asp:Label>:</td>
	<td class="FormFieldCell">
		<asp:TextBox id="MetaValueCtrl" runat="server"></asp:TextBox>
		<asp:RangeValidator MinimumValue="-100000000000" MaximumValue="100000000000" Type="Double" ID="MetaValueRgValidator"
			Runat="server" ErrorMessage="*" ControlToValidate="MetaValueCtrl" Display="Dynamic"></asp:RangeValidator>
		<asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ErrorMessage="*" Display="Dynamic" ControlToValidate="MetaValueCtrl"></asp:RequiredFieldValidator>
		<br><asp:Label CssClass="FormFieldDescription" id="MetaDescriptionCtrl" runat="server" Text="<%$ Resources:SharedStrings, Label %>"></asp:Label>
	</td>
</tr>

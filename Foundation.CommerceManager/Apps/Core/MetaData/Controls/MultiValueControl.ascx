<%@ Control Language="c#" Inherits="Mediachase.Commerce.Manager.Core.MetaData.MetaControls.MultiValueControl" Codebehind="MultiValueControl.ascx.cs" %>
<tr>
	<td class="FormLabelCell"><asp:Label id="MetaLabelCtrl" runat="server" Text="<%$ Resources:SharedStrings, Label %>"></asp:Label>:</td>
	<td class="FormFieldCell">
		<asp:TextBox id="MetaValueCtrl" runat="server"></asp:TextBox>
		<asp:Button Text="<%$ Resources:SharedStrings, Add_Value %>" Runat="server" ID="AddValue" onclick="AddValue_Click"></asp:Button>
		<br><asp:ListBox DataTextField=value DataValueField=id Runat="server" ID="MetaMultiValueCtrl" Width="249px"></asp:ListBox>
		<br><asp:Label CssClass="FormFieldDescription" id="MetaDescriptionCtrl" runat="server" Text="<%$ Resources:SharedStrings, Label %>"></asp:Label>
	</td>
</tr>
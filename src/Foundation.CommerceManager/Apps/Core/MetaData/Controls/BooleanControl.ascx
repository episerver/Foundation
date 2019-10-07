<%@ Control Language="c#" Inherits="Mediachase.Commerce.Manager.Core.MetaData.MetaControls.BooleanControl" Codebehind="BooleanControl.ascx.cs" %>
<tr>
	<td class="FormLabelCell"><asp:Label id="MetaLabelCtrl" runat="server" Text="<%$ Resources:SharedStrings, Label %>"></asp:Label>:</td>
	<td class="FormFieldCell">
    <asp:RadioButtonList id="MetaValueCtrl" RepeatDirection="Horizontal" runat="Server">
        <asp:ListItem Value="true" Text="<%$ Resources:SharedStrings, True %>" />
        <asp:ListItem Value="false" Selected="true" Text="<%$ Resources:SharedStrings, False %>" />
    </asp:RadioButtonList>
		<asp:Label CssClass="FormFieldDescription" id="MetaDescriptionCtrl" runat="server" Text="<%$ Resources:SharedStrings, Label %>"></asp:Label>
	</td>
</tr>

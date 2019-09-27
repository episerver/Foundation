<%@ Control Language="c#" Inherits="Mediachase.Commerce.Manager.Core.MetaData.MetaControls.LongHtmlStringControl" Codebehind="LongHtmlStringControl.ascx.cs" %>
<%@ Register Src="~/Apps/Core/Controls/HtmlEditControl.ascx" TagName="HtmlEditorControl" TagPrefix="core" %>
<tr>
	<td class="FormLabelCell"><asp:Label id="MetaLabelCtrl" runat="server" Text="<%$ Resources:SharedStrings, Label %>"></asp:Label>:</td>
	<td class="FormFieldCell">
        <core:HtmlEditorControl Width="700px" height="400px" ID="MetaValueCtrl" runat="server" /><br />
		<asp:Label id="MetaDescriptionCtrl" runat="server" Text="<%$ Resources:SharedStrings, Label %>"></asp:Label>
	</td>
</tr>

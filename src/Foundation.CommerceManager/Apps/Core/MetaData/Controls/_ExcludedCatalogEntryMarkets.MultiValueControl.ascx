<%@ Control Language="c#" Inherits="Mediachase.Commerce.Manager.Core.Catalog.ExcludedCatalogEntryMarketsFieldControl" Codebehind="_ExcludedCatalogEntryMarkets.MultiValueControl.ascx.cs" %>
<tr>
	<td class="FormLabelCell"><asp:Label id="MetaLabelCtrl" runat="server" Text="<%$ Resources:SharedStrings, Label %>"></asp:Label>:</td>
	<td class="FormFieldCell">
		<br><asp:ListBox Runat="server" ID="lbMarkets" Width="249px"></asp:ListBox>
		<br><asp:Label CssClass="FormFieldDescription" id="MetaDescriptionCtrl" runat="server" Text="<%$ Resources:SharedStrings, Label %>"></asp:Label>
	</td>
</tr>
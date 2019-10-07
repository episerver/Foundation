<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DynamicPackage.ascx.cs" Inherits="Mediachase.Commerce.Manager.Apps.Order.EntryTypes.DynamicPackage" %>
<%@ Register Src="~/Apps/Core/MetaData/EditTab.ascx" TagName="MetaData" TagPrefix="ecf" %>

<table cellpadding="0" cellspacing="10" width="100%">
	<tr runat="server" id="PackageItemsRow" visible="false">
		<td style="border: solid 1px #999999;">
		    <asp:Literal runat="server" Text="<%$ Resources:CatalogStrings, Entry_Dynamic_Package_Items %>"></asp:Literal>
			<asp:DataGrid runat="server" ID="PackageItemsGrid" Width="100%" AutoGenerateColumns="false" AllowPaging="false"
			 AllowSorting="false" CellSpacing="0" CellPadding="5" CssClass="Grid" GridLines="None">
				<HeaderStyle BackColor="#eeeeee" />
				<Columns>
					<asp:BoundColumn DataField="CatalogEntryId" Visible="False"></asp:BoundColumn>
					<asp:BoundColumn DataField="ID" HeaderText="<%$ Resources:CatalogStrings, Entry_Id %>" visible="True" ItemStyle-CssClass="ibn-vb2" HeaderStyle-CssClass="ibn-vh2"></asp:BoundColumn>
					<asp:BoundColumn DataField="DisplayName" HeaderText="<%$ Resources:CatalogStrings, Entry_Name %>" ItemStyle-CssClass="ibn-vb2" HeaderStyle-CssClass="ibn-vh2"></asp:BoundColumn>
					<asp:BoundColumn DataField="Quantity" HeaderText="<%$ Resources:CatalogStrings, Entry_Quantity %>" HeaderStyle-Width="60" ItemStyle-CssClass="ibn-vb2" HeaderStyle-CssClass="ibn-vh2" DataFormatString="{0:F}"></asp:BoundColumn>
				</Columns>
			</asp:DataGrid>
		</td>
	</tr>
</table>
<span style="font-size:1.1em;margin:10px;"><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:OrderStrings, Entry_Dynamic_Package_Cannot_Configure %>"></asp:Literal></span>
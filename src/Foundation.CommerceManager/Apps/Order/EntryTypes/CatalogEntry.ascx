<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CatalogEntry.ascx.cs" Inherits="Mediachase.Commerce.Manager.Apps.Order.EntryTypes.CatalogEntry" %>
<%@ Register Src="~/Apps/Core/MetaData/EditTab.ascx" TagName="MetaData" TagPrefix="ecf" %>
<script type="text/javascript">
	
	function clickclear(thisfield, defaulttext) {
	if (thisfield.value == defaulttext) {
		thisfield.value = "";
	}
	}
</script>

<table cellpadding="0" cellspacing="10" width="100%">
	<tr runat="server" id="VariationRow">
		<td style="width: 100px; text-align: right;" class="FormLabelCell">
			<asp:Literal runat="server" Text="<%$ Resources:OrderStrings, PickVariation %>"></asp:Literal>:
		</td>
		<td>
			<asp:DropDownList runat="server" ID="VariationList" AutoPostBack="true" OnSelectedIndexChanged="VariationList_SelectedIndexChanged">
			</asp:DropDownList>
		</td>
	</tr>
	<tr runat="server" id="WarehouseRow">
		<td style="width: 100px; text-align: right;" class="FormLabelCell">
			<asp:Literal runat="server" Text="<%$ Resources:OrderStrings, Warehouse_PickFulfillment %>"></asp:Literal>:
		</td>
		<td>
			<asp:DropDownList runat="server" ID="WarehouseList" AutoPostBack="true" OnSelectedIndexChanged="WarehouseList_SelectedIndexChanged">
			</asp:DropDownList>
			<asp:CustomValidator runat="server" ID="WarehouseValidator" ControlToValidate="WarehouseList"
								ErrorMessage="<%$ Resources:OrderStrings, Warehouse_PickFulfillment_Already_Exist %>" OnServerValidate="WarehouseList_Validate" CssClass="ErrorRed"></asp:CustomValidator>
		</td>
	</tr>
    <tr runat="server" id="PackageItemsRow">
		<td colspan="2" style="border: solid 1px #999999;">
		    <asp:Literal runat="server" Text="<%$ Resources:CatalogStrings, Entry_Package_Items %>"></asp:Literal>
			<asp:DataGrid runat="server" ID="PackageItemsGrid" Width="100%" AutoGenerateColumns="false" AllowPaging="false"
			 AllowSorting="false" CellSpacing="0" CellPadding="5" CssClass="Grid" GridLines="None">
				<HeaderStyle BackColor="#eeeeee" />
				<Columns>
					<asp:BoundColumn DataField="CatalogEntryId" Visible="False"></asp:BoundColumn>
					<asp:BoundColumn DataField="ID" HeaderText="<%$ Resources:CatalogStrings, Entry_Id %>" visible="True" ItemStyle-CssClass="ibn-vb2" HeaderStyle-CssClass="ibn-vh2"></asp:BoundColumn>
					<asp:BoundColumn DataField="DisplayName" HeaderText="<%$ Resources:CatalogStrings, Entry_Name %>" ItemStyle-CssClass="ibn-vb2" HeaderStyle-CssClass="ibn-vh2"></asp:BoundColumn>
					<asp:BoundColumn DataField="Quantity" HeaderText="<%$ Resources:CatalogStrings, Entry_Quantity %>" HeaderStyle-Width="60" ItemStyle-CssClass="ibn-vb2" HeaderStyle-CssClass="ibn-vh2"></asp:BoundColumn>
				</Columns>
			</asp:DataGrid>
		</td>
	</tr>
    <tr runat="server" id="BundleItemsRow">
		<td colspan="2" style="border: solid 1px #999999;">
		    <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:CatalogStrings, Entry_Bundle_Items %>"></asp:Literal>
			<asp:DataGrid runat="server" ID="BundleItemsGrid" Width="100%" AutoGenerateColumns="false" AllowPaging="false"
			 AllowSorting="false" CellSpacing="0" CellPadding="5" CssClass="Grid" GridLines="None">
				<HeaderStyle BackColor="#eeeeee" />
				<Columns>
					<asp:BoundColumn DataField="CatalogEntryId" Visible="False"></asp:BoundColumn>
					<asp:BoundColumn DataField="ID" HeaderText="<%$ Resources:CatalogStrings, Entry_Id %>" visible="True" ItemStyle-CssClass="ibn-vb2" HeaderStyle-CssClass="ibn-vh2"></asp:BoundColumn>
					<asp:BoundColumn DataField="DisplayName" HeaderText="<%$ Resources:CatalogStrings, Entry_Name %>" ItemStyle-CssClass="ibn-vb2" HeaderStyle-CssClass="ibn-vh2"></asp:BoundColumn>
					<asp:BoundColumn DataField="Quantity" HeaderText="<%$ Resources:CatalogStrings, Entry_Quantity %>" HeaderStyle-Width="60" ItemStyle-CssClass="ibn-vb2" HeaderStyle-CssClass="ibn-vh2"></asp:BoundColumn>
					<asp:BoundColumn DataField="ListPrice" HeaderText="<%$ Resources:OrderStrings, List_Price %>" HeaderStyle-Width="60" ItemStyle-CssClass="ibn-vb2" HeaderStyle-CssClass="ibn-vh2"></asp:BoundColumn>
                    <asp:BoundColumn DataField="InStock" HeaderText="<%$ Resources:CatalogStrings, Entry_In_Stock %>" HeaderStyle-Width="60" ItemStyle-CssClass="ibn-vb2" HeaderStyle-CssClass="ibn-vh2"></asp:BoundColumn>
					<asp:BoundColumn DataField="Reserved" HeaderText="<%$ Resources:SharedStrings, Reserved %>" HeaderStyle-Width="60" ItemStyle-CssClass="ibn-vb2" HeaderStyle-CssClass="ibn-vh2"></asp:BoundColumn>
				</Columns>
			</asp:DataGrid>
		</td>
	</tr>
    <tr runat="server" id="SuggestedPriceRow">
		<td style="width: 100px; text-align: right;" class="FormLabelCell">
			<asp:Literal runat="server" Text="<%$ Resources:OrderStrings, PriceTier %>"></asp:Literal>:
		</td>
		<td>
			<asp:DropDownList runat="server" ID="SuggestedPriceList" AutoPostBack="true" OnSelectedIndexChanged="SuggestedPriceList_SelectedIndexChanged">
			</asp:DropDownList>
		</td>
	</tr>
	<tr runat="server" id="DisplayPriceRow">
		<td style="width: 100px; text-align: right;" class="FormLabelCell">
			<asp:Literal runat="server" ID="DisplayPriceLiteral" Text="<%$ Resources:OrderStrings, DisplayPrice %>"></asp:Literal>:
		</td>
		<td>
			<asp:Label runat="server" ID="DisplayPriceLabel"></asp:Label>
		</td>
	</tr>
	<tr runat="server" id="UnitPriceRow">
		<td style="text-align: right;" class="FormLabelCell">
			<asp:Literal runat="server" ID="PriceLiteral" Text="<%$ Resources:SharedStrings, Price %>"></asp:Literal>:
		</td>
		<td>
			<asp:TextBox runat="server" ID="UnitPrice" AutoPostBack="true" OnTextChanged="UnitPrice_TextChanged"></asp:TextBox>
			<asp:RequiredFieldValidator runat="server" ID="PriceRFValidator" ControlToValidate="UnitPrice"
				Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator>
			<asp:RangeValidator runat="server" ID="PriceRangeValidator" ControlToValidate="UnitPrice"
				ErrorMessage="*" MinimumValue="0" MaximumValue="999999" Type="Currency"></asp:RangeValidator>
            <asp:HiddenField runat="server" ID="UnitPriceStatus" />
		</td>
	</tr>
	<tr runat="server" id="QuantityValueRow">
		<td style="text-align: right;" class="FormLabelCell">
			<asp:Literal runat="server" ID="QuantityLiteral" Text="<%$ Resources:SharedStrings, Quantity %>"></asp:Literal>:
		</td>
		<td>
			<asp:TextBox runat="server" ID="QuantityValue" AutoPostBack="true"></asp:TextBox>
			<asp:RequiredFieldValidator runat="server" ID="QuantityRFValidator" ControlToValidate="QuantityValue"
				Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator>
			<asp:RangeValidator runat="server" ID="QuantityRangeValidator" ControlToValidate="QuantityValue"
				ErrorMessage="*" MinimumValue="0" MaximumValue="999999" Type="Double"></asp:RangeValidator>
		</td>
	</tr>
	<tr runat="server" id="InventoryRow">
		<td>
		</td>
		<td class="FormFieldDescription">
			<asp:Literal runat="server" ID="InStockLiteral" Text="<%$ Resources:CatalogStrings, Entry_In_Stock %>"></asp:Literal>:
			<asp:Label runat="server" ID="InStockLabel"></asp:Label>.
			<asp:Literal runat="server" ID="ReservedLiteral" Text="<%$ Resources:SharedStrings, Reserved %>"></asp:Literal>:
			<asp:Label runat="server" ID="ReservedLabel"></asp:Label>.
		</td>
	</tr>
	<tr runat="server" id="DiscountRow">
		<td style="text-align: right;" class="FormLabelCell">
			<asp:Literal runat="server" ID="DiscountLiteral" Text="<%$ Resources:SharedStrings, Discount %>"></asp:Literal>:
		</td>
		<td>
			<table>
				<tbody>
					<tr>
						<td runat="server" id="DiscountDescrCell">
							<asp:TextBox runat="server" ID="DiscountDescr"></asp:TextBox>
						</td>
						<td>
							<asp:TextBox ID="DiscountAmount" runat="server" Width="50px" AutoPostBack="true"></asp:TextBox>
						</td>
						<td>
							<asp:DropDownList runat="server" ID="DiscountAmountType" AutoPostBack="true">
								<asp:ListItem Text="<%$ Resources:MarketingStrings, Promotion_Percentage_Based %>" Value="1"></asp:ListItem>
								<asp:ListItem Text="<%$ Resources:MarketingStrings, Promotion_Value_Based %>" Value="2"></asp:ListItem>
							</asp:DropDownList>
							<asp:RequiredFieldValidator runat="server" ControlToValidate="DiscountAmount" Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator>                   
							<asp:CustomValidator runat="server" ID="DiscountAmountValidator" ControlToValidate="DiscountAmount"
								ErrorMessage="*" OnServerValidate="DiscountAmountValidator_Validate" Type="Currency"></asp:CustomValidator>
						</td>
					</tr>
					<tr align="center" runat="server" id="DiscountDescrRow">
						<td>
							<asp:Label CssClass="FormFieldDescription" runat="server" Text="<%$ Resources:OrderStrings, New_LineItem_Discount_Description%>"></asp:Label>
						</td>
						<td>
							<asp:Label CssClass="FormFieldDescription" runat="server" Text="<%$ Resources:OrderStrings, New_LineItem_Discount_Amount%>"></asp:Label>
						</td>
						<td>
						</td>
					</tr>
				</tbody>
			</table>
		</td>
	</tr>
	<tr runat="server" id="DiscountTotalRow">
		<td style="text-align: right;" class="FormLabelCell">
			<asp:Literal runat="server" ID="DiscountTotalLiteral" Text="<%$ Resources:OrderStrings, Discounts_Total %>"></asp:Literal>:
		</td>
		<td>
			<asp:Label runat="server" ID="DiscountTotalLabel" Font-Bold="true"></asp:Label>
		</td>
	</tr>
	<tr runat="server" id="TotalRow">
		<td style="text-align: right;" class="FormLabelCell">
			<asp:Literal runat="server" ID="TotalLiteral" Text="<%$ Resources:SharedStrings, Total %>"></asp:Literal>:
		</td>
		<td>
			<asp:Label runat="server" ID="TotalLabel" Font-Bold="true"></asp:Label>
		</td>
	</tr>
</table>
<div>
	<table class="DataForm"> 
		<ecf:MetaData ValidationGroup="OrderCreateVG" runat="server" ID="MetaDataTab" />
	</table>
</div>

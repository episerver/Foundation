<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WarehouseInventoryEditTab.ascx.cs" Inherits="Mediachase.Commerce.Manager.Apps.Catalog.Tabs.WarehouseInventoryEditTab" %>
<%@ Register Src="~/Apps/Core/Controls/CalendarDatePicker.ascx" TagName="CalendarDatePicker" TagPrefix="ecf" %>

<asp:UpdatePanel ID="WarehouseUpdatePanel" UpdateMode="Conditional" RenderMode="Inline" runat="server">
    <ContentTemplate>
        <div id="DataForm">
            <table class="DataForm">
                <tr>
                    <td class="FormLabelCell">
                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:SharedStrings, Warehouse %>"></asp:Label>:</td>
                    <td class="FormFieldCell">
                        <asp:DropDownList runat="server" ID="WarehouseList" DataTextField="FriendlyName"
                            DataValueField="Id" AutoPostBack="True">
                            <asp:ListItem Value="0" Text="<%$ Resources:CatalogStrings, Entry_Select_Warehouse %>"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td colspan="3" class="FormSpacerCell">
                    </td>
                </tr>
                <tr>
                    <td colspan="5" class="FormSpacerCell">
                    </td>
                </tr>
                <tr>
                    <td class="FormLabelCell">
                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:CatalogStrings, Entry_In_Stock %>"></asp:Label>:</td>
                    <td class="FormFieldCell">
                        <asp:TextBox runat="server" Width="50" ID="InStockQty"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ID="InStockRequired" ControlToValidate="InStockQty" Display="Dynamic" 
                            ErrorMessage="<%$ Resources:CatalogStrings, Entry_In_Stock_Quantity_Required %>" />
				        <asp:RangeValidator runat="server" ID="InStockRange" ControlToValidate="InStockQty" MinimumValue="-1000000000" MaximumValue="1000000000" Type="Double" 
                            Display="Dynamic" ErrorMessage="<%$ Resources:CatalogStrings, Entry_Enter_Valid_Quantity %>"></asp:RangeValidator>
                    </td>
                </tr>
                <tr>
                    <td colspan="5" class="FormSpacerCell">
                    </td>
                </tr>
                <tr>
                    <td class="FormLabelCell">
                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:SharedStrings, Reserved %>"></asp:Label>:</td>
                    <td class="FormFieldCell">
                        <asp:TextBox runat="server" Width="50" ID="ReservedQty"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ID="ReservedQtyRequired" ControlToValidate="ReservedQty" Display="Dynamic" 
                            ErrorMessage="<%$ Resources:CatalogStrings, Entry_Reserved_Quantity_Required %>" />
                        <asp:RangeValidator runat="server" ID="ReservedQtyRange" ControlToValidate="ReservedQty" MinimumValue="0" MaximumValue="1000000000" Type="Double" 
                            Display="Dynamic" ErrorMessage="<%$ Resources:CatalogStrings, Entry_Enter_Valid_Quantity %>"></asp:RangeValidator>
                        <asp:CompareValidator ID="ReservedQtyValidator" ControlToValidate="ReservedQty" runat="server"  Type="Double"
                            ControlToCompare="InStockQty" Operator="LessThanEqual" ErrorMessage="<%$ Resources:CatalogStrings, Entry_Inventory_ReservedQty_Invalid %>"></asp:CompareValidator>
                    </td>
                </tr>
                <tr>
                    <td colspan="5" class="FormSpacerCell">
                    </td>
                </tr>
                <tr>
                    <td class="FormLabelCell">
                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:CatalogStrings, Entry_Reorder_Min_Qty %>"></asp:Label>:</td>
                    <td class="FormFieldCell">
                        <asp:TextBox runat="server" Width="50" ID="ReorderMinQty"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ID="ReorderMinQtyRequired" ControlToValidate="ReorderMinQty" Display="Dynamic" 
                            ErrorMessage="<%$ Resources:CatalogStrings, Entry_Reorder_Min_Qty_Required %>" />
                        <asp:RangeValidator runat="server" ID="ReorderMinQtyRange" ControlToValidate="ReorderMinQty" MinimumValue="0" MaximumValue="1000000000" Type="Double" 
                            Display="Dynamic" ErrorMessage="<%$ Resources:CatalogStrings, Entry_Enter_Valid_Quantity %>"></asp:RangeValidator>
                    </td>
                </tr>
                <tr>
                    <td colspan="5" class="FormSpacerCell">
                    </td>
                </tr>
                <tr>
                    <td colspan="5" class="FormSpacerCell">
                    </td>
                </tr>
                <tr>
                    <td class="FormLabelCell">
                        <asp:Label ID="Label6" runat="server" Text="<%$ Resources:CatalogStrings, Entry_Preorder_Qty %>"></asp:Label>:</td>
                    <td class="FormFieldCell">
                        <asp:TextBox runat="server" Width="50" ID="PreorderQty"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ID="PreorderQtyRequired" ControlToValidate="PreorderQty" Display="Dynamic" ErrorMessage="<%$ Resources:CatalogStrings, Entry_Preorder_Qty_Required %>" />
                        <asp:RangeValidator runat="server" ID="PreorderQtyRange" ControlToValidate="PreorderQty" MinimumValue="0" MaximumValue="1000000000" Type="Double" Display="Dynamic" ErrorMessage="<%$ Resources:CatalogStrings, Entry_Enter_Valid_Quantity %>"></asp:RangeValidator>
                    </td>
                </tr>
                <tr>
                    <td colspan="5" class="FormSpacerCell">
                    </td>
                </tr>
                <tr>
                    <td class="FormLabelCell">
                        <asp:Label ID="Label7" runat="server" Text="<%$ Resources:CatalogStrings, Entry_Preorder_Available %>"></asp:Label>:</td>
                    <td class="FormFieldCell">
                        <ecf:CalendarDatePicker runat="server" ID="PreorderAvail" />
                    </td>
                </tr>
                <tr>
                    <td colspan="5" class="FormSpacerCell">
                    </td>
                </tr>
                <tr>
                    <td colspan="5" class="FormSpacerCell">
                    </td>
                </tr>
                <tr>
                    <td class="FormLabelCell">
                        <asp:Label ID="Label9" runat="server" Text="<%$ Resources:CatalogStrings, Entry_Backorder_Qty %>"></asp:Label>:</td>
                    <td class="FormFieldCell">
                        <asp:TextBox runat="server" Width="50" ID="BackorderQty"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ID="BackorderQtyRequired" ControlToValidate="BackorderQty" Display="Dynamic" ErrorMessage="<%$ Resources:CatalogStrings, Entry_Backorder_Qty_Required %>" />
                        <asp:RangeValidator runat="server" ID="BackorderQtyRange" ControlToValidate="BackorderQty" MinimumValue="0" MaximumValue="1000000000" Type="Double" Display="Dynamic" ErrorMessage="<%$ Resources:CatalogStrings, Entry_Enter_Valid_Quantity %>"></asp:RangeValidator>
                    </td>
                </tr>
                <tr>
                    <td colspan="5" class="FormSpacerCell">
                    </td>
                </tr>
                <tr>
                    <td class="FormLabelCell">
                        <asp:Label ID="Label11" runat="server" Text="<%$ Resources:CatalogStrings, Entry_Backorder_Available %>"></asp:Label>:</td>
                    <td class="FormFieldCell">
                        <ecf:CalendarDatePicker runat="server" ID="BackorderAvail" />
                    </td>
                </tr>
                <tr>
                    <td colspan="5" class="FormSpacerCell">
                    </td>
                </tr>
                <tr>
                    <td class="FormLabelCell">
                        <asp:Label ID="Label10" runat="server" Text="<%$ Resources:CatalogStrings, Entry_Inventory_Status %>"></asp:Label>:
                    </td>
                    <td class="FormFieldCell">
                        <asp:DropDownList runat="server" ID="InventoryStatusList" DataTextField="FriendlyName"
                            DataValueField="Name">
                            <asp:ListItem Value="False" Text="<%$ Resources:SharedStrings, Disabled %>"></asp:ListItem>
                            <asp:ListItem Value="True" Text="<%$ Resources:SharedStrings, Enabled %>"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
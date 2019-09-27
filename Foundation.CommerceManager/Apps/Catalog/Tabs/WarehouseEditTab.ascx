<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WarehouseEditTab.ascx.cs" Inherits="Mediachase.Commerce.Manager.Catalog.Tabs.WarehouseEditTab" %>
<%@ Register Src="../../Core/Controls/BooleanEditControl.ascx" TagName="BooleanEditControl" TagPrefix="ecf" %>
<div id="DataForm">
    <table class="DataForm">
        <tr>
            <td class="FormLabelCell">
                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:SharedStrings, Name %>"></asp:Label>:</td>
            <td class="FormFieldCell">
                <asp:TextBox runat="server" Width="250" ID="Name" MaxLength="255"></asp:TextBox><br />
                <asp:RequiredFieldValidator runat="server" ID="NameRequired" ControlToValidate="Name" Display="Dynamic" ErrorMessage="<%$ Resources:CatalogStrings, Catalog_Name_Required %>" />
                <asp:RegularExpressionValidator runat="server" ErrorMessage="<%$ Resources:CatalogStrings, Warehouse_Name_Invalid %>"
                    ControlToValidate="Name" ValidationExpression="^.{0,255}$" Display="Dynamic"></asp:RegularExpressionValidator>
                <asp:CustomValidator runat="server" ErrorMessage="<%$ Resources:CatalogStrings, Warehouse_Name_Invalid %>"
                    ControlToValidate="Name" OnServerValidate="SpecialCharacterValidate" Display="Dynamic"></asp:CustomValidator>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="FormSpacerCell"></td>
        </tr>
        <tr>
            <td class="FormLabelCell">
                * <asp:Label ID="Label9" runat="server" Text="<%$ Resources:SharedStrings, Code %>"></asp:Label>:</td>
            <td class="FormFieldCell">
                <asp:TextBox runat="server" Width="250" ID="CodeText" MaxLength="50"></asp:TextBox><br />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="CodeText" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                <asp:CustomValidator runat="server" ID="CodeUniqueCustomValidator" ControlToValidate="CodeText"
                    OnServerValidate="WarehouseCodeCheck" Display="Dynamic" ErrorMessage="<%$ Resources:CatalogStrings, Warehouse_Code_AlreadyExists %>" />
                <asp:RegularExpressionValidator runat="server" ErrorMessage="<%$ Resources:CatalogStrings, Warehouse_Code_Invalid %>"
                    ControlToValidate="CodeText" ValidationExpression="^[- \w]{0,50}$" Display="Dynamic"></asp:RegularExpressionValidator>              
            </td>
        </tr>
        <tr>
            <td colspan="2" class="FormSpacerCell"></td>
        </tr>
        <tr>
            <td class="FormLabelCell">
                <asp:Label ID="Label11" runat="server" Text="<%$ Resources:SharedStrings, Sort_Order %>"></asp:Label>:</td>
            <td class="FormFieldCell">
                <asp:TextBox runat="server" Width="50" ID="SortOrder" Text="0"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="SortOrder" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                <asp:RangeValidator ID="RangeValidator1" runat="server" Type="Integer" ControlToValidate="SortOrder" MinimumValue="-2147483648" MaximumValue="2147483647" ErrorMessage="*" Display="Dynamic"></asp:RangeValidator>
                <br />
                <asp:Label ID="Label5" CssClass="FormFieldDescription" runat="server" Text="<%$ Resources:CatalogStrings, Entry_Sort_Order %>"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="FormSpacerCell"></td>
        </tr>
        <tr>
            <td class="FormLabelCell">
                <asp:Label ID="Label6" runat="server" Text="<%$ Resources:SharedStrings, Available %>"></asp:Label>:</td>
            <td class="FormFieldCell">
                <ecf:BooleanEditControl id="IsActive" runat="server" MDContext="<%# Mediachase.Commerce.Catalog.CatalogContext.MetaDataContext %>"></ecf:BooleanEditControl>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="FormSpacerCell"></td>
        </tr>
        <tr>
            <td class="FormLabelCell">
                <asp:Label ID="Label3" runat="server" Text="<%$ Resources:SharedStrings, Is_Primary %>"></asp:Label>:</td>
            <td class="FormFieldCell">
                <ecf:BooleanEditControl id="IsPrimary" runat="server" MDContext="<%# Mediachase.Commerce.Catalog.CatalogContext.MetaDataContext %>"></ecf:BooleanEditControl>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="FormSpacerCell"></td>
        </tr>
        <tr>
            <td class="FormLabelCell">
                <asp:Label ID="Label2" runat="server" Text="<%$ Resources:CatalogStrings, Warehouse_IsFulfillmentCenter %>"></asp:Label>:</td>
            <td class="FormFieldCell">
                <ecf:BooleanEditControl id="IsFulfillmentCenter" runat="server" MDContext="<%# Mediachase.Commerce.Catalog.CatalogContext.MetaDataContext %>"></ecf:BooleanEditControl>
                <asp:Label ID="Label8" CssClass="FormFieldDescription" runat="server" Text="<%$ Resources:CatalogStrings, Warehouse_IsFulfillmentCenter_Description %>"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="FormSpacerCell"></td>
        </tr>
        <tr>
            <td class="FormLabelCell">
                <asp:Label ID="Label4" runat="server" Text="<%$ Resources:CatalogStrings, Warehouse_IsPickupLocation %>"></asp:Label>:</td>
            <td class="FormFieldCell">
                <ecf:BooleanEditControl id="IsPickupLocation" runat="server" MDContext="<%# Mediachase.Commerce.Catalog.CatalogContext.MetaDataContext %>"></ecf:BooleanEditControl>
                <asp:Label ID="Label10" CssClass="FormFieldDescription" runat="server" Text="<%$ Resources:CatalogStrings, Warehouse_IsPickupLocation_Description %>"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="FormSpacerCell"></td>
        </tr>
        <tr>
            <td class="FormLabelCell">
                <asp:Label ID="Label7" runat="server" Text="<%$ Resources:CatalogStrings, Warehouse_IsDeliveryLocation %>"></asp:Label>:</td>
            <td class="FormFieldCell">
                <ecf:BooleanEditControl id="IsDeliveryLocation" runat="server" MDContext="<%# Mediachase.Commerce.Catalog.CatalogContext.MetaDataContext %>"></ecf:BooleanEditControl>
                <asp:Label ID="Label12" CssClass="FormFieldDescription" runat="server" Text="<%$ Resources:CatalogStrings, Warehouse_IsDeliveryLocation_Description %>"></asp:Label>
            </td>
        </tr>
    </table>
</div>

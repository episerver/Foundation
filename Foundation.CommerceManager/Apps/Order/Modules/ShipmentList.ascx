<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ShipmentList.ascx.cs" Inherits="Mediachase.Commerce.Manager.Apps.Order.ShipmentList" %>
<%@ Register Src="~/Apps/Core/Controls/EcfListViewControl.ascx" TagName="EcfListViewControl" TagPrefix="core" %>
<% if (ShowFilter)
   { %>
<style type="text/css">
	.DockContainer
	{
		background-color: #DFE8F6;
	}
	.x-toolbar
	{
		border-width: 1px;
	}
</style>
<% } %>
<IbnWebControls:McDock ID="DockLeft" runat="server" Anchor="left" EnableSplitter="False" DefaultSize="5">
	<DockItems>
	</DockItems>
</IbnWebControls:McDock>
<IbnWebControls:McDock ID="DockRight" runat="server" Anchor="right" EnableSplitter="False" DefaultSize="5">
	<DockItems>
	</DockItems>
</IbnWebControls:McDock>
<IbnWebControls:McDock ID="DockTop" runat="server" Anchor="top" EnableSplitter="False" DefaultSize="30">
	<DockItems>
		<div style="padding: 6px 5px 6px 7px">
			<asp:Label runat="server" ID="WarehouseLabel" AssociatedControlID="Warehouse" Text="<%$ Resources:OrderStrings, Warehouse_Colon %>" Visible="false"></asp:Label>
			<asp:DropDownList runat="server" ID="Warehouse" OnSelectedIndexChanged="WarehouseChanged" AutoPostBack="true" Visible="false">
			</asp:DropDownList>
		</div>
	</DockItems>
</IbnWebControls:McDock>
<IbnWebControls:McDock ID="DockBottom" runat="server" Anchor="bottom" EnableSplitter="False" DefaultSize="5">
	<DockItems>
	</DockItems>
</IbnWebControls:McDock>
<asp:ObjectDataSource ID="ObjectDataSource1" runat="server" TypeName="Mediachase.Commerce.Manager.Apps.Order.ShipmentList"
        EnablePaging="true" SelectCountMethod="GetTotalShipmentCount" SelectMethod="FindShipment" SortParameterName="orderBy" 
        StartRowIndexParameterName="startingRecord" MaximumRowsParameterName="recordsToRetrieve" >
    <SelectParameters>
        <asp:ControlParameter Name="selectedWarehouseCode" ControlID="Warehouse" PropertyName="SelectedValue" />
        <asp:QueryStringParameter Name="selectedPickListId" QueryStringField="PickListId" />
        <asp:QueryStringParameter Name="shipmentStatus" QueryStringField="status" DefaultValue="Released" />
    </SelectParameters>
</asp:ObjectDataSource>
<core:EcfListViewControl ID="MyListView" runat="server" DataSourceID="ObjectDataSource1" AppId="Order" ViewId="Shipment-List-Released" ShowTopToolbar="true"></core:EcfListViewControl>

<%@ Control Language="C#" AutoEventWireup="true" Inherits="Mediachase.Commerce.Manager.Order.OrdersList" Codebehind="OrderList.ascx.cs" %>
<%@ Register Src="~/Apps/Core/Controls/EcfListViewControl.ascx" TagName="EcfListViewControl" TagPrefix="core" %>
<%@ Register TagPrefix="orders" Namespace="Mediachase.Commerce.Orders.DataSources" Assembly="Mediachase.Commerce" %>
<%@ Register TagPrefix="IbnWebControls" Namespace="Mediachase.BusinessFoundation" Assembly="Mediachase.BusinessFoundation" %>
<IbnWebControls:McDock ID="DockTop" Visible="<%# EnableMarketDropdown() %>" runat="server" Anchor="Top" EnableSplitter="False" DefaultSize="<%# EnableMarketDropdown()?30:0 %>">
    <DockItems>
   <div style="padding-left: 10px; padding-top: 5px;">
        <strong><asp:Literal runat="server" ID="LabelMarketFilter" Text="<%$ Resources:SharedStrings, Filter_By_Market %>"></asp:Literal></strong>
                <asp:DropDownList runat="server" ID="Markets" OnSelectedIndexChanged="Markets_OnSelectedIndexChanged" AutoPostBack="true"/>
       </div>


	</DockItems>
</IbnWebControls:McDock>
<orders:OrderDataSource runat="server" ID="OrderListDataSource"></orders:OrderDataSource>
<core:EcfListViewControl id="MyListView" runat="server" DataSourceID="OrderListDataSource" AppId="Order" ViewId="Orders-List" ShowTopToolbar="true"></core:EcfListViewControl>


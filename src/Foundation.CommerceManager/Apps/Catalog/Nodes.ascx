<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Nodes.ascx.cs" Inherits="Mediachase.Commerce.Manager.Catalog.Nodes" %>
<%@ Register Src="../Core/Controls/EcfListViewControl.ascx" TagName="EcfListViewControl" TagPrefix="core" %>
<core:EcfListViewControl id="MyListView2" DataSourceID="CatalogItemsDataSource" runat="server" AppId="Catalog" ViewId="Node-List" ShowTopToolbar="true"></core:EcfListViewControl>
<catalog:CatalogItemsDataSource runat="server" ID="CatalogItemsDataSource"></catalog:CatalogItemsDataSource>
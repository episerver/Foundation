<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Packages.ascx.cs" Inherits="Mediachase.Commerce.Manager.Order.Shipping.Packages" %>
<%@ Register Src="~/Apps/Core/Controls/EcfListViewControl.ascx" TagName="EcfListViewControl" TagPrefix="core" %>
<core:EcfListViewControl id="MyListView" runat="server" DataKey="PackageId" AppId="Order" ViewId="Packages-List" ShowTopToolbar="true"></core:EcfListViewControl>

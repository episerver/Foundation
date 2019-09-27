<%@ Control Language="c#" Inherits="Mediachase.Commerce.Manager.Order.Taxes" Codebehind="Taxes.ascx.cs" %>
<%@ Register Src="~/Apps/Core/Controls/EcfListViewControl.ascx" TagName="EcfListViewControl" TagPrefix="core" %>
<core:EcfListViewControl id="MyListView" runat="server" DataKey="TaxId" AppId="Order" ViewId="Taxes-List" ShowTopToolbar="true"></core:EcfListViewControl>
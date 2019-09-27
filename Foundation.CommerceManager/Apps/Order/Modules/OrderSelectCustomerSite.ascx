<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrderSelectCustomerSite.ascx.cs" Inherits="Mediachase.Commerce.Manager.Apps.Order.Modules.OrderSelectCustomerSite" %>
<%@ Register TagPrefix="mc" TagName="EntityDD" Src="~/Apps/MetaUIEntity/Modules/EntityDropDown.ascx" %>
<%@ Register TagPrefix="mc2" Assembly="Mediachase.BusinessFoundation" Namespace="Mediachase.BusinessFoundation" %>
<style>
	.orderform-blockheaderlight-datatable2 tr td
	{
		padding: 5px;
	}
	.orderform-label orderform-label-normal
	{
		font-weight: bold;
	}
</style>
<div runat="server" id="mainDiv" style="overflow:auto;background-color:White;margin-top:-1px; margin-left:-1px; height: 100%;" class="ibn-stylebox2 ibn-propertysheet">
	<div class="popup-outer">
	<table width="95%" runat="server" id="MainOrderInfoTable">
		<tr>
			<td style="padding: 7px;" valign="top">
			<table class="orderform-blockheaderlight-datatable2" cellpadding="5" cellspacing="3">
				<tr>
					<td  class="orderform-label orderform-label-normal">
						<asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:SharedStrings, Customer%>" />:
					</td>
					<td class="orderform-field">
						<mc:EntityDD ID="refObjects" ItemCount="10" runat="server" Width="100%" ViewName="SelectContact" />
						<asp:Label runat =server ID="lblCustomer" Visible="false"></asp:Label>
						<asp:CustomValidator runat="server" ID="ctrlValidator" ValidationGroup="SelectPreOrder" ErrorMessage="*"></asp:CustomValidator>
					</td>
				</tr>
			</table>
			</td>
		</tr>
	</table>
	</div>
</div>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReturnOrderDetails.ascx.cs" Inherits="Mediachase.Commerce.Manager.Apps.Order.Modules.ReturnOrderDetails" %>
<%@ Register src="~/Apps/Core/Controls/ButtonsHolder.ascx" tagname="ButtonsHolder" tagprefix="uc1" %>
<%@ Register TagPrefix="mc2" Namespace="Mediachase.Commerce.Manager.Apps.Common.Design"
	Assembly="Mediachase.ConsoleManager" %>
<table width="100%">
	<tr>
		<td style="width: 30%;" class="orderform-datatable-column">
		    <mc2:BlockHeaderLight HeaderCssClass="ibn-toolbar-light"  runat="server" Title="<%$ Resources:SharedStrings, Overview %>"></mc2:BlockHeaderLight>
			<table class="orderform-blockheaderlight-datatable">
				<tr>
					<td class="orderform-label orderform-label-left">
						<asp:Label ID="Label4" runat="server" Text="<%$ Resources:OrderStrings, Date_Time_Initiated %>"></asp:Label>:
					</td>
					<td class="orderform-field">
						<asp:Label ID="lblCreateDateTime" runat="server"></asp:Label>
					</td>
				</tr>
				<tr>
					<td class="orderform-label orderform-label-left">
						<asp:Label ID="Label1" runat="server" Text="<%$ Resources:SharedStrings, Created_By %>"></asp:Label>:
					</td>
					<td class="orderform-field">
						<asp:Label ID="lblCreatedBy" runat="server"></asp:Label>
					</td>
				</tr>
				<tr>
					<td class="orderform-label orderform-label-left">
						<asp:Label ID="Label10" runat="server" Text="<%$ Resources:SharedStrings, Status %>"></asp:Label>:
					</td>
					<td class="orderform-field">
						<asp:Label ID="lblStatus" runat="server"></asp:Label>
					</td>
				</tr>
			</table>
            <mc2:BlockHeaderLight HeaderCssClass="ibn-toolbar-light"  runat="server" Title="<%$ Resources:SharedStrings, Total_Information %>"></mc2:BlockHeaderLight>
            <table class="orderform-blockheaderlight-datatable">
                <tr>
					<td class="orderform-label orderform-label-left orderform-label-normal">
						<asp:Label runat="server" Text="<%$ Resources:SharedStrings, Line_Items %>"></asp:Label>:
					</td>
					<td class="orderform-field">
						<asp:Label ID="lblItemsTotal" runat="server"></asp:Label>
					</td>
				</tr>
                <tr>
					<td class="orderform-label orderform-label-left label-divider orderform-label-normal">
						<asp:Label ID="Label3" runat="server" Text="<%$ Resources:SharedStrings, Invalidated_Discounts %>"></asp:Label>:
					</td>
					<td class="orderform-field label-divider">
						<asp:Label ID="lblDiscountTotal" runat="server"></asp:Label>
					</td>
				</tr>
				<tr>
					<td class="orderform-label orderform-label-left">
						<asp:Label ID="Label11" runat="server" Text="<%$ Resources:OrderStrings, Return_Total %>"></asp:Label>:
					</td>
					<td class="orderform-field">
						<asp:Label ID="lblReturnTotal" runat="server"></asp:Label>
					</td>
				</tr>
            </table>
		</td>
		<td style="width: 30%;" class="orderform-datatable-column">
			<table class="orderform-innertable">
				<tr>
					<td class="orderform-label orderform-label-left">
						<asp:Label ID="Label2" runat="server" Text="<%$ Resources:SharedStrings, Notes %>"></asp:Label>:
					</td>
				</tr>
				<tr>
					<td class="orderform-field">
						<asp:Label ID="lblNotes" runat="server"></asp:Label>
					</td>
				</tr>
			</table>
		</td>
		<td class="orderform-datatable-column-last">
			<mc2:BlockHeaderLight HeaderCssClass="ibn-toolbar-light" ID="bh2" runat="server"
				Title="<%$ Resources:OrderStrings, Returns_Actions %>"></mc2:BlockHeaderLight>
			<table class="orderform-blockheaderlight-datatable">
				<tr>
					<td style="padding:7px;text-align:center;" align="center">
					    <uc1:ButtonsHolder ID="ReturnsButtonsHolder"  Direction="Vertical" runat="server" />
					</td>
				</tr>
			</table>
		</td>
		<td class="orderform-datatable-column-last">
			<mc2:BlockHeaderLight HeaderCssClass="ibn-toolbar-light" ID="BlockHeaderLight1" runat="server"
				Title="<%$ Resources:OrderStrings, Exchange_Actions %>"></mc2:BlockHeaderLight>
			<table class="orderform-blockheaderlight-datatable">
				<tr>
					<td style="padding: 7px; text-align: center;" align="center">
						<uc1:ButtonsHolder ID="ExchangeButtonsHolder" Direction="Vertical" runat="server" />
					</td>
				</tr>
			</table>
		</td>
	</tr>
</table>
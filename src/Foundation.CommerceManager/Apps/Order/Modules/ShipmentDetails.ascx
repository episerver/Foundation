<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ShipmentDetails.ascx.cs"
	Inherits="Mediachase.Commerce.Manager.Apps.Order.Modules.ShipmentDetails" %>
<%@ Register TagPrefix="mc2" Namespace="Mediachase.Commerce.Manager.Apps.Common.Design"
	Assembly="Mediachase.ConsoleManager" %>
<%@ Register Src="~/Apps/Customer/Modules/EcfListViewControlWithoutDockTop.ascx"
	TagName="EcfListViewControl" TagPrefix="cm" %>
<table cellpadding="5" cellspacing="10" width="100%">
	<tr>
		<td style="padding: 3px; width: 250px;" valign="top">
			<table width="100%" >
				<tr>
					<td style="padding: 3px; width: 250px;" valign="top">
						<table style="border-collapse: collapse;">
							<tr>
								<td class="centertext">
									<cm:EcfListViewControl ID="MyListView" runat="server" ShowTopToolbar="false" LayoutResizeEnable="false"
										AutoFullHeight="false" AutocountHeaderBottom="true"></cm:EcfListViewControl>
								</td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
		</td>
	</tr>
	<tr>
		<td>
			<table>
				<tr>
					<td style="padding: 3px; width: 100%;" valign="top">
						<mc2:BlockHeaderLight HeaderCssClass="ibn-toolbar-light" ID="BlockHeaderLight1" runat="server"
							Title="<%$ Resources:OrderStrings, Shipping_Information %>"></mc2:BlockHeaderLight>
						<table class="orderform-blockheaderlight-datatable" style="table-layout: fixed; width: 350px;">
							<tr>
								<td valign="top" style="width: 340px; font-weight: bold; text-align: left;">
									<asp:Label ID="Label4" runat="server" Text="<%$ Resources:OrderStrings, Shipping_Address %>"></asp:Label>:
								</td>
							</tr>
							<tr>
								<td>
									<asp:Literal Mode="Encode" ID="lblShipAddress" runat="server"></asp:Literal>
								</td>
							</tr>
							<tr>
								<td valign="top" style="width: 340px; font-weight: bold; text-align: left;">
									<asp:Label ID="Label1" runat="server" Text="<%$ Resources:OrderStrings, Shipping_Method %>"></asp:Label>:
								</td>
							</tr>
							<tr>
								<td>
									<asp:Label ID="lblShipMethod" runat="server"></asp:Label>
								</td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
		</td>
	</tr>
</table>

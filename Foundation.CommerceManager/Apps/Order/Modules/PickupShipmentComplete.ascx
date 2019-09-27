<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PickupShipmentComplete.ascx.cs" Inherits="Mediachase.Commerce.Manager.Apps.Order.Modules.PickupShipmentComplete" %>
<div style="padding:10px;">
	<asp:Label runat="server" ID="InfoLabel" Visible="false" CssClass="ibn-alerttext"></asp:Label>
	<table cellpadding="0" cellspacing="5" width="100%" runat="server" id="InfoTable">
		<tr>
			<td style="width:150px;" class="FormLabelCell">
			<asp:Label ID="Label1" runat="server" Text="<%$ Resources:OrderStrings, Shipment_Id %>"></asp:Label>:
			</td>
			
			<td class="FormFieldCell">
				<asp:Label runat="server" ID="ShipmentIdLabel"></asp:Label>
			</td>
		</tr>
		<tr>
			<td class="FormLabelCell">
		    <asp:Label ID="Label2" runat="server" Text="<%$ Resources:SharedStrings, Customer %>"></asp:Label>:
            </td>
			<td class="FormFieldCell">
				<asp:Label runat="server" ID="CustomerLabel"></asp:Label>
			</td>
		</tr>
		<tr>
			<td class="FormLabelCell">
			<asp:Label ID="Label3" runat="server" Text="<%$ Resources:OrderStrings, Shipping_Address %>"></asp:Label>:
            </td>
			<td class="FormFieldCell">
				<asp:Literal Mode="Encode" runat="server" ID="AddressLabel"></asp:Literal>
			</td>
		</tr>
		<tr>
			<td class="FormLabelCell">
			<asp:Label ID="Label4" runat="server" Text="<%$ Resources:OrderStrings, Shipping_Method %>"></asp:Label>:
            </td>
			<td class="FormFieldCell">
				<asp:Label runat="server" ID="MethodLabel"></asp:Label>
			</td>
		</tr>
        <!-- This shouldn't be here except the validation group stuff... figure it out -->
		<tr runat="server" visible="false">
			<td class="FormLabelCell">
			<asp:Label ID="Label5" runat="server" Text="<%$ Resources:OrderStrings, Tracking_Number %>"></asp:Label>:
            </td>
			<td class="FormFieldCell">
				<asp:TextBox runat="server" ID="TrackingNumber" Width="200"></asp:TextBox>
				<asp:RequiredFieldValidator runat="server" ID="TrackingNumberValidator" ControlToValidate="TrackingNumber" Display="Dynamic" ValidationGroup="requiredGroup" ErrorMessage="*"></asp:RequiredFieldValidator>
			</td>
		</tr>
	</table>
</div>
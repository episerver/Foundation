<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ShipmentCompleteAny.ascx.cs" Inherits="Mediachase.Commerce.Manager.Apps.Order.Modules.ShipmentCompleteAny" %>
<div style="padding:10px;">
    
    <asp:ListView ID="Shipments" runat="server" DataKeyNames="ShipmentId" ItemPlaceholderID="itemplaceholder">
    <LayoutTemplate>
    	<table cellpadding="0" cellspacing="5" width="100%">
	    <tr><td colspan="2"><hr /></td></tr>
        <asp:placeholder ID="itemplaceholder" runat="server"></asp:placeholder>
        </table>
    </LayoutTemplate>
    <ItemTemplate>
    		<tr>
			<td style="width:200px;"  class="FormLabelCell">
			<asp:Label ID="Label2" runat="server" Text="<%$ Resources:OrderStrings, Shipment_Id %>"></asp:Label>:
		    </td>
			<td class="FormFieldCell">
				<asp:Label runat="server" ID="ShipmentNoLabel" Text='<%#Eval("ShipmentId") %>'></asp:Label>
			</td>
		</tr>
		<tr>
			<td class="FormLabelCell">
			<asp:Label ID="Label6" runat="server" Text="<%$ Resources:SharedStrings, Customer %>"></asp:Label>:
		    </td>
			<td class="FormFieldCell">
				<asp:Label runat="server" ID="CustomerLabel"><%#Eval("CustomerName")%></asp:Label>
			</td>
		</tr>
		<tr>
			<td class="FormLabelCell">
			<asp:Label ID="Label3" runat="server" Text="<%$ Resources:OrderStrings, Shipping_Address %>"></asp:Label>:
			</td>
			<td class="FormFieldCell">
				<asp:Label runat="server" ID="AddressLabel"><%#HttpUtility.HtmlEncode(Eval("ShippingAddress"))%></asp:Label>
			</td>
		</tr>
		<tr>
			<td class="FormLabelCell">
			<asp:Label ID="Label4" runat="server" Text="<%$ Resources:OrderStrings, Shipping_Method %>"></asp:Label>:
			</td>
			<td class="FormFieldCell">
				<asp:Label runat="server" ID="MethodLabel"><%#Eval("ShippingMethodName")%></asp:Label>
			</td>
		</tr>
        <tr>
			<td class="FormLabelCell">
			<asp:Label ID="Label5" runat="server" Text="<%$ Resources:OrderStrings, Tracking_Number %>"></asp:Label>:
			</td>
			<td class="DataCell">
				<asp:TextBox runat="server" ID="TrackingNumber" Width="200" ></asp:TextBox>
				<asp:RequiredFieldValidator runat="server" ID="TrackingNumberValidator" ControlToValidate="TrackingNumber" ErrorMessage="*" Display="static"></asp:RequiredFieldValidator>
			</td>
		</tr>
                <tr><td colspan="2"><hr /></td></tr>
    </ItemTemplate>
    </asp:ListView>
    <table cellpadding="0" cellspacing="0" border="0" width="100%" >
        <tr>
            <td style="padding:10px;">
                <asp:Label ID="ValidationText" runat="server" Text="" Visible="false"></asp:Label>
            </td>
        </tr>
	    <tr>
            <td align="center" style="padding:10px;">
                <asp:Button ID="btnSave" CausesValidation="true" runat="server" Text="<%$ Resources:SharedStrings, OK %>" OnClick="btnSave_ServerClick" style="width: 105px;" />
                <asp:Button ID="btnCancel" CausesValidation="false" runat="server" Text="<%$ Resources:SharedStrings, Cancel %>" OnClick="btnCancel_ServerClick" style="width: 105px;"/>
            </td>
	    </tr>
    </table>
</div>
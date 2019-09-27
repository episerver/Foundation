<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrderBillingAddress.ascx.cs" Inherits="Mediachase.Commerce.Manager.Apps.Order.CustomPrimitives.OrderBillingAddress" %>
<table cellpadding="0" cellspacing="0" border="0" width="100%">
    <tr>
	    <td valign="top" style="width: 140px; font-weight:bold; text-align:right;padding: 5px;">
		    <asp:Label ID="Label1" runat="server" Text="<%$ Resources:SharedStrings, Name %>"></asp:Label>:
	    </td>
	    <td style="padding: 5px;width:300px;">
            <asp:Literal Mode="Encode" ID="addrName" runat="server" ></asp:Literal>
	    </td>
	    <td style="padding: 5px;" rowspan="2" valign="top">
		    <button id="btnEditAddress" runat="server" style="width:150px;">
		        <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:SharedStrings, AddressEditBilling %>" />
		    </button>
	    </td>
    </tr>
    <tr>
	    <td valign="top" style="width: 140px; font-weight:bold; text-align:right;padding: 5px;">
		    <asp:Label ID="Label2" runat="server" Text="<%$ Resources:SharedStrings, Address %>"></asp:Label>:
	    </td>
	    <td style="padding: 5px;">
		    <asp:Literal Mode="Encode" ID="lblAddress" runat="server"></asp:Literal>
	    </td>
    </tr>
    <tr>
	    <td valign="top" style="width: 140px; font-weight:bold; text-align:right;padding: 5px;">
		    <asp:Label ID="Label3" runat="server" Text="<%$ Resources:SharedStrings, Phone_Number %>"></asp:Label>:
	    </td>
	    <td style="padding: 5px;">
		    <asp:Literal Mode="Encode" ID="lblPhone" runat="server"></asp:Literal>
	    </td>
    </tr>
</table>
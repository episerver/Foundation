<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrderLineItemEdit.ascx.cs"
	Inherits="Mediachase.Commerce.Manager.Apps.Order.Modules.OrderLineItemEdit" %>
<%@ Register TagPrefix="mc2" Assembly="Mediachase.BusinessFoundation" Namespace="Mediachase.BusinessFoundation" %>
<%@ Register Src="~/Apps/Core/MetaData/EditTab.ascx" TagName="MetaData" TagPrefix="ecf" %>
<div style="padding: 5px;">
	<asp:Label runat="server" ID="lblErrorInfo" Style="color: Red"></asp:Label>
	<table width="100%" class="DataForm">
		<tr>
			<td class="FormLabelCell">
				<asp:Label ID="Label7" runat="server" Text="<%$ Resources:SharedStrings, Code %>"></asp:Label>:
			</td>
			<td class="FormFieldCell">
				<asp:Label ID="CodeLabel" runat="server"></asp:Label>
			</td>
		</tr>
		<tr>
			<td class="FormLabelCell">
				<asp:Label ID="Label1" runat="server" Text="<%$ Resources:SharedStrings, Display_Name %>"></asp:Label>:
			</td>
			<td class="FormFieldCell">
				<asp:Label runat="server" ID="DisplayNameLabel"></asp:Label>
			</td>
		</tr>
		<tr>
			<td class="FormLabelCell">
				<asp:Label ID="Label3" runat="server" Text="<%$ Resources:SharedStrings, List_Price %>"></asp:Label>:
			</td>
            <td class="FormFieldCell">
			    <asp:Label runat="server" ID="ListPrice"></asp:Label>
		    </td>
		</tr>
        <tr>
            <td class="FormLabelCell">
                <asp:Label ID="LabelPlacedPrice" runat="server" Text="<%$ Resources:SharedStrings, Placed_Price %>"></asp:Label>:
            </td>
            <td class="FormFieldCell">
                <asp:TextBox runat="server" ID="PlacedPrice"></asp:TextBox>
			    <asp:RequiredFieldValidator runat="server" ID="rfvPlacedPrice" ControlToValidate="PlacedPrice"
				    Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator>
			    <asp:RangeValidator runat="server" ID="rvPlacedPrice" ControlToValidate="PlacedPrice"
				    ErrorMessage="*" MinimumValue="0" MaximumValue="999999" Type="Currency"></asp:RangeValidator>
            </td>
        </tr>
		<tr>
			<td class="FormLabelCell">
				<asp:Label ID="Label4" runat="server" Text="<%$ Resources:SharedStrings, Discount %>"></asp:Label>:
			</td>
			<td>
                <table>
                    <tbody>
                        <tr>
                            <td runat="server" id="DiscountDescrCell">
                                <asp:TextBox runat="server" ID="DiscountDescription"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="DiscountAmount" Width="50px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="DiscountAmountType">
                                    <asp:ListItem Text="<%$ Resources:MarketingStrings, Promotion_Percentage_Based %>" Value="1"></asp:ListItem>
						            <asp:ListItem Text="<%$ Resources:MarketingStrings, Promotion_Value_Based %>" Value="2"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1"
                                    ControlToValidate="DiscountAmount" Display="Dynamic"
                                    ErrorMessage="*"></asp:RequiredFieldValidator>
                                <asp:CustomValidator runat="server" ID="DiscountAmountValidator"
                                    ControlToValidate="DiscountAmount"
                                    ErrorMessage="*" OnServerValidate="DiscountAmountValidate" Type="Currency"></asp:CustomValidator>
                            </td>
                        </tr>
                        <tr runat="server" id="DiscountDescrRow" align="center">
                            <td>
                                <asp:Label runat="server" ID="Label13" CssClass="FormFieldDescription" Text="<%$ Resources:OrderStrings, New_LineItem_Discount_Description%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="Label5" CssClass="FormFieldDescription" Text="<%$ Resources:OrderStrings, New_LineItem_Discount_Amount%>"></asp:Label>
                            </td>
                            <td>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </td>
		</tr>
		<tr>
			<td class="FormLabelCell">
				<asp:Label ID="Label2" runat="server" Text="<%$ Resources:SharedStrings, Quantity %>"></asp:Label>:
			</td>
			<td class="FormFieldCell">
				<asp:TextBox runat="server" ID="Quantity" AutoPostBack="true"></asp:TextBox>
				<asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ValidationGroup="LineItemDetails"
					ControlToValidate="Quantity" Display="Dynamic" ErrorMessage="*" />
				<asp:RangeValidator ID="RangeValidator3" runat="server" ValidationGroup="LineItemDetails"
					ControlToValidate="Quantity" Display="Dynamic" ErrorMessage="*" Type="Double"
					MinimumValue="0" MaximumValue="10000"></asp:RangeValidator>
			</td>
		</tr>
        <tr runat="server" id="chargeForShipmentRow">
            <td class="FormLabelCell">
                <asp:CheckBox runat="server" ID="chkboxChargeForShipmentChange" />
                <asp:Label ID="lblChargeForShipment" runat="server" Text="<%$ Resources:OrderStrings, Shipment_ChargeForShipment %>" />
            </td>
        </tr>
	</table>
</div>
<div id="DataForm">
    <table class="DataForm"> 
        <ecf:MetaData ValidationGroup="OrderCreateVG" runat="server" ID="MetaDataTab" />
        <tr>
            <td>
                <mc2:IMButton ID="btnSave" runat="server" style="width: 105px;" OnServerClick="btnSave_ServerClick"></mc2:IMButton>
            </td>
            <td>
                <mc2:IMButton ID="btnCancel" runat="server" style="width: 105px;" CausesValidation="false"></mc2:IMButton>
            </td>
        </tr>
    </table>
</div>

            
  
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrderLineItemMove.ascx.cs" Inherits="Mediachase.Commerce.Manager.Apps.Order.Modules.OrderLineItemMove" %>
<%@ Import Namespace="Mediachase.Commerce.Shared" %>
<%@ Register TagPrefix="mc2" Assembly="Mediachase.BusinessFoundation" Namespace="Mediachase.BusinessFoundation" %>
<script type="text/javascript" src="<%= CommerceHelper.GetAbsolutePath("~/Apps/Shell/Scripts/jquery.js") %>" ></script>
<script type="text/javascript" src="<%= CommerceHelper.GetAbsolutePath("~/Apps/Shell/Scripts/jQuery/lib/jquery.bgiframe.js") %>" ></script>
<script type="text/javascript" src="<%= CommerceHelper.GetAbsolutePath("~/Apps/Shell/Scripts/jQuery/lib/jquery.linkselect.js") %>" ></script>
<link type="text/css" rel="stylesheet" href="<%= CommerceHelper.GetAbsolutePath("~/Apps/Shell/Scripts/jQuery/css/jquery.linkselect.style.select.css") %>" />
<script type="text/javascript">
    function ToggleShipmentTable() {
        var useExistingShipment = $get('<%= rbExistingShipment.ClientID %>').checked;

        ValidatorEnable($get('<%= ShipmentRequiredValidator.ClientID %>'), useExistingShipment);
        ValidatorEnable($get('<%= NewAddressRequiredValidator.ClientID %>'), !useExistingShipment);
        ValidatorEnable($get('<%= ShippingMethodRequiredValidator.ClientID %>'), !useExistingShipment);
    }
    function initSelector(selectCtrlId, descCtrlId, val) {
        $("select.jQuerySelect").linkselect({
            change: function() { $("#" + descCtrlId).val($("#" + selectCtrlId).linkselect("val")); }
        });
    }
</script>
<div class="popup-outer">
	<asp:Label runat="server" ID="lblErrorInfo" Style="color: Red"></asp:Label>
	<asp:Panel runat="server" ID="FormPanel">
	    <table width="100%" class="DataForm">
	        <asp:UpdatePanel UpdateMode="Conditional" ID="MainContentPanel" runat="server" RenderMode="Block">
		        <ContentTemplate>
		            <tr>
			            <td class="FormLabelCell">
				            <asp:Label ID="Label7" runat="server" Text="Quantity to move"></asp:Label>:
			            </td>
			            <td class="FormFieldCell">
				            <asp:TextBox runat="server" ID="tbQuantityToMove" Width="20"></asp:TextBox>
				            <asp:RequiredFieldValidator runat="server" ID="QuantityToMoveRequired" ValidationGroup="LineItemMoveOptionGroup"
					            ControlToValidate="tbQuantityToMove" Display="Dynamic" ErrorMessage="*" />
				            <asp:RangeValidator ID="QuantityToMoveRangeValidator" runat="server" ValidationGroup="LineItemMoveOptionGroup"
					            ControlToValidate="tbQuantityToMove" Display="Dynamic" ErrorMessage="*" Type="Integer"
					            MinimumValue="0" MaximumValue="10000"></asp:RangeValidator>
			            </td>
		            </tr>
		            <tr>
			            <td class="FormLabelCell">
                            <asp:RadioButton id="rbExistingShipment" runat="server" ValidationGroup="LineItemMoveOptionGroup" OnCheckedChanged="rbExistingShipment_CheckedChanged" 
                                AutoPostBack="true" GroupName="MoveShipmentToggle" />
                            <asp:Label runat="server" Text="Move to existing shipment" />
			            </td>
			            <td class="FormFieldCell">
    			            <asp:DropDownList runat="server" ID="ddlShipments" Width="200" DataValueField="ShipmentId" DataTextField="ShipmentId"
                                OnSelectedIndexChanged="ddlShipments_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
    			            <asp:RequiredFieldValidator id="ShipmentRequiredValidator" Runat="server" ControlToValidate="ddlShipments" Display="Dynamic" ValidationGroup="LineItemMoveOptionGroup">*</asp:RequiredFieldValidator>
			            </td>
		            </tr>
		            <tr>
			            <td class="FormLabelCell" colspan="2">
                            <asp:RadioButton id="rbNewShipment" runat="server" ValidationGroup="LineItemMoveOptionGroup" OnCheckedChanged="rbNewShipment_CheckedChanged" 
                                AutoPostBack="true" GroupName="MoveShipmentToggle" />
                            <asp:Label ID="Label1" runat="server" Text="Create new shipment" />
			            </td>
		            </tr>
		            <tr runat="server" id="NewShippingRow">
		                <td class="FormFieldCell" colspan="2">
                            <fieldset id="fsNewShippingDetails">
                                <legend>
                                </legend>
                                <table id="tblNewShippingDetails" runat="server">
                                    <tr>
                                        <td class="FormLabelCell">
                                            <asp:Label ID="lblAddress" runat="server" Text="Address"></asp:Label>:
                                        </td>
                                        <td class="FormFieldCell">
                                            <asp:HiddenField runat="server" ID="hfSelectedShippingAddress" />
                                            <asp:DropDownList runat="server" ID="ddlNewShippingAddress" Width="350" ></asp:DropDownList>
                                            <asp:RequiredFieldValidator id="NewAddressRequiredValidator" Runat="server" ControlToValidate="ddlNewShippingAddress" Display="Dynamic" ValidationGroup="LineItemMoveOptionGroup">*</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="FormLabelCell">
                                            <asp:Label ID="lblShippingMethod" runat="server" Text="<%$ Resources:OrderStrings, Shipping_Method %>"></asp:Label>:
                                        </td>
                                        <td class="FormFieldCell">
                                            <asp:DropDownList runat="server" ID="ddlNewShippingMethod" Width="300" DataValueField="ShippingMethodId" DataTextField="DisplayName" OnSelectedIndexChanged="ddlNewShippingMethod_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                            <asp:RequiredFieldValidator id="ShippingMethodRequiredValidator" Runat="server" ControlToValidate="ddlNewShippingMethod" Display="Dynamic" ValidationGroup="LineItemMoveOptionGroup">*</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
		            </tr>
                    <tr>
                        <td class="FormFieldCell">
                            <asp:CheckBox runat="server" ID="chkboxChargeForFromShipmentChange" />
                            <asp:Label ID="lblChargeForFromShipment" runat="server" Text="<%$ Resources:OrderStrings, Shipment_ChargeForFromShipment %>" />
                        </td>
                    </tr>
                    <tr>
                        <td class="FormFieldCell">
                            <asp:CheckBox runat="server" ID="chkboxChargeForToShipmentChange" />
                            <asp:Label ID="lblChargeForToShipment" runat="server" Text="<%$ Resources:OrderStrings, Shipment_ChargeForToShipment %>" />
                        </td>
                    </tr>
	            </ContentTemplate>
            </asp:UpdatePanel>
	    </table>
	</asp:Panel>
</div>
<%@ Control Language="C#" AutoEventWireup="true" Inherits="Mediachase.Commerce.Manager.Order.Tabs.OrderShipmentEditTab"
	CodeBehind="OrderShipmentEditTab.ascx.cs" %>
<%@ Register Src="~/Apps/Core/MetaData/EditTab.ascx" TagName="MetaData" TagPrefix="ecf" %>

<script type="text/javascript">
    function ecf_UpdateShipmentDialogControl(val) {
        var btn = $get('<%= SaveChangesButton.ClientID %>');
        if (btn != null)
            btn.disabled = false;
            
        var ctrl = $get('<%=DialogTrigger.ClientID%>');
        ctrl.value = val;
        __doPostBack('<%=DialogTrigger.UniqueID %>','');
    }
    
    function OrderShipment_UpdateSelectedField(ddObj, objToUpdate)
    {
        if(ddObj != null && objToUpdate != null)
            objToUpdate.value = ddObj.value;
    }

    function OrderShipmentSaveChangesButton_onClientClick(btn) {
        if (Page_ClientValidate('ShipmentMetaData')) {
            btn.disabled = true;
            __doPostBack('<%= SaveChangesButton.UniqueID %>', 'shipmentChanged');
        }
    }
</script>

<asp:HiddenField runat="server" ID="DialogTrigger" />
<asp:UpdatePanel UpdateMode="Conditional" ID="DialogContentPanel" runat="server"
	RenderMode="Inline">
	<Triggers>
		<asp:AsyncPostBackTrigger ControlID="DialogTrigger" />
	</Triggers>
	<ContentTemplate>
		<table width="100%" cellpadding="0" cellspacing="0">
			<tr>
				<td valign="middle" style="padding: 1px; width: 5px;">
				</td>
				<td style="padding: 1px;" align="left" valign="middle">
					<!-- Content Area -->
					<asp:HiddenField runat="server" ID="SelectedAddressField" />
					<asp:HiddenField runat="server" ID="SelectedShippingMethodField" />
					<table class="DataForm">
						<tr>
							<td class="FormLabelCell">
								<asp:Label ID="Label4" runat="server" Text="<%$ Resources:SharedStrings, Shipping_Method %>"></asp:Label>:
							</td>
							<td class="FormFieldCell">
								<asp:DropDownList runat="server" ID="ShippingMethodList" DataMember="ShippingMethod"
									DataTextField="DisplayName" DataValueField="ShippingMethodId">
								</asp:DropDownList>
								<asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ValidationGroup="ShipmentMetaData"
									ControlToValidate="ShippingMethodList" Display="Dynamic" ErrorMessage="<%$ Resources:SharedStrings, Shipping_Method_Required %>" />
							</td>
						</tr>
						<tr>
							<td class="FormLabelCell">
								<asp:Label ID="Label6" runat="server" Text="<%$ Resources:SharedStrings, Method_Name %>"></asp:Label>:
							</td>
							<td class="FormFieldCell">
								<asp:TextBox runat="server" ID="MethodName"></asp:TextBox>
							</td>
						</tr>
						<tr>
							<td class="FormLabelCell">
								<asp:Label ID="Label5" runat="server" Text="<%$ Resources:SharedStrings, Address %>"></asp:Label>:
							</td>
							<td class="FormFieldCell">
								<asp:DropDownList runat="server" ID="AddressList" DataTextField="Name" DataValueField="Name">
								</asp:DropDownList>
								<asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator0" ValidationGroup="ShipmentMetaData"
									ControlToValidate="AddressList" Display="Dynamic" ErrorMessage="<%$ Resources:SharedStrings, Address_Required %>" />
							</td>
						</tr>
						<tr>
							<td class="FormLabelCell">
								<asp:Label ID="Label1" runat="server" Text="<%$ Resources:SharedStrings, Tracking_Number %>"></asp:Label>:
							</td>
							<td class="FormFieldCell">
								<asp:TextBox runat="server" ID="TrackingNumber"></asp:TextBox>
							</td>
						</tr>
						<tr>
							<td class="FormLabelCell">
								<asp:Label ID="Label2" runat="server" Text="<%$ Resources:SharedStrings, Total %>"></asp:Label>:
							</td>
							<td class="FormFieldCell">
								<asp:TextBox runat="server" ID="ShipmentTotal"></asp:TextBox>
								<asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator7" ValidationGroup="ShipmentMetaData"
									ControlToValidate="ShipmentTotal" Display="Dynamic" ErrorMessage="*" />
								<asp:RangeValidator ID="RangeValidatorAmount" runat="server" ValidationGroup="ShipmentMetaData"
									ControlToValidate="ShipmentTotal" Display="Dynamic" ErrorMessage="*" Type="Currency"
									MinimumValue="0" MaximumValue="1000000000"></asp:RangeValidator>
							</td>
						</tr>
						<tr>
							<td class="FormLabelCell">
								<asp:Label ID="Label3" runat="server" Text="<%$ Resources:SharedStrings, Discount %>"></asp:Label>:
							</td>
							<td class="FormFieldCell">
								<asp:TextBox runat="server" ID="DiscountAmount"></asp:TextBox>
								<asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ValidationGroup="ShipmentMetaData"
									ControlToValidate="DiscountAmount" Display="Dynamic" ErrorMessage="*" />
								<asp:RangeValidator ID="RangeValidator1" runat="server" ValidationGroup="ShipmentMetaData"
									ControlToValidate="DiscountAmount" Display="Dynamic" ErrorMessage="*" Type="Currency"
									MinimumValue="0" MaximumValue="1000000000"></asp:RangeValidator>
							</td>
						</tr>
						<tr>
							<td class="FormLabelCell">
								<asp:Label ID="Label7" runat="server" Text="<%$ Resources:SharedStrings, Status %>"></asp:Label>:
							</td>
							<td class="FormFieldCell">
								<asp:TextBox runat="server" ID="Status"></asp:TextBox>
							</td>
						</tr>
						<tr>
							<td class="FormLabelCell">
								<asp:Label ID="Label8" runat="server" Text="<%$ Resources:SharedStrings, Items_To_Ship %>"></asp:Label>:
							</td>
							<td class="FormFieldCell">
								<asp:DataList runat="server" ID="ShipmentItemsList" ShowHeader="false" ShowFooter="false"
									DataKeyField="LineItemIndex" CellSpacing="0" CellPadding="0" Width="100%" CssClass="bordered">
									<ItemTemplate>
										<asp:CheckBox runat="server" ID="chbIsInShipment" Checked='<%# Eval("Checked") %>' Text='<%# Eval("Name") %>' />
									</ItemTemplate>
								</asp:DataList>
								<asp:Label ID="lblItemsToShip" runat="server"></asp:Label>
							</td>
						</tr>
						<asp:PlaceHolder runat="server" ID="MetaPlaceHolder"></asp:PlaceHolder>
						<ecf:MetaData ValidationGroup="ShipmentMetaData" runat="server" ID="MetaDataTab" />
					</table>
					<!-- /Content Area -->
				</td>
			</tr>
			<tr>
				<td colspan="2" style="background-image: url(../../../Apps/Shell/Styles/images/dialog/bottom_content.gif);
					height: 41px; padding-right: 10px;" align="right">
					<asp:Button runat="server" ID="SaveChangesButton" ValidationGroup="ShipmentMetaData"
						OnClientClick="OrderShipmentSaveChangesButton_onClientClick(this);return false;"
						Text="<%$ Resources:SharedStrings, Save_Changes %>" />
				</td>
			</tr>
		</table>
	</ContentTemplate>
</asp:UpdatePanel>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrderCreate.ascx.cs"
	Inherits="Mediachase.Commerce.Manager.Apps.Order.Modules.OrderCreate" %>
<%@ Register TagPrefix="mc2" Assembly="Mediachase.BusinessFoundation" Namespace="Mediachase.BusinessFoundation" %>
<%@ Register TagPrefix="mc3" Namespace="Mediachase.Commerce.Manager.Apps.Common.Design"
	Assembly="Mediachase.ConsoleManager" %>
<%@ Register Src="~/Apps/Customer/Modules/EcfListViewControlWithoutDockTop.ascx"
	TagName="EcfListViewControl" TagPrefix="cm" %>
<%@ Register Src="~/Apps/Order/Modules/OrderAddressEdit.ascx" TagName="AddressEdit"
	TagPrefix="cm" %>
<%@ Register TagPrefix="mc" TagName="EntityDD" Src="~/Apps/MetaUIEntity/Modules/EntityDropDown.ascx" %>
<%@ Register Src="~/Apps/Core/MetaData/EditTab.ascx" TagName="MetaData" TagPrefix="ecf" %>

<script type="text/javascript">
    $(document).ready(function () {
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(OrderCreateEndRequestHandler);
		if (document.URL && document.URL.indexOf('DialogPage') > 0) {
			$('form').css("overflow", "auto");
		}
    });

    function OrderCreateEndRequestHandler() {
        for (var i = 0; i < Page_Validators.length; i++) {
            Page_Validators[i].errormessage = "*";
            Page_Validators[i].innerHTML = "*";
        }
    }
</script>

<script type="text/javascript">
	function disableButton() {

		var applyBtn = $("#<%=btnApplyCouponCode.ClientID%>");
		var couponCode = $("#<%=tbCouponCode.ClientID%>");
		var couponResult = $("#<%=lbCouponResult.ClientID%>");
		couponResult.hide();
		if (couponCode.val() == '') 
		{
			applyBtn.attr("disabled", "disabled");
		}
		else 
		{
			applyBtn.removeAttr("disabled");
		}
	}
	
	function attachToBlurEvent()
	{
		//alert($('#addressContainerTable input[type="text"]').length);
		$('#addressContainerTable input[type="text"]').blur(function() {
		    var createorderButton = document.getElementById('<%= btnSave.ClientID %>');
		    if (Page_ClientValidate()) {
		        createorderButton.removeAttribute("disabled");
		    }
		    else {
		        createorderButton.setAttribute("disabled", "disabled");
		    }
			});
		/*each(function(index) {
			this.blur(function() { alert(Page_IsValid()); if (Page_IsValid())
				{
					createorderButton.removeAttribute("disabled"); 
				}
				else
				{
					createorderButton.setAttribute("disabled", "disabled"); 
				}
				});
		});		   */
	}

	function checkParentOrderId() {
	    //	    alert("In checkParentOrderId");
	    var createorderButton = document.getElementById('<%= btnSave.ClientID %>');
	    var parentOrderGroupInput = $('#orderMetaData input[type="text"]');
	    if (Page_ClientValidate()) {
	        createorderButton.removeAttribute("disabled");
	    }
	    else {
	        createorderButton.setAttribute("disabled", "disabled");
	    }
	}

	function validateMetaDataEvent() {
	    //	    alert("In validateMetaDataEvent");
	    $('#orderMetaData input[type="text"]').blur(checkParentOrderId);
	}
	
	$(document).ready(function() {
	    attachToBlurEvent();
	    validateMetaDataEvent();
	});
</script>


<div runat="server" id="mainDiv" style="overflow: auto; background-color: White;
	margin-top: -1px; margin-left: -1px;" class="ibn-stylebox2 ibn-propertysheet">
	<div class="popup-outer">
		<asp:Label runat="server" ID="lblErrorInfo" Style="color: Red"></asp:Label>
		<asp:Panel runat="server" ID="FormPanel">
			<asp:UpdatePanel UpdateMode="Conditional" ID="DialogContentPanel" runat="server"
				RenderMode="Block">
				<ContentTemplate>
					<table width="50%" runat="server" id="MainOrderInfoTable">
						<tr>
							<td style="padding: 7px;" valign="top" class="episerveroverwriteimage">
                                <!-- Title is set in code behind -->
								<mc2:BlockHeaderLight HeaderCssClass="ibn-toolbar-light" ID="BlockHeaderLightBasic"
									runat="server" Title="Basic Info"></mc2:BlockHeaderLight>
								<table class="orderform-blockheaderlight-datatable">
									<tr>
										<td style="width: 170px;" class="orderform-label orderform-label-normal"">
											<asp:Label ID="Label9" runat="server" Text="<%$ Resources:SharedStrings, Customer %>"></asp:Label>:
										</td>
										<td class="orderform-field">
											<asp:Label runat="server" ID="lblCustomer"></asp:Label>
										</td>
									</tr>
                                    <tr>
                                        <td class="orderform-label orderform-label-normal">
											<asp:Label ID="Label10" runat="server" Text="<%$ Resources:SharedStrings, Market %>"></asp:Label>:
										</td>
										<td class="orderform-field">
											<asp:UpdatePanel UpdateMode="Conditional" ID="OrderMarketUpdatePanel" runat="server"
												RenderMode="Inline">
												<ContentTemplate>
													<asp:DropDownList runat="server" ID="OrderMarketList" DataMember="Market" DataTextField="MarketName"
														DataValueField="MarketId" AutoPostBack="true">
													</asp:DropDownList>
													<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="OrderMarketList"
														Display="Dynamic" ValidationGroup="OrderCreateVG">*</asp:RequiredFieldValidator>
                                                    <ajaxToolkit:ModalPopupExtender ID="ConfirmMarketChangeModal" runat="server"
                                                        TargetControlID="hiddenTargetControlForModalPopup" PopupControlID="popUpModal"
                                                        BackgroundCssClass="modalPopupBackground"></ajaxToolkit:ModalPopupExtender>
                                                    <asp:Panel runat="server" ID="popUpModal" Style="display:none" CssClass="modalPopupWindow">
                                                        <div>
                                                            <asp:Label ID="Label11" runat="server" Text="Changing the market will clear the order, do you want to continue?"></asp:Label>
                                                            <br />
                                                            <asp:Button ID="btnYes" runat="server" Text="Yes" OnClick="btnYes_Click" CausesValidation="false"/>
                                                            <asp:Button ID="btnNo" runat="server" Text="No" OnClick="btnNo_Click" CausesValidation="false"/>
                                                        </div>
                                                    </asp:Panel>
                                                    <asp:Button runat="server" ID="hiddenTargetControlForModalPopup" Style="display: none" />
												</ContentTemplate>
											</asp:UpdatePanel>
										</td>
                                    </tr>
									<tr>
										<td class="orderform-label orderform-label-normal">
											<asp:Label ID="Label8" runat="server" Text="<%$ Resources:SharedStrings, Currency %>"></asp:Label>:
										</td>
										<td class="orderform-field">
											<asp:UpdatePanel UpdateMode="Conditional" ID="OrderCurrencyUpdatePanel" runat="server"
												RenderMode="Inline">
												<ContentTemplate>
													<asp:DropDownList runat="server" ID="OrderCurrencyList" DataMember="Currency" DataTextField="Name"
														DataValueField="CurrencyCode" AutoPostBack="true">
													</asp:DropDownList>
													<asp:RequiredFieldValidator ID="rfvOrderCurrency" runat="server" ControlToValidate="OrderCurrencyList"
														Display="Dynamic" ValidationGroup="OrderCreateVG">*</asp:RequiredFieldValidator>
												</ContentTemplate>
											</asp:UpdatePanel>
										</td>
									</tr>
									<tr>
										<td class="orderform-label orderform-label-normal">
											<asp:Label ID="Label6" runat="server" Text="<%$ Resources:SharedStrings, Coupon_Code %>"></asp:Label>:
										</td>
										<td class="orderform-field">
											<asp:UpdatePanel UpdateMode="Conditional" ID="UpdatePanel2" runat="server" RenderMode="Inline">
												<ContentTemplate>
												<span>
													<asp:TextBox runat="server" ID="tbCouponCode" CssClass="applyCouponButtonClass"></asp:TextBox>
													<mc2:IMButton ID="btnApplyCouponCode" runat="server" style="width: 105px;" CausesValidation="false">
													</mc2:IMButton>
												</span>
												<div>
													<asp:Label ID="lbCouponResult" CssClass="FormFieldDescription" runat="server" Visible="false"></asp:Label>
												</div>
												</ContentTemplate>
											</asp:UpdatePanel>
										</td>
									</tr>
								</table>
							</td>
						</tr>
					</table>
					<table width="100%">
						<tr>
							<td style="padding: 0px; width: 250px;" valign="top">
								<table>
									<tr>
										<td>
											<cm:EcfListViewControl ID="MyListView" runat="server" ShowTopToolbar="true" LayoutResizeEnable="true"
												AutoFullHeight="false" AutocountHeaderBottom="true"></cm:EcfListViewControl>
										</td>
									</tr>
								</table>
							</td>
						</tr>
					</table>
					<table width="90%" id="addressContainerTable">
						<tr>
							<td>
								<table width="500px">
									<tr>
										<td style="padding: 3px;" valign="top" class="episerveroverwriteimage">
											<mc2:BlockHeaderLight HeaderCssClass="ibn-toolbar-light" ID="BlockHeaderLightBillingAddress"
												runat="server"></mc2:BlockHeaderLight>
											<table class="orderform-blockheaderlight-datatable">
												<tr>
													<td>
														<%--<asp:UpdatePanel runat="server" ID="UpdatePanel1" ChildrenAsTriggers="true">
															<ContentTemplate>--%>
																<cm:AddressEdit runat="server" ID="ctrlEditBilling" ListOnlyHandoffLocations="false" />
															<%--</ContentTemplate>
														</asp:UpdatePanel>--%>
													</td>
												</tr>
											</table>
										</td>
									</tr>
								</table>
							</td>
							<td valign="top">
								<table width="500px">
									<tr>
										<td style="padding: 3px;" valign="top" class="episerveroverwriteimage">
                                             <!-- Title is set in code behind -->
											<mc2:BlockHeaderLight HeaderCssClass="ibn-toolbar-light" ID="BlockHeaderLightShipmentHeader"
												runat="server"></mc2:BlockHeaderLight>
											<table class="orderform-blockheaderlight-datatable">
												<tr>
													<td>
														<asp:UpdatePanel runat="server" ID="upAddressEdit" ChildrenAsTriggers="true" UpdateMode="Conditional">
															<ContentTemplate>
																<cm:AddressEdit runat="server" ID="ctrlEdit" IsShippingAddress="true" />
															</ContentTemplate>
														</asp:UpdatePanel>
													</td>
												</tr>
											</table>
										</td>
									</tr>
								</table>
							</td>
						</tr>
					</table>
                    <table width="90%">
					    <tr>
					        <td>
					            <table width="500px">
						            <tr>
							            <td style="padding: 3px;" valign="top" class="episerveroverwriteimage">
								            <mc2:BlockHeaderLight HeaderCssClass="ibn-toolbar-light" ID="BlockHeaderLightShipment"
									            runat="server" Title="{SharedStrings:Shipment}"></mc2:BlockHeaderLight>
								            <table class="orderform-blockheaderlight-datatable">
                                                <tr>
                                                    <td class="orderform-label orderform-label-normal">
                                                        <asp:Label ID="lblShippingMethod" runat="server" Text="<%$ Resources:SharedStrings, Shipping_Method %>"></asp:Label>:
                                                    </td>
                                                    <td class="orderform-field">
                                                        <asp:UpdatePanel runat="server" ID="ShippingMethodsPanel" ChildrenAsTriggers="False" UpdateMode="Conditional" EnableViewState="True" RenderMode="Inline">
                                                            <ContentTemplate>
                                                                <asp:DropDownList runat="server" ID="ddlShippingMethod" Width="200" DataValueField="ShippingMethodId"
                                                                    DataTextField="DisplayName" OnSelectedIndexChanged="ddlShippingMethod_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                                <asp:RequiredFieldValidator ID="ShippingMethodRequiredValidator" runat="server" ControlToValidate="ddlShippingMethod"
                                                                    Display="Dynamic" ValidationGroup="OrderCreateVG">*</asp:RequiredFieldValidator>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
									            
									            <tr>
										            <td colspan="2" style="text-align: right;">
											            <asp:UpdatePanel UpdateMode="Conditional" ChildrenAsTriggers="false" ID="upShippingInfo"
												            runat="server" RenderMode="Inline">
												            <ContentTemplate>
													            <mc2:IMButton ID="btnGetTotals" runat="server" style="width: 105px;" CausesValidation="false">
													            </mc2:IMButton>
												            </ContentTemplate>
											            </asp:UpdatePanel>
										            </td>
									            </tr>
								            </table>
							            </td>
						            </tr>
						            <tr>
							            <td style="padding: 3px; width: 250px;" valign="top" class="episerveroverwriteimage">
								            <mc2:BlockHeaderLight HeaderCssClass="ibn-toolbar-light" ID="BlockHeaderLightTotals"
									            runat="server" Title="Shipment"></mc2:BlockHeaderLight>
								            <asp:UpdatePanel runat="server" ID="OrderSummaryPanel" ChildrenAsTriggers="false"
									            UpdateMode="Conditional" EnableViewState="true" RenderMode="Inline">
									            <ContentTemplate>
										            <table class="orderform-blockheaderlight-datatable">
											            <tr>
												            <td style="width: 170px;" class="orderform-label">
													            <asp:Label ID="Label4" runat="server" Text="<%$ Resources:OrderStrings, Item_Subtotal %>"></asp:Label>:
												            </td>
												            <td class="orderform-field">
													            <asp:Label ID="lblItemsSubTotal" runat="server"></asp:Label>
												            </td>
											            </tr>
                                                        <tr>
												            <td class="orderform-label orderform-label-normal">
													            <asp:Label ID="Label14" runat="server" Text="<%$ Resources:OrderStrings, Order_Level_Discounts %>"></asp:Label>:
												            </td>
												            <td class="orderform-field">
													            <asp:Label ID="lblOrderLevelDiscount" runat="server"></asp:Label>
												            </td>
											            </tr>
											            <tr>
												            <td class="orderform-label orderform-label-normal">
													            <asp:Label ID="Label13" runat="server" Text="<%$ Resources:OrderStrings, Order_Subtotal %>"></asp:Label>:
												            </td>
												            <td class="orderform-field">
													            <asp:Label ID="lblOrderSubTotal" runat="server"></asp:Label>
												            </td>
											            </tr>
											            <tr>
												            <td class="orderform-label orderform-label-normal">
													            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:OrderStrings, Shipping_Cost %>"></asp:Label>:
												            </td>
												            <td class="orderform-field">
													            <asp:Label ID="lblShipCost" runat="server"></asp:Label>
												            </td>
											            </tr>
											            <tr>
												            <td class="orderform-label label-divider-red orderform-label-normal">
													            <asp:Label ID="Label2" runat="server" Text="<%$ Resources:OrderStrings, Less_Shipment_Discount %>"></asp:Label>:
												            </td>
												            <td class="orderform-field">
													            <asp:Label ID="lblShipDiscount" runat="server"></asp:Label>
												            </td>
											            </tr>
                                                        <tr>
												            <td class="orderform-label label-divider orderform-label-normal">
													            <asp:Label ID="Label12" runat="server" Text="<%$ Resources:SharedStrings, Handling_Total %>"></asp:Label>:
												            </td>
												            <td class="orderform-field label-divider">
													            <asp:Label ID="lblHandlingCost" runat="server"></asp:Label>
												            </td>
											            </tr>
											            <tr>
												            <td class="orderform-label">
													            <asp:Label ID="Label3" runat="server" Text="<%$ Resources:OrderStrings, Total_Before_Tax %>"></asp:Label>:
												            </td>
												            <td class="orderform-field">
													            <asp:Label ID="lblTotalBeforeTax" runat="server"></asp:Label>
												            </td>
											            </tr>
											            <tr>
												            <td class="orderform-label orderform-label-normal">
													            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:OrderStrings, Item_Taxes %>"></asp:Label>:
												            </td>
												            <td class="orderform-field">
													            <asp:Label ID="lblItemTaxes" runat="server"></asp:Label>
												            </td>
											            </tr>
											            <tr>
												            <td class="orderform-label">
													            <asp:Label ID="Label7" runat="server" Text="<%$ Resources:OrderStrings, Order_Total %>"></asp:Label>:
												            </td>
												            <td class="orderform-field">
													            <asp:Label ID="lblShipTotal" runat="server"></asp:Label>
												            </td>
											            </tr>
										            </table>
									            </ContentTemplate>
								            </asp:UpdatePanel>
							            </td>
						            </tr>
					            </table>
					            <!-- end additional info -->
                            </td>
					        <td>
					            <table width="500px" id="orderMetaData">
					                 <tr>
							            <td style="padding: 3px;" valign="top" class="episerveroverwriteimage">
								            <mc2:BlockHeaderLight HeaderCssClass="ibn-toolbar-light" ID="BlockHeaderLightAttributes"
									            runat="server" Title="Shipment"></mc2:BlockHeaderLight>
									        <table class="orderform-blockheaderlight-datatable">
									            <tr>
										            <td>
									                    <ecf:MetaData ValidationGroup="OrderCreateVG" runat="server" ID="MetaDataTab" />
									                </td>
									           </tr>
									       </table>    
									    </td>
									</tr>
					            </table>
					        </td>
					    </tr>
					</table>
					<table width="100%">
						<tr>
							<td class="popup-buttons" colspan="2">
								<asp:UpdatePanel UpdateMode="Conditional" ChildrenAsTriggers="false" ID="upButtons"
									runat="server" RenderMode="Inline">
									<ContentTemplate>
										<mc2:IMButton ID="btnSave" runat="server" class="btn-save" OnServerClick="btnSave_ServerClick"
											ValidationGroup="OrderCreateVG" Text="<%$ Resources:Common, btnOK %>">
										</mc2:IMButton>
										&nbsp;
										<mc2:IMButton ID="btnCancel" runat="server" class="btn-save" CausesValidation="false"
											OnServerClick="btnCancel_ServerClick" Text="<%$ Resources:Common, btnCancel %>">
										</mc2:IMButton>
									</ContentTemplate>
								</asp:UpdatePanel>
							</td>
						</tr>
					</table>
				</ContentTemplate>
			</asp:UpdatePanel>
		</asp:Panel>
	</div>
</div>
<style>
	html, form, body
	{
		overflow: hidden !important;
	}
</style>
<script type="text/javascript">
    function updateTableBackground() {
        // Change the left corner, right corner and linehz image
        $('td.episerveroverwriteimage > table:first-child td:first-child img').attr('src', '../../Shell/EPi/Shell/Resources/leftCorner.GIF');
        $('td.episerveroverwriteimage > table:first-child td:last-child img').attr('src', '../../Shell/EPi/Shell/Resources/rightCorner.GIF');
        $('td.episerveroverwriteimage > table:first-child td[background]').attr('background', '../../Shell/EPi/Shell/Resources/linehz.GIF');
    }
    $(document).ready(function () {
        updateTableBackground();

        // after post-back event, to update the table background images.
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            updateTableBackground();
        });
    });
</script>

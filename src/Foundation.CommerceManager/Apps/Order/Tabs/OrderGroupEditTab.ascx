<%@ Control Language="C#" AutoEventWireup="true" Inherits="Mediachase.Commerce.Manager.Order.Tabs.OrderGroupEditTab" CodeBehind="OrderGroupEditTab.ascx.cs" %>
<%@ Register Src="~/Apps/Core/MetaData/EditTab.ascx" TagName="MetaData" TagPrefix="ecf" %>
<script type="text/javascript">
    function ecf_UpdateOrderAddressDropDown() {
        var ctrl = $get('<%=OrderGroupTrigger.ClientID%>');
        ctrl.value = Date().toString();
        __doPostBack('<%= OrderGroupTrigger.UniqueID %>', '');
    }
</script>

<div id="DataForm">
    <table width="100%">
        <tr>
            <td valign="top">
                <table class="DataForm">
                    <tr>
                        <td class="FormLabelCell">
                            <asp:Label runat="server" Text="<%$ Resources:SharedStrings, Status %>"></asp:Label>:
                        </td>
                        <td class="FormFieldCell">
                            <asp:DropDownList runat="server" ID="OrderStatusList" DataMember="OrderStatus" DataTextField="Name"
                                DataValueField="Name">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="FormSpacerCell">
                        </td>
                    </tr>
                    <tr>
                        <td class="FormLabelCell">
                            <asp:Label ID="Label2" runat="server" Text="<%$ Resources:SharedStrings, Currency %>"></asp:Label>:
                        </td>
                        <td class="FormFieldCell">
                            <asp:DropDownList runat="server" ID="OrderCurrencyList" DataMember="Currency" DataTextField="Name"
                                DataValueField="CurrencyCode">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="FormSpacerCell">
                        </td>
                    </tr>
                    <tr>
                        <td class="FormLabelCell">
                            <asp:Label ID="Label3" runat="server" Text="<%$ Resources:SharedStrings, Customer %>"></asp:Label>:
                        </td>
                        <td class="FormFieldCell">
                            <ComponentArt:ComboBox ID="CustomerName" runat="server" RunningMode="Callback" DataTextField="Name"
                                DataValueField="PrincipalId" DropDownPageSize="10" Width="350" CssClass="comboBox"
                                HoverCssClass="comboBoxHover" FocusedCssClass="comboBoxHover" TextBoxCssClass="comboTextBox"
                                TextBoxHoverCssClass="comboBoxHover" DropDownCssClass="comboDropDown" ItemCssClass="comboItem"
                                ItemHoverCssClass="comboItemHover" SelectedItemCssClass="comboItemHover" 
                                DropHoverImageUrl="../../../Apps/Shell/Styles/images/combobox/drop_hover.gif"
                                DropImageUrl="../../../Apps/Shell/Styles/images/combobox/drop.gif" TextBoxClientTemplateId="itemTemplate">
                                <ClientTemplates>
                                    <ComponentArt:ClientTemplate ID="itemTemplate" runat="server">
                                        <a href="javascript:CSManagementClient.ChangeBafView('Contact', 'View','objectid=## DataItem.get_value() ##');">## DataItem.get_text() ##</a>
                                    </ComponentArt:ClientTemplate>
                                </ClientTemplates>
                            </ComponentArt:ComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="FormSpacerCell">
                        </td>
                    </tr>
                    <tr>
                        <td class="FormLabelCell">
                            <asp:Label ID="Label4" runat="server" Text="<%$ Resources:SharedStrings, Billing_Address %>"></asp:Label>:
                        </td>
                        <td class="FormFieldCell">
                            <asp:UpdatePanel UpdateMode="Conditional" ID="OrderGroupContentPanel" runat="server" RenderMode="Inline">
                                <ContentTemplate>
                                    <asp:DropDownList runat="server" ID="AddressesList" DataTextField="Name" DataValueField="Name" Width="350px">
                                        <asp:ListItem Value="" Text="<%$ Resources:SharedStrings, Order_Select_Billing_Address %>"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:HiddenField runat="server" id="OrderGroupTrigger"/>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="FormSpacerCell">
                        </td>
                    </tr>
                    <tr>
                        <td class="FormLabelCell">
                            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:SharedStrings, Affiliate %>"></asp:Label>:
                        </td>
                        <td class="FormFieldCell">
                            <asp:DropDownList runat="server" ID="AffiliateList" DataTextField="FriendlyName"
                                DataValueField="Id">
                                <asp:ListItem Value="" Text="<%$ Resources:SharedStrings, select_affiliate %>"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <ecf:MetaData ValidationGroup="OrderGroupMetaData" runat="server" ID="MetaDataTab" />                    
                </table>
            </td>
            <td valign="top">           
                <asp:UpdatePanel runat="server" ID="OrderSummaryPanel" ChildrenAsTriggers="true" UpdateMode="Conditional" EnableViewState="true" RenderMode="Inline">
	                <ContentTemplate>
                        <table class="DataForm" width="300">
                            <tr>
                                <td class="FormLabelCell" colspan="2">
                                    <b><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:SharedStrings, Order_Summary %>" />:</b>
                                </td>
                            </tr>
                            <tr>
                                <td class="FormLabelCell">
                                    <asp:Label ID="Label7" runat="server" Text="<%$ Resources:SharedStrings, Sub_Total %>"></asp:Label>:
                                </td>
                                <td class="FormFieldCell">
                                    <asp:Label ID="OrderSubTotal" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" class="FormSpacerCell">
                                </td>
                            </tr>
                            <tr>
                                <td class="FormLabelCell">
                                    <asp:Label ID="Label8" runat="server" Text="<%$ Resources:SharedStrings, Tax_Total %>"></asp:Label>:
                                </td>
                                <td class="FormFieldCell">
                                    <asp:Label ID="OrderTaxTotal" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" class="FormSpacerCell">
                                </td>
                            </tr>
                            <tr>
                                <td class="FormLabelCell">
                                    <asp:Label ID="Label10" runat="server" Text="<%$ Resources:SharedStrings, Shipping_Total %>"></asp:Label>:
                                </td>
                                <td class="FormFieldCell">
                                    <asp:Label ID="OrderShippingTotal" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" class="FormSpacerCell">
                                </td>
                            </tr>
                            <tr>
                                <td class="FormLabelCell">
                                    <asp:Label ID="Label11" runat="server" Text="<%$ Resources:SharedStrings, Handling_Total %>"></asp:Label>:
                                </td>
                                <td class="FormFieldCell">
                                    <asp:Label ID="OrderHandlingTotal" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" class="FormSpacerCell">
                                </td>
                            </tr>
                            <tr>
                                <td class="FormLabelCell">
                                    <asp:Label ID="Label13" runat="server" Text="<%$ Resources:SharedStrings, Discount_Total %>"></asp:Label>:
                                </td>
                                <td class="FormFieldCell">
                                    <asp:Label ID="DiscountTotal" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" class="FormSpacerCell">
                                </td>
                            </tr>
                            <tr>
                                <td class="FormLabelCell">
                                    <asp:Label ID="Label12" runat="server" Text="<%$ Resources:SharedStrings, TOTAL_AllCaps %>"></asp:Label>:
                                </td>
                                <td class="FormFieldCell">
                                    <asp:Label ID="OrderTotal" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
</div>
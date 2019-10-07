<%@ Control Language="C#" AutoEventWireup="true" Inherits="Mediachase.Commerce.Manager.Order.Tabs.LineItemEditPopup" Codebehind="LineItemEditPopup.ascx.cs" %>
<%@ Register Src="~/Apps/Core/MetaData/EditTab.ascx" TagName="MetaData" TagPrefix="ecf" %>
<script type="text/javascript">
    function ecf_UpdateLineItemAddressesDialogControl(val) {
        var btn = $get('<%= SaveChangesButton.ClientID %>');
        if (btn != null)
            btn.disabled = false;
    
        var ctrl = $get('<%=DialogTrigger.ClientID%>');
        ctrl.value = val;
        __doPostBack('<%=DialogTrigger.UniqueID %>','');
    }
    
//    function OrderLineItemSaveChangesButton_onClientClick(btn) {
//        if (Page_ClientValidate('LineItemDetails')) {
//            btn.disabled = true;
//            alert('OrderLineItemSaveChangesButton_onClientClick');
//            __doPostBack('<%= SaveChangesButton.UniqueID %>', 'lineItemChanged');
//        }
//    }
</script>

<asp:HiddenField runat="server" id="DialogTrigger"/>

<asp:UpdatePanel UpdateMode="Conditional" ID="DialogContentPanel" runat="server" RenderMode="Inline">
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
                    <table class="DataForm">
                        <tr>
                            <td class="FormLabelCell">
                                <asp:Label ID="Label7" runat="server" text="<%$ Resources:SharedStrings, Code %>"></asp:Label>:
                            </td>
                            <td class="FormFieldCell">
                                <asp:Label ID="CodeLabel" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabelCell">
                                <asp:Label ID="Label1" runat="server" text="<%$ Resources:SharedStrings, Display_Name %>"></asp:Label>:
                            </td>
                            <td class="FormFieldCell">
                                <asp:TextBox runat="server" ID="DisplayName" Width="300px"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator4" ValidationGroup="LineItemDetails" ControlToValidate="DisplayName" Display="Dynamic" ErrorMessage="*" />
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabelCell">
                                <asp:Label ID="Label3" runat="server" text="<%$ Resources:SharedStrings, List_Price %>"></asp:Label>:
                            </td>
                            <td class="FormFieldCell">
                                <asp:TextBox runat="server" ID="ListPrice"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ValidationGroup="LineItemDetails" ControlToValidate="ListPrice" Display="Dynamic" ErrorMessage="*" />
                                <asp:RangeValidator ID="RangeValidator2" runat="server" ValidationGroup="LineItemDetails" ControlToValidate="ListPrice" Display="Dynamic" ErrorMessage="*" Type="Currency" MinimumValue="0" MaximumValue="1000000000"></asp:RangeValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabelCell">
                                <asp:Label ID="Label5" runat="server" text="<%$ Resources:SharedStrings, Discount %>"></asp:Label>:
                            </td>
                            <td class="FormFieldCell">
                                <asp:TextBox runat="server" ID="DiscountAmount"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ValidationGroup="LineItemDetails" ControlToValidate="DiscountAmount" Display="Dynamic" ErrorMessage="*" />
                                <asp:RangeValidator ID="RangeValidator1" runat="server" ValidationGroup="LineItemDetails" ControlToValidate="DiscountAmount" Display="Dynamic" ErrorMessage="*" Type="Currency" MinimumValue="0" MaximumValue="1000000000"></asp:RangeValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabelCell">
                                <asp:Label ID="Label2" runat="server" text="<%$ Resources:SharedStrings, Quantity %>"></asp:Label>:
                            </td>
                            <td class="FormFieldCell">
                                <asp:TextBox runat="server" ID="Quantity"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ValidationGroup="LineItemDetails" ControlToValidate="Quantity" Display="Dynamic" ErrorMessage="*" />
                                <asp:RangeValidator ID="RangeValidator3" runat="server" ValidationGroup="LineItemDetails" ControlToValidate="Quantity" Display="Dynamic" ErrorMessage="*" Type="Double" MinimumValue="0" MaximumValue="10000"></asp:RangeValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabelCell">
                                <asp:Label ID="Label4" runat="server" text="<%$ Resources:SharedStrings, Shipping_Address %>"></asp:Label>:
                            </td>
                           <td class="FormFieldCell">
                                <asp:Label ID="lblShippingAddress" runat="server"></asp:Label>
                           </td>
                        </tr>
                        <tr>
                            <td class="FormLabelCell">
                                <asp:Label ID="Label6" runat="server" text="<%$ Resources:SharedStrings, Shipping_Method %>"></asp:Label>:
                            </td>
                            <td class="FormFieldCell">
                                <asp:Label ID="lblShippingMethod" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <asp:PlaceHolder runat="server" ID="MetaPlaceHolder"></asp:PlaceHolder>
                        <ecf:MetaData ValidationGroup="LineItemDetails" runat="server" ID="MetaDataTab" />
                     </table>
                     <!-- /Content Area -->
                </td>
            </tr>
            <tr>
                <td colspan="2" style="background-image: url(../../../Apps/Shell/Styles/images/dialog/bottom_content.gif); height: 41px; padding-right: 10px;" align="right">
                    <asp:Button runat="server" ID="SaveChangesButton" ValidationGroup="LineItemDetails" text="<%$ Resources:SharedStrings, Save_Changes %>" />
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
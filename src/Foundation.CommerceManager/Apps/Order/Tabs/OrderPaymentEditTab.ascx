<%@ Control Language="C#" AutoEventWireup="true" Inherits="Mediachase.Commerce.Manager.Order.Tabs.OrderPaymentEditTab"
    CodeBehind="OrderPaymentEditTab.ascx.cs" %>
<%@ Register Src="~/Apps/Core/MetaData/EditTab.ascx" TagName="MetaData" TagPrefix="ecf" %>

<script type="text/javascript">
    function ecf_UpdatePaymentDialogControl(val) {
        var btn = $get('<%= SaveChangesButton.ClientID %>');
        if (btn != null)
            btn.disabled = false;

        var ctrl = $get('<%=DialogTrigger.ClientID%>');
        ctrl.value = val;
        __doPostBack('<%=DialogTrigger.UniqueID %>','');
    }
    
    function OrderPayment_UpdateSelectedField(ddObj, objToUpdate)
    {
        if(ddObj != null && objToUpdate != null)
            objToUpdate.value = ddObj.value;
    }

    function OrderPaymentSaveChangesButton_onClientClick(btn) {
        if (Page_ClientValidate('PaymentMetaData')) {
            btn.disabled = true;
            __doPostBack('<%= SaveChangesButton.UniqueID %>', 'paymentChanged');
        }
    }
</script>

<asp:HiddenField runat="server" ID="DialogTrigger" />
<asp:UpdatePanel UpdateMode="Conditional" ID="DialogContentPanel" runat="server"
    RenderMode="Block">
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
                    <asp:HiddenField runat="server" ID="SelectedPaymentStatusField" />
                    <asp:HiddenField runat="server" ID="SelectedPaymentMethodField" />
                    <asp:HiddenField runat="server" ID="SelectedPaymentTypeField" />
                    <table class="DataForm">
                        <tr>
                            <td class="FormLabelCell">
                                <asp:Label ID="Label5" runat="server" Text="<%$ Resources:OrderStrings, Payment_Type %>"></asp:Label>:
                            </td>
                            <td class="FormFieldCell">
                                <asp:DropDownList runat="server" ID="PaymentType" DataMember="PaymentMethod" DataTextField="Name" DataValueField="PaymentMethodId" AutoPostBack="true">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ValidationGroup="PaymentMetaData" ControlToValidate="PaymentType" Display="Dynamic" ErrorMessage="*" />
                                <br />
                                <asp:Label CssClass="FormFieldDescription" runat="server" Text="<%$ Resources:OrderStrings, Payment_Attributes_Description %>"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2"></td>
                        </tr>
                        <tr>
                            <td class="FormLabelCell">
                                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:OrderStrings, Payment_Method %>"></asp:Label>:
                            </td>
                            <td class="FormFieldCell">
                                <asp:DropDownList runat="server" ID="PaymentMethodList" DataMember="PaymentMethod" DataTextField="Name" DataValueField="PaymentMethodId"></asp:DropDownList>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ValidationGroup="PaymentMetaData" ControlToValidate="PaymentMethodList" Display="Dynamic" ErrorMessage="*" />
                                <br />
                                <asp:Label ID="Label6" CssClass="FormFieldDescription" runat="server" Text="<%$ Resources:SharedStrings, Payment_Method_Description %>"></asp:Label>                                
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2"></td>
                        </tr>
                        <tr>
                            <td class="FormLabelCell">
                                <asp:Label ID="Label2" runat="server" Text="<%$ Resources:SharedStrings, Name %>"></asp:Label>:
                            </td>
                            <td class="FormFieldCell">
                                <asp:TextBox runat="server" ID="Name"></asp:TextBox>
                                <br />
                                <asp:Label ID="Label7" CssClass="FormFieldDescription" runat="server" Text="<%$ Resources:OrderStrings, Payment_Name_Description %>"></asp:Label>                                                                
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2"></td>
                        </tr>
                        <tr>
                            <td class="FormLabelCell">
                                <asp:Label ID="Label3" runat="server" Text="<%$ Resources:SharedStrings, Amount %>"></asp:Label>:
                            </td>
                            <td class="FormFieldCell">
                                <asp:TextBox runat="server" ID="Amount"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator7" ValidationGroup="PaymentMetaData"
                                    ControlToValidate="Amount" Display="Dynamic" ErrorMessage="*" />
                                <asp:RangeValidator ID="RangeValidatorAmount" runat="server" ValidationGroup="PaymentMetaData"
                                    ControlToValidate="Amount" Display="Dynamic" ErrorMessage="*" Type="Currency"
                                    MinimumValue="0" MaximumValue="1000000000"></asp:RangeValidator>
                                <br />
                                <asp:Label ID="Label8" CssClass="FormFieldDescription" runat="server" Text="<%$ Resources:OrderStrings, Payment_For_Description %>"></asp:Label>                                                                    
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2"></td>
                        </tr>
                        <tr>
                            <td class="FormLabelCell">
                                <asp:Label ID="Label4" runat="server" Text="<%$ Resources:SharedStrings, Status %>"></asp:Label>:
                            </td>
                            <td class="FormFieldCell">
                                <asp:DropDownList runat="server" ID="PaymentStatus">
                                    <asp:ListItem Value="Pending" Text="<%$ Resources:SharedStrings, Pending %>"></asp:ListItem>
                                    <asp:ListItem Value="Failed" Text="<%$ Resources:SharedStrings, Failed %>"></asp:ListItem>
                                    <asp:ListItem Value="Processed" Text="<%$ Resources:SharedStrings, Processed %>"></asp:ListItem>
                                </asp:DropDownList>
                                <br />
                                <asp:Label ID="Label9" CssClass="FormFieldDescription" runat="server" Text="<%$ Resources:OrderStrings, Payment_Status_Description %>"></asp:Label>                                
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2"></td>
                        </tr>
                        <asp:PlaceHolder runat="server" ID="MetaPlaceHolder"></asp:PlaceHolder>
                        <ecf:MetaData ValidationGroup="PaymentMetaData" runat="server" ID="MetaDataTab" />
                    </table>
                    <!-- /Content Area -->
                </td>
            </tr>
            <tr>
                <td colspan="2" style="background-image: url(../../../Apps/Shell/Styles/images/dialog/bottom_content.gif);
                    height: 41px; padding-right: 10px;" align="right">
                    <asp:Button runat="server" ID="SaveChangesButton" ValidationGroup="PaymentMetaData" 
                        OnClientClick="OrderPaymentSaveChangesButton_onClientClick(this);return false;" 
                        Text="<%$ Resources:SharedStrings, Save_Changes %>" />
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>

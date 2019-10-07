<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TaxValueEditPopup.ascx.cs" Inherits="Mediachase.Commerce.Manager.Order.Tabs.TaxValueEditPopup" %>
<%@ Import Namespace="Mediachase.Commerce.Shared" %>
<%@ Register Src="~/Apps/Core/Controls/CalendarDatePicker.ascx" TagName="CalendarDatePicker" TagPrefix="ecf" %>
<script type="text/javascript">
    function ecf_UpdateTaxValueDialogControl(val)
    {
        var ctrl = $get('<%=DialogTrigger.ClientID%>');
        ctrl.value = val;
        __doPostBack('<%=DialogTrigger.UniqueID %>','');
    }
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
                    <asp:HiddenField runat="server" id="SelectedTaxField"/>
                    <table class="DataForm">
                        <tr>
                            <td class="FormLabelCell">
                                <asp:Label ID="lblTaxCategory" runat="server" text="<%$ Resources:SharedStrings, Tax_Category %>"></asp:Label>:
                            </td>
                            <td class="FormFieldCell">
                                <asp:DropDownList runat="server" id="TaxCategoriesList">
                                    <asp:ListItem Value="" text="<%$ Resources:SharedStrings, select_tax_category %>"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ValidationGroup="TaxValueValidationGroup" runat="server" ID="rfvTaxCategoriesList" ControlToValidate="TaxCategoriesList" ErrorMessage="<%$ Resources:SharedStrings, Tax_Category_Required %>" Display="Dynamic"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>  
                            <td class="FormLabelCell"><asp:Label ID="Label4" runat="server" text="<%$ Resources:OrderStrings, Jurisdiction_Group %>"></asp:Label>:</td> 
                            <td class="FormFieldCell">
                                <asp:DropDownList runat="server" id="JurisdictionGroupsList">
                                    <asp:ListItem Value="" text="<%$ Resources:OrderStrings, select_jurisdiction_group %>"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ValidationGroup="TaxValueValidationGroup" runat="server" ID="rfvJurisdictionGroupsList" ControlToValidate="JurisdictionGroupsList" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabelCell">
                                <asp:Label ID="Label7" runat="server" text="<%$ Resources:SharedStrings, Rate %>"></asp:Label>:
                            </td>
                            <td class="FormFieldCell">
                                <asp:TextBox runat="server" ID="tbRate"></asp:TextBox>
                                <asp:RequiredFieldValidator ValidationGroup="TaxValueValidationGroup" runat="server" ID="RequiredFieldValidator3" ControlToValidate="tbRate" Display="Dynamic" ErrorMessage="*" />
                                <asp:RangeValidator ValidationGroup="TaxValueValidationGroup" ID="RangeValidator2" runat="server" ControlToValidate="tbRate" Display="Dynamic" ErrorMessage="*" Type="Double" MinimumValue="0" MaximumValue="100"></asp:RangeValidator>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" class="FormSpacerCell"></td>
                        </tr>
                        <tr>
                            <td class="FormLabelCell">
                                <asp:Label ID="Label3" runat="server" text="<%$ Resources:SharedStrings, Effective_Date %>"></asp:Label>:
                            </td> 
                            <td class="FormFieldCell">
                                <ecf:CalendarDatePicker ValidationGroup="TaxValueValidationGroup" runat="server" ID="AffectiveDate" />
                            </td> 
                        </tr>
                     </table>
                     <!-- /Content Area -->
                </td>
            </tr>
            <tr>
                <td colspan="2" style="background-image: url(<%= CommerceHelper.GetAbsolutePath("~/Apps/Shell/Styles/images/dialog/bottom_content.gif") %>); height: 41px; padding-right: 10px;" align="right">
                    <asp:Button runat="server" ID="SaveChangesButton" ValidationGroup="TaxValueValidationGroup" OnClick="SaveChangesButton_Click" Text="<%$ Resources:SharedStrings, Save_Changes %>" />
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>

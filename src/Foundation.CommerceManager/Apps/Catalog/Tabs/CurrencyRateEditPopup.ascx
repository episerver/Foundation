<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CurrencyRateEditPopup.ascx.cs" Inherits="Mediachase.Commerce.Manager.Catalog.Tabs.CurrencyRateEditPopup" %>
<%@ Import Namespace="Mediachase.Commerce.Shared" %>
<%@ Register Src="~/Apps/Core/Controls/CalendarDatePicker.ascx" TagName="CalendarDatePicker" TagPrefix="ecf" %>
<script type="text/javascript">
	function ecf_UpdateCurrencyRateDialogControl(currencyRateId, currencyName, currencyCode)
    {
    	$get('<%=DialogTrigger.ClientID%>').value = currencyRateId;
        $get('<%=CurrencyName.ClientID%>').value = currencyName;
        $get('<%=CurrencyCode.ClientID%>').value = currencyCode;
        __doPostBack('<%=DialogTrigger.UniqueID %>', '');
    }
    
    function UpdateCurrency()
    {
        var list = $get('<%= ToCurrenciesList.ClientID %>');
        if(list != null)
        {
            var field = $get('<%= SelectedCurrencyField.ClientID %>');
            if(field != null)
                field.value = list.value;
        }
    }
    function CurrencyRateEditPopup_CloseDialog() {
        document.getElementById('<%=DialogTrigger.ClientID%>').value = "";
        CurrencyRateDialog.close();
        CurrencyRatesGrid.callback();
        CSManagementClient.MarkDirty();
    }
</script>

<asp:HiddenField runat="server" id="DialogTrigger"/>
<asp:HiddenField runat="server" ID="CurrencyName" />
<asp:HiddenField runat="server" ID="CurrencyCode" />

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
                    <asp:HiddenField runat="server" id="SelectedCurrencyField"/>
                    <table class="DataForm">
                        <tr>
                            <td class="FormLabelCell">
                                <asp:Label ID="Label8" runat="server" Text="<%$ Resources:CatalogStrings, Currency_From %>"></asp:Label>:
                            </td>
                            <td class="FormFieldCell">
                                <asp:Label ID="FromCurrencyLabel" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabelCell">
                                <asp:Label ID="Label7" runat="server" Text="<%$ Resources:CatalogStrings, Currency_To %>"></asp:Label>:
                            </td>
                            <td class="FormFieldCell">
                                <asp:Label ID="ToCurrencyLabel" runat="server"></asp:Label>
                                <asp:DropDownList runat="server" ID="ToCurrenciesList" AutoPostBack="false" onchange="javascript:UpdateCurrency();" DataTextField="Name" DataValueField="CurrencyId"></asp:DropDownList>
                                <asp:RequiredFieldValidator runat="server" ID="ToCurrencyRequiredValidator" ValidationGroup="CurrencyRateValidationGroup" ControlToValidate="ToCurrenciesList" Display="Dynamic" ErrorMessage="*" />
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabelCell">
                                <asp:Label ID="Label3" runat="server" Text="<%$ Resources:CatalogStrings, Currency_End_Of_Day_Rate %>"></asp:Label>:
                            </td>
                            <td class="FormFieldCell">
                                <asp:TextBox runat="server" ID="tbRate"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ValidationGroup="CurrencyRateValidationGroup" ControlToValidate="tbRate" Display="Dynamic" ErrorMessage="*" />
                                <asp:RangeValidator ID="RangeValidator2" runat="server" ValidationGroup="CurrencyRateValidationGroup" ControlToValidate="tbRate" Display="Dynamic" ErrorMessage="*" Type="Double" MinimumValue="0" MaximumValue="9000000"></asp:RangeValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabelCell">
                                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:CatalogStrings, Currency_Average_Rate %>"></asp:Label>:
                            </td>
                            <td class="FormFieldCell">
                                <asp:TextBox runat="server" ID="tbAverageRate"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator6" ValidationGroup="CurrencyRateValidationGroup" ControlToValidate="tbAverageRate" Display="Dynamic" ErrorMessage="*" />
                                <asp:RangeValidator ID="RangeValidator1" runat="server" ValidationGroup="CurrencyRateValidationGroup" ControlToValidate="tbAverageRate" Display="Dynamic" ErrorMessage="*" Type="Double" MinimumValue="0" MaximumValue="9000000"></asp:RangeValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabelCell">
                                <asp:Label ID="Label5" runat="server" Text="<%$ Resources:CatalogStrings, Currency_Rate_Date %>"></asp:Label>:
                            </td>
                            <td class="FormFieldCell">
                                <ecf:CalendarDatePicker runat="server" ID="CurrencyRateDate" ValidationGroup="CurrencyRateValidationGroup" />
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ValidationGroup="CurrencyRateValidationGroup" ControlToValidate="CurrencyRateDate" Display="Dynamic" ErrorMessage="*" />
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabelCell">
                                <asp:Label ID="Label2" runat="server" Text="<%$ Resources:SharedStrings, Modified %>"></asp:Label>:
                            </td>
                            <td class="FormFieldCell">
                                <asp:Label runat="server" ID="ModificationDate"></asp:Label>
                            </td>
                        </tr>
                     </table>
                     <!-- /Content Area -->
                </td>
            </tr>
            <tr>
                <td style="background-image: url('<%= this.ResolveUrl("~/Apps/Shell/Styles/images/dialog/bottom_content.gif") %>'); height: 41px; padding-right: 10px;" align="right">
                    <asp:Button runat="server" ID="SaveChangesButton" ValidationGroup="CurrencyRateValidationGroup" OnClick="SaveChangesButton_Click" Text="<%$ Resources:SharedStrings, Save_Changes %>" />
                </td>
                <td style="background-image: url('<%= this.ResolveUrl("~/Apps/Shell/Styles/images/dialog/bottom_content.gif") %>');
					height: 41px; padding-right: 10px;" align="right">
					<asp:Button runat="server" ID="CancelChangesButton" causesvalidation="false"
						OnClientClick="CurrencyRateEditPopup_CloseDialog()" Text="<%$ Resources:CatalogStrings, Entry_Cancel_Changes %>" />
				</td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>

<%@ Control Language="C#" AutoEventWireup="true" Inherits="Mediachase.Commerce.Manager.Catalog.Tabs.CatalogOverviewEditTab"
    CodeBehind="CatalogOverviewEditTab.ascx.cs" %>
<%@ Import Namespace="Mediachase.Commerce.Shared" %>
<%@ Register Src="~/Apps/Core/Controls/BooleanEditControl.ascx" TagName="BooleanEditControl"
    TagPrefix="ecf" %>
<%@ Register Src="~/Apps/Core/Controls/CalendarDatePicker.ascx" TagName="CalendarDatePicker"
    TagPrefix="ecf" %>
<%@ Reference Control="~/Apps/MetaUIEntity/Modules/EntityDropDown.ascx" %>
<%@ Register Src="~/Apps/MetaUIEntity/Modules/EntityDropDown.ascx" TagName="EntityDD"
    TagPrefix="mc" %>
<script type="text/javascript" src='<%=CommerceHelper.GetAbsolutePath("~/Apps/MetaDataBase/Scripts/main.js") %>'></script>
<style type="text/css">
    .ajax__validatorcallout_popup_table {
        top: 100px;
        left: 380px;
        border: none 0px;
        background-color: transparent;
        padding: 0px;
        margin: 0px;
    }

    .ajax__validatorcallout_popup_table1 {
        top: 230px;
        left: 260px;
        border: none 0px;
        background-color: transparent;
        padding: 0px;
        margin: 0px;
    }

    .ajax__validatorcallout_popup_table2 {
        top: 260px;
        left: 260px;
        border: none 0px;
        background-color: transparent;
        padding: 0px;
        margin: 0px;
    }
</style>
<script type="text/javascript">


    function extendToolkitHidePopupTimer() {
        if (AjaxControlToolkit && AjaxControlToolkit.PopupBehavior) {
            extendToolkitHidePopup();
        }
        else {
            window.setTimeout(extendToolkitHidePopup, 500);
        }
    }

    function extendToolkitHidePopup() {
        AjaxControlToolkit.PopupBehavior.prototype.hide = function() {
            var eventArgs = new Sys.CancelEventArgs();
            this.raiseHiding(eventArgs);
            if (eventArgs.get_cancel()) {
                return;
            }

            // Either hide the popup or play an animation that does
            this._visible = false;
            if (this._onHide) {
                this.onHide();
            } else {
                this._hidePopup();
                this._hideCleanup();
            }
        }
    }

    extendToolkitHidePopupTimer();

</script>
<div id="DataForm">
    <table class="DataForm">
        <tr>
            <td class="FormLabelCell">
                <asp:Label runat="server" Text="<%$ Resources:CatalogStrings, Catalog_Catalog_Name %>"></asp:Label>:
            </td>
            <td class="FormFieldCell">
                <asp:TextBox runat="server" Width="250" ID="CatalogName" MaxLength="100"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ID="CatalogNameRequired" ControlToValidate="CatalogName"
                    Display="Dynamic" ErrorMessage="<%$ Resources:CatalogStrings, Catalog_Catalog_Name_Required %>" />
                <asp:CustomValidator runat="server" ID="NameUniqueCustomValidator" ControlToValidate="CatalogName"
                    OnServerValidate="NameCheck" Display="Dynamic" ErrorMessage="<%$ Resources:CatalogStrings, Catalog_With_Name_AlreadyExists %>" />
                <br />
                <asp:Label ID="Label9" CssClass="FormFieldDescription" runat="server" Text="<%$ Resources:CatalogStrings, Catalog_Name_Description %>"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="FormSpacerCell"></td>
        </tr>
        <tr runat="server" id="trSecurityScope">
            <td class="FormLabelCell" style="width: 100px">
                <asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:CatalogStrings, SecurityScope %>" />:
            </td>
            <td class="FormFieldCell" style="width: 250px;">
                <mc:EntityDD ID="refObjects" ItemCount="6" runat="server" Width="100%" />
            </td>
        </tr>
        <tr>
            <td colspan="2" class="FormSpacerCell"></td>
        </tr>
        <tr>
            <td class="FormLabelCell">
                <asp:Label runat="server" Text="<%$ Resources:SharedStrings, Available_From %>"></asp:Label>:
            </td>
            <td class="FormFieldCell">
                <ecf:CalendarDatePicker runat="server" ID="AvailableFrom" />
                <asp:CustomValidator runat="server" ID="AvailableFromValidator" ControlToValidate="AvailableFrom" Display="Dynamic"
                    OnServerValidate="AvailableFromValidator_Validate" ErrorMessage="<%$ Resources:CatalogStrings, Entry_AvailableFrom_Too_Late %>" />
            </td>
        </tr>
        <tr>
            <td colspan="2" class="FormSpacerCell"></td>
        </tr>
        <tr>
            <td class="FormLabelCell">
                <asp:Label runat="server" ID="Label6" Text="<%$ Resources:SharedStrings, Expires_On %>"></asp:Label>:
            </td>
            <td class="FormFieldCell">
                <ecf:CalendarDatePicker runat="server" ID="ExpiresOn" />
                <asp:CustomValidator runat="server" ID="ExpiresOnValidator" ControlToValidate="ExpiresOn" Display="Dynamic"
                    OnServerValidate="ExpiresOnValidator_Validate" ErrorMessage="<%$ Resources:CatalogStrings, Entry_ExpiresOn_Too_Early %>" />
            </td>
        </tr>
        <tr>
            <td colspan="2" class="FormSpacerCell"></td>
        </tr>
        <tr>
            <td class="FormLabelCell">
                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:CatalogStrings, Catalog_Default_Currency %>"></asp:Label>:
            </td>
            <td class="FormFieldCell">
                <asp:DropDownList runat="server" ID="DefaultCurrency">
                    <asp:ListItem Value="" Text="<%$ Resources:CatalogStrings, Catalog_Select_Primary_Currency %>"></asp:ListItem>
                </asp:DropDownList>
                <asp:RequiredFieldValidator runat="server" ID="DefaultCurrencyRequired" ControlToValidate="DefaultCurrency"
                    Display="Dynamic" ErrorMessage="<%$ Resources:CatalogStrings, Catalog_Default_Currency_Required %>"
                    InitialValue="" />
            </td>
        </tr>
        <tr>
            <td colspan="2" class="FormSpacerCell"></td>
        </tr>
        <tr>
            <td class="FormLabelCell">
                <asp:Label ID="Label2" runat="server" Text="<%$ Resources:CatalogStrings, Catalog_Default_Language %>"></asp:Label>:
            </td>
            <td class="FormFieldCell">
                <asp:DropDownList runat="server" ID="DefaultLanguage">
                    <asp:ListItem Value="" Text="<%$ Resources:CatalogStrings, Catalog_Select_Primary_Language %>"></asp:ListItem>
                </asp:DropDownList>
                <br />
                <asp:RequiredFieldValidator runat="server" ID="DefaultLanguageRequired" ControlToValidate="DefaultLanguage"
                    Display="Dynamic" ErrorMessage="<%$ Resources:CatalogStrings, Catalog_Default_Language_Required %>"
                    InitialValue="" />
            </td>
        </tr>
        <tr>
            <td colspan="2" class="FormSpacerCell"></td>
        </tr>
        <tr>
            <td class="FormLabelCell">
                <asp:Label ID="Label3" runat="server" Text="<%$ Resources:CatalogStrings, Catalog_Base_Weight %>"></asp:Label>:
            </td>
            <td class="FormFieldCell">
                <asp:DropDownList runat="server" ID="BaseWeight">
                    <asp:ListItem Value="lbs" Text="<%$ Resources:CatalogStrings, Catalog_In_Pounds %>"></asp:ListItem>
                    <asp:ListItem Value="kgs" Text="<%$ Resources:CatalogStrings, Catalog_In_Kilograms %>"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="FormSpacerCell"></td>
        </tr>
                <tr>
            <td class="FormLabelCell">
                <asp:Label ID="Label7" runat="server" Text="<%$ Resources:CatalogStrings, Catalog_Base_Length %>"></asp:Label>:
            </td>
            <td class="FormFieldCell">
                <asp:DropDownList runat="server" ID="BaseLength">
                    <asp:ListItem Value="" Text="-"></asp:ListItem>
                    <asp:ListItem Value="cm" Text="<%$ Resources:CatalogStrings, Catalog_In_Centimeters %>"></asp:ListItem>
                    <asp:ListItem Value="in" Text="<%$ Resources:CatalogStrings, Catalog_In_Inches %>"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="FormSpacerCell"></td>
        </tr>
        <tr>
            <td class="FormLabelCell">
                <asp:Label ID="Label4" runat="server" Text="<%$ Resources:CatalogStrings, Catalog_Other_Languages %>"></asp:Label>:
            </td>
            <td class="FormFieldCell">
                <asp:ListBox runat="server" ID="OtherLanguagesList" Width="250" SelectionMode="Multiple"
                    DataTextField="LanguageCode" DataValueField="LanguageCode"></asp:ListBox>
                <br />
                <asp:Label runat="server" ID="lblLanguageWarning" Text="<%$ Resources:CatalogStrings, Catalog_Language_Warning %>" />
            </td>
        </tr>
        <tr>
            <td colspan="2" class="FormSpacerCell"></td>
        </tr>
        <tr>
            <td class="FormLabelCell">
                <asp:Label ID="Label8" runat="server" Text="<%$ Resources:SharedStrings, Sort_Order %>"></asp:Label>:
            </td>
            <td class="FormFieldCell">
                <asp:TextBox runat="server" ID="SortOrder"></asp:TextBox>
                <br />
                <asp:RequiredFieldValidator runat="server" ID="SortOrderRequiredValidator" ControlToValidate="SortOrder" ErrorMessage="<%$ Resources:SharedStrings, Sort_Order_Required %>" Display="Dynamic"></asp:RequiredFieldValidator>
                <asp:RangeValidator runat="server" ID="SortOrderRangeValidator" ControlToValidate="SortOrder" MinimumValue="0" MaximumValue="1000000000" Type="Integer" ErrorMessage="<%$ Resources:SharedStrings, Sort_Order_Invalid %>" Display="Dynamic"></asp:RangeValidator>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="FormSpacerCell"></td>
        </tr>
        <tr>
            <td class="FormLabelCell">
                <asp:Label ID="Label5" runat="server" Text="<%$ Resources:SharedStrings, Available %>"></asp:Label>:
            </td>
            <td class="FormFieldCell">
                <ecf:BooleanEditControl ID="IsCatalogActive" runat="server" MDContext="<%# Mediachase.Commerce.Catalog.CatalogContext.MetaDataContext %>">
                </ecf:BooleanEditControl>
            </td>
        </tr>
    </table>
</div>

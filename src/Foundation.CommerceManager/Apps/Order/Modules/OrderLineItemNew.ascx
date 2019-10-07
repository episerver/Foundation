<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrderLineItemNew.ascx.cs" Inherits="Mediachase.Commerce.Manager.Apps.Order.Modules.OrderLineItemNew" %>
<%@ Register TagPrefix="mc2" Assembly="Mediachase.BusinessFoundation" Namespace="Mediachase.BusinessFoundation" %>
<script type="text/javascript">
    function Order_SelectedEntryChanged(listBox) {
        var entriesData = $get('<%= hfEntries.ClientID %>');
        try {
            var data = Sys.Serialization.JavaScriptSerializer.deserialize(entriesData.value);
            for (var i = 0; i < data.results.length; i++) {
                if (data.results[i].CatalogEntryId == listBox.value) {
                    // fill entry details form
                    var curEntry = data.results[i];
                    $get('<%= hfSelectedEntryId.ClientID %>').value = curEntry.CatalogEntryId;
                    $get('<%= hfSelectedEntryCode.ClientID %>').value = unescape(curEntry.Code);
                    $get('<%= EntryCodeLabel.ClientID %>').innerHTML = unescape(curEntry.Code);
                    $get('<%= DisplayName.ClientID %>').value = unescape(curEntry.Name);
                    $get('<%= ListPrice.ClientID %>').value = curEntry.FormattedPrice;
                    $get('<%= DiscountAmount.ClientID %>').value = curEntry.FormattedDiscount;
                    $get('<%= Quantity.ClientID %>').value = curEntry.FormattedQuantity;
                    $get('<%= btnSave.ClientID %>').disabled = false;
                }
            }
        }
        catch (e) {
            alert('A problem occured with parsing selected entry properties');
            return;
        }
    }
</script>

<asp:Label runat="server" ID="lblErrorInfo" Style="color: Red"></asp:Label>
<asp:Panel runat="server" ID="FormPanel" Visible="true" Height="100%" Width="100%">
<asp:UpdatePanel ID="upSearchButton" ChildrenAsTriggers="false" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
<table class="orderform-lineitem-add" cellspacing="2" cellpadding="0">
    <tr>
        <!-- BEGIN: left column -->
        <td class="half top">
            <!-- BEGIN: left column (search form) -->
            <asp:Panel runat="server" ID="pnlMain" DefaultButton="btnSearch" Height="190px" Width="100%">
                <table class="orderform-lineitem-add-search">
                    <tr>
                        <td>
                            <b><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:CatalogStrings, Catalog_Search_By_Keyword %>" />:</b>
                        </td>
                        <td>
                            <asp:TextBox ID="tbKeywords" Width="240" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:CatalogStrings, Catalog_Search_By_Code_Id %>" />:
                        </td>
                        <td>
                            <asp:TextBox ID="tbCode" Width="100" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:CatalogStrings, Catalog_Filter_By_Language %>" />:
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ListLanguages" Width="300" DataValueField="LanguageId" DataTextField="Name"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="Literal5" runat="server" Text="<%$ Resources:CatalogStrings, Catalog_Filter_By_Catalog %>" />:
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ListCatalogs" Width="300" DataValueField="CatalogId" DataTextField="Name"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="right">
                            <asp:Button ID="btnSearch" runat="server" Width="100" Text="<%$ Resources:SharedStrings, Search %>" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <!-- END: left column (search form) -->
            <hr class="orderform-lineitem-add" />
            <!-- BEGIN: left column (edit form) -->
            <asp:UpdatePanel ID="upSelectedEntry" ChildrenAsTriggers="false" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:HiddenField runat="server" ID="hfSelectedEntryId" />
                    <asp:HiddenField runat="server" ID="hfSelectedEntryCode" />
                    <asp:HiddenField runat="server" ID="hfEntries" />
                    <table>
                        <tr>
			                <td class="FormLabelCell">
				                <asp:Label ID="Label7" runat="server" Text="<%$ Resources:SharedStrings, ID %>"></asp:Label>:
			                </td>
			                <td class="FormFieldCell">
				                <asp:Label ID="EntryCodeLabel" runat="server"></asp:Label>
			                </td>
		                </tr>
		                <tr>
			                <td class="FormLabelCell">
				                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:SharedStrings, Display_Name %>"></asp:Label>:
			                </td>
			                <td class="FormFieldCell">
				                <asp:TextBox runat="server" ID="DisplayName" Width="300px"></asp:TextBox>
			                </td>
		                </tr>
		                <tr>
			                <td class="FormLabelCell">
				                <asp:Label ID="Label3" runat="server" Text="<%$ Resources:SharedStrings, List_Price %>"></asp:Label>:
			                </td>
			                <td class="FormFieldCell">
				                <asp:TextBox runat="server" ID="ListPrice"></asp:TextBox>
				                <asp:RequiredFieldValidator runat="server" ID="rfvListPrice" ValidationGroup="LineItemDetails"
					                ControlToValidate="ListPrice" Display="Dynamic" ErrorMessage="*" />
				                <asp:RangeValidator ID="rvListPrice" runat="server" ValidationGroup="LineItemDetails"
					                ControlToValidate="ListPrice" Display="Dynamic" ErrorMessage="*" Type="Currency"
					                MinimumValue="0" MaximumValue="1000000000"></asp:RangeValidator>
			                </td>
		                </tr>
		                <tr>
			                <td class="FormLabelCell">
				                <asp:Label ID="Label5" runat="server" Text="<%$ Resources:SharedStrings, Discount %>"></asp:Label>:
			                </td>
			                <td class="FormFieldCell">
				                <asp:TextBox runat="server" ID="DiscountAmount"></asp:TextBox>
				                <asp:RequiredFieldValidator runat="server" ID="rfvDiscountAmount" ValidationGroup="LineItemDetails"
					                ControlToValidate="DiscountAmount" Display="Dynamic" ErrorMessage="*" />
				                <asp:RangeValidator ID="rvDiscountAmount" runat="server" ValidationGroup="LineItemDetails"
					                ControlToValidate="DiscountAmount" Display="Dynamic" ErrorMessage="*" Type="Currency"
					                MinimumValue="0" MaximumValue="1000000000"></asp:RangeValidator>
			                </td>
		                </tr>
		                <tr>
			                <td class="FormLabelCell">
				                <asp:Label ID="Label2" runat="server" Text="<%$ Resources:SharedStrings, Quantity %>"></asp:Label>:
			                </td>
			                <td class="FormFieldCell">
				                <asp:TextBox runat="server" ID="Quantity"></asp:TextBox>
				                <asp:RequiredFieldValidator runat="server" ID="rfvQuantity" ValidationGroup="LineItemDetails"
					                ControlToValidate="Quantity" Display="Dynamic" ErrorMessage="*" />
				                <asp:RangeValidator ID="rvQuantity" runat="server" ValidationGroup="LineItemDetails"
					                ControlToValidate="Quantity" Display="Dynamic" ErrorMessage="*" Type="Double"
					                MinimumValue="1" MaximumValue="10000"></asp:RangeValidator>
			                </td>
		                </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
            <!-- END: left column (edit form) -->
        </td>
        <!-- END: left column -->
        <!-- BEGIN: right column -->
        <td class="top">
            <div class="pad5">
                <div runat="server" id="entriesGrid"></div>
                <asp:UpdatePanel ID="upSearchResult" ChildrenAsTriggers="false" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:ListBox runat="server" ID="EntriesToAdd" Rows="25" SelectionMode="Single" Width="100%"
                             DataValueField="CatalogEntryId" DataTextField="Name" AutoPostBack="false" Visible="true" onchange="javascript:Order_SelectedEntryChanged(this);return false;"></asp:ListBox>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </td>
        <!-- END: right column -->
    </tr>
    <tr>
        <td align="right" style="padding-top: 10px; padding-right: 10px;" class="half">
            <mc2:IMButton ID="btnSave" runat="server" style="width: 105px;" OnServerClick="btnSave_ServerClick" ValidationGroup="LineItemDetails">
            </mc2:IMButton>
            &nbsp;
            <mc2:IMButton ID="btnCancel" runat="server" style="width: 105px;" CausesValidation="false">
            </mc2:IMButton>            
        </td>
    </tr>
</table>
    </ContentTemplate>
</asp:UpdatePanel>
</asp:Panel>
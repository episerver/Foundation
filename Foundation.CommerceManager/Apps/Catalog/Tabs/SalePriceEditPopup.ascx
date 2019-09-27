<%@ Control Language="C#" AutoEventWireup="true" Inherits="Mediachase.Commerce.Manager.Catalog.Tabs.SalePriceEditPopup"	CodeBehind="SalePriceEditPopup.ascx.cs" %>
<%@ Import Namespace="Mediachase.Commerce.Shared" %>
<%@ Register Src="~/Apps/Core/MetaData/EditTab.ascx" TagName="MetaData" TagPrefix="ecf" %>
<%@ Register Src="~/Apps/Core/Controls/CalendarDatePicker.ascx" TagName="CalendarDatePicker"
	TagPrefix="ecf" %>

<script type="text/javascript">
    function SalePriceEditPopup_CloseDialog() {
        document.getElementById('<%=DialogTrigger.ClientID%>').value = "";
        SalePriceEditDialog.close();
        SalePricesGrid.callback();
        CSManagementClient.MarkDirty();
    }
	function ecf_UpdateSalePriceEditDialogControl(val)
	{
		var ctrl = $get('<%=DialogTrigger.ClientID%>');
		ctrl.value = val;
		__doPostBack('<%=DialogTrigger.UniqueID %>', '');
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
					<table class="DataForm">
						<tr>
							<td class="FormLabelCell">
								<asp:Label ID="Label4" runat="server" Text="<%$ Resources:CatalogStrings, Sale_Type %>"></asp:Label>:
							</td>
							<td class="FormFieldCell">
								<asp:DropDownList runat="server" ID="SaleTypeFilter" AutoPostBack="true" DataValueField="SaleTypeId" DataTextField="SaleTypeName">
								</asp:DropDownList>
							</td>
						</tr>
						<tr>
							<td class="FormLabelCell">
								<asp:Label ID="Label7" runat="server" Text="<%$ Resources:SharedStrings, Sale_Code %>"></asp:Label>:
							</td>
							<td class="FormFieldCell">
								<asp:TextBox runat="server" ID="SaleCode"></asp:TextBox>
								<asp:PlaceHolder runat="server" ID="SaleCodeControlPlaceHolder"></asp:PlaceHolder>
							</td>
						</tr>
						<tr>
							<td class="FormLabelCell">
								<asp:Label ID="Label3" runat="server" Text="<%$ Resources:CatalogStrings, Sale_Unit_Price %>"></asp:Label>:
							</td>
							<td class="FormFieldCell">
								<asp:TextBox runat="server" ID="UnitPrice"></asp:TextBox>
								<asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ValidationGroup="SalePriceDetails"
									ControlToValidate="UnitPrice" Display="Dynamic" ErrorMessage="*" />
								<asp:RangeValidator ID="RangeValidator2" runat="server" ValidationGroup="SalePriceDetails"
									ControlToValidate="UnitPrice" Display="Dynamic" ErrorMessage="*" Type="Currency"
									MinimumValue="0" MaximumValue="1000000000"></asp:RangeValidator>
							</td>
						</tr>
						<tr>
							<td class="FormLabelCell">
								<asp:Label ID="Label1" runat="server" Text="<%$ Resources:SharedStrings, Currency %>"></asp:Label>:
							</td>
							<td class="FormFieldCell">
								<asp:DropDownList runat="server" ID="CurrencyFilter" DataValueField="CurrencyCode" DataTextField="Name" AutoPostBack="true">
								</asp:DropDownList>
							</td>
						</tr>
						<tr>
							<td class="FormLabelCell">
								<asp:Label ID="Label2" runat="server" Text="<%$ Resources:CatalogStrings, Entry_Min_Quantity %>"></asp:Label>:
							</td>
							<td class="FormFieldCell">
								<asp:TextBox runat="server" ID="MinQuantity"></asp:TextBox>
								<asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ValidationGroup="SalePriceDetails"
									ControlToValidate="MinQuantity" Display="Dynamic" ErrorMessage="*" />
								<asp:RangeValidator ID="RangeValidator3" runat="server" ValidationGroup="SalePriceDetails"
									ControlToValidate="MinQuantity" Display="Dynamic" ErrorMessage="*" Type="Double"
									MinimumValue="0" MaximumValue="10000"></asp:RangeValidator>
							</td>
						</tr>
						<tr>
							<td class="FormLabelCell">
								<asp:Label ID="Label5" runat="server" Text="<%$ Resources:SharedStrings, Start_Date %>"></asp:Label>:
							</td>
							<td class="FormFieldCell">
								<ecf:CalendarDatePicker runat="server" ID="StartDate" ValidationGroup="SalePriceDetails" />
							</td>
						</tr>
						<tr>
							<td class="FormLabelCell">
								<asp:Label ID="Label8" runat="server" Text="<%$ Resources:SharedStrings, End_Date %>"></asp:Label>:
							</td>
							<td class="FormFieldCell">
								<ecf:CalendarDatePicker runat="server" ID="EndDate" ValidationGroup="SalePriceDetails" />
							</td>
						</tr>
                        <tr>
                            <td class="FormLabelCell">
                                <asp:Label ID="Label9" runat="server" Text="Market"></asp:Label>
                            </td>
                            <td class="FormFieldCell">
                                <asp:DropDownList runat="server" ID="MarketFilter" AutoPostBack="true" DataValueField="MarketId" DataTextField="MarketName"></asp:DropDownList>
                            </td>
                        </tr>
						<ecf:MetaData ValidationGroup="SalePriceDetails" runat="server" ID="MetaDataTab" />
					</table>
					<!-- /Content Area -->
				</td>
			</tr>
			<tr>
				<td style="background-image: url(<%= CommerceHelper.GetAbsolutePath("~/Apps/Shell/Styles/images/dialog/bottom_content.gif") %>);
					height: 41px; padding-right: 10px;" align="right">
					<asp:Button runat="server" ID="SaveChangesButton" ValidationGroup="SalePriceDetails"
						OnClick="SaveChangesButton_Click" Text="<%$ Resources:CatalogStrings, Entry_Save_Changes %>" />
				</td>
                <td style="background-image: url(<%= CommerceHelper.GetAbsolutePath("~/Apps/Shell/Styles/images/dialog/bottom_content.gif") %>);
					height: 41px; padding-right: 10px;" align="right">
					<asp:Button runat="server" ID="CancelChangesButton" causesvalidation="false"
						OnClientClick="SalePriceEditPopup_CloseDialog()" Text="<%$ Resources:CatalogStrings, Entry_Cancel_Changes %>" />
				</td>
			</tr>
		</table>
	</ContentTemplate>
</asp:UpdatePanel>

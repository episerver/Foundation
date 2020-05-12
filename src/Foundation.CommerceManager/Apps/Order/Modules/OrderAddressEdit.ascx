<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrderAddressEdit.ascx.cs"
	Inherits="Mediachase.Commerce.Manager.Apps.Order.Modules.OrderAddressEdit" %>
<%@ Register TagPrefix="mc2" Assembly="Mediachase.BusinessFoundation" Namespace="Mediachase.BusinessFoundation" %>
<%@ Register Src="~/Apps/Core/MetaData/EditTab.ascx" TagName="MetaData" TagPrefix="ecf" %>
<div class="popup-outer" style="height: 475px;">
	<asp:Label runat="server" ID="lblErrorInfo" Style="color: Red"></asp:Label>
<%--	<asp:UpdatePanel UpdateMode="Always" ID="OuterPanel" runat="server" RenderMode="Block">
		<ContentTemplate>
--%>		   
			<table width="100%" class="DataForm" style="table-layout: fixed; height: 450px;">
			    <tr runat="server" id="ExistingAddressesRow">
					<td class="FormLabelCell" style="width: 125px;">
						<asp:Label ID="lblAddress" runat="server" Text="<%$ Resources:Customer, Address %>"></asp:Label>:
					</td>
					<td class="FormFieldCell">
					    <asp:DropDownList runat="server" ID="ddlShippingAddress" Width="325" CssClass="jQuerySelect" AutoPostBack="true"></asp:DropDownList>
					    <div style="padding-top: 7px;">
					    <asp:CheckBox runat="server" ID="cbSameAsBilling" Text="<%$ Resources:Customer, Same_As_Billing_Address %>" Visible="false" CausesValidation="false" />
					    </div>
					</td>
				</tr>
				<asp:Panel ID="mainPanelEnabled" runat="server">
<%--				<asp:UpdatePanel runat="server" ID="MainPanel" ChildrenAsTriggers="false" UpdateMode="Conditional" EnableViewState="true" RenderMode="Block">
                    <ContentTemplate>
--%>				        <tr>
					        <td class="FormLabelCell">
						        <asp:Label ID="Label5" runat="server" Text="<%$ Resources:SharedStrings, Name %>"></asp:Label>:
					        </td>
					        <td class="FormFieldCell">
						        <asp:TextBox runat="server" ID="Name" Width="230"></asp:TextBox>
						        <asp:Literal Mode="Encode" ID="lblName" runat="server"></asp:Literal>




					        </td>
				        </tr>
				        <tr>
					        <td class="FormLabelCell">
						        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:SharedStrings, First_Name %>"></asp:Label>:
					        </td>
					        <td class="FormFieldCell">
						        <asp:TextBox runat="server" ID="FirstName" MaxLength="64" ValidationGroup="AddressMetaData" Width="230"></asp:TextBox>
						        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator0" ValidationGroup="AddressMetaData"
							        ControlToValidate="FirstName" Display="Dynamic" ErrorMessage="*" />
					        </td>
				        </tr>
				        <tr>
					        <td class="FormLabelCell">
						        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:SharedStrings, Last_Name %>"></asp:Label>:
					        </td>
					        <td class="FormFieldCell">
						        <asp:TextBox runat="server" ID="LastName" MaxLength="64" ValidationGroup="AddressMetaData" Width="230"></asp:TextBox>
						        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ValidationGroup="AddressMetaData"
							        ControlToValidate="LastName" Display="Dynamic" ErrorMessage="*" />
					        </td>
				        </tr>
                        <tr>
                            <td class="FormLabelCell">
                                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:SharedStrings, Organization %>"></asp:Label>:
                            </td>
                            <td class="FormFieldCell">
                                <asp:TextBox runat="server" ID="Organization" MaxLength="64" Width="230"></asp:TextBox>
                            </td>
                        </tr>
				        <tr>
					        <td class="FormLabelCell">
						        <asp:Label ID="Label6" runat="server" Text="<%$ Resources:SharedStrings, Line_1 %>"></asp:Label>:
					        </td>
					        <td class="FormFieldCell">
						        <asp:TextBox runat="server" ID="Line1" MaxLength="64" ValidationGroup="AddressMetaData" Width="230"></asp:TextBox>
						        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ValidationGroup="AddressMetaData"
							        ControlToValidate="Line1" Display="Dynamic" ErrorMessage="*" />
					        </td>
				        </tr>
				        <tr>
					        <td class="FormLabelCell">
						        <asp:Label ID="Label7" runat="server" Text="<%$ Resources:SharedStrings, Line_2 %>"></asp:Label>:
					        </td>
					        <td class="FormFieldCell">
						        <asp:TextBox runat="server" ID="Line2" MaxLength="64" Width="230"></asp:TextBox>
					        </td>
				        </tr>
				        <tr>
					        <td class="FormLabelCell">
						        <asp:Label ID="Label8" runat="server" Text="<%$ Resources:SharedStrings, City %>"></asp:Label>:
					        </td>
					        <td class="FormFieldCell">
						        <asp:TextBox runat="server" ID="City" MaxLength="64" ValidationGroup="AddressMetaData" Width="230"></asp:TextBox>
						        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ValidationGroup="AddressMetaData"
							        ControlToValidate="City" Display="Dynamic" ErrorMessage="*" />
					        </td>
				        </tr>
				        <tr>
					        <td class="FormLabelCell">
						        <asp:Label ID="Label10" runat="server" Text="<%$ Resources:SharedStrings, Country_Name %>"></asp:Label>:
					        </td>
					        <td class="FormFieldCell">
					            <asp:DropDownList AutoPostBack="True" ID="ddlCountry" runat="server" DataTextField="Name"
                                    DataMember="Country" DataValueField="Code" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged">
                                </asp:DropDownList>
						        <asp:TextBox runat="server" ID="tbCountryCode" MaxLength="50" ValidationGroup="AddressMetaData" Width="230" Visible="false"></asp:TextBox>
						        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator5" ValidationGroup="AddressMetaData"
							        ControlToValidate="ddlCountry" Display="Dynamic" ErrorMessage="*" />
					        </td>
				        </tr>
				        <tr>
					        <td class="FormLabelCell">
						        <asp:Label ID="lblStateRegion" runat="server" Text="<%$ Resources:SharedStrings, State %>"></asp:Label>:
					        </td>
					        <td class="FormFieldCell">
					            <asp:DropDownList ID="ddlState" runat="server" DataTextField="Name" DataValueField="Name" ></asp:DropDownList>
						        <asp:TextBox runat="server" ID="tbState" MaxLength="50" ValidationGroup="AddressMetaData" Width="230"></asp:TextBox>
						        <asp:RequiredFieldValidator runat="server" ID="StateRequiredValidator" ValidationGroup="AddressMetaData"
							        ControlToValidate="tbState" Display="Dynamic" ErrorMessage="*" Enabled="false"/>
					        </td>
				        </tr>
				        <tr>
					        <td class="FormLabelCell">
						        <asp:Label ID="Label12" runat="server" Text="<%$ Resources:SharedStrings, Postal_Code %>"></asp:Label>:
					        </td>
					        <td class="FormFieldCell">
						        <asp:TextBox runat="server" ID="PostalCode" MaxLength="20" ValidationGroup="AddressMetaData" Width="230"></asp:TextBox>
						        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator6" ValidationGroup="AddressMetaData"
							        ControlToValidate="PostalCode" Display="Dynamic" ErrorMessage="*" />
					        </td>
				        </tr>
				        <tr>
					        <td class="FormLabelCell">
						        <asp:Label ID="Label15" runat="server" Text="<%$ Resources:SharedStrings, Day_Phone %>"></asp:Label>:
					        </td>
					        <td class="FormFieldCell">
						        <asp:TextBox runat="server" ID="DayTimePhone" MaxLength="32" Width="230"></asp:TextBox>
					        </td>
				        </tr>
				        <tr>
					        <td class="FormLabelCell">
						        <asp:Label ID="Label16" runat="server" Text="<%$ Resources:SharedStrings, Evening_Phone %>"></asp:Label>:
					        </td>
					        <td class="FormFieldCell">
						        <asp:TextBox runat="server" ID="EveningPhone" MaxLength="32" Width="230"></asp:TextBox>
					        </td>
				        </tr>
				        <tr>
					        <td class="FormLabelCell">
						        <asp:Label ID="Label18" runat="server" Text="<%$ Resources:SharedStrings, Email %>"></asp:Label>:
					        </td>
					        <td class="FormFieldCell">
						        <asp:TextBox runat="server" ID="Email" MaxLength="64" ValidationGroup="AddressMetaData" Width="230"></asp:TextBox>
						        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator7" ValidationGroup="AddressMetaData" Enabled="false"
							        ControlToValidate="Email" Display="Dynamic" ErrorMessage="*" />
						        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ValidationGroup="AddressMetaData"
							        ControlToValidate="Email" Display="Dynamic" ErrorMessage="*" ValidationExpression="[\w\.-]+(\+[\w-]*)?@([\w-]+\.)+[\w-]+"></asp:RegularExpressionValidator>
					        </td>
				        </tr>
				    <ecf:metadata validationgroup="AddressMetaData" runat="server" id="MetaDataTab" />
<%--				    </ContentTemplate>
				</asp:UpdatePanel>
--%>				</asp:Panel>
				<tr id="AddCustomerAddressRow" runat="server">
				    <td>&nbsp;</td>
					<td class="FormFieldCell">
						<asp:CheckBox ID="chbAddCustomerAddress" runat="server" CssClass="withpadding"></asp:CheckBox>
					</td>
				</tr>
				<tr>
					<td align="right" colspan="2" style="padding-top: 10px; padding-right: 10px;">
						<mc2:IMButton ID="btnSave" runat="server" style="width: 105px;" OnServerClick="btnSave_ServerClick" ValidationGroup="AddressMetaData">
						</mc2:IMButton>
						&nbsp;
						<mc2:IMButton ID="btnCancel" runat="server" style="width: 105px;" CausesValidation="false">
						</mc2:IMButton>
					</td>
				</tr>
			</table>
<%--		</ContentTemplate>
	</asp:UpdatePanel>--%>
</div>
<style type="text/css">
	form
	{
		overflow: auto ! important;
	}
	
</style>


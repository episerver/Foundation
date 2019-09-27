<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddressEdit.ascx.cs" Inherits="Mediachase.Commerce.Manager.Apps.Customer.Modules.AddressEdit" %>
<%@ Register TagPrefix="mc2" Assembly="Mediachase.BusinessFoundation" Namespace="Mediachase.BusinessFoundation" %>

<style type="text/css">
table.padding5 {table-layout:fixed}
table.padding5 td {padding:5px;}
table.padding5 table td {padding:0px;}
</style>
<asp:Label CssClass="ibn-alerttext" runat="server" ID="Messages" Text="<%$Resources:Customer, Address_mf_AddressType %>" Visible="False" ></asp:Label>
<table width="100%" class="ibn-propertysheet" border="0" cellSpacing="0" cellPadding="0">
	<tr>
		<td>
			<div class="text" style="padding:10px;">
				<table class="padding5">
					<tr>
						<td valign="top" style="width:90px;" class="labelSmartTableLayoutItem" rowspan="3">
							<asp:Literal runat="server" ID="AddressTypeLabel" Text="<%$Resources:Customer, Address_mf_AddressType %>"></asp:Literal>:
						</td>
						<td valign="top" style="width:150px;" rowspan="3">
							<div style="border: 1px solid #7F9DB9; padding:5px; height:60px; overflow-y: auto;">
								<asp:DataGrid id="grdMain" runat="server" allowsorting="False" allowpaging="False" width="100%" autogeneratecolumns="False" borderwidth="0" gridlines="None" cellpadding="0" CellSpacing="0" ShowFooter="False" ShowHeader="False">
									<columns>
										<asp:boundcolumn visible="false" datafield="Id"></asp:boundcolumn>
										<asp:templatecolumn itemstyle-cssclass="text">
											<itemtemplate>
												<asp:checkbox runat="server" id="chkItem" Text='<%# Eval("Name")%>' TabIndex="1"></asp:checkbox>
											</itemtemplate>
										</asp:templatecolumn>
									</columns>
								</asp:DataGrid>
							</div>
						</td>
						<td style="width:20px;" rowspan="3">
							<asp:CustomValidator runat="server" ID="AddressTypeValidator" Display="Dynamic" 
								ErrorMessage="*" onservervalidate="AddressTypeValidator_ServerValidate"></asp:CustomValidator>
						</td>
						<td style="width:90px;" class="labelSmartTableLayoutItem">
							<asp:Literal runat="server" ID="Literal1" Text="<%$Resources:Customer, Address_mf_Line1 %>"></asp:Literal>:
						</td>
						<td style="width:150px;">
							<asp:TextBox runat="server" ID="Line1Text" Width="150" TabIndex="8" MaxLength="80"></asp:TextBox>
						</td>
                        <td>
							<asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="Line1Text" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
						</td>
						<td style="width:20px;"></td>
					</tr>
					<tr>
						<td class="labelSmartTableLayoutItem">
							<asp:Literal runat="server" ID="Literal2" Text="<%$Resources:Customer, Address_mf_Line2 %>"></asp:Literal>:
						</td>
						<td>
							<asp:TextBox runat="server" ID="Line2Text" Width="150" TabIndex="9" MaxLength="80"></asp:TextBox>
						</td>
					</tr>
					<tr>
						<td class="labelSmartTableLayoutItem">
							<asp:Literal runat="server" ID="Literal4" Text="<%$Resources:Customer, Address_mf_City %>"></asp:Literal>:
						</td>
						<td>
							<asp:TextBox runat="server" ID="CityText" Width="150" TabIndex="10" MaxLength="64"></asp:TextBox>
						</td>
                        <td>
							<asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="CityText" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
						</td>
					</tr>
					<tr>
						<td class="labelSmartTableLayoutItem">
							<asp:Literal runat="server" ID="Literal3" Text="<%$Resources:Customer, Address_mf_Name %>"></asp:Literal>:
						</td>
						<td>
							<asp:TextBox runat="server" ID="NameText" Width="150" TabIndex="2" MaxLength="64"></asp:TextBox>
						</td>
						<td>
							<asp:RequiredFieldValidator runat="server" ID="NameTextValidator" ControlToValidate="NameText" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
						</td>
						<td class="labelSmartTableLayoutItem">
							<asp:Literal runat="server" ID="Literal6" Text="<%$Resources:Customer, Address_mf_CountryName %>"></asp:Literal>:
						</td>
						<td>
							<asp:DropDownList runat="server" ID="CountryList" Width="150" AutoPostBack="true" TabIndex="11"
								DataMember="Country" DataTextField="Name" DataValueField="Code" 
								onselectedindexchanged="CountryList_SelectedIndexChanged"></asp:DropDownList>
						</td>
					</tr>
					<tr>
						<td class="labelSmartTableLayoutItem">
							<asp:Literal runat="server" ID="Literal5" Text="<%$Resources:Customer, Address_mf_FirstName %>"></asp:Literal>:
						</td>
						<td>
							<asp:TextBox runat="server" ID="FirstNameText" Width="150" TabIndex="3" MaxLength="64"></asp:TextBox>
						</td>
                        <td>
							<asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ControlToValidate="FirstNameText" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
						</td>
						<td class="labelSmartTableLayoutItem">
							<asp:Literal runat="server" ID="Literal8" Text="<%$Resources:Customer, Address_mf_State %>"></asp:Literal>:
						</td>
						<td>
							<asp:DropDownList runat="server" ID="StateList" DataTextField="Name" DataValueField="Name" Width="150" TabIndex="12"></asp:DropDownList>
							<asp:TextBox runat="server" ID="StateText" Width="150" TabIndex="12" MaxLength="64"></asp:TextBox>
						</td>
					</tr>
					<tr>
						<td class="labelSmartTableLayoutItem">
							<asp:Literal runat="server" ID="Literal7" Text="<%$Resources:Customer, Address_mf_LastName %>"></asp:Literal>:
						</td>
						<td>
							<asp:TextBox runat="server" ID="LastNameText" Width="150" TabIndex="4" MaxLength="64"></asp:TextBox>
						</td>
                        <td>
							<asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator4" ControlToValidate="LastNameText" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
						</td>
						<td class="labelSmartTableLayoutItem">
							<asp:Literal runat="server" ID="Literal10" Text="<%$Resources:Customer, Address_mf_PostalCode %>"></asp:Literal>:
						</td>
						<td>
							<asp:TextBox runat="server" ID="PostalCodeText" Width="150" TabIndex="13" MaxLength="20"></asp:TextBox>
						</td>
                        <td>
							<asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator5" ControlToValidate="PostalCodeText" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
						</td>
					</tr>
					<tr>
						<td class="labelSmartTableLayoutItem">
							<asp:Literal runat="server" ID="Literal9" Text="<%$Resources:Customer, Address_mf_DaytimePhoneNumber %>"></asp:Literal>:
						</td>
						<td>
							<asp:TextBox runat="server" ID="DayPhoneText" Width="150" TabIndex="5" MaxLength="32"></asp:TextBox>
						</td>
                        <td>
							<asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator6" ControlToValidate="DayPhoneText" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
						</td>
						<td class="labelSmartTableLayoutItem">
							<asp:Literal runat="server" ID="Literal13" Text="<%$Resources:Customer, Address_mf_RegionCode %>"></asp:Literal>:
						</td>
						<td>
							<asp:TextBox runat="server" ID="RegionCodeText" Width="150" TabIndex="14" MaxLength="50"></asp:TextBox>
						</td>
					</tr>
					<tr>
						<td class="labelSmartTableLayoutItem">
							<asp:Literal runat="server" ID="Literal11" Text="<%$Resources:Customer, Address_mf_EveningPhoneNumber %>"></asp:Literal>:
						</td>
						<td>
							<asp:TextBox runat="server" ID="EveningPhoneText" Width="150" TabIndex="6" MaxLength="32"></asp:TextBox>
						</td>
						<td></td>
						<td class="labelSmartTableLayoutItem">
							<asp:Literal runat="server" ID="Literal14" Text="<%$Resources:Customer, Address_mf_RegionName %>"></asp:Literal>:
						</td>
						<td>
							<asp:TextBox runat="server" ID="RegionnameText" Width="150" TabIndex="15" MaxLength="64"></asp:TextBox>
						</td>
						<td></td>
					</tr>
					<tr>
						<td class="labelSmartTableLayoutItem">
							<asp:Literal runat="server" ID="Literal12" Text="<%$Resources:Customer, Address_mf_Email %>"></asp:Literal>:
						</td>
						<td>
							<asp:TextBox runat="server" ID="EmailText" Width="150" TabIndex="7" MaxLength="64"></asp:TextBox>
						</td>
						<td>
							<asp:RegularExpressionValidator id="EmailText_RegEx" runat="server" ErrorMessage="*" ControlToValidate="EmailText" ValidationExpression="[\w\.-]+(\+[\w-]*)?@([\w-]+\.)+[\w-]+" Display="Dynamic"></asp:RegularExpressionValidator>
						</td>
					</tr>
				</table>
			</div>
		</td>
	</tr>
	<tr>
		<td align="right" style="padding-top: 10px; padding-right: 50px; text-align:right;">
			<mc2:IMButton ID="SaveButton" runat="server" style="width: 105px;" OnServerClick="SaveButton_ServerClick">
			</mc2:IMButton>
			&nbsp;
			<mc2:IMButton ID="CancelButton" runat="server" style="width: 105px;" CausesValidation="false">
			</mc2:IMButton>
		</td>
	</tr>
</table>

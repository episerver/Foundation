<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrderPaymentEdit.ascx.cs"
	Inherits="Mediachase.Commerce.Manager.Apps.Order.Modules.OrderPaymentEdit" %>
<%@ Register TagPrefix="mc2" Assembly="Mediachase.BusinessFoundation" Namespace="Mediachase.BusinessFoundation" %>
<%@ Register Src="~/Apps/Core/MetaData/EditTab.ascx" TagName="MetaData" TagPrefix="ecf" %>
<div style="padding: 5px;">
	<asp:Label runat="server" ID="lblErrorInfo" Style="color: Red"></asp:Label>
	<asp:UpdatePanel UpdateMode="Conditional" ID="DialogContentPanel" runat="server"
		RenderMode="Block">
		<ContentTemplate>
			<table width="100%" class="DataForm">
				<tr>
					<td class="FormLabelCell">
						<asp:Label ID="Label5" runat="server" Text="<%$ Resources:OrderStrings, Payment_Type %>"></asp:Label>:
					</td>
					<td class="FormFieldCell">
						<asp:DropDownList runat="server" ID="PaymentType" DataMember="PaymentMethod" DataTextField="Name"
							DataValueField="PaymentMethodId" AutoPostBack="true">
						</asp:DropDownList>
						<asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ValidationGroup="PaymentMetaData"
							ControlToValidate="PaymentType" Display="Dynamic" ErrorMessage="*" />
						<br />
						<asp:Label ID="Label1" CssClass="FormFieldDescription" runat="server" Text="<%$ Resources:OrderStrings, Payment_Attributes_Description %>"></asp:Label>
					</td>
				</tr>
				<tr>
					<td colspan="2">
					</td>
				</tr>
				<tr>
					<td class="FormLabelCell">
						<asp:Label ID="Label2" runat="server" Text="<%$ Resources:OrderStrings, Payment_Method %>"></asp:Label>:
					</td>
					<td class="FormFieldCell">
						<asp:DropDownList runat="server" ID="PaymentMethodList" DataMember="PaymentMethod"
							DataTextField="Name" DataValueField="PaymentMethodId">
						</asp:DropDownList>
						<asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ValidationGroup="PaymentMetaData"
							ControlToValidate="PaymentMethodList" Display="Dynamic" ErrorMessage="*" />
						<br />
						<asp:Label ID="Label6" CssClass="FormFieldDescription" runat="server" Text="<%$ Resources:SharedStrings, Payment_Method_Description %>"></asp:Label>
					</td>
				</tr>
				<tr>
					<td colspan="2">
					</td>
				</tr>
				<tr>
					<td class="FormLabelCell">
						<asp:Label ID="Label7" runat="server" Text="<%$ Resources:SharedStrings, Name %>"></asp:Label>:
					</td>
					<td class="FormFieldCell">
						<asp:TextBox runat="server" ID="Name"></asp:TextBox>
						<br />
						<asp:Label ID="Label8" CssClass="FormFieldDescription" runat="server" Text="<%$ Resources:OrderStrings, Payment_Name_Description %>"></asp:Label>
					</td>
				</tr>
				<tr>
					<td colspan="2">
					</td>
				</tr>
				<tr>
					<td class="FormLabelCell">
						<asp:Label ID="Label9" runat="server" Text="<%$ Resources:SharedStrings, Amount %>"></asp:Label>:
					</td>
					<td class="FormFieldCell">
						<asp:TextBox runat="server" ID="Amount"></asp:TextBox>
						<asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator7" ValidationGroup="PaymentMetaData"
							ControlToValidate="Amount" Display="Dynamic" ErrorMessage="*" />
						<asp:RangeValidator ID="RangeValidatorAmount" runat="server" ValidationGroup="PaymentMetaData"
							ControlToValidate="Amount" Display="Dynamic" ErrorMessage="*" Type="Currency"
							MinimumValue="0" MaximumValue="1000000000"></asp:RangeValidator>
						<br />
						<asp:Label ID="Label10" CssClass="FormFieldDescription" runat="server" Text="<%$ Resources:OrderStrings, Payment_For_Description %>"></asp:Label>
					</td>
				</tr>
				<tr>
					<td colspan="2">
					</td>
				</tr>
				<tr>
					<td class="FormLabelCell">
						<asp:Label ID="Label11" runat="server" Text="<%$ Resources:SharedStrings, Status %>"></asp:Label>:
					</td>
					<td class="FormFieldCell">
						<asp:DropDownList runat="server" ID="PaymentStatus">
							<asp:ListItem Value="Pending" Text="<%$ Resources:SharedStrings, Pending %>"></asp:ListItem>
							<asp:ListItem Value="Failed" Text="<%$ Resources:SharedStrings, Failed %>"></asp:ListItem>
							<asp:ListItem Value="Processed" Text="<%$ Resources:SharedStrings, Processed %>"></asp:ListItem>
						</asp:DropDownList>
						<br />
						<asp:Label ID="Label12" CssClass="FormFieldDescription" runat="server" Text="<%$ Resources:OrderStrings, Payment_Status_Description %>"></asp:Label>
					</td>
				</tr>
				<tr>
					<td colspan="2">
					</td>
				</tr>
				<asp:PlaceHolder runat="server" ID="MetaPlaceHolder"></asp:PlaceHolder>
				<ecf:MetaData ValidationGroup="PaymentMetaData" runat="server" ID="MetaDataTab" />
				<tr>
					<td align="right" colspan="2" style="padding-top: 10px; padding-right: 10px;">
						<mc2:IMButton ID="btnSave" runat="server" style="width: 105px;" OnServerClick="btnSave_ServerClick">
						</mc2:IMButton>
						&nbsp;
						<mc2:IMButton ID="btnCancel" runat="server" style="width: 105px;" CausesValidation="false">
						</mc2:IMButton>
					</td>
				</tr>
			</table>
		</ContentTemplate>
	</asp:UpdatePanel>
</div>

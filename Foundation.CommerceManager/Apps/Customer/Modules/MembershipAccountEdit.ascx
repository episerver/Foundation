<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MembershipAccountEdit.ascx.cs"
	Inherits="Mediachase.Commerce.Manager.Apps.Customer.Modules.MembershipAccountEdit" %>
<%@ Register Src="~/Apps/Core/Controls/BooleanEditControl.ascx" TagName="BooleanEditControl"
	TagPrefix="ecf" %>
<%@ Register TagPrefix="mc2" Assembly="Mediachase.BusinessFoundation" Namespace="Mediachase.BusinessFoundation" %>
<div style="padding: 5px;">
	<asp:Label runat="server" ID="lblErrorInfo" Style="color: Red"></asp:Label>
	<table width="100%" class="DataForm">
		<tr>
			<td class="FormLabelCell">
				<asp:Label runat="server" ID="UserNameLabel" Text="<%$ Resources:SharedStrings, User_Name %>"></asp:Label>:
			</td>
			<td class="FormFieldCell">
				<asp:TextBox runat="server" ID="UserNameTextBox" Width="250" MaxLength="256"></asp:TextBox>
				<asp:RequiredFieldValidator runat="server" ID="UserNameRequired" ControlToValidate="UserNameTextBox"
											ErrorMessage="<%$ Resources:SharedStrings, User_Name_Required %>" >*</asp:RequiredFieldValidator>
			</td>
			
		</tr>
		<tr>
			<td colspan="2" class="FormSpacerCell">
			</td>
		</tr>
		<tr runat="server" id="TrPassword">
			<td class="FormLabelCell">
				<asp:Label runat="server" ID="LabelPassword" Text="<%$ Resources:SharedStrings, Password %>"></asp:Label>:
			</td>
			<td class="FormFieldCell">
				<asp:TextBox runat="server" ID="TbPassword" TextMode="Password" Width="250"></asp:TextBox>
				<asp:RequiredFieldValidator runat="server" ID="PasswordRequired" ControlToValidate="TbPassword"
											 ErrorMessage="<%$ Resources:SharedStrings, RequiredField %>">*</asp:RequiredFieldValidator>
				<asp:CustomValidator ID="PasswordCustomValidator" runat="server" ControlToValidate="TbPassword"
					OnServerValidate="UserPassword_ServerValidate" dispaly="dynamic" />
			</td>
		</tr>
		<tr>
			<td class="FormLabelCell">
				<asp:Label ID="Label14" runat="server" Text="<%$ Resources:SharedStrings, Description %>"></asp:Label>:
			</td>
			<td class="FormFieldCell">
				<asp:TextBox runat="server" Width="250" ID="tbDescription"></asp:TextBox><br />
			</td>
		</tr>
		<tr>
			<td colspan="2" class="FormSpacerCell">
			</td>
		</tr>
		<tr>
			<td class="FormLabelCell">
				<asp:Label runat="server" ID="Label5" Text="<%$ Resources:SharedStrings, Email %>"></asp:Label>:
			</td>
			<td class="FormFieldCell">
				<asp:TextBox runat="server" ID="tbEmailText" Width="250" MaxLength="256"></asp:TextBox>
				<asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="tbEmailText"
											 ErrorMessage="<%$ Resources:SharedStrings, RequiredField %>">*</asp:RequiredFieldValidator>
				<asp:RegularExpressionValidator id="RegularExpressionValidator1" 
					  runat="server" ControlToValidate="tbEmailText" 
					  display="Dynamic"
					  ErrorMessage="<%$ Resources:SharedStrings, Valid_Email_Required%>" 
					  ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
				</asp:RegularExpressionValidator>
			</td>
		</tr>
		<tr>
			<td colspan="2" class="FormSpacerCell">
			</td>
		</tr>
		<tr>
			<td class="FormLabelCell">
				<asp:Label runat="server" ID="Label6" Text="<%$ Resources:SharedStrings, Approved %>"></asp:Label>:
			</td>
			<td class="FormFieldCell">
				<ecf:BooleanEditControl ID="IsApproved" runat="server"></ecf:BooleanEditControl>
			</td>
		</tr>
		<tr>
			<td colspan="2" class="FormSpacerCell">
			</td>
		</tr>
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
</div>

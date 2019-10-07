<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MembershipAccountChangePassword.ascx.cs"
	Inherits="Mediachase.Commerce.Manager.Apps.Customer.Modules.MembershipAccountChangePassword" %>
<%@ Register Src="~/Apps/Core/Controls/BooleanEditControl.ascx" TagName="BooleanEditControl"
	TagPrefix="ecf" %>
<%@ Register TagPrefix="mc2" Assembly="Mediachase.BusinessFoundation" Namespace="Mediachase.BusinessFoundation" %>
<div style="padding: 5px;">
	<asp:Label runat="server" ID="lblErrorInfo" Style="color: Red"></asp:Label>
	<table width="100%" class="DataForm">
		<tr runat="server" id="TrChangePasswordForced" visible="false">
			<td>
				<table border="0" cellpadding="0" width="350px">
					<tr>
						<td align="center" colspan="2">
							<asp:Label runat="server" ID="label1" Font-Bold="true" Font-Size="Small" Text="<%$ Resources:Customer, ChangeUserPassword %>"></asp:Label>
						</td>
					</tr>
					<tr>
						<td colspan="2" class="FormSpacerCell">
							<br />
						</td>
					</tr>
					<tr>
						<td class="FormLabelCell">
							<asp:Label runat="server" ID="LabelPassword" Text="<%$ Resources:SharedStrings, User_Name %>"></asp:Label>:
						</td>
						<td class="FormFieldCell">
							<asp:TextBox runat="server" ID="TbUserName" Width="150"></asp:TextBox>
						</td>
					</tr>
					<tr>
						<td class="FormLabelCell">
							<asp:Label runat="server" ID="Label2" Text="<%$ Resources:SharedStrings, New_Password %>"></asp:Label>:
						</td>
						<td class="FormFieldCell">
							<asp:TextBox runat="server" ID="TbNewPassword" TextMode="Password" Width="150"></asp:TextBox>
							<br />
							<asp:Label runat="server" ID="PasswordChangeDescription" class="FormFieldDescription" Text="" />
							<br />
							<asp:CustomValidator ID="PasswordCustomValidator" runat="server" ControlToValidate="TbNewPassword"
								OnServerValidate="UserPassword_ServerValidate" dispaly="dynamic" ValidateEmptyText="true" />
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
			</td>
		</tr>
		<tr runat="server" id="TrChangePassword" visible="false">
			<td>
				<table>
					<tr>
						<td colspan="2">
							<asp:ChangePassword ID="aspChangePassword" runat="server" DisplayUserName="True">
								<ChangePasswordTemplate>
									<table border="0" cellpadding="1">
										<tr>
											<td>
												<table border="0" cellpadding="0" width="350px">
													<tr>
														<td align="center" colspan="2">
															<asp:Label runat="server" ID="label2" Font-Bold="true" Font-Size="Small" Text="<%$ Resources:Customer, ChangeUserPassword %>"></asp:Label>
														</td>
													</tr>
													<tr>
														<td colspan="2" class="FormSpacerCell">
															<br />
														</td>
													</tr>
													<tr>
														<td  class="FormLabelCell">
															<asp:Label AssociatedControlID="UserName" ID="UserNameLabel" runat="server" Text="<%$ Resources:SharedStrings, User_Name %>"></asp:Label>:
														</td>
														<td class="FormFieldCell">
															<asp:TextBox ID="UserName" runat="server" ></asp:TextBox>
															<asp:RequiredFieldValidator ControlToValidate="UserName" ErrorMessage="<%$ Resources:SharedStrings, RequiredField %>"
																ID="UserNameRequired" runat="server" ToolTip="<%$ Resources:SharedStrings, User_Name %>"
																ValidationGroup="ChangePassword1">*</asp:RequiredFieldValidator>
														</td>
													</tr>
													<tr>
														<td class="FormLabelCell">
															<asp:Label ID="CurrentPasswordLabel" runat="server" AssociatedControlID="CurrentPassword"
																Text="<%$ Resources:SharedStrings, Password %>"></asp:Label>:
														</td>
														<td  class="FormFieldCell">
															<asp:TextBox ID="CurrentPassword" runat="server" TextMode="Password"></asp:TextBox>
															<asp:RequiredFieldValidator ID="CurrentPasswordRequired" runat="server" ControlToValidate="CurrentPassword"
																ErrorMessage="<%$ Resources:SharedStrings, RequiredField %>" ToolTip="<%$ Resources:SharedStrings, Password %>"
																ValidationGroup="ChangePassword1">*</asp:RequiredFieldValidator>
														</td>
													</tr>
													<tr>
														<td class="FormLabelCell">
															<asp:Label ID="NewPasswordLabel" runat="server" AssociatedControlID="NewPassword"
																Text="<%$ Resources:SharedStrings, New_Password %>"></asp:Label>:
														</td>
														<td  class="FormFieldCell">
															<asp:TextBox ID="NewPassword" runat="server" TextMode="Password"></asp:TextBox>
															<asp:RequiredFieldValidator ID="NewPasswordRequired" runat="server" ControlToValidate="NewPassword"
																ErrorMessage="<%$ Resources:SharedStrings, RequiredField %>" ToolTip="<%$ Resources:SharedStrings, New_Password %>"
																ValidationGroup="ChangePassword1">*</asp:RequiredFieldValidator>
														</td>
													</tr>
													<tr>
														<td class="FormLabelCell">
															<asp:Label ID="ConfirmNewPasswordLabel" runat="server" AssociatedControlID="ConfirmNewPassword"
																Text="<%$ Resources:SharedStrings, ConfirmNewPassword %>"></asp:Label>:
														</td>
														<td  class="FormFieldCell">
															<asp:TextBox ID="ConfirmNewPassword" runat="server" TextMode="Password"></asp:TextBox>
															<asp:RequiredFieldValidator ID="ConfirmNewPasswordRequired" runat="server" ControlToValidate="ConfirmNewPassword"
																ErrorMessage="<%$ Resources:SharedStrings, RequiredField %>" ToolTip="<%$ Resources:SharedStrings, ConfirmNewPassword %>"
																ValidationGroup="ChangePassword1">*</asp:RequiredFieldValidator>
														</td>
													</tr>
													<tr>
														<td class="FormLabelCell" colspan="2">
															<asp:CompareValidator ID="NewPasswordCompare" runat="server" ControlToCompare="NewPassword"
																ControlToValidate="ConfirmNewPassword" Display="Dynamic" ErrorMessage="<%$ Resources:SharedStrings, ConfirmAndNewPasswordNotEquals %>"
																ValidationGroup="ChangePassword1"></asp:CompareValidator>
														</td>
													</tr>
													<tr>
														<td align="center" colspan="2" style="color: red">
															<asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
														</td>
													</tr>
													<tr>
														<td align="center" colspan="2" style="padding-top: 10px; padding-right: 10px;">
															<mc2:IMButton ID="ChangePasswordPushButton" runat="server" style="width: 105px;"
																ValidationGroup="ChangePassword1">
															</mc2:IMButton>
															&nbsp;
															<mc2:IMButton ID="CancelPushButton" runat="server" style="width: 105px;" CausesValidation="false"
																CommandName="Cancel">
															</mc2:IMButton>
															<asp:Button runat="server" ID="btnRealChangePassword" ValidationGroup="ChangePassword1"
																CommandName="ChangePassword" />
														</td>
													</tr>
												</table>
											</td>
										</tr>
									</table>
								</ChangePasswordTemplate>
							</asp:ChangePassword>
						</td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
</div>

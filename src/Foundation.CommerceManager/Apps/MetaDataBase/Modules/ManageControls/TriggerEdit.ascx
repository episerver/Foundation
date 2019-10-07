<%@ Control Language="C#" AutoEventWireup="true" Inherits="Mediachase.Ibn.Web.UI.Apps.MetaDataBase.Modules.ManageControls.TriggerEdit" Codebehind="TriggerEdit.ascx.cs" %>
<table style="width:100%;">
	<tr>
		<td style="padding:10px;">
			<asp:UpdatePanel runat="server" ID="upMain">
			<ContentTemplate>
				<table cellpadding="3" cellspacing="0" border="0" width="100%" class="ibn-propertysheet" style="table-layout:fixed">
					<tr>
						<td style='width:<%= labelColumnWidth%>'>
							<asp:Literal ID="Literal1" Text="<%$Resources : GlobalMetaInfo, Name%>" runat="server"></asp:Literal>:
						</td>
						<td>
							<asp:TextBox runat="server" ID="txtName" Width="100%"></asp:TextBox>
						</td>
						<td style="width:20px;">
							<asp:RequiredFieldValidator runat="server" ID="rfvName" ControlToValidate="txtName" Display="dynamic" ErrorMessage="*"></asp:RequiredFieldValidator>
						</td>
					</tr>
					<tr>
						<td valign="top">
							<asp:Literal ID="Literal6" Text="<%$Resources : GlobalMetaInfo, Description%>" runat="server"></asp:Literal>:
						</td>
						<td>
							<asp:TextBox runat="server" ID="txtDescription" Width="100%" TextMode="MultiLine" Rows="3"></asp:TextBox>
						</td>
						<td></td>
					</tr>
					<tr>
						<td>
							<asp:Literal ID="Literal7" Text="<%$Resources : GlobalMetaInfo, Operation%>" runat="server"></asp:Literal>:
						</td>
						<td>
							<asp:CheckBox runat="server" ID="chkInsert" Text="Insert" />
							<asp:CheckBox runat="server" ID="chkUpdate" Text="Update" />
							<asp:CheckBox runat="server" ID="chkDelete" Text="Delete" />
						</td>
						<td></td>
					</tr>
					<tr>
						<td>
							<asp:Literal ID="Literal2" Text="<%$Resources : GlobalMetaInfo, TriggerCondition%>" runat="server"></asp:Literal>:
						</td>
						<td>
							<asp:DropDownList runat="server" ID="ddlCondition" Width="100%" OnSelectedIndexChanged="ddlCondition_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
						</td>
						<td></td>
					</tr>
				</table>
				<div style="padding-top:5px; padding-left:3px;">
					<asp:Literal ID="Literal4" Text="<%$Resources : GlobalMetaInfo, Parameters%>" runat="server"></asp:Literal>:
				</div>
				<div style="height:100px; overflow-y:auto; border:2px inset; margin-top:5px; margin-bottom:15px; background-color:#eeeeee; margin-right:20px;">
					<asp:Table runat="server" ID="tblCondition" CellPadding="3" CellSpacing="0" CssClass="ibn-propertysheet" style="table-layout:fixed">
					</asp:Table>
				</div>
				<table cellpadding="3" cellspacing="0" border="0" width="100%" class="ibn-propertysheet" style="table-layout:fixed">
					<tr>
						<td style='width:<%= labelColumnWidth%>'>
							<asp:Literal ID="Literal3" Text="<%$Resources : GlobalMetaInfo, TriggerAction%>" runat="server"></asp:Literal>:
						</td>
						<td>
							<asp:DropDownList runat="server" ID="ddlAction" Width="100%" OnSelectedIndexChanged="ddlAction_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
						</td>
						<td style="width:20px"></td>
					</tr>
				</table>
				<div style="padding-top:5px; padding-left:3px;">
					<asp:Literal ID="Literal5" Text="<%$Resources : GlobalMetaInfo, Parameters%>" runat="server"></asp:Literal>:
				</div>
				<div style="height:100px; overflow-y:auto; border:2px inset; margin-top:5px; margin-bottom:5px; background-color:#eeeeee; margin-right:20px;">
					<asp:Table runat="server" ID="tblAction" CellPadding="3" CellSpacing="0" CssClass="ibn-propertysheet" style="table-layout:fixed">
					</asp:Table>
				</div>
			</ContentTemplate>
			</asp:UpdatePanel>
			
			<div style="padding-top:10px; text-align:center;">
				<asp:Button runat="server" ID="btnSave" Text="<%$Resources : Global, _mc_Save%>" OnClick="btnSave_Click" Width="80" />
				<asp:Button runat="server" ID="btnCancel" Text="<%$Resources : Global, _mc_Cancel%>" OnClientClick="window.close();return false;" Width="80" style="margin-left:15px;" CausesValidation="false" />
			</div>
		</td>
	</tr>
</table>
<%@ Control Language="C#" AutoEventWireup="true" Inherits="Mediachase.Ibn.Web.UI.MetaDataBase.Modules.ManageControls.EnumViewControl" Codebehind="EnumViewControl.ascx.cs" %>
<%@ Import Namespace="System.Web" %>
<%@ Register TagPrefix="ibn" TagName="BlockHeader" Src="~/Apps/MetaDataBase/Common/Design/BlockHeader.ascx" %>
<table cellspacing="0" cellpadding="0" border="0" width="100%" class="ibn-stylebox2">
	<tr>
		<td>
			<ibn:BlockHeader id="secHeader" runat="server" />
		</td>
	</tr>
	<tr>
		<td class="ibn-light ibn-separator">
			<table cellspacing="0" cellpadding="0" width="100%" border="0" style="padding:5px"><tr><td>
				<table class="ibn-propertysheet" width="100%" border="0" cellpadding="5" cellspacing="0">
					<tr>
						<td class="ibn-label" align="right" style="width:110px;">
							<asp:Literal ID="Literal9" runat="server" Text="<%$Resources : GlobalMetaInfo, SystemName%>" />:
						</td>
						<td class="ibn-value">
							<asp:Label runat="server" ID="lblSystemName"></asp:Label>
						</td>
						<td class="ibn-label" align="right">
							<asp:Literal ID="Literal1" runat="server" Text="<%$Resources : GlobalMetaInfo, FriendlyName%>" />:
						</td>
						<td class="ibn-value">
							<asp:Label runat="server" ID="lblFriendlyName"></asp:Label>
						</td>
						<td class="ibn-label" align="right">
							<asp:Literal ID="Literal2" runat="server" Text="<%$Resources : GlobalMetaInfo, Type%>" />:
						</td>
						<td class="ibn-value">
							<asp:Label runat="server" ID="lbType"></asp:Label>
						</td>
					</tr>
				</table>
			</td></tr></table>
		</td>
	</tr>
	<tr>
		<td>
			<asp:UpdatePanel runat="server" ID="upMain">
				<ContentTemplate>
					<asp:Button ID="btnAddNewItem" Runat="server" CausesValidation="False" style="display:none" OnClick="btnAddNewItem_Click"></asp:Button> 
					<asp:DataGrid runat="server" ID="grdMain" AutoGenerateColumns="false" Width="100%" 
						CellPadding="4" GridLines="Horizontal" AllowPaging="false" AllowSorting="false" OnCancelCommand="grdMain_CancelCommand" OnDeleteCommand="grdMain_DeleteCommand" OnEditCommand="grdMain_EditCommand" OnUpdateCommand="grdMain_UpdateCommand">
						<Columns>
							<asp:TemplateColumn HeaderText="№" >
								<ItemStyle CssClass="ibn-vb" Width="50px" />
								<HeaderStyle CssClass="ibn-vh" Width="50px" />
								<ItemTemplate>
									<%# Eval("OrderId")%>
								</ItemTemplate>
								<EditItemTemplate>
									<asp:DropDownList runat="server" ID="ddlOrder" Width="50px"></asp:DropDownList>
								</EditItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="<%$Resources : GlobalMetaInfo, ItemValue%>" >
								<ItemStyle CssClass="ibn-vb" Width="40%" />
								<HeaderStyle CssClass="ibn-vh" Width="40%" />
								<ItemTemplate>
									<%# HttpUtility.HtmlEncode(Eval("Name")) %>
								</ItemTemplate>
								<EditItemTemplate>
									<asp:TextBox ID="txtName" Text='<%# DataBinder.Eval(Container.DataItem,"Name") %>' CssClass="text" Width="90%" Runat="server" MaxLength="255"></asp:TextBox>
									<img runat="server" id="imResourceTemplate" style="width:16px; height:16px" alt="" />
									<asp:RequiredFieldValidator ID="rfName" ControlToValidate="txtName" ErrorMessage="*" Display="Dynamic" Runat="server"/>
								</EditItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="<%$Resources : GlobalMetaInfo, ItemDisplayValue%>" >
								<ItemStyle CssClass="ibn-vb" />
								<HeaderStyle CssClass="ibn-vh" />
								<ItemTemplate>
									<%# HttpUtility.HtmlEncode(Eval("DisplayName")) %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn>
								<ItemStyle CssClass="ibn-vb" Width="50px"/>
								<HeaderStyle CssClass="ibn-vh" Width="50px" />
								<ItemTemplate>
									<asp:ImageButton id="ibEdit" runat="server" ImageUrl="~/Apps/MetaDataBase/images/edit.gif" ToolTip="<%$Resources : GlobalMetaInfo, Edit%>" Width="16" Height="16" CommandName="Edit" causesvalidation="False" CommandArgument='<%# Eval("Id") %>'></asp:ImageButton>
									&nbsp;
									<asp:ImageButton ID="ibDelete" Runat="server" ImageUrl="~/Apps/MetaDataBase/images/delete.gif" ToolTip="<%$Resources : GlobalMetaInfo, Delete%>" Width="16" Height="16" CommandName="<%$Resources : GlobalMetaInfo, Delete%>" CommandArgument='<%# Eval("Id") %>'></asp:ImageButton> 
								</ItemTemplate>
								<EditItemTemplate>
									<asp:ImageButton id="ibSave" runat="server" ToolTip="<%$Resources : GlobalMetaInfo, Save%>" ImageUrl="~/Apps/MetaDataBase/images/SaveItem.gif" commandname="Update" causesvalidation="True" CommandArgument='<%# Eval("Id") %>'></asp:ImageButton>
									&nbsp;
									<asp:imagebutton id="ibCancel" runat="server" ToolTip="<%$Resources : GlobalMetaInfo, Cancel%>" ImageUrl="~/Apps/MetaDataBase/images/cancel.gif" commandname="Cancel" causesvalidation="False">
									</asp:imagebutton>
								</EditItemTemplate>
							</asp:TemplateColumn>
						</Columns>
					</asp:DataGrid>
				</ContentTemplate>
			</asp:UpdatePanel>
		</td>
  </tr>
</table>

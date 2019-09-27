<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EnumMultiValue.Manage.ascx.cs" Inherits="Mediachase.Ibn.Web.UI.MetaDataBase.Primitives.EnumMultiValue_Manage " %>
<%@ Import Namespace="Mediachase.Commerce.Shared" %>
<table cellpadding="3" cellspacing="1" border="0" width="100%" class="ibn-propertysheet" style="table-layout:fixed;">
	<tr id="trName" runat="server">
		<td class="ibn-label" style="width:120px">
			<asp:Literal ID="Literal2" runat="server" Text="<%$Resources : GlobalFieldManageControls, tName %>" />:
		</td>
		<td>
			<asp:TextBox ID="txtEnumName" runat="server" Width="99%"></asp:TextBox>
		</td>
		<td style="width:16px"></td>
		<td style="width:20px">
			<asp:RequiredFieldValidator id="vldEnumName_Required" runat="server" ErrorMessage="*" ControlToValidate="txtEnumName" Display="Dynamic"></asp:RequiredFieldValidator>
			<asp:RegularExpressionValidator ID="vldEnumName_RegEx" ControlToValidate="txtEnumName" Runat="server" ValidationExpression="^[A-Za-z0-9][\w]*$"></asp:RegularExpressionValidator>
		</td>
	</tr>
	<tr id="trFriendlyName" runat="server">
		<td class="ibn-label" style="width:120px">
			<asp:Literal ID="Literal3" runat="server" Text="<%$Resources : GlobalFieldManageControls, FriendlyName %>" />:
		</td>
		<td>
			<asp:TextBox ID="txtFriendlyName" runat="server" Width="99%"></asp:TextBox>
		</td>
		<td align="left" style="width:16px">
			<img src='<%=CommerceHelper.GetAbsolutePath("/Apps/MetaDataBase/Images/resource.gif")%>' title='<%=GetGlobalResourceObject("GlobalMetaInfo", "ResourceTooltip").ToString()%>' alt='' style="width:16px; height:16px" />
		</td>
		<td style="width:20px">
			<asp:RequiredFieldValidator id="vldFriendlyName_Required" runat="server" ErrorMessage="*" ControlToValidate="txtFriendlyName" Display="Dynamic"></asp:RequiredFieldValidator>
		</td>
	</tr>
	<tr>
		<td colspan="4" style="border: 1px solid #aeaeae;">
			<asp:DataGrid runat="server" ID="grdMain" AutoGenerateColumns="false" Width="100%" 
				CellPadding="4" GridLines="None" AllowPaging="false" AllowSorting="false" 
				OnCancelCommand="grdMain_CancelCommand" OnDeleteCommand="grdMain_DeleteCommand" 
				OnEditCommand="grdMain_EditCommand" OnUpdateCommand="grdMain_UpdateCommand"
				OnItemCommand="grdMain_ItemCommand">
				<Columns>
					<asp:TemplateColumn HeaderText="#" >
						<ItemStyle CssClass="ibn-vb2" Width="50px" />
						<HeaderStyle CssClass="ibn-vh2" Width="50px" />
						<ItemTemplate>
							<%# Eval("OrderId")%>
						</ItemTemplate>
						<EditItemTemplate>
							<asp:DropDownList runat="server" ID="ddlOrder" Width="50px"></asp:DropDownList>
						</EditItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn>
						<ItemStyle CssClass="ibn-vb2" />
						<HeaderStyle CssClass="ibn-vh2" />
						<HeaderTemplate>
							<asp:Literal ID="headText" runat="server" Text='<%$Resources : GlobalMetaInfo, ItemValue%>'></asp:Literal>
							<asp:ImageButton ID="btnAsc" runat="server" ToolTip="<%$Resources : GlobalMetaInfo, AscSort%>" CommandName="Asc" ImageAlign="AbsMiddle" CausesValidation="false" ImageUrl="~/Apps/MetaDataBase/Images/Sort-Ascending.png" />&nbsp;&nbsp;
							<asp:ImageButton ID="btnDesc" runat="server" ToolTip="<%$Resources : GlobalMetaInfo, DescSort%>" CommandName="Desc" ImageAlign="AbsMiddle" CausesValidation="false" ImageUrl="~/Apps/MetaDataBase/Images/Sort-Descending.png" />
						</HeaderTemplate>
						<ItemTemplate>
							<%# Eval("DisplayName")%>
						</ItemTemplate>
						<EditItemTemplate>
							<asp:TextBox ID="txtName" Text='<%# DataBinder.Eval(Container.DataItem,"Name") %>' CssClass="text" Width="90%" Runat="server" MaxLength="255"></asp:TextBox>
							<img runat="server" id="imResourceTemplate" style="width:16px; height:16px" alt="" />
						</EditItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="<%$Resources : GlobalFieldManageControls, Default%>">
						<ItemStyle CssClass="ibn-vb2" Width="50px" HorizontalAlign="Center" />
						<HeaderStyle CssClass="ibn-vh2" />
						<ItemTemplate>
							<img runat="server" id="imIsDefault" src="~/Apps/MetaDataBase/Images/accept_1.gif" visible='<%# (bool)Eval("IsDefault") %>' style="width:16px; height:16px" alt="" />
						</ItemTemplate>
						<EditItemTemplate>
							<asp:Label ID="lblDefault" runat="server" Visible="false" Text='<%# ((bool)Eval("IsDefault")).ToString() %>'></asp:Label>
							<asp:CheckBox ID="cbDefault" runat="server" CssClass="text" />							
						</EditItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn>
						<ItemStyle CssClass="ibn-vb2" Width="50px"/>
						<HeaderStyle CssClass="ibn-vh-right" Width="50px" />
						<HeaderTemplate>
							<asp:ImageButton id="ibAdd" runat="server" ImageUrl="~/Apps/MetaDataBase/Images/newitem.gif" ToolTip="<%$Resources : GlobalMetaInfo, NewItem%>" Width="16" Height="16" CommandName="NewItem" causesvalidation="False" CommandArgument='<%# Eval("Id") %>'></asp:ImageButton>
							&nbsp;
						</HeaderTemplate>
						<ItemTemplate>
							<asp:ImageButton id="ibEdit" runat="server" ImageUrl="~/Apps/MetaDataBase/Images/edit.gif" ToolTip="<%$Resources : GlobalMetaInfo, Edit%>" Width="16" Height="16" CommandName="Edit" causesvalidation="False" CommandArgument='<%# Eval("Id") %>'></asp:ImageButton>
							&nbsp;
							<asp:ImageButton ID="ibDelete" Runat="server" ImageUrl="~/Apps/MetaDataBase/Images/delete.gif" ToolTip="<%$Resources : GlobalMetaInfo, Delete%>" Width="16" Height="16" CommandName="Delete" CausesValidation="false" CommandArgument='<%# Eval("Id") %>'></asp:ImageButton> 
						</ItemTemplate>
						<EditItemTemplate>
							<asp:ImageButton id="ibSave" runat="server" ToolTip="<%$Resources : GlobalMetaInfo, Save%>" ImageUrl="~/Apps/MetaDataBase/Images/SaveItem.gif" commandname="Update" causesvalidation="False" CommandArgument='<%# Eval("Id") %>'></asp:ImageButton>
							&nbsp;
							<asp:imagebutton id="ibCancel" runat="server" ToolTip="<%$Resources : GlobalMetaInfo, Cancel%>" ImageUrl="~/Apps/MetaDataBase/Images/cancel.gif" commandname="Cancel" causesvalidation="False">
							</asp:imagebutton>
						</EditItemTemplate>
					</asp:TemplateColumn>
				</Columns>
			</asp:DataGrid>
		</td>
	</tr>
	<tr>
		<td></td>
		<td>
			<asp:CheckBox runat="server" ID="chkEditable" Text="<%$Resources : GlobalFieldManageControls, EditableDictionary%>" Checked="false" />
		</td>
		<td></td>
		<td></td>
	</tr>
	<tr>
		<td></td>
		<td>
			<asp:CheckBox runat="server" ID="chkPublic" Text="<%$Resources : GlobalFieldManageControls, PublicDictionary%>" Checked="false" />
		</td>
		<td></td>
		<td></td>
	</tr>
</table>
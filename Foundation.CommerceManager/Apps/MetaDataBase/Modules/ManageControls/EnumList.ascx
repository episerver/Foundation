<%@ Control Language="C#" AutoEventWireup="true" Inherits="Mediachase.Ibn.Web.UI.Apps.MetaDataBase.Modules.ManageControls.EnumList" Codebehind="EnumList.ascx.cs" %>
<%@ Register TagPrefix="mc" TagName="BlockHeader" Src="~/Apps/MetaDataBase/Common/Design/BlockHeader.ascx" %>
<table cellspacing="0" cellpadding="0" border="0" width="100%" class="ibn-stylebox2">
	<tr>
    <td>
      <mc:BlockHeader id="secHeader" runat="server" />
    </td>
  </tr>
	<tr>
		<td>
			<asp:UpdatePanel runat="server" ID="upMain">
				<ContentTemplate>
					<asp:GridView runat="server" ID="grdMain" AutoGenerateColumns="false" Width="100%" 
						CellPadding="4" GridLines="None" AllowPaging="false" AllowSorting="true" OnRowCommand="grdMain_RowCommand" OnRowDeleting="grdMain_RowDeleting" OnSorting="grdMain_Sorting">
						<Columns>
							<asp:TemplateField HeaderText="<%$Resources : GlobalMetaInfo, Name%>" SortExpression="Name" >
								<ItemTemplate>
									<a href='EnumView.aspx?type=<%# Eval("Name")%>'><%# Eval("Name")%></a>
								</ItemTemplate>
								<ItemStyle CssClass="ibn-vb" />
								<HeaderStyle CssClass="ibn-vh" />
							</asp:TemplateField>
							<asp:BoundField HeaderText="<%$Resources : GlobalMetaInfo, FriendlyName%>" ItemStyle-CssClass="ibn-vb" HeaderStyle-CssClass="ibn-vh" DataField="FriendlyName" ItemStyle-Width="30%" HeaderStyle-Width="30%" SortExpression="FriendlyName" />
							<asp:BoundField HeaderText="<%$Resources : GlobalMetaInfo, Type%>" ItemStyle-CssClass="ibn-vb" HeaderStyle-CssClass="ibn-vh" DataField="Type" ItemStyle-Width="30%" HeaderStyle-Width="30%" SortExpression="Type" />
							<asp:TemplateField>
								<ItemTemplate>
									<asp:ImageButton ImageUrl="../../Images/edit.gif" runat="server" ID="ibEdit" ToolTip="<%$Resources : GlobalMetaInfo, Edit%>" Width="16" Height="16" CommandName="Edit" CommandArgument='<%# Eval("Name") %>' />&nbsp;&nbsp;
									<asp:ImageButton ImageUrl="../../Images/delete.gif" Runat="server" ToolTip="<%$Resources : GlobalMetaInfo, Delete%>" Width="16" Height="16" CommandName="Delete" CommandArgument='<%# Eval("Name") %>' ID="ibDelete" Visible='<%# !(bool)Eval("IsUsed") %>' ></asp:ImageButton> 
								</ItemTemplate>
								<ItemStyle CssClass="ibn-vb" Width="50px" Wrap="false" />
								<HeaderStyle CssClass="ibn-vh" Width="50px" Wrap="false"/>
							</asp:TemplateField>
						</Columns>
					</asp:GridView>
				</ContentTemplate>
			</asp:UpdatePanel>
		</td>
	</tr>
</table>
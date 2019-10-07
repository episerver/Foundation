<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ListProfiles.ascx.cs" Inherits="Mediachase.Ibn.Web.UI.MetaUIEntity.Modules.ListProfiles" %>
<br />
<table width="100%" class="ibn-bottomBorderLight">
	<tr>
		<td class="section"></td>
		<td align="right"><asp:HyperLink runat="server" ID="lnkNew"></asp:HyperLink></td>
	</tr>
</table>
<asp:DataGrid runat="server" ID="grdMain" AutoGenerateColumns="false" Width="100%"
	CellPadding="4" AllowPaging="false" AllowSorting="false" GridLines="None"
	OnItemCommand="grdMain_RowCommand">
	<Columns>
		<asp:TemplateColumn HeaderText="<%$Resources : GlobalMetaInfo, Name%>">
			<ItemTemplate>
				<%# Eval("Name")%>
			</ItemTemplate>
			<ItemStyle CssClass="ibn-vb2" />
			<HeaderStyle CssClass="ibn-vh2"  />
		</asp:TemplateColumn>
		<asp:TemplateColumn>
			<ItemTemplate>
				<asp:HyperLink id="ibEdit" runat="server" ImageUrl="~/Apps/MetaDataBase/images/edit.gif" NavigateUrl='<%# Eval("EditLink")%>' ToolTip="<%$Resources : GlobalMetaInfo, Edit%>"></asp:HyperLink>&nbsp;&nbsp;
				<asp:ImageButton ImageUrl="~/Apps/MetaDataBase/images/delete.gif" Runat="server" ToolTip="<%$Resources : GlobalMetaInfo, Delete%>" Width="16" Height="16" CommandName='<%# ConstDeleteCommand %>' CommandArgument='<%# Eval("Id") %>' ID="ibDelete" Visible='<%# Eval("CanDelete")%>'></asp:ImageButton>
				<asp:ImageButton ImageUrl="~/Apps/MetaDataBase/images/Undo.png" Runat="server" ToolTip="<%$Resources : GlobalMetaInfo, ResetToDefault%>" Width="16" Height="16" CommandName='<%# ConstResetCommand %>' CommandArgument='<%# Eval("Id") %>' ID="ibReset" Visible='<%# Eval("CanReset")%>'></asp:ImageButton>
			</ItemTemplate>
			<ItemStyle CssClass="ibn-vb2" Width="50px" />
			<HeaderStyle CssClass="ibn-vh2" Width="50px" />
		</asp:TemplateColumn>
	</Columns>
</asp:DataGrid>
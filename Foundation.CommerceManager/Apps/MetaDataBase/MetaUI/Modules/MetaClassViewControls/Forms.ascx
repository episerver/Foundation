<%@ Control Language="C#" AutoEventWireup="true" Inherits="Mediachase.Ibn.Web.UI.MetaUI.Modules.MetaClassViewControls.Forms" Codebehind="Forms.ascx.cs" %>
<br />
<table width="100%">
	<tr>
		<td class="section"></td>
		<td align="right"><asp:HyperLink runat="server" ID="lnkNew"></asp:HyperLink></td>
	</tr>
</table>
<asp:DataGrid runat="server" ID="grdMain" AutoGenerateColumns="false" Width="100%"
	CellPadding="4" AllowPaging="false" AllowSorting="false" GridLines="None"
	OnItemCommand="grdMain_RowCommand" OnItemDataBound="grdMain_RowDataBound">
	<Columns>
		<asp:BoundColumn DataField="PublicFormName" Visible="false" />
		<asp:TemplateColumn HeaderText="<%$Resources : GlobalMetaInfo, Name%>">
			<ItemTemplate>
				<%# Eval("Name")%>
				<br /><asp:TextBox ID="txtLink" TextMode="MultiLine" Rows="5" Columns="80" runat="server" CssClass="text"></asp:TextBox>
			</ItemTemplate>
			<ItemStyle CssClass="ibn-vb2" />
			<HeaderStyle CssClass="ibn-vh2"  />
		</asp:TemplateColumn>
		<asp:TemplateColumn>
			<ItemTemplate>
				<asp:HyperLink id="ibEdit" runat="server" ImageUrl="~/Apps/MetaDataBase/Images/edit.gif" NavigateUrl='<%# Eval("EditLink")%>' ToolTip="<%$Resources : GlobalMetaInfo, Edit%>"></asp:HyperLink>&nbsp;&nbsp;
				<asp:ImageButton ID="ibDelete" Runat="server" CommandName='<%# deleteCommand %>' CommandArgument='<%# Eval("Id") %>' ImageUrl="~/Apps/MetaDataBase/Images/delete.gif" ToolTip="<%$Resources : GlobalMetaInfo, Delete%>" Width="16" Height="16" Visible='<%# Eval("CanDelete")%>'></asp:ImageButton> 
				<asp:ImageButton ImageUrl="~/Apps/MetaDataBase/images/Undo.png" Runat="server" ToolTip="<%$Resources : GlobalMetaInfo, ResetToDefault%>" Width="16" Height="16" CommandName='<%# resetCommand %>' CommandArgument='<%# Eval("Id") %>' ID="ibReset" Visible='<%# Eval("CanReset")%>'></asp:ImageButton> 
			</ItemTemplate>
			<ItemStyle CssClass="ibn-vb2" Width="50px" />
			<HeaderStyle CssClass="ibn-vh2" Width="50px" />
		</asp:TemplateColumn>
	</Columns>
</asp:DataGrid>
<asp:Button runat="server" ID="btnRefresh" OnClick="btnRefresh_Click" Text="Refresh" style="display:none" />
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MetaClassRelationsNN.ascx.cs" Inherits="Mediachase.Ibn.Web.UI.MetaDataBase.Modules.MetaClassViewControls.MetaClassRelationsNN" %>
<table width="100%" class="ibn-bottomBorderLight">
	<tr>
		<td class="section"></td>
		<td align="right"><asp:HyperLink runat="server" ID="NewLink"></asp:HyperLink></td>
	</tr>
</table>
<asp:GridView runat="server" ID="MainGrid" AutoGenerateColumns="false" Width="100%" 
	CellPadding="4" GridLines="None" AllowPaging="false" AllowSorting="true" 
	OnRowCommand="MainGrid_RowCommand" OnSorting="MainGrid_Sorting">
	<Columns>
		<asp:TemplateField HeaderText="">
			<ItemTemplate>
				<asp:Image runat="server" ID="FieldTypeImage" ImageUrl='<%#Eval("FieldTypeImageUrl") %>' Width="16px" Height="16px" />
			</ItemTemplate>
			<ItemStyle CssClass="ibn-vb2" Width="20px" HorizontalAlign="Center" />
			<HeaderStyle CssClass="ibn-vh2" Width="20px" />
		</asp:TemplateField>
		<asp:TemplateField HeaderText="<%$Resources : GlobalMetaInfo, RelationName%>" SortExpression="Name" >
			<ItemTemplate>
				<%# Eval("Name")%>
			</ItemTemplate>
			<ItemStyle CssClass="ibn-vb2"  />
			<HeaderStyle CssClass="ibn-vh2" />
		</asp:TemplateField>
		<asp:TemplateField HeaderText="<%$Resources : GlobalMetaInfo, Name%>" SortExpression="DisplayName" >
			<ItemTemplate>
				<%# Eval("DisplayName")%>
			</ItemTemplate>
			<ItemStyle CssClass="ibn-vb2"  />
			<HeaderStyle CssClass="ibn-vh2" />
		</asp:TemplateField>
		<asp:TemplateField HeaderText="<%$Resources : GlobalMetaInfo, CurrentObject%>" SortExpression="CurrentName">
			<ItemTemplate>
				<%# Eval("CurrentName")%>
			</ItemTemplate>
			<ItemStyle CssClass="ibn-vb2" />
			<HeaderStyle CssClass="ibn-vh2" />
		</asp:TemplateField>
		<asp:TemplateField HeaderText="<%$Resources : GlobalMetaInfo, RelatedObject%>" SortExpression="RelatedName">
			<ItemTemplate>
				<%# Eval("RelatedName")%>
			</ItemTemplate>
			<ItemStyle CssClass="ibn-vb2" />
			<HeaderStyle CssClass="ibn-vh2" />
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
				<asp:HyperLink id="EditButton" runat="server" ImageUrl="~/Apps/MetaDataBase/images/edit.gif" NavigateUrl='<%# Eval("EditLink")%>' ToolTip="<%$Resources : GlobalMetaInfo, Edit%>"></asp:HyperLink>&nbsp;&nbsp;
				<asp:ImageButton ID="DeleteButton" Runat="server" ImageUrl="~/Apps/MetaDataBase/images/delete.gif" ToolTip="<%$Resources : GlobalMetaInfo, Delete%>" Width="16" Height="16" CommandName='<%# deleteCommand %>' CommandArgument='<%# Eval("Name") %>' Visible='<%# !((bool)Eval("IsSystem")) %>'></asp:ImageButton> 
			</ItemTemplate>
			<ItemStyle CssClass="ibn-vb2" Width="50px" />
			<HeaderStyle CssClass="ibn-vh2" Width="50px" />
		</asp:TemplateField>
	</Columns>
</asp:GridView>
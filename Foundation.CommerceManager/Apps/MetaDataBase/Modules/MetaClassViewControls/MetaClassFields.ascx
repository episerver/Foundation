<%@ Control Language="C#" AutoEventWireup="true" Inherits="Mediachase.Ibn.Web.UI.MetaDataBase.Modules.MetaClassViewControls.MetaClassFields" Codebehind="MetaClassFields.ascx.cs" %>
<asp:GridView runat="server" ID="grdMain" AutoGenerateColumns="false" Width="100%" 
	CellPadding="4" GridLines="None" AllowPaging="false" AllowSorting="true" OnRowCommand="grdMain_RowCommand" OnSorting="grdMain_Sorting">
	<Columns>
		<asp:TemplateField HeaderText="">
			<ItemTemplate>
				<asp:Image runat="server" ID="imFieldType" ImageUrl='<%#Eval("FieldTypeImageUrl") %>' Width="16px" Height="16px" />
			</ItemTemplate>
			<ItemStyle CssClass="ibn-vb2" Width="20px" HorizontalAlign="Center" />
			<HeaderStyle CssClass="ibn-vh2" Width="20px" />
		</asp:TemplateField>
		<asp:TemplateField HeaderText="<%$Resources : GlobalMetaInfo, SystemName%>" SortExpression="Name" >
			<ItemTemplate>
				<%# Eval("Name")%>
			</ItemTemplate>
			<ItemStyle CssClass="ibn-vb2" Width="35%" />
			<HeaderStyle CssClass="ibn-vh2" Width="35%" />
		</asp:TemplateField>
		<asp:BoundField HeaderText="<%$Resources : GlobalMetaInfo, FriendlyName%>" DataField="FriendlyName" ItemStyle-CssClass="ibn-vb2" HeaderStyle-CssClass="ibn-vh2" HeaderStyle-Width="35%" ItemStyle-Width="35%" SortExpression="FriendlyName"/>
		<asp:TemplateField HeaderText="<%$Resources : GlobalMetaInfo, Type%>" SortExpression="TypeName" >
			<ItemTemplate>
				<%# Eval("TypeName")%>
			</ItemTemplate>
			<ItemStyle CssClass="ibn-vb2" />
			<HeaderStyle CssClass="ibn-vh2" />
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
				<asp:HyperLink id="ibEdit" runat="server" ImageUrl="../../Images/edit.gif" NavigateUrl='<%# Eval("EditLink")%>' ToolTip="<%$Resources : GlobalMetaInfo, Edit%>" Visible='<%# !((bool)Eval("IsSystem")) %>'></asp:HyperLink>&nbsp;&nbsp;
				<asp:ImageButton ImageUrl="../../Images/delete.gif" Runat="server" ToolTip="<%$Resources : GlobalMetaInfo, Delete%>" Width="16" Height="16" CommandName='<%# deleteCommand %>' CommandArgument='<%# Eval("Name") %>' ID="ibDelete" Visible='<%# !((bool)Eval("IsSystem")) %>'></asp:ImageButton> 
			</ItemTemplate>
			<ItemStyle CssClass="ibn-vb2" Width="50px" />
			<HeaderStyle CssClass="ibn-vh2" Width="50px" />
		</asp:TemplateField>
	</Columns>
</asp:GridView>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MetaFormSelector.ascx.cs" Inherits="Mediachase.Ibn.Web.UI.MetaDataBase.Modules.ManageControls.MetaFormSelector" %>
<div style="border: 1px solid #aeaeae;">
<asp:DataGrid runat="server" ID="grdMain" AutoGenerateColumns="false" Width="100%"  ShowFooter="false"
	CellPadding="1" GridLines="None" AllowPaging="false" AllowSorting="false" ShowHeader="false">
	<Columns>
		<asp:BoundColumn DataField="ClassName" Visible="false"></asp:BoundColumn>
		<asp:BoundColumn DataField="IsForm" Visible="false"></asp:BoundColumn>
		<asp:TemplateColumn>
			<ItemStyle CssClass="ibn-propertysheet" />
			<ItemTemplate>
				<asp:CheckBox ID="cbIsAdd" Checked="true" runat="server" Text='<%# Eval("DisplayName") %>' />
			</ItemTemplate>
		</asp:TemplateColumn>
	</Columns>
</asp:DataGrid>
</div>
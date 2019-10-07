<%@ Control Language="C#" AutoEventWireup="true" Inherits="Mediachase.Ibn.Web.UI.Apps.MetaUI.Modules.SelectControls.SelectItem" Codebehind="SelectItem.ascx.cs" %>
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
						<td style="width:100px;" class="ibn-label">
							<asp:Literal ID="Literal1" runat="server" Text="Table Name"></asp:Literal>:
						</td>
						<td class="ibn-value">
							<asp:Label runat="server" ID="lblClassName"></asp:Label>
						</td>
						<td class="ibn-value" align="right" >
							<asp:TextBox runat="server" ID="tbSearchString" Width="150px"></asp:TextBox>
							<asp:Button runat="server" ID="btnSearch" Text="Search" />
						</td>
					</tr>
				</table>
			</td></tr></table>
		</td>
	</tr>
	<tr>
		<td>
			<asp:DataGrid runat="server" ID="grdMain" AutoGenerateColumns="false" Width="100%" 
				CellPadding="4" GridLines="None" AllowPaging="true" AllowSorting="true" PageSize="25" >
				<PagerStyle Mode="NumericPages" Position="Bottom" CssClass="ibn-pager"/>
				<Columns>
					<asp:boundcolumn visible="false" datafield="Id"></asp:boundcolumn>
					<asp:TemplateColumn HeaderText="Item Name" SortExpression="Name" >
						<ItemStyle CssClass="ibn-vb" />
						<HeaderStyle CssClass="ibn-vh" />
						<ItemTemplate>
							<%# Eval("Name")%>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn>
						<ItemStyle CssClass="ibn-vb" Width="25px"/>
						<HeaderStyle CssClass="ibn-vh" Width="25px" />
						<ItemTemplate>
							<asp:ImageButton id="ibRelate" runat="server" imageurl="../../../Images/relate.gif" ToolTip="Relate" Width="16" Height="16"></asp:ImageButton>
						</ItemTemplate>
					</asp:TemplateColumn>
				</Columns>
			</asp:DataGrid>
		</td>
  </tr>
</table>
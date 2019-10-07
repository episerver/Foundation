<%@ Control Language="C#" AutoEventWireup="true" Inherits="Mediachase.Ibn.Web.UI.Apps.MetaDataBase.Modules.ManageControls.IdentifierList" Codebehind="IdentifierList.ascx.cs" %>
<%@ Register TagPrefix="ibn" TagName="BlockHeader" Src="~/Apps/MetaDataBase/Common/Design/BlockHeader.ascx" %>
<table cellspacing="0" cellpadding="0" border="0" width="100%" class="ibn-stylebox2">
	<tr>
    <td>
      <ibn:BlockHeader id="secHeader" runat="server" />
    </td>
  </tr>
	<tr>
		<td>
			<asp:UpdatePanel runat="server" ID="upMain">
				<ContentTemplate>
					<asp:GridView runat="server" ID="grdMain" AutoGenerateColumns="false" Width="100%" 
						CellPadding="4" GridLines="None" AllowPaging="false" AllowSorting="true" OnRowCommand="grdMain_RowCommand" OnRowDeleting="grdMain_RowDeleting" OnSorting="grdMain_Sorting">
						<Columns>
							<asp:TemplateField HeaderText="<%$Resources : GlobalMetaInfo, Name%>" SortExpression="FriendlyName">
								<ItemTemplate>
									<%#Eval("FriendlyName")%>
								</ItemTemplate>
								<ItemStyle CssClass="ibn-vb"  />
								<HeaderStyle  CssClass="ibn-vh" />
							</asp:TemplateField>
							<asp:TemplateField HeaderText="<%$Resources : GlobalMetaInfo, IdentifierMask%>" SortExpression="Mask">
								<ItemTemplate>
									<%#Eval("Mask")%>
								</ItemTemplate>
								<ItemStyle CssClass="ibn-vb" />
								<HeaderStyle CssClass="ibn-vh" />
							</asp:TemplateField>
							<asp:TemplateField HeaderText="<%$Resources : GlobalMetaInfo, CounterLength%>" SortExpression="CounterLength">
								<ItemTemplate>
									<%#Eval("CounterLength")%>
								</ItemTemplate>
								<ItemStyle CssClass="ibn-vb" />
								<HeaderStyle CssClass="ibn-vh" />
							</asp:TemplateField>
							<asp:TemplateField HeaderText="<%$Resources : GlobalMetaInfo, CounterReset%>" SortExpression="CounterReset">
								<ItemTemplate>
									<%#Eval("CounterReset")%>
								</ItemTemplate>
								<ItemStyle CssClass="ibn-vb" />
								<HeaderStyle CssClass="ibn-vh" />
							</asp:TemplateField>
							<asp:TemplateField HeaderText="<%$Resources : GlobalMetaInfo, IdentifierScope%>" SortExpression="Scope">
								<ItemTemplate>
									<%#Eval("Scope")%>
								</ItemTemplate>
								<ItemStyle CssClass="ibn-vb" />
								<HeaderStyle CssClass="ibn-vh" />
							</asp:TemplateField>
							<asp:TemplateField>
								<ItemTemplate>
									<asp:ImageButton ImageUrl="../../Images/edit.gif" Runat="server" ToolTip="<%$Resources : GlobalMetaInfo, Edit%>" Width=16 Height=16 CommandName="Edit" CommandArgument='<%# Eval("Name") %>' ID="ibEdit" ></asp:ImageButton>&nbsp;&nbsp;
									<asp:ImageButton ImageUrl="../../Images/delete.gif" Runat="server" ToolTip="<%$Resources : GlobalMetaInfo, Delete%>" Width=16 Height=16 CommandName="Delete" CommandArgument='<%# Eval("Name") %>' ID="ibDelete" Visible='<%# !(bool)Eval("IsUsed") %>'></asp:ImageButton> 
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
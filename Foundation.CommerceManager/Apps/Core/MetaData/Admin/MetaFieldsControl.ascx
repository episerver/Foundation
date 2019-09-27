<%@ Control Language="c#" Inherits="Mediachase.Commerce.Manager.Core.MetaData.Admin.MetaFieldsControl" Codebehind="MetaFieldsControl.ascx.cs" %>
<table width="600" id="tblAttributes" runat="server">
	<tr>
		<td colSpan="2">
			<asp:DataGrid id="ItemsGrid" runat="server" CssClass="ca-Grid" AllowPaging="True" Width="100%" PageSize="20" ShowFooter="True" DataKeyField="MetaFieldId" AutoGenerateColumns="False">
				<ItemStyle CssClass="ca-GridItem"></ItemStyle>
				<HeaderStyle CssClass="ca-GridHeader"></HeaderStyle>
				<FooterStyle CssClass="ca-GridFooter"></FooterStyle>
				<Columns>
					<asp:TemplateColumn HeaderText="<%$ Resources:SharedStrings, Name %>" HeaderStyle-Width="90%">
						<ItemTemplate>
							<asp:HyperLink runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FriendlyName")%>' NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.MetaFieldId", "~/AttributeEdit.aspx?id={0}") %>' ID="Hyperlink2">
							</asp:HyperLink>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="<%$ Resources:SharedStrings, Options %>" HeaderStyle-Width=10%>
						<ItemTemplate>
							<asp:imagebutton id="ibEdit" runat="server" borderwidth="0" alternatetext='<%# RM.GetString("GENERAL_EDIT")%>' imageurl="../images/edit.gif" commandname="Edit" causesvalidation="False" />
						</ItemTemplate>					
					</asp:TemplateColumn>
				</Columns>
				<PagerStyle CssClass=text Mode="NumericPages"></PagerStyle>
			</asp:DataGrid>
		</td>
	</tr>
</table>
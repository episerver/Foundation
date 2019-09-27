<%@ Control Language="C#" AutoEventWireup="true" Inherits="Mediachase.Commerce.Manager.Apps.Shell.Modules.LeftTemplate" Codebehind="LeftTemplate.ascx.cs" %>
<asp:Repeater ID="TabItems" runat="server">
    <ItemTemplate>
		<div class="NavigationNavBarItem" onclick="leftTemplate_onNavMenuSelect(this, <%# Container.ItemIndex %>)" onmouseover="leftTemplate_onMouseOver(this);" onmouseout="leftTemplate_onMouseOut(this);">
			<table cellpadding="0" cellspacing="1" width="100%">
				<td style="width:36px;padding:0px;"><asp:Image ID="Image2" ImageAlign="AbsMiddle" Width="28px" Height="28px" runat="server" BorderWidth="0" ImageUrl='<%#Eval("ImageUrl")%>' /></td>
				<td valign="middle"><%#Eval("Title") %></td>
			</table>
		</div>
	</ItemTemplate>
</asp:Repeater>
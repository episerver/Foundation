<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PageTemplateTab.ascx.cs" Inherits="Mediachase.Commerce.Manager.Core.Layout.Modules.Tabs.PageTemplateTab" %>
<div style="display: block;width:100%" runat="server" id="divControlSet">
    <div style="margin: 0px; height: 240px; width:100%; overflow-y: none; overflow-x: scroll;">
		<asp:Repeater runat="server" ID="repTemplates">														
			<ItemTemplate>
				<div class="customizeWSTemplateItem" runat="server" id="mainItemDiv" 
				onmouseover="if (this.className != 'customizeWSTemplateItemSelected') this.className='customizeWSTemplateItemHover';" 
				onmouseout="if (this.className != 'customizeWSTemplateItemSelected') this.className='customizeWSTemplateItem';">
					<table cellpadding="0" cellspacing="0" border="0" class="text" style="width: 95%;">
						<tr>
							<td align="left" style="width: 150px;"><asp:Image runat="server" ID="imgTemplate" AlternateText="<%$ Resources:CoreStrings, Template_Image %>" ImageUrl='<%# Eval("ImageUrl") %>' /></td>
							<td><%# Eval("Description") %></td>											
						</tr>
					</table>									
					<asp:Button runat="server" ID="btnCommand" CommandName="Click" CommandArgument='<%# Eval("Id") %>' Visible="false" />
				</div>
			</ItemTemplate>
		</asp:Repeater>
	</div>
</div>
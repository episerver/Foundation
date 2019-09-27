<%@ Control Language="c#" Inherits="Mediachase.Commerce.Manager.Core.ErrorModule" Codebehind="ErrorModule.ascx.cs" %>
<asp:UpdatePanel runat="server" ID="ErrorsPanel" ChildrenAsTriggers="false" UpdateMode="Conditional" RenderMode="Block">
<ContentTemplate>
    <asp:Repeater Runat="server" ID="ErrorMessages">
	    <HeaderTemplate>
    <table cellpadding="2" cellspacing="2" width="100%">
	    <tr>
		    <td>	
	    </HeaderTemplate>
	    <FooterTemplate>
		    </td>
	    </tr>
    </table>	
	    </FooterTemplate>
	    <SeparatorTemplate>
		    <table cellpadding="2" width="100%"><tr><td colspan="2"></td></tr></table>
	    </SeparatorTemplate>
	    <ItemTemplate>
			    <table cellpadding="2" width="100%" class="ErrorBox" cellspacing="2">
				    <tr>
					    <td width="10"><asp:Image Runat="server" ImageUrl="~/Apps/Shell/styles/images/Shell/Caution.gif" ID="Image1"></asp:Image></td>
					    <td width="100%" align="left"><asp:Label EnableViewState="false" runat="server" ForeColor="red" id="Label1"><%# DataBinder.Eval(Container, "DataItem")%></asp:Label></td>
				    </tr>
			    </table>	
	    </ItemTemplate>
    </asp:Repeater>
</ContentTemplate>
</asp:UpdatePanel>
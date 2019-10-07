<%@ Control Language="C#" AutoEventWireup="true" Inherits="Mediachase.Ibn.Web.UI.ListSelectControl" Codebehind="ListSelectControl.ascx.cs" %>
<table cellspacing="0" cellpadding="0" border="0" class="ibn-propertysheet">
	<tr>
		<td>
			<div id="txtName" runat="server" style="height:71px;border:1px solid #888888;overflow:auto;overflow-y:auto;"></div>
		</td>
		<td valign="bottom"><div id="divSearch" runat="server" style="border:1px solid #888888;border-left:0px;padding:2px;height:18px;vertical-align:middle;"><asp:Label ID="lblSearch" runat="server" Text="Search"></asp:Label></div></td>
	</tr>
</table>
<asp:HiddenField ID="hfValue" runat="server" />
<asp:HiddenField ID="hfClear" runat="server" />
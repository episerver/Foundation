<%@ Control Language="C#" AutoEventWireup="true" Inherits="Mediachase.Ibn.Web.UI.SelectControl" Codebehind="SelectControl.ascx.cs" %>
<script type="text/javascript">
	function CancelBubble_SelectPopups(e)
	{
		e = (e) ? e : ((event) ? event : null);
		if (e)
		{
			e.cancelBubble = true;
			if(e.stopPropagation)
				e.stopPropagation();
		}
	}
</script>
<table cellspacing="0" cellpadding="0" border="0" class="ibn-propertysheet">
	<tr>
		<td>
			<div id="txtName" runat="server" style="height:20px;border:1px solid #888888;vertical-align:middle;"></div>
		</td>
		<td><div id="divSearch" runat="server" style="border:1px solid #888888;border-left:0px;width:18px;height:18px;padding:1px;"><asp:Label ID="lblSearch" runat="server" Text="Search"></asp:Label></div></td>
	</tr>
</table>
<asp:HiddenField ID="hfValue" runat="server" />
<asp:HiddenField ID="hfClear" runat="server" />
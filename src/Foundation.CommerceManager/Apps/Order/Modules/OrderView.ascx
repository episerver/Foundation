<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrderView.ascx.cs" Inherits="Mediachase.Commerce.Manager.Apps.Order.Modules.OrderView" %>
<%@ Import Namespace="Mediachase.Commerce.Shared" %>
<%@ Register TagPrefix="ibn" Namespace="Mediachase.BusinessFoundation" Assembly="Mediachase.BusinessFoundation" %>
<script type="text/javascript" src="<%= CommerceHelper.GetAbsolutePath("~/Apps/Shell/EPi/Shell/Light/jquery.min.js") %>"></script>
<script type="text/javascript">

	var blockColl;
	var tabColl;
	//$(document).ready(MainJQueryClientTabs);

	function MainJQueryClientTabs() {
		var headColl = $("div.blockHead");
		blockColl = $("table.blockContent");

		var n = 0;
		var hidField = $get('<%=hidTabValue.ClientID %>');
		if (hidField && hidField.value)
			n = parseInt(hidField.value);

		if (headColl && blockColl && headColl.length == blockColl.length && blockColl.length > 0) {
			var tabs = "<ul class=\"tabs\">";
			for (var i = 0; i < blockColl.length; i++) {
				tabs += "<li class=\"tabGap\"></li>";
				tabs += "<li class=\"tabUnselected\" onclick=\"toggleTab(" + i + ")\">" + headColl[i].innerHTML + "</li>";
			}
			tabs += "<li class=\"tabGap\"></li>";
			tabs += "</ul><div class=\"tabsHr\"></div>";
			$("table.blockContent:first").before(tabs);

			tabColl = $("li.tabUnselected");

			tabColl[n].className = "tabSelected";
			blockColl[n].className = "blockSelected";
		}
		window.setTimeout(function() { toggleTab(n); }, 1000);
	}

	function toggleTab(n) {

		var hidField = $get('<%=hidTabValue.ClientID %>');
		hidField.value = n;
		
		for (var i = 0; i < blockColl.length; i++) {
			if (i == n) {
				tabColl[i].className = "tabSelected";
				blockColl[i].className = "blockSelected";
			}
			else {
				tabColl[i].className = "tabUnselected";
				blockColl[i].className = "blockContent";
			}
		}

		var gridsColl = $(".serverGridBody .WrapperDiv");
		if (gridsColl && gridsColl.length > 0) {
			for (var i = 0; i < gridsColl.length; i++) {
				if (gridsColl[i].offsetHeight > 0) {
					var gridI = $find(gridsColl[i].id);

					gridI.autoSize();
					gridI.onWindowResize(null);
					gridI.recalculateHeader();
					gridI.onWindowResize(null);
				}
			}
		}
	}
</script>

<style type="text/css">
	.tabsHr
	{
		padding:0px;
		margin: 0px;
		height: 1px;
		border-top: 1px solid #95b7f3;
	}
	.blockHead
	{
		display:none;
	}
	.blockContent
	{
		display:none;
	}
	.blockSelected
	{
		display:;
	}
	ul.tabs li.tabUnselected
	{
		background-color: #e1ecfc;
		color: #003399;
		font-weight:normal;
	}
	ul.tabs li.tabSelected
	{
		color: #ce3431;
		font-weight:bold;
		border-bottom: #ffffff 1px solid;
		background-color: #ffffff;
	}
	ul.tabs
	{
		list-style-type:none;
		clear: both;
		overflow: hidden;
		padding:0px;
		margin:0px;
		position: relative;
		top:1px;
	}
	ul.tabs li
	{
		float:left;
		height:18px;
		overflow:hidden;
		width:120px;
		text-align:center;
		border: #95b7f3 1px solid;
		cursor:default;
		font-family:Verdana;
		font-size:12px;
		padding-top:2px;
	}
	ul.tabs li.tabGap
	{
		float:left;
		height:18px;
		overflow:hidden;
		width:5px;
		border-top: #ffffff 1px solid;
		border-left: none;
		border-right: none;
		border-bottom: #95b7f3 1px solid;
		padding-top:2px;
	}
</style>
<asp:UpdatePanel runat="server" ID="ContentPanel">
	<ContentTemplate>
		<table cellpadding="0" cellspacing="0" border="0" width="100%" >
			<tr>
				<td valign="top" style="padding:5px;">
					<div id="mainDiv" style="overflow:auto;background-color:White;" class="ibn-stylebox2 ibn-propertysheet borderBoxXmlFormBulder">
						<ibn:XMLFormBuilder ID="xmlStruct" runat="server" />
					</div>
				</td>
			</tr>
		</table>
	</ContentTemplate>
</asp:UpdatePanel>
<asp:HiddenField ID="hidTabValue" runat="server" />
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ControlSetTab.ascx.cs" Inherits="Mediachase.Commerce.Manager.Core.Layout.Modules.Tabs.ControlSetTab" %>
<!-- control set tab  /*class="ibn-WPBorder"*/ -->
<div style="display: block;width:100%" runat="server" id="divControlSet">
    <div style="margin: 0px; height: 240px; width:100%; overflow-y: none; overflow-x: scroll;">
        <table cellpadding="0" cellspacing="0" border="0" width="100%" class="text">
		    <tr>
			    <td class="text">
				    <asp:DataGrid runat="server" ID="grdMain" PageSize="50" AutoGenerateColumns="false" CssClass="text" CellPadding="5" CellSpacing="0" ShowHeader="false" Width="100%" GridLines="none" BorderWidth="0">
					    <Columns>
						    <asp:BoundColumn DataField="Id" Visible="false" />					
						    <asp:TemplateColumn>										
							    <ItemTemplate>
								    <asp:CheckBox runat="server" ID="cbId" Checked="false" />
							    </ItemTemplate>
						    </asp:TemplateColumn>
						    <asp:TemplateColumn HeaderText="<%$ Resources:SharedStrings, Title %>">
							    <ItemTemplate>
								    <%# Eval("Title") %>
							    </ItemTemplate>
						    </asp:TemplateColumn>
						    <asp:TemplateColumn HeaderText="<%$ Resources:SharedStrings, Description %>">
							    <ItemTemplate>
								    <%# Eval("Description") %>
							    </ItemTemplate>
						    </asp:TemplateColumn>
					    </Columns>
				    </asp:DataGrid>
			    </td>
		    </tr>
	    </table>
	</div>
</div>
<%--
<div runat="server" id="divTemplateUpdate" style="position: absolute; display: none; width: 90%; left: 5%; height: 50px; top: 35%; margin: 17px 7px 7px 7px;" class="infoBlock">
	<blockquote class="infoBlockInner">
		<asp:Literal ID="Literal1" runat="server" Text="<%$Resources : IbnFramework.Global, _mc_TemplateUpdate%>" />
		<div class="infoText">
			<a href="#" onclick="document.getElementById('<%= divTemplateUpdate.ClientID %>').style.display = 'none';"> <%= Mediachase.Ibn.Web.UI.CHelper.GetResFileString("{IbnFramework.Global:_mc_Close}")%></a>
		</div>
	</blockquote>
</div>

<script type="text/javascript">
function hidedivTemplateUpdate()
{
	var obj_divTemplateUpdate = document.getElementById('<%= divTemplateUpdate.ClientID %>');
	if (obj_divTemplateUpdate)
	{
		obj_divTemplateUpdate.style.display = 'none';
	}
}

$addHandler(document.body, 'click', hidedivTemplateUpdate);
</script>--%>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EntryMarketEditTab.ascx.cs" Inherits="Mediachase.Commerce.Manager.Apps.Catalog.Tabs.EntryMarketEditTab" %>
<%@ Register Assembly="Mediachase.BusinessFoundation" Namespace="Mediachase.BusinessFoundation" TagPrefix="mc" %>

<div id="DataForm">
	<table class="DataForm">
		<tr>
			<td class="FormLabelCell">
				<asp:Literal ID="litEntryMarkets" runat="server"></asp:Literal>
			</td>
			<td class="FormFieldCell">
				<mc:ListToListSelector runat="server" ID="ltlSelector" OneItemToTargetButtonId="btnMoveSelectedToTarget" OneItemToSourceButtonId="btnMoveSelectedToSource" SourceListId="lbSource" TargetListId="lbTarget" SelectAllSourceItemsButtonId="btnSelectSourceAll" SelectAllTargetItemsButtonId="btnSelectTargetAll">
				</mc:ListToListSelector>
				<table border="0">
					<tr>
						<td style="padding:10px;">
							<asp:Literal runat="server" Text="<%$ Resources:CatalogStrings, Entry_Market_Available %>"></asp:Literal>&nbsp;&nbsp;
                            <asp:Button ID="btnSelectSourceAll" runat="server" Text="<%$ Resources:SharedStrings, Select_All %>" OnClientClick="return false;" /><br />
							<div class="scrollable">
								<asp:ListBox runat="server" ID="lbSource" SelectionMode="Multiple" Height="205" />
								<br />
							</div>							
						</td>
						<td  style="vertical-align:middle; padding:10px; text-align:center;">
							<asp:Button runat="server" ID="btnMoveSelectedToTarget" Text=">" OnClientClick="return false;" />
                            <br />
							<asp:Button runat="server" ID="btnMoveSelectedToSource" Text="<" OnClientClick="return false;" />
						</td>
						<td style="padding:10px">
							<asp:Literal runat="server" Text="<%$ Resources:CatalogStrings, Entry_Market_Unavailable %>"></asp:Literal>&nbsp;&nbsp;
                            <asp:Button ID="btnSelectTargetAll" runat="server" Text="<%$ Resources:SharedStrings, Select_All %>" OnClientClick="return false;" /><br />
							<div class="scrollable">
								<asp:ListBox runat="server" ID="lbTarget" SelectionMode="Multiple" Height="205px" />
								<br />
							</div>
						</td>
					</tr>
                    <tr>
                        <td colspan="3">
                            <asp:CheckBox runat="server" ID="cbShowDisabledMarkets" Text="<%$ Resources:CatalogStrings, List_Inactive_Markets %>" AutoPostBack="true" />
                            <asp:HiddenField runat="server" ID="hfHiddenItems" />
                        </td>
                    </tr>
				</table>
			</td>
			<td colspan="3" class="FormSpacerCell">
			</td>
		</tr>
		<tr>
			<td colspan="5" class="FormSpacerCell">
			</td>
		</tr>
	</table>
</div>
<script type="text/javascript">
	$(document).ready(function () {
		//	updateListBoxHeight();

		if (Mediachase && Mediachase.ListToListSelector) {
			Mediachase.ListToListSelector.prototype.AddOption = function (objTo, Option) {
				var oOption = document.createElement("OPTION");
				oOption.text = Option.text;
				oOption.value = Option.value;
				if (objTo != null)
					objTo.options[objTo.options.length] = oOption;
				//				updateListBoxHeight();
			};
		}
	});

	function updateListBoxHeight() {
		var selectArr = $('.scrollable select');
		for (var i = 0; i < selectArr.length; i++) {
			selectArr[i].style.height = selectArr[i].scrollHeight + 'px';
		}
	}
</script>
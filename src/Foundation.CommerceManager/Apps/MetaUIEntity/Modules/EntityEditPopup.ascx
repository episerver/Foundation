<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EntityEditPopup.ascx.cs" Inherits="Mediachase.Ibn.Web.UI.MetaUIEntity.Modules.EntityEditPopup" %>
<%@ Reference Control="~/Apps/MetaDataBase/MetaUI/MetaForm/FormDocumentView.ascx" %>
<%@ Register TagPrefix="ibn" TagName="formView" Src="~/Apps/MetaDataBase/MetaUI/MetaForm/FormDocumentView.ascx" %>
<%@ Register TagPrefix="mc2" Assembly="Mediachase.BusinessFoundation" Namespace="Mediachase.BusinessFoundation" %>
<script type="text/javascript">
function LayoutResizeHandler(sender, eventArgs)
{
    var layoutBaseNode = this._containerId.parentNode;
    if (layoutBaseNode) {
        layoutBaseNode.style.paddingTop = 0
    }
}
</script>
<style type="text/css">
.text { 	COLOR: windowtext; FONT-FAMILY: Verdana, Arial, Helvetica; font-size:8pt; }

</style>
<table class="ibn-propertysheet" cellspacing="0" cellpadding="0" width="100%" border="0">
	<tr>
		<td>
			<ibn:formView ID="EditForm" runat="server" />
		</td>
	</tr>
	<tr>
		<td style="padding:10px; text-align:right;">
			<table align="right">
				<tr>
					<td>
						<mc2:IMButton id="btnSave" runat="server" style="width:105px;" OnServerClick="btnSave_ServerClick"></mc2:IMButton>&nbsp;
						<mc2:IMButton id="btnCancel" runat="server" style="width:105px;" CausesValidation="false"></mc2:IMButton>
					</td>
				</tr>
				<tr id="trGoToView" runat="server">
					<td align="left" style="padding-top:10px">
						<asp:CheckBox ID="cbGoToView" runat="server" Text="Go to View Page" />
					</td>
				</tr>
			</table>
		</td>
	</tr> 
</table>
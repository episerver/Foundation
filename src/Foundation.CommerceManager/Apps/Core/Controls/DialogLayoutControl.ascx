<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DialogLayoutControl.ascx.cs" Inherits="Mediachase.Commerce.Manager.Apps.Core.Controls.DialogLayoutControl" %>
<%@ Register TagPrefix="ibn" Namespace="Mediachase.BusinessFoundation" Assembly="Mediachase.BusinessFoundation" %>
<script type="text/javascript">
    function RunValidation(arrVGroups) {
        var validated = true;
        if (arrVGroups != null && (arrVGroups instanceof Array) && (typeof(Page_ClientValidate) == 'function')) {
            for (var i = 0; i < arrVGroups.length; i++) {
                validated = Page_ClientValidate(arrVGroups[i]);
                if (!validated)
                    break;
            }

            //remove the flag to block the submit if it was raised
            Page_BlockSubmit = false;
        }
        //return the results
        return validated;
    }
</script>
<table cellpadding="0" cellspacing="0" border="0" width="100%" >
	<tr>
		<td valign="top" style="padding:5px;">
			<div id="mainDiv" style="overflow:auto;background-color:White;" class="ibn-propertysheet">
				<ibn:XMLFormBuilder ID="xmlStruct" runat="server" />
			</div>
		</td>
	</tr>
	<tr>
		<td align="center" style="padding:10px;">
			<ibn:IMButton ID="btnSave" runat="server" style="width: 105px;">
			</ibn:IMButton>
			&nbsp;
			<ibn:IMButton ID="btnCancel" runat="server" style="width: 105px;" CausesValidation="false">
			</ibn:IMButton>
		</td>
	</tr>
</table>
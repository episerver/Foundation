<%@ Control Language="C#" AutoEventWireup="true" Inherits="Mediachase.Commerce.Manager.Core.SaveControl" Codebehind="SaveControl.ascx.cs" %>
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
<div class="ButtonContainer">
<asp:Button runat="server" ID="SaveChangesButton" CssClass="button" OnClientClick="isSubmit = true;" Text="<%$ Resources:SharedStrings, OK %>" Width="72" />
<asp:Button ID="CancelButton" runat="server" CssClass="button" Text="<%$ Resources:SharedStrings, Cancel %>" CausesValidation="false" Width="72"/>&nbsp;
<asp:Button ID="DeleteButton" Visible="false" CssClass="button" runat="server" Text="<%$ Resources:SharedStrings, Delete %>" CausesValidation="false" Width="72"/>
</div>
<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Mediachase.Commerce.Manager.Core.Controls.EditViewControl" Codebehind="EditViewControl.ascx.cs" %>
<asp:ValidationSummary runat="server" ID="sumValidator" HeaderText="<%$ Resources:SharedStrings, Validation_Fix_The_Following_Problems %>" />
<ajaxToolkit:TabContainer ID="DefaultTabContainer" runat="server" CssClass="gray" OnClientActiveTabChanged="CloseDropdown">
</ajaxToolkit:TabContainer>

<script type="text/javascript">
    function CloseDropdown() {

        if (typeof AssetsEditTabClose == 'function') {
            AssetsEditTabClose();
        }
        if (typeof AssociationEditTabClose == 'function') {
            AssociationEditTabClose();
        }
    }
</script>
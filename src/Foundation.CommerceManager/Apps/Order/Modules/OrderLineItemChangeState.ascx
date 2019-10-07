<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrderLineItemChangeState.ascx.cs" Inherits="Mediachase.Commerce.Manager.Apps.Order.Modules.OrderLineItemChangeState" %>
<%@ Register TagPrefix="mc2" Assembly="Mediachase.BusinessFoundation" Namespace="Mediachase.BusinessFoundation" %>
<div style="padding: 5px;">
    <table style="width: 100%;" class="DataForm">
        <tr>
            <td class="FormLabelCell">
                <span>Status:</span>
            </td>
            <td><asp:DropDownList runat="server" ID="ddStates" /></td>
        </tr>
    </table>
    <div align="center" style="margin-top: 25px; float: right; margin-right: 20px;">
	    <mc2:IMButton ID="btnSave" runat="server" style="width: 105px;" OnServerClick="btnSave_ServerClick">
	    </mc2:IMButton>
	    &nbsp;
	    <mc2:IMButton ID="btnCancel" runat="server" style="width: 105px;" CausesValidation="false">
	    </mc2:IMButton>
    </div>
</div>
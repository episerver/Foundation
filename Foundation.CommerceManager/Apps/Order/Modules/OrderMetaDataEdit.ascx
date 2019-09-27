<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrderMetaDataEdit.ascx.cs" Inherits="Mediachase.Commerce.Manager.Apps.Order.Modules.OrderMetaDataEdit" %>
<%@ Register TagPrefix="mc2" Assembly="Mediachase.BusinessFoundation" Namespace="Mediachase.BusinessFoundation" %>
<%@ Register Src="~/Apps/Core/MetaData/EditTab.ascx" TagName="MetaData" TagPrefix="ecf" %>
<div id="DataForm">
    <table class="DataForm"> 
        <ecf:MetaData ValidationGroup="OrderCreateVG" runat="server" ID="MetaDataTab" />
        <tr>
            <td>
                <mc2:IMButton ID="btnSave" runat="server" style="width: 105px;" OnServerClick="btnSave_ServerClick"></mc2:IMButton>
            </td>
            <td>
                <mc2:IMButton ID="btnCancel" runat="server" style="width: 105px;" CausesValidation="false"></mc2:IMButton>
            </td>
        </tr>
    </table>
</div>

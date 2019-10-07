<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CommonSettings.ascx.cs" Inherits="Mediachase.Commerce.Manager.Core.CommonSettings" %>
<%@ Register Src="~/Apps/Core/SaveControl.ascx" TagName="SaveControl" TagPrefix="ecf" %>
<%@ Register Src="~/Apps/Core/Controls/EditViewControl.ascx" TagName="EditViewControl" TagPrefix="ecf" %>
<div class="editDiv">
<ecf:EditViewControl AppId="Core" ViewId="CommonSettings" id="ViewControl" runat="server"></ecf:EditViewControl>
<ecf:SaveControl id="EditSaveControl" CancelClientScript="CSManagementClient.ChangeView('Core', 'CommonSettings', '');" SavedClientScript="CSManagementClient.ChangeView('Core', 'CommonSettings', '');" runat="server"></ecf:SaveControl>
</div>
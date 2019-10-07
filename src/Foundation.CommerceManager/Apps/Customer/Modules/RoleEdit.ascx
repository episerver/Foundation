<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RoleEdit.ascx.cs" Inherits="Mediachase.Commerce.Manager.Apps.Customer.Modules.RoleEdit" %>
<%@ Register Src="~/Apps/Core/SaveControl.ascx" TagName="SaveControl" TagPrefix="ecf" %>
<%@ Register Src="~/Apps/Core/Controls/EditViewControl.ascx" TagName="EditViewControl" TagPrefix="ecf" %>
<div class="editDiv">
<ecf:EditViewControl AppId="Customer" ViewId="Role-Edit" id="ViewControl" runat="server"></ecf:EditViewControl>
<ecf:SaveControl id="EditSaveControl" ShowDeleteButton="false" CancelClientScript="CSManagementClient.ChangeBafView('Customer','Roles-List');" SavedClientScript="CSManagementClient.ChangeView('Customer','Roles-List');" runat="server"></ecf:SaveControl>
</div>
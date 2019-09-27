<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EntityEdit.ascx.cs"
	Inherits="Mediachase.Ibn.Web.UI.MetaUIEntity.Modules.EntityEdit" %>
<%@ Register Src="~/Apps/Core/Controls/EditViewControl.ascx" TagName="EditViewControl" TagPrefix="ecf" %>
<%@ Register Src="~/Apps/Core/SaveControl.ascx" TagName="SaveControl" TagPrefix="ecf" %>

<div class="editDiv">
<ecf:EditViewControl id="ViewControl" runat="server"></ecf:EditViewControl>
<ecf:SaveControl id="EditSaveControl" ShowDeleteButton="false" runat="server"></ecf:SaveControl>
</div>
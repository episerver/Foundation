<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SearchManager.ascx.cs"
    Inherits="Mediachase.Commerce.Manager.Apps.Core.Search.SearchManager" %>
<%@ Register Src="~/Apps/Core/Controls/EditViewControl.ascx" TagName="EditViewControl"
    TagPrefix="ecf" %>
<div class="editDiv">
<ecf:EditViewControl AppId="Core" ViewId="Search" id="ViewControl" runat="server"></ecf:EditViewControl>
</div>

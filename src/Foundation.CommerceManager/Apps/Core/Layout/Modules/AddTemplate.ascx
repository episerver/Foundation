<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddTemplate.ascx.cs" Inherits="Mediachase.Ibn.Web.UI.Layout.Modules.AddTemplate" %>
<div class="IbnWidgetHeader ibn-portlet-addButton" onmouseover="this.className = this.className.replace(' ibn-portlet-addButton', '');"
         onmouseout="this.className += ' ibn-portlet-addButton';">
    <div id="clickDiv" runat="server" style="height: 20px; padding: 4px 0px 4px 0px; cursor: pointer;">
	    <img src='<%= this.ResolveUrl("~/Apps/Core/images/Cross-Green.png") %>' border='0'/><span class="text"><asp:Literal runat="server" Text="<%$ Resources:SharedStrings, Add %>"/></span>
    </div>
</div>
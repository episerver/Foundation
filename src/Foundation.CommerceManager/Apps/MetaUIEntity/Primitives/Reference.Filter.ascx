<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Reference.Filter.ascx.cs" Inherits="Mediachase.UI.Web.Apps.MetaUIEntity.Primitives.Reference_Filter" %>
<div runat="server" id="container" style="display: inline;">
	<asp:Label runat="server" ID="lblValue" CssClass="dropLabel dropLabelText" />
</div>
<asp:LinkButton runat="server" ID="lbAddItems" Visible="false" />
<asp:CustomValidator runat="server" ID="customNotNull" SetFocusOnError="true" />

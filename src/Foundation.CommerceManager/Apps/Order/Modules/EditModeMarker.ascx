<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditModeMarker.ascx.cs" Inherits="Mediachase.Commerce.Manager.Apps.Order.Modules.EditModeMarker" %>
<%@ Register src="../../Core/Controls/ButtonsHolder.ascx" tagname="ButtonsHolder" tagprefix="uc1" %>
<div style="padding:10px; border-bottom:solid 1px #95B7F3;vertical-align:middle;background-color:#D6E8FF;color:Red;font-weight:bold;" id="mainDiv" runat="server">
	<asp:Literal ID="ltrAttentionText" runat="server"></asp:Literal>
	
	<uc1:ButtonsHolder ID="btnHolder" runat="server" />
</div>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Decimal.Filter.@.@.Between.ascx.cs" Inherits="Mediachase.UI.Web.Apps.MetaUIEntity.Primitives.Decimal_Filter__Between" %>
<div runat="server" id="container" style="display: inline;" changeVisibility="1">
	<asp:TextBox runat="server" ID="tbText1" CssClass="dropLabelText" Width="50px" />
	<asp:TextBox runat="server" ID="tbText2" CssClass="dropLabelText" Width="50px" />
</div><div runat="server" id="textContainer" >
	<asp:Label runat="server" ID="lblText" CssClass="dropLabel dropLabelText" />
	<asp:Label runat="server" ID="lblAnd" CssClass="dropLabel dropLabelGreen" />
	<asp:Label runat="server" ID="lblText2" CssClass="dropLabel" />
</div>
<asp:Label runat="server" ID="lblError" CssClass="ErrorRed" />
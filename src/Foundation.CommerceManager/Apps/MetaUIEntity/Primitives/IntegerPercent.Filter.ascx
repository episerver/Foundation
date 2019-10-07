<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IntegerPercent.Filter.ascx.cs" Inherits="Mediachase.UI.Web.Apps.MetaUIEntity.Primitives.IntegerPercent_Filter" %>
<div runat="server" id="container" style="display: inline;" changeVisibility="1">
	<asp:TextBox runat="server" ID="tbText1" CssClass="dropLabelText" Width="50px" />
</div><div runat="server" id="textContainer" >
	<asp:Label runat="server" ID="lblText" CssClass="dropLabel dropLabelText" />
</div>
<asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="tbText1" ErrorMessage="*" ValidationExpression="[0-9]+" runat="server" />
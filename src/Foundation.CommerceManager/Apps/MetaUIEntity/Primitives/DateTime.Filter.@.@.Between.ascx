<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DateTime.Filter.@.@.Between.ascx.cs" Inherits="Mediachase.UI.Web.Apps.MetaUIEntity.Primitives.DateTime_Filter__Between" %>
<%@ Register Src="~/Apps/MetaDataBase/Common/DateTimePickerAjax/PickerControl.ascx" TagName="Picker" TagPrefix="mc" %>
<div runat="server" id="container" style="display: inline;" class="innerInline" changevisibility="1">
	<mc:Picker runat="server" ID="ctrlPicker" DateCssClass="text" TimeCssClass="text" DateWidth="85px" ShowImageButton="false" ShowTime="true" DateIsRequired="true" AutoPostBack="true"/>
	<mc:Picker runat="server" ID="ctrlPicker2" DateCssClass="text" TimeCssClass="text" DateWidth="85px" ShowImageButton="false" ShowTime="true" DateIsRequired="true" AutoPostBack="true"/>
</div><div runat="server" id="textContainer">
	<asp:Label runat="server" ID="lblText" CssClass="dropLabel" />
	<asp:Label runat="server" ID="lblAnd" CssClass="dropLabel dropLabelGreen" />
	<asp:Label runat="server" ID="lblText2" CssClass="dropLabel" />
</div>
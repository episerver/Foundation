<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Date.Filter.@.@.@.ascx.cs" Inherits="Mediachase.UI.Web.Apps.MetaUIEntity.Primitives.Date_Filter___" %>
<%@ Register Src="~/Apps/MetaDataBase/Common/DateTimePickerAjax/PickerControl.ascx" TagName="Picker" TagPrefix="mc" %>
<div runat="server" id="container" style="display: inline;" class="innerInline">
	<mc:Picker runat="server" ID="ctrlPicker" DateCssClass="text" TimeCssClass="text" DateWidth="85px" ShowImageButton="false" ShowTime="false" DateIsRequired="true" AutoPostBack="true"/>
</div><div runat="server" id="textContainer"><asp:Label runat="server" ID="lblText" CssClass="dropLabel" /></div>
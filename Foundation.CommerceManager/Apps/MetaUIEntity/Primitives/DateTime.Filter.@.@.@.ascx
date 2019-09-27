<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DateTime.Filter.@.@.@.ascx.cs" Inherits="Mediachase.UI.Web.Apps.MetaUIEntity.Primitives.DateTime_Filter___" %>
<%@ Register Src="~/Apps/MetaDataBase/Common/DateTimePickerAjax/PickerControl.ascx" TagName="Picker" TagPrefix="mc" %>
<div runat="server" id="container" class="innerInline" changeVisibility="1">
	<mc:Picker runat="server" ID="ctrlPicker" DateCssClass="dropLabelText" TimeCssClass="dropLabelText" DateWidth="85px" ShowImageButton="false" ShowTime="true" DateIsRequired="true" AutoPostBack="true" />
</div><div runat="server" id="textContainer"><asp:Label runat="server" ID="lblText" CssClass="dropLabel" /></div>
<asp:Button runat="server" ID="imgOk" Visible="false" />
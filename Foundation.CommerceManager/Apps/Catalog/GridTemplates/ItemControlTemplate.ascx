<%@ Control Language="C#" AutoEventWireup="true" Inherits="Apps_Catalog_GridTemplates_ItemControlTemplate" Codebehind="ItemControlTemplate.ascx.cs" %>
<%@ Register Src="~/Apps/Core/Controls/BooleanEditControl.ascx" TagName="BooleanEditControl" TagPrefix="ecf" %>
<%@ Register TagPrefix="ibn" TagName="DatePickerControl" src="~/Apps/MetaDataBase/Common/DateTimePickerAjax/PickerControl.ascx" %>
<%@ Import Namespace="Mediachase.Web.Console.Controls" %>
<asp:Label runat="server" ID="lblItem" Visible="false" />
<asp:TextBox runat="server" ID="tbItem" Width="400" Visible="false"></asp:TextBox>
<asp:DropDownList runat="server" ID="ddlItem" Width="200" Visible="false"/>
<ecf:BooleanEditControl runat="server" ID="becItem" Visible="false" />
<ibn:DatePickerControl id="cdpItem" runat="server" Visible="false" ShowTime="true" />
<asp:RequiredFieldValidator runat="server" ID="rfValidator" Display="Dynamic" ErrorMessage="*" Enabled="false" ControlToValidate="tbItem" />
<table width="100%">
<col style="display:none" />
<asp:PlaceHolder Runat="server" ID="MetaControls" EnableViewState="true"></asp:PlaceHolder>
</table>
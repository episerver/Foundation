<%@ Page Language="C#" AutoEventWireup="true" Inherits="Mediachase.Ibn.Web.UI.MetaDataBase.Pages.Admin.EnumView" MasterPageFile="~/Apps/MetaDataBase/MasterPages/DialogMasterPage.master" Codebehind="EnumView.aspx.cs" %>
<%@ Register TagPrefix="ibn" TagName="EnumViewControl" Src="~/Apps/MetaDataBase/Modules/ManageControls/EnumViewControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phMain" Runat="Server">
	<ibn:EnumViewControl runat="server" ID="ucEnumViewControl" />
</asp:Content>

<%@ Page Language="C#" MasterPageFile="~/Apps/MetaDataBase/MasterPages/DialogMasterPage.master" AutoEventWireup="true" Inherits="Mediachase.Ibn.Web.UI.MetaDataBase.Pages.Public.EnumView" Title="Untitled Page" Codebehind="EnumView.aspx.cs" %>
<%@ Reference Control="~/Apps/MetaDataBase/Modules/ManageControls/EnumViewControl.ascx" %>
<%@ Register TagPrefix="ibn" TagName="EnumViewControl" Src="~/Apps/MetaDataBase/Modules/ManageControls/EnumViewControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phMain" Runat="Server">
<ibn:EnumViewControl runat="server" ID="ucEnumViewControl" />
</asp:Content>


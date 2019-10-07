<%@ Page Language="C#" AutoEventWireup="true" Inherits="Mediachase.Ibn.Web.UI.MetaDataBase.Pages.Admin.MetaBridgeEdit" MasterPageFile="~/Apps/MetaDataBase/MasterPages/ExtGrid.master" Codebehind="MetaBridgeEdit.aspx.cs" %>
<%@ Register TagPrefix="mc" TagName="bridgeEdit" Src="~/Apps/MetaDataBase/Modules/ManageControls/MetaBridgeEdit.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
	<mc:bridgeEdit ID="ucBridgeEdit" runat="server" />
</asp:Content>
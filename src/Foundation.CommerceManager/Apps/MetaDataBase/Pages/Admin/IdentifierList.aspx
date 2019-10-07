<%@ Page Language="C#" AutoEventWireup="true" Inherits="Mediachase.Ibn.Web.UI.MetaDataBase.Pages.Admin.IdentifierList" MasterPageFile="~/Apps/MetaDataBase/MasterPages/ExtGrid.master" Codebehind="IdentifierList.aspx.cs" %>
<%@ Register TagPrefix="mc" TagName="identList" Src="~/Apps/MetaDataBase/Modules/ManageControls/IdentifierList.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
	<mc:identList ID="ucIdentList" runat="server" />
</asp:Content>
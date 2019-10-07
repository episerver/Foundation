<%@ Page Language="C#" AutoEventWireup="true" Inherits="Mediachase.Ibn.Web.UI.MetaDataBase.Pages.Admin.IdentifierEdit" MasterPageFile="~/Apps/MetaDataBase/MasterPages/ExtGrid.master" Codebehind="IdentifierEdit.aspx.cs" %>
<%@ Register TagPrefix="mc" TagName="identEdit" Src="~/Apps/MetaDataBase/Modules/ManageControls/IdentifierEdit.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
	<mc:identEdit ID="ucIdentEdit" runat="server" />
</asp:Content>
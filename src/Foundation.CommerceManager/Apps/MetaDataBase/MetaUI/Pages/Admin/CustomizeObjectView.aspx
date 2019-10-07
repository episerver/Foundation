<%@ Page Language="C#" AutoEventWireup="true" Inherits="Mediachase.Ibn.Web.UI.MetaUI.Pages.Admin.CustomizeObjectView" MasterPageFile="~/Apps/MetaDataBase/MasterPages/ExtGrid.master" Codebehind="CustomizeObjectView.aspx.cs" %>
<%@ Reference Control="~/Apps/MetaDataBase/MetaUI/MetaForm/FormDocumentDesigner.ascx" %>
<%@ Register TagPrefix="mc" TagName="Designer" src="~/Apps/MetaDataBase/MetaUI/MetaForm/FormDocumentDesigner.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
	<mc:Designer id="designer1" runat="server" />
</asp:Content>
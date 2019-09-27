<%@ Page Language="C#" AutoEventWireup="true" Inherits="Mediachase.Ibn.Web.UI.MetaUI.Pages.Admin.CustomizeObjectView2" MasterPageFile="~/Apps/MetaDataBase/MasterPages/DialogMasterPage.master" Theme="" Codebehind="CustomizeObjectView2.aspx.cs" %>
<%@ Reference Control="~/Apps/MetaDataBase/MetaUI/MetaForm/FormDocumentDesigner.ascx" %>
<%@ Register TagPrefix="mc" TagName="Designer" src="~/Apps/MetaDataBase/MetaUI/MetaForm/FormDocumentDesigner.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phMain" Runat="Server">
	<mc:Designer id="designer1" runat="server"></mc:Designer>
</asp:Content>
<%@ Page Language="C#" AutoEventWireup="true" Inherits="Mediachase.Ibn.Web.UI.MetaDataBase.Pages.Admin.MetaClassEdit" MasterPageFile="~/Apps/MetaDataBase/MasterPages/ExtGrid.master" Codebehind="MetaClassEdit.aspx.cs" %>
<%@ Register TagPrefix="mc" TagName="classEdit" Src="~/Apps/MetaDataBase/Modules/ManageControls/MetaClassEdit.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
	<mc:classEdit ID="ucClassEdit" runat="server" />
</asp:Content>
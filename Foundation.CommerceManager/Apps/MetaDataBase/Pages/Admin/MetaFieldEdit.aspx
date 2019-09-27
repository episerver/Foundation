<%@ Page Language="C#" AutoEventWireup="true" Inherits="Mediachase.Ibn.Web.UI.MetaDataBase.Pages.Admin.MetaFieldEdit" MasterPageFile="~/Apps/MetaDataBase/MasterPages/ExtGrid.master" Codebehind="MetaFieldEdit.aspx.cs" %>
<%@ Register TagPrefix="mc" TagName="fieldEdit" Src="~/Apps/MetaDataBase/Modules/ManageControls/MetaFieldEdit.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
	<mc:fieldEdit id="ucFieldEdit" runat="server" />
</asp:Content>


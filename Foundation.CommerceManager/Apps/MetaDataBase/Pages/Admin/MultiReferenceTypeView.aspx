<%@ Page Language="C#" AutoEventWireup="true" Inherits="Mediachase.Ibn.Web.UI.MetaDataBase.Pages.Admin.MultiReferenceTypeView" MasterPageFile="~/Apps/MetaDataBase/MasterPages/ExtGrid.master" Codebehind="MultiReferenceTypeView.aspx.cs" %>
<%@ Register TagPrefix="mc" TagName="typeView" Src="~/Apps/MetaDataBase/Modules/ManageControls/MultiReferenceTypeView.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
	<mc:typeView id="ucTypeView" runat="server" />
</asp:Content>
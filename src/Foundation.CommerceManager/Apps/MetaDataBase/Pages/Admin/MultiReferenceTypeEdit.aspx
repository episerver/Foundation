<%@ Page Language="C#" AutoEventWireup="true" Inherits="Mediachase.Ibn.Web.UI.MetaDataBase.Pages.Admin.MultiReferenceTypeEdit" MasterPageFile="~/Apps/MetaDataBase/MasterPages/ExtGrid.master" Codebehind="MultiReferenceTypeEdit.aspx.cs" %>
<%@ Register TagPrefix="mc" TagName="typeEdit" Src="~/Apps/MetaDataBase/Modules/ManageControls/MultiReferenceTypeEdit.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
	<mc:typeEdit id="ucTypeEdit" runat="server" />
</asp:Content>